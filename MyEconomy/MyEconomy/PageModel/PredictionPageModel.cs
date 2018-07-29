using FreshMvvm;
using MyEconomy.Models;
using MyEconomy.Services;
using MyEconomy.UIModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyEconomy.PageModels
{
    [AddINotifyPropertyChangedInterface] // uses fody for property changed
    public class PredictionPageModel : FreshBasePageModel
    {
        readonly IDataService _dataService;

        public PredictionPageModel() // injected from IOC
        {
            _dataService = new DataServiceMock(); // Todo inject
        }

        public ObservableCollection<CategoryListElement> Categories { get; set; } = new ObservableCollection<CategoryListElement>();
        public PlotModel PredictionPlotModel { get; set; } = new PlotModel();
        public double CurrentBalance { get; set; } = 0;
        public DateTime FromDate { get; set; } = DateTime.UtcNow;
        public DateTime ToDate { get; set; } = DateTime.UtcNow.AddDays(90);

        public override void Init(object initData)
        {
            
        }

        // Methods are automatically wired up to page
        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            PopulateCategoryCollection(_dataService.GetCategories());
            UpdateGraph();
            //ApplyPlotModelToGraph(GenerateBalancePlotModel(Categories, CurrentBalance, FromDate, ToDate));
        }

        private void PopulateCategoryCollection(List<Category> categories)
        {
            foreach (Category category in categories)
            {
                Categories.Add(new CategoryListElement(category));
            }
        }

        protected override void ViewIsDisappearing(object sender, System.EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
        }

        // This is called when a page id pop'd
        public override void ReverseInit(object value)
        {

        }

        public Command RefreshGraph
        {
            get
            {
                return new Command( () =>
                {
                    UpdateGraph();
                });
            }
        }

        private async void UpdateGraph()
        {
            PredictionPlotModel.Series.Clear();
            PredictionPlotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(FromDate),
                Maximum = DateTimeAxis.ToDouble(ToDate),
                StringFormat = "MMM"
            });
            List<CategoryListElement> selectedElements = GetSelectedCategoryListElements();
            foreach (CategoryListElement element in selectedElements)
                PredictionPlotModel.Series.Add(GenerateLineSeries(element.Category));
            PredictionPlotModel.InvalidatePlot(false);
        }

        private List<CategoryListElement> GetSelectedCategoryListElements()
        {
            List<CategoryListElement> categoryListElements = new List<CategoryListElement>();
            foreach(CategoryListElement element in Categories)
            {
                if (element.Selected)
                    categoryListElements.Add(element);
            }
            return categoryListElements;
        }

        private void ApplyDefaultPlotModel()
        {
            Category category = Categories[0].Category;

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

        private PlotModel GenerateBalancePlotModel(ObservableCollection<CategoryListElement> categories, double currentBalance, DateTime fromDate, DateTime toDate)
        {
            PlotModel plotModel = new PlotModel { Title = "Predicted balance" };

            var minValue = DateTimeAxis.ToDouble(fromDate);
            var maxValue = DateTimeAxis.ToDouble(toDate);

            plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "MMM" });

            plotModel.Series.Add(GenerateBalanceLineSeries(categories, currentBalance));

            return plotModel;
        }

        private LineSeries GenerateBalanceLineSeries(ObservableCollection<CategoryListElement> categories, double currentBalance)
        {
            LineSeries lineSeries = new LineSeries();

            List<Transaction> mergedCategoryTransactions = MergeCategories(categories);

            List<Transaction> transactionList = FilterTransactionPeriod(mergedCategoryTransactions, DateTime.UtcNow, new DateTime(2100, 01, 01));
            transactionList.Sort((x, y) => x.Date.CompareTo(y.Date));

            List<DataPoint> dateSummedTransactions = GetDateSummedDataPoints(transactionList);

            List<DataPoint> accumulatedDailySum = AccumulateDataPoints(dateSummedTransactions, currentBalance);

            foreach (DataPoint dataPoint in accumulatedDailySum)
            {
                lineSeries.Points.Add(dataPoint);
            }

            return lineSeries;
        }

        private List<Transaction> FilterTransactionPeriod(List<Transaction> transactions, DateTime fromDate, DateTime toDate)
        {
            List<Transaction> filteredTransactions = new List<Transaction>();

            foreach(Transaction transaction in transactions)
            {
                if (transaction.Date.Date >= fromDate.Date && transaction.Date.Date <= toDate.Date)
                    filteredTransactions.Add(transaction);
            }
            return filteredTransactions;
        }

        private List<Transaction> MergeCategories(ObservableCollection<CategoryListElement> categories)
        {
            List<Transaction> mergedCategoryTransactions = new List<Transaction>();

            foreach (CategoryListElement category in categories)
            {
                mergedCategoryTransactions.AddRange(category.Category.Transactions);
            }

            return mergedCategoryTransactions;
        }

        private bool IsSameDate(DateTime d1, DateTime d2)
        {
            return (d1.Date == d2.Date);
        }

        private List<DataPoint> AccumulateDataPoints(List<DataPoint> dataPoints, double initialValue = 0)
        {
            List<DataPoint> accumulatedDataPoints = new List<DataPoint>();
            double accumulatedValue = initialValue;
            
            foreach(DataPoint dataPoint in dataPoints)
            {
                accumulatedValue += dataPoint.Y;
                accumulatedDataPoints.Add(new DataPoint(dataPoint.X, accumulatedValue));
            }

            return accumulatedDataPoints;
        }

        private List<DataPoint> GetDateSummedDataPoints(List<Transaction> transactions)
        {
            transactions.Sort((x, y) => x.Date.CompareTo(y.Date));
            List<DataPoint> dailyGroupedDataPoints = new List<DataPoint>();

            double dailySum = 0;
            DateTime dateIterator = transactions[0].Date;

            foreach (Transaction transaction in transactions)
            {
                if (IsSameDate(transaction.Date, dateIterator))
                    dailySum += transaction.Amount;
                else
                {
                    dailyGroupedDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dateIterator), dailySum));
                    dateIterator = transaction.Date;
                    dailySum = transaction.Amount;
                }
                if(transaction == transactions[transactions.Count - 1])
                    dailyGroupedDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(dateIterator), dailySum));

            }

            return dailyGroupedDataPoints;
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