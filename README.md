# DbKit

ADO.NET数据库开发连接辅助工具。

A helpful database kit for ADO.NET development.

## 简介 Introduction

这是一个专门为ADO.NET开发的辅助类工具，目前已完成对 Microsoft SQL Server 数据库的增删改查操作，支持连接模式和断开模式的操作。基于抽象工厂模式（Abstract factory pattern）和反射（Reflection）的原理进行封装（encapsulation）。

This is a specialized kit for ADO.NET development, has supported the common data management for Microsoft SQL Server database in connection mode or disconnect mode.And encapsulating it based on **abstract factory pattern and reflection principle.

## 开始 Start

可用以下方式获取`DbKit.cs`文件 Several start options are available:：

- [点击下载 Click here to download](https://codeload.github.com/tatwd/dbkit/zip/v2-dev)

- 使用`git`工具克隆 Use `git` to clone the repo：
  
  ``` bash
  $ git clone https://github.com/tatwd/dbkit.git 
  ```

推荐使用第二种方式。Recommend the second way.

## 用法 Usage

### DbHelper 类

该类只定义了一个属性和一个方法，用于获取一个连接对象。

#### 属性 

- **ConnectorClassName**
  
  只读的静态私有属性，表示连接器类名。

#### 方法

- **GetConnector(params string[] dbConnStrName)**
  
  静态公共方法，用来获取一个对应用户数据库类型的连接器对象，目前支持最多传入一个参数。

  **参数**：

  `dbConStrname`：可变参数数组，表示在配置文件中的连接字符串名。

  **返回值**：
  
  连接器（Connector）。

  **举例**：

  ``` c#
  using DbKit;

  Connector connector = DbHelper.GetConnector("MyDB");
  ```
  
### Connector 接口

该接口定义了6个接口方法，用于对数据库的管理。

#### 方法

- **Execute(string executeType, string cmdText, params object[] cmdParams)**
  
  用来执行执行SQL语句（支持带安全参数）并返回执行的结果，传入三种参数。

  **参数**：
  
  `executeType`：表示执行类型，有3个值：`Non`、`Reader`、`Scalar`、`Xml`，即对应执行 ExecuteNonQuery、ExecuteReader、ExecuteScalar、ExecuteXmlReader 方法，[参考文档](https://msdn.microsoft.com/zh-cn/library/system.data.sqlclient.sqlcommand.aspx)。

  `cmdText`：表示命令文本，可以是SQL语句或存储过程，但`此方法只支持SQL语句的执行`。

  `cmdParams`：表示命令参数对象的可变数组，SQL语句带安全参数时传入。

  **返回值**：

  执行结果，object类型数据。赋值时需类型转换（拆箱）。

  **举例**：

  ``` c#
  // 先创建一个连接器
  Connector connector = DbHelper.GetConnector("MyDB");

  string username = "John";
  string password = "test";

  // 设置安全参数
  SqlParameter[] parameter = new SqlParameter[]
  {
    new SqlParameter("@username", username),
    new SqlParameter("@password", password)
  };

  // 插入语句
  string insert           = String.Format("insert into [user_info] values('{0}', '{1}')", username, password);
  string insertWithParams = "insert into [user_iinfo] values(@username, @password)";

  // 查询语句
  
  // 获取所有信息的查询
  string query1           = String.Format("select * frome [user_info] where [username] = '{0}' and [password] = '{1}'", username, password);
  string query1WithParams = "select * frome [user_info] where [username] = @username and [password] = @password";
  
  // 获取单值的信息查询
  string query2           = String.Format("select [user_id] frome [user_info] where [username] = '{0}' and [password] = '{1}'", username, password);
  string query2WithParams = "select [user_id] frome [user_info] where [username] = @username and [password] = @password";


  // 插入
  // 返回值为受影响行数，当不需要获取返回行数时，允许不赋值

  // connector.Execute(insert) 
  int line1 = (int)connector.Execute("non", insert);
  
  // 带安全参数的插入操作
  // connector.Execute(insertWithParams, parameter)
  int line2 = (int)connector.Execute("non", insertWithParams, parameter);

  // 删除、更新操作同上

  // 查询

  // 查询所有信息
  SqlDaTaReader reader1 = (SqlDaTaReader)connector.Execute("reader", query1);
  SqlDaTaReader reader2 = (SqlDaTaReader)connector.Execute("reader", query1WithParams, parameter); // 带安全参数的插入操作

  // 单值的查询
  int id1 = (int)connector.Execute("scalar", query2);
  int id2 = (int)connector.Execute("scalar", query2WithParams, parameter);                         // 带安全参数的插入操作

  ```

- **Execute(string executeType, string cmdText, CommandType cmdType, params object[] cmdParams)**

  上一个 Execute 方法的重载，支持执行存储过程，增加一个新参数。

  **参数**：

  `cmdType`：命令类型，有3个值：`CommandType.StoredProcedure`（存储过程）、`CommandType.Text`（SQL语句）、`TableDirect`（数据表）,`此方法支持前两个类型`，[参考文档](https://msdn.microsoft.com/zh-cn/library/system.data.commandtype(VS.80).aspx)。

  **返回值**：

  执行结果，object类型数据。赋值时需类型转换（拆箱）。

- **GetDataTable(string cmdText, params object[] cmdParams)**

  断开模式下，用于获取数据表（DataTable）。

  **参数**：

  `cmdText`：表示命令文本，可以是SQL语句或存储过程，但**此方法只支持SQL语句的执行**。

  `cmdParams`：表示命令参数对象的可变数组，SQL语句带安全参数时传入。

  **返回值**：

  数据表，DataTable类型。

- **GetDataTable(string cmdText, CommandType cmdType, params object[] cmdParams)**

  上一个方法的重载，支持执行存储过程，增加一个参数。

  **参数**：

  `cmdType`：命令类型，有3个值：`CommandType.StoredProcedure`（存储过程）、`CommandType.Text`（SQL语句）、`TableDirect`（数据表）,`此方法支持前两个类型`，[参考文档](https://msdn.microsoft.com/zh-cn/library/system.data.commandtype(VS.80).aspx)。

  **返回值**：

  数据表，DataTable类型。

- **DataSet GetDataSet(string cmdText, params object[] cmdParams)**

  断开模式下，用于获取数据集（DataSet）。

  **参数**:
  
  `cmdText`：表示命令文本，可以是SQL语句或存储过程，但**此方法只支持SQL语句的执行**。

  `cmdParams`：表示命令参数对象的可变数组，SQL语句带安全参数时传入。


  **返回值**：

  数据集，DataSet类型。  

- **GetDataSet(string cmdText, CommandType cmdType, params object[] cmdParams)**

  上一个方法的重载，支持执行存储过程，增加一个参数。

  **参数**:

  `cmdType`：命令类型，有3个值：`CommandType.StoredProcedure`（存储过程）、`CommandType.Text`（SQL语句）、`TableDirect`（数据表）,`此方法支持前两个类型`，[参考文档](https://msdn.microsoft.com/zh-cn/library/system.data.commandtype(VS.80).aspx)。

  **返回值**：

  数据集，DataSet类型。

### MsSqlConnector 类

  Microsoft SQL Server 数据库连接器，使用 SQL Server 数据提供程序，是对 Connector 接口的实现。

## 计划 Plans

- 实现 MySQL 数据库连接器（MySqlConnector）。

- 异步管理数据。

----

Loading... :smile: