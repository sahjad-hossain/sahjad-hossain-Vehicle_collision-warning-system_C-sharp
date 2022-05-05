using System;
using System.ComponentModel;
using Baselabs.Statistics.Models.LinearTransition;
using Baselabs.Statistics.Models.TypeConverters;
using Baselabs.Statistics.Spaces;

namespace DataFusion
{
    [DisplayName("Constant velocity (CV)")]
    [Description(
        "A motion model of an object represented by a position and velocity components. " +
        "The model assumes a motion with a constant velocity.")]
    public class CVVectorialModel : SystemModel<CVComponentsSpace, CVComponentsErrorSpace>
    {
        public CVVectorialModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CVVectorialModel" /> class
        /// with the given process noise covariance matrix.
        /// </summary>
        /// <param name="processNoiseCovariance">The process noise covariance matrix.</param>
        public CVVectorialModel(PositiveDefiniteMatrix<CVComponentsErrorSpace> processNoiseCovariance)
            : base(processNoiseCovariance) { }
        
        public override void FillTransitionMatrix(TimeSpan deltaT, ref SquareMatrix<CVComponentsSpace> F)
        {
            var T = deltaT.TotalSeconds;

            //F[StateIdxs.X, StateIdxs.X] = ...
            //F[StateIdxs.X, StateIdxs.Y] = ...
            //F[StateIdxs.X, StateIdxs.Vx] = ...
            //F[StateIdxs.X, StateIdxs.Vy] = ...
            //F[StateIdxs.Y, StateIdxs.X] = ...
            //...
            F[StateIdxs.X, StateIdxs.X] = 1;
            F[StateIdxs.X, StateIdxs.Vx] = T;
            F[StateIdxs.X, StateIdxs.Y] = 0;
            F[StateIdxs.X, StateIdxs.Vy] = 0;

            F[StateIdxs.Y, StateIdxs.X] = 0;
            F[StateIdxs.Y, StateIdxs.Vx] = 0;
            F[StateIdxs.Y, StateIdxs.Y] = 1;
            F[StateIdxs.Y, StateIdxs.Vy] = T;

            F[StateIdxs.Vx, StateIdxs.X] = 0;
            F[StateIdxs.Vx, StateIdxs.Vx] = 1;
            F[StateIdxs.Vx, StateIdxs.Y] = 0;
            F[StateIdxs.Vx, StateIdxs.Vy] = 0;

            F[StateIdxs.Vy, StateIdxs.X] = 0;
            F[StateIdxs.Vy, StateIdxs.Vx] = 0;
            F[StateIdxs.Vy, StateIdxs.Y] = 0;
            F[StateIdxs.Vy, StateIdxs.Vy] = 1;




        }

        public override void FillNoiseDiscretizationMatrix(TimeSpan deltaT, ref Matrix<CVComponentsSpace, CVComponentsErrorSpace> G)
        {
            var T = deltaT.TotalSeconds;

            //G[StateIdxs.X, NoiseIdxs.Ax] = ...
            //G[StateIdxs.X, NoiseIdxs.Ay] = ...

            //G[StateIdxs.Y, NoiseIdxs.Ax] = ...
            //G[StateIdxs.Y, NoiseIdxs.Ay] = ...

            //G[StateIdxs.Vx, NoiseIdxs.Ax] = ...
            //G[StateIdxs.Vx, NoiseIdxs.Ay] = ...

            //G[StateIdxs.Vy, NoiseIdxs.Ax] = ...
            //G[StateIdxs.Vy, NoiseIdxs.Ay] = ...

            G[StateIdxs.X, NoiseIdxs.Ax] = (.5)*(T*T);
            G[StateIdxs.X, NoiseIdxs.Ay] = 0;

            G[StateIdxs.Y, NoiseIdxs.Ax] = 0;
            G[StateIdxs.Y, NoiseIdxs.Ay] = (.5)*(T*T);

            G[StateIdxs.Vx, NoiseIdxs.Ax] = T;
            G[StateIdxs.Vx, NoiseIdxs.Ay] = 0;

            G[StateIdxs.Vy, NoiseIdxs.Ax] = 0;
            G[StateIdxs.Vy, NoiseIdxs.Ay] = T;
        }

        /// <summary>
        /// Provides the indexes of the state space.
        /// </summary>
        protected static readonly CVComponentsSpace StateIdxs = Space.GetIndices<CVComponentsSpace>();

        /// <summary>
        /// Provides the indexes of the process noise space.
        /// </summary>
        protected static readonly CVComponentsErrorSpace NoiseIdxs = Space.GetIndices<CVComponentsErrorSpace>();

        /// <summary>
        /// The standard deviation of the acceleration process noise [m/s²].
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value to set is less or equal to zero, infinite or not a number.</exception>     
        /// <remarks>
        /// The value is equal to the square root of the corresponding entry
        /// on the diagonal of the measurement noise covariance matrix.
        /// </remarks>
        [DisplayName(@"Acceleration X process noise")]
        [Description(
            "The standard deviation of the acceleration X process noise.")]
        [Unit("m/s\xB2")]
        [TypeConverter(typeof(PositiveNonZeroValueConverter))]
        [DefaultValue(2.0)]
        [Category("System characteristics")]
        public double SigmaAccelerationX
        {
            get
            {
                return System.Math.Sqrt(NoiseCovariance[NoiseIdxs.Ax, NoiseIdxs.Ax]);
            }
            set
            {
                if (value <= 0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"Standard deviation of the acceleration X process noise is less or equal to zero, infinite or is NaN.");
                }
                NoiseCovariance[NoiseIdxs.Ax, NoiseIdxs.Ax] = value * value;
            }
        }

        /// <summary>
        /// The standard deviation of the acceleration process noise [m/s²].
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value to set is less or equal to zero, infinite or not a number.</exception>     
        /// <remarks>
        /// The value is equal to the square root of the corresponding entry
        /// on the diagonal of the measurement noise covariance matrix.
        /// </remarks>
        [DisplayName(@"Acceleration Y process noise")]
        [Description(
            "The standard deviation of the acceleration Y process noise.")]
        [Unit("m/s\xB2")]
        [TypeConverter(typeof(PositiveNonZeroValueConverter))]
        [DefaultValue(2.0)]
        [Category("System characteristics")]
        public double SigmaAccelerationY
        {
            get
            {
                return System.Math.Sqrt(NoiseCovariance[NoiseIdxs.Ay, NoiseIdxs.Ay]);
            }
            set
            {
                if (value <= 0 || double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        @"Standard deviation of the acceleration Y process noise is less or equal to zero, infinite or is NaN.");
                }
                NoiseCovariance[NoiseIdxs.Ay, NoiseIdxs.Ay] = value * value;
            }
        }
    }
}
