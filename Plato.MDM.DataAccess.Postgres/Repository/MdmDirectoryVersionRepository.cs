using Microsoft.EntityFrameworkCore;
using Plato.MDM.Storage.Data;
using Plato.MDM.Storage.Models;


namespace Plato.MDM.Storage.Repositories
{
    public interface IMdmDirectoryVersionRepository
    {
        Task<bool> AddVersionAsync(MdmDirectoryVersionEntity version);
        Task<bool> EditVersionAsync(MdmDirectoryVersionEntity updatedVersion);
        Task<bool> DeleteVersionAsync(Guid id);
        Task<List<MdmDirectoryVersionEntity>> GetAllVersionsAsync();
        Task<List<MdmDirectoryVersionEntity>> GetAllVersionsByDirectoryAsync(Guid directoryId);
        Task<MdmDirectoryVersionEntity> GetVersionByIdAsync(Guid id);
    }
    public class MdmDirectoryVersionRepository : IMdmDirectoryVersionRepository
    {
        private readonly MdmDbContext _context;

        public MdmDirectoryVersionRepository(MdmDbContext context) : base()
        {
            _context = context;
        }

        public async Task<bool> AddVersionAsync(MdmDirectoryVersionEntity varsion)
        {
            await _context.MdmDirectoryVersions.AddAsync(varsion);
            return await SaveAsync();
        }

        public async Task<bool> DeleteVersionAsync(Guid id)
        {
            var version = await _context.MdmDirectoryVersions.FindAsync(id);
            if (version == null) return false;

            _context.MdmDirectoryVersions.Remove(version);
            return await SaveAsync();
        }

        public async Task<bool> EditVersionAsync(MdmDirectoryVersionEntity updatedVersion)
        {
            var version = await _context.MdmDirectoryVersions.FirstOrDefaultAsync(v => v.Id == updatedVersion.Id);
            if (version == null) return false;

            _context.Entry(version).CurrentValues.SetValues(updatedVersion);
            return await SaveAsync();
        }

        public async Task<List<MdmDirectoryVersionEntity>> GetAllVersionsAsync() =>
            await _context.MdmDirectoryVersions.Include(x => x.Directory).ToListAsync();

        public async Task<List<MdmDirectoryVersionEntity>> GetAllVersionsByDirectoryAsync(Guid directoryId)
            => await _context.MdmDirectoryVersions.Where(d => d.DirectoryId == directoryId).ToListAsync();

        public Task<MdmDirectoryVersionEntity> GetVersionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
