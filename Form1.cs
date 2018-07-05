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

namespace MetaDataEditor
{
	public partial class Form1 : Form
	{
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
	}
}
