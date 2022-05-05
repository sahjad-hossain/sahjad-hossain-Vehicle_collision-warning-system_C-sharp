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
    [DisplayName(@"Camera track proposer")]
    [Description("Creates tracks from a measurement.")]
    public class CameraTrackProposer : TrackProposer<CameraSpace, CVComponentsSpace>
    {
        private double _initialExistenceProbability = 0.6;
        private double _sigmaX = 3.0;
        private double _sigmaY = 1.0;
        private double _sigmaVx = 5.0;
        private double _sigmaVy = 2.0;
        private double _egoVelocity;
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

        public sealed override IEnumerable<GaussianTrack<CVComponentsSpace>> CreateTracks(CameraSpace measurement)
        {
            var c = _calibrationMatrix;
            var imageX = measurement.Column;
            var imageY = measurement.Row;

            var denominator
                = ((c[0] * c[5]) - (c[1] * c[4]))
                  + ((c[1] * c[8]) - (c[0] * c[9])) * imageY
                  + ((c[4] * c[9]) - (c[5] * c[8])) * imageX;

            if (Math.Abs(denominator) < 10e-8)
            {
                throw new InvalidOperationException();
            }

            var stateXNumerator
                = (c[1] * c[7] - c[3] * c[5])
                  + (c[11] * c[5] - c[7] * c[9]) * imageX
                  + (c[3] * c[9] - c[1] * c[11]) * imageY;

            var stateYNumerator
                = (c[0] * c[7]) - (c[3] * c[4])
                  + ((c[11] * c[4]) - (c[7] * c[8])) * imageX
                  + ((c[3] * c[8]) - (c[0] * c[11])) * imageY;
            
            var expectation = new CVComponentsSpace();
            expectation.X = stateXNumerator / denominator;
            expectation.Y = -stateYNumerator / denominator;
            expectation.Vx = EgoVelocity;
            expectation.Vy = 0;

            var covariance = new PositiveDefiniteMatrix<CVComponentsSpace>(_sigmaX * _sigmaX, _sigmaY * _sigmaY,
                _sigmaVx * _sigmaVx, _sigmaVy * _sigmaVy);

            var state = new Gaussian<CVComponentsSpace>(expectation, covariance);
            
            var existence = new Existence(_initialExistenceProbability);
         
            var createdTrack = new GaussianTrack<CVComponentsSpace>(state, existence);

            return new[] { createdTrack };
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
    }
}
