using FreshMvvm;
using MyEconomy.PageModels;
using MyEconomy.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MyEconomy
{
	public partial class App : Application
	{
        public string NavContainerName { get; private set; } = "mainContainer";

		public App ()
		{
			InitializeComponent();

            FreshIOC.Container.Register<IDataService, DataServiceMock>();
            
            var tabbedNavigation = new FreshTabbedNavigationContainer(NavContainerName);
            tabbedNavigation.AddTab <CategoriesPageModel> ("Categories", null);
            tabbedNavigation.AddTab<PredictionPageModel>("Prediction", null);
            MainPage = tabbedNavigation;

            //MainPage = new MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
