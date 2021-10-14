using System.Linq;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Data
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly DataContext _context;



        public TemplateRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<Templates> GetTemplates()
        {
            return await _context.Templates.Where(t => t.Id == 1).FirstOrDefaultAsync();
        }
    }
}
