using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using Path = System.Windows.Shapes.Path;

namespace OBRmain;

public partial class MainWindow
{
    private string _selectedImagePath;
    private string _selectedFolderPath;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SelectFolder_Click(object sender, RoutedEventArgs e)
    {
        OpenFolderDialog folderDialog = new OpenFolderDialog();

        if (folderDialog.ShowDialog() == true)
        {
            _selectedFolderPath = folderDialog.FolderName;
            ChosenFolderPathLabel.Content = _selectedFolderPath;
            ChosenFolderPathLabel.ToolTip = _selectedFolderPath;
            ToolTipService.SetIsEnabled(ChosenFolderPathLabel, true);
        }
    }

    private void SelectImage_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            _selectedImagePath = openFileDialog.FileName;
            ChosenImagePathLabel.Content = _selectedImagePath;
            ChosenImagePathLabel.ToolTip = _selectedImagePath;
            ToolTipService.SetIsEnabled(ChosenImagePathLabel, true);
        }
    }

    private void StartReplacement_Click(object sender, RoutedEventArgs e)
    {
        if (!File.Exists(_selectedImagePath) || !Directory.Exists(_selectedFolderPath))
        {
            MessageBox.Show(Application.Current.MainWindow, "File or folder not exists.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        List<string> replacedFiles = new List<string>();

        foreach (var folder in Directory.GetDirectories(_selectedFolderPath))
        {
            var imageFiles = Directory.GetFiles(folder, "*.*")
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png"))
                .ToList();

            if (imageFiles.Count > 0)
            {
                foreach (var imageFile in imageFiles)
                {
                    string newFileName = System.IO.Path.GetFileName(imageFile);
                    string newFilePath = System.IO.Path.Combine(folder, newFileName);

                    try
                    {
                        File.Copy(_selectedImagePath, newFilePath, true);
                        replacedFiles.Add(newFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Application.Current.MainWindow, $"Issue: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
        }

        ChangedFilesListBox.ItemsSource = replacedFiles;
        MessageBox.Show(Application.Current.MainWindow, $"Successfully changed {replacedFiles.Count} files.", "Done",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void FileDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0 &&
                (files[0].EndsWith(".jpg") || files[0].EndsWith(".jpeg") || files[0].EndsWith(".png")))
            {
                _selectedImagePath = files[0];
                ChosenImagePathLabel.Content = _selectedImagePath;
                ChosenImagePathLabel.ToolTip = _selectedImagePath;
                ToolTipService.SetIsEnabled(ChosenImagePathLabel, true);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "The dropped item is not a .png .jpg .jpeg file.",
                    "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

    private void FolderDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string path = files[0];

                if (Directory.Exists(path))
                {
                    _selectedFolderPath = path;
                    ChosenFolderPathLabel.Content = _selectedFolderPath;
                    ChosenFolderPathLabel.ToolTip = _selectedFolderPath;
                    ToolTipService.SetIsEnabled(ChosenFolderPathLabel, true);
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "The dropped item is not a folder.", "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}