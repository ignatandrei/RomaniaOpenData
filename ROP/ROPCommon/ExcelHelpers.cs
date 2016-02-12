using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROPCommon
{
    public class ExcelHelpers
    {
        public static string BuildExcelConnectionString(string Filename, bool FirstRowContainsHeaders)
        {
            return string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = '{0}'; Extended Properties =\"Excel 8.0;HDR={1};\"",
        Filename.Replace("'", "''"), FirstRowContainsHeaders ? "Yes" : "No");
        }

        public static string BuildExcel2007ConnectionString(string Filename, bool FirstRowContainsHeaders)
        {
            return string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source ={ 0}; Extended Properties =\"Excel 12.0;HDR={1}\";",
        Filename.Replace("'", "''"), FirstRowContainsHeaders ? "Yes" : "No");

        }
        public static async Task<long> NrRows(string pathToExcel,string SheetName= "[Sheet1$]")
        {
            using (var m = new OleDbConnection())
            {
                m.ConnectionString = ExcelHelpers.BuildExcelConnectionString(pathToExcel, false);
                await m.OpenAsync();
                var query = @"Select count(*) From " + SheetName;
                using (var cmd = new OleDbCommand(query, m))
                {
                    var dr = await cmd.ExecuteScalarAsync();
                    return long.Parse(dr.ToString());
                    
                }

            }
        }

    }
}
