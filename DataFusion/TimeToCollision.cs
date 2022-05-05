using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baselabs.Statistics.Spaces;
using Baselabs.Statistics.Tracking.Tracks;

namespace DataFusion
{
    static class TimeToCollision
    {
        public static double GetTimeToCollision(IEnumerable<GaussianTrack<CVComponentsSpace>> tracks, double egoVelocity)
        {
            double ttc = 250;
            double mindis = 250.0;
            double OTHVelocity = 20.0;
            //Tracking
            //TrackExistP = track.Existence.ExistenceProbability;
            //DistanceX = trackState.Expectation.X;
            //LateralTProab = track.State.Expectation.X;
            //longitudinalProb = track.State.Expectation.Y;
            //OTH_V_Velocity = track.State.Expectation.Vx;

            foreach (var track in tracks)
            {
                if (track.Existence.ExistenceProbability > .99)
                {
                    if ((track.State.Expectation.Y < 0.80) || (track.State.Expectation.Y > -0.80))
                    {
                        if (track.State.Expectation.X < mindis)
                        {
                            mindis = track.State.Expectation.X;
                            if (track.State.Expectation.Vx < egoVelocity)
                            {
                                OTHVelocity = egoVelocity - track.State.Expectation.Vx;
                            }
                            ttc = mindis / OTHVelocity;
                        }

                    }

                }

            }
            return ttc;
        }
    }
}
