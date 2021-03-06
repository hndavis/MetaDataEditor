﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Data.OleDb;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;


namespace MetaDataEditor
{
	public partial class frmEditorFrame : Form
	{
		private static string WindowTitle = "DataCloud API MetaDataEditor";

		private MetaMetaData metaMetaData;
		private DataTable metaDT;
		private Boolean showAll = true;
		public frmEditorFrame()
		{
			InitializeComponent();
		}



		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var currentMetaMeta = Properties.Settings.Default.MetaMeta;

			var workDir = Directory.GetCurrentDirectory();
			var metaPathFile = workDir + "..\\..\\..\\Data\\MetaMeta.xml";
			if (!File.Exists(metaPathFile))
			{
				var metaPath = workDir + "..\\..\\..\\Data";
				OpenFileDialog openFileDialogMetaDef = new OpenFileDialog()
				{
					FileName = "Select a Meta Definiton file MetaMeta.xml",
					Filter = "Meta Config (*.xml)|*.xml",
					Title = "Open xml meta definition file",
					InitialDirectory = metaPath,

				};

				if (openFileDialogMetaDef.ShowDialog() == DialogResult.OK)
				{
					metaPathFile = openFileDialogMetaDef.FileName;
					Properties.Settings.Default.MetaMeta = metaPathFile;
					Properties.Settings.Default.Save();
				}
			}
			metaMetaData = new MetaMetaData(metaPathFile);
			
			metaMetaData.Load();
			metaDT = metaMetaData.ToDataTableStructure(metaMetaData.Cols);

			var initialMetaDataFle =Properties.Settings.Default.LastMetaData;
			string initialDataDir="";
			if (!string.IsNullOrEmpty(Properties.Settings.Default.LastMetaData))
			{
				try
				{
					initialDataDir = Path.GetDirectoryName(Properties.Settings.Default.LastMetaData);
				}
				catch (Exception)
				{
					initialDataDir = @"c:\";
				}
			}
			OpenFileDialog openFileDialogMeta = new OpenFileDialog()
			{
				FileName = "Select a Meta file xxxxx.xml",
				Filter = "Meta file (*.xml)|*.xml",
				Title = "Open xml meta file",
				InitialDirectory = initialDataDir,

			};

			string path="";// = workDir + "..\\..\\..\\Data\\d_adcdls.xml";
			if (openFileDialogMeta.ShowDialog() == DialogResult.OK)
			{
				path = openFileDialogMeta.FileName;
				Properties.Settings.Default.LastMetaData = path;
				Properties.Settings.Default.Save();
			

			var metaData = new MetaData(path);
			metaData.Load();
			var dt = metaData.ToDataTable(metaDT);
			dataGridView1.DataSource = dt;
			showAllColsToolStripMenuItem_Click(this, null);
			}
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

				DataGridViewRow row1 = x as DataGridViewRow;
				DataGridViewRow row2 = y as DataGridViewRow;


				int compRes = 0;
				foreach (frmSort.SortItem si in sortItems)
				{
				//	compRes = row1.Cells[si.ord].Value.ToString().CompareTo(row2.Cells[si.ord].Value.ToString());

				}

				return compRes;
			}
		}

		private void sortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sortForm = new frmSort();

			var colHeaders = new List<Tuple<string, string, int>>();
			int i = 0;
			foreach (DataGridViewColumn col in dataGridView1.Columns)
			{
				colHeaders.Add(new Tuple<string, string,int>(col.Name, col.HeaderText, i++));
			}

			sortForm.Populate(colHeaders);
			sortForm.ShowDialog();
			if (DialogResult.Yes == sortForm.DialogResult)
			{
				DataTable dtSort = (DataTable)dataGridView1.DataSource;
				RowComparer rowComparer = new RowComparer(SortOrder.Ascending, sortForm.SortedFields);
				StringBuilder sortBuilder = new StringBuilder();
				foreach (frmSort.SortItem sortField in sortForm.SortedFields)
				{
					if (sortBuilder.Length > 0)
						sortBuilder.Append(" , ");
					sortBuilder.Append(sortField.val);
				}

				dtSort.DefaultView.Sort = sortBuilder.ToString();
				//var metaDataRows = (DataTable)dataGridView1.DataSource;
				//dataGridView1.DataSource = null;
				//RowComparer rc = new RowComparer(SortOrder.Ascending, sortForm.SortedFields);
				//switch (sortForm.SortedFields.Count)
				//{
				//	case 1:
				//	{
				//		metaDataRows.Sort( (x,y) =>
				//		{
				//			Type m = metaDataRows[0].GetType();
				//			PropertyInfo p1Info = m.GetProperty(sortForm.SortedFields[0].val);
				//			var gMethod = p1Info.GetGetMethod();
				//			string x1 = (string) gMethod.Invoke(x, null);
				//			string y1 = (string) gMethod.Invoke(y, null);
				//			return string.Compare(x1, y1);
				//		});
				//	}
				//		break;
				//}

				////sortForm.SortedFields
				////metaDataRows.Sort(rc);
				//dataGridView1.DataSource = metaDataRows;
				////dataGridView1.Sort( new RowComparer(SortOrder.Ascending, sortForm));
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

			metaMetaData.Load();
			metaDT = metaMetaData.ToDataTableStructure(metaMetaData.Cols);

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
			filtForm.MetaDataGridView = dataGridView1;
			filtForm.ShowDialog();
			if (DialogResult.OK == filtForm.DialogResult)
			{
				//dataGridView1.DataSource = DoFilter(metaDataRows, filtForm.FilterFields[0].name,
				//	(string) filtForm.FilterFields[0].val);

				DoFilter(dataGridView1, filtForm.FilterFields);

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


		void DoFilter(DataGridView allRows, List<frmFilter.FilterItem> fltrItems)
		{
			if (allRows.CurrentCell!= null )
				allRows.BeginEdit(false);
			List<MetaDataGridRow> filterRows = new List<MetaDataGridRow>();
			//var  rowT  allRow
			allRows.CurrentCell = null;
			foreach (DataGridViewRow row in allRows.Rows)
			{
				foreach (frmFilter.FilterItem fltrItem in fltrItems)
				{
					row.Visible = true;
					var comp1 = row.Cells[fltrItem.name].Value;
					if (comp1 != null)
					{
						if (!comp1.ToString().Contains((string)fltrItem.val))
						{
							row.Visible = false;
							break;
						}
					}
				}

			}

			allRows.EndEdit();
		}
		private void showAllColsToolStripMenuItem_Click(object sender, EventArgs e)
		{

			foreach (DataGridViewColumn dgvcol in dataGridView1.Columns)
			{
				if (metaMetaData.LayoutColumns.ContainsKey(dgvcol.Name) )
				{
					dgvcol.HeaderText = metaMetaData.LayoutColumns[dgvcol.Name].header;
					if (!showAll)
					{
						dgvcol.Visible = metaMetaData.LayoutColumns[dgvcol.Name].visible;
						dgvcol.Width = metaMetaData.LayoutColumns[dgvcol.Name].width;

					}
					else
					{
						dgvcol.Visible = true;
					}
				}

			}

			if (showAll)
				showAll = false;
			else
				showAll = true;


		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MetaDataWriter mdw = new MetaDataWriter(@"c:\temp\outtestmeta.xml", metaMetaData, (DataTable)dataGridView1.DataSource);
			mdw.Write();
		}

		
		private void Form1_Load_1(object sender, EventArgs e)
		{
			string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			//future use ... ?
			//string assemblyVersion = Assembly.LoadFile('your assembly file').GetName().Version.ToString();
			
			//string fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
			//string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
			//this.Name = WindowTitle +" "+ fileVersion;


			this.Text = WindowTitle + " " + assemblyVersion;

		}

		private void importFromExcelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			loadMetaToolStripMenuItem_Click(this, null);
			OpenFileDialog openFileDlgExcel = new OpenFileDialog();
			openFileDlgExcel.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
			//string connInfo = "Provider=Micrsoft.ACE.OLEDB.12.0;Data Source=";
			if (openFileDlgExcel.ShowDialog() == DialogResult.OK)
			{
				var excelFileName = openFileDlgExcel.FileName;
				// need to install the runtime access engine
				//var PathConn = connInfo + excelFileName + ";Extendend Properties=\"Excel 8.0;HDR=Yes;\";";
				//OleDbConnection conn = new OleDbConnection(PathConn);

				//OleDbDataAdapter excelDataAdapter1 = new OleDbDataAdapter("Select * from [Sheet1$]", conn);

				//DataTable dt = new DataTable();

				//excelDataAdapter1.Fill(dt);

				//dataGridView1.DataSource = dt;

				var wb = new XLWorkbook(excelFileName);
				var ws = wb.Worksheet("Items");
				var firstRowUsed = ws.FirstRowUsed();

				var firstPossibleAddress = ws.Row(firstRowUsed.RowNumber()).FirstCell().Address;
				var lastPossibleAddress = ws.LastCellUsed().Address;

				// Get a range with the remainder of the worksheet data (the range used)
				var range = ws.Range(firstPossibleAddress, lastPossibleAddress).AsRange(); //.RangeUsed();
				// Treat the range as a table (to be able to use the column names)
				var table = range.AsTable();
				DataTable dt = new DataTable();
				foreach (IXLTableField col in table.Fields)
				{
					dt.Columns.Add(col.Name);

				}

				foreach (IXLTableRow row in table.DataRange.RowsUsed())
				{
					var newRow = dt.NewRow();
					foreach (DataColumn dataColumn in dt.Columns)
					{
						newRow[dataColumn.ColumnName] = row.Field(dataColumn.ColumnName).GetString();
					}

					dt.Rows.Add(newRow);
				}
				dataGridView1.DataSource = dt;
			}
		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
	}
}
