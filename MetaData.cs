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
        Dictionary<string, string> DataBase = new Dictionary<string, string>();
		private List<Dictionary<string, string>> vals = new List<Dictionary<string, string>>();
		private List<Dictionary<string, string>> valsByColumn = new List<Dictionary<string, string>>();
		private Dictionary<string, string> dataBaseAttributes = new Dictionary<string, string>();
		private Dictionary<string, string> catAttributes;
		String Path{ get; set; }

		public MetaData(string path)
		{
			Path = path;

		}
		public bool Load()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(Path);
			string xmlcontents = doc.InnerXml;
			XmlNode dataBase = doc.ChildNodes.Item(0);
			foreach (XmlAttribute dbAttrib in dataBase.Attributes)
			{
				dataBaseAttributes[dbAttrib.Name] = dbAttrib.Value;
				DataBase.Add(dbAttrib.Name, dbAttrib.Value);
			}
			foreach (XmlNode pCat in dataBase.ChildNodes)
			{
				catAttributes = new Dictionary<string, string>();
				if (pCat.Attributes != null)
				{
					foreach (XmlAttribute catAttrib in pCat.Attributes)
					{
						catAttributes[catAttrib.Name] = catAttrib.Value;
						//Names.Add(catAttrib.Name);
					}
				}

				foreach (XmlNode cat in pCat.ChildNodes)
				{
					var itemRow = new Dictionary<string, string>();
					//if (item.Attributes != null)
					//{
					//	foreach (XmlAttribute att in item.Attributes)
					//	{
					//		itemRow[att.Name] = att.Value;

					//	}
					//}
					foreach (XmlNode item in cat.ChildNodes)
					{
						if (item.Attributes != null)
						{
							foreach (XmlAttribute att in item.Attributes)
							{
								itemRow[att.Name] = att.Value;
								//Names.Add(att.Name);
							}
						}

						if (item.HasChildNodes)
						{
							foreach (XmlNode inParam in item.ChildNodes)
							{

                                itemRow[inParam.Attributes[0].Value] = inParam.FirstChild.Value;
								//Names.Add(inParam.Name);
							}
						}

						vals.Add(itemRow);
					}


				}
			}

			//foreach(var pair in vals.)
			//foreach (string ColName in Names)
			//{
				
			//}

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
