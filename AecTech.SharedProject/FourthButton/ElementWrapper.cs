using Autodesk.Revit.DB;

namespace AecTech.FourthButton
{
    public class ElementWrapper
    {
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }

        public ElementWrapper(Element e)
        {
            var doc = e.Document;
            if (!(doc.GetElement(e.GetTypeId()) is FamilySymbol type))
                return;

            FamilyName = type.FamilyName;
            FamilyType = type.Name;
        }
    }
}
