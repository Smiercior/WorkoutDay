using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WorkoutMaster.Models;
using WorkoutMaster.Views;
using Xamarin.Forms;

namespace WorkoutMaster.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public Command EditWorkoutDayCommand { get; }

        public int ID { get; set; }

        private INavigation navigation;

        public INavigation Navigation
        {
            get { return navigation; }
            set { navigation = value; }
        }


        public ItemsViewModel(INavigation _navigation)
        {
            navigation = _navigation;
            Title = "Browse";
  
            EditWorkoutDayCommand = new Command(() => _navigation.PushAsync(new ItemDetailPage(ID), true));

            ItemTapped = new Command<Item>(OnItemSelected);
            //Console.WriteLine($"Dziala: {Idd}");
        }

        public void OnAppearing(int id)
        {
            ID = id;
            IsBusy = true;
        }
      
        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }
    }
}