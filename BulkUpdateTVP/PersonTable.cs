using System;
using System.Data;

namespace BulkUpdateTVP
{
    public class PersonTable : DataTable
    {

        public const string DBTypeName = "Person";

        public PersonTable()
        {
            this.TableName = this.GetType().Name;
            this.Columns.Add("PersonID", typeof(Int32)).AllowDBNull = false;
            this.Columns.Add("LastName", typeof(String)).AllowDBNull = false;
            this.Columns.Add("FirstName", typeof(String)).AllowDBNull = false;
            this.Columns.Add("HireDate", typeof(DateTime)).AllowDBNull = true;
            this.Columns.Add("EnrollmentDate", typeof(DateTime)).AllowDBNull = true;
            this.Columns.Add("Discriminator", typeof(String)).AllowDBNull = false;
            this.Columns.Add("DataVersion", typeof(Byte[])).AllowDBNull = false;
            this.Columns.Add("Created", typeof(DateTime)).AllowDBNull = true;
            this.Columns.Add("Updated", typeof(DateTime)).AllowDBNull = true;
        }

        protected override Type GetRowType()
        {
            return typeof(PersonTableRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new PersonTableRow(builder);
        }

        public PersonTableRow this[int rowIndex]
        {
            get { return (PersonTableRow)Rows[rowIndex]; }
        }

        public void AddRow(PersonTableRow row)
        {
            Rows.Add(row);
        }

        public new PersonTableRow NewRow()
        {
            return (PersonTableRow)base.NewRow();
        }
    }

    public class PersonTableRow : DataRow
    {

        public PersonTableRow(DataRowBuilder rowBuilder) : base(rowBuilder) { }

        public Int32 PersonID { get => (Int32)this["PersonID"]; set => this["PersonID"] = value; }

        public String LastName { get => (String)this["LastName"]; set => this["LastName"] = value; }

        public String FirstName { get => (String)this["FirstName"]; set => this["FirstName"] = value; }

        public DateTime? HireDate { get => this["HireDate"] is DBNull ? null : (DateTime?)this["HireDate"]; set => this["HireDate"] = value == null ? DBNull.Value : (object)value; }

        public DateTime? EnrollmentDate { get => this["EnrollmentDate"] is DBNull ? null : (DateTime?)this["EnrollmentDate"]; set => this["EnrollmentDate"] = value == null ? DBNull.Value : (object)value; }

        public String Discriminator { get => (String)this["Discriminator"]; set => this["Discriminator"] = value; }

        public Byte[] DataVersion { get => (Byte[])this["DataVersion"]; set => this["DataVersion"] = value; }

        public DateTime? Created { get => this["Created"] is DBNull ? null : (DateTime?)this["Created"]; set => this["Created"] = value == null ? DBNull.Value : (object)value; }

        public DateTime? Updated { get => this["Updated"] is DBNull ? null : (DateTime?)this["Updated"]; set => this["Updated"] = value == null ? DBNull.Value : (object)value; }

    }
}
