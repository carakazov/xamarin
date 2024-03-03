using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.dto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Frontend
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenerationHistoryPage : ContentPage
    {
        public GenerationHistoryPage()
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            History.ItemsSource = (List<GenerationDto>)BindingContext;
        }

        private async void GoToDetails(object sender, SelectedItemChangedEventArgs e)
        {
            GenerationDto generation = (GenerationDto)e.SelectedItem;
            GeneratePageDto pageDto = new GeneratePageDto();
            GenerateImageResponse response = new GenerateImageResponse()
            {
                Base64Image = generation.Image,
                Id = generation.Id,
                Rating = generation.Rating,
                IsPublic = generation.IsPublic
            };
            pageDto.Response = response;
            pageDto.Promt = generation.Promt;
            GenerateResultPage page = new GenerateResultPage();
            page.BindingContext = pageDto;
            await Navigation.PushAsync(page);
        }
    }
}