﻿
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BASELABS Create.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using  System;
using System.Collections.Generic ;

namespace DataFusion
{
    partial class DataFusion1
    {
        #region Connector Fields
		#region visualization
		// when the project item 'DataFusionVisualization1.tt' is no longer available and this section generates compiler errors, right-click on 'DataFusionConnector.tt' and select 'Run Custom Tool' to regenerate this file

		private DataFusion1VisualizationConnector _visualization_4d046121_ad77_4648_87bb_8cf48a6f01cd;

		#endregion visualization
        #endregion Connector Fields

        partial void Initialize()
        {
#if DATAFUSIONEVENTS
            #region Connector Initializations
			#region visualization
			// when the project item 'DataFusionVisualization1.tt' is no longer available and this section generates compiler errors, right-click on 'DataFusionConnector.tt' and select 'Run Custom Tool' to regenerate this file

			_visualization_4d046121_ad77_4648_87bb_8cf48a6f01cd = new DataFusion1VisualizationConnector(this);

			#endregion visualization
            #endregion Connector Initializations
#endif
        }

        partial void Cleanup()
        {
#if DATAFUSIONEVENTS
            #region Connector Cleanups
			#region visualization
			// when the project item 'DataFusionVisualization1.tt' is no longer available and this section generates compiler errors, right-click on 'DataFusionConnector.tt' and select 'Run Custom Tool' to regenerate this file

			if(_visualization_4d046121_ad77_4648_87bb_8cf48a6f01cd is IDisposable){ ((IDisposable)_visualization_4d046121_ad77_4648_87bb_8cf48a6f01cd).Dispose(); }

			#endregion visualization
            #endregion Connector Cleanups
#endif
        }
    }
}