using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BulkUpdateTVP
{
    /// <summary>
    /// Represnts base implementation Data adapter class specific to Bulk operations.
    /// </summary>
    public abstract class BulkDataAdapter
    {
        #region fields

#if DEBUG
        private StringBuilder sqlPrintLog = null;
#endif

        #endregion

        /*

        #region constructor

        protected BulkDataAdapter(Storage store)
        {
            DbStorage = store;
        }

        protected BulkDataAdapter():this(Session.Current.CreateStorage())
        {
        }
        protected BulkDataAdapter(Storage store, int timeout) : this(store)
        {
            this.SqlTimeout = timeout;
        }

        #endregion
        */
        #region properties

        public int SqlTimeout { get; set; }

        protected SqlCommand UpdateCommand { get; set; }

        protected DataTable Table { get; private set; }

        #endregion

        #region implemented methods

        /*
        public virtual int BulkUpdate<T>(IList<T> objectList)
        {
            return this.BulkUpdate(objectList.ToDataTable());
        }
        */
        /// <summary>
        /// Update data of datatable to database.
        /// </summary>
        /// <param name="table">input Datatable</param>
        /// <returns></returns>
        public virtual int BulkUpdate(DataTable table)
        {
            var rowsAffected = 0;
            var isConnectionClose = false;
            Table = table;
            BuildSqlCommand();
            //DbStorage.UpdateConnection(UpdateCommand);

            // Following code will grab the print messages from SP execution and dump into sbLog variable. This can be used for debugging purposes.
#if DEBUG
            sqlPrintLog = new StringBuilder();
            UpdateCommand.Connection.InfoMessage += (object obj, SqlInfoMessageEventArgs e) =>
            {
                sqlPrintLog.AppendLine(e.Message);
            };
#endif

            try
            {
                if (UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    UpdateCommand.Connection.Open();
                    isConnectionClose = true;
                }

                rowsAffected = UpdateCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new System.Exception($"{UpdateCommand.CommandText}: {ex.Message}", ex);
            }
            finally
            {
                if (isConnectionClose)
                {
                    UpdateCommand.Connection.Close();
                }
            }

            return rowsAffected;
        }

        // the return of datatable should be handled in store procedure
        public virtual DataTable BulkUpdateAndReturnDataTable(DataTable table)
        {
            Table = table;
            var isConnectionClose = false;
            BuildSqlCommand();
            //DbStorage.UpdateConnection(this.UpdateCommand);
            var resultTable = new DataTable();
            try
            {
                if (UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    UpdateCommand.Connection.Open();
                    isConnectionClose = true;
                }

                using (var da = new SqlDataAdapter(UpdateCommand))
                {
                    try
                    {
                        da.Fill(resultTable);
                    }
                    catch (System.Exception ex)
                    {
                        throw new System.Exception($"{UpdateCommand.CommandText}: {ex.Message}", ex);
                    }
                }
            }
            finally
            {
                if (isConnectionClose)
                {
                    UpdateCommand.Connection.Close();
                }
            }

            return resultTable;
        }

        public virtual void BulkUpdateWithReadback(DataTable table)
        {
            Table = table;
            var isConnectionClose = false;
            BuildSqlCommand();
            //DbStorage.UpdateConnection(this.UpdateCommand);
            var resultTable = new DataTable();
            try
            {
                if (UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    UpdateCommand.Connection.Open();
                    isConnectionClose = true;
                }

                using (var da = new SqlDataAdapter(UpdateCommand))
                {
                    try
                    {
                        da.Fill(resultTable);
                    }
                    catch (System.Exception ex)
                    {
                        throw new System.Exception($"{UpdateCommand.CommandText}: {ex.Message}", ex);
                    }
                }
            }
            finally
            {
                if (isConnectionClose)
                {
                    UpdateCommand.Connection.Close();
                }
            }

            var rowsTobeDeleted = Table.Rows.Cast<DataRow>().Where(r => r.RowState == DataRowState.Added).ToArray();

            foreach (var r in rowsTobeDeleted)
            {
                r.Delete();
            }
            Table.AcceptChanges();

            Table.PrimaryKey = new DataColumn[] { Table.Columns[0] };

            resultTable.PrimaryKey = new DataColumn[] { resultTable.Columns[0] };

            Table.Merge(resultTable, false);

            Table.AcceptChanges();
        }
        #endregion

        #region abstract methods

        protected abstract void BuildSqlCommand();

        #endregion
    }
}
