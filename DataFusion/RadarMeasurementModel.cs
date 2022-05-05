
using System;
using System.ComponentModel;
using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Models.AdditiveNoise;

namespace DataFusion
{
    [DisplayName(@"Radar measurement model")]
    [Description("Describes the measurement capabilities of my sensor.")]

    class RadarMeasurementModel : MeasurementModel<RadarSpace, CVComponentsSpace>
    {
        private double _egoVelocity;
        private double _egoYawRate;

        private double _sensorPositionX;
        private double _sensorPositionY;
        private double _sensorRotationZ;
        
        public RadarMeasurementModel()
        {
        }

        [Unit("m")]
        [DisplayName("Range measurement noise")]
        [Description("The standard deviation of the radar's range measurement noise.")]
        [DefaultValue(1.2)]
        [Category("Sensor characteristics")]
        public double SigmaRange
        {
            get
            {
                return Math.Sqrt(NoiseCovariance[0, 0]);
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                    "SigmaRange", (object)value,
                    @"The standard deviation of the radar's range measurement noise is less or equal to zero, is infinite or is NaN.");
                }

                NoiseCovariance[0, 0] = value * value;
            }
        }

        // This property can be configured, e.g. in the user interface of a dml file.
        [DisplayName("Azimuth measurement noise")]
        [Unit("rad")]
        [Description("The standard deviation of the radar's azimuth measurement noise.")]
        [DefaultValue(0.01)]
        [Category("Sensor characteristics")]
        public double SigmaAzimuth
        {
            get
            {
                return Math.Sqrt(NoiseCovariance[1, 1]);
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                    "SigmaAzimuth", (object)value,
                    @"The standard deviation of the radar's azimuth measurement noise is less or equal to zero, is infinite or is NaN.");
                }

                NoiseCovariance[1, 1] = value * value;
            }
        }

        // This property can be configured, e.g. in the user interface of a dml file.
        [Description("The standard deviation of the radar's range rate measurement noise.")]
        [DisplayName("Range rate measurement noise")]
        [Unit("m/s")]
        [DefaultValue(0.45)]
        [Category("Sensor characteristics")]
        public double SigmaRangeRate
        {
            get
            {
                return Math.Sqrt(NoiseCovariance[2, 2]);
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                    "SigmaRangeRate", (object)value,
                    @"The standard deviation of the radar's range rate measurement noise is less or equal to zero, is infinite or is NaN.");
                }

                NoiseCovariance[2, 2] = value * value;
            }
        }

        // This property can be configured, e.g. in the user interface of a dml file.
        [DisplayName(@"Longitudinal sensor position")]
        [Description(
            "The displacement of the sensor along the X axis relative to the coordinate frame of the vehicle.")]
        [Unit("m")]
        [DefaultValue(0.0)]
        [Category("Mounting")]
        public double SensorPositionX
        {
            get { return _sensorPositionX; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"The sensor position along X axis is infinite or not a number.");
                }

                _sensorPositionX = value;
            }
        }

        [DisplayName(@"Lateral sensor position")]
        [Description(
            "The displacement of the sensor along the Y axis relative to the coordinate frame of the vehicle.")]
        [Unit("m")]
        [DefaultValue(0.0)]
        [Category("Mounting")]
        public double SensorPositionY
        {
            get { return _sensorPositionY; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"The sensor position along Y axis is infinite or not a number.");
                }

                _sensorPositionY = value;
            }
        }

        [DisplayName(@"Sensor rotation")]
        [Description("The rotation around the Z axis of the sensor relative to the coordinate frame of the vehicle.")]
        [Unit("rad")]
        [DefaultValue(0.0)]
        [Category("Mounting")]
        public double SensorRotationZ
        {
            get { return _sensorRotationZ; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value) || value < -Math.PI || value >= Math.PI)
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"The sensor rotation around Z axis is less than -π, greater than or equal to π, infinite or not a number.");
                }

                _sensorRotationZ = value;
            }
        }

        [DisplayName(@"Ego velocity")]
        [Description("The magnitude of the vehicle's ego motion velocity calculated at the origin of the vehicle's coordinate system.")]
        [Unit("m/s")]
        [Category("Ego motion")]
        public double EgoVelocity
        {
            get { return _egoVelocity; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"The ego velocity is infinite or not a number.");
                }

                _egoVelocity = value;
            }
        }

        [DisplayName(@"Ego yaw rate")]
        [Description("The ego yaw rate of the vehicle with a positive value assumed for counterclockwise rotation.")]
        [Unit("rad/s")]
        [Category("Ego motion")]
        public double EgoYawRate
        {
            get { return _egoYawRate; }
            set
            {
                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"The ego yaw rate is infinite or not a number.");
                }

                _egoYawRate = value;
            }
        }

        public override void h(CVComponentsSpace state, ref RadarSpace predictedMeasurement)
        {
            // todo: calculate predictedMeasurement from state
            //var m = state.X;
            //var n = state.Y;
            //var Vx = state.Vx;
            //var Vy = state.Vy;
            //var radialVel = Math.Sqrt((Vx * Vx) + Vy * Vy);
            //var Phi = Math.Atan2(n, m);
            //var range = Math.Sqrt(m * m + n * n);

            //predictedMeasurement.RadialVelocity = radialVel;
            //predictedMeasurement.Phi = Phi;
            //predictedMeasurement.Range = range;



            //deriving from predictedMeasurement class
            //ref. nonlinear radar measurement model
            predictedMeasurement.RadialVelocity = Math.Sqrt((state.Vx - EgoVelocity) * (state.Vx - EgoVelocity) + (state.Vy) * (state.Vy));

            predictedMeasurement.Phi = Math.Atan2(state.Y-SensorPositionY, state.X-SensorPositionX );

            predictedMeasurement.Range = Math.Sqrt((state.X - SensorPositionX) * (state.X - SensorPositionX)+(state.Y - SensorPositionY) * (state.Y - SensorPositionY));
        }
    }
}
