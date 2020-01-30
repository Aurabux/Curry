using System;
using System.Collections.Generic;
using System.Text;

namespace Curry.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
       ICategoryRepository Category{ get;}
       IFoodTypeRepository FoodType { get;}
       IMenuItemRepository MenuItem { get; }
        void Save();
    }
}
