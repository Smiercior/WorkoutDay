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
    public partial class ExercisesPage : ContentPage
    {
        public ExercisesPage()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            ExercisesView.ItemsSource = (await App.DataBase.GetExercises());
        }

        async void OnAddExercise(object sender, EventArgs e)
        {
            string name = ((Button)sender).Parent.FindByName<Entry>("Name").Text;
            string description = ((Button)sender).Parent.FindByName<Entry>("Description").Text;

            Exercise exercise = new Exercise { Name = name, Description = description };
            await App.DataBase.SaveExercise(exercise);

            // Clear input fields
            ((Button)sender).Parent.FindByName<Entry>("Name").Text = "";
            ((Button)sender).Parent.FindByName<Entry>("Description").Text = "";

            this.OnAppearing();
        }

        async void OnDelExercise(object sender, EventArgs e)
        {
            bool canDelete = true;
            List<ExerciseEntry> exerciseEntries = (await App.DataBase.GetExerciseEntries());
            int id = Int32.Parse(((Button)sender).CommandParameter.ToString());
            foreach (ExerciseEntry exerciseEntry in exerciseEntries)
            {
                if(exerciseEntry.ExerciseId == id ) canDelete = false;
            }
            
            if (canDelete) await App.DataBase.DelExercise(id);
            this.OnAppearing();
        }
    }
}