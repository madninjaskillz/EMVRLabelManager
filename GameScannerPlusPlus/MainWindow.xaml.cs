using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameScannerplusplus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            MainViewModel vm = this.DataContext as MainViewModel;
            await vm.ScanFile();
         //   MessageBox.Show("Done!");
            this.IsEnabled = true;

        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ConvertCarts();
         //   MessageBox.Show("Done!");
            this.IsEnabled = true;
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.DownloadCartImages();
         //   MessageBox.Show("Done!");
            this.IsEnabled = true;
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ClearGames();
          //  MessageBox.Show("Done!");
            this.IsEnabled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.SaveConfig();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ScrollToEnd();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;

            vm.SetSelectedSystem(((ListView) sender).SelectedItem);
        }

        private void SaveConsoleConfigClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.SaveConsoleConfig();
        }
    }
}
