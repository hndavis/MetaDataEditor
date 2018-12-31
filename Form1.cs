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
using System.Reflection;


namespace MetaDataEditor
{
	public partial class Form1 : Form
	{
		dynamic databaseItems = new ExpandoObject();

		public List<MetaDataGridRow> metaDataRows;

		private MetaMetaData metaMetaData;
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
			private List<frmSort.SortItem> sortItems;
			public RowComparer(SortOrder sortOrder,  List<frmSort.SortItem> sortItems)
			{
				this.sortItems = sortItems;

				if (sortOrder == SortOrder.Descending)
				{
					sortOrderModifier = -1;
				}
				else if (sortOrder == SortOrder.Ascending)
				{
					sortOrderModifier = 1;
				}
			}

			//public int Compare(object x, object y)
			//{
			//	DataGridViewRow DataGridViewRow1 = (DataGridViewRow) x;
			//	DataGridViewRow DataGridViewRow2 = (DataGridViewRow) y;

			//	// Try to sort based on the Last Name column.
			//	int CompareResult = System.String.Compare(
			//		DataGridViewRow1.Cells[1].Value.ToString(),
			//		DataGridViewRow2.Cells[1].Value.ToString());

			//	// If the Last Names are equal, sort based on the First Name.
			//	if (CompareResult == 0)
			//	{
			//		CompareResult = System.String.Compare(
			//			DataGridViewRow1.Cells[0].Value.ToString(),
			//			DataGridViewRow2.Cells[0].Value.ToString());
			//	}

			//	return CompareResult * sortOrderModifier;
			//}
			public int Compare(object x, object y)
			{

				MetaDataGridRow row1 = (MetaDataGridRow)x;
				MetaDataGridRow row2 = (MetaDataGridRow)y;


					return 0;
				int compRes = 0;
				foreach (frmSort.SortItem si in sortItems)
				{
					//int compareResult = System.String.Compare(
					//	row1.
					//	dataGridViewRow1.Cells[1].Value.ToString(),
					//	dataGridViewRow2.Cells[1].Value.ToString());

				}

				return compRes;
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
			sortForm.ShowDialog();
			if (DialogResult.Yes == sortForm.DialogResult)
			{
				var metaDataRows = (List<MetaDataGridRow>)dataGridView1.DataSource;
				dataGridView1.DataSource = null;
				//RowComparer rc = new RowComparer(SortOrder.Ascending, sortForm.SortedFields);
				switch (sortForm.SortedFields.Count)
				{
					case 1:
					{
						metaDataRows.Sort( (x,y) =>
						{
							Type m = metaDataRows[0].GetType();
							PropertyInfo p1Info = m.GetProperty(sortForm.SortedFields[0].val);
							var gMethod = p1Info.GetGetMethod();
							string x1 = (string) gMethod.Invoke(x, null);
							string y1 = (string) gMethod.Invoke(y, null);
							return string.Compare(x1, y1);
						});
					}
						break;
				}

				//sortForm.SortedFields
				//metaDataRows.Sort(rc);
				dataGridView1.DataSource = metaDataRows;
				//dataGridView1.Sort( new RowComparer(SortOrder.Ascending, sortForm));
			}
		}

		private void editToolStripMenuItem1_Click(object sender, EventArgs e)
		{

		}

		private void loadMetaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var workDir = Directory.GetCurrentDirectory();
			var path = workDir + "..\\..\\..\\Data\\MetaMeta.xml";
			metaMetaData = new MetaMetaData(path);
			object rowType = metaMetaData.RowType;
		}

		private void filterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var filtForm = new frmFilter();

			List<string> colHeaders = new List<string>();
			foreach (DataGridViewColumn col in dataGridView1.Columns)
			{
				colHeaders.Add(col.HeaderText);
			}

			filtForm.Populate(colHeaders);
			filtForm.MetaDataGridRows = metaDataRows;
			filtForm.ShowDialog();
			if (DialogResult.OK == filtForm.DialogResult)
			{
				dataGridView1.DataSource = DoFilter(metaDataRows, filtForm.FilterFields[0].name,
					(string) filtForm.FilterFields[0].val);
			}
		}

		List<MetaDataGridRow> DoFilter(List<MetaDataGridRow> allRows, string name, string value)
		{
			List<MetaDataGridRow> filterRows = new List<MetaDataGridRow>();
			//var  rowT  allRow

			var pInfo = allRows[0].GetType().GetProperty(name);
			var gMethod = pInfo.GetGetMethod();
			foreach (var metaDataGridRow in allRows)
			{
				if ((string) gMethod.Invoke(metaDataGridRow, null) == value)
				{
					filterRows.Add(metaDataGridRow);
				}
			}

			return filterRows;
		}
	}
}
