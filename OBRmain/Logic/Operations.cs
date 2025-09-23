using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace OBRmain.Logic;

public class Operations
{
    public static async Task<List<string>> Replacement(IProgress<int>? progress = null)
    {
        try
        {
            if (!File.Exists(ImageOperations.SelectedImagePath) ||
                !Directory.Exists(FolderOperations.SelectedFolderPath))
            {
                MainWindow.ShowMessageBox("File or folder not exists.", "Error");
                return new List<string>();
            }

            List<string> replacedFiles = new List<string>();

            var allImageFiles = await Task.Run(() => Directory.GetDirectories(FolderOperations.SelectedFolderPath)
                .SelectMany(folder => Directory.GetFiles(folder, "*.*")
                    .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)))
                .ToList());

            int total = allImageFiles.Count;
            int current = 0;

            foreach (var folder in Directory.GetDirectories(FolderOperations.SelectedFolderPath))
            {
                var imageFiles = await Task.Run(() => Directory.GetFiles(folder, "*.*")
                    .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg"))
                    .ToList());

                if (imageFiles.Count > 0)
                {
                    foreach (var imageFile in imageFiles)
                    {
                        string newFileName = Path.GetFileName(imageFile);
                        string newFilePath = Path.Combine(folder, newFileName);

                        try
                        {
                            await Task.Run(() => File.Copy(ImageOperations.SelectedImagePath, newFilePath, true));
                            replacedFiles.Add(newFilePath);
                        }
                        catch (Exception exception)
                        {
                            MainWindow.ShowException($"Issue:  ", exception.Message);
                            return new List<string>();
                        }

                        current++;
                        int percent = (int)((double)current / total * 100);
                        progress?.Report(percent);
                    }
                }
            }

            await Application.Current.Dispatcher.InvokeAsync(() =>
                MainWindow.ShowMessageBox($"Successfully changed {replacedFiles.Count} files.", "Done",
                    MessageBoxImage.Information));
            return replacedFiles;
        }
        catch (Exception exception)
        {
            MainWindow.ShowException($"Error in replacing: ", exception.Message);
            return new List<string>();
        }
    }
}