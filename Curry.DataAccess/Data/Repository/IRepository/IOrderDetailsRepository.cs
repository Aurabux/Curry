using System;
using System.Collections.Generic;
using System.Text;
using Curry.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Curry.DataAccess.Data.Repository.IRepository
{
    public interface IOrderDetailsRepository : IRepository<OrderDetails>
    {
       
        void Update(OrderDetails orderDetails);
    }

}
