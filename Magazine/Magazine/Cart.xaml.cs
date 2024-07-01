using Magazine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        public Cart()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.Appearing += Cart_Appearing;
            Load();
        }

        private void Cart_Appearing(object sender, EventArgs e)
        {
            ClearCart();
            Load();
        }



        public async void Load()
        {

            //ClearCart();
            try
            {
                var userId = App.CurrentUser.Id;
                var repository = App.Db;
                var cartProducts = await repository.GetCartProductsAsync(userId);
                DisplayCartProducts(cartProducts);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load cart products: {ex.Message}", "OK");
            }
        }

        // В методе DisplayCartProducts измените логику следующим образом:

        // В методе DisplayCartProducts измените логику следующим образом:

        private async void DisplayCartProducts(IEnumerable<Products> products)
        {
            ClearCart();

            decimal totalSum = 0;
            bool hasProducts = false;

            foreach (var product in products)
            {
                var cartItems = await App.Db.GetCartItemsAsync(App.CurrentUser.Id);
                var cartItem = cartItems.FirstOrDefault(item => item.Product_id == product.Id);

                var frame = new Frame
                {
                    CornerRadius = 10,
                    HasShadow = true,
                    Margin = new Thickness(25, 10, 25, 0),
                    WidthRequest = 350, // Увеличиваем ширину фрейма
                    HeightRequest = 180
                };

                var image = new Image
                {
                    Source = ImageSource.FromUri(new Uri(product.Foto)),
                    Aspect = Aspect.AspectFill,
                    HeightRequest = 80,
                    WidthRequest = 80
                };

                var nameLabel = CreateLabel(product.Namee, 20, Color.FromHex("#561429"), LayoutOptions.EndAndExpand, new Thickness(40, 5, 0, 0));
                var priceLabel = CreateLabel($"Цена: {product.Price}", 18, Color.FromHex("#561429"), LayoutOptions.EndAndExpand, new Thickness(40, 5, 0, 0));
                var quantityLabel = CreateLabel($"Количество: {cartItem?.Quantity ?? 0}", 18, Color.FromHex("#561429"), LayoutOptions.EndAndExpand, new Thickness(40, 5, 0, 0));
                var totalPriceLabel = CreateLabel($"Итого: {(cartItem?.Moneys * (cartItem?.Quantity ?? 0)) ?? 0}", 18, Color.FromHex("#561429"), LayoutOptions.EndAndExpand, new Thickness(40, 5, 0, 0));

                var deleteButton = new Button
                {                    
                    Text = "Удалить",
                    FontSize = 20,
                    BackgroundColor = Color.FromHex("#b33760"),
                    TextColor = Color.White,
                    CornerRadius = 10,
                    WidthRequest = 100, // Устанавливаем ширину кнопки
                                        //HorizontalOptions = LayoutOptions.End,
                    HeightRequest= 44,
                    Margin = new Thickness(0, 5, 10, 0)
                };
                deleteButton.Clicked += async (sender, e) =>
                {
                    if (cartItem != null && cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        await App.Db.UpdateCartItemAsync(cartItem); // Вызываем метод UpdateCartItemAsync для уменьшения количества товара
                    }
                    else if (cartItem != null && cartItem.Quantity == 1)
                    {
                        await App.Db.DeleteCartItemAsync(cartItem); // Вызываем метод DeleteCartItemAsync для полного удаления товара из корзины
                    }
                    // После удаления товара обновляем отображение корзины
                    Load();
                };

                var stackInsideFrame = new StackLayout { Orientation = StackOrientation.Horizontal };
                stackInsideFrame.Children.Add(image);
                stackInsideFrame.Children.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = { nameLabel, priceLabel, quantityLabel, totalPriceLabel }
                });

                image.GestureRecognizers.Add(new TapGestureRecognizer(async (s, e) => await Navigation.PushModalAsync(new ImagePage(image.Source))));

                var stackInsideFrame0 = new StackLayout { Orientation = StackOrientation.Vertical };
                stackInsideFrame0.Children.Add(stackInsideFrame);
                stackInsideFrame0.Children.Add(deleteButton); // Добавляем кнопку "Удалить" к внутреннему стеку

                frame.Content = stackInsideFrame0;

                cartStackLayout.Children.Add(frame);

                // Вычисляем общую сумму
                totalSum += (cartItem?.Moneys * (cartItem?.Quantity ?? 0)) ?? 0;
                if (cartItem != null)
                {
                    hasProducts = true;
                }
            }

            if (hasProducts)
            {
                // Добавляем итоговую сумму
                var totalSumLabel = new Label
                {
                    Text = $"Итого: {totalSum}",
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromHex("#561429"),
                    Margin = new Thickness(25, 20, 0, 0)
                };
                cartStackLayout.Children.Add(totalSumLabel);

                // Добавляем кнопку "Оформить заказ"
                var checkoutButton = new Button
                {
                    Text = "Оформить заказ",
                    FontSize = 20,
                    BackgroundColor = Color.FromHex("#b33760"),
                    TextColor = Color.White,
                    CornerRadius = 10,
                    WidthRequest = 350,
                    HeightRequest = 50,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(25, 20, 0, 0)
                };
                cartStackLayout.Children.Add(checkoutButton);
                checkoutButton.Clicked += async (sender, e) =>
                {
                    // Ваш код для оформления заказа
                    await DisplayAlert("Сообщение", "Заказ оформлен. Ожидайте сообщения о прибытии доставки, которое придет на ваш номер. Спасибо, что пользуетесь нашими услугами!", "OK");

                    // Очистка корзины в базе данных
                    await App.Db.ClearCartAsync(App.CurrentUser.Id);
                    Load();

                    //
                };

                var Button = new Button
                {
                    Text = "Очистить корзину",
                    FontSize = 20,
                    BackgroundColor = Color.FromHex("#b33760"),
                    TextColor = Color.White,
                    CornerRadius = 10,
                    WidthRequest = 350,
                    HeightRequest = 50,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(25, 10, 0, 20)
                };
                cartStackLayout.Children.Add(Button);
                Button.Clicked += async (sender, e) =>
                {
                    // Очистка корзины в базе данных
                    await App.Db.ClearCartAsync(App.CurrentUser.Id);
                    Load();

                    //
                };
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
        public void ClearCart()
        {
            cartStackLayout.Children.Clear();
        }
    }
}
