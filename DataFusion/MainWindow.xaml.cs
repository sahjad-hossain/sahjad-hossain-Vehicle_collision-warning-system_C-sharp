using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Tracking.Tracks;
using Color = System.Drawing.Color;

namespace DataFusion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _imageDirectory;
        private long[] _imageTimes;
        private bool _stop = true;

        public MainWindow()
        {
            _imageDirectory = Path.Combine(Environment.CurrentDirectory, "..\\..\\Images");
            InitializeComponent();
            this.DataContext = this;
            var imageFiles = Directory.GetFiles(_imageDirectory);
            _imageTimes = imageFiles.Select(_ => Convert.ToInt64(Path.GetFileNameWithoutExtension(_))).ToArray();
            SpeedSlider.Value = SpeedSlider.Maximum;
        }

        public delegate void UpdateImageCallback(long ticks);

        private void UpdateImage(long ticks)
        {
            var imagePath = Path.Combine(_imageDirectory, ticks + ".jpg");
            var image = new BitmapImage(new Uri(imagePath));
            ImagePanel.Source = image;
        }

        public delegate void UpdateTtcCallback(double ttc);

        private void UpdateTtc(double ttc)
        {
            TtcBox.Text = ttc.ToString(CultureInfo.InvariantCulture) + " s";
            if (ttc < 20)
            {
                TtcBox.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
            }
            else
            {
                TtcBox.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
            }
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            _stop = false;
            var sleepPeriod = (int)((SpeedSlider.Maximum - SpeedSlider.Value) / 10);
            new System.Threading.Tasks.Task(() => Run(sleepPeriod)).Start();
        }

        private BitmapImage _image;

        public BitmapImage ImageSource
        {
            get { return _image; }
            set
            {
                _image = value;
            }
        }

        private void Run(int sleepPeriod)
        {
            var streamReader = new StreamReader("..\\..\\measurements.txt");
            var dataFusion = new DataFusion1();
            var egoMotionProcessed = false;
            ImagePanel.Dispatcher.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { _imageTimes[0] });
            long nextImageTime = _imageTimes[1];
            var imageIndex = 2;
            double velocity = 0;
            while (!streamReader.EndOfStream && !_stop)
            {
                var line = streamReader.ReadLine();
                if (line == null)
                {
                    continue;
                }

                var elements = line.Split(';');
                if (elements.Length < 2)
                {
                    throw new InvalidOperationException("The data string has an incorrect format.");
                }
                var ticks = Convert.ToInt64(elements[1]);
                var timestamp = new DateTime(ticks).ToUniversalTime();
                if (nextImageTime < ticks && imageIndex < _imageTimes.Length)
                {
                    ImagePanel.Dispatcher.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { nextImageTime });

                    nextImageTime = _imageTimes[imageIndex++];
                }

                int index;
                switch (elements[0])
                {
                    case "Ego":
                        if (elements.Length != 4)
                        {
                            throw new InvalidOperationException("The ego motion string has an incorrect format.");
                        }
                        velocity = Convert.ToDouble(elements[2], CultureInfo.InvariantCulture);
                        var yawRate = Convert.ToDouble(elements[3], CultureInfo.InvariantCulture);
                        dataFusion.ProcessVelocity(timestamp, new VelocitySpace { V = velocity });
                        dataFusion.ProcessYawRate(timestamp, new YawRateSpace { W = yawRate });
                        egoMotionProcessed = true;
                        break;
                    case "Radar":
                        if (!egoMotionProcessed)
                        {
                            continue;
                        }
                        index = 2;
                        var radarMeasurements = new List<RadarSpace>();
                        while (index < elements.Length)
                        {
                            var range = Convert.ToDouble(elements[index++], CultureInfo.InvariantCulture);
                            var azimuth = Convert.ToDouble(elements[index++], CultureInfo.InvariantCulture);
                            var rangeRate = Convert.ToDouble(elements[index++], CultureInfo.InvariantCulture);
                            radarMeasurements.Add(new RadarSpace
                            {
                                Range = range,
                                Phi = azimuth,
                                RadialVelocity = rangeRate
                            });
                        }
                        dataFusion.ProcessMeasurements0(timestamp, radarMeasurements);
                        break;
                    case "Camera":
                        if (!egoMotionProcessed)
                        {
                            continue;
                        }
                        index = 2;
                        var cameraMeasurements = new List<CameraSpace>();
                        while (index < elements.Length)
                        {
                            var row = Convert.ToInt32(elements[index++]);
                            var column = Convert.ToInt32(elements[index++]);
                            cameraMeasurements.Add(new CameraSpace
                            {
                                Row = row,
                                Column = column
                            });
                        }
                        dataFusion.ProcessMeasurements1(timestamp, cameraMeasurements);
                        IEnumerable<GaussianTrack<CVComponentsSpace>> tracks;
                        if (dataFusion.GetTracks(timestamp, out tracks))
                        {
                            var ttc = TimeToCollision.GetTimeToCollision(tracks, velocity);
                            TtcBox.Dispatcher.Invoke(new UpdateTtcCallback(this.UpdateTtc), new object[] { ttc });
                        }

                        break;
                }

                Thread.Sleep(sleepPeriod);
            }
        }

        public event EventHandler<ImageEventArgs> UpdateImageEvent;

        protected virtual void OnUpdateImage(ImageEventArgs eventArgs)
        {
            EventHandler<ImageEventArgs> handler = UpdateImageEvent;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            _stop = true;
        }
    }

    public class ImageEventArgs : EventArgs
    {
        public long Time { get; set; }
    }
}
