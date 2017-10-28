using System;

// using System.Collections.Generic;
// using System.Linq;
// using System.Web;

using System.Reflection;      // must 

using System.Configuration;   // must

using System.Data;
using System.Data.SqlClient;  // for 'mssql'

using MySql.Data.MySqlClient; // for 'mysql'

namespace DbKit
{
    // 抽象工厂模式（Abstract factory pattern） + 反射（Reflection）
    //

    // Summary:
    //   ConnectorFactory - 连接器生产工厂
    //
    public class ConnecterFactory
    {
        // Summary:
        //   连接器类名
        //   如果在Web.config中设置了数据库类型（DbType）的节点，则可依下列方法从中读取：
        //    
        //   private static string ConnectorClassName
        //   {
        //       set
        //       {
        //           string className = ConfigurationManager.AppSettings["DbType"]; // 获取Web.config中的数据库类型
        //
        //           if (className.Equals("MSSQL", StringComparison.CurrentCultureIgnoreCase)) // 忽略大小写
        //           {
        //               value = "DbKit.MsSqlConnector"; // for mssql
        //           }
        //           else (className.Equals("MYSQL", StringComparison.CurrentCultureIgnoreCase))
        //           {
        //               value = "DbKit.MySqlConnector"; // for mysql
        //           }
        //       }
        //       get
        //       {
        //           return ConnectorClassName;
        //       }
        //   }
        //
        private static readonly string ConnectorClassName = "DbKit.MsSqlConnector";

        // Summary:
        //   获取当前程序集全名，只读
        // private static readonly string AssemblyName = Assembly.GetExecutingAssembly().FullName;

        // Summary:
        //   根据数据库类型，获取一个通用连接数据库对象
        //
        // Parameters:
        //   dbConnStrName:
        //     在Web.config中设置的数据库连接字符串名称，可变参数数组
        //
        public static Connector GetConnector(params string[] dbConnStrName)
        {
            // return (Connector)Assembly.Load(AssemblyName).CreateInstance(ConnectorClassName); // 利用Assembly反射创建一个连接器

            //
            // 反射实现带参类的创建

            Type classType = Type.GetType(ConnectorClassName); // 获取构造类型

            object[] parameter = (dbConnStrName.Length == 1) ? dbConnStrName : null; // 设置构造函数参数，最多传入一个参数

            return (Connector)Activator.CreateInstance(classType, parameter); // 利用用Activator反射创建类
        }

        // -------------------------------------
        // Extend other connector creators here.
        // -------------------------------------

        // User ...
    }

    // Summary
    //   Connector 连接器 - 接口, 定义了一些与ADO.NET相关的通用方法，包含连接模式（Maybe OK）和断开模式（Maybe OK）
    //
    // TODO: 实行异步操作
    //
    public interface Connector
    {
        // Summary:
        //   连接数据库
        void Connect();

        // Summary:
        //   完成连接字符串的获取、连接对象的创建及打开数据库
        //
        // Parameters:
        //   dbConnStrName:
        //     在Web.config中设置的数据库连接字符串名称
        void Connect(string dbConnStrName);

        // Summary:
        //   获取ExecuteReader执行的结果
        //
        // Parameters:
        //   cmdText:
        //     SQL语句字符串
        //
        //   parameter:
        //     SQL参数数组
        //
        // Returns:
        //   返回DataReader数据集
        object GetDataReader(string cmdText, params object[] parameter);

        // Summary:
        //   获取ExecuteScalar执行的结果
        //
        // Parameters:
        //   cmdText:
        //     SQL语句字符串
        //
        //   parameter:
        //     SQL参数数组
        //
        // Returns:
        //   返回一个object类型的数据
        object GetScalar(string cmdText, params object[] parameter);

        // Summary:
        //   利用泛型对数据库数据进行增、删、改、查管理
        // 
        // Parameters:
        //   excuteType:
        //     执行类型，三个值
        //       0 - 执行增、删、改操作并返回受影响行数
        //       1 - 执行单条语句查询并返回第一行第一列
        //       2 - 执行查询语句并返回DataReader数据集
        //
        //   cmdText:
        //     SQL语句，带参或不带参
        //
        //   parameter:
        //     SQL参数数组
        //
        // Returns:
        //   返回T类型数据
        //
        T ManageData<T>(int executeType, string cmdText, params object[] parameter);

        // Summary:
        //   利用泛型对数据库数据进行增、删、改、查管理
        // 
        // Parameters:
        //   excuteType:
        //     执行类型，三个值：
        //       0 - 执行增、删、改操作并返回受影响行数
        //       1 - 执行单条语句查询并返回第一行第一列
        //       2 - 执行查询语句并返回DataReader数据集
        //
        //   procdureName:
        //     存储过程名
        //   
        //   securityType:
        //     安全类型 
        // 
        //   parameter:
        //     SqlParameter参数数组
        //
        //  TODO: how to declare this method?
        //
        // T ManageData<T>(int executeType, string procdureName, string securityType, params object[] parameter);

        // Summary:
        //   断开模式查询数据库，支持带参查询
        //
        // Parameters:
        //   cmdText:
        //     查询语句字符串
        //
        //   parameter:
        //     SQL参数数组
        // 
        // Returns:
        //   返回DataSet数据集
        DataSet GetDataSet(string cmdText, params object[] parameter);

        // Summary:
        //   断开模式下获取数据表
        // 
        // Parameters:
        //   cmdText:
        //     SQL语句字符串
        //
        //   parameters:
        //     SQL参数数组
        //
        // Returns:
        //   返回DataTable数据集
        DataTable GetDataTable(string cmdText, params object[] parameter);

        // Summary:
        //   将设置好的数据表（dataTable）更新到数据库表（tableName）中
        // 
        // Parameters:
        //   tableName:
        //     数据库中表名
        // 
        //   dataRow:
        //     已设置好的数据表行
        void ManageDataOffMode(string manageType, string tableName, params object[] paramter);

        // Summary:
        //   判断数据库中是否已存在该项
        //
        // Parameters:
        //   cmdText:
        //     SQL语句，带参或不带参
        //
        //   parameter:
        //     SQL参数数组
        //
        // Returns:
        //   返回bool值，true表示存在，false表示不存在
        //
        bool HasData(string cmdText, params object[] parameter);

        // Summary:
        //   打开数据库
        void OpenDb();

        // Summary:
        //   关闭数据库
        void CloseDb();

        // Summary:
        //   关闭所有资源
        void CloseAll();
    }

    // Summary:
    //   MySqlConnector - MySql连接器
    //
    // TODO 
    //
    public class MySqlConnector : Connector
    {
        // Override
        public void Connect() { }

        // Override
        public void Connect(string dbName)
        {
            throw new NotImplementedException();
        }

        // Override
        public object GetDataReader(string sql, params object[] parameter)
        {
            throw new NotImplementedException();
        }

        // Override
        public object GetScalar(string sql, params object[] parameter)
        {
            throw new NotImplementedException();
        }

        // Override
        public T ManageData<T>(int executeType, string cmdText, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        // Override
        public DataSet GetDataSet(string cmdText, params object[] parameter)
        {
            throw new NotImplementedException();
        }

        // Override
        public DataTable GetDataTable(string cmdText, params object[] parameter)
        {
            throw new NotImplementedException();
        }

        // Override
        public void ManageDataOffMode(string manageType, string tableName, params object[] paramters)
        {
            throw new NotImplementedException();
        }

        // Override
        public bool HasData(string cmdText, params object[] parameter)
        {
            throw new NotImplementedException();
        }

        // Override
        public void OpenDb()
        {
            throw new NotImplementedException();
        }

        // Override
        public void CloseDb()
        {
            throw new NotImplementedException();
        }

        // Override
        public void CloseAll()
        {
            throw new NotImplementedException();
        }
        
    }
}