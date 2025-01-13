using Microsoft.EntityFrameworkCore;
using Plato.MDM.Storage.Data;
using Plato.MDM.Storage.Models;

namespace Plato.MDM.Storage.Repositories
{
    public interface IMdmDirectoryLevelRepository
    {
        Task<List<MdmDirectoryLevelEntity>> GetAllLevelsAsync();
    }
    public class MdmDirectoryLevelRepository : IMdmDirectoryLevelRepository
    {
        private readonly MdmDbContext _context;

        public MdmDirectoryLevelRepository(MdmDbContext context) : base()
        {
            _context = context;
        }

        public async Task<List<MdmDirectoryLevelEntity>> GetAllLevelsAsync()
             => await _context.MdmDirectoryLevels.ToListAsync();
    }
}
