using FreshMvvm;
using MyEconomy.Models;
using MyEconomy.Services;
using PropertyChanged;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MyEconomy.PageModels
{
    [AddINotifyPropertyChangedInterface] // uses fody for property changed
    public class CategoriesPageModel : FreshBasePageModel
    {
        readonly IDataService _dataService;

        public CategoriesPageModel(IDataService dataService)
        {
             _dataService = dataService;
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

        public Command AddNewTransactionCommand
        {
            get
            {
                return new Command<string>(
                    (category) =>
                {
                    Transaction t = new Transaction(200, DateTime.UtcNow, "TestTransaction");
                    AddNewTransaction(Categories[0], t);
                    //await CoreMethods.PushNewNavigationServiceModal();
                });
            }
        }

        public void AddNewTransaction(Category category, Transaction transaction)
        {
            category.AddTransaction(transaction);
        }

    }
}