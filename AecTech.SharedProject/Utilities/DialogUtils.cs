using System.Windows.Forms;

namespace AecTech.Utilities
{
    public static class DialogUtils
    {
        public static string SelectFile()
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = "*.xlsx",
                Multiselect = false
            };

            var result = dialog.ShowDialog();
            var filePath = dialog.FileName;

            return result != DialogResult.OK ? string.Empty : filePath;
        }
    }
}
