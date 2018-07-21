using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyEconomy.Models;

namespace Tests
{
    [TestClass]
    public class CategoryUnitTests
    {
        [TestMethod]
        public void InstantiateCategory()
        {
            string title = "Title";
            string description = "Description";
            Category category = new Category(title, description);

            Assert.AreEqual(title, category.Title);
        }
    }
}
