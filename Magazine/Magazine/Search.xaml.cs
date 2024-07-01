using Magazine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        private List<Products> products;

        public Search()
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
                products = await repository.GetProductsAsync(); // Получение продуктов для поиска
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Не удалось загрузить продукты: {ex.Message}", "OK");
            }
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string searchText = searchEntry.Text?.Trim();

                if (string.IsNullOrEmpty(searchText))
                {
                    await DisplayAlert("Ошибка", "Введите текст для поиска.", "OK");
                    return;
                }

                var searchResults = await App.Db.SearchProductsAsync(searchText);

                DisplaySearchResults(searchResults);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Не удалось выполнить поиск: {ex.Message}", "OK");
            }
        }

        private void DisplaySearchResults(IEnumerable<Products> searchResults)
        {
            searchResultsStack.Children.Clear();

            foreach (var product in searchResults)
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
                    Aspect = Aspect.AspectFit,
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

                searchResultsStack.Children.Add(frame);
            }
        }

        // Этот метод обрабатывает завершение ввода текста в поле поиска
        private void OnSearchCompleted(object sender, EventArgs e)
        {
            // Вызываем метод для обработки нажатия на кнопку поиска
            OnSearchClicked(sender, e);
        }

        // Метод для обработки нажатия на кнопку поиска
        private void OnSearchClicked(object sender, EventArgs e)
        {
            // Получаем текст из поля поиска
            string query = searchEntry.Text?.Trim();
            if (string.IsNullOrEmpty(query))
            {
                DisplayAlert("Ошибка", "Введите текст для поиска.", "OK");
                return;
            }
            // Ищем товары, чье имя содержит введенный запрос, без учета регистра
            var searchResults = products.Where(p => p.Namee.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            // Отображаем результаты поиска
            DisplaySearchResults(searchResults);
        }

        private Button CreateButton(string text, EventHandler clickHandler, Products product)
        {
            var button = new Button
            {
                Text = text,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
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
                    await DisplayAlert("Ошибка", "Не удалось добавить товар в корзину.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", $"Не удалось добавить товар в корзину: {ex.Message}", "OK");
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
    }
}
