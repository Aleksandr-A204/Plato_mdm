namespace Plato.MDM.Storage.Models;

public partial class MdmDirectoryVersionEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? DirectoryId { get; set; }

    public string? DataSourceName { get; set; }

    public string? DataSourceDate { get; set; }

    public string? DataSourceUrl { get; set; }

    public string Version { get; set; } = null!;

    public DateOnly? VersionDate { get; set; }

    public string? VersionDescription { get; set; }

    public string? TableName { get; set; }

    public MdmDirectoryEntity? Directory { get; set; }
}
