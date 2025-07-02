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
    internal class SnippetsEditOperation
    {
        /// <summary>
        /// Moves the selected features in the first feature layer of the active map view by a fixed offset.
        /// </summary>
        /// <remarks>This method retrieves the selected features from the first feature layer in the
        /// active map view  and moves them by a fixed offset of 10 units along both the X and Y axes. If no features
        /// are  selected, the method performs no action.</remarks>
        /// <returns></returns>
        internal static async Task MoveFeaturesAsync()
        {
            // Get all of the selected ObjectIDs from the first feature layer in the active mapview.
            var firstLayer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault();
            var selectionfromMap = firstLayer.GetSelection();

            await QueuedTask.Run(() =>
            {
                // If there are no selected features, return
                if (selectionfromMap.GetObjectIDs().Count == 0)
                    return;
                // set up a dictionary to store the layer and the object IDs of the selected features
                var selectionDictionary = new Dictionary<MapMember, List<long>>();
                selectionDictionary.Add(firstLayer as MapMember, selectionfromMap.GetObjectIDs().ToList());

                var moveFeature = new EditOperation() { Name = "Move features" };
                //at 2.x - moveFeature.Move(selectionDictionary, 10, 10);  //specify your units along axis to move the geometry
                moveFeature.Move(SelectionSet.FromDictionary(selectionDictionary), 10, 10);  //specify your units along axis to move the geometry
                if (!moveFeature.IsEmpty)
                {
                    var result = moveFeature.Execute(); //Execute and ExecuteAsync will return true if the operation was successful and false if not
                }
            });
            
        }
}
}
