using System;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using NULPSchedule.IoC;
using NULPSchedule.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Montserrat.ttf", Alias = "Montserrat")]
namespace NULPSchedule
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            InitializeDependencies();
            MainPage = new AppShell();
        }
        protected void InitializeDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CrossplatformModule());

            var locator = new AutofacServiceLocator(builder.Build());
            ServiceLocator.SetLocatorProvider(() => locator);
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
