using MySql.Data.MySqlClient;
using System;
using System.Data;

public class SqlTool
{
    public static MySqlConnection mySqlConnection;//连接类对象

    private static string host;     //IP地址。如果只是在本地的话，写localhost就可以。
    private static string id;       //用户名。
    private static string pwd;      //密码。
    private static string dataBase; //数据库名称。

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="_host">IP地址</param>
    /// <param name="_id">用户名</param>
    /// <param name="_pwd">密码</param>
    /// <param name="_dataBase">数据库名称</param>
    public SqlTool(string _host, string _id, string _pwd, string _dataBase)
    {
        host = _host;
        id = _id;
        pwd = _pwd;
        dataBase = _dataBase;
        OpenSql();
    }

    /// <summary>  
    /// 打开数据库  
    /// </summary>  
    public static void OpenSql()
    {
        try
        {
            //string.Format是将指定的 String类型的数据中的每个格式项替换为相应对象的值的文本等效项。  
            string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};CharSet=utf8;", dataBase, host, id, pwd, "3306");
            mySqlConnection = new MySqlConnection(mySqlString);
            mySqlConnection.Open();
        }
        catch (Exception e)
        {
            throw new Exception("服务器连接失败，请重新检查是否打开MySql服务。" + e.Message.ToString());
        }
    }

    /// <summary>  
    /// 创建表  
    /// </summary>  
    /// <param name="name">表名</param>  
    /// <param name="colName">属性列</param>  
    /// <param name="colType">属性类型</param>  
    /// <returns></returns>  
    public DataSet CreateTable(string name, string[] colName, string[] colType)
    {
        if (colName.Length != colType.Length)
        {
            throw new Exception("输入不正确：" + "columns.Length != colType.Length");
        }
        string query = "CREATE TABLE  " + name + "(" + colName[0] + " " + colType[0];
        for (int i = 1; i < colName.Length; i++)
        {
            query += "," + colName[i] + " " + colType[i];
        }
        query += ")";
        return QuerySet(query);
    }

    /// <summary>  
    /// 创建具有id自增的表  
    /// </summary>  
    /// <param name="name">表名</param>  
    /// <param name="col">属性列</param>  
    /// <param name="colType">属性列类型</param>  
    /// <returns></returns>  
    public DataSet CreateTableAutoID(string name, string[] col, string[] colType)
    {
        if (col.Length != colType.Length)
        {
            throw new Exception("columns.Length != colType.Length");
        }
        string query = "CREATE TABLE  " + name + " (" + col[0] + " " + colType[0] + " NOT NULL AUTO_INCREMENT";
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ", PRIMARY KEY (" + col[0] + ")" + ")";
        return QuerySet(query);
    }

    /// <summary>  
    /// 插入一条数据，包括所有，不适用自动累加ID。  
    /// </summary>  
    /// <param name="tableName">表名</param>  
    /// <param name="values">插入值</param>  
    /// <returns></returns>  
    public DataSet InsertInto(string tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + "'" + values[i] + "'";
        }
        query += ")";
        return QuerySet(query);
    }

    /// <summary>  
    /// 插入部分ID  
    /// </summary>  
    /// <param name="tableName">表名</param>  
    /// <param name="col">属性列</param>  
    /// <param name="values">属性值</param>  
    /// <returns></returns>  
    public DataSet InsertInto(string tableName, string[] col, string[] values)
    {
        if (col.Length != values.Length)
        {
            throw new Exception("columns.Length != colType.Length");
        }
        string query = "INSERT INTO " + tableName + " (" + col[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i];
        }
        query += ") VALUES (" + "'" + values[0] + "'";
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + "'" + values[i] + "'";
        }
        query += ")";
        return QuerySet(query);
    }

    /// <summary>  
    /// 查询表数据 
    /// </summary>  
    /// <param name="tableName">表名</param>  
    /// <param name="items">需要查询的列</param>  
    /// <param name="whereColName">查询的条件列</param>  
    /// <param name="operation">条件操作符</param>  
    /// <param name="value">条件的值</param>  
    /// <returns></returns>  
    public DataSet Select(string tableName, string[] items, string[] whereColName, string[] operation, string[] value)
    {
        if (whereColName.Length != operation.Length || operation.Length != value.Length)
        {
            throw new Exception("输入不正确：" + "col.Length != operation.Length != values.Length");
        }
        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            query += "," + items[i];
        }
        query += "  FROM  " + tableName + "  WHERE " + " " + whereColName[0] + operation[0]  + value[0];
        for (int i = 1; i < whereColName.Length; i++)
        {
            query += " AND " + whereColName[i] + operation[i] + value[i] ;
        }

        //MessageBox.Show(query);
        //Debug.Log(query);
        return QuerySet(query);
    }

    /// <summary>  
    /// 更新表数据 
    /// </summary>  
    /// <param name="tableName">表名</param>  
    /// <param name="cols">更新列</param>  
    /// <param name="colsvalues">更新的值</param>  
    /// <param name="selectkey">条件：列</param>  
    /// <param name="selectvalue">条件：值</param>  
    /// <returns></returns>  
    public DataSet UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += ", " + cols[i] + " =" + colsvalues[i];
        }
        query += " WHERE " + selectkey + " = " + selectvalue + " ";
        return QuerySet(query);
    }

    /// <summary>  
    /// 删除表数据  
    /// </summary>  
    /// <param name="tableName">表名</param>  
    /// <param name="cols">条件：删除列</param>  
    /// <param name="colsvalues">删除该列属性值所在得行</param>  
    /// <returns></returns>  
    public DataSet Delete(string tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " or " + cols[i] + " = " + colsvalues[i];
        }
        return QuerySet(query);
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Close()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
        }
    }

    /// <summary>    
    /// 执行Sql语句  
    /// </summary>  
    /// <param name="sqlString">sql语句</param>  
    /// <returns></returns>  
    public static DataSet QuerySet(string sqlString)
    {
        if (mySqlConnection.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                mySqlDataAdapter.Fill(ds);
            }
            catch (Exception e)
            {
                throw new Exception("SQL:" + sqlString + "/n" + e.Message.ToString());
            }
            finally
            {
            }
            return ds;
        }
        return null;
    }




}