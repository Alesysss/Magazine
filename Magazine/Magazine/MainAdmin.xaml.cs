using Magazine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainAdmin : ContentPage
    {
        private List<Products> products;
        private List<RadioButton> radioButtons; 

        public MainAdmin()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            products = new List<Products>();
            radioButtons = new List<RadioButton>();
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

            ProductStackLayout.Children.Clear();

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
                    HeightRequest = 120
                };
                var radiobutStack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center
                };

                var radioBut = new RadioButton
                {
                    GroupName = "prod",
                    BindingContext = product
                };
                radioButtons.Add(radioBut);
                radiobutStack.Children.Add(radioBut);
                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(product.Foto)),
                    Aspect = Aspect.AspectFit, // Изменение аспекта изображения, чтобы изображение не обрезалось
                    HeightRequest = 100,
                    WidthRequest = 120
                };

                var nameLabel = CreateLabel(product.Namee, 20, Color.FromHex("#561429"), LayoutOptions.End, new Thickness(40, 5, 0, 0));
                var priceLabel = CreateLabel(product.Price.ToString(), 18, Color.FromHex("#561429"), LayoutOptions.End, new Thickness(40, 5, 0, 0));



                image.GestureRecognizers.Add(new TapGestureRecognizer(async (s, e) => await Navigation.PushModalAsync(new ImagePage(image.Source))));

                var stackInsideFrame = new StackLayout { Orientation = StackOrientation.Horizontal };
                stackInsideFrame.Children.Add(radiobutStack);
                stackInsideFrame.Children.Add(image);
                stackInsideFrame.Children.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = { nameLabel, priceLabel }
                });

                var stackInsideFrame0 = new StackLayout { Orientation = StackOrientation.Vertical };
                stackInsideFrame0.Children.Add(stackInsideFrame);

                frame.Content = stackInsideFrame0;

                ProductStackLayout.Children.Add(frame);
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
            ProductStackLayout.Children.Clear();

            // Products StackLayout
            var productsStack = new StackLayout();

            foreach (var product in searchResults)
            {
                var frame = new Frame
                {
                    CornerRadius = 10,
                    HasShadow = true,
                    Margin = new Thickness(25, 10, 25, 0),
                    WidthRequest = 260,
                    HeightRequest = 120
                };
                var radiobutStack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center
                };

                var radioBut = new RadioButton
                {
                    GroupName = "prod",
                    BindingContext = product
                };
                radioButtons.Add(radioBut);
                radiobutStack.Children.Add(radioBut);
                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(product.Foto)),
                    Aspect = Aspect.AspectFit, // Изменение аспекта изображения, чтобы изображение не обрезалось
                    HeightRequest = 100,
                    WidthRequest = 120
                };

                var nameLabel = CreateLabel(product.Namee, 20, Color.FromHex("#561429"), LayoutOptions.End, new Thickness(40, 5, 0, 0));
                var priceLabel = CreateLabel(product.Price.ToString(), 18, Color.FromHex("#561429"), LayoutOptions.End, new Thickness(40, 5, 0, 0));



                image.GestureRecognizers.Add(new TapGestureRecognizer(async (s, e) => await Navigation.PushModalAsync(new ImagePage(image.Source))));

                var stackInsideFrame = new StackLayout { Orientation = StackOrientation.Horizontal };
                stackInsideFrame.Children.Add(radiobutStack);
                stackInsideFrame.Children.Add(image);
                stackInsideFrame.Children.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = { nameLabel, priceLabel }
                });

                var stackInsideFrame0 = new StackLayout { Orientation = StackOrientation.Vertical };
                stackInsideFrame0.Children.Add(stackInsideFrame);

                frame.Content = stackInsideFrame0;

                ProductStackLayout.Children.Add(frame);
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
                ProductStackLayout.Children.Clear();
                DisplayProducts(products);
                return;
            }
            // Ищем товары, чье имя содержит введенный запрос, без учета регистра
            var searchResults = products.Where(p => p.Namee.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            // Отображаем результаты поиска
            DisplaySearchResults(searchResults);
        }

        private async void ToAbout(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void Add(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAdmin());
        }

        private async void Update(object sender, EventArgs e)
        {
            var selectedProduct = products.FirstOrDefault(p => radioButtons.Any(rb => rb.IsChecked && rb.BindingContext == p));

            if (selectedProduct == null)
            {
                await DisplayAlert("Ошибка", "Выберите товар для изменения.", "OK");
                return;
            }

            await Navigation.PushAsync(new UpdateAdmin(selectedProduct));
        }


        private async void Delete(object sender, EventArgs e)
        {
            var selectedProduct = products.FirstOrDefault(p => radioButtons.Any(rb => rb.IsChecked && rb.BindingContext == p));

            if (selectedProduct == null)
            {
                await DisplayAlert("Ошибка", "Выберите товар для удаления.", "OK");
                return;
            }

            var confirm = await DisplayAlert("Подтвердить удаление", $"Вы действительно хотите удалить товар '{selectedProduct.Namee}'?", "Да", "Отмена");

            if (confirm)
            {
                try
                {
                    var repository = App.Db;
                    var result = await repository.DeleteProducts(selectedProduct);

                    if (result > 0)
                    {
                        await DisplayAlert("Успешно", "Товар успешно удален.", "OK");
                        Load(); // Обновляем список товаров после удаления
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Не удалось удалить товар.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", $"Не удалось выполнить удаление: {ex.Message}", "OK");
                }
            }
        }
    }
}