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
    public partial class PublicViewList : ContentPage
    {
        public PublicViewList()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PublicList.ItemsSource = (List<GenerationDto>)BindingContext;
        }

        private async void GoToDetails(object sender, SelectedItemChangedEventArgs e)
        {
            GenerationDto generationDto = (GenerationDto)e.SelectedItem;
            PublicListItemPage page = new PublicListItemPage();
            page.BindingContext = generationDto;
            await Navigation.PushAsync(page);
        }
    }
}