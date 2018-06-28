using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.Reflection.Emit;

namespace MetaDataEditor
{
	public class MetaData
	{
		HashSet<string> Names = new HashSet<string>();
		private List<Dictionary<string, string>> vals = new List<Dictionary<string, string>>();
		private List<Dictionary<string, string>> valsByColumn = new List<Dictionary<string, string>>();
		private Dictionary<string, string> dataBaseAttributes = new Dictionary<string, string>();
		private Dictionary<string, string> catAttributes;
		private string[] GridRowData;
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
				Names.Add(dbAttrib.Name);
			}
			foreach (XmlNode pCat in dataBase.ChildNodes)
			{
				catAttributes = new Dictionary<string, string>();
				if (pCat.Attributes != null)
				{
					foreach (XmlAttribute catAttrib in pCat.Attributes)
					{
						catAttributes[catAttrib.Name] = catAttrib.Value;
						Names.Add(catAttrib.Name);
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
								Names.Add(att.Name);
							}
						}

						if (item.HasChildNodes)
						{
							foreach (XmlNode inParam in item.ChildNodes)
							{
								itemRow[inParam.Name] = inParam.Name;
								Names.Add(inParam.Name);
							}
						}

						vals.Add(itemRow);
					}


				}
			}

			Type metaDataRowType = MetaDataRowType.CreateRowType("adcdls", Names.ToList());
			List<metaDataRowType> d

			foreach (var row in vals)
			{
				GridRowData = new string[Names.Count];

			}
			foreach (string ColName in Names)
			{
				
			}

			return false;
		}
		//https://stackoverflow.com/questions/41784393/how-to-emit-a-type-in-net-core
		object DefineARow(string newClassName, List<string> names)
		{
			AssemblyName aName = new AssemblyName("dynamic" + newClassName);
			AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder mb = ab.DefineDynamicModule("dynamic.dll");
			TypeBuilder tb = mb.DefineType(newClassName, TypeAttributes.AutoClass |
			                                             TypeAttributes.AnsiClass |
			                                             TypeAttributes.BeforeFieldInit |
			                                             TypeAttributes.AutoLayout,
														null);

			//Define your type here based on the info in your xml
			Type theType = tb.CreateType();
			//theType.a

			//instanciate your object
			ConstructorInfo ctor = theType.GetConstructor(Type.EmptyTypes);
			object inst = ctor.Invoke(new object[] { });

			PropertyInfo[] pList = theType.GetProperties(BindingFlags.DeclaredOnly);
			//iterate through all the properties of the instance 'inst' of your new Type
			foreach (PropertyInfo pi in pList)
				Console.WriteLine(pi.GetValue(inst, null));

			return new object();
		}
		public class FieldDescriptor
		{
			public FieldDescriptor(string fieldName, Type fieldType)
			{
				FieldName = fieldName;
				FieldType = fieldType;
			}
			public string FieldName { get; }
			public Type FieldType { get; }
		}

		public static class MetaDataRowType
		{
			public static Type CreateRowType(string newClass, List<String> dataItems)
			{
				var typeInfo = CompileResultTypeInfo(newClass, dataItems);
				var myType = typeInfo.AsType();
				return myType;
				
			}

			public static TypeInfo CompileResultTypeInfo(string newClass, List<String> dataItems)
			{
				TypeBuilder tb = GetTypeBuilder(newClass);
				ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);


				var fields = new List<FieldDescriptor>(dataItems.Count);
				foreach (var item in dataItems)
				{
					fields.Add(new FieldDescriptor(item, typeof(string)));
				}
				
				foreach (var field in fields)
					CreateProperty(tb, field.FieldName, field.FieldType);

				TypeInfo objectTypeInfo = tb.CreateTypeInfo();
				return objectTypeInfo;
			}

			private static TypeBuilder GetTypeBuilder(string newClassName)
			{
				var typeSignature =  newClassName;
				var an = new AssemblyName(typeSignature);
				var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);
				ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
				TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
					TypeAttributes.Public |
					TypeAttributes.Class |
					TypeAttributes.AutoClass |
					TypeAttributes.AnsiClass |
					TypeAttributes.BeforeFieldInit |
					TypeAttributes.AutoLayout,
					null);
				return tb;
			}

			private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
			{
				FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

				PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
				MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
				ILGenerator getIl = getPropMthdBldr.GetILGenerator();

				getIl.Emit(OpCodes.Ldarg_0);
				getIl.Emit(OpCodes.Ldfld, fieldBuilder);
				getIl.Emit(OpCodes.Ret);

				MethodBuilder setPropMthdBldr =
					tb.DefineMethod("set_" + propertyName,
						MethodAttributes.Public |
						MethodAttributes.SpecialName |
						MethodAttributes.HideBySig,
						null, new[] { propertyType });

				ILGenerator setIl = setPropMthdBldr.GetILGenerator();
				Label modifyProperty = setIl.DefineLabel();
				Label exitSet = setIl.DefineLabel();

				setIl.MarkLabel(modifyProperty);
				setIl.Emit(OpCodes.Ldarg_0);
				setIl.Emit(OpCodes.Ldarg_1);
				setIl.Emit(OpCodes.Stfld, fieldBuilder);

				setIl.Emit(OpCodes.Nop);
				setIl.MarkLabel(exitSet);
				setIl.Emit(OpCodes.Ret);

				propertyBuilder.SetGetMethod(getPropMthdBldr);
				propertyBuilder.SetSetMethod(setPropMthdBldr);
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
}
