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
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            History.ItemsSource = (List<HistoryDto>)BindingContext;
        }

        private async void GoToHistoryItem(object sender, SelectedItemChangedEventArgs e)
        {
            HistoryDto item = (HistoryDto)e.SelectedItem;
            HistoryItemPage page = new HistoryItemPage();
            page.BindingContext = item;
            await Navigation.PushAsync(page);
        }
    }
}