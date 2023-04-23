using System.Collections.Generic;
using System.ComponentModel;
using WorkoutMaster.Models;
using WorkoutMaster.ViewModels;
using Xamarin.Forms;
using System;
using System.Globalization;
using System.Collections.ObjectModel;

namespace WorkoutMaster.Views
{
    [QueryProperty(nameof(ID), nameof(ID))]
    public partial class ItemDetailPage : ContentPage
    {
        private int id;

        public int ID
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

        public Point actualScrollViewPosition { get; set; }

        public static WorkoutDay currentDay;

        public int CurrentItemID { get; set; }

        public bool ignoreScroll = false;

        public ItemDetailPage(int id)
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
            ID = id;
            CurrentItemID = -1;
        }
        public string FormattedDate { get; set; }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
     
            if (id != -1 && !ignoreScroll)
            {
                // Edit new workout day or existing workout day
                currentDay = await App.DataBase.GetWorkoutDayById(ID);
                if(currentDay == null)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    FormattedDate = currentDay.Date.ToShortDateString();
                    WorkoutDayLayout.BindingContext = currentDay;

                    // Scroll to certain item
                    if (currentDay.ExerciseEntries.Count > 0)
                    {
                        if (CurrentItemID >= 0) exerciseEntriesView.ScrollTo(CurrentItemID);
                        else if (CurrentItemID == -1) exerciseEntriesView.ScrollTo(currentDay.ExerciseEntries.Count - 1);
                    }
                }      
            }
            ignoreScroll = false;
        }

        async void OnNewExercise(object sender, EventArgs e)
        {
            CurrentItemID = -1;
            await Navigation.PushAsync(new ItemAddExercise(id), true);
        }

        async void OnAddSet(object sender, EventArgs e)
        {
            int exerciseEntryId = Int32.Parse(((Button)sender).CommandParameter.ToString());
            string KG = ((Button)sender).Parent.FindByName<Entry>("KG").Text;
            string Reps = ((Button)sender).Parent.FindByName<Entry>("Reps").Text;
            Set set = new Set();
            set.ExerciseEntryId = exerciseEntryId;

            // Validator //
            try
            {
                // Only comma
                set.KG = Double.Parse(KG, NumberStyles.Any, CultureInfo.InvariantCulture);

                // Check if reps are valid
                if(Reps[Reps.Length-1] == ',') Reps = Reps.Substring(0, Reps.Length-1);
                set.Reps = Reps;
                await App.DataBase.SaveSet(set);
            }
            catch (Exception ex)
            {

            }

            ScrollToCertainItem(exerciseEntryId);
            this.OnAppearing();
        }

        async void OnSetDelete(object sender, EventArgs e)
        {
            int ID = Int32.Parse(((Button)sender).CommandParameter.ToString());
            Set set = (await App.DataBase.GetSets()).Find(x => x.Id == ID);

            var answer = await DisplayAlert("Question?", $"Would you like to delete \"{set.Reps}\" set?", "Yes", "No");

            if(answer)
            {
                await App.DataBase.DelSet(ID);

                // Scroll to last scroll view posiotion
                //SetActualScrollViewPosiotion(exercisesListView.ScrollX, exercisesListView.ScrollY);

                ScrollToCertainItem(set.ExerciseEntryId);
                this.OnAppearing();
            }    
        }

        async void OnCommentSave(object sender, EventArgs e)
        {
            int exerciseEntryId = Int32.Parse(((Button)sender).CommandParameter.ToString());
            string comment = ((Button)sender).Parent.FindByName<Entry>("Comment").Text;
            Console.WriteLine(exerciseEntryId);

            ExerciseEntry exerciseEntry = (await App.DataBase.GetExerciseEntries()).Find(x => x.Id == exerciseEntryId);
            exerciseEntry.Comment = comment;

            await App.DataBase.UpdateExerciseEntry(exerciseEntry);

            // Scroll to last scroll view posiotion
            //SetActualScrollViewPosiotion(exercisesListView.ScrollX, exercisesListView.ScrollY);

            ScrollToCertainItem(exerciseEntryId);
            this.OnAppearing();
        }

        async void OnExerciseEntryDelete(object sender, EventArgs e)
        {
            int exerciseEntryId = Int32.Parse(((Button)sender).CommandParameter.ToString());
            ExerciseEntry exerciseEntry = (await App.DataBase.GetExerciseEntries()).Find(x => x.Id==exerciseEntryId);

            
            var answer = await DisplayAlert("Question?", $"Would you like to delete \"{exerciseEntry.ExerciseName}\" entry?", "Yes", "No");

            if(answer)
            {
                await App.DataBase.DelExerciseEntry(exerciseEntryId);
                // Scroll to last scroll view posiotion
                //SetActualScrollViewPosiotion(exercisesListView.ScrollX, exercisesListView.ScrollY);

                CurrentItemID = -1;           
                this.OnAppearing();
            }    
        }

        async void OnChangeExerciseType(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemChangeExerciseType(ID),true);
        }

        private void OnSetTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Entry e = (Entry)sender;
            List<char> numbers = new List<char>() {'0','1','2','3','4','5','6','7','8','9', ','};

            // Replace . with ,
            if (e.Text.Contains(".")) e.Text = e.Text.Replace('.', ',');

            
            try
            {
                // Check if user use only numbers or comma
                if (!(numbers.Contains(e.Text[e.Text.Length - 1]))) e.Text = e.Text.Substring(0, e.Text.Length - 1);

                // Dosen't allow multiple comma in a row
                if (e.Text[e.Text.Length - 1] == ',' && e.Text[e.Text.Length - 2] == ',') e.Text = e.Text.Substring(0, e.Text.Length - 1);
            }
            catch(IndexOutOfRangeException eq)
            {

            }          
        }

        private void OnKGTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Entry e = (Entry)sender;

            // Replace , with .
            if (e.Text.Contains(",")) e.Text = e.Text.Replace(',', '.');
        }

        async void OnExerciseStats(object sender, EventArgs e)
        {   
            string exerciseName = ((Button)sender).CommandParameter.ToString();
            ignoreScroll = true;
            await Navigation.PushAsync(new ExerciseStats(currentDay,exerciseName),true);
        }

        public void SetActualScrollViewPosiotion(double x, double y)
        {
            Point p = new Point();
            p.X = x;
            p.Y = y;
            actualScrollViewPosition = p;
        }

        public async void ScrollToCertainItem(int exerciseEntryId)
        {
            int itemID = 0;
            ExerciseEntry eE = (await App.DataBase.GetExerciseEntries()).Find(x => x.Id == exerciseEntryId);

            foreach(ExerciseEntry exerciseEntry in currentDay.ExerciseEntries)
            {
                if(exerciseEntry.ExerciseName == eE.ExerciseName)
                {
                    CurrentItemID = itemID;
                    break;
                }
                else
                {
                    CurrentItemID = 0;
                }
                itemID++;
            }
        }
    }
}