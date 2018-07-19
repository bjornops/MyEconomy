using FreshMvvm;
using PropertyChanged;
using Xamarin.Forms;

namespace MyEconomy.PageModels
{
    [AddINotifyPropertyChangedInterface] // uses fody for property changed
    public class CategoriesPageModel : FreshBasePageModel
    {

        public CategoriesPageModel() // injected from IOC
        {
            LabelText = "Test";
        }

        public string LabelText { get; set; }

        public override void Init(object initData)
        {

        }

        // Methods are automatically wired up to page
        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            CoreMethods.DisplayAlert("Page is appearing", "", "Ok");
            base.ViewIsAppearing(sender, e);
        }

        protected override void ViewIsDisappearing(object sender, System.EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
        }

        // This is called when a page id pop'd
        public override void ReverseInit(object value)
        {

        }

        public Command AddQuote
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<CategoriesPageModel>();
                });
            }
        }

    }
}