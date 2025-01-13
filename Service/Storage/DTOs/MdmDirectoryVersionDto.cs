namespace Plato.MDM.Storage.DTOs
{
    public class MdmDirectoryVersionDto
    {
        public Guid Id { get; set; }
        public Guid? DirectoryId { get; set; }
        public string? DataSourceName { get; set; }
        public string? DataSourceDate { get; set; }
        public string? DataSourceUrl { get; set; }
        public string Version { get; set; } = null!;
        public string? VersionDate { get; set; }
        public string? VersionDescription { get; set; }
        public string? TableName { get; set; }
    }
}
