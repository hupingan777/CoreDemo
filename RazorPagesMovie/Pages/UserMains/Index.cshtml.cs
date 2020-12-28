using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.UserMains
{
    public class IndexModel : PageModel
    {
        public IList<UserMain> UserMain { get; set; }

        private readonly RazorPagesMovieContext _dbContext;

        public IndexModel(RazorPagesMovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            UserMain = await _dbContext.UserMain.ToListAsync();
        }
    }
}
