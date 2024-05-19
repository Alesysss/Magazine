using Magazine.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Magazine
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

           
        }

        private async void Button_Start(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Registration1());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           

           
        }
    }
}
