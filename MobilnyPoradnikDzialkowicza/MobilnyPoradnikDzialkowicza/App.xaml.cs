using MobilnyPoradnikDzialkowicza.Model;
using MobilnyPoradnikDzialkowicza.Repository;
using MobilnyPoradnikDzialkowicza.Views;
using SQLite;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilnyPoradnikDzialkowicza
{

    public partial class App : Application
    {
        public static string DbPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InjectorStoreTest1.db");
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            try
            {
                VersionTracking.Track();
                var firstLaunch = VersionTracking.IsFirstLaunchEver;
                if (firstLaunch)
                {
                    var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                    using (Stream stream = assembly.GetManifestResourceStream("MobilnyPoradnikDzialkowicza.InjectorStoreTest1.db"))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            File.WriteAllBytes(DbPath, memoryStream.ToArray());
                        }
                    }
                }
                else { Console.WriteLine("Database is allright."); }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
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
