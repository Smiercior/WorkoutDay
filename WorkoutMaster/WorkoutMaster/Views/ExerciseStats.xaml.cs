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

        public ExerciseStats(WorkoutDay cD, string eN)
        {
            InitializeComponent();
            CurentDay = cD;
            ExerciseName = eN;
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            //ExerciseEntry exerciseEntry = (await App.DataBase.GetExerciseEntries()).Find(x => x.Id == ExerciseEntryID);
            //WorkoutDay workoutDay = await App.DataBase.GetWorkoutDayById(exerciseEntry.WorkoutDayId);
            //WorkoutDay workoutDay = (await App.DataBase.GetWorkoutDays()).Find(x => x.Id == exerciseEntry.WorkoutDayId);

            string exerciseType = CurentDay.Type;
            string exerciseName = ExerciseName;

            Title.Title = exerciseName;

            //Console.WriteLine("exercise Type: " + exerciseType);
            //Console.WriteLine("exercise Name: " + exerciseName);

            //List<WorkoutDay> workoutDays = (await App.DataBase.GetWorkoutDays()).FindAll(x => x.Type == exerciseType);
            List<WorkoutDay> workoutDays = await App.DataBase.GetWorkoutDaysByType(exerciseType);
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
    }

    public class ExerciseStat
    {
        public DateTime Date { get; set; }
        public List<Set> Sets { get; set; }

        public string Comment { get; set; }
    }

}