using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace RazorPagesMovie.Pages.UserMains
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesMovieContext _dbContext;

        public CreateModel(RazorPagesMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserMain UserMain { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserMain == null)
            {
                return Redirect("../Error");
            }

            var oldModel = await _dbContext.UserMain.FirstOrDefaultAsync(x => x.UserLoginName == UserMain.UserLoginName);
            if (oldModel != null)
            {
                return Redirect("../Error");
            }
            
            UserMain.CreateTime = DateTime.Now;
            UserMain.UpdateTime = DateTime.Now;
            await _dbContext.UserMain.AddAsync(UserMain);
            await _dbContext.SaveChangesAsync();
            return Redirect("./Index");
        }
    }
}
