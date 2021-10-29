using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AecTech.Utilities.BaseClasses;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;

namespace AecTech.SecondButton
{
    public class SecondButtonModel : ModelBase
    {
        public SecondButtonModel(UIApplication uiApp) : base(uiApp)
        {
        }

        public ObservableCollection<RoomWrapper> CollectSpatialObjects()
        {
            var spatialObjects = new FilteredElementCollector(Doc)
                .OfClass(typeof(SpatialElement))
                .WhereElementIsNotElementType()
                .Cast<SpatialElement>()
                .Where(x => x is Room || x is Space)
                .Select(x => new RoomWrapper(x))
                .OrderBy(x => x.Area);

            return new ObservableCollection<RoomWrapper>(spatialObjects);
        }

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
