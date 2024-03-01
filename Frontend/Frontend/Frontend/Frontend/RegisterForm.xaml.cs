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
    public partial class RegisterForm : ContentPage
    {
        public RegisterForm()
        {
            InitializeComponent();
        }
        
        private async void Register(object sender, EventArgs e)
        {
            string login = loginField.Text;
            string password = "";
            string name = "";
            string surname = "";
            if (loginField.Text != null)
            {
                login = loginField.Text.Trim();
                if (login.Length < 3)
                {
                    await DisplayAlert("Ошибка", "Логин должен быть больше 3 символов", "Ok");
                    return;
                }   
            }

            if (passwordField.Text != null)
            {
                password = passwordField.Text.Trim();
                if (password.Length < 3)
                {
                    await DisplayAlert("Ошибка", "Пароль должен быть больше 3 символов", "Ok");
                    return;
                }    
            }

            if (nameField.Text != null)
            {
                name = nameField.Text.Trim();
                if (name.Length < 2)
                {
                    await DisplayAlert("Ошибка", "Имя должно быть больше 2 символов", "Ok");
                    return;
                }   
            }

            if (surnameField.Text != null)
            {
                surname = surnameField.Text.Trim();
                if (surname.Length < 2)
                {
                    await DisplayAlert("Ошибка", "Фамилия должна быть больше 3 символов", "Ok");
                    return;
                }    
            }
            
            
            
            RegisterRequest request = new RegisterRequest()
            {
                Login = login,
                Password = password,
                Name = name,
                Surname = surname
            };

            Response response = await App.ApiClient.Register(request);
            if (response.Code == HttpStatusCode.InternalServerError)
            {
                await DisplayAlert("Ошибка", "Такой логин уже существует", "Ok");
                return;
            }

            await Navigation.PushAsync(new MainPage());
        }
    }
}