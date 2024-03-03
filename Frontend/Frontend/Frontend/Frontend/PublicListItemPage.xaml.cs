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
    public partial class PublicListItemPage : ContentPage
    {
        public PublicListItemPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            GenerationDto dto = (GenerationDto)BindingContext;
            ImageVal.Source = Xamarin.Forms.ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(dto.Image)));
            Promt.Text = dto.Promt;
            Rate.Text = "Оценка - " + Convert.ToString(dto.Rating);
        }
    }
}