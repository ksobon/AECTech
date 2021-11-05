using System;
using System.Collections.Generic;
using System.Linq;
using AecTech.SecondButton;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;

namespace AecTech.ThirdButton
{
    /// <summary>
    /// We can use Enums to list out any actions that we want to be handled by the ExternalEventHandler.
    /// </summary>
    public enum RequestId
    {
        None,
        Delete,
        Select
    }

    public class ThirdButtonRequestHandler : IExternalEventHandler
    {
        // (Konrad) Type of request that we want to process here. We only have Delete/Select for now, but this can grow.
        public RequestId Request { get; set; }

        // (Konrad) When such request is made, we sometimes want to pass something to it. We can just put it into a generic
        // object type property, so that it's reusable for different kinds of actions. 
        public object Arg1 { get; set; }
        
        public void Execute(UIApplication app)
        {
            try
            {
                switch (Request)
                {
                    case RequestId.None:
                        return;
                    case RequestId.Delete:
                        Delete(app);
                        break;
                    case RequestId.Select:
                        Select(app);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                // ignored
            }
        }

        private void Select(UIApplication app)
        {
            if (!(Arg1 is List<RoomWrapper> selected))
                return;

            var ids = selected.Select(x => x.Id).ToList();
            app.ActiveUIDocument.Selection.SetElementIds(ids);
        }

        private void Delete(UIApplication app)
        {
            if (!(Arg1 is List<RoomWrapper> selected))
                return;

            var doc = app.ActiveUIDocument.Document;
            var ids = selected.Select(x => x.Id).ToList();
            
            using (var trans = new Transaction(doc, "Delete Room/Space"))
            {
                trans.Start();
                doc.Delete(ids);
                trans.Commit();
            }

            // (Konrad) When our delete request is finished we want to notify the UI that these Rooms were deleted.
            // Keep in mind that UI runs on a different thread, so it doesn't know when this would be done. 
            Messenger.Default.Send(new SpatialObjectDeletedMessage(ids));
        }

        public string GetName()
        {
            return "Third Button Request Handler";
        }
    }
}
