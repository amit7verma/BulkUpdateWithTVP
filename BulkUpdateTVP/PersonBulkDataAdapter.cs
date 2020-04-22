using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BulkUpdateTVP
{
    public class PersonBulkDataAdapter : BulkDataAdapter
    {
        public PersonBulkDataAdapter() : base()
        {
        }

        protected override void BuildSqlCommand()
        {
            var table = Table.Clone();
            table.Columns.Add("DataState", typeof(string));
            foreach (var r in Table.Rows.Cast<DataRow>())
            {
                var newRow = table.NewRow();
                if (r.RowState != DataRowState.Deleted)
                {
                   
                    newRow.ItemArray = r.ItemArray;
                    newRow["DataState"] = r.RowState.ToString();
                }
                else
                {
                    for (int i = 0; i < r.Table.Columns.Count; i++)
                    {
                        newRow[i] = r[i, DataRowVersion.Original];
                    }
                    newRow["DataState"] = r.RowState.ToString();
                }
                table.Rows.Add(newRow);

            }

            UpdateCommand = new SqlCommand
            {
                Connection = new SqlConnection(@"Data Source=.\sql16; Initial Catalog = School; Integrated Security = true"),
                CommandText = "BulkUpdatePerson",
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandTimeout = SqlTimeout
            };
            var tableParam = UpdateCommand.Parameters.AddWithValue("@inputData", table);
            tableParam.SqlDbType = System.Data.SqlDbType.Structured;
        }

        public void FillData(PersonTable table)
        {
            using (var conn = new SqlConnection(@"Data Source=.\sql16; Initial Catalog = School; Integrated Security = true"))
            {
                using (var cmd = new SqlCommand("select * from dbo.Person", conn))
                {
                    var da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                }
            }
        }
    }
}
