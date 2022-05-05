using Baselabs.Statistics.Spaces;

namespace DataFusion
{
    partial class DataFusion1Visualization
    {
        double[] _calibrationMatrix =
        {
            -224.11252745164600, 935.04128990035599, 11.549136555426900, 226.49587100989399,
            -119.96299327410701, 4.4416026412363901e-011, 942.46405013179594, -1139.5051802733601,
            -0.70035164828640895, 1.4634127243340299e-013, 0.036091051735730698, 1.0
        };
        
        //partial void MeasurementToSystem0(RadarSpace measurement, CartesianSpace measurementInSystemCoordinates)
        //{
        //    ToSystem(measurement, 3.55, 0.0, 0.0, measurementInSystemCoordinates);
        //}

        //partial void MeasurementToSystem1(CameraSpace measurement, CartesianSpace measurementInSystemCoordinates)
        //{
        //    var imageX = measurement.Column;
        //    var imageY = measurement.Row;
//
        //    var denominator = _calibrationMatrix[0] * _calibrationMatrix[5] - _calibrationMatrix[1] * _calibrationMatrix[4] +
        //        (_calibrationMatrix[1] * _calibrationMatrix[8] - _calibrationMatrix[0] * _calibrationMatrix[9]) * imageY +
        //        (_calibrationMatrix[4] * _calibrationMatrix[9] - _calibrationMatrix[5] * _calibrationMatrix[8]) * imageX;
//
        //    measurementInSystemCoordinates.X = (_calibrationMatrix[1] * _calibrationMatrix[7] - _calibrationMatrix[3] * _calibrationMatrix[5] +
        //        (_calibrationMatrix[11] * _calibrationMatrix[5] - _calibrationMatrix[7] * _calibrationMatrix[9]) * imageX +
        //        (_calibrationMatrix[3] * _calibrationMatrix[9] - _calibrationMatrix[1] * _calibrationMatrix[11]) * imageY) / denominator;
//
        //    measurementInSystemCoordinates.Y = (_calibrationMatrix[0] * _calibrationMatrix[7] - _calibrationMatrix[3] * _calibrationMatrix[4] +
        //        (_calibrationMatrix[11] * _calibrationMatrix[4] - _calibrationMatrix[7] * _calibrationMatrix[8]) * imageX +
        //        (_calibrationMatrix[3] * _calibrationMatrix[8] - _calibrationMatrix[0] * _calibrationMatrix[11]) * imageY) / -denominator;
        //}
    }
}
