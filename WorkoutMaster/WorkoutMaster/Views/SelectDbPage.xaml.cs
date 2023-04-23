using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutMaster.Views
{
    public partial class SelectDbPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public SelectDbPage()
        {
            InitializeComponent();

            string[] dbNames = Directory.GetFiles(App.DBFolder);
            ObservableCollection<string> observableDbNames = new ObservableCollection<string>(dbNames.Select(fileName => fileName.Replace(App.DBFolder + "/","")).ToArray());
            MyListView.ItemsSource = observableDbNames;
        }

        // When user tapped on certain db name
        async void DbSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            // Replace the current database with the database that was selected by the user
            try
            {
                await Task.Run(() =>
                {
                    File.Copy(Path.Combine(App.DBFolder, e.Item.ToString()), App.DBPath, true);
                });
                await DisplayAlert($"Selected {e.Item}", "Now all your progress will be saved to that file", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert($"Error",$"Something went wrong when selecting your file. {ex.Message}", "OK");
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}
