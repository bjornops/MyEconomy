using MyEconomy.Models;
using PropertyChanged;

namespace MyEconomy.UIModels
{
    [AddINotifyPropertyChangedInterface]
    public class CategoryListElement
    {
        public bool Selected { get; set; } = true;
        public Category Category { get; private set; }

        public CategoryListElement(Category category)
        {
            Category = category;
        }
    }
}