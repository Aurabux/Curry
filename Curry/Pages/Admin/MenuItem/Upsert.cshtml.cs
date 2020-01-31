using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curry.DataAccess.Data.Repository.IRepository;
using Curry.DataAccess.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Curry.Models.ViewModels;
using System.IO;

namespace Curry.Pages.Admin.MenuItem
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnviroment;
        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment )
        {
            _unitOfWork = unitOfWork;
            _hostEnviroment = hostEnvironment;
        }
        [BindProperty]
        public MenuItemVM MenuItemObj { get; set; }
        public IActionResult OnGet(int? id)
        {
            MenuItemObj = new MenuItemVM
            {
                CategoryList =
                _unitOfWork.Category.GetCategoryListForDropDown(),

                FoodTypeList =
                _unitOfWork.FoodType.GetFoodTypeListForDropDown(),

                MenuItem = new Models.MenuItem()
            };
            if (id!=null)
            {
                MenuItemObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if(MenuItemObj == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            string webRootPath = _hostEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(MenuItemObj.MenuItem.Id == 0)
            {
                string fileName = Guid.NewGuid().ToString();

                var uploads = Path.Combine(webRootPath, @"images\menuItems");

                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                MenuItemObj.MenuItem.Image = @"\images\menuItems\" + fileName + extension;
                _unitOfWork.MenuItem.Add(MenuItemObj.MenuItem);
            }
            else
            {
                var objFromDb =
                    _unitOfWork.MenuItem.Get(MenuItemObj.MenuItem.Id);
                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var uploads = Path.Combine(webRootPath, @"\images\menuItems\");

                    var extension = Path.GetExtension(files[0].FileName);

                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));
                    if(System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    MenuItemObj.MenuItem.Image = @"\images\menuItems\" + fileName + extension;

                }
                else
                {
                    MenuItemObj.MenuItem.Image = objFromDb.Image;
                }
                _unitOfWork.MenuItem.Update(MenuItemObj.MenuItem);
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}