## 介绍

使用 C# 写的 [MetingJS](https://github.com/metowolf/MetingJS) 后端，纯粹为了造轮子而造轮子。换句话说就是有了把锤子，看什么都像钉子。


## 使用方法

### 运行环境

需要安装 [.net core](https://dotnet.microsoft.com/download)3.0

### 配置文件

修改 `appsetting.json` ,将 `Url` 中的协议和域名替换为你的站点的协议和域名， `Replace` 项为自定义替换，如果不懂就不需要改动。

### 反向代理

在 Windows 服务器上程序运行端口为 5001 ，暂时不能修改（我猜也没多少人会在 Windows 服务器上跑这个东西）， Linux 使用 Unix 域套接字通信，需要代理 `unix:/tmp/metingJS.Server.sock` ，暂时不支持自定义

### API

理论上完全兼容 MetingJS ，只需要将 MetingJS 的后端配置修改为你的就可以了

## Q&A

#### 有没有搭建好的后端供大家使用？  
暂时没有，也许以后会有，但是没有这个计划，我自己用的还是 php 写的，因为迁移成本有点高。

#### 如果没有服务器怎么办？
下一步计划推出一个云函数版，使用云函数搭建。

#### 有些歌曲获取失败是怎么回事？
按理说付费歌曲都不能播放，某个平台已经下架的歌曲也不能播放。

## 感谢

本项目基于 [.net core](https://dotnet.microsoft.com) 开发，API解析使用了 [Meting4Net](https://github.com/yiyungent/Meting4Net)。