using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;


namespace MetaDataEditor
{
	
	public class MetaCreator
	{
		static string rowClassName = "MetaRow";
		AssemblyName asemblyName;

		public void newRow()
		{
			this.asemblyName = new AssemblyName(rowClassName);
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(this.asemblyName, AssemblyBuilderAccess.Run);
		}
	}
}
