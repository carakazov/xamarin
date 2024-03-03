using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Frontend.dto;
using Frontend.holder;
using Xamarin.Forms;

namespace Frontend
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Image1.Source = ImageSource.FromResource("Frontend.icon.png");
            Image2.Source = ImageSource.FromResource("Frontend.icontele.png");
            Image3.Source = ImageSource.FromResource("Frontend.iconvib.png");
        }

        private async void SignIn(object sender, EventArgs e)
        {
            string login = "";
            string password = "";
            if (loginField.Text != null)
            {
                login = loginField.Text.Trim();
                if (login.Length < 3)
                {
                    await DisplayAlert("Ошибка", "Логин должен быть более 3 символов", "Ok");
                    return;
                }
            }

            if (passwordField.Text != null)
            {
                password = passwordField.Text.Trim();
                if (password.Length < 3)
                {
                    await DisplayAlert("Ошибка", "Пароль должен быть более 3 символов", "Ok");
                    return;
                }
            }

            AuthRequest request = new AuthRequest();
            request.Login = login;
            request.Password = password;
            Response response = await App.ApiClient.Auth(request);
            if (response.Code == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Ошибка", "Неверный логин или пароль", "Ok");
            }
            else
            {
                AuthResponse authResponse = (AuthResponse)response.Data;
                UserTokenHolder.Token = authResponse.Token;
                string decodedToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authResponse.Token));
                string[] parts = decodedToken.Split('|');
                string role = parts[2];
                if (role == "ROLE_USER")
                {
                    await Navigation.PushAsync(new RecognizeForm());   
                }
                else
                {
                    UserListPage userListPage = new UserListPage();
                    Response usersResponse = await App.ApiClient.GetUsers();
                    List<AdminUserDto> users = (List<AdminUserDto>)usersResponse.Data;
                    userListPage.BindingContext = users;
                    await Navigation.PushAsync(userListPage);
                }
            }
        }

        private async void GoToRegister(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterForm());
        }
    }
}