using Magazine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAdmin : ContentPage
    {
        public AddAdmin()
        {
            InitializeComponent();
        }
        private async void AddProd(object sender, EventArgs e)
        {
            string name = Name.Text.Trim();
            string price = Price.Text.Trim();
            string foto = Foto.Text.Trim();



            if (name.Length < 3 || name.Length > 25 || name.Length == 0)
            {
                await DisplayAlert("Ошибка", "Название не может быть менее трех и более двадцати пяти символов", "Ok");
                return;
            }
            else if (!price.All(char.IsDigit))
            {
                await DisplayAlert("Ошибка", "Цена должно содержать только цифры", "Ok");
                return;
            }

            else if (price.Length < 3)
            {
                await DisplayAlert("Ошибка", "Цена не может быть такой маленькой", "Ok");
                return;
            }
            else if (price.Length > 8)
            {
                await DisplayAlert("Ошибка", "Цена не может быть такой большой", "Ok");
                return;
            }
            else
            {

                // Создание нового объекта клиента
                Products newProducts = new Products
                {
                    Namee = name,
                    Price = Convert.ToDecimal(price),
                    Descriptionn = "Description of product",
                    Category_id = 1,
                    Foto = foto
                };

                // Сохранение клиента в базе данных
                await App.Db.SaveProducts(newProducts);


                await DisplayAlert("Ура", "Товар успешно добавлен", "OK");
                await Navigation.PushAsync(new MainAdmin());
            }
        }
    }
}