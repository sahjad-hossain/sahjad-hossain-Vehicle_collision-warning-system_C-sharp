using System;
using System.ComponentModel;
using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Tracking.Models;

namespace DataFusion
{
    // To do: Replace all occurrences of 'CASpace' by the space on which this detection model should operate.
    // Typically, this space is identical to the measurement space used by the measurement model.
    [DisplayName(@"My detection model")]
    [Description("Describes the detection capabilities of my sensor.")]
    class CameraDetectionModel : SingleDetectionModel<CVComponentsSpace>
    {
        public CameraDetectionModel()
        {
            double DetectionRangeMax;
            double DetectionRangeMin;
            double DetectionAngleMin;
            double DetectionAngleMax;
        }

        // This property can be configured, e.g. in the user interface of a dml file.
        [DisplayName(@"Maximum detection range")]
        [Description("The sensor can detect objects up to this value.")]
        [Category("Sensor characteristics")]
        [DefaultValue(80.0)]
        [Unit("m")]
        public double DetectionRangeMax { get; set; }

        [DisplayName(@"Minimum detection range")]
        [Description("The sensor can detect objects up to this value.")]
        [Category("Sensor characteristics")]
        [DefaultValue(7.0)]
        [Unit("m")]
        public double DetectionRangeMin { get; set; }

        [DisplayName(@"Maximum detection Angle")]
        [Description("The sensor can detect objects up to this value.")]
        [Category("Sensor characteristics")]
        [DefaultValue(0.26)]
        [Unit("rad")]
        public double DetectionAngleMax { get; set; }

        [DisplayName(@"Minimum detection Angle")]
        [Description("The sensor can detect objects up to this value.")]
        [Category("Sensor characteristics")]
        [DefaultValue(-0.26)]
        [Unit("rad")]
        public double DetectionAngleMin { get; set; }


        protected override double GetDetectionProbability(CVComponentsSpace sample)
        {
            if (sample.X > DetectionRangeMax)
            {
                // The object is outside the field of view and thus, the sensor cannot detect it.
                return 0;
            }
            else
            {
                // Calculates the detection probability for one detection inside the field of view
                // ref class 'RadarDetectionModel'
                if ((Math.Sqrt(sample.Y * sample.Y + sample.X * sample.X) >= DetectionRangeMin) && (Math.Sqrt(sample.Y * sample.Y + sample.X * sample.X) <= DetectionRangeMax))
                {
                    if ((Math.Atan2(sample.Y, sample.X) >= DetectionAngleMin) && (Math.Atan2(sample.Y, sample.X)) <= DetectionAngleMax)
                    {

                        double detectionProbability = 0.9;
                        return detectionProbability;
                    }
                    else
                    {
                        // The object is outside the field of view and thus, the sensor cannot detect it.
                        return 0;
                    }

                }
                else
                {

                    return 0;
                }
                // To do: Determine and return the detection probability inside the field of view:
                // double detectionProbability = ...
                // return detectionProbability;

                //To do: Remove the following line when the model has been implemented.
                //throw new NotImplementedException();
            }
        }
    }
}
