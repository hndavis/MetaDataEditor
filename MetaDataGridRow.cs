using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars.Ribbon.Drawing;
using DevExpress.XtraSplashScreen;

namespace MetaDataEditor
{
	public class MetaDataGridRow
	{
		public string DatabaseID { get; set; }
		public string CategoryId { get; set; }

		public string SubCategoryId { get; set; }
		public string ID { get; set; }

		public string Name { get; set; }
		public string Label { get; set; }
		public string Description { get; set; }
		public string ValueUnit { get; set; }
		public string ColumnClass { get; set; }
		public string ParameterClass { get; set; }
		public string DataType { get; set;  }

		public string s { get; set; }
		public string ss { get; set; }

		public string isScreen { get; set; }
		public string isGrid { get; set; }
		public string App { get; set; }
		public string pcc { get; set; }

		public string in_id { get; set;}

		public string in_table { get; set; }

		public string in_CurrAvailility { get; set; }
		public string in_tableHostCurr { get; set; }
		public string in_viewName { get; set; }



	}
}
