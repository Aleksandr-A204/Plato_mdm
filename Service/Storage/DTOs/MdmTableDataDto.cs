using Newtonsoft.Json.Linq;

namespace Plato.MDM.Storage.DTOs
{
    public class MdmTableDataDto
    {
        public required string TableName { get; set; }
        public required JArray MainTable { get; set; }
        public Dictionary<string, JArray> ForeignTables { get; set; } = new Dictionary<string, JArray>();
    }
}
