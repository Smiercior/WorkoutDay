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
    public partial class ExerciseStats : ContentPage
    {
        int ExerciseEntryID { get; set; }
        string ExerciseName { get; set; }
        WorkoutDay CurentDay { get; set; }
        string userEntriesNumber = "3";

        public ExerciseStats(WorkoutDay cD, string eN)
        {
            InitializeComponent();
            CurentDay = cD;
            ExerciseName = eN;
        }
        async protected override void OnAppearing()
        {
            base.OnAppearing();     
            string exerciseType = CurentDay.Type;
            string exerciseName = ExerciseName;

            Title.Title = exerciseName;
            List<WorkoutDay> workoutDays = new List<WorkoutDay>();
            if (userEntriesNumber == "all")
            {
                workoutDays = await App.DataBase.GetWorkoutDaysByType(exerciseType);
            }
            else
            {
                int entriesNumber = int.Parse(userEntriesNumber);
                workoutDays = await App.DataBase.GetWorkoutDaysByType(exerciseType, entriesNumber);
            }
           
            workoutDays.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
            List<ExerciseStat> exerciseStats = new List<ExerciseStat>();

            // Get all data about certain exercise
            foreach (WorkoutDay wDay in workoutDays)
            {
                // Except current day
                if(wDay.Id != CurentDay.Id)
                {
                    //Console.WriteLine(wDay.Date);

                    foreach (ExerciseEntry eE in wDay.ExerciseEntries)
                    {
                        // Only get exercise entry which has certain exercise name
                        if (eE.ExerciseName == exerciseName)
                        {
                            exerciseStats.Add(new ExerciseStat{ Date = wDay.Date, Sets = eE.Sets, Comment = eE.Comment });
                        }
                    }
                }
                
            }

            exerciseStatsView.ItemsSource = exerciseStats;
        }

        void OnPickerSelectedIndexChange(object sender, EventArgs e)
        {
            userEntriesNumber = ((Picker)sender).SelectedItem.ToString();
            this.OnAppearing();
        }
    }

    public class ExerciseStat
    {
        public DateTime Date { get; set; }
        public List<Set> Sets { get; set; }

        public string Comment { get; set; }
    }

}