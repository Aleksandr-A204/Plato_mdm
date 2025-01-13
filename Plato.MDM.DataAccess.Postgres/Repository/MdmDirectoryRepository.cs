using Microsoft.EntityFrameworkCore;
using Plato.MDM.Storage.Data;
using Plato.MDM.Storage.Models;
using System.Data.Common;

namespace Plato.MDM.Storage.Repositories
{
    public interface IMdmDirectoryRepository
    {
        Task<bool> CreateDirectoryAsync(MdmDirectoryEntity directory);
        Task<bool> UpdateDirectoryAsync(MdmDirectoryEntity updatedDirectory);
        Task<bool> DeleteDirectoryAsync(Guid id);
        Task<List<MdmDirectoryEntity>> GetAllDirectoriesAsync();
        // Task<List<MdmDirectoryVersionEntity>> GetAllVersionsByDirectoryAsync(Guid directoryId);
        Task<MdmDirectoryEntity> GetDirectoryByIdAsync(Guid id);
    }
    public class MdmDirectoryRepository : IMdmDirectoryRepository
    {
        private readonly MdmDbContext _context;

        public MdmDirectoryRepository(MdmDbContext context) : base()
        {
            _context = context;
        }

        public async Task<bool> CreateDirectoryAsync(MdmDirectoryEntity directory)
        {
            await _context.MdmDirectories.AddAsync(directory);
            return await SaveAsync();
        }

        public async Task<bool> DeleteDirectoryAsync(Guid id)
        {
            var directory = await _context.MdmDirectories.FindAsync(id);
            if (directory == null) return false;

            _context.MdmDirectories.Remove(directory);
            return await SaveAsync();
        }

        public async Task<bool> UpdateDirectoryAsync(MdmDirectoryEntity updatedDirectory)
        {
            var directory = await _context.MdmDirectories.FirstOrDefaultAsync(d => d.Id == updatedDirectory.Id);
            if (directory == null) return false;

            _context.Entry(directory).CurrentValues.SetValues(updatedDirectory);
            return await SaveAsync();
        }

        public async Task<List<MdmDirectoryEntity>> GetAllDirectoriesAsync()
            => await _context.MdmDirectories.ToListAsync();

        //public async Task<List<MdmDirectoryVersionEntity>> GetAllVersionsByDirectoryAsync(Guid directoryId)
        //    => await _context.MdmDirectoryVersions.Include(x => x.Directory).ToListAsync();

        public async Task<bool> SaveAsync()
            => await _context.SaveChangesAsync() > 0;

        public async Task<MdmDirectoryEntity> GetDirectoryByIdAsync(Guid id) 
            => await _context.MdmDirectories.FirstOrDefaultAsync(x => x.Id == id) ?? new MdmDirectoryEntity();
    }
}
