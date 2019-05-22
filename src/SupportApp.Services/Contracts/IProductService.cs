using System.Collections.Generic;
using SupportApp.Entities;

namespace SupportApp.Services.Contracts
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        IList<Product> GetAllProducts();
    }
}