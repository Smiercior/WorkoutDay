using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using WorkoutMaster.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WorkoutMaster;
using WorkoutMaster.ViewModels;

namespace WorkoutMaster.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        async protected override void OnParentSet()
        {
            base.OnParentSet();
            await Shell.Current.GoToAsync($"////{nameof(ItemsPage)}");
        }

        // Get all workout days on page appearing
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Clear all messages
            messageLabel.Text = "";

            try
            {
                List<WorkoutDay> workoutDays = await App.DataBase.GetWorkoutDaysBasic();
                workoutDays.Sort((x,y) => DateTime.Compare(y.Date,x.Date));
                collectionView.ItemsSource = workoutDays;
            }
            catch (Exception ex)
            {
                await Shell.Current.GoToAsync($"////{nameof(ItemsPage)}");
            }

           //await App.DataBase.MockData();

        }

        // User tap certain workout day
        async void OnDisplay(object sender, EventArgs e)
        {
            //Console.WriteLine(((Button)sender).CommandParameter);
            await Shell.Current.GoToAsync($"////{nameof(ItemsPage)}?{nameof(ItemsPage.ID)}={((Button)sender).CommandParameter}");
        }

        // User want to create current day or if it exists select that day
        async void OnCreateNewDay(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"////{nameof(ItemsPage)}");
        }

        // User wanto to make a copy of database
        async void OnMakeDbCopy(object sender, EventArgs e)
        {
            try
            {
                string dbPath = App.DBPath;
                string dbCopyPath = App.DBPath.Replace(".db3", $"{DateTime.Now.ToString("ddMMyyyy")}.db3");
                await Task.Run(() =>
                {
                    File.Copy(dbPath, dbCopyPath);
                });

                messageLabel.TextColor = Color.Green;
                messageLabel.Text = "Succeed to make a copy";
            }
            catch (Exception ex)
            {
                messageLabel.TextColor = Color.Red;
                if (ex.Message.Contains("File already exists."))
                {
                    messageLabel.Text = $"Failed to a make copy, because: File with today's data already exists";
                }
                else
                {
                    messageLabel.Text = $"Failed to make copy, because: Unknown error";
                }
            }
        }

        async void OnReadDb(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectDbPage());
        }
    }
}