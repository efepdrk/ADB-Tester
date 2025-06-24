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
                
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = ".txt",
                    FileName = "ADB_Output.txt",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

              
                if (saveFileDialog.ShowDialog() == true)
                {
                  
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
        
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = ".txt",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

  
                if (openFileDialog.ShowDialog() == true)
                {
           
                    var lines = File.ReadAllLines(openFileDialog.FileName);

                    MessageBox.Show("Kayıt başarıyla yüklendi!", "Load Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                    return lines;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt yüklenirken hata: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return Enumerable.Empty<string>();
        }
    }
}