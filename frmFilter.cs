using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Extensions;

namespace MetaDataEditor
{

	public partial class frmFilter : Form
	{
		private List<FilterItem> availibleFields;
		private List<FilterItem> filterFields;
		private List<MetaDataGridRow> metaDataRows;

		private DialogResult DlgDialogResult;

		public List<FilterItem> FilterFields
		{
			get { return filterFields; }
		}

		public List<MetaDataGridRow> MetaDataGridRows
		{
			set { metaDataRows = value; }
		}

		private DataGridView metaDataGridView;
		public DataGridView MetaDataGridView
		{
			set { metaDataGridView = value; }
		}


		public frmFilter()
		{
			InitializeComponent();
			availibleFields = new List<FilterItem>();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void pbCancel_Click(object sender, EventArgs e)
		{
			DlgDialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void frmFilter_Load(object sender, EventArgs e)
		{

		}

		public void Populate(List<string> fields)
		{

			for (int i = 0; i < fields.Count; i++)
			{
				var si = new FilterItem() {ord = i, name = fields[i]};
				availibleFields.Add(si);
			}

			List<FilterItem> availibleFields2=new List<FilterItem>(availibleFields);
			List<FilterItem>  availibleFields3= new List<FilterItem>(availibleFields);
	

			cmbxFld1.DataSource = availibleFields;
			cmbxFld1.DisplayMember = "name";
			cmbxFld1.ValueMember = "ord";


			cmbxFld2.DataSource = availibleFields2;
			cmbxFld2.DisplayMember = "name";
			cmbxFld2.ValueMember = "ord";


			cmbxFld3.DataSource = availibleFields3;
			cmbxFld3.DisplayMember = "name";
			cmbxFld3.ValueMember = "ord";

		}

		public class FilterItem
		{
			public enum CmpType
			{
				Str, Num
			}
			public String name { get; set; }
			public object val { get; set; }
			public int ord { get; set; }
			public CmpType cmpType { get; set; }
		}

		private void pbOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(cmbxVal1.Text))
			{
				filterFields = new List<FilterItem>();
				FilterItem filterItem = new FilterItem();
				filterItem.name = cmbxFld1.Text;
				filterItem.val = cmbxVal1.Text;
				filterItem.cmpType = FilterItem.CmpType.Str;
				filterFields.Add(filterItem);
			}

			if (!string.IsNullOrEmpty(cmbxVal2.Text))
			{
				FilterItem filterItem2 = new FilterItem();
				filterItem2.name = cmbxFld2.Text;
				filterItem2.val = cmbxVal2.Text;
				filterItem2.cmpType = FilterItem.CmpType.Str;
				filterFields.Add(filterItem2);
			}

			if (!string.IsNullOrEmpty(cmbxVal3.Text))
			{
				FilterItem filterItem3 = new FilterItem();
				filterItem3.name = cmbxFld3.Text;
				filterItem3.val = cmbxVal3.Text;
				filterItem3.cmpType = FilterItem.CmpType.Str;
				filterFields.Add(filterItem3);

			}

			DialogResult = DialogResult.OK;
			this.Close();


		}

		private void cmbxFld1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			
			if (comboBox.SelectedItem != null)
			{
				var selectField = ((FilterItem)comboBox.SelectedItem).name;
				var uniques = GetUniqueList(selectField, metaDataGridView);
				if (uniques != null)
				{
					switch (comboBox.Name)
					{
						case "cmbxFld1":
							cmbxVal1.DataSource = uniques.ToList();
							break;

						case "cmbxFld2":
							cmbxVal2.DataSource = uniques.ToList();
							break;

						case "cmbxFld3":
							cmbxVal3.DataSource = uniques.ToList();
							break;
					}

				}
			}

		}

		private HashSet<string> GetUniqueList(string colName, DataGridView metaDataView)
		{
			if (metaDataView == null)
				return null;
			var uniques = new HashSet<string>();
			//var pInfo =   metaDatRows[0].GetType().GetProperty(colName);
			//var gMethod = pInfo.GetGetMethod();
			foreach (DataGridViewRow row in metaDataView.Rows)
			{
				var val = row.Cells[colName].Value;
				string possibleUnique="";
				if ( val != null )
					possibleUnique =  val.ToString();
				if (!uniques.Contains(possibleUnique))
				{
					uniques.Add(possibleUnique);
				}

			}

			return uniques;
		}

		private void cmbxVal1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void lbl1_Click(object sender, EventArgs e)
		{

		}
	}
}
