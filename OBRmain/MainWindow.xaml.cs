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
        ChangedFilesListBox.PreviewMouseWheel += ListBox_PreviewMouseWheel;
    }

    private void SelectFolder_Click(object sender, RoutedEventArgs e)
    {
        FolderOperations.ChooseFolderManually();

        ChosenFolderPathLabel.Content = FolderOperations.SelectedFolderPath;
        ChosenFolderPathLabel.ToolTip = FolderOperations.SelectedFolderPath;
        ToolTipService.SetIsEnabled(ChosenFolderPathLabel, true);
        ChosenFolderPathLabel.Visibility = Visibility.Visible;
    }

    private void SelectImage_Click(object sender, RoutedEventArgs e)
    {
        ImageOperations.ChooseImageManually();

        ChosenImagePathLabel.Content = ImageOperations.SelectedImagePath;
        ChosenImagePathLabel.ToolTip = ImageOperations.SelectedImagePath;
        ToolTipService.SetIsEnabled(ChosenImagePathLabel, true);
        ChosenImagePathLabel.Visibility = Visibility.Visible;
    }

    private void FolderDrop(object sender, DragEventArgs e)
    {
        FolderOperations.DragAndDropFolder(e);


        ChosenFolderPathLabel.Content = FolderOperations.SelectedFolderPath;
        ChosenFolderPathLabel.ToolTip = FolderOperations.SelectedFolderPath;
        ToolTipService.SetIsEnabled(ChosenFolderPathLabel, true);
        ChosenFolderPathLabel.Visibility = Visibility.Visible;
    }

    private void FileDrop(object sender, DragEventArgs e)
    {
        ImageOperations.DragAndDropImage(e);

        ChosenImagePathLabel.Content = ImageOperations.SelectedImagePath;
        ChosenImagePathLabel.ToolTip = ImageOperations.SelectedImagePath;
        ToolTipService.SetIsEnabled(ChosenImagePathLabel, true);
        ChosenImagePathLabel.Visibility = Visibility.Visible;
    }


    private async void StartReplacement_Click(object sender, RoutedEventArgs e)
    {
        if (!FolderOperations.SelectedFolderPath.Contains("osu!\\Songs", StringComparison.OrdinalIgnoreCase))
        {
            var wrongPathResult = MessageBox.Show(
                "The selected path doesn't contain the \"osu!\\Songs\" folder. This might mean you selected the wrong directory. Are you sure you want to continue with the replacement?",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (wrongPathResult != MessageBoxResult.Yes) return;
        }

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

    private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var listBox = sender as ListBox;
        var scrollViewer = GetScrollViewer(listBox);

        if (scrollViewer == null) return;

        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta / 60);
            e.Handled = true;
        }
        else
        {
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta / 60);
            e.Handled = true;
        }
    }

    private ScrollViewer? GetScrollViewer(DependencyObject dep)
    {
        if (dep is ScrollViewer) return dep as ScrollViewer;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
        {
            var child = VisualTreeHelper.GetChild(dep, i);
            var result = GetScrollViewer(child);
            if (result != null) return result;
        }

        return null;
    }
}