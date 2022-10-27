using System;
using Autodesk.Revit.UI;

namespace AecTech.FourthButton
{
    public static class DockablePanelUtils
    {
        public static void RegisterDockablePanel(UIControlledApplication app)
        {
            var vm = new DockablePanelViewModel();
            var v = new DockablePanelPage
            {
                DataContext = vm
            };

            _ = new DockablePaneProviderData
            {
                FrameworkElement = v,
                InitialState = new DockablePaneState
                {
                    DockPosition = DockPosition.Tabbed,
                    TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
                },
                VisibleByDefault = true
            };

            var paneId = new DockablePaneId(new Guid("50035162-82b1-4fc8-a083-23afd845f625"));
            try
            {
                // (Konrad) It's possible that a dock-able panel with the same id already exists
                // This ensures that we don't get an exception here. 
                app.RegisterDockablePane(paneId, "Aec Tech", v);
            }
            catch
            {
                // ignored
            }
        }

        public static void ShowDockablePanel(UIApplication app)
        {
            var paneId = new DockablePaneId(new Guid("50035162-82b1-4fc8-a083-23afd845f625"));
            var dp = app.GetDockablePane(paneId);
            dp?.Show();
        }
    }
}
