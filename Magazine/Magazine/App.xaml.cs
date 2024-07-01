using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Magazine.Model;
using System.IO;
using Magazine;
using System.Collections.Generic;

namespace Magazine
{
    public partial class App : Application
    {
        private static DB db;
        public static Users CurrentUser { get; set; }
        public static DB Db 
        { 
            get
            {
                if (db == null)
                {
                    db = new DB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db.sqlite3"));
                }
                return db;
            }
        }
        public App()
        {
            InitializeComponent();
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db.sqlite3");
            App.db = new DB(databasePath);

            //MainPage.SetValue(ConverterProperty, new ByteArrayToImageSourceConverter());

            //!!! MainPage = new NavigationPage(new MainPage());
            MainPage = new NavigationPage(new MainAdmin());

        }

        // Метод для установки текущего пользователя
        public static void SetCurrentUser(Users user)
        {
            CurrentUser = user;
        }

        // Метод для очистки текущего пользователя (например, при выходе из системы)
        public static void ClearCurrentUser()
        {
            CurrentUser = null;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
