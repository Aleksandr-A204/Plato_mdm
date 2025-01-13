using Microsoft.EntityFrameworkCore;
using Plato.MDM.Storage.Data;
using Plato.MDM.Storage.Models;

namespace Plato.MDM.Storage.Repositories
{
    public interface IMdmDirectoryDomainRepository
    {
        Task<List<MdmDirectoryDomainEntity>> GetAllDomainsAsync();
    }
    public class MdmDirectoryDomainRepository : IMdmDirectoryDomainRepository
    {
        private readonly MdmDbContext _context;

        public MdmDirectoryDomainRepository(MdmDbContext context) : base()
        {
            _context = context;
        }

        public async Task<List<MdmDirectoryDomainEntity>> GetAllDomainsAsync()
            => await _context.MdmDirectoryDomains.ToListAsync();
    }
}
