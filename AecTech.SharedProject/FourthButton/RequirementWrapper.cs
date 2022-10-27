using System.ComponentModel;
using System.Windows.Media;

namespace AecTech.FourthButton
{
    public class RequirementWrapper : INotifyPropertyChanged
    {
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }

        private int _requiredCount;
        public int RequiredCount
        {
            get { return _requiredCount; }
            set { _requiredCount = value; RaisePropertyChanged(nameof(RequiredCount)); }
        }

        private int _placedCount;
        public int PlacedCount
        {
            get { return _placedCount; }
            set
            {
                _placedCount = value;

                if (value < RequiredCount)
                    PlacedColor = new SolidColorBrush(Colors.SandyBrown);
                else if (value == RequiredCount)
                    PlacedColor = new SolidColorBrush(Colors.LightGreen);
                else if (value > RequiredCount)
                    PlacedColor = new SolidColorBrush(Colors.SandyBrown);

                RaisePropertyChanged(nameof(PlacedCount));
            }
        }

        private SolidColorBrush _placedColor = new SolidColorBrush(Colors.SandyBrown);
        public SolidColorBrush PlacedColor
        {
            get { return _placedColor; }
            set { _placedColor = value; RaisePropertyChanged(nameof(PlacedColor)); }
        }

        public RequirementWrapper(string fn, string ft, int count)
        {
            FamilyName = fn;
            FamilyType = ft;
            RequiredCount = count;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
