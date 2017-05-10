using Flexlive.CQP.Framework;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Teemo.CoolQ.Plugins
{
    public class MyPlugin : CQAppAbstract
    {
        /// <summary>
        /// 应用初始化，用来初始化应用的基本信息。
        /// </summary>
        public override void Initialize()
        {
            
            MemberIdCache.Add("徐晨辰", 219365999);
            MemberIdCache.Add("邹佳佳", 479995230);
            MemberIdCache.Add("龙亦瑞", 596537568);
            MemberIdCache.Add("陈楠茜", 550562023);
            MemberIdCache.Add("杨媛媛", 439642185);
            MemberIdCache.Add("张琼予", 498635931);
            
            this.Name = "口袋直播监听";
            this.Version = new Version("1.0.0.1");
            this.Author = "Teemo Studio";
            this.Description = "没啥好说的....跪求大佬捐助啊！";
        }

        static Hashtable MemberIdCache = new Hashtable();
        static Hashtable LiveMsgCache = new Hashtable();
        static int RunCount;

        static Thread ListenTask = null;

        public override void Startup()
        {
            ListenTask = new Thread(()=> {
                while (true)
                {
                    GetLive();
                    RunCount++;
                    Thread.Sleep(5000);
                }
            });
            ListenTask.Start();
        }

        public void GetLive()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri("https://plive.48.cn/livesystem/api/live/v1/memberLivePage"));
                req.Method = "POST";
                req.UserAgent = "kd listen";
                req.Headers.Add("token", "0");

                JObject rss = new JObject(
                    new JProperty("lastTime", 0),
                    new JProperty("limit", 20),
                    new JProperty("groupId", 0),
                    new JProperty("memberId", 0),
                    new JProperty("type", 1),
                    new JProperty("giftUpdTime", 1490857731000)
                );

                #region 数据请求 返回strResult:string
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
                #endregion

                JObject json = JObject.Parse(strResult);
                if ((int)json["status"] == 200)
                {
                    IEnumerable<JToken> datas = json.SelectTokens("$.content.liveList[*]");
                    foreach (JToken liveInfo in datas)
                    {
                        if (LiveMsgCache.ContainsKey(liveInfo["liveId"]))
                            return;
                        //懒得想算法了，直接很粗暴的按的字分割了
                        //title字段师xxx的直播间/电台，所以上面的idcache就直接看看有没有这个键，有的话直接推送了
                        string[] name = liveInfo["title"].ToString().Split(new string[] { "的" }, StringSplitOptions.RemoveEmptyEntries);
                        if (MemberIdCache.ContainsKey(name[0]))
                        {
                            File.AppendAllText("LiveError.log", "捕获：" + liveInfo["liveId"] + "\r\n");
                            LiveMsgCache.Add(liveInfo["liveId"], 1);
                            CQ.SendGroupMessage(long.Parse(MemberIdCache[name[0]].ToString()), "直播提醒：\r\n你的小偶像" + name[0] + "开了一个" + name[1]+ "\r\n请尽量打开口袋48观看！\r\n不方便的同学可以使用KD for PC（非官方）观看或打开网址登陆：https://h5.48.cn/2017appshare/memberLiveShare/index.html?id="+liveInfo["liveId"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText("LiveError.log", ex.ToString() + "\r\n" + ex.StackTrace + ex.Data +"\r\n");
            }
        }

        
        /// <summary>
        /// 打开设置窗口。
        /// </summary>
        public override void OpenSettingForm()
        {
            // 打开设置窗口的相关代码。
            FormSettings frm = new FormSettings();
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
