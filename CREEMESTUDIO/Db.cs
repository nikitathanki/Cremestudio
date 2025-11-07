using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace CREEMESTUDIO
{
    public class Db
    {
        public static SqlConnection GetConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["CremeStudioDb"];
            if (cs == null) throw new Exception("Missing connection string: CremeStudioDb");
            return new SqlConnection(cs.ConnectionString);
        }

        // Optional helper used by a few pages (e.g., ProductDetails you pasted earlier)
        public static DataTable Query(string sql, params SqlParameter[] prms)
        {
            using (var con = GetConnection())
            using (var cmd = new SqlCommand(sql, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (prms != null) cmd.Parameters.AddRange(prms);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}