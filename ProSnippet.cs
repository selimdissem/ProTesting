using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLayerExamples
{
  class ProSnippet : MapTool
  {
    public void CreateGraphicsLayer()
    {
      // cref: ArcGIS.Desktop.Mapping.GraphicsLayerCreationParams
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
      // cref: ArcGIS.Desktop.Mapping.LayerFactory
      // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateGroupLayer(ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
      #region Create GraphicsLayer
      var map = MapView.Active.Map;
      if (map.MapType != MapType.Map)
        return;// not 2D

      var gl_param = new GraphicsLayerCreationParams { Name = "Graphics Layer" };
      QueuedTask.Run(() =>
      {
        //By default will be added to the top of the TOC
        var graphicsLayer = LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.GraphicsLayer>(gl_param, map);

        //Add to the bottom of the TOC
        gl_param.MapMemberIndex = -1; //bottom
        LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.GraphicsLayer>(gl_param, map);

        //Add a graphics layer to a group layer...
        var group_layer = map.GetLayersAsFlattenedList().OfType<GroupLayer>().First();
        LayerFactory.Instance.CreateLayer<ArcGIS.Desktop.Mapping.GraphicsLayer>(gl_param, group_layer);

        //TODO...use the graphics layer
        //

        // or use the specific CreateGroupLayer method
        LayerFactory.Instance.CreateGroupLayer(map, -1, "Graphics Layer");
      });
      #endregion
    }
  }
}
