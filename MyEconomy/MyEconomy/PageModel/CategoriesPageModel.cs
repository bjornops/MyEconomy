using FreshMvvm;
using MyEconomy.Models;
using MyEconomy.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MyEconomy.PageModels
{
    [AddINotifyPropertyChangedInterface] // uses fody for property changed
    public class CategoriesPageModel : FreshBasePageModel
    {
        readonly IDataService _dataService;

        public CategoriesPageModel() // injected from IOC
        {
             _dataService = new DataServiceMock(); // Todo inject
            LabelText = "Label";
        }

        public string LabelText { get; set; }
        public ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();

        public override void Init(object initData)
        {

        }

        // Methods are automatically wired up to page
        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            PopulateCategoryList(_dataService.GetCategories());
        }

        public void PopulateCategoryList(List<Category> categories)
        {
            foreach (Category category in categories)
                Categories.Add(category);
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