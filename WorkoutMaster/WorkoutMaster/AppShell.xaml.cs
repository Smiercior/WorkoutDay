using System;
using System.Collections.Generic;
using WorkoutMaster.ViewModels;
using WorkoutMaster.Views;
using Xamarin.Forms;

namespace WorkoutMaster
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
