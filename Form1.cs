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
			var workDir = Directory.GetCurrentDirectory();
			var path = workDir + "..\\..\\..\\Data\\d_adcdls.xml";
			var metaData = new MetaData(path);
			metaData.Load();
			metaDataRows = metaData.Denormalize();
			gridControl2.DataSource = metaDataRows;
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
	}
}
