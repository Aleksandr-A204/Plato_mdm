using Plato.MDM.DataAccess.Postgres.Protos;

namespace Plato.MDM.Extensions
{
    public static class MdmServiceProvider
    {
        private static string grpcUrl = "http://localhost:5087";

        public static void AddMdmService(this IServiceCollection services)
        {
            services.AddGrpcClient<DirectoryService.DirectoryServiceClient>(options => options.Address = new Uri(grpcUrl));
            services.AddGrpcClient<DirectoryVersionService.DirectoryVersionServiceClient>(options => options.Address = new Uri(grpcUrl));
            services.AddGrpcClient<DirectoryLevelService.DirectoryLevelServiceClient>(options => options.Address = new Uri(grpcUrl));
            services.AddGrpcClient<DirectoryDomainService.DirectoryDomainServiceClient>(options => options.Address = new Uri(grpcUrl));
            services.AddGrpcClient<DirectoryDataService.DirectoryDataServiceClient>(options => options.Address = new Uri(grpcUrl));
        }
    }
}
