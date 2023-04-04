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
    public partial class ItemAddExercise : ContentPage
    {
        public int ID { get; set; }
        public ItemAddExercise(int id)
        {
            InitializeComponent();
            ID = id;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            exercisesView.ItemsSource = await App.DataBase.GetExercises();
        }

        async void OnAddExercise(object sender, EventArgs e)
        {
            ExerciseEntry exerciseEntry = new ExerciseEntry();
            exerciseEntry.WorkoutDayId = ID;
            int exerciseId = Int32.Parse(((Button)sender).CommandParameter.ToString());
            exerciseEntry.ExerciseId = exerciseId;
            Exercise exercise = new Exercise();
            exercise = (await App.DataBase.GetExercises()).Find(x => x.Id == exerciseId);
            if(exercise != null)
            {
                exerciseEntry.ExerciseName = exercise.Name;
                await App.DataBase.SaveExerciseEntry(exerciseEntry);
            }
            await Navigation.PopAsync();
        }
    }
}