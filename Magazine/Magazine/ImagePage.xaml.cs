using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        public ImagePage(ImageSource imageSource)
        {
            InitializeComponent();
            var image = new Image { Source = imageSource };
            Content = new ScrollView { Content = image };
            BackgroundColor = Color.Black;
            image.GestureRecognizers.Add(new TapGestureRecognizer(async (s, e) => await Navigation.PopModalAsync()));
        }
    }
}
