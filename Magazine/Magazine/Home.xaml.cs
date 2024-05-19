using Magazine.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        private List<Products> products;

        public Home()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            products = new List<Products>();
            Load();
        }

        private async void Load()
        {
            try
            {
                var repository = App.Db;
                products = await repository.GetProductsAsync(); // Assuming GetProductsAsync is a method to retrieve product data from your database
                DisplayProducts(products);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load products: {ex.Message}", "OK");
            }
        }

        private void DisplayProducts(IEnumerable<Products> products)
        {
            var mainStack = new StackLayout();

            // Top StackLayout
            var topStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromHex("#B6174B"),
                HeightRequest = 70
            };

            var label = new Label
            {
                Text = "MONrO",
                FontFamily = "Montserrat",
                FontSize = 35,
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            var favouriteButton = new Button
            {
                Text = "Избранное",
                FontFamily = "Montserrat",
                FontSize = 17,
                TextColor = Color.FromHex("#DDA8B9"),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Fill,
                Margin = new Thickness(64, 0, 0, 0),
                FontAttributes = FontAttributes.Bold
            };
            favouriteButton.Clicked += ToFavourites;

            var aboutButton = new Button
            {
                Text = "О нас",
                FontFamily = "Montserrat",
                FontSize = 17,
                TextColor = Color.FromHex("#DDA8B9"),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.Fill,
                Margin = new Thickness(0),
                FontAttributes = FontAttributes.Bold
            };
            aboutButton.Clicked += ToAbout;

            topStack.Children.Add(label);
            topStack.Children.Add(favouriteButton);
            topStack.Children.Add(aboutButton);

            // Products StackLayout
            var productsStack = new StackLayout();

            foreach (var product in products)
            {
                var frame = new Frame
                {
                    CornerRadius = 10,
                    HasShadow = true,
                    Margin = new Thickness(25, 10, 25, 0),
                    WidthRequest = 260,
                    HeightRequest = 150
                };

                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(product.Foto)),
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 80,
                    WidthRequest = 80
                };

                var nameLabel = CreateLabel(product.Namee, 18, Color.Black, LayoutOptions.End, new Thickness(40, 5, 0, 0));
                var priceLabel = CreateLabel(product.Price.ToString(), 16, Color.Black, LayoutOptions.End, new Thickness(40, 5, 0, 0));

                var button1 = CreateButton("В корзину", Button1_Clicked);

                var stackInsideFrame = new StackLayout { Orientation = StackOrientation.Horizontal };
                stackInsideFrame.Children.Add(image);
                stackInsideFrame.Children.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = { nameLabel, priceLabel }
                });

                var stackInsideFrame1 = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.End };
                stackInsideFrame1.Children.Add(button1);

                var stackInsideFrame0 = new StackLayout { Orientation = StackOrientation.Vertical };
                stackInsideFrame0.Children.Add(stackInsideFrame);
                stackInsideFrame0.Children.Add(stackInsideFrame1);

                frame.Content = stackInsideFrame0;

                productsStack.Children.Add(frame);
            }

            var scrollView = new ScrollView { Content = productsStack };

            // Main StackLayout
            mainStack.Children.Add(topStack);
            mainStack.Children.Add(scrollView);

            Content = mainStack;
        }

        private Button CreateButton(string text, EventHandler clickHandler)
        {
            var button = new Button
            {
                Text = text,
                FontSize = 12,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#B6174B"),
                HeightRequest = 50,
                WidthRequest = 100,
                CornerRadius = 10
            };

            button.Clicked += clickHandler;

            return button;
        }

        private async void Button1_Clicked(object sender, EventArgs e)
        {
            // Обработчик события для кнопки "В корзину"
        }

        private Label CreateLabel(string text, int fontSize, Color textColor, LayoutOptions horizontalOptions, Thickness margin)
        {
            return new Label
            {
                Text = text,
                FontSize = fontSize,
                TextColor = textColor,
                HorizontalOptions = horizontalOptions,
                Margin = margin
            };
        }

        private async void ToFavourites(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Favourites());
        }

        private async void ToAbout(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }
    }
}
