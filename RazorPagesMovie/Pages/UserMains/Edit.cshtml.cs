using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.UserMains
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesMovieContext _dbContext;

        public EditModel(RazorPagesMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public UserMain UserMain { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id,string userName)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserMain = await _dbContext.UserMain.FirstOrDefaultAsync(x => x.Id == id);

            if (UserMain == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// 命名规范：On+Get/Post/Delete+Hanlder （Hanlder为自定义名称）
        /// 前端请求方式:url为:/UserMains/Edit?Hanlder=CreateTest
        /// 具体操作可以参考这篇博客：https://www.jb51.net/article/133437.htm
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostCreateTest(string UserName)
        {
            var list = await _dbContext.UserMain.ToListAsync();
            return new JsonResult(list);
        }
    }
}
