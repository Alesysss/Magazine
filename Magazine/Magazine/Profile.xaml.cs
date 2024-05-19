using Magazine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace Magazine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        private Users users;
        public Profile()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // Параметризованный конструктор
        public Profile(int usersId) : this() // Вызывает конструктор по умолчанию
        {
            LoadUserDataAsync(usersId);
        }

        private async Task LoadUserDataAsync(int usersId)
        {
            var DB = App.Db;
            this.users = await DB.GetUsersAsync(usersId); // Используйте поле класса, а не локальную переменную

            if (this.users != null)
            {
                Surn.Text = this.users.Surname;
                Name.Text = this.users.Namee;

                DateTime dateOfBirth;
                bool success = DateTime.TryParse(this.users.DateOfBirth, out dateOfBirth);
                if (success)
                {
                    Dat.Date = dateOfBirth;
                }
                else
                {
                    // Обработка случая, если строка не является корректной датой
                    await DisplayAlert("Ошибка", "Неверный формат даты рождения", "OK");
                }

                Tel.Text = this.users.Number;
            }
            else
            {
                await DisplayAlert("Ошибка", "Сообщение об ошибке", "OK");
            }
        }

        private async void ToNotifications(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Notifications());
        }
        private async void Bye(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}