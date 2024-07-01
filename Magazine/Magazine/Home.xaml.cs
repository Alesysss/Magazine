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

        private async void DisplayProducts(IEnumerable<Products> products)
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
                Text = "О нас",
                FontFamily = "Montserrat",
                FontSize = 20,
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
                Text = "Выход",
                FontFamily = "Montserrat",
                FontSize = 20,
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
                    HeightRequest = 170
                };

                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(product.Foto)),
                    Aspect = Aspect.AspectFit, // Изменение аспекта изображения, чтобы изображение не обрезалось
                    HeightRequest = 100,
                    WidthRequest = 120
                };

                var nameLabel = CreateLabel(product.Namee, 20, Color.FromHex("#561429"), LayoutOptions.End, new Thickness(40, 5, 0, 0));
                var priceLabel = CreateLabel(product.Price.ToString(), 18, Color.FromHex("#561429"), LayoutOptions.End, new Thickness(40, 5, 0, 0));

                var button1 = CreateButton("В корзину", Button1_Clicked, product);

                image.GestureRecognizers.Add(new TapGestureRecognizer(async (s, e) => await Navigation.PushModalAsync(new ImagePage(image.Source))));

                var stackInsideFrame = new StackLayout { Orientation = StackOrientation.Horizontal };
                stackInsideFrame.Children.Add(image);
                stackInsideFrame.Children.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = { nameLabel, priceLabel }
                });

                var stackInsideFrame0 = new StackLayout { Orientation = StackOrientation.Vertical };
                stackInsideFrame0.Children.Add(stackInsideFrame);
                stackInsideFrame0.Children.Add(button1);

                frame.Content = stackInsideFrame0;

                ProductStackLayout.Children.Add(frame);
            }
        }


        private Button CreateButton(string text, EventHandler clickHandler, Products product)
        {
            var button = new Button
            {
                Text = text,
                FontSize = 20,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#b33760"),
                HeightRequest = 50,
                WidthRequest = 100,
                CornerRadius = 10,
                Margin = new Thickness(0, 25, 10, 0),
                BindingContext = product // Установка контекста кнопки
            };

            button.Clicked += clickHandler;

            return button;
        }


        private async void Button1_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (Button)sender;
                var product = (Products)button.BindingContext;

                int userId = App.CurrentUser.Id;
                decimal price = product.Price;

                var result = await App.Db.AddToCartAsync(userId, product.Id, price);

                if (result != -1)
                {
                    await DisplayAlert("Супер", "Товар добавлен в корзину", "OK");

                    var cartPage = new Cart();
                    NavigationPage.SetHasNavigationBar(cartPage, false);
                    
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add product to cart.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to add product to cart: {ex.Message}", "OK");
            }
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
            await Navigation.PushAsync(new MainPage());
        }
    }
}
