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
  class SnippetsGraphicsLayer : MapTool
  {
    // cref: ArcGIS.Desktop.Mapping.GraphicsLayerCreationParams
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateLayer<T>(ArcGIS.Desktop.Mapping.LayerCreationParams,ArcGIS.Desktop.Mapping.ILayerContainerEdit)
    // cref: ArcGIS.Desktop.Mapping.LayerFactory
    // cref: ArcGIS.Desktop.Mapping.LayerFactory.CreateGroupLayer(ArcGIS.Desktop.Mapping.ILayerContainerEdit, System.Int32, System.String)
    #region Create GraphicsLayer
    /// <summary>
    /// Creates a new graphics layer in the active 2D map.
    /// </summary>
    /// <remarks>This method adds a graphics layer to the active map if it is a 2D map. The layer is added to
    /// the top of the table of contents by default,  but can also be added to the bottom or within a group layer. If
    /// the active map is not a 2D map, the method returns without creating a layer.</remarks>
    /// <param name="graphicLayerName">The name to assign to the new graphics layer.</param>
    /// <param name="map"> The map to which the graphics layer will be added.</param>
    public static async Task CreateGraphicsLayerAsync(string graphicLayerName, Map map)
    {
      if (map?.MapType != MapType.Map)
        return;// not 2D

      var gl_param = new GraphicsLayerCreationParams { Name = graphicLayerName };
      await QueuedTask.Run(() =>
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
    }
    #endregion
  }
}
