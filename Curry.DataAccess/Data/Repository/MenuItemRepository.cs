using Curry.DataAccess.Data.Repository.IRepository;
using Curry.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Curry.DataAccess.Data.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _db;

        public MenuItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuItem objectToBeUpdated)
        {
            var objFromDb = _db.MenuItem.FirstOrDefault(s => s.Id == objectToBeUpdated.Id);

            objFromDb.Name = objectToBeUpdated.Name;
            objFromDb.Price = objectToBeUpdated.Price;
            objFromDb.Desription = objectToBeUpdated.Desription;
            if(objectToBeUpdated.Image != null)
            {
                objFromDb.Image = objectToBeUpdated.Image;
            }
            objFromDb.Image = objectToBeUpdated.Image;
            objFromDb.CategoryId = objectToBeUpdated.CategoryId;
            objFromDb.Category = objectToBeUpdated.Category;
            objFromDb.FoodTypeId = objectToBeUpdated.FoodTypeId;
            objFromDb.FoodType = objectToBeUpdated.FoodType;

            _db.SaveChanges();


        }
    }
}
