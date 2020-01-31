using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curry.DataAccess.Data.Repository.IRepository;
using Curry.DataAccess.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
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
        public Models.MenuItem MenuItemObj { get; set; }
        public IActionResult OnGet(int? id)
        {
            MenuItemObj = new Models.MenuItem();
            if (id!=null)
            {
                MenuItemObj = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if(MenuItemObj == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(MenuItemObj.Id == 0)
            {
                _unitOfWork.MenuItem.Add(MenuItemObj);
            }
            else
            {
                _unitOfWork.MenuItem.Update(MenuItemObj);
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}