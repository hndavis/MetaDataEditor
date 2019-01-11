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
	public class MetaData
	{
        Dictionary<string,DataBase> DataBases = new Dictionary<string, DataBase>();
		public List<Dictionary<string, string>> vals = new List<Dictionary<string, string>>();
		private List<Dictionary<string, string>> valsByColumn = new List<Dictionary<string, string>>();
		private Dictionary<string, string> dataBaseAttributes = new Dictionary<string, string>();
		private Dictionary<string, string> catAttributes;
		private XmlDocument doc;
	

		String Path{ get; set; }

		public MetaData(string path)
		{
			Path = path;
			doc = new XmlDocument();
		}


		/// <summary>
		/// this constructor loads back an a set of denormolized Rows
		/// </summary>
		/// <param name="path"></param>
		/// <param name="rows"></param>
		public MetaData(string path, List<MetaDataGridRow> rows)
		{
			Path = path;
			doc = new XmlDocument();
		}

		public List<MetaDataGridRow> Denormalize()
		{
			var denormalizedMetaData = new List<MetaDataGridRow>();
			foreach (var database in DataBases)
			{
				foreach (var category in database.Value.Categories)
				{
					foreach (var subCat in category.SubCategories)
					{
						foreach (var item in subCat.Value.Items)
						{
							var row = new MetaDataGridRow();

							//PropertyInfo dbPropertyInfo = row.GetType().GetProperty("DatabaseID");
							foreach (var param in item.Value.InternalParams)
							{
								switch (param.Key)   // switch is used here in case translation from abrev needed
								{
									default:
									{
										PropertyInfo propertyInfo = row.GetType().GetProperty(param.Key);
										if (propertyInfo != null)
											propertyInfo.SetValue(row, item.Value.InternalParams[param.Key]);
									}
										break;
								}
							}

							foreach (var param in item.Value.Attribs)
							{

								switch (param.Key)
								{
									case "i_id":
										SetValue(row, "ID", item.Value.Attribs[param.Key]);
										break;

									case "i_n":
										SetValue(row, "Name", item.Value.Attribs[param.Key]);
										break;

									case "i_l":
										SetValue(row, "Label", item.Value.Attribs[param.Key]);
										break;

									case "i_d":
										SetValue(row, "Description", item.Value.Attribs[param.Key]);
										break;

									case "i_pc":
										SetValue(row, "ParameterClass", item.Value.Attribs[param.Key]);
										break;

									case "i_cc":
										SetValue(row, "ColumnClass", item.Value.Attribs[param.Key]);
										break;

									case "i_vu":
										SetValue(row, "ValueUnit", item.Value.Attribs[param.Key]);
										break;

									case "i_dt":
										SetValue(row, "DataType", item.Value.Attribs[param.Key]);
										break;

									case "i_app":
										SetValue(row, "App", item.Value.Attribs[param.Key]);
										break;

									case "i_universe":
										SetValue(row, "Universe", item.Value.Attribs[param.Key]);
										break;

									case "i_isGrid":
										SetValue(row, "isGrid", item.Value.Attribs[param.Key]);
										break;

									default:
									{
										PropertyInfo propertyInfo = row.GetType().GetProperty(param.Key);
										if (propertyInfo != null)
											propertyInfo.SetValue(row, item.Value.Attribs[param.Key]);

									}
										break;
								}

							}

							row.DatabaseID = database.Value.Attributes["d_id"];
							row.CategoryId = category.Attributes["c_id"];
							row.SubCategoryId = subCat.Value.Attributes["c_id"];

							denormalizedMetaData.Add(row);
						}
					}

				}
			}

			return denormalizedMetaData;
		}

		public DataTable ToDataTable( DataTable EmptyDataTable)
		{

			var dt = EmptyDataTable.Clone();
			foreach (var database in DataBases)
			{
				foreach (var category in database.Value.Categories)
				{
					foreach (var subCat in category.SubCategories)
					{
						foreach (var item in subCat.Value.Items)
						{
							var row = dt.NewRow();
							
							try
							{
								foreach (var param in item.Value.Attribs)
								{
									row[param.Key] = item.Value.Attribs[param.Key];

								}

								foreach (var iparam in item.Value.InternalParams)
								{
									row[iparam.Key] = item.Value.InternalParams[iparam.Key];

								}

								foreach (var dbAttribute in database.Value.Attributes)
								{
									row[dbAttribute.Key] = dbAttribute.Value;
								}

								foreach (var catAttribute  in category.Attributes)
								{
									row[catAttribute.Key.Replace("c_", "cat_")] = catAttribute.Value;

								}
								foreach (var scat in subCat.Value.Attributes)
								{
										row[scat.Key.Replace("c_","s_cat_")] = scat.Value;

								}

								dt.Rows.Add(row);
							}
							catch (Exception ex)
							{
								 MessageBox.Show("On Item:"+ item.Key +" \n" + ex.Message, "Check Definitions", MessageBoxButtons.OK);
								return null;
							}

						}
					}

				}
			}

			return dt;
		}

		void SetValue(object o, string name, string value)
		{
			PropertyInfo propertyInfo = o.GetType().GetProperty(name);
			if (propertyInfo != null)
				propertyInfo.SetValue(o, value);

		}


		public bool Load(bool prefix = true)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(Path);

			XmlNodeList databaseNodes = doc.ChildNodes;
			foreach (XmlNode databaseNode in databaseNodes)
			{
				dataBaseAttributes = new Dictionary<string, string>();
				foreach (XmlAttribute dbAttrib in databaseNode.Attributes)
				{
					dataBaseAttributes[databaseNode.Name + "_" + dbAttrib.Name.Replace(":","__")] = dbAttrib.Value;
				}

				DataBases[databaseNode.Name] = new DataBase
				{
					Attributes = dataBaseAttributes
				};



				foreach (XmlNode pCat in databaseNode.ChildNodes)
				{
					catAttributes = new Dictionary<string, string>();
					var cat = new Category();
					if (pCat.Attributes != null)
					{
						foreach (XmlAttribute catAttrib in pCat.Attributes)
						{
							catAttributes[pCat.Name+"_"+catAttrib.Name] = catAttrib.Value;
						}
					}

					cat.Attributes = catAttributes;
					DataBases[databaseNode.Name].Categories.Add(cat);
					foreach (XmlNode subCatNode in pCat.ChildNodes)
					{
						var subCat = new SubCategory();
						var subCatAttribs = new Dictionary<string, string>();
						if (subCatNode.Attributes == null)
							break;

						foreach (XmlAttribute att in subCatNode.Attributes)
						{
							subCatAttribs[subCatNode.Name+"_" +att.Name] = att.Value;

						}

						subCat.Attributes = subCatAttribs;

						foreach (XmlNode item in subCatNode.ChildNodes)
						{
							if (item.NodeType == XmlNodeType.Comment)
								continue;

							var itemRow = new Item();
							var itemAttributes = new Dictionary<string, string>();

							if (item.Attributes != null)
							{
								foreach (XmlAttribute att in item.Attributes)
								{
									if (att.Name == "n")
										itemRow.Name = att.Value;

									itemAttributes[item.Name+"_" + att.Name] = att.Value;
								}
							}

							itemRow.Attribs = itemAttributes;
							var internalParams = new Dictionary<string, string>();
							if (item.HasChildNodes)
							{
								foreach (XmlNode inParam in item.ChildNodes)
								{
									if (inParam.Attributes == null)
										continue;

									if (inParam.ChildNodes.Count > 0)
										internalParams[inParam.Name.Replace(":sp", "") + "_" + inParam.Attributes[0].Value] = inParam.FirstChild.Value;

								}
							}

							if (itemRow.Attribs == null || itemRow.Attribs.Count == 0)
								continue;

							itemRow.InternalParams = internalParams;
							subCat.Items.Add(itemRow.Attribs["i_id"], itemRow);
						}

						cat.SubCategories.Add(subCatAttribs.First().Value, subCat);
					}

				}
			}


			return false;
		}

		public void Update(List<MetaDataGridRow> changedRows)
		{

			// for every row find correct node based key ( database, category, category, id )

			XPathNodeIterator NodeIter;
			String strExpression;

			//var mainDoc = new XElement(Path);
			//var mainDoc =  new XElement("Students.xml");
			var mainDoc = new XElement("d_adcdls_part.xml");
			

			//var nav = doc.CreateNavigator();
			foreach (var changedRow in changedRows)
			{
				var databases = from database in mainDoc.Descendants()
								select new
								{
									DatabaseName = database.Attribute("id"),
									Name = database.Attribute("n"),
									Description = database.Attribute("d")

								};

				foreach (var database in databases)
				{
					Console.WriteLine("Database ={0} Name={1} Desc={2}", database.DatabaseName, database.Name, database.Description);
				}
				//	strExpression = string.Format("/dsdas/c");
				//	var node = nav.Evaluate(strExpression);
			}
			
			
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


	internal class Item
	{
		public String Name { get; set; }
		public Dictionary<string, string> Attribs = new Dictionary<string, string>();
		public Dictionary<string, string> InternalParams = new Dictionary<string, string>();
		public Dictionary<string, string> OutParams = new Dictionary<string, string>();  //todo need to understand how this is used

	}

	internal class SubCategory
	{
		public string Name { get; set; }

		public Dictionary<string, Item> Items = new Dictionary<string, Item>();
		public Dictionary<string, string> Attributes = new Dictionary<string, string>();
	}

	internal class Category
	{
		public string Name { get; set; }
		public Dictionary<string, string> Attributes = new Dictionary<string, string>();
		public Dictionary<string, SubCategory>  SubCategories = new Dictionary<string, SubCategory>();
	}

	internal class DataBase
	{
		public string Name { get; set; }

		public Dictionary<string, string> Attributes = new Dictionary<string,string >();
		public List<Category>  Categories = new List<Category>();
	}
}
