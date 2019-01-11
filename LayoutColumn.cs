using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Data;
using System.Windows.Forms;

namespace MetaDataEditor
{
	public class LayoutColumn
	{
		public string id { get; set; }
		public string header { get; set; }
		public bool visible { get; set; }
		public int width { get; set; }
	}
}