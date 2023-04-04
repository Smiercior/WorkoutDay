using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutMaster.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        async void OnNewExercise(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExercisesPage(), true);
        }

        async void OnNewExerciseType(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExerciseTypesPage(), true);
        }
    }
}