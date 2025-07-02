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
