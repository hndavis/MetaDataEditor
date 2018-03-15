using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MetaDataEditor
{
	public class MetaData
	{
		List<string> Names = new List<string>();
		List<List<string>> vals = new List<List<string>>();
		String Path{ get; set; }

		MetaData(string path)
		{
			Path = path;
		}
		public bool Load(string path)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			string xmlcontents = doc.InnerXml;
			return false;
		}


		public bool SaveAs()
		{
			return Save();
		}
		public bool Save()
		{
			return false;
		}
	}
}
