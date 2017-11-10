# DbKit

ADO.NET数据库开发连接辅助工具。

A helpful database kit for ADO.NET development.

## 简介 Introduction

这是一个专门为ADO.NET开发的辅助类工具，目前已完成对`Microsoft SQL Server` 数据库`增`、`删`、`改`、`查`操作，支持`连接模式`和`断开模式`的操作。

This is a specialized kit for ADO.NET development, has supported the common data management for `Microsoft SQL Server` database in `connection mode` or `disconnect mode`.

## 开始 Start

可用以下方式获取`DbKit.cs`文件 Several start options are available:：

- [点击下载 Click here to download](https://codeload.github.com/tatwd/dbkit/zip/v2-dev)

- 使用`git`工具克隆 Use `git` to clone the repo：
  
  ``` bash
  $ git clone https://github.com/tatwd/dbkit.git 
  ```
推荐使用第二种方式。Recommend the second way.

## 用法 Usage

### `DbHelper`类

该类只定义了一个属性和一个方法：

- #### `ConnectorClassName`
  
  只读的静态私有属性，表示连接器类名。

- #### `GetConnector(params string[] dbConnStrName)`
  
  静态公共方法，用来获取一个对应用户数据库类型的连接器对象，目前支持最多传入一个参数：

  - `dbConStrname`：可变参数数组，表示在 Web.config 中的连接字符串名。

  - `返回值`：连接器（Connector）。
  
### `Connector`接口

该接口定义了6个接口方法：

- ### `Execute(string executeType, string cmdText, params object[] cmdParams)`
  
  用来执行执行SQL语句（支持带安全参数）并返回执行的结果，传入三种参数：

  - `executeType`：表示执行类型，有3个值：`Non`、`Reader`、`Scalar`、`Xml`，即对应执行 ExecuteNonQuery、ExecuteReader、ExecuteScalar、ExecuteXmlReader 方法，[参考文档](https://msdn.microsoft.com/zh-cn/library/system.data.sqlclient.sqlcommand.aspx)。

  - `cmdText`：表示命令文本，可以是SQL语句或存储过程，但`此方法只支持SQL语句的执行`。

  - `cmdParams`：表示命令参数对象的可变数组，SQL语句带安全参数时传入。

- ### `Execute(string executeType, string cmdText, CommandType cmdType, params object[] cmdParams)`

  上一个 Execute 方法的重载，支持执行存储过程，增加一个新参数：

  - `cmdType`：命令类型，有3个值：`CommandType.StoredProcedure`（存储过程）、`CommandType.Text`（SQL语句）、`TableDirect`（数据表）,`此方法支持前两个类型`，[参考文档](https://msdn.microsoft.com/zh-cn/library/system.data.commandtype(VS.80).aspx)。

 > Loading... :smile: