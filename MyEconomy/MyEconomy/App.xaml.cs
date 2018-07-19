using FreshMvvm;
using MyEconomy.PageModels;
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

            var tabbedNavigation = new FreshTabbedNavigationContainer(NavContainerName);
            tabbedNavigation.AddTab <CategoriesPageModel> ("Categories", null);
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
