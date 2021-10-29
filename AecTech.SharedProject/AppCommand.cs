using System;
using System.Linq;
using AecTech.FirstButton;
using AecTech.SecondButton;
using AecTech.ThirdButton;
using Autodesk.Revit.UI;

namespace AecTech
{
    public class AppCommand : IExternalApplication
    {
        public static ThirdButtonRequestHandler ThirdButtonHandler { get; set; }
        public static ExternalEvent ThirdButtonEvent { get; set; }

        public Result OnStartup(UIControlledApplication app)
        {
            try
            {
                app.CreateRibbonTab("AEC Tech");
            } 
            catch (Exception ex)
            {
                // ignored
            }

            var ribbonPanel = app.GetRibbonPanels("AEC Tech").FirstOrDefault(x => x.Name == "AEC Tech") ??
                              app.CreateRibbonPanel("AEC Tech", "AEC Tech");

            FirstButtonCommand.CreateButton(ribbonPanel);
            SecondButtonCommand.CreateButton(ribbonPanel);
            ThirdButtonCommand.CreateButton(ribbonPanel);

            ThirdButtonHandler = new ThirdButtonRequestHandler();
            ThirdButtonEvent = ExternalEvent.Create(ThirdButtonHandler);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
