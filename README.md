## 介绍

使用 C# 写的 [MetingJS](https://github.com/metowolf/MetingJS) 后端，纯粹为了造轮子而造轮子。换句话说就是有了把锤子，看什么都像钉子。


## 使用方法

### 运行环境

需要安装 [.net core](https://dotnet.microsoft.com/download)3.1

### 配置文件

修改 `appsetting.json` ,将

- `Url` 中的协议和域名替换为你的站点的协议和域名
- `Replace` 项为自定义替换，如果不懂就不需要改动

### 反向代理

在 Windows 服务器上程序运行端口为 5001 ，需要修改的请修改配置文件中的 `Urls` 选项，多个端口使用 `;` 分割。或者在启动时使用 `--urls` 参数，例如 `./MetingJS.Server.exe --urls http://localhost:8000`。

Linux 使用 Unix 套接字通信，需要代理 `unix:/tmp/metingJS.Server.sock` ，暂不支持自定义。

### API

理论上完全兼容 MetingJS ，只需要将 MetingJS 的后端配置修改为你的就可以了。

### 云函数版

注册阿里云并开通 `函数计算` 服务

下载或者自己编译fc版本

默认不需要修改配置文件

`控制台` -> `函数计算` -> `新建函数` -> `HTTP函数`

函数入口为：`MetingJS.Server.Fc::MetingJS.Server.Fc.FcRemoteEntrypoint::HandleRequest`

请求地址为(Meting填写下方地址) ：

`https://xxxxxxxxxxxx.cn-shanghai.fc.aliyuncs.com/xxxx-xx-xx/proxy/MetingJS/GetMusic/api/music` 或  
`https://xxxxxxxxxxxx.cn-shanghai.fc.aliyuncs.com/xxxx-xx-xx/proxy/MetingJS/GetMusic/api.php`

例如
`https://xxxxxxxxxxxx.cn-shanghai.fc.aliyuncs.com/xxxx-xx-xx/proxy/MetingJS/GetMusic/api/music?server=Tencent&type=lrc&id=0008yfgO0dmovi`

![函数设置](https://s1.ax1x.com/2020/04/15/JiepY8.png)  
![触发器设置](https://s1.ax1x.com/2020/04/15/JiZxTP.png)  
![测试结果](https://s1.ax1x.com/2020/04/16/JFCCg1.png)

## Q&A

#### 有没有搭建好的后端供大家使用？
`https://sm.sm9.top/api/music`  
`https://sm.sm9.top/api.php`

上面两个都没有配置跨域，不自己搞代理的话不能用，仅仅是用于演示，不能用于生产环境。

云函数版请勿用于生成环境，如果不能访问就代表我已经把这个函数删了

`https://1384366657049717.cn-shanghai.fc.aliyuncs.com/2016-08-15/proxy/MetingJS/GetMusic/api/music`

#### 如果没有服务器怎么办？

可以使用云函数版

#### 有些歌曲获取失败是怎么回事？
按理说付费歌曲都不能播放，某个平台已经下架的歌曲也不能播放。

## 感谢

本项目基于 [.net core](https://dotnet.microsoft.com) 开发，API解析使用了 [Meting4Net](https://github.com/yiyungent/Meting4Net)。
