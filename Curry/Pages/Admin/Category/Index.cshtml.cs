using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curry.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//CATEGOERY INDEXCSHARP
namespace Curry
{
    
    [Authorize(Roles = SD.ManagerRole)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}