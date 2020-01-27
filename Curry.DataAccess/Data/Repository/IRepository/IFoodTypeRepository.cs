using System;
using System.Collections.Generic;
using System.Text;
using Curry.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Curry.DataAccess.Data.Repository.IRepository
{
    public interface IFoodTypeRepository : IRepository<FoodType>
    {
        IEnumerable<SelectListItem> GetFoodTypeListForDropDown();

        void Update(FoodType foodType);
    }
}
