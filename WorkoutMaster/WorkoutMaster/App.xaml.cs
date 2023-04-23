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
                    //dataBase = new DataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "workout.db3"));
                    //dataBase = new DataBase(Path.Combine("/storage/emulated/0/Android/data/com.companyname.workoutmaster", "workout.db3"));
                    dataBase = new DataBase(DBPath);
                }
                return dataBase;
            } 
        }

        private static string dbPath;
        public static string DBPath
        {
            get
            {
                return dbPath;
            }
            private set
            {
                dbPath = value;
            }
        }

        private static string dbFolder;
        public static string DBFolder
        {
            get
            {
                return dbFolder;
            }
            private set
            {
                dbFolder = value;
            }
        }

        public App()
        {
            DBPath = Path.Combine("/storage/emulated/0/Android/data/com.companyname.workoutmaster", "workout.db3");
            DBFolder = "/storage/emulated/0/Android/data/com.companyname.workoutmaster";
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
