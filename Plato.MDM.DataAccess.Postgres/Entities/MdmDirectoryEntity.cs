
namespace Plato.MDM.Storage.Models;

public partial class MdmDirectoryEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid? DirectoryDomainId { get; set; }
    public Guid? DirectoryLevelId { get; set; }
    public MdmDirectoryDomainEntity? DirectoryDomain { get; set; }
    public MdmDirectoryLevelEntity? DirectoryLevel { get; set; }
    public List<MdmDirectoryVersionEntity> MdmDirectoryVersions { get; set; } = [];
}
