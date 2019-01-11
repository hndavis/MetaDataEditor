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
using DevExpress.Utils.Extensions;

namespace MetaDataEditor
{
	public class MetaMetaData
	{
		private Dictionary<string, List<string>> dataBaseAttributes = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> catAttributes = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> scatAttributes = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> itemsAttributes = new Dictionary<string, List<string>>();
		private Dictionary<string, List<string>> itemsInParams = new Dictionary<string, List<string>>();

		public Dictionary<string, List<string>> DataBaseAttributes
		{
			get { return dataBaseAttributes; }
		}

		public Dictionary<string, List<string>> CatAttributes
		{
			get { return catAttributes; }
		}

		public Dictionary<string, List<string>> ScatAttributes
		{
			get { return scatAttributes; }
		}

		public Dictionary<string, List<string>> ItemsAttributes
		{
			get { return itemsAttributes; }
		}
		public Dictionary<string, List<string>> ItemsInParams
		{
			get { return itemsInParams; }
		}


		private string path;

		Dictionary<string, DataBase> DataBases = new Dictionary<string, DataBase>();

		private object rowType = null;

		private List<string> cols;

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

		public List<string> Cols
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

		public void LoadLayout(XmlNode layoutNode)
		{
			Dictionary<string, LayoutColumn> columnLayout = new Dictionary<string, LayoutColumn>();
			foreach (XmlNode colNode in layoutNode.ChildNodes)
			{
				var col = new LayoutColumn();
				col.id = colNode.Attributes["id"].Value;
				col.header = colNode.Attributes["header"].Value;
				col.visible = Boolean.Parse(colNode.Attributes["visible"].Value);
				col.width = Int32.Parse(colNode.Attributes["width"].Value);

				columnLayout.Add(col.id, col);
			}

			layoutColumns = columnLayout;

		}
		public void LoadMetaStructure(XmlNode databaseNode)
		{
			List<string> attribs = new List<string>();
			foreach (XmlNode dbItemNode in databaseNode.ChildNodes)
			{
				if (dbItemNode.Name == "layout")
					LoadLayout(dbItemNode);

				if (dbItemNode.Name == "a") //database attribute
				{
					string attrib = databaseNode.Name + "_" + dbItemNode.Attributes[0].Value;
					attribs.Add(attrib);

				}
				if (dbItemNode.Name == "cat") // represents a catterory
				{
					dataBaseAttributes["d_id"] = attribs;
					List<string> cat_attribs = new List<string>();
					foreach (XmlNode catChild in dbItemNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "a"))
					{
						cat_attribs.Add(dbItemNode.Name + "_" + catChild.Attributes[0].Value);
					}
					catAttributes["c_id"] = cat_attribs;
					foreach (XmlNode catChild in dbItemNode.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "s_cat"))
					{

						List<string> subCatAttribs = new List<string>();
						foreach (XmlNode subCatChild in catChild.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "a"))
						{

							subCatAttribs.Add(catChild.Name + "_" + subCatChild.Attributes[0].Value);
						}

						scatAttributes["scat_id"] = subCatAttribs;
						foreach (XmlNode subCatChild in catChild.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "i"))
						{
							List<string> itemAttribs = new List<string>();
							foreach (XmlNode itemNode in subCatChild.ChildNodes.Cast<XmlNode>()
								.Where(x => x.Name == "a"))
							{
								itemAttribs.Add(subCatChild.Name + "_" + itemNode.Attributes[0].Value);
							}

							itemsAttributes["i_id"] = itemAttribs;
							foreach (XmlNode itemNode in subCatChild.ChildNodes.Cast<XmlNode>()
								.Where(x => x.Name == "in"))
							{
								List<string> inputParams = new List<string>();
								foreach (XmlNode inParamNode in itemNode.ChildNodes)
								{
									if (inParamNode.Name == "kval")
									{
										inputParams.Add(
											itemNode.Name + "_" + inParamNode.Attributes[0].Value);

									}
									// now have all defined columns
									var cols = GetFields(attribs, cat_attribs,
										subCatAttribs, itemAttribs, inputParams);
									// are there are dups
									var dups = cols.GroupBy(x => x).Where(x => x.Count() > 1)
										.Select(x => x.Key);
									if (dups.Count() != 0)
									{
										throw new Exception("There are duplicated Fields.");

									}

									this.cols = cols;
								}

								itemsInParams["i_id"] = inputParams;
							}

						}

					}
				}

			}
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


		public DataTable ToDataTableStructure(List<string> fields)
		{
			var dt = new DataTable();
			foreach (var fieldname in fields)
				dt.Columns.Add(fieldname, typeof(string));


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
