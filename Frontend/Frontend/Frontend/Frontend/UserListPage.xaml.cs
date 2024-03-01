using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.dto;
using Frontend.holder;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserListPage : ContentPage
    {
        public UserListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            List<AdminUserDto> users = (List<AdminUserDto>)BindingContext;
            Users.ItemsSource = users;
        }

        private async void GoToUser(object sender, SelectedItemChangedEventArgs e)
        {
            AdminUserDto user = (AdminUserDto)e.SelectedItem;
            UserItemPage page = new UserItemPage();
            page.BindingContext = user;
            await Navigation.PushAsync(page);
        }

        private async void SigOut(object sender, EventArgs e)
        {
            UserTokenHolder.Token = null;
            MainPage mainPage = new MainPage();
            await Navigation.PushAsync(mainPage);
        }
    }
}