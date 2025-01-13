using Plato.MDM.Storage.Repositories;

namespace Plato.MDM.DataAccess.Postgres.Mappers
{
    public static class MdmStorageServiceProvider
    {
        public static void AddStorageService(this IServiceCollection services)
        {
            services.AddScoped<IMdmDirectoryRepository, MdmDirectoryRepository>();
            services.AddScoped<IMdmDirectoryVersionRepository, MdmDirectoryVersionRepository>();
            services.AddScoped<IMdmDirectoryLevelRepository, MdmDirectoryLevelRepository>();
            services.AddScoped<IMdmDirectoryDomainRepository, MdmDirectoryDomainRepository>();
            services.AddScoped<IMdmDirectoryDataRepository, MdmDirectoryDataRepository>();

            services.AddScoped<DirectoryDataMapper>();
            services.AddScoped<DirectoryMapper>();
            services.AddScoped<DirectoryLevelMapper>();
            services.AddScoped<DirectoryDomainMapper>();
            services.AddScoped<DirectoryVersionMapper>();
        }
    }
}
