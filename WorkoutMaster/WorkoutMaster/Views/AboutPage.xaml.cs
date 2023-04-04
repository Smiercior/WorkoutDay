using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
            bool copy = false;
            if(copy)
            {
                try
                {
                    string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "workout.db3");
                    string copypath = Path.Combine("/storage/emulated/0/Android/data/com.companyname.workoutmaster", Path.GetFileName(filepath));
                    File.Copy(filepath, copypath);
                    Console.WriteLine("Success !!!!!!!!!!!!!!!!!!!!!!!!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd !!!!!!!!!!!: " + ex.Message);
                }
            }
            
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
    }
}