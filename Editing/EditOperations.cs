using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Core.Internal.Geometry;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLayers
{
  /// <summary>
  /// Provides operations for moving features within a feature layer in an ArcGIS map view.
  /// </summary>
  /// <remarks>This class includes methods to move selected features by a specified offset or to a specific
  /// coordinate. It operates on the first feature layer in the active map view and requires that features be selected
  /// for the operations to take effect. If no features are selected, the operations will not be performed.</remarks>
  internal class SnippetsEditOperation
  {
    // cref: ArcGIS.Desktop.Editing.EditOperation.Move(ArcGIs.Desktop.Mapping.SelectionSet, System.Double, System.Double)
    #region Move features
    /// <summary>
    /// Moves the shapes (geometries) of all selected features of a given feature layer of the active map view by a fixed offset.
    /// </summary>
    /// <remarks>This method retrieves the selected features from the first feature layer in the
    /// active map view  and moves them by a fixed offset of 10 units along both the X and Y axes. If no features
    /// are  selected, the method performs no action.</remarks>
    /// <param name="featureLayer"> The feature layer containing the features to be moved.</param>
    /// <returns>true if geometries were moved, false otherwise</returns>
    internal static async Task<bool> MoveFeaturesByOffsetAsync(FeatureLayer featureLayer, double xOffset, double yOffset)
    {
      return await QueuedTask.Run<bool>(() =>
      {
        // If there are no selected features, return
        if (featureLayer.GetSelection().GetObjectIDs().Count == 0)
          return false;
        // set up a dictionary to store the layer and the object IDs of the selected features
        var selectionDictionary = new Dictionary<MapMember, List<long>>
          {
                  { featureLayer, featureLayer.GetSelection().GetObjectIDs().ToList() }
          };
        var moveEditOperation = new EditOperation() { Name = "Move features" };
        moveEditOperation.Move(SelectionSet.FromDictionary(selectionDictionary), xOffset, yOffset);  //specify your units along axis to move the geometry
        if (!moveEditOperation.IsEmpty)
        {
          var result = moveEditOperation.Execute();
          return result; // return the operation result: true if successful, false if not
        }
        return false; // return false to indicate that the operation was not empty
      });
    }
    #endregion

    // cref: ArcGIS.Desktop.Editing.EditOperation.Modify(LAYER,INT64,GEOMETRY,DICTIONARY{STRING,OBJECT})
    #region Move feature to a specific coordinate
    /// <summary>
    /// Moves the first selected feature in the specified feature layer to the given coordinates.
    /// </summary>
    /// <remarks>This method modifies the geometry of the first selected feature in the specified layer to
    /// match the provided coordinates. If no features are selected, the operation will not be performed, and the method
    /// will return <see langword="false"/>.</remarks>
    /// <param name="featureLayer">The feature layer containing the feature to be moved. Cannot be null.</param>
    /// <param name="xCoordinate">The x-coordinate to which the feature will be moved.</param>
    /// <param name="yCoordinate">The y-coordinate to which the feature will be moved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the feature was
    /// successfully moved; otherwise, <see langword="false"/>.</returns>
    internal static async Task<bool> MoveFeatureToCoordinateAsync(FeatureLayer featureLayer, double xCoordinate, double yCoordinate)
    {
      return await QueuedTask.Run<bool>(() =>
      {
        //Get all of the selected ObjectIDs from the layer.
        var mySelection = featureLayer.GetSelection();
        var selOid = mySelection.GetObjectIDs().FirstOrDefault();

        var moveToPoint = new MapPointBuilderEx(xCoordinate, yCoordinate, 0.0, 0.0, featureLayer.GetSpatialReference());

        var moveEditOperation = new EditOperation() { Name = "Move features" };
        moveEditOperation.Modify(featureLayer, selOid, moveToPoint.ToGeometry());  //Modify the feature to the new geometry 
        if (!moveEditOperation.IsEmpty)
        {
          var result = moveEditOperation.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
          return result; // return the operation result: true if successful, false if not
        }
        return false; // return false to indicate that the operation was not empty
      });
    }
    #endregion

  }
}