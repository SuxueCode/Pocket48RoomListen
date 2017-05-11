using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newbe.CQP.Framework;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Collections;

namespace Teemo.CoolQ.NewPlugins
{
    public class PocketPlugins : PluginBase
    {
        
        public override string AppId => "Teemo.CoolQ.NewPlugins";

        static Task GetMsgTask;
        static Task GetLiveTask;
        static ListenConfig ListenConfig;

        static bool First = true;
        static long Lasttime = 0;
        static int MsgCount;
        
        static bool ExitTask = false;

        static int LiveCount;
        static Hashtable MemberIdCache = new Hashtable();
        static Hashtable LiveMsgCache = new Hashtable();

        public PocketPlugins(ICoolQApi coolQApi) : base(coolQApi)
        {

        }

        public override int Enabled()
        {
            //面板重新启用时候判断
            if (ExitTask)
                ExitTask = false;

            if (File.Exists("config.ini"))
            {
                string config = File.ReadAllText("config.ini");
                try
                {
                    JObject configJson = JObject.Parse(config);
                    First = true;
                    ListenConfig = new ListenConfig();
                    ListenConfig.QQGroup = long.Parse(configJson["QQGroup"].ToString());
                    ListenConfig.KDRoomId = long.Parse(configJson["KDRoomId"].ToString());
                    ListenConfig.IdolName = configJson["IdolName"].ToString();
                    ListenConfig.GetRoomMsgDelay = int.Parse(configJson["GetRoomMsgDelay"].ToString());
                    ListenConfig.GetLiveDelay = int.Parse(configJson["GetLiveDelay"].ToString());
                    ListenConfig.GetWeiboDelay = int.Parse(configJson["GetWeiboDelay"].ToString());
                    ListenConfig.HitYouText = configJson["NeedYou"].ToString();
                    
                    MemberIdCache.Add(ListenConfig.IdolName, ListenConfig.QQGroup);
                    CoolQApi.AddLog(CoolQLogLevel.Info, "cache load ok");
                    GetMsgTask = new Task(() => {
                        while (!ExitTask)
                        {
                            GetRoomMsg();
                            MsgCount++;
                            CoolQApi.AddLog(CoolQLogLevel.Debug, "get room msg count:" + MsgCount + " msg lasttime:" + Lasttime + " member:" + ListenConfig.IdolName + " kdroomid" + ListenConfig.KDRoomId);
                            Thread.Sleep(ListenConfig.GetRoomMsgDelay);
                        }
                    });
                    GetMsgTask.Start();
                    GetLiveTask = new Task(() =>
                    {
                        while (!ExitTask)
                        {
                            GetLive();
                            LiveCount++;
                            CoolQApi.AddLog(CoolQLogLevel.Debug, "get live count:" + LiveCount);
                            Thread.Sleep(ListenConfig.GetLiveDelay);
                        }
                    });
                    GetLiveTask.Start();
                }
                catch(Exception ex)
                {
                    File.AppendAllText("error.log", DateTime.Now.ToString() + "\r\n" + ex.ToString() + "\r\n" + ex.StackTrace + "\r\n");
                    CoolQApi.AddLog(CoolQLogLevel.Error, "请检查config.ini是否配置正确,七项缺一不可");
                    return base.Enabled();
                }
                
            }
            else
                CoolQApi.AddLog(CoolQLogLevel.Debug, "config error");
            return base.Enabled();
        }

        public override int Disabled()
        {
            ExitTask = true;
            return base.Disabled();
        }

        public override int CoolQExited()
        {
            ExitTask = true;
            return base.CoolQExited();
        }

        public void GetRoomMsg()
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri("https://pjuju.48.cn/imsystem/api/im/v1/member/room/message/chat"));
                req.Method = "POST";
                req.UserAgent = "okhttp/3.4.1";

                JObject rss = new JObject(
                    new JProperty("roomId", ListenConfig.KDRoomId),
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
                        if ((long)msgs["msgTime"] > Lasttime)
                        {
                            //本次消息时间
                            if ((long)msgs["msgTime"] > tmpTime)
                                tmpTime = (long)msgs["msgTime"];
                            JObject msg = JObject.Parse(msgs["extInfo"].ToString());
                            //首次运行，直接退出循环
                            if (First)
                                break;
                            if ((long)msgs["msgTime"] < Lasttime)
                                break;
                            switch (msg["messageObject"].ToString())
                            {
                                case "deleteMessage":
                                    //CQ.SendGroupMessage(qqGroup,"你的小偶像删除了一条口袋房间的消息");
                                    break;
                                case "text":
                                    CoolQApi.SendGroupMsg(ListenConfig.QQGroup, String.Format("口袋房间：\r\n{0}:{1}\r\n发送时间:{2}", msg["senderName"].ToString(), msg["text"].ToString(), msgs["msgTimeStr"].ToString()));
                                    break;
                                case "image":
                                    JObject img = JObject.Parse(msgs["bodys"].ToString());
                                    string imgFilename = GetImage(img["url"].ToString());
                                    if (imgFilename == "")
                                        return;
                                    CoolQApi.SendGroupMsg(ListenConfig.QQGroup, String.Format("口袋房间：\r\n{0}:\r\n{1}", msg["senderName"].ToString(), CoolQCode.Image(imgFilename)));
                                    break;
                                case "faipaiText":
                                    CoolQApi.SendGroupMsg(ListenConfig.QQGroup, String.Format("口袋房间：\r\n翻牌辣！{3}:{4}\r\n{0} 回复:{1}\r\n被翻牌的大佬不来集资一发吗？" + ListenConfig.HitYouText + " \r\n发送时间:{2}", msg["senderName"].ToString(), msg["messageText"].ToString(), msgs["msgTimeStr"].ToString(), msg["faipaiName"].ToString(), msg["faipaiContent"].ToString()));
                                    break;
                                case "audio":
                                    JObject audio = JObject.Parse(msgs["bodys"].ToString());
                                    string audioFilename = GetAudio(audio["url"].ToString(), audio["ext"].ToString());
                                    if (audioFilename == "")
                                        return;
                                    CoolQApi.SendGroupMsg(ListenConfig.QQGroup, String.Format("口袋房间：\r\n{0}:\r\n{1}", msg["senderName"].ToString(), CoolQCode.ShareRecord(audioFilename)));
                                    break;
                                default:
                                    CoolQApi.SendGroupMsg(ListenConfig.QQGroup, "你的小偶像有一条新消息，TeemoBot无法支持该类型消息，请打开口袋48查看~~");
                                    break;
                            }
                        }
                    }
                    if (tmpTime != 0)
                        Lasttime = tmpTime;
                }
                if (First)
                    First = false;
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", DateTime.Now.ToString() + "\r\n" + ex.ToString() + "\r\n" + ex.StackTrace + "\r\n");
            }

        }
        public string GetAudio(string url, string suffix)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                WebResponse rep = req.GetResponse();
                string filename = Path.GetRandomFileName() + "." + suffix;
                Stream stream = new FileStream(Path.Combine("data/record/", filename), FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = rep.GetResponseStream().Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = rep.GetResponseStream().Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                rep.GetResponseStream().Close();
                return filename;
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", DateTime.Now.ToString() + "\r\n" + ex.ToString() + "\r\n" + ex.StackTrace + "\r\n");
                return "";
            }

        }
        public string GetImage(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                WebResponse rep = req.GetResponse();
                Bitmap img = new Bitmap(rep.GetResponseStream());
                string filename = Path.GetRandomFileName() + ".jpg";
                img.Save(Path.Combine("data/image/", filename));
                return filename;
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", DateTime.Now.ToString() + "\r\n" + ex.ToString() + "\r\n" + ex.StackTrace + "\r\n");
                return "";
            }

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
                            CoolQApi.SendGroupMsg(long.Parse(MemberIdCache[name[0]].ToString()), "直播提醒：\r\n你的小偶像" + name[0] + "开了一个" + name[1] + "\r\n请尽量打开口袋48观看！\r\n不方便的同学可以使用KD for PC（非官方）观看或打开网址登陆：https://h5.48.cn/2017appshare/memberLiveShare/index.html?id=" + liveInfo["liveId"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                File.AppendAllText("LiveError.log", ex.ToString() + "\r\n" + ex.StackTrace + ex.Data + "\r\n");
            }
        }

    }

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
}
