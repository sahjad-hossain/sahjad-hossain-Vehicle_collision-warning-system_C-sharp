
using System;
using System.ComponentModel;
using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Models.AdditiveNoise;

 

namespace DataFusion
{
    [DisplayName(@"My camera measurement model")]
    [Description("Describes the measurement capabilities of my sensor.")]

    class CameraMeasurementModel : MeasurementModel<CameraSpace, CVComponentsSpace>
    {
        private readonly double[] _calibrationMatrix =
        {
            -224.11252745164600,
            935.04128990035599,
            11.549136555426900,
            226.49587100989399,
            -119.96299327410701,
            4.4416026412363901e-011,
            942.46405013179594,
            -1139.5051802733601,
            -0.70035164828640895,
            1.4634127243340299e-013,
            0.036091051735730698,
            1.0
        };

        public CameraMeasurementModel()
        {
        }

        [Unit("px")]
        [DisplayName("Row measurement noise")]
        [DefaultValue(6)]
        [Category("Sensor characteristics")]
        public double SigmaRow
        {
            get
            {
                return System.Math.Sqrt(NoiseCovariance[0, 0]);
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "SigmaRow", (object)value,
                        @"The standard deviation of the row measurement noise is less or equal to zero, is infinite or is NaN.");
                }

                NoiseCovariance[0, 0] = value * value;
            }
        }

        // This property can be configured, e.g. in the user interface of a dml file.
        [DisplayName("Column measurement noise")]
        [Unit("px")]
        [DefaultValue(6)]
        [Category("Sensor characteristics")]
        public double SigmaColumn
        {
            get
            {
                return System.Math.Sqrt(NoiseCovariance[1, 1]);
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "SigmaColumn", (object)value,
                        @"The standard deviation of the column measurement noise is less or equal to zero, is infinite or is NaN.");
                }

                NoiseCovariance[1, 1] = value * value;
            }
        }

        public override void h(CVComponentsSpace state, ref CameraSpace predictedMeasurement)
        {
            var x = state.X;
            var y = state.Y;

            //var row = ...
            //var column = ...

            // predictedMeasurement.Row = row;
            // predictedMeasurement.Column = column;


            // ref. to camera calibration matrix
            var row = x;
            var column = y;
            //var c = _calibrationMatrix;

            state.X = (_calibrationMatrix[0] * x + _calibrationMatrix[1] * y + _calibrationMatrix[3]) / (_calibrationMatrix[8] * x + _calibrationMatrix[9] * y + _calibrationMatrix[11]);
            state.Y = (_calibrationMatrix[4] * x + _calibrationMatrix[5] * y + _calibrationMatrix[7]) / (_calibrationMatrix[8] * x + _calibrationMatrix[9] * y + _calibrationMatrix[11]);

            predictedMeasurement.Row = row;
            predictedMeasurement.Column = column;
        }
    }
}
