using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstract
{
    public interface IProductDal:IGenericDal<Product>
    {
        List<Product> GetProducsWithCategories();
        int ProductCount();
        int ProductCountByCategoryNameHamburger();
        int ProductCountByCategoryNameDrink();
        decimal productPriceAvg();
        string ProductPriceByMaxPrice();
        string ProductPriceByMinPrice();
        decimal ProductAvgPriceByHamburger();
        List<Product> GetLast9Products();


	}
}
