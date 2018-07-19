using MyEconomy.Models;
using System.Collections.Generic;

namespace MyEconomy.Services
{
    interface IDataService
    {
        List<Category> GetCategories();
    }
}
