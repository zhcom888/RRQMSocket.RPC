<p></p>
<p></p>
<p align="center">
<img src="https://ftp.bmp.ovh/imgs/2021/06/351eeccfadc07014.png" width = "100" height = "100" alt="图片名称" align=center />
</p>

 <div align="center"> 
  
[![NuGet version (RRQMSocket.RPC)](https://img.shields.io/nuget/v/RRQMSocket.RPC.svg?style=flat-square)](https://www.nuget.org/packages/RRQMSocket.RPC/)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)
[![Download](https://img.shields.io/nuget/dt/RRQMSocket.RPC)](https://www.nuget.org/packages/RRQMSocket.RPC/)
<a href="https://jq.qq.com/?_wv=1027&k=gN7UL4fw">
<img src="https://img.shields.io/badge/QQ群-234762506-red" alt="QQ">
</a>
</div>  

<div align="center">

天生我才必有用，马踏飞燕影无踪。

</div>

## 💿描述
| 名称|地址 |描述|
|---|---|---|
|[![NuGet version (RRQMSocket.RPC)](https://img.shields.io/nuget/v/RRQMSocket.RPC.svg?label=RRQMSocket.RPC)](https://www.nuget.org/packages/rrqmsocket.rpc)|[Gitee](https://gitee.com/RRQM_Home/rrqmsocket.rpc)<br>[Github](https://github.com/RRQM/RRQMSocket.RPC) |RPC是一个超轻量、高性能、可扩展的微服务管理平台框架，<br>目前已完成开发**RRQMRPC**、**XmlRpc**、**JsonRpc**、**WebApi**部分。<br> **RRQMRPC**部分使用RRQM专属协议，支持客户端**异步调用**，<br>服务端**异步触发**、以及**out**和**ref**关键字，**函数回调**等。<br>在调用效率上也是非常强悍，在调用空载函数，且返回状态时，<br>**10w**次调用仅用时**3.8**秒，不返回状态用时**0.9**秒。<br>其他协议调用性能详看性能评测。
|[![NuGet version (RRQMSocket.RPC.XmlRpc)](https://img.shields.io/nuget/v/RRQMSocket.RPC.XmlRpc.svg?label=RRQMSocket.RPC.XmlRpc)](https://www.nuget.org/packages/rrqmsocket.rpc.xmlrpc)|[Gitee](https://gitee.com/RRQM_Home/rrqmsocket.rpc)<br>[Github](https://github.com/RRQM/RRQMSocket.RPC) | XmlRpc是一个扩展于RRQMSocket.RPC的XmlRpc组件，可以通过<br>该组件创建XmlRpc服务解析器，完美支持XmlRpc数据类型，类型嵌套，<br>Array等，也能与CookComputing.XmlRpcV2完美对接。<br>不限Web，Android等平台。|
| [![NuGet version (RRQMSocket.RPC.JsonRpc)](https://img.shields.io/nuget/v/RRQMSocket.RPC.JsonRpc.svg?label=RRQMSocket.RPC.JsonRpc)](https://www.nuget.org/packages/rrqmsocket.rpc.jsonrpc)|[Gitee](https://gitee.com/RRQM_Home/rrqmsocket.rpc)<br>[Github](https://github.com/RRQM/RRQMSocket.RPC) | JsonRpc是一个扩展于RRQMSocket.RPC的JsonRpc组件，<br>可以通过该组件创建JsonRpc服务解析器，支持JsonRpc全部功能，可与Web，Android等平台无缝对接。|
|[![NuGet version (RRQMSocket.RPC.WebApi)](https://img.shields.io/nuget/v/RRQMSocket.RPC.WebApi.svg?label=RRQMSocket.RPC.WebApi)](https://www.nuget.org/packages/rrqmsocket.rpc.webapi)|[Gitee](https://gitee.com/RRQM_Home/rrqmsocket.rpc)<br>[Github](https://github.com/RRQM/RRQMSocket.RPC) | WebApi是一个扩展于RRQMSocket.RPC的WebApi组件，可以通过<br>该组件创建WebApi服务解析器，让桌面端、Web端、移动端可以<br>跨语言调用RPC函数。功能支持路由、Get传参、Post传参等。|

## 🎀依赖、扩展库
| 名称|地址 |描述|
|---|---|---|
| [![NuGet version (RRQMCore)](https://img.shields.io/nuget/v/RRQMCore.svg?label=RRQMCore)](https://www.nuget.org/packages/RRQMCore)|[Gitee](https://gitee.com/dotnetchina/RRQMSocket)<br>[Github](https://github.com/RRQM/RRQMSocket) | RRQMCore是为RRQM系提供基础服务功能的库，其中包含：<br>**内存池**、**对象池**、**等待逻辑池**、**AppMessenger**、**3DES加密**、<br>**Xml快速存储**、**运行时间测量器**、**文件快捷操作**、<br>**高性能序列化器**、**规范日志接口**等。 |
|[![NuGet version (RRQMSocket)](https://img.shields.io/nuget/v/RRQMSocket.svg?label=RRQMSocket)](https://www.nuget.org/packages/RRQMSocket/)|[Gitee](https://gitee.com/dotnetchina/RRQMSocket)<br>[Github](https://github.com/RRQM/RRQMSocket)| RRQMSocket是一个整合性的、超轻量级的网络通信框架。<br>包含了TCP、UDP、Ssl、Channel、Protocol、Token、<br>租户模式等一系列的通信模块。其扩展组件包含：WebSocket、<br>大文件传输、RPC、WebApi、XmlRpc、JsonRpc等内容|
|[![NuGet version](https://img.shields.io/nuget/v/RRQMSocketFramework.svg?label=RRQMSocketFramework)](https://www.nuget.org/packages/RRQMSocketFramework/)|[Gitee](https://gitee.com/dotnetchina/RRQMSocket)<br>[Github](https://github.com/RRQM/RRQMSocket) |**RRQMSocketFramework**是RRQMSocket系列的增强企业版，<br>两者在基础功能上没有区别，但是在扩展功能上有一定差异性，<br>例如RPC中的EventBus、文件传输中的限速功能等，<br>具体差异请看[RRQM商业运营](https://gitee.com/RRQM_OS/RRQM/wikis/%E5%95%86%E4%B8%9A%E8%BF%90%E8%90%A5)|
| [![NuGet version (RRQMSocket.WebSocket)](https://img.shields.io/nuget/v/RRQMSocket.WebSocket.svg?label=RRQMSocket.WebSocket)](https://www.nuget.org/packages/rrqmsocket.websocket)|[Gitee](https://gitee.com/dotnetchina/RRQMSocket)<br>[Github](https://github.com/RRQM/RRQMSocket) |  RRQMSocket.WebSocket是一个高效，超轻量级的WebSocket框架。<br>它包含了Service和Client两大组件，支持Ssl，同时定义了文本、二进制或<br>其他类型数据的快捷发送、分片发送接口，可与js等任意WebSocket组件交互|
| [![NuGet version (RRQMSocket.Http)](https://img.shields.io/nuget/v/RRQMSocket.Http.svg?label=RRQMSocket.Http)](https://www.nuget.org/packages/rrqmsocket.http)|[Gitee](https://gitee.com/dotnetchina/RRQMSocket)<br>[Github](https://github.com/RRQM/RRQMSocket)  |  RRQMSocket.Http是一个能够简单解析Http的服务组件，<br>能够快速响应Http服务请求。|
| [![NuGet version (RRQMSocket.FileTransfer)](https://img.shields.io/nuget/v/RRQMSocket.FileTransfer.svg?label=RRQMSocket.FileTransfer)](https://www.nuget.org/packages/rrqmsocket.filetransfer)|[Gitee](https://gitee.com/RRQM_Home/rrqmsocket.filetransfer)<br>[Github](https://github.com/RRQM/RRQMSocket.FileTransfer) |  这是一个高性能的C/S架构的文件传输框架，您可以用它传输<br>**任意大小**的文件，它可以完美支持**上传下载混合式队列传输**、<br>**断点续传**、 **快速上传** 、**传输限速**、**获取文件信息**、**删除文件**等。<br>在实际测试中，它的传输速率可达1000Mb/s。 |


## 🖥支持环境
- .NET Framework4.5及以上。
- .NET Core3.1及以上。
- .NET Standard2.0及以上。

## 🥪支持框架
- WPF
- Winform
- Blazor
- Xamarin
- Mono
- Unity（在IL2cpp编译时，需要导入源码）
- 其他（即所有C#系）

## 🌴 RPC特点速览

#### 【RRQMRPC-TCP】

RRQMRPC-TCP是基于“TCP+RRQM自制定协议”的RPC组件，也是在RRQMSocket.RPC中最强悍的，其特性：

- 支持**自定义**类型参数。
- 支持**Ssl**加密调用。
- 支持具有**默认值**的参数设定。
- 支持**out、ref** 关键字参数。
- 支持服务器**回调客户端** 。
- 支持**客户端**之间**相互调用**。
- 支持异步调用。
- 支持权限管理，让非法调用死在萌芽时期。
- 支持**静态织入调用**，**静态编译调用**，也支持**方法名+参数**调用。
- 支持**调用配置**（类似MQTT的AtMostOnce，AtLeastOnce，ExactlyOnce）。
- **支持EventBus**。
- 支持**自定义序列化**。
- **全异常反馈** ，服务器调用状态会完整的反馈到客户端（可以设置不反馈）。
- 高性能，在保证送达但不返回的情况下，10w次调用用时0.8s，在返回的情况下，用时3.9s。

#### 【RRQMRPC-UDP】

RRQMRPC-UDP是基于“UDP+RRQM自制定协议”的RPC组件，性能和RRQMRPC-TCP一致，但是由于基于原生UDP，所以调用可能会丢失，其特性：

- 支持**自定义**类型参数。
- 支持具有**默认值**的参数设定。
- 支持**out、ref** 关键字参数。
- 支持异步调用。
- 支持权限管理，让非法调用死在萌芽时期。
- 支持**静态织入调用**，**静态编译调用**，也支持**方法名+参数**调用。
- 支持**调用配置**（类似MQTT的AtMostOnce，AtLeastOnce，ExactlyOnce）。
- 支持**自定义序列化**。
- **全异常反馈** ，服务器调用状态会完整的反馈到客户端（可以设置不反馈）。
- 高性能，在保证送达但不返回的情况下，10w次调用用时0.8s，在返回的情况下，用时3.9s。

#### 【JsonRpc】

JsonRpc解析器是遵循JsonRpc2.0的RPC服务组件，能够让使用者通过Json字符串基于TCP、HTTP/HTTPS协议就可以调用RPC服务，其特点有：

- 支持**TCP、HTTP/HTTPS**协议 。
- **C#端支持代理生成** 。
- **全异常反馈** 。
- 支持自定义类型。
- 支持类型嵌套。
- 支持内联调用。
- 支持缺参调用。

#### 【XmlRpc】

使用XmlRpc解析器，就可以在RPCService中通过XmlRpc的调用方式直接调用服务，客户端可以使用**CookComputing.XmlRpcV2**进行对接，其特点：

- **异常反馈** 
- 支持**HTTP/HTTPS**协议 。
- **C#端支持代理生成** 。
- 支持自定义类型。
- 支持类型嵌套。
- 支持Array及自定义Array嵌套。

#### 【WebApi】
使用WebApi解析器，就可以在RPCService中通过WebApi的调用方式直接调用服务。

- 高性能，100个客户端，10w次调用，仅用时17s。
- 支持**HTTP/HTTPS**协议 。
- **全异常反馈** 。
- 支持大部分路由规则。
- 支持js、Android等调用。

## 🔗联系作者

- [CSDN博客主页](https://blog.csdn.net/qq_40374647)
- [哔哩哔哩视频](https://space.bilibili.com/94253567)
- [源代码仓库主页](https://gitee.com/RRQM_Home) 
- 交流QQ群：234762506

## 🌟API手册
- [ API首页 ](https://gitee.com/RRQM_Home/RRQMBox/wikis/API%E6%89%8B%E5%86%8C)
- [说明](https://gitee.com/RRQM_Home/RRQMBox/wikis/%E8%AF%B4%E6%98%8E(%E4%BD%BF%E7%94%A8%E5%89%8D%E5%BF%85%E8%A6%81%E9%98%85%E8%AF%BB))
- [ 历史更新 ](https://gitee.com/RRQM_Home/RRQMBox/wikis/%E5%8E%86%E5%8F%B2%E6%9B%B4%E6%96%B0)
- [ 商业运营 ](https://gitee.com/RRQM_Home/RRQMBox/wikis/%E5%95%86%E4%B8%9A%E8%BF%90%E8%90%A5)
- [疑难解答](https://gitee.com/RRQM_Home/RRQMBox/wikis/%E7%96%91%E9%9A%BE%E8%A7%A3%E7%AD%94)


## ✨简单示例

 **_更多配置请查看API文档的配置说明文档，一下仅以最简方式创建实例。_** 

【定义服务】

```
[Route("/[controller]/[action]")]
public class XUnitTestServer : ControllerBase
{

    [XmlRpc]
    [JsonRpc]
    [Route]
    [RRQMRPC]
    public int Sum(int a, int b)
    {
        return a + b;
    }
}
```
【创建RRQMRPC-TCP】

```
private static IRPCParser CreateRRQMTcpParser(int port)
{
    TcpRpcParser tcpRPCParser = new TcpRpcParser();

    //创建配置
    var config = new TcpRpcParserConfig();
    config.ListenIPHosts = new IPHost[] { new IPHost(port) };//监听一个IP地址
    config.ThreadCount = 1;//设置多线程数量
    config.ClearInterval = -1;//规定不清理无数据客户端
    config.VerifyTimeout = 3 * 1000;//令箭验证超时时间，3秒
    config.VerifyToken = "123RPC";//令箭值
    config.ProxyToken = "RPC";//默认服务代理令箭
    //载入配置
    tcpRPCParser.Setup(config);

    //启动服务
    tcpRPCParser.Start();

    Console.WriteLine($"TCP解析器添加完成，端口号：{port}，VerifyToken={tcpRPCParser.VerifyToken}，ProxyToken={tcpRPCParser.ProxyToken}");
    return tcpRPCParser;
}

```
【创建RRQMRPC-UDP】

```
private static IRPCParser CreateRRQMUdpParser(int port)
{
    UdpRpc udpRPCParser = new UdpRpc();
    var config = new UdpRpcParserConfig();
    config.BindIPHost = new IPHost(port);
    config.BufferLength = 1024;
    config.ThreadCount = 1;
    config.ProxyToken = "RPC";

    udpRPCParser.Setup(config);

    udpRPCParser.Start();

    Console.WriteLine($"UDP解析器添加完成，端口号：{port}，ProxyToken={udpRPCParser.ProxyToken}");
    return udpRPCParser;
}

```
【创建JsonRpc】

```
private static IRPCParser CreateJsonRpcParser(int port, JsonRpcProtocolType protocolType)
{
    JsonRpcParser jsonRpcParser = new JsonRpcParser();

    var config = new JsonRpcParserConfig();
    config.BufferLength = 1024;
    config.ThreadCount = 1;//设置多线程数量
    config.ClearInterval = -1;//规定不清理无数据客户端
    config.ListenIPHosts = new IPHost[] { new IPHost(port) };
    config.ProtocolType = protocolType;
    config.ProxyToken = "RPC";
    jsonRpcParser.Setup(config);
    jsonRpcParser.Start();
    Console.WriteLine($"jsonRpcParser解析器添加完成，端口号：{port}，协议：{protocolType}");
    return jsonRpcParser;
}

```
【创建XmlRpc】

```
private static IRPCParser CreateXmlRpcParser(int port)
{
    XmlRpcParser xmlRpcParser = new XmlRpcParser();
    var config = new XmlRpcParserConfig();
    config.BufferLength = 1024;
    config.ThreadCount = 1;//设置多线程数量
    config.ClearInterval = -1;//规定不清理无数据客户端
    config.ListenIPHosts = new IPHost[] { new IPHost(port) };
    config.ProxyToken = "RPC";
    xmlRpcParser.Setup(config);
    xmlRpcParser.Start();

    Console.WriteLine($"xmlRpcParser解析器添加完成，端口号：{port}");
    return xmlRpcParser;
}

【创建WebApi】

private static IRPCParser CreateWebApiParser(int port, ApiDataConverter dataConverter)
{
    WebApiParser webApiParser = new WebApiParser();
    var config = new WebApiParserConfig();
    config.BufferLength = 1024;
    config.ThreadCount = 1;//设置多线程数量
    config.ClearInterval = -1;//规定不清理无数据客户端
    config.ListenIPHosts = new IPHost[] { new IPHost(port) };
    config.ApiDataConverter = dataConverter;
    webApiParser.Setup(config);
    webApiParser.Start();
    Console.WriteLine($"webApiParser解析器添加完成，端口号：{port}，序列化器：{dataConverter.GetType().Name}");
    return webApiParser;
}

```

【服务注册与启动】

```
RPCService rpcService = new RPCService();

rpcService.AddRPCParser("tcpRPCParser", CreateRRQMTcpParser(7794));

rpcService.AddRPCParser("udpRPCParser", CreateRRQMUdpParser(7797));

rpcService.AddRPCParser("webApiParser_Xml", CreateWebApiParser(7800, new XmlDataConverter()));
rpcService.AddRPCParser("webApiParser_Json", CreateWebApiParser(7801, new JsonDataConverter()));

rpcService.AddRPCParser("xmlRpcParser", CreateXmlRpcParser(7802));

rpcService.AddRPCParser("JsonRpcParser_Tcp", CreateJsonRpcParser(7803, JsonRpcProtocolType.Tcp));
rpcService.AddRPCParser("JsonRpcParser_Http", CreateJsonRpcParser(7804, JsonRpcProtocolType.Http));
rpcService.RegisterServer<XUnitTestServer>();//注册服务


foreach (var item in ((WebApiParser)rpcService.RPCParsers["webApiParser_Xml"]).RouteMap.Urls)
{
    Console.WriteLine($"使用：http://127.0.0.1:7800" + item);
}

foreach (var item in ((WebApiParser)rpcService.RPCParsers["webApiParser_Json"]).RouteMap.Urls)
{
    Console.WriteLine($"使用：http://127.0.0.1:7801" + item);
}

```

【客户端调用】

除RRQMRPC以外，其他端的调用均可使用其他工具调用调试，但是RRQM提供了便捷的客户端和代理生成。下面就展示一下如何在C#中优雅的调用。

【代理生成】

[获取代理详情](https://blog.csdn.net/qq_40374647/article/details/109143243#:~:text=%E5%B9%B3%E5%8F%B0%E3%80%81%E8%B7%A8%E8%AF%AD%E8%A8%80%EF%BC%89-,%E5%85%AD%E3%80%81%E8%8E%B7%E5%8F%96%E4%BB%A3%E7%90%86,-%E4%BD%BF%E7%94%A8RPC%E7%9A%84)




## 🧲应用场景模拟
[场景入口](https://gitee.com/RRQM_Home/RRQMBox/wikis/%E5%BA%94%E7%94%A8%E5%9C%BA%E6%99%AF%E6%A8%A1%E6%8B%9F)

***

## 致谢

谢谢大家对我的支持，如果还有其他问题，请加群QQ：234762506讨论。

## 支持作者

[支持入口](https://gitee.com/RRQM_Home/RRQMBox/wikis/%E6%94%AF%E6%8C%81%E4%BD%9C%E8%80%85)
