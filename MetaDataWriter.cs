﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace MetaDataEditor
{
	public class MetaDataWriter
	{
		private MetaMetaData metaMetaData;
		private string fileName;
		private DataTable metaDataTable;
		private int indentLevel;

		public MetaDataWriter(String fileName, MetaData metaData)
		{
			throw new Exception("Not Implemented");
		}

		public MetaDataWriter(string fileName, MetaMetaData metaMetaData, DataTable metaDataTable)
		{
			this.fileName = fileName;
			this.metaMetaData = metaMetaData;
			this.metaDataTable = metaDataTable;
		}


		public void Write()
		{

			using (StreamWriter outStream = new StreamWriter(fileName))
			{

				// or sort by db, cat, sub

				DataView dv = metaDataTable.DefaultView;
				dv.Sort = "d_id, cat_id, s_cat_id, i_id";

				indentLevel = 0;
				string currentDb = "", currentCat = "", currentSubCat = "";
				foreach (DataRow row in dv.ToTable().Rows)
				{
					//outStream.Write("<!-- " + row["d_id"] + "\t" + row["cat_id"] + "\t" + row["s_cat_id"] + "\t" +
					//				"-->" + Environment.NewLine);
					//outStream.Flush();

					if (currentDb != (string) row["d_id"]) // write the complete database
					{
						currentDb = (string) row["d_id"];
						//write the database node

						outStream.Write(BeginDatabaseNode(row));
					}

					if (currentCat != (string) row["cat_id"])
					{
						if (!string.IsNullOrEmpty(currentCat))
						{
							outStream.Write(EndCategoryNode(row));

						}

						currentCat = (string) row["cat_id"];
						//write the complete category
						outStream.Write(BeginCategoryNode(row));

					}

					if (currentSubCat != (string) row["s_cat_id"]) // write the complete contents of the sub cat
					{
						string prevSubCat = currentSubCat;

						if (!row.IsNull("s_cat_id")  )
						{
						
							currentSubCat = (string) row["s_cat_id"];
						}

						if (prevSubCat != currentSubCat && !string.IsNullOrEmpty(prevSubCat))
							outStream.Write(EndSCategoryNode(row));


						// write the catagory node
						if (prevSubCat != currentSubCat )
							outStream.Write(BeginSCategoryNode(row));
					}

					//write the item
					outStream.Write(BeginItemNode(row));
					outStream.Write(EndItemNode(row));

				}

				outStream.Write(EndSCategoryNode());
				outStream.Write(EndCategoryNode());
				outStream.Write(EndDatabaseNode());
			}

		}


		private string BeginDatabaseNode(DataRow row)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < indentLevel; i++)
				sb.AppendFormat("\t");
			indentLevel++;
			sb.AppendFormat("<d ");
			foreach (string fld in metaMetaData.DataBaseAttributes["d_id"])
			{
				sb.AppendFormat("{0}=\"{1}\" ", fld.Replace("d_", "").Replace("__", ":"), row[fld]);
			}

			sb.AppendFormat(">" + Environment.NewLine);
			return sb.ToString();
		}

		private string EndDatabaseNode(DataRow row = null)
		{
			indentLevel--;
			return "</d>" + Environment.NewLine;
		}

		private string BeginCategoryNode(DataRow row)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < indentLevel; i++)
				sb.AppendFormat("\t");
			indentLevel++;
			sb.AppendFormat("<c ");
			foreach (string fld in metaMetaData.CatAttributes["c_id"])
			{
				sb.AppendFormat("{0}=\"{1}\" ", fld.Replace("c_", ""), row[fld]);
			}

			sb.AppendFormat(">" + Environment.NewLine);
			return sb.ToString();
		}

		private string EndCategoryNode(DataRow row = null)
		{
			return EndSCategoryNode(row);
		}

		private string BeginSCategoryNode(DataRow row)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < indentLevel; i++)
				sb.AppendFormat("\t");
			sb.AppendFormat("<c ");
			foreach (string fld in metaMetaData.ScatAttributes["scat_id"])
			{
				sb.AppendFormat("{0}=\"{1}\" ", fld.Replace("scat_", ""), row[fld]);
			}

			sb.AppendFormat(">" + Environment.NewLine);
			return sb.ToString();
		}

		private string EndSCategoryNode(DataRow row = null)
		{
			indentLevel--;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < indentLevel; i++)
				sb.AppendFormat("\t");
			indentLevel--;
			sb.Append("</c>" + Environment.NewLine);
			return sb.ToString();
		}

		private string BeginItemNode(DataRow row)
		{
			indentLevel++;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < indentLevel; i++)
				sb.AppendFormat("\t");

			sb.AppendFormat("<i ");
			foreach (string itm in metaMetaData.ItemsAttributes["i_id"])
			{
				if (! row.IsNull(itm))
					sb.AppendFormat("{0}=\"{1}\" ", itm.Replace("i_", ""), row[itm]);
			}
			sb.AppendFormat(">" + Environment.NewLine);

			foreach (string inParam in metaMetaData.ItemsInParams["i_id"])
			{
				for (int i = 0; i < indentLevel; i++)
					sb.AppendFormat("\t");
				sb.AppendFormat("\t");

				if (!row.IsNull(inParam))
				{
					sb.AppendFormat("<in:sp k=\"{0}\">{1}</in:sp>" + Environment.NewLine, inParam.Replace("in_", ""),
						row[inParam]);
				}

			}
			sb.Append(Environment.NewLine);

			return sb.ToString();
		}
		private string EndItemNode(DataRow row = null)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < indentLevel; i++)
				sb.AppendFormat("\t");
			indentLevel--;
			sb.Append( "</i>" + Environment.NewLine);
			return sb.ToString();
		}
	}
}
