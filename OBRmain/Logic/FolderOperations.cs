using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace OBRmain.Logic;

public class FolderOperations
{
    public static string SelectedFolderPath { get; set; }

    public static void ChooseFolderManually()
    {
        OpenFolderDialog folderDialog = new OpenFolderDialog();

        try
        {
            if (folderDialog.ShowDialog() == true)
            {
                SelectedFolderPath = folderDialog.FolderName;
            }
        }
        catch (Exception exception)
        {
            MainWindow.ShowException("folder", exception.Message);
        }
    }

    public static void DragAndDropFolder(DragEventArgs e)
    {
        try
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string path = files[0];

                    if (Directory.Exists(path))
                    {
                        SelectedFolderPath = path;
                    }
                    else
                    {
                        MainWindow.ShowMessageBox("The dropped item is not a folder.", "Error");
                    }
                }
            }
        }
        catch (Exception exception)
        {
            MainWindow.ShowException($"Error selecting folder: ", exception.Message);
        }
    }
}