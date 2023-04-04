using System;
using System.Collections.Generic;
using System.Text;
using WorkoutMaster.Views;
using Xamarin.Forms;

namespace WorkoutMaster.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            Title = "Add elements";
        }
    }
}
