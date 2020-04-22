using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BulkUpdateTVP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            personData = new PersonTable();
                        
            personData.RowChanged += PersonData_RowChanged;
            personData.RowDeleted += PersonData_RowChanged;
            dataGridView1.DataSource = personData;

            lblRowCount.Text = "";
        }
        private int changeCount = 0;
        private void PersonData_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            changeCount++;
            btnSave.Text = $"Save({changeCount})";
        }

        PersonTable personData = null;

        private void btlClear_Click(object sender, EventArgs e)
        {
            personData.RowChanged -= PersonData_RowChanged;
            personData.RowDeleted -= PersonData_RowChanged;
            personData.Clear();
            personData.AcceptChanges();
            changeCount = 0;
            btnSave.Text = $"Save";

            personData.RowChanged += PersonData_RowChanged;
            personData.RowDeleted += PersonData_RowChanged;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            PersonBulkDataAdapter da = new PersonBulkDataAdapter();
            da.FillData(personData);
            changeCount = 0;
            btnSave.Text = $"Save";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PersonBulkDataAdapter da = new PersonBulkDataAdapter();
            da.BulkUpdateWithReadback(personData);
            changeCount = 0;
            btnSave.Text = $"Save";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}
