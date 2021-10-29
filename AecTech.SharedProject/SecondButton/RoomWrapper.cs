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
