using System.ComponentModel;
using System.Drawing;
using Baselabs.Statistics;
using Baselabs.Statistics.Distributions;
using Baselabs.Statistics.Spaces;

namespace DataFusion
{
    /// <summary>
    /// A nonlinear transformation to compensate the ego motion in vehicle tracking applications.
    /// </summary>
    /// <typeparam name="TStateSpace">The <see cref="Space"/> of the tracked vehicle.</typeparam>
    /// <typeparam name="TEgoMotionSpace">The <see cref="Space"/> of the ego vehicle.</typeparam>
    /// <remarks>
    /// When vehicle-mounted sensors like a radar are used to determine the position
    /// of a vehicle in front, this model can be used to separate the motion of the ego vehicle from the motion
    /// of the tracked vehicle.
    /// </remarks>
    /// <seealso cref="Conditional.Marginalize{TSpace,TConditional,TConditionSpace1,TConditionSpace2}(TConditional,Baselabs.Statistics.Distributions.SigmaPoints{TConditionSpace1},Baselabs.Statistics.Distributions.SigmaPoints{TConditionSpace2})"/>
    [Description(
        "An ego motion compensation model. " +
        "The transformation implements a compensation of the ego motion in tracking applications.")]
    [ToolboxBitmap(typeof(ResourceFinder), "BaselabsCreate.png")]
    public class EgoMotionCompensation<TStateSpace, TEgoMotionSpace> :
        IExpectation<TStateSpace>,
        IConditional<TStateSpace, TEgoMotionSpace>
        where TStateSpace : Space, ICartesian, new()
        where TEgoMotionSpace : Space, ICartesian, IHeading, new()
    {
        /// <summary>
        /// Gets or sets the first condition, which is the system state of the tracked object.
        /// </summary>
        public TStateSpace Condition { get; set; }

        /// <summary>
        /// Gets or sets the second condition, which is the system state of the ego vehicle.
        /// </summary>
        public TEgoMotionSpace Condition2 { get; set; }

        /// <inheritdoc/>
        public TStateSpace Expectation
        {
            get
            {
                var result = Condition.Clone();

                result.X = (Condition.X - Condition2.X) * System.Math.Cos(Condition2.G) + (Condition.Y - Condition2.Y) * System.Math.Sin(Condition2.G);
                result.Y = -(Condition.X - Condition2.X) * System.Math.Sin(Condition2.G) + (Condition.Y - Condition2.Y) * System.Math.Cos(Condition2.G);

                return result;
            }
        }
    }
}