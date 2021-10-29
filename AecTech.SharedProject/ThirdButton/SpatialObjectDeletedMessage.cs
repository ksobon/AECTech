using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace AecTech.ThirdButton
{
    public class SpatialObjectDeletedMessage
    {
        public List<ElementId> Ids { get; set; }

        public SpatialObjectDeletedMessage(List<ElementId> ids)
        {
            Ids = ids;
        }
    }
}
