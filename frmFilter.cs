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

			cmbxFld1.DataSource = availibleFields;
			cmbxFld1.DisplayMember = "name";
			cmbxFld1.ValueMember = "ord";

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
			filterFields = new List<FilterItem>();
			FilterItem filterItem = new FilterItem();
			filterItem.name = cmbxFld1.Text;
			filterItem.val = cmbxVal1.Text;
			filterItem.cmpType = FilterItem.CmpType.Str;
			filterFields.Add(filterItem);
			DialogResult = DialogResult.OK;
			this.Close();


		}

		private void cmbxFld1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.SelectedItem != null)
			{
				var selectField = ((FilterItem)comboBox.SelectedItem).name;
				var uniques = GetUniqueList(selectField, metaDataRows);
				if ( uniques !=  null)
					cmbxVal1.DataSource = uniques.ToList();
			}

		}

		private HashSet<string> GetUniqueList(string colName, List<MetaDataGridRow> metaDatRows)
		{
			if (metaDatRows == null)
				return null;
			var uniques = new HashSet<string>();
			var pInfo = metaDatRows[0].GetType().GetProperty(colName);
			var gMethod = pInfo.GetGetMethod();
			foreach (var metaDataGridRow in metaDatRows)
			{
				string val = (string)gMethod.Invoke(metaDataGridRow, null);
				if (!uniques.Contains(val))
				{
					uniques.Add(val);
				}
				//var getFieldMethod = memberInfo.
			}

			return uniques;
		}
	}
}
