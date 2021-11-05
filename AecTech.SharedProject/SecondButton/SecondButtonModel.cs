using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AecTech.Utilities.BaseClasses;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace AecTech.SecondButton
{
    public class SecondButtonModel : ModelBase
    {
        public SecondButtonModel(UIApplication uiApp) : base(uiApp)
        {
        }

        /// <summary>
        /// Method for retrieving Rooms from the Revit Database. We use Observable Collection here because
        /// WPF can bind to it. Typical list doesn't have INotifyCollectionChanged implemented for it while
        /// Observable Collections do. That's why every time there is a change it can notify the UI about it.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<RoomWrapper> CollectSpatialObjects()
        {
            var spatialObjects = new FilteredElementCollector(Doc)
                .OfClass(typeof(SpatialElement))
                .WhereElementIsNotElementType()
                .Cast<SpatialElement>()
                .Where(x => x is Room)
                .Select(x => new RoomWrapper(x))
                .OrderBy(x => x.Area);

            return new ObservableCollection<RoomWrapper>(spatialObjects);
        }

        /// <summary>
        /// Method for deleting Rooms from the Revit model. Every time we make changes to the Revit model
        /// we have to do it inside of a Transaction. Transactions ensure that nothing else is making a
        /// change to the model at the time that we are avoiding conflicts and corruption in the file.
        /// </summary>
        /// <param name="selected"></param>
        public void Delete(List<RoomWrapper> selected)
        {
            var ids = selected.Select(x => x.Id).ToList();
            using (var trans = new Transaction(Doc, "Delete Room/Space"))
            {
                trans.Start();
                Doc.Delete(ids);
                trans.Commit();
            }
        }
    }
}
