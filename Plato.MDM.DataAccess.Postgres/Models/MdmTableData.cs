using Newtonsoft.Json.Linq;

namespace Plato.MDM.Storage.DTOs
{
    public class MdmTableData
    {
        public string TableName { get; set; } = null!;
        public JArray MainTable { get; set; } = null!;
        public Dictionary<string, JArray> ForeignTables { get; set; } = new Dictionary<string, JArray>();
    }
}
