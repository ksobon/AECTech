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
        public RelayCommand Delete { get; set; }
        public RelayCommand Select { get; set; }

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
            Delete = new RelayCommand(OnDelete);
            Select = new RelayCommand(OnSelect);

            // (Konrad) In order for our UI to be updated when Rooms are deleted (we want to remove them from the table in the UI)
            // we have to listen for that action to be completed. Remember that the ExternalEventHandler runs on the main thread
            // while this UI is now running on a UI thread. They technically don't know about each others actions. Messaging/Events 
            // are what allows them to communicate between each other. 
            Messenger.Default.Register<SpatialObjectDeletedMessage>(this, OnSpatialObjectDeletedMessage);
        }

        private void OnSelect()
        {
            var selected = SpatialObjects.Where(x => x.IsSelected).ToList();
            Model.Select(selected);
        }

        /// <summary>
        /// When ExternalEventHandler finishes deleting Rooms, it sends a message that contains a list of Ids of Rooms that were
        /// deleted. We can then update the SpatialObjects collection to make our UI not display just deleted Rooms anymore.
        /// </summary>
        /// <param name="obj"></param>
        private void OnSpatialObjectDeletedMessage(SpatialObjectDeletedMessage obj)
        {
            var spatialObjects = SpatialObjects.Where(x => !obj.Ids.Contains(x.Id));
            SpatialObjects = new ObservableCollection<RoomWrapper>(spatialObjects);
        }

        private void OnDelete()
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
