using Magazine.Model;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateAdmin : ContentPage
    {
        private Products selectedProduct;

        public UpdateAdmin(Products product)
        {
            InitializeComponent();
            selectedProduct = product;
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            Name.Text = selectedProduct.Namee;
            Price.Text = selectedProduct.Price.ToString();
            Foto.Text = selectedProduct.Foto;
        }

        private async void UpdateProd(object sender, EventArgs e)
        {
            string name = Name.Text.Trim();
            string price = Price.Text.Trim();
            string foto = Foto.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(price) || string.IsNullOrWhiteSpace(foto))
            {
                await DisplayAlert("Ошибка", "Все поля должны быть заполнены.", "ОК");
                return;
            }

            if (name.Length < 3 || name.Length > 25)
            {
                await DisplayAlert("Ошибка", "Название не может быть менее трех и более двадцати пяти символов", "Ok");
                return;
            }

            if (!price.All(char.IsDigit))
            {
                await DisplayAlert("Ошибка", "Цена должна содержать только цифры", "Ok");
                return;
            }

            if (price.Length < 3)
            {
                await DisplayAlert("Ошибка", "Цена не может быть такой маленькой", "Ok");
                return;
            }

            if (price.Length > 8)
            {
                await DisplayAlert("Ошибка", "Цена не может быть такой большой", "Ok");
                return;
            }

            selectedProduct.Namee = name;
            selectedProduct.Price = Convert.ToDecimal(price);
            selectedProduct.Foto = foto;

            await App.Db.UpdateProductAsync(selectedProduct);

            await DisplayAlert("Успех", "Товар успешно обновлен.", "ОК");
            await Navigation.PushAsync(new MainAdmin());
        }
    }
}
