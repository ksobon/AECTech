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

                if (View != null)
                {
                    if (View.WindowState == WindowState.Minimized) View.WindowState = WindowState.Normal;
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
                View.Closing += OnViewClosing;

                var unused = new WindowInteropHelper(v)
                {
                    Owner = Process.GetCurrentProcess().MainWindowHandle
                };

                v.Show();

                return Result.Succeeded;
            }
            catch (Exception e)
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
