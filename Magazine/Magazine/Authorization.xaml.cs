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
            string login = Log.Text.Trim(); // Замените на логин, введенный пользователем
            string password = Pas.Text.Trim(); // Замените на пароль, введенный пользователем

            var db = App.Db;
            var user = await db.AuthenticateAsync(login, password);

            if (user != null)
            {
              
                await Navigation.PushAsync(new MainTab());
            }
            else
            {
                await DisplayAlert("Ошибка", "Сообщение об ошибке", "OK");
            }
            
        }

        private async void ToRegistration(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registration1());
        }
    }
}