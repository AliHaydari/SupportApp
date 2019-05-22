using System.Collections.Generic;
using SupportApp.Entities;

namespace SupportApp.Services.Contracts
{
    public interface ICategoryService
    {
        void AddNewCategory(Category category);
        IList<Category> GetAllCategories();
    }
}