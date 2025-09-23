using System.IO;
using System.Windows;

namespace OBRmain.Logic;

public class Operations
{
    public static List<string> Replacement()
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

            foreach (var folder in Directory.GetDirectories(FolderOperations.SelectedFolderPath))
            {
                var imageFiles = Directory.GetFiles(folder, "*.*")
                    .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg"))
                    .ToList();

                if (imageFiles.Count > 0)
                {
                    foreach (var imageFile in imageFiles)
                    {
                        string newFileName = System.IO.Path.GetFileName(imageFile);
                        string newFilePath = System.IO.Path.Combine(folder, newFileName);

                        try
                        {
                            File.Copy(ImageOperations.SelectedImagePath, newFilePath, true);
                            replacedFiles.Add(newFilePath);
                        }
                        catch (Exception exception)
                        {
                            MainWindow.ShowException($"Issue:  ", exception.Message);
                            return new List<string>();
                        }
                    }
                }
            }

            MainWindow.ShowMessageBox($"Successfully changed {replacedFiles.Count} files.", "Done",
                MessageBoxImage.Information);
            return replacedFiles;
        }
        catch (Exception exception)
        {
            MainWindow.ShowException($"Error in replacing: ", exception.Message);
            return new List<string>();
        }
    }
}