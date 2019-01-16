using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MetaDataEditor
{
	public partial class frmSort : Form
	{
		private readonly List<SortItem> availibleFields;
		private readonly List<SortItem> sortedFields;

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

		public void Populate(List<Tuple<string, string, int>> fields)
		{
			//foreach (var s in fields)
			//{
			//	this.availibleFields.Add(s);
			//}
			for (int i = 0;  i < fields.Count; i++)
			{
				var si = new  SortItem() {val = fields[i].Item1, disp = fields[i].Item2, ord = fields[i].Item3};
				availibleFields.Add(si);
			}

			lstAvailFlds.DataSource = availibleFields;
			lstAvailFlds.DisplayMember = "disp";
			lstAvailFlds.ValueMember = "val";
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
			lstSortFlds.DisplayMember = "disp";
			lstSortFlds.ValueMember = "val";
			lstAvailFlds.DataSource = items;
			lstAvailFlds.DisplayMember = "disp";
			lstAvailFlds.ValueMember = "val";
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

			lstSortFlds.DisplayMember = "disp";
			lstSortFlds.ValueMember = "val";
			lstAvailFlds.DataSource = availibleItems;
			lstAvailFlds.DisplayMember = "disp";
			lstAvailFlds.ValueMember = "val";
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
			public String disp { get; set; }
			public  int ord { get; set; }
		}

		private void pbOk_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Yes;
			this.Close();
		}
	}
}
