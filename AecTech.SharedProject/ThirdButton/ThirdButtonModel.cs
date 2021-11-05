using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AecTech.SecondButton;
using AecTech.Utilities.BaseClasses;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;

namespace AecTech.ThirdButton
{
    public class ThirdButtonModel : ModelBase
    {
        public ThirdButtonModel(UIApplication uiApp) : base(uiApp)
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
            AppCommand.ThirdButtonHandler.Arg1 = selected;
            AppCommand.ThirdButtonHandler.Request = RequestId.Delete;
            AppCommand.ThirdButtonEvent.Raise();
        }

        public void Select(List<RoomWrapper> selected)
        {
            AppCommand.ThirdButtonHandler.Arg1 = selected;
            AppCommand.ThirdButtonHandler.Request = RequestId.Select;
            AppCommand.ThirdButtonEvent.Raise();
        }
    }
}
