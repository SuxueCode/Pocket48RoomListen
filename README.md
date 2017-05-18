# Pocket48RoomListen
口袋48房间监听搬运

### 项目基础
<p>酷Q air 机器人 http://cqp.cc/</p>
<p>酷Q C# SDK https://cqp.cc/t/32788</p>

### 注意
从源码开始改动部署时间较长，容易出错，新手请直接看微博！
http://weibo.com/1689360850/F2NT52o3W
新版本占用低，真的很稳定！

### 使用方法
<p>0.将插件解压到酷Q根目录</p>
<p>1.先运行NDP452-KB2901954-Web.exe看看.net 4.5.2安装了吗~没安装会自动安装，安装了会直接提示你已经安装了的</p>
<p>2.打开酷Q开发者模式，不懂看<a href="http://d.cqp.me/Pro/%E5%BC%80%E5%8F%91/%E5%BF%AB%E9%80%9F%E5%85%A5%E9%97%A8">这里</a></p>
<p>3.编辑根目录的config.ini</p>
<p>4.启动酷Q，启用插件，打开酷Q日志，看看是否有加载信息，有即为成功</p>

### 新版config.ini释义
新版config.ini基本和上一版本一致，只不过由于支持了多人，所以做了些许改动
config格式为json，不明白的请去了解啥是json

##### GetLiveDelay 获取口袋直播消息时间间隔
##### Idols 你要监听的小偶像配置信息

<p>Idols下的配置信息</p>

##### QQGroup = 应援群
##### KDRoomId = 口袋房间id 需要自行抓口袋信息获取
##### IdolName = 小偶像名字
##### GetRoomMsgDelay = 获取口袋房间消息时间间隔
##### GetLiveDelay = 获取口袋直播消息时间间隔 //这个理论上已经失效了，但是还是留着吧
##### GetWeiboDelay = 获取微博更新消息时间间隔 //暂时也没卵用
##### NeedYou = 如果口袋消息是翻牌时的追加文案，\n\r为换行 //新版没面板了，如果需要更改文案，直接在config里面改，改完重新启用下插件即可

### Q&A
<p>Q：解压文件了启动酷Q不见插件啊~</p>
<p>A：麻烦先登陆一次后再开启开发者模式~</p>
<p>Q：启动酷Q报插件错误</p>
<p>A：请检查.net是否安装，如果安装后还报错，建议重新下载覆盖，还是报错，请联系我查看现场</p>
<p>Q：能不能多加一点东西，我见其他家机器人蛮多功能的</p>
<p>A：如果有啥建议可以微博跟我说，但是做不做进来我会有所考虑，应援群的生态应该维持在一个平衡点，如果啥都扔进来，怕是机器人刷屏也不一定</p>

### 关于
<p>Plugins Powered by Teemo Studio</p>
<p>微博：迟早药丸的提莫队长</p>
<p>捐助：</p>

![donate](https://github.com/BeatenMo/Pocket48RoomListen/raw/master/donate.jpg)
