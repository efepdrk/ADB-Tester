using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace ADBTester.services
{
    public static class FileService
    {

        public static void SaveOutputToFile(IEnumerable<string> items)
        {
            try
            {
                // Create a SaveFileDialog
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = ".txt",
                    FileName = "ADB_Output.txt",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                // Show the dialog and check if the user clicked OK
                if (saveFileDialog.ShowDialog() == true)
                {
                    // Write items to the selected file
                    File.WriteAllLines(saveFileDialog.FileName, items);

                    MessageBox.Show("Kayıt başarılı!", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt ederken hata: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public static IEnumerable<string> LoadOutputFromFile()
        {
            try
            {
                // Create an OpenFileDialog
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = ".txt",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                // Show the dialog and check if the user clicked OK
                if (openFileDialog.ShowDialog() == true)
                {
                    // Read all lines from the selected file
                    var lines = File.ReadAllLines(openFileDialog.FileName);

                    MessageBox.Show("Kayıt başarıyla yüklendi!", "Load Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Return the loaded lines
                    return lines;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt yüklenirken hata: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Return an empty list if loading fails
            return Enumerable.Empty<string>();
        }
    }
}