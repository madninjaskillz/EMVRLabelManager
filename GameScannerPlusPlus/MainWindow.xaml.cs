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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
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

            vm.SetSelectedSystem(((ListView)sender).SelectedItem);
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

        private void ShowCartImage(object sender, RoutedEventArgs e)
        {
            TitleModel tm = (TitleModel)((Button)sender).DataContext;
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ShowModalImage = true;
            vm.ModalImagePath = tm.CartImagePath;
        }
        private void ShowLabelImage(object sender, RoutedEventArgs e)
        {
            TitleModel tm = (TitleModel)((Button)sender).DataContext;
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ShowModalImage = true;
            vm.ModalImagePath = tm.LabelImagePath;
        }
        private void ShowCartUrl(object sender, RoutedEventArgs e)
        {
            TitleModel tm = (TitleModel)((Button)sender).DataContext;
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ShowModalImage = true;
            vm.ModalImagePath = tm.CartUrl;
        }

        private void CloseCart(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.ShowModalImage = false;
        }


        private static DispatcherTimer myClickWaitTimer;
            

        private void SetConsole(object sender, MouseButtonEventArgs e)
        {
            // Stop the timer from ticking.
            myClickWaitTimer.Stop();
            e.Handled = true;

            Button button = sender as Button;
            MainViewModel.System system = button.DataContext as MainViewModel.System;

            MainViewModel vm = this.DataContext as MainViewModel;

            if (vm.FoundSystems.Count(x => x.IsVisible) == 1 && vm.FoundSystems.First(x => x.IsVisible).Name == system.Name)
            {
                foreach (var x in vm.FoundSystems)
                {
                    x.IsVisible = true;
                }
            }
            else
            {
                foreach (var x in vm.FoundSystems)
                {
                    x.IsVisible = x.Name == system.Name;
                }
            }

            vm.UpdateVisibleTitles();
            
        }

        private void ToggleConsoleVisible(object sender, RoutedEventArgs e)
        {
            myClickWaitTimer?.Stop();

            myClickWaitTimer= new DispatcherTimer(
                TimeSpan.FromMilliseconds(200),
                DispatcherPriority.Background,
                (y,u)=>mouseWaitTimer_Tick(sender,e),
                Dispatcher.CurrentDispatcher);

            myClickWaitTimer.Start();
        }

        private void mouseWaitTimer_Tick(object sender, EventArgs e)
        {
            myClickWaitTimer.Stop();

            // Handle Single Click Actions
            Button button = sender as Button;
            MainViewModel.System system = button.DataContext as MainViewModel.System;

            MainViewModel vm = this.DataContext as MainViewModel;

            vm.FoundSystems.First(x => x.Name == system.Name).IsVisible = !vm.FoundSystems.First(x => x.Name == system.Name).IsVisible;
            vm.UpdateVisibleTitles();
        }
    }
}
