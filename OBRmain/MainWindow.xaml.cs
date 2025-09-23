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
using OBRmain.Logic;
using Path = System.Windows.Shapes.Path;

namespace OBRmain;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SelectFolder_Click(object sender, RoutedEventArgs e)
    {
        FolderOperations.ChooseFolderManually();

        ChosenFolderPathLabel.Content = FolderOperations.SelectedFolderPath;
        ChosenFolderPathLabel.ToolTip = FolderOperations.SelectedFolderPath;
        ToolTipService.SetIsEnabled(ChosenFolderPathLabel, true);
    }

    private void SelectImage_Click(object sender, RoutedEventArgs e)
    {
        ImageOperations.ChooseImageManually();

        ChosenImagePathLabel.Content = ImageOperations.SelectedImagePath;
        ChosenImagePathLabel.ToolTip = ImageOperations.SelectedImagePath;
        ToolTipService.SetIsEnabled(ChosenImagePathLabel, true);
    }

    private void FolderDrop(object sender, DragEventArgs e)
    {
        FolderOperations.DragAndDropFolder(e);

        ChosenFolderPathLabel.Content = FolderOperations.SelectedFolderPath;
        ChosenFolderPathLabel.ToolTip = FolderOperations.SelectedFolderPath;
        ToolTipService.SetIsEnabled(ChosenFolderPathLabel, true);
    }

    private void FileDrop(object sender, DragEventArgs e)
    {
        ImageOperations.DragAndDropImage(e);

        ChosenImagePathLabel.Content = ImageOperations.SelectedImagePath;
        ChosenImagePathLabel.ToolTip = ImageOperations.SelectedImagePath;
        ToolTipService.SetIsEnabled(ChosenImagePathLabel, true);
    }


    private async void StartReplacement_Click(object sender, RoutedEventArgs e)
    {
        var progress = new Progress<int>(p => ReplacingProgressBar.Value = p);

        ReplacingProgressBar.Visibility = Visibility.Visible;
        List<string> replacedFiles = await Operations.Replacement(progress);
        ReplacingProgressBar.Visibility = Visibility.Collapsed;

        ChangedFilesListBox.ItemsSource = replacedFiles;
    }

    public static void ShowException(string text, string message)
    {
        MessageBox.Show(Application.Current.MainWindow,
            $"{text} {message}",
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    public static void ShowMessageBox(string text, string caption, MessageBoxImage icon = MessageBoxImage.Error,
        MessageBoxButton button = MessageBoxButton.OK)
    {
        MessageBox.Show(Application.Current.MainWindow, $"{text}",
            $"{caption}", button,
            icon);
    }
}