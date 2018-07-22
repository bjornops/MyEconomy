using FreshMvvm;
using MyEconomy.Models;
using MyEconomy.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MyEconomy.PageModels
{
    [AddINotifyPropertyChangedInterface] // uses fody for property changed
    public class PredictionPageModel : FreshBasePageModel
    {
        readonly IDataService _dataService = new DataServiceMock(); // Todo inject

        public PredictionPageModel() // injected from IOC
        {

        }

        public PlotModel PredictionPlotModel { get; private set; }

        public override void Init(object initData)
        {
            ApplyDefaultPlotModel();
        }

        // Methods are automatically wired up to page
        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
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

        private void ApplyDefaultPlotModel()
        {
            List<Category> categories = _dataService.GetCategories();
            Category category = categories[0];

            DateTime earliestDate = DateTime.MaxValue, latestDate = DateTime.MinValue;

            foreach(Transaction transaction in category.Transactions)
            {
                if (transaction.Date < earliestDate)
                    earliestDate = transaction.Date;
                if (transaction.Date > latestDate)
                    latestDate = transaction.Date;
            }

            PredictionPlotModel = GenerateSimpleModel(category, earliestDate, latestDate);
        }

        private PlotModel GenerateSimpleModel(Category category, DateTime startDate, DateTime endDate)
        {
            PlotModel plotModel = new PlotModel { Title = category.Title };

            var minValue = DateTimeAxis.ToDouble(startDate);
            var maxValue = DateTimeAxis.ToDouble(endDate);

            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "MMM" });

            plotModel.Series.Add(GenerateLineSeries(category));

            return plotModel;
        }

        private LineSeries GenerateLineSeries(Category category)
        {
            LineSeries lineSeries = new LineSeries();

            foreach(Transaction transaction in category.Transactions)
            {
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(transaction.Date), transaction.Amount));
            }

            return lineSeries;
        }

    }
}