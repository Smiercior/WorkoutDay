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
    public partial class ItemChangeExerciseType : ContentPage
    {
        public int ID { get; set; }
        public ItemChangeExerciseType(int Id)
        {
            InitializeComponent();
            ID = Id;
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load exercise types
            List<ExerciseType> exerciseTypes = new List<ExerciseType>();
            exerciseTypes = (await App.DataBase.GetExerciseTypes());
            exerciseTypesView.ItemsSource = exerciseTypes;
        }

        async void OnAddExerciseType(object sender, EventArgs e)
        {
            try
            {
                string exerciseTypeName = ((Button)sender).CommandParameter.ToString();
                //WorkoutDay workoutDay = (await App.DataBase.GetWorkoutDays()).Find(x => x.Id == ID);
                WorkoutDay workoutDay = await App.DataBase.GetWorkoutDayById(ID);
                workoutDay.Type = exerciseTypeName;
                await App.DataBase.UpdateWorkoutDay(workoutDay);
            }
            catch (Exception ex)
            {

            }
            await Navigation.PopAsync();
        }
    }
}