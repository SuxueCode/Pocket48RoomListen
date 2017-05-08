using Flexlive.CQP.Framework;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Drawing;

namespace Teemo.CoolQ.Plugins
{

    public class ListenConfig
    {
        public long QQGroup { get; set; }
        public long KDRoomId { get; set; }
        public string IdolName { get; set; }
        public int GetRoomMsgDelay { get; set; }
        public int GetWeiboDelay { get; set; }
        public int GetLiveDelay { get; set; }
        public string HitYouText { get; set; }
        public Version Version { get; set; }
        public int GetRoomMsgCount { get; set; }
    }

    public class MyPlugin : CQAppAbstract
    {
        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            //listenConfig = new ListenConfig() { QQGroup = 498635931, KDRoomId = 5783223, IdolName = "张琼予", GetRoomMsgDelay = 2000, GetLiveDelay = 5000, GetWeiboDelay = 3000 };
            //listenConfig = new ListenConfig() { QQGroup = 439642185, KDRoomId = 5776973, IdolName = "杨媛媛", GetRoomMsgDelay = 2000, GetLiveDelay = 5000, GetWeiboDelay = 3000 };
            //listenConfig = new ListenConfig() { QQGroup = 219365999, KDRoomId = 5773746, IdolName = "徐晨辰", GetRoomMsgDelay = 2000, GetLiveDelay = 5000, GetWeiboDelay = 3000 };
            //listenConfig = new ListenConfig() { QQGroup = 550562023, KDRoomId = 5777241, IdolName = "陈楠茜", GetRoomMsgDelay = 2000, GetLiveDelay = 5000, GetWeiboDelay = 3000 };
            //listenConfig = new ListenConfig() { QQGroup = 596537568, KDRoomId = 5770640, IdolName = "龙亦瑞", GetRoomMsgDelay = 2000, GetLiveDelay = 5000, GetWeiboDelay = 3000 };
            listenConfig = new ListenConfig() { QQGroup = 479995230, KDRoomId = 5774531, IdolName = "邹佳佳", GetRoomMsgDelay = 2000, GetLiveDelay = 5000, GetWeiboDelay = 3000 };

            listenConfig.Version = new Version("1.0.0.9");
            listenConfig.HitYouText = "";

            this.Name = "口袋房间监听(" + listenConfig.IdolName + ")";
            this.Version = listenConfig.Version;
            this.Author = "Teemo Studio";
            this.Description = "应援群：" + listenConfig.QQGroup + "\r\n口袋id：" + listenConfig.KDRoomId + "\r\n" + listenConfig.IdolName;
        }

        static ListenConfig listenConfig;

        static bool first = true;
        static long lasttime = 0;

        Thread tasks = null;

        public override void Startup()
        {
            tasks = new Thread(()=> {
                while (true)
                {
                    GetRoomMsg();
                    listenConfig.GetRoomMsgCount++;
                    Thread.Sleep(listenConfig.GetRoomMsgDelay);
                }
            });
            tasks.Start();
        }

        public void GetRoomMsg()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri("https://pjuju.48.cn/imsystem/api/im/v1/member/room/message/chat"));
                req.Method = "POST";
                req.UserAgent = "okhttp/3.4.1";

                JObject rss = new JObject(
                    new JProperty("roomId", listenConfig.KDRoomId),
                    new JProperty("lastTime", 0),
                    new JProperty("limit", 10)
                );

                string postJson = rss.ToString();
                byte[] bytes = Encoding.UTF8.GetBytes(postJson);

                req.ContentType = "application/json";
                req.ContentLength = bytes.Length;

                Stream reqstream = req.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;

                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                string strResult = streamReader.ReadToEnd();

                streamReceive.Dispose();
                streamReader.Dispose();


                JObject json = JObject.Parse(strResult);
                if ((int)json["status"] == 200)
                {
                    IEnumerable<JToken> datas = json.SelectTokens("$.content.data[*]");

                    //记录本次最大时间戳
                    long tmpTime = 0;
                    
                    foreach (JToken msgs in datas)
                    {
                        //历史最后时间戳比对
                        if ((long)msgs["msgTime"] > lasttime)
                        {
                            //本次消息时间
                            if ((long)msgs["msgTime"] > tmpTime)
                                tmpTime = (long)msgs["msgTime"];
                            JObject msg = JObject.Parse(msgs["extInfo"].ToString());
                            //首次运行，直接退出循环
                            if (first)
                                break;
                            if ((long)msgs["msgTime"] < lasttime)
                                break;
                            switch (msg["messageObject"].ToString())
                            {
                                case "deleteMessage":
                                    //CQ.SendGroupMessage(qqGroup,"你的小偶像删除了一条口袋房间的消息");
                                    break;
                                case "text":
                                    CQ.SendGroupMessage(listenConfig.QQGroup, String.Format("口袋房间：\r\n{0}:{1}\r\n发送时间:{2}", msg["senderName"].ToString(), msg["text"].ToString(), msgs["msgTimeStr"].ToString()));
                                    break;
                                case "image":
                                    JObject img = JObject.Parse(msgs["bodys"].ToString());
                                    CQ.SendGroupMessage(listenConfig.QQGroup, String.Format("口袋房间：\r\n{0}:发送了图片，但是机器人机器人不能发图，表情连接如下:\r\n{1}\r\n发送时间:{2}", msg["senderName"].ToString(), img["url"].ToString(), msgs["msgTimeStr"].ToString()));
                                    break;
                                case "faipaiText":
                                    CQ.SendGroupMessage(listenConfig.QQGroup, String.Format("口袋房间：\r\n翻牌辣！{3}:{4}\r\n{0} 回复:{1}\r\n被翻牌的大佬不来集资一发吗？" + listenConfig.HitYouText + " \r\n发送时间:{2}", msg["senderName"].ToString(), msg["messageText"].ToString(), msgs["msgTimeStr"].ToString(), msg["faipaiName"].ToString(), msg["faipaiContent"].ToString()));
                                    break;
                                default:
                                    CQ.SendGroupMessage(listenConfig.QQGroup, "你的小偶像有一条新消息，TeemoBot无法支持该类型消息，请打开口袋48查看~~");
                                    break;
                            }
                        } 
                    }
                    if (tmpTime != 0)
                        lasttime = tmpTime;
                }
                if (first)
                    first = false;
            }catch(Exception ex)
            {
                //CQ.SendGroupMessage(qqGroup, "啊！程序出问题了！赶紧让辣鸡犹存通知队长看bug！");
                File.AppendAllText("error.log", ex.ToString() + "\r\n" + ex.StackTrace);
            }
            
        }

        
        /// <summary>
        /// 打开设置窗口。
        /// </summary>
        public override void OpenSettingForm()
        {
            // 打开设置窗口的相关代码。
            FormSettings frm = new FormSettings(ref tasks, ref listenConfig);
            frm.ShowDialog();
        }

        /// <summary>
        /// Type=21 私聊消息。
        /// </summary>
        /// <param name="subType">子类型，11/来自好友 1/来自在线状态 2/来自群 3/来自讨论组。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void PrivateMessage(int subType, int sendTime, long fromQQ, string msg, int font)
        {
            // 处理私聊消息。
        }

        /// <summary>
        /// Type=2 群消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="fromAnonymous">来源匿名者。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void GroupMessage(int subType, int sendTime, long fromGroup, long fromQQ, string fromAnonymous, string msg, int font)
        {

        }

        /// <summary>
        /// Type=4 讨论组消息。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromDiscuss">来源讨论组。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">消息内容。</param>
        /// <param name="font">字体。</param>
        public override void DiscussMessage(int subType, int sendTime, long fromDiscuss, long fromQQ, string msg, int font)
        {
            // 处理讨论组消息。
        }

        /// <summary>
        /// Type=11 群文件上传事件。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="file">上传文件信息。</param>
        public override void GroupUpload(int subType, int sendTime, long fromGroup, long fromQQ, string file)
        {
            // 处理群文件上传事件。
        }

        /// <summary>
        /// Type=101 群事件-管理员变动。
        /// </summary>
        /// <param name="subType">子类型，1/被取消管理员 2/被设置管理员。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupAdmin(int subType, int sendTime, long fromGroup, long beingOperateQQ)
        {
            // 处理群事件-管理员变动。
        }

        /// <summary>
        /// Type=102 群事件-群成员减少。
        /// </summary>
        /// <param name="subType">子类型，1/群员离开 2/群员被踢 3/自己(即登录号)被踢。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberDecrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
            // 处理群事件-群成员减少。
        }

        /// <summary>
        /// Type=103 群事件-群成员增加。
        /// </summary>
        /// <param name="subType">子类型，1/管理员已同意 2/管理员邀请。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="beingOperateQQ">被操作QQ。</param>
        public override void GroupMemberIncrease(int subType, int sendTime, long fromGroup, long fromQQ, long beingOperateQQ)
        {
        }

        /// <summary>
        /// Type=201 好友事件-好友已添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        public override void FriendAdded(int subType, int sendTime, long fromQQ)
        {

        }

        /// <summary>
        /// Type=301 请求-好友添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddFriend(int subType, int sendTime, long fromQQ, string msg, string responseFlag)
        {

        }

        /// <summary>
        /// Type=302 请求-群添加。
        /// </summary>
        /// <param name="subType">子类型，目前固定为1。</param>
        /// <param name="sendTime">发送时间(时间戳)。</param>
        /// <param name="fromGroup">来源群号。</param>
        /// <param name="fromQQ">来源QQ。</param>
        /// <param name="msg">附言。</param>
        /// <param name="responseFlag">反馈标识(处理请求用)。</param>
        public override void RequestAddGroup(int subType, int sendTime, long fromGroup, long fromQQ, string msg, string responseFlag)
        {

        }


    }
}
