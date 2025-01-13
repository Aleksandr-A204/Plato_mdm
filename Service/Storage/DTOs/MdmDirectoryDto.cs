namespace Plato.MDM.Storage.DTOs
{
    public class MdmDirectoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? DirectoryDomainId { get; set; }
        public Guid? DirectoryLevelId { get; set; }
        public MdmDirectoryDomainDto? DirectoryDomain { get; set; }
        public MdmDirectoryLevelDto? DirectoryLevel { get; set; }
    }
}
