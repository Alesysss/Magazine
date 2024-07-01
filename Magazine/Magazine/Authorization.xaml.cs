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
	public partial class Authorization : ContentPage
	{
		public Authorization ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ToInitial(object sender, EventArgs e)
        {
            string login = Log.Text.Trim();
            string password = Pas.Text.Trim();
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            var db = App.Db;
            var user = await db.AuthenticateAsync(login, password);
            
            if (user != null||string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                if (login=="admin")
                {
                   // App.SetCurrentUser(user); // Установка текущего пользователя в приложении
                    await Navigation.PushAsync(new MainAdmin());
                }
                else {
                    App.SetCurrentUser(user); // Установка текущего пользователя в приложении
                    await Navigation.PushAsync(new MainTab()); 
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Пользователь не найден", "OK");
            }
        }


        private async void ToRegistration(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration1());
        }
    }
}