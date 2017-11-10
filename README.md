# DbKit

:fire: ADO.NET数据库开发连接辅助工具。A helpful database kit for ADO.NET development.

## 简介 Introduction

这是一个专门为ADO.NET开发的辅助类工具，目前已完成对`Microsoft SQL Server` 数据库`增`、`删`、`改`、`查`操作，支持`连接模式`和`断开模式`的操作。

This is a specialized kit for ADO.NET development, has supported the common data management for `Microsoft SQL Server` database in `connection mode` or `disconnect mode`.

## 开始 Start

可用以下方式获取`DbKit.cs`文件：

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

  - `dbConStrname`：可变参数数组，表示在 Web.config 中的连接字符串名
  
### `Connector`接口

> loading...