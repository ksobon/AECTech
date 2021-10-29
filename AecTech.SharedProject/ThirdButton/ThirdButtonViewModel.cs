using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using AecTech.SecondButton;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace AecTech.ThirdButton
{
    public class ThirdButtonViewModel : ViewModelBase
    {
        public ThirdButtonModel Model { get; set; }

        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> Delete { get; set; }

        private ObservableCollection<RoomWrapper> _spatialObjects;
        public ObservableCollection<RoomWrapper> SpatialObjects
        {
            get { return _spatialObjects; }
            set { _spatialObjects = value; RaisePropertyChanged(() => SpatialObjects); }
        }

        public ThirdButtonViewModel(ThirdButtonModel model)
        {
            Model = model;

            SpatialObjects = Model.CollectSpatialObjects();

            Close = new RelayCommand<Window>(OnClose);
            Delete = new RelayCommand<Window>(OnDelete);

            Messenger.Default.Register<SpatialObjectDeletedMessage>(this, OnSpatialObjectDeletedMessage);
        }

        private void OnSpatialObjectDeletedMessage(SpatialObjectDeletedMessage obj)
        {
            var spatialObjects = SpatialObjects.Where(x => !obj.Ids.Contains(x.Id));
            SpatialObjects = new ObservableCollection<RoomWrapper>(spatialObjects);
        }

        private void OnDelete(Window win)
        {
            var selected = SpatialObjects.Where(x => x.IsSelected).ToList();
            Model.Delete(selected);
        }

        private static void OnClose(Window win)
        {
            win.Close();
        }
    }
}
