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
using Microsoft.WindowsAPICodePack.Dialogs;

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
           
            MainViewModel vm = this.DataContext as MainViewModel;
            await vm.ScanFile();
         //   MessageBox.Show("Done!");
           

        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
           
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ConvertCarts();
         //   MessageBox.Show("Done!");
           
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.DownloadCartImages();
         //   MessageBox.Show("Done!");
            
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
           
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ClearGames();
          //  MessageBox.Show("Done!");
           
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

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.CurrentTab = "games";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.CurrentTab = "config";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.CurrentTab = "log";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.CurrentTab = "systems";
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;

            var dlg = new CommonOpenFileDialog();
            dlg.Title = "My Title";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = vm.GameScannerPath;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = vm.GameScannerPath;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folder = dlg.FileName;
                
                vm.GameScannerPath = folder;
                // Do something with selected folder string
            }
        }
    }
}
