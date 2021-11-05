using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using AecTech.Utilities;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AecTech.ThirdButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ThirdButtonCommand : IExternalCommand
    {
        private static ThirdButtonView View { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiApp = commandData.Application;

                // (Konrad) Because this is a Modeless Dialog (launched via Show() instead of ShowDialog()), this command
                // doesn't wait for that dialog to be closed. It means that you can press this button multiple times, and
                // that could potentially open the UI multiple times. To prevent that from happening, we store a reference
                // to the View here, and check if it's still open/minimized. Instead of re-creating it we can then just
                // maximize/restore it. 
                if (View != null)
                {
                    if (View.WindowState == WindowState.Minimized)
                        View.WindowState = WindowState.Normal;
                    View.Activate();

                    return Result.Succeeded;
                }

                var m = new ThirdButtonModel(uiApp);
                var vm = new ThirdButtonViewModel(m);
                var v = new ThirdButtonView
                {
                    DataContext = vm
                };

                View = v;
                // (Konrad) We can keep track of the Window Closing event to know when that UI is actually closed. When that
                // happens we would reset the View property here to Null, so that when user presses the button we can create
                // a brand new instance of that View.
                View.Closing += OnViewClosing;

                var unused = new WindowInteropHelper(v)
                {
                    Owner = Process.GetCurrentProcess().MainWindowHandle
                };

                v.Show();

                return Result.Succeeded;
            }
            catch
            {
                return Result.Failed;
            }
        }

        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            panel.AddItem(
                new PushButtonData(
                    MethodBase.GetCurrentMethod().DeclaringType?.Name,
                    "Third" + Environment.NewLine + "Button",
                    assembly.Location,
                    MethodBase.GetCurrentMethod().DeclaringType?.FullName)
                {
                    ToolTip = "Third button tooltip.",
                    LargeImage = ImageUtils.LoadImage(assembly, "_32x32.thirdButton.png")
                });
        }

        private static void OnViewClosing(object sender, CancelEventArgs e)
        {
            View = null;
            if (sender is ThirdButtonView view)
                view.Closing -= OnViewClosing;
        }
    }
}
