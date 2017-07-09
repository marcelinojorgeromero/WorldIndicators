using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.Utils
{
    public class DataTable<T> where T : class
    {
        public static string SerializeToJson(int sEcho, int totalRow, int totalFilter, T objectGeneric)
        {
            var dataResult = new StringBuilder();
            var dataSerialize = JsonConvert.SerializeObject(objectGeneric, Formatting.None);
            dataResult.Append("{");
            dataResult.Append($"\"sEcho\": {sEcho},");
            dataResult.Append($"\"iTotalRecords\": {totalRow},");
            dataResult.Append($"\"iTotalDisplayRecords\": {totalFilter},");
            dataResult.Append($"\"aaData\": {dataSerialize}");
            dataResult.Append("}");

            return dataResult.ToString();
        }
    }
}
