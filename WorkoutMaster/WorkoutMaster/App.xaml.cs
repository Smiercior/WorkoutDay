using System;
using System.IO;
using WorkoutMaster.Services;
using WorkoutMaster.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutMaster
{
    public partial class App : Application
    {
        private static DataBase dataBase;
        public static DataBase DataBase
        {
            get
            {
                if(dataBase == null)
                {
                    dataBase = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "workout.db3"));
                }
                return dataBase;
            } 
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
