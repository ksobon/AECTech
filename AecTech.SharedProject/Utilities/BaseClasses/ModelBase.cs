using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AecTech.Utilities.BaseClasses
{
    public abstract class ModelBase
    {
        public UIApplication UiApp { get; }
        public Document Doc { get; }

        protected ModelBase(UIApplication uiApp)
        {
            UiApp = uiApp;
            Doc = UiApp.ActiveUIDocument.Document;
        }
    }
}
