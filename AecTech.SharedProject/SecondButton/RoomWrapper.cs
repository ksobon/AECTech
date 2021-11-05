using System.ComponentModel;
using Autodesk.Revit.DB;

namespace AecTech.SecondButton
{
    public class RoomWrapper : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double Area { get; set; }
        public ElementId Id { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(nameof(IsSelected)); }
        }

        /// <summary>
        /// We don't want to pass the actual Revit object around (SpatialElement). It has a lot of
        /// properties and methods associated with it that are of no interest to us. For the UI that
        /// we are building we only need to know its Name/Area/Id. This wrapper also allows us to
        /// establish our own relationships with the UI like passing the Row's IsSelected property
        /// into this wrapper's IsSelected property. 
        /// </summary>
        /// <param name="se"></param>
        public RoomWrapper(SpatialElement se)
        {
            Name = se.Name;
            Area = se.Area;
            Id = se.Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
