using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebSite.ViewComponents
{
    public class DepartmentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName, object model)
        {
            return await Task.FromResult(View(viewName, model));
        }
    }
}
