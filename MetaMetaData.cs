using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MetaDataEditor
{
	public class MetaMetaData
	{
		private Dictionary<string, List<string>> dataBaseAttributes = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> catAttributes;

		private string path;

		Dictionary<string, DataBase> DataBases = new Dictionary<string, DataBase>();

		private object rowType = null;

		public object RowType {
			get
			{
				if (rowType == null)
				{
					rowType = Load();
				}

				return rowType;
			} }

		public MetaMetaData(string path)
		{
			this.path = path;
		}

		public object Load()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			XmlNodeList databaseNodes = doc.ChildNodes;
			foreach (XmlNode databaseNode in databaseNodes)
			{
				if ( databaseNode.Name !="d")  // skip the xml version row
					continue;

				
				//DataBases[databaseNode.Name] = new DataBase
				//{
				//	Attributes = dataBaseAttributes
				//};

				List<string> attribs = new List<string>();
				foreach (XmlNode dbItemNode in databaseNode.ChildNodes)
				{
					if (dbItemNode.Name == "a")   //database attribute
						attribs.Add(databaseNode.Name+ "_"+dbItemNode.Attributes[0].Value);


					if (dbItemNode.Name == "cat") // represents a catterory
					{
						List<string> cat_attribs = new List<string>();
						foreach (XmlNode catChild in dbItemNode.ChildNodes)
						{
							if (catChild.Name == "a")
							{
								cat_attribs.Add(dbItemNode.Name+ "_"+catChild.Attributes[0].Value);
							}

							if (catChild.Name == "s_cat")
							{
								List<string> subCatAttribs = new List<string>();
								foreach (XmlNode subCatChild in catChild.ChildNodes)
								{
									if (subCatChild.Name == "a")
									{
										subCatAttribs.Add(catChild.Name +"_"+subCatChild.Attributes[0].Value);
									}

									if (subCatChild.Name == "item")
									{
										List<string> itemAttribs = new List<string>();
										foreach (XmlNode itemNode in subCatChild.ChildNodes)
										{

											if (itemNode.Name == "a")
											{
												itemAttribs.Add(subCatChild.Name +"_"+ itemNode.Attributes[0].Value);
											}

											if (itemNode.Name == "in")
											{
												List<string> inputParams = new List<string>();
												foreach (XmlNode inParamNode in itemNode.ChildNodes)
												{
													if (inParamNode.Name == "kval")
													{
														inputParams.Add(itemNode.Name +"_" + inParamNode.Attributes[0].Value);

													}
												}

												// now have all defined columns
												var cols = GetFields(attribs, cat_attribs,
													subCatAttribs, itemAttribs, inputParams);
												// are there are dups
												var dups = cols.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key);
												if (dups.Count() != 0)
												{
													throw new Exception("There are duplicated Fields.");

												}

												return 	GetMetaRowType(cols);
											}
	
										}
									}
								}

							}
							
						}
					}

				}
			}

			return null;
		}

		private List<string> GetFields(List<string> databases, List<string> categories,
			List<string> subCategories, List<string> items, List<string> inputParams)
		{
			List<string> fields = new List<string>();
			foreach (var i in databases)
			{
				fields.Add(i);
			}
			foreach (var i in categories)
			{
				fields.Add(i);
			}
			foreach (var i in subCategories)
			{
				fields.Add(i);
			}

			foreach (var i in items)
			{
				fields.Add(i);
			}
			foreach (var i in inputParams)
			{
				fields.Add(i);
			}
			return fields;
		}

		private object GetMetaRowType(List<string> fields)
		{
			List<DynaField> dynaFields = new List<DynaField>();
			foreach (var fieldname in fields)
			{
				dynaFields.Add(new DynaField(fieldname,Type.GetType("A")));
			}
			dynamic obj = new DynamicMetaClass(dynaFields);
			return obj;
		}
	}
}
