using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Extensions;
using DevExpress.XtraRichEdit.UI;

namespace MetaDataEditor
{
	public partial class frmSort : Form
	{
		private List<SortItem> availibleFields;
		private List<SortItem> sortedFields;

		public List<SortItem> SortedFields {get { return sortedFields;}
}
		public frmSort()
		{
			InitializeComponent();
			availibleFields = new List<SortItem>();
			sortedFields = new List<SortItem>();
			DialogResult = DialogResult.Cancel;
		}

		public DialogResult DialogResult { get; set; }

		private void button1_Click(object sender, EventArgs e)
		{

		}

		private void pbUp_Click(object sender, EventArgs e)
		{

		}

		private void frmSort_Load(object sender, EventArgs e)
		{

		}

		public void Populate(List<string> fields)
		{
			//foreach (var s in fields)
			//{
			//	this.availibleFields.Add(s);
			//}
			for (int i = 0;  i < fields.Count; i++)
			{
				var si = new  SortItem() {ord = i, val = fields[i]};
				availibleFields.Add(si);
			}

			lstAvailFlds.DataSource = availibleFields;
			lstAvailFlds.DisplayMember = "val";
			lstAvailFlds.ValueMember = "ord";
			lstSortFlds.DataSource = sortedFields;

		}

		private void pbCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void pbMoveRight_Click(object sender, EventArgs e)
		{
			lstAvailFlds.BeginUpdate();
			lstSortFlds.BeginUpdate();
			var items = ( List<SortItem>)lstAvailFlds.DataSource;
			var sortItems = (List<SortItem>) lstSortFlds.DataSource;


			var item = (SortItem)lstAvailFlds.SelectedItem;
			lstAvailFlds.DataSource = null;
			lstSortFlds.DataSource = null;
			lstAvailFlds.Items.Clear();
			items.Remove(item);
			sortItems.Add(item);
			lstSortFlds.DataSource = sortItems;
			lstSortFlds.DisplayMember = "val";
			lstSortFlds.ValueMember = "ord";
			lstAvailFlds.DataSource = items;
			lstAvailFlds.DisplayMember = "val";
			lstAvailFlds.ValueMember = "ord";
			lstAvailFlds.EndUpdate();
			lstSortFlds.EndUpdate();
		}

		private void pbRemove_Click(object sender, EventArgs e)
		{
			lstSortFlds.BeginUpdate();
			lstAvailFlds.BeginUpdate();
			var sortItem = (SortItem)lstSortFlds.SelectedItem;
			var sortItems = (List<SortItem>) lstSortFlds.DataSource;

			lstSortFlds.DataSource = null;
			lstSortFlds.Items.Clear();
			sortItems.Remove(sortItem);
			lstSortFlds.DataSource = sortItems;



			var availibleItems = (List<SortItem>)lstAvailFlds.DataSource;
			int pos = FindCurrentPosition(availibleItems, sortItem);
			lstAvailFlds.DataSource = null;
			lstAvailFlds.Items.Clear();
			availibleItems.Insert(pos, sortItem);

			lstSortFlds.DisplayMember = "val";
			lstSortFlds.ValueMember = "ord";
			lstAvailFlds.DataSource = availibleItems;
			lstAvailFlds.DisplayMember = "val";
			lstAvailFlds.ValueMember = "ord";
			lstAvailFlds.EndUpdate();
			lstSortFlds.EndUpdate();
		}

		int FindCurrentPosition(List<SortItem> currList, SortItem si)
		{
			for (int i = 0; i < currList.Count; i++)
			{
				if (currList[i].ord > si.ord)
					return i;
			}

			return -1;
		}

		public class SortItem
		{
			public String val { get; set; }
			public int ord { get; set; }
		}

		private void pbOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Yes;
			this.Close();
		}
	}
}
