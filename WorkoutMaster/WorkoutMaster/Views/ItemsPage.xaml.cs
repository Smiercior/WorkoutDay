using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutMaster.Models;
using WorkoutMaster.ViewModels;
using WorkoutMaster.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutMaster.Views
{
    [QueryProperty(nameof(ID), nameof(ID))]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        public static WorkoutDay basicCurrentDay;
        public static WorkoutDay currentDay;
        private string id;

        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemsViewModel(Navigation);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if(ID != null) _viewModel.OnAppearing(Int32.Parse(ID));
            else _viewModel.OnAppearing(-1);

            // Null ID means that user want to create new workout day or go to actual workout day
            // Not Null ID means that user tap certain workout day


            // Get current workout day or create new one
            if (id == null)
            {
                // Try to get actual workout date
                basicCurrentDay = ((await App.DataBase.GetWorkoutDaysBasic()).Where(x => x.Date == DateTime.Now.Date)).FirstOrDefault<WorkoutDay>();

                // If current workout day doesn't exists
                if (basicCurrentDay == null)
                {
                    var answer = await DisplayAlert("Question?", "Would you like to create new workout day?", "Yes", "No");

                    // Create empty workout day
                    if (answer)
                    {
                        await App.DataBase.SaveWorkoutDay(new WorkoutDay { Date = DateTime.Now.Date });
                        currentDay = await App.DataBase.GetWorkoutDayByDate(DateTime.Now.Date);

                        //currentDay = ((await App.DataBase.GetWorkoutDays()).Where(x => x.Date == DateTime.Now.Date)).FirstOrDefault<WorkoutDay>();
                        ID = currentDay.Id.ToString();
                        this.OnAppearing();
                    }

                    // Go back to Home page
                    else await Shell.Current.GoToAsync($"////{nameof(AboutPage)}");
                }

                // If current workout day exists
                else
                {
                    currentDay = await App.DataBase.GetWorkoutDayById(basicCurrentDay.Id);
                    ID = currentDay.Id.ToString();
                    this.OnAppearing();
                    //await Shell.Current.GoToAsync($"////{nameof(ItemsPage)}?{nameof(ItemsPage.ID)}={currentDay.Id}");
                }
            }

            // Get existing workout day, and show it
            else
            {
                currentDay = await App.DataBase.GetWorkoutDayById(Int32.Parse(ID));
            }

            // Set workout day
            List<WorkoutDay> workoutDays = new List<WorkoutDay>();
            workoutDays.Add(currentDay);
            workoutDayView.ItemsSource = workoutDays; 
        }
    }
}