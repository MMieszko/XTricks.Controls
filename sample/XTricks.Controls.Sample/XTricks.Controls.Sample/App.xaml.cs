using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XTricks.Controls.Sample.Services;
using XTricks.Controls.Sample.Views;

namespace XTricks.Controls.Sample
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
