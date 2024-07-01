using Magazine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Windows; // Для класса Window



namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration1 : ContentPage
    {
        public Registration1()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
           
        }
        private async void AuthorizationReg(object sender, EventArgs e)
        {
            string firstName = Surn.Text.Trim();
            string lastName = Name.Text;
            string data = Dat.Date.ToString("dd-MMM-yyyy");
            string phone = Tel.Text;
            string login = Log.Text;
            string password = Pas.Text;
            //string passwordRepeat = PasRep.Text;
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(phone)
                || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }
            else if (firstName.Length < 3 || firstName.Length > 15)
            {
                await DisplayAlert("Ошибка", "Фамилия не может быть менее трех и более пятнадцати символов", "Ok");
                return;
            }
            else if (lastName.Length < 2 || lastName.Length > 15)
            {
                await DisplayAlert("Ошибка", "Имя не может быть менее двух и более пятнадцати символов", "Ok");
                return;
            }
            //ДАТА РОЖДЕНИЯ
            //else if (firstName.Length < 3 || firstName.Length > 15)
            //{
            //    await DisplayAlert("Ошибка", "Фамилия не может быть менее восьми", "Ok");
            //    return;
            //}
            else if (phone.Length != 11)
            {
                await DisplayAlert("Ошибка", "Телефон состоит из одиннадцати символов", "Ok");
                return;
            }
            else if (login.Length < 3 || login.Length > 15)
            {
                await DisplayAlert("Ошибка", "Логин не может быть менее трех и более пятнадцати символов", "Ok");
                return;
            }
            else if (password.Length < 8 || password.Length > 20)
            {
                await DisplayAlert("Ошибка", "Пароль не может быть менее восьми и более двадцати символов", "Ok");
                return;
            }



            // Создание нового объекта клиента
            Users newUser = new Users
            {
                Surname = lastName,
                Namee = firstName,
                DateOfBirth = data,
                Number = phone,
                Email = login,
                Passwordd = password
            };

            // Сохранение клиента в базе данных
            await App.Db.SaveUsers(newUser);

           
                await DisplayAlert("Ура", "Вы успешно зарегистрированы", "OK");
                await Navigation.PushAsync(new Authorization());
                       
        }

        private async void Authorization(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Authorization());
        }
      
    }
}