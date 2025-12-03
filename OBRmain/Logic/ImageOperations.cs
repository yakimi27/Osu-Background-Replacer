using Microsoft.Win32;
using System.Windows;

namespace OBRmain.Logic;

public class ImageOperations
{
    public static string SelectedImagePath { get; set; }

    public static void ChooseImageManually()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
        };
        try
        {
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagePath = openFileDialog.FileName;
            }
        }
        catch (Exception exception)
        {
            MainWindow.ShowException("image", exception.Message);
        }
    }

    public static void DragAndDropImage(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 &&
                    (files[0].EndsWith(".jpg") || files[0].EndsWith(".jpeg") || files[0].EndsWith(".png")))
                {
                    SelectedImagePath = files[0];
                }
                else
                {
                    MainWindow.ShowMessageBox("The dropped item is not a .png .jpg .jpeg file.", "Error");
                }
            }
        }
        catch (Exception exception)
        {
            MainWindow.ShowException($"Error selecting image: ", exception.Message);
        }
    }
}