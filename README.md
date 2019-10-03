## 介绍

使用 C# 写的 [MetingJS](https://github.com/metowolf/MetingJS) 后端，纯粹为了造轮子而造轮子。换句话说就是有了把锤子，看什么都像钉子。


## 使用方法

### 运行环境

需要安装 [.net core](https://dotnet.microsoft.com/download)3.0

### 配置文件

修改 `appsetting.json` ,将 `Url` 中的协议和域名替换为你的站点的协议和域名， `Replace` 项为自定义替换，如果不懂就不需要改动。

### 反向代理

在 Windows 服务器上程序运行端口为 5001 ，需要修改的请修改配置文件中的 `Urls` 选项，多个端口使用 `;` 分割。或者在启动时使用 `--urls` 参数，例如 `./MetingJS.Server.exe --urls http://localhost:8000`。

Linux 使用 Unix 套接字通信，需要代理 `unix:/tmp/metingJS.Server.sock` ，暂不支持自定义。

### API

理论上完全兼容 MetingJS ，只需要将 MetingJS 的后端配置修改为你的就可以了。

## Q&A

#### 有没有搭建好的后端供大家使用？  
`https://sm.sm9.top/api/music`
`https://am.sm9.top/api.php`

上面两个都没有配置跨域，不自己搞代理的话不能用，仅仅是用于演示，不能用于生产环境。

#### 如果没有服务器怎么办？
下一步计划推出一个云函数版，使用云函数搭建。

#### 有些歌曲获取失败是怎么回事？
按理说付费歌曲都不能播放，某个平台已经下架的歌曲也不能播放。

## 感谢

本项目基于 [.net core](https://dotnet.microsoft.com) 开发，API解析使用了 [Meting4Net](https://github.com/yiyungent/Meting4Net)。