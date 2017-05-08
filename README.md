# Pocket48RoomListen
口袋48房间监听搬运

### 项目基础
<p>酷Q air 机器人 http://cqp.cc/</p>
<p>酷Q C# SDK https://cqp.cc/t/28865</p>

### 使用方法
<p>1、git clone或者下载项目到本地。</p>
<p>2、修改MyPlugin.cs的Initialize方法中，修改有关listenConfig的内容</p>

##### QQGroup = 应援群
##### KDRoomId = 口袋房间id 需要自行抓口袋信息获取
##### IdolName = 小偶像名字
##### GetRoomMsgDelay = 获取口袋房间消息时间间隔
##### GetLiveDelay = 获取口袋直播消息时间间隔 //暂时没卵用
##### GetWeiboDelay = 获取微博更新消息时间间隔 //暂时也没卵用
##### HitYouText = 如果口袋消息是翻牌时的追加文案 //这个可以通过面板去更改，不必编译时更改

<p>3、编译</p>
<p>4、将项目目录中Publish内的所有文件带走，合并至酷Q Air文件夹</p>
<p>5、运行Flexlive.CQP.CSharpProxy.exe，就可以看看新编译出来的插件了~</p>
<p>6、运行CQA，登录机器人帐号~这个时候如果CQA消息收取正常，代理这边应该也能正常发送信息了，可以看代理的功能测试面板是否有消息流入</p>

### 其他问题
<p>另外，如果退出酷Q的C#代理，代理进程依旧还会驻留！所以依旧可能继续在发消息！</p>

### 关于
<p>Plugins Powered by Teemo Studio</p>
<p>微博：迟早药丸的提莫队长</p>
<p>捐助：</p>

![donate](https://github.com/BeatenMo/Pocket48RoomListen/raw/master/donate.jpg)
