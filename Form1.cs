using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using DevExpress.Utils.Extensions;
using DevExpress.Utils.MVVM;

namespace MetaDataEditor
{
	public partial class Form1 : Form
	{
		dynamic databaseItems = new ExpandoObject();

		public List<MetaDataGridRow> metaDataRows;
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//string path = @"C:\Users\u6035303\git\api\api\snapshot\components\snapshotmeta\service\xml\tree_parts\d_adcdls.xml";
            var workDir = Directory.GetCurrentDirectory();
            var path = workDir + "..\\..\\..\\Data\\d_adcdls.xml";
            var metaData = new MetaData(path);
			metaData.Load();
			//gridControl1.BindingContext =(metaData.vals);
		}

		private void gridControl1_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click_1(object sender, EventArgs e)
		{

		}

		private void Load_Click(object sender, EventArgs e)
		{
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void Form1_Load_1(object sender, EventArgs e)
		{

		}

		private void button1_Click_2(object sender, EventArgs e)
		{
			databaseItems.Id = "1234234";
			
		}

		private void metaDataBindingSource_CurrentChanged(object sender, EventArgs e)
		{

		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var workDir = Directory.GetCurrentDirectory();
			var path = workDir + "..\\..\\..\\Data\\d_adcdls.xml";
			var metaData = new MetaData(path);
			metaData.Load();
			metaDataRows = metaData.Denormalize();
			dataGridView1.DataSource = metaDataRows;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void insertToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private class RowComparer : System.Collections.IComparer
		{
			private static int sortOrderModifier = 1;

			public RowComparer(SortOrder sortOrder)
			{
				if (sortOrder == SortOrder.Descending)
				{
					sortOrderModifier = -1;
				}
				else if (sortOrder == SortOrder.Ascending)
				{
					sortOrderModifier = 1;
				}
			}

			public int Compare(object x, object y)
			{
				DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
				DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;

				// Try to sort based on the Last Name column.
				int CompareResult = System.String.Compare(
					DataGridViewRow1.Cells[1].Value.ToString(),
					DataGridViewRow2.Cells[1].Value.ToString());

				// If the Last Names are equal, sort based on the First Name.
				if (CompareResult == 0)
				{
					CompareResult = System.String.Compare(
						DataGridViewRow1.Cells[0].Value.ToString(),
						DataGridViewRow2.Cells[0].Value.ToString());
				}
				return CompareResult * sortOrderModifier;
			}
		}

		private void sortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sortForm = new frmSort();

			List<string> colHeaders = new List<string>();
			foreach (DataGridViewColumn col in dataGridView1.Columns)
			{
				colHeaders.Add(col.HeaderText);
			}

			sortForm.Populate(colHeaders);
			var dlgRslt = sortForm.ShowDialog();
		}
	}
}
