using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Teemo.CoolQ.Plugins
{
    public partial class FormSettings : Form
    {
        public Thread Task;
        public ListenConfig ListenConfig;
        public FormSettings(ref Thread _task,ref ListenConfig _listenConfig)
        {
            InitializeComponent();
            Task = _task;
            ListenConfig = _listenConfig;
            this.Text = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name + " 参数设置";
            Get();
        }

        public void Get()
        {
            lab_thread_id.Text = "当前线程ID：" + Task.ManagedThreadId;
            lab_thread_status.Text = "当前线程状态：" + Task.IsAlive;
            lab_roommsg_count.Text = "房间信息获取次数：" + ListenConfig.GetRoomMsgCount;
            txt_fanpai.Text = ListenConfig.HitYouText;
            txt_roommsg_delay.Text = ListenConfig.GetRoomMsgDelay.ToString();
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int RoomDelay;
            try
            {
                RoomDelay = int.Parse(txt_roommsg_delay.Text);
            }
            catch
            {
                MessageBox.Show("延时设定出错，请确认是否正确！");
                return;
            }
            ListenConfig.GetRoomMsgDelay = RoomDelay;
            ListenConfig.HitYouText = txt_fanpai.Text;
            this.btnExit_Click(null, null);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Task.Abort();
        }

        private void btn_reget_Click(object sender, EventArgs e)
        {
            Get();
        }
    }
}
