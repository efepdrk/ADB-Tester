using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using ADBTester.services;


namespace ADBTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    
        private bool isRecording = false;
      
        
        public MainWindow()
        {
            InitializeComponent();

        }

     
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("A button was pressed");
            Button button = sender as Button;
            

            Debug.WriteLine($"{button.Name}");

            if (!string.IsNullOrEmpty(button.Name))
            {

                
                if(!string.IsNullOrEmpty(DeviceIDTextBox.Text))
                {
                    if (isRecording)
                    {
                        OutputBox.Items.Add(button.Name);
                    }

                    if(checkbox_ImmediateCommand.IsChecked == true)
                    {
                        await AdbService.ExecuteCommand($"shell input keyevent {button.Name}");
                    }
                  
                }
                else
                {
                    MessageBox.Show("Cihaz ID boş!","Hata",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }


        }

        private async Task Playback()
        {
            if (OutputBox.Items.IsEmpty)
            {
                MessageBox.Show("Oynatılacak komut yok!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(DeviceIDTextBox.Text))
            {
                MessageBox.Show("Cihaz ID boş kalmamalı!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var commands = OutputBox.Items.Cast<string>().ToList();

            for (int i = 0; i < commands.Count; i++)
            {
               
                OutputBox.SelectedIndex = i;
                OutputBox.ScrollIntoView(OutputBox.SelectedItem);

               
                Debug.WriteLine(commands[i]);
                await AdbService.ExecuteCommand($"shell input keyevent {commands[i]}", DeviceIDTextBox.Text);

               
            }

            // Clear the selection after playback is complete
            OutputBox.SelectedIndex = -1;

        }

        //disable ui buttons during playback
        private void SetPlaybackUiState(bool isEnabled)
        {
            btn_StartRecording.IsEnabled = isEnabled;
            btn_PlayRecording.IsEnabled = isEnabled;
            btn_SaveOutput.IsEnabled = isEnabled;
            btn_LoadSave.IsEnabled = isEnabled;
            btn_ClearOutput.IsEnabled = isEnabled;
            btn_DeleteCommand.IsEnabled = isEnabled;

        }


        private void btn_StartRecording_Click(object sender, RoutedEventArgs e)
        {
            if (isRecording)
            {
                btn_StartRecording.Content = "Giriş kaydını başlat";
                isRecording = false;
            }
            else
            {
                btn_StartRecording.Content = "Giriş kaydını durdur";
                isRecording = true;
            }

        }

        private async void btn_RefreshDevices_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                deviceListBox.Text = "";


                var result = await AdbService.ExecuteCommand("devices");


                deviceListBox.Text = result;

                Debug.WriteLine($"ADB Output:\n{result}");
            }
            catch (Exception ex)
            {
                deviceListBox.Text = $"Error: {ex.Message}";
                Debug.WriteLine(ex.ToString());
            }
        }



        private void btn_SaveOutput_Click(object sender, RoutedEventArgs e)
        {
            var items = OutputBox.Items.Cast<string>().ToList();
            FileService.SaveOutputToFile(items);
        }

        private void btn_LoadSave_Click(object sender, RoutedEventArgs e)
        {
            var loadedItems = FileService.LoadOutputFromFile();

            // Clear the ListBox and add the loaded items
            OutputBox.Items.Clear();
            foreach (var item in loadedItems)
            {
                OutputBox.Items.Add(item);
            }
        }



        private void btn_ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            OutputBox.Items.Clear();
        }

        private async void btn_PlayRecording_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{RecordingRepeatTextBox.Text}");
            string repeatCount = string.IsNullOrEmpty(RecordingRepeatTextBox.Text) ? "0" : RecordingRepeatTextBox.Text;

            if (!int.TryParse(repeatCount, out int repeat) || repeat <= 0)
            {
                MessageBox.Show("Geçersiz tekrar sayısı! Lütfen pozitif bir sayı girin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

                
            MessageBoxResult result = MessageBox.Show($"Kayıt {repeatCount} kere tekrarlanıcak", "Onay", MessageBoxButton.OKCancel, MessageBoxImage.Information);
           
            if (result == MessageBoxResult.OK)
            {
                SetPlaybackUiState(false);
                for(int i = 0; i < repeat;i++)
                {
                    await Playback();

                }
                SetPlaybackUiState(true);
            }
           else
            {
                return;
            }
            
        }

        private void btn_DeleteCommand_Click(object sender, RoutedEventArgs e)
        {


        
            if (OutputBox.SelectedItems.Count > 0)
            {
                
                for (int i = OutputBox.SelectedItems.Count - 1; i >= 0; i--)
                {
                    OutputBox.Items.Remove(OutputBox.SelectedItems[i]);
                }
            }

          
            OutputBox.SelectedIndex = -1;
        }

        private void btn_WirelessADB_Click(object sender, RoutedEventArgs e)
        {
            WirelessADBWindow wirelessADBWindow = new WirelessADBWindow();
            wirelessADBWindow.Show();
        }
    }



    
    
}