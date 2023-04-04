using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WorkoutMaster.Models;

namespace WorkoutMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExerciseTypesPage : ContentPage
    {
        public ExerciseTypesPage()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            ExerciseTypesView.ItemsSource = (await App.DataBase.GetExerciseTypes());
        }

        async void OnAddExerciseType(object sender, EventArgs e)
        {
            string name = ((Button)sender).Parent.FindByName<Entry>("Name").Text;
            string description = ((Button)sender).Parent.FindByName<Entry>("Description").Text;

            ExerciseType exerciseType = new ExerciseType { Name = name, Description = description };
            await App.DataBase.SaveExerciseType(exerciseType);

            // Clear input fields
            ((Button)sender).Parent.FindByName<Entry>("Name").Text = "";
            ((Button)sender).Parent.FindByName<Entry>("Description").Text = "";

            this.OnAppearing();
        }

        async void OnDelExerciseType(object sender, EventArgs e)
        {
            bool canDelete = true;
            //List<WorkoutDay> workoutDays = (await App.DataBase.GetWorkoutDays());
            int id = Int32.Parse(((Button)sender).CommandParameter.ToString());
            ExerciseType exerciseType = (await App.DataBase.GetExerciseTypes()).Find(x => x.Id == id);

            if((await App.DataBase.GetWorkoutDaysByType(exerciseType.Name)).Count != 0)
            {
                canDelete = false;
            }

            /*foreach (WorkoutDay workoutDay in workoutDays)
            {
                if (workoutDay.Type == exerciseType.Name) canDelete = false;
            }*/

            if (canDelete) await App.DataBase.DelExerciseType(id);
            this.OnAppearing();
        }
    }
}