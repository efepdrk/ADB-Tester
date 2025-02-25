using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADBTester
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WirelessADBWindow : Window
    {
        bool Paired = false;
        public WirelessADBWindow()
        {
            InitializeComponent();
        }

        private async void btn_Connect_Click(object sender, RoutedEventArgs e)
        {
            string connectIP = connectIP_textbox.Text;
            
            try
            {
                OutputTextBox.Clear();
                var result = await AdbService.ExecuteCommand($"connect {connectIP}");
                OutputTextBox.Text = result;

            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Error: {ex.Message}";
                Debug.WriteLine(ex.ToString());
            }

        }

        private async void btn_Pair_Click(object sender, RoutedEventArgs e)
        {
            string pairIP = pairIP_textbox.Text;

            try
            {
                OutputTextBox.Clear();
                var result = await AdbService.ExecuteCommand($"pair {pairIP}");
                OutputTextBox.Text = result;

            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Error: {ex.Message}";
                Debug.WriteLine(ex.ToString());
            }

        }

        private void checkbox_Android14_Checked(object sender, RoutedEventArgs e)
        {
            btn_Pair.Visibility = Visibility.Visible;
            pairIP_textbox.Visibility = Visibility.Visible;

        }

        private void checkbox_Android14_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_Pair.Visibility = Visibility.Hidden;
            pairIP_textbox.Visibility = Visibility.Hidden;
        }
    }
}
