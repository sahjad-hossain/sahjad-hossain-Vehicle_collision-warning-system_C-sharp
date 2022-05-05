using System;
using System.Collections.Generic;
using System.ComponentModel;
using Baselabs.Statistics.Distributions;
using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Tracking.Distributions;
using Baselabs.Statistics.Tracking.Management;
using Baselabs.Statistics.Tracking.Tracks;

namespace DataFusion
{
    // To do: Replace 'RadarSpace' by the space that is used by the measurement model.
    // To do: Replace 'CVComponentsSpace' by the space that is used by the system model.
    [DisplayName(@"Radar track proposer")]
    [Description("Creates tracks from a measurement.")]
    public class RadarTrackProposer : TrackProposer<RadarSpace, CVComponentsSpace>
    {
        private double _egoVelocity;
        private double _egoYawRate;

        private double _initialExistenceProbability;
        private double _sensorPositionX;
        private double _sensorPositionY;
        private double _sensorRotationZ;
        private double _sigmaX;
        private double _sigmaY;
        private double _sigmaVx;
        private double _sigmaVy;
        
        public sealed override IEnumerable<GaussianTrack<CVComponentsSpace>> CreateTracks(RadarSpace measurement)
        {
            // To do: Create tracks from the measurement.
            // SmartSensorLongitudinalVelocityTrackProposer class 
            // creates a single track from the given smart sensor measurement
            // mapping from radar space to new tracks 
            var state = new CVComponentsSpace();
            //state.X = ...
            //state.Y = ...
            //state.Vx = ...
            //state.Vy = ...
            state.X = System.Math.Cos(measurement.Phi)*(measurement.Range) + SensorPositionX;
            state.Y = System.Math.Sin(measurement.Phi)*(measurement.Range) + SensorPositionY;

            state.Vx = System.Math.Cos(measurement.Phi)*(measurement.RadialVelocity) + EgoVelocity;
            state.Vy = System.Math.Sin(measurement.Phi) * (measurement.RadialVelocity);

            var createdTrack = new GaussianTrack<CVComponentsSpace>(
                new Gaussian<CVComponentsSpace>(state,
                    new PositiveDefiniteMatrix<CVComponentsSpace>(_sigmaX * _sigmaX, _sigmaY * _sigmaY, _sigmaVx * _sigmaVx, _sigmaVy * _sigmaVy)),
                new Existence(_initialExistenceProbability));

            return new[] { createdTrack };
        }

        [Unit("m")]
        [DisplayName("Inflation noise X")]
        [Description("The standard deviation of the proposed track's position X.")]
        [Category("Sensor characteristics")]
        public double SigmaX
        {
            get
            {
                return _sigmaX;
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                    "SigmaX", (object)value,
                    @"The standard deviation of the inflation sigma x is less or equal to zero, is infinite or is NaN.");
                }

                _sigmaX = value;
            }
        }

        [Unit("m")]
        [DisplayName("Inflation noise Y")]
        [Description("The standard deviation of the proposed track's position Y.")]
        [Category("Sensor characteristics")]
        public double SigmaY
        {
            get
            {
                return _sigmaY;
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "SigmaY", (object)value,
                        @"The standard deviation of the inflation sigma y is less or equal to zero, is infinite or is NaN.");
                }

                _sigmaY = value;
            }
        }

        [Unit("m/s")]
        [DisplayName("Inflation noise Vx")]
        [Description("The standard deviation of the proposed track's velocity X.")]
        [Category("Sensor characteristics")]
        public double SigmaVx
        {
            get
            {
                return _sigmaVx;
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "SigmaVx", (object)value,
                        @"The standard deviation of the inflation sigma Vx is less or equal to zero, is infinite or is NaN.");
                }

                _sigmaVx = value;
            }
        }

        [Unit("m/s")]
        [DisplayName("Inflation noise Vy")]
        [Description("The standard deviation of the proposed track's velocity Y.")]
        [Category("Sensor characteristics")]
        public double SigmaVy
        {
            get
            {
                return _sigmaVy;
            }
            set
            {
                if (value <= 0.0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "SigmaVy", (object)value,
                        @"The standard deviation of the inflation sigma Vy is less or equal to zero, is infinite or is NaN.");
                }

                _sigmaVy = value;
            }
        }

        [DisplayName(@"Longitudinal sensor position")]
        [Description(
            "The displacement of the sensor along the X axis relative to the coordinate frame of the vehicle.")]
        [Unit("m")]
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

        [DisplayName(@"Initial existence probability")]
        [Description("The initial existence probability of the proposed new track.")]
        [Category("Tracking")]
        public double InitialExistenceProbability
        {
            get { return _initialExistenceProbability; }
            set
            {
                if (!(value >= 0 && value <= 1))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"The value of the initial existence probability is not between zero and one.");
                }
                _initialExistenceProbability = value;
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
    }
}
