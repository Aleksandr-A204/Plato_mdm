namespace Plato.MDM.Storage.Models;

public partial class MdmDirectoryDomainEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public virtual List<MdmDirectoryEntity> MdmDirectories { get; set; } = new List<MdmDirectoryEntity>();
}
