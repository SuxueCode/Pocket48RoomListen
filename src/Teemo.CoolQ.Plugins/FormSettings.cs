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
        public FormSettings()
        {
            InitializeComponent();
            this.Text = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name + " 参数设置";
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
            
            this.btnExit_Click(null, null);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Task.Abort();
        }

        private void btn_reget_Click(object sender, EventArgs e)
        {
            
        }
    }
}
