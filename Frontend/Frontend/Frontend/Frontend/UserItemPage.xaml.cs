using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Frontend.dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserItemPage : ContentPage
    {
        public UserItemPage()
        {
            InitializeComponent();
        }

        private async void ChangeStatus(object sender, EventArgs e)
        {
            AdminUserDto userDto = (AdminUserDto)BindingContext;
            ChangeStatusRequest request = new ChangeStatusRequest()
            {
                UserId = userDto.Id
            };
            Response response = await App.ApiClient.ChangeStatus(request);
            if (response.Code != HttpStatusCode.OK)
            {
                await DisplayAlert("Сообщение об ошибке", "Произошла какая-то ошибка", "OK");
            }

            Response usersResponse = await App.ApiClient.GetUsers();
            List<AdminUserDto> users = (List<AdminUserDto>)usersResponse.Data;
            UserListPage page = new UserListPage();
            page.BindingContext = users;
            await Navigation.PushAsync(page);
        }
    }
}