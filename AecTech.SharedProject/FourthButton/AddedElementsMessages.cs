using System.Collections.Generic;

namespace AecTech.FourthButton
{
    public class AddedElementsMessages
    {
        public List<ElementWrapper> Added { get; set; }

        public AddedElementsMessages(List<ElementWrapper> elements)
        {
            Added = elements;
        }
    }
}
