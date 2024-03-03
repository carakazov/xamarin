using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenerateResultPage : ContentPage
    {
        public GenerateResultPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            GeneratePageDto pageDto = (GeneratePageDto)BindingContext;
            ImageVal.Source = Xamarin.Forms.ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(pageDto.Response.Base64Image)));
            Promt.Text = pageDto.Promt;
            Rate.Text = "Оценка: " + Convert.ToString(pageDto.Response.Rating);
            if (pageDto.Response.IsPublic)
            {
                IsPublic.Text = "Публично - Да";
            }
            else
            {
                IsPublic.Text = "Публично - Нет";
            }
        }

        public async void RateOne(object sender, EventArgs e)
        {
            await RateImage(1);
        }

        public async void RateTwo(object sender, EventArgs e)
        {
            await RateImage(2);
        }

        public async void RateThree(object sender, EventArgs e)
        {
            await RateImage(3);
        }

        public async void RateFour(object sender, EventArgs e)
        {
            await RateImage(4);
        }

        public async void RateFive(object sender, EventArgs e)
        {
            await RateImage(5);
        }

        public async void MakePublic(object sender, EventArgs e)
        {
            RateImgDto request = new RateImgDto()
            {
                Id = ((GeneratePageDto)BindingContext).Response.Id
            };
            Response response = await App.ApiClient.MakePublic(request);
            if (IsPublic.Text.Contains("Да"))
            {
                IsPublic.Text = "Публично - Нет";
            }
            else
            {
                IsPublic.Text = "Публично - Да";
            }
        }
        
        private async Task<Response> RateImage(int rate)
        {
            RateImgDto rateImgDto = new RateImgDto()
            {
                Id = ((GeneratePageDto)BindingContext).Response.Id,
                Rate = rate
            };
            Rate.Text = "Оценка: " + Convert.ToString(rate);
            Response response = await App.ApiClient.RateImg(rateImgDto);
            return response;
        }
    }
}