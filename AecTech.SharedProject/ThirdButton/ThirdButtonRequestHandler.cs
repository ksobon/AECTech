using System;
using System.Collections.Generic;
using System.Linq;
using AecTech.SecondButton;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight.Messaging;

namespace AecTech.ThirdButton
{
    public enum RequestId
    {
        None,
        Delete,
    }

    public class ThirdButtonRequestHandler : IExternalEventHandler
    {
        public RequestId Request { get; set; }
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                throw;
            }
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

            Messenger.Default.Send(new SpatialObjectDeletedMessage(ids));
        }

        public string GetName()
        {
            return "Third Button Request Handler";
        }
    }
}
