namespace Plato.MDM.Storage.DTOs
{
    public class DeleteMdmTableDataRequest
    {
        public required string Tablename { get; set; }
        public required List<Guid> Ids { get; set; }
    }
}
