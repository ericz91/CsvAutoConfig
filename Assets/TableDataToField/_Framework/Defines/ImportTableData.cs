﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
public class ImportTableData
{


    public static string TABLE_PARAMS = "table_param";
   
	
    public static object MUTEX_OBJECT = new object();

   
	public static void importDataFromTableByAttribute(object  destObject)
	{
		lock(MUTEX_OBJECT)
        {
            FieldInfo[] fields = destObject.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                foreach (object attribute in field.GetCustomAttributes(true))
                {
                    Type type = attribute.GetType();
                    if (type.Equals(Type.GetType("System.TableAttribute")))
                    {
                        FieldInfo[] tableAttribute = type.GetFields();
                        string tableName = tableAttribute[0].GetValue(attribute).ToString();
                        string primaryKey = tableAttribute[1].GetValue(attribute).ToString();
                        string key = tableAttribute[2].GetValue(attribute).ToString();
                        string value = ReadTable.GetInstance().GetValue(tableName,primaryKey,key);
                        field.SetValue(destObject,TypeDescriptor.GetConverter(field.FieldType).ConvertFrom(value));
                    }
                }
            }
        }
	}
    /// <summary>
    /// 横向配表函数
    /// </summary>
    /// <param name="destObject"></param>
    /// <param name="tableName"></param>
    /// <param name="mainKey"></param>
	public static void importDataFromTable(object destObject, string tableName, string mainKey)
	{
        lock(MUTEX_OBJECT)
        {
            Dictionary<string, Dictionary<string, string>> tmp_tables = ReadTable.GetInstance().GetTable(tableName);
            if(!tmp_tables.ContainsKey(mainKey))
                return;
            List<FieldInfo> table_params = new List<FieldInfo>();
            FieldInfo[] field_infos = destObject.GetType().GetFields();
            foreach (FieldInfo tmp in field_infos)
	        {
		        if(tmp.Name.Contains(TABLE_PARAMS))
                {
                    table_params.Add(tmp);
                }
	        }
            foreach (FieldInfo tmp in table_params)
	        {
		        tmp.SetValue(destObject,TypeDescriptor.GetConverter(tmp.FieldType).ConvertFrom(tmp_tables[mainKey][tmp.Name]));
	        }
        }
	}
    /// <summary>
    /// 竖向配表函数
    /// </summary>
    /// <param name="destObject"></param>
    /// <param name="tableName"></param>
    /// <param name="valueName"></param>
	public static void improtDataFromTable(object destObject, string tableName, string valueName)
	{
        lock(MUTEX_OBJECT)
        {
            Dictionary<string, Dictionary<string, string>> tmp_tables = ReadTable.GetInstance().GetTable(tableName);
            List<FieldInfo> table_params = new List<FieldInfo>();
            FieldInfo[] field_infos = destObject.GetType().GetFields();
            foreach (FieldInfo tmp in field_infos)
            {
                if (tmp.Name.Contains(TABLE_PARAMS))
                {
                    table_params.Add(tmp);
                }
            }
            foreach (FieldInfo tmp in table_params)
            {
                if (!tmp_tables.ContainsKey(tmp.Name))
                    continue;
                tmp.SetValue(destObject,TypeDescriptor.GetConverter(tmp.FieldType).ConvertFrom(tmp_tables[tmp.Name][valueName]));
            }
        }
	}

}

