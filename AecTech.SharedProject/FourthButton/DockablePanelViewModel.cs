using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AecTech.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OfficeOpenXml;

namespace AecTech.FourthButton
{
    public class DockablePanelViewModel : ViewModelBase
    {
        public RelayCommand LoadRequirements { get; set; }
        public RelayCommand ClearRequirements { get; set; }

        private ObservableCollection<RequirementWrapper> _requirements = new ObservableCollection<RequirementWrapper>();
        public ObservableCollection<RequirementWrapper> Requirements
        {
            get { return _requirements; }
            set { _requirements = value; RaisePropertyChanged(() => Requirements); }
        }

        public DockablePanelViewModel()
        {
            LoadRequirements = new RelayCommand(OnLoadRequirements);
            ClearRequirements = new RelayCommand(OnClearRequirements);

            Messenger.Default.Register<AddedElementsMessages>(this, OnAddedElementsMessages);
        }

        private void OnClearRequirements()
        {
            foreach (var rw in Requirements)
            {
                rw.PlacedCount = 0;
            }
        }

        private void OnLoadRequirements()
        {
            // (Konrad) Make sure we set the license to be non-commercial.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var requirements = new List<RequirementWrapper>();
            var filePath = DialogUtils.SelectFile();
            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.End.Row;

                // (Konrad) Starting at Row 2 to skip headers.
                for (var row = 2; row <= rowCount; row++)
                {
                    var fn = worksheet.Cells[row, 1].Value.ToString();
                    var ft = worksheet.Cells[row, 2].Value.ToString();
                    var count = int.Parse(worksheet.Cells[row, 3].Value.ToString());
                    var req = new RequirementWrapper(fn, ft, count);
                    requirements.Add(req);
                }
            }

            // (Konrad) In order to trigger collection changed and update UI
            // we want to add to the collection instead of just replacing it.
            Requirements.Clear();
            requirements.ForEach(x => Requirements.Add(x));
        }

        private void OnAddedElementsMessages(AddedElementsMessages obj)
        {
            foreach (var element in obj.Added)
            {
                var found = Requirements.FirstOrDefault(x =>
                    x.FamilyName == element.FamilyName && x.FamilyType == element.FamilyType);
                if (found == null)
                    continue;

                found.PlacedCount++;
            }
        }
    }
}
