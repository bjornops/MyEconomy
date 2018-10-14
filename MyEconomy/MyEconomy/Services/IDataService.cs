using MyEconomy.Models;
using System.Collections.Generic;

namespace MyEconomy.Services
{
    public interface IDataService
    {
        List<Category> GetCategories();
    }
}
