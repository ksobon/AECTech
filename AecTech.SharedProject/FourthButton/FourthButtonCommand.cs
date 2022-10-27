using System;
using System.Reflection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AecTech.Utilities;

namespace AecTech.FourthButton
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class FourthButtonCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                DockablePanelUtils.ShowDockablePanel(commandData.Application);

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
                    MethodBase.GetCurrentMethod()?.DeclaringType?.Name,
                    "Fourth" + Environment.NewLine + "Button",
                    assembly.Location,
                    MethodBase.GetCurrentMethod()?.DeclaringType?.FullName)
                {
                    ToolTip = "Fourth button tooltip.",
                    LargeImage = ImageUtils.LoadImage(assembly, "_32x32.fourthButton.png")
                });
        }
    }
}
