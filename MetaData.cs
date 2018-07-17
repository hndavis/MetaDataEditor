using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MetaDataEditor
{
	public class MetaData
	{
        Dictionary<string,DataBase> DataBases = new Dictionary<string, DataBase>();
		public List<Dictionary<string, string>> vals = new List<Dictionary<string, string>>();
		private List<Dictionary<string, string>> valsByColumn = new List<Dictionary<string, string>>();
		private Dictionary<string, string> dataBaseAttributes = new Dictionary<string, string>();
		private Dictionary<string, string> catAttributes;
		String Path{ get; set; }

		public MetaData(string path)
		{
			Path = path;

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
									case "id":
										SetValue(row, "ID", item.Value.Attribs[param.Key]);
										break;

									case "n":
										SetValue(row, "Name", item.Value.Attribs[param.Key]);
										break;

									case "l":
										SetValue(row, "Label", item.Value.Attribs[param.Key]);
										break;

									case "d":
										SetValue(row, "Description", item.Value.Attribs[param.Key]);
										break;

									case "pc":
										SetValue(row, "ParameterClass", item.Value.Attribs[param.Key]);
										break;

									case "cc":
										SetValue(row, "ColumnClass", item.Value.Attribs[param.Key]);
										break;

									case "vu":
										SetValue(row, "ValueUnit", item.Value.Attribs[param.Key]);
										break;

									case "dt":
										SetValue(row, "DataType", item.Value.Attribs[param.Key]);
										break;

									case "app":
										SetValue(row, "App", item.Value.Attribs[param.Key]);
										break;

									case "universe":
										SetValue(row, "Universe", item.Value.Attribs[param.Key]);
										break;

									case "isGrid":
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

							row.DatabaseID = database.Value.Attributes["id"];
							row.CategoryId = category.Attributes["id"];
							row.SubCategoryId = subCat.Value.Attributes["id"];

							denormalizedMetaData.Add(row);
						}
					}

				}
			}

			return denormalizedMetaData;
		}

		void SetValue(object o, string name, string value)
		{
			PropertyInfo propertyInfo = o.GetType().GetProperty(name);
			if (propertyInfo != null)
				propertyInfo.SetValue(o, value);

		}


		public bool Load()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(Path);

			XmlNodeList databaseNodes = doc.ChildNodes;
			foreach (XmlNode databaseNode in databaseNodes)
			{
				dataBaseAttributes = new Dictionary<string, string>();
				foreach (XmlAttribute dbAttrib in databaseNode.Attributes)
				{
					dataBaseAttributes[dbAttrib.Name] = dbAttrib.Value;
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
							catAttributes[catAttrib.Name] = catAttrib.Value;
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
							subCatAttribs[att.Name] = att.Value;

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

									itemAttributes[att.Name] = att.Value;
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
										internalParams["in" + "_" + inParam.Attributes[0].Value] = inParam.FirstChild.Value;

								}
							}

							if (itemRow.Attribs == null || itemRow.Attribs.Count == 0)
								continue;

							itemRow.InternalParams = internalParams;
							subCat.Items.Add(itemRow.Attribs["id"], itemRow);
						}

						cat.SubCategories.Add(subCatAttribs.First().Value, subCat);
					}

				}
			}


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
