using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CSharp;
using System.Data;


namespace MetaDataEditor
{

	public class MetaMetaData
	{
		private HashSet<string> dataBaseAttributes;
		private HashSet<string> catAttributes;
		private HashSet<string> scatAttributes;
		private HashSet<string> itemsAttributes;
		private HashSet<string> itemsInParams ;
		private Dictionary<string,string> ExcelNametoXmlName;
		private Dictionary<string, string> XmlNametoExcelName;

		public String GetMetaXmlName(string ExcelName)
		{
			if (ExcelNametoXmlName.ContainsKey(ExcelName))
				return ExcelNametoXmlName[ExcelName];
			return null;
		}

		public String GetMetaExcelName(string XmlName)
		{
			if (XmlNametoExcelName.ContainsKey(XmlName))
				return XmlNametoExcelName[XmlName];
			return null;
		}
		public HashSet<string> DataBaseAttributes
		{
			get { return dataBaseAttributes; }
		}

		public HashSet<string> CatAttributes
		{
			get { return catAttributes; }
		}

		public HashSet<string> ScatAttributes
		{
			get { return scatAttributes; }
		}

		public HashSet<String> ItemsAttributes
		{
			get { return itemsAttributes; }
		}
		public HashSet<String> ItemsInParams
		{
			get { return itemsInParams; }
		}


		private string path;

		Dictionary<string, DataBase> DataBases = new Dictionary<string, DataBase>();

		private object rowType = null;

		private Dictionary<string, Dictionary<string,string>> cols;

		private Dictionary<string, LayoutColumn> layoutColumns;


		public Dictionary<string, LayoutColumn> LayoutColumns
		{
			get
			{
				if (layoutColumns != null)
				{
					return layoutColumns;
				}
				else
				{
					return null;
				}
			}
		}

		public Dictionary<string, Dictionary<string, string>> Cols
		{
			get { return cols; }
		}

		private string rowClassString;
		private string classString;

		public object RowType
		{
			get
			{
				if (Cols == null)
				{
					Load();
				}

				return Cols;
			}
		}

		public CompilerResults results = null;

		public CompilerResults Results
		{
			get
			{
				if (results == null)
					Load();

				return results;
			}
		}

		public MetaMetaData(string path)
		{
			this.path = path;
		}

		public void Load()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			XmlNodeList metaChildNodes = doc.ChildNodes;
			foreach (XmlNode topLevelNode in metaChildNodes)
			{
				if (topLevelNode.Name == "d")
					LoadMetaStructure(topLevelNode);


			}
		}

		public void LoadMetaStructure(XmlNode databaseNode)
		{
			Dictionary<string, Dictionary<string, string>> attribs = new Dictionary<string, Dictionary<string, string>>();
			var excelNametoXmlName = new Dictionary<string, string>();
			var loadItemsAttributes = new HashSet<string>();
			var loadItemsInParams = new HashSet<string>();
			var loadDataBaseAttributes = new HashSet<string>();
			var loadCatAttributes = new  HashSet<string>();
			var loadScatAttributes = new  HashSet<String>();
			var xmlNametoExcelName = new Dictionary<string, string>();


			foreach (XmlNode dbItemNode in databaseNode.ChildNodes)
			{
				if (dbItemNode.Name == "a") //database attribute
				{
					string attribKey = databaseNode.Name + "_" + dbItemNode.Attributes["attribname"].Value;
					var itemAttribInfo = new Dictionary<string, string>();
					foreach (XmlNode attribute in dbItemNode.Attributes)
					{
						itemAttribInfo[attribute.Name] =attribute.Value;
					}

					attribs[attribKey] = itemAttribInfo;
					if (itemAttribInfo.ContainsKey("header"))
					{
						excelNametoXmlName[itemAttribInfo["header"]] = attribKey;
						xmlNametoExcelName[attribKey] = itemAttribInfo["header"];
						loadDataBaseAttributes.Add(itemAttribInfo["header"]);
					}

				}
				if (dbItemNode.Name == "cat") // represents a catterory
				{
					foreach (XmlNode catChild in dbItemNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "a"))
					{

						var itemAttribInfo = new Dictionary<string, string>();
						string attribKey = dbItemNode.Name + "_" + catChild.Attributes["attribname"].Value;
						foreach (XmlAttribute attrib in catChild.Attributes)
						{
							itemAttribInfo[attrib.Name] = attrib.Value;
						}
						attribs[attribKey] = itemAttribInfo;
						if (itemAttribInfo.ContainsKey("header"))
						{ 
							excelNametoXmlName[itemAttribInfo["header"]] = attribKey;
							xmlNametoExcelName[attribKey] = itemAttribInfo["header"];
						}
						if (itemAttribInfo.ContainsKey("header"))
							loadCatAttributes.Add(itemAttribInfo["header"]);

					}

					foreach (XmlNode catChild in dbItemNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "s_cat"))
					{
						foreach (XmlNode subCatChild in catChild.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "a"))
						{
							string attribKey = catChild.Name + "_" + subCatChild.Attributes["attribname"].Value;
							var itemAttribInfo = new Dictionary<string, string>();
							foreach (XmlAttribute attrib in subCatChild.Attributes)
							{
								itemAttribInfo[attrib.Name] = attrib.Value;
							}
							attribs[attribKey] = itemAttribInfo;
							if (itemAttribInfo.ContainsKey("header"))
							{ 
								excelNametoXmlName[itemAttribInfo["header"]] = attribKey;
								xmlNametoExcelName[attribKey] = itemAttribInfo["header"];
								loadScatAttributes.Add(itemAttribInfo["header"]);
							}
						}

						foreach (XmlNode subCatChild in catChild.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "i"))
						{

							foreach (XmlNode itemNode in subCatChild.ChildNodes.Cast<XmlNode>()
								.Where(x => x.Name == "a"))
							{
								string attribKey = subCatChild.Name + "_" + itemNode.Attributes["attribname"].Value;
								var itemAttribInfo = new Dictionary<string, string>();
								foreach (XmlAttribute attrib in itemNode.Attributes)
								{
									itemAttribInfo[attrib.Name] = attrib.Value;
								}
								attribs[attribKey] = itemAttribInfo;
								if (itemAttribInfo.ContainsKey("header"))
								{
									excelNametoXmlName[itemAttribInfo["header"]] = attribKey;
									xmlNametoExcelName[attribKey] = itemAttribInfo["header"];
									loadItemsAttributes.Add(itemAttribInfo["header"]);
								}
							}

							foreach (XmlNode itemNode in subCatChild.ChildNodes.Cast<XmlNode>()
								.Where(x => x.Name == "in"))
							{
								foreach (XmlNode inParamNode in itemNode.ChildNodes)
								{
									if (inParamNode.Name == "kval")
									{
										string attribKey = subCatChild.Name + "_" + inParamNode.Attributes["name"].Value;
										var itemAttribInfo = new Dictionary<string, string>();
										foreach (XmlAttribute attrib in inParamNode.Attributes)
										{
											itemAttribInfo[attrib.Name] = attrib.Value;
										}
										attribs[itemNode.Name + "_" + itemAttribInfo["name"]] = itemAttribInfo;
										if (itemAttribInfo.ContainsKey("header"))
										{
											excelNametoXmlName[itemAttribInfo["header"]] = attribKey;
											xmlNametoExcelName[attribKey] = itemAttribInfo["header"];
											loadItemsInParams.Add(itemAttribInfo["header"]);
										}
									}
									// now have all defined columns
									//var cols = GetFields(attribs, cat_attribs,
									//	subCatAttribs, itemAttribs, inputParams);
									//// are there are dups
									//var dups = cols.GroupBy(x => x).Where(x => x.Count() > 1)
									//	.Select(x => x.Key);
									//if (dups.Count() != 0)
									//{
									//	throw new Exception("There are duplicated Fields.");

									//}


								}


//itemsInParams["i_id"] = inputParams;
							}

						}

					}
				}

			}
			this.cols = attribs;
			ExcelNametoXmlName = excelNametoXmlName;
			itemsAttributes = loadItemsAttributes;
			itemsInParams = loadItemsInParams;
			dataBaseAttributes = loadDataBaseAttributes;
			catAttributes = loadCatAttributes;
			scatAttributes = loadScatAttributes;
			XmlNametoExcelName = xmlNametoExcelName;
		}

		private List<string> GetFields(List<string> databases, List<string> categories,
			List<string> subCategories, List<string> items, List<string> inputParams)
		{
			List<string> fields = new List<string>();
			foreach (var i in databases)
			{
				fields.Add(i.Replace(":", "__"));
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

		private object GetMetaRowTypeDef(List<string> fields)
		{
			List<DynaField> dynaFields = new List<DynaField>();
			foreach (var fieldname in fields)
			{
				dynaFields.Add(new DynaField(fieldname, Type.GetType("A")));
			}
			dynamic obj = new DynamicMetaClass(dynaFields);
			return obj;
		}


		public DataTable ToDataTableStructure(Dictionary<string, Dictionary<string, string>> fields)
		{
			var dt = new DataTable();
			var lcs = new Dictionary<string, LayoutColumn>();
			foreach (var kvpair in fields)
			{
				var lc = new LayoutColumn();
				lc.id = kvpair.Key;
				dt.Columns.Add(kvpair.Key, typeof(string));
				if (kvpair.Value.ContainsKey("header"))
				{
					dt.Columns[kvpair.Key].Caption = kvpair.Value["header"];
					lc.header = kvpair.Value["header"];
				}
				else
				{
					lc.header = kvpair.Key;
				}

				if (kvpair.Value.ContainsKey("visible"))
				{
					lc.visible = Boolean.Parse(kvpair.Value["visible"]);
					//dt.Columns[kvpair.Key].
				}
				else
				{
					lc.visible = false;
				}

				if (kvpair.Value.ContainsKey("width"))
				{
					lc.width = Int32.Parse(kvpair.Value["width"]);
				}
				else
				{
					lc.width = 60;
				}

				lcs[kvpair.Key] = lc;
			}

			layoutColumns = lcs;

			return dt;
		}



		private string GetDynaRowClassSource(List<string> fields)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("public class DynaRowData \n{\n");
			foreach (var field in fields)
			{
				sb.Append("\tpublic string ");
				sb.Append(field);
				sb.Append(" { get; set; }\n");
			}

			sb.Append("}");
			return sb.ToString();
		}

		public object GetNewRow()
		{
			string newRow = "new DynaRowData(); ";
			return DynaComplile(newRow);
		}

		private string GetUsings()
		{
			return @"using System;
					 using System.Collections.Generic; 
					";
		}
		private string GetDynaMetaSource()
		{
			IEnumerable<dynamic> items = null;
			var data = items.ToArray();
			return @"List<DynaRowData>  rowData";

		}

		private object DynaComplile(string source)
		{
			var compilerResults = new CSharpCodeProvider()
				.CompileAssemblyFromSource(
					new CompilerParameters
					{
						GenerateInMemory = true,
						ReferencedAssemblies =
						{
							"System.dll",
							Assembly.GetExecutingAssembly().Location
						}
					},
					source);
			return compilerResults;
		}
		private CompilerResults GenerateMetaAssembly(string source)
		{
			//https://softwareengineering.stackexchange.com/questions/93322/generating-a-class-dynamically-from-types-that-are-fetched-at-runtime

			string outputFileName = null;
			string status = null;
			CSharpCodeProvider csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
			CompilerParameters parameters = new CompilerParameters(new[] {"mscorlib.dll","System.Core.dll", "System.dll",
																			Assembly.GetExecutingAssembly().Location,
																			"mscorlib.dll", "netstandard.dll"
		},
				outputFileName, true);
			parameters.GenerateExecutable = false;
			var compileUnit = new CodeCompileUnit();
			var ns = new CodeNamespace("MetaMetaData");
			ns.Imports.Add(new CodeNamespaceImport("System"));


			CompilerResults results = csc.CompileAssemblyFromSource(parameters, source);
			if (results.Errors.HasErrors)
			{

				results.Errors.Cast<CompilerError>().ToList().ForEach(error => status += error.ErrorText + "\r\n");
				throw new Exception(status);


			}

			return results;
		}
	}
}
