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
    public partial class HistoryItemPage : ContentPage
    {
        public HistoryItemPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            HistoryDto item = (HistoryDto)BindingContext;
            ImageVal.Source = Xamarin.Forms.ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(item.Image)));
        }
        private void Cancel(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}