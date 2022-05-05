using System;
using System.ComponentModel;

using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Tracking.Management;
using Baselabs.Statistics.Tracking.Tracks;

namespace DataFusion
{
    [DisplayName(@"Clutter track remover")]
    [Description("Describes which tracks shall be removed.")]
    public class ClutterRemover : TrackRemover<CVComponentsSpace>
    {
        private double _minimumExistenceProbability;

        public ClutterRemover() : this(0.1) { }

        public ClutterRemover(double minimumExistenceProbability)
        {
            MinimumExistenceProbability = minimumExistenceProbability;
        }

        [DisplayName(@"Minimum existence probability")]
        [Description("The minimum existence probability against which the existence probability " +
                     "of the track is compared. The track is suggested for removal if its " +
                     "existence probability is smaller than this value.")]
        [Category("Sensor characteristics")]
        [DefaultValue(0.1)]
        public double MinimumExistenceProbability
        {
            get { return _minimumExistenceProbability; }
            set
            {
                if (!(value >= 0 && value <= 1))
                {
                    throw new ArgumentOutOfRangeException(
                        "value",
                        value,
                        "The value of the minimum existence probability is not between zero and one.");
                }
                _minimumExistenceProbability = value;
            }
        }

        public override bool ShouldRemove(GaussianTrack<CVComponentsSpace> track)
        {
            return track.Existence.ExistenceProbability < _minimumExistenceProbability;
        }
    }
}
