namespace Teemo.CoolQ.Plugins
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lab_thread_status = new System.Windows.Forms.Label();
            this.lab_thread_id = new System.Windows.Forms.Label();
            this.lab_fanpai = new System.Windows.Forms.Label();
            this.txt_fanpai = new System.Windows.Forms.TextBox();
            this.lab_roommsg_delay = new System.Windows.Forms.Label();
            this.txt_roommsg_delay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lab_thread_action = new System.Windows.Forms.Label();
            this.btn_stop = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_reget = new System.Windows.Forms.Button();
            this.lab_roommsg_count = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(397, 318);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(478, 318);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lab_thread_status
            // 
            this.lab_thread_status.AutoSize = true;
            this.lab_thread_status.Location = new System.Drawing.Point(31, 26);
            this.lab_thread_status.Name = "lab_thread_status";
            this.lab_thread_status.Size = new System.Drawing.Size(89, 12);
            this.lab_thread_status.TabIndex = 3;
            this.lab_thread_status.Text = "当前线程状态：";
            // 
            // lab_thread_id
            // 
            this.lab_thread_id.AutoSize = true;
            this.lab_thread_id.Location = new System.Drawing.Point(43, 56);
            this.lab_thread_id.Name = "lab_thread_id";
            this.lab_thread_id.Size = new System.Drawing.Size(77, 12);
            this.lab_thread_id.TabIndex = 4;
            this.lab_thread_id.Text = "当前线程ID：";
            // 
            // lab_fanpai
            // 
            this.lab_fanpai.AutoSize = true;
            this.lab_fanpai.Location = new System.Drawing.Point(31, 173);
            this.lab_fanpai.Name = "lab_fanpai";
            this.lab_fanpai.Size = new System.Drawing.Size(89, 12);
            this.lab_fanpai.TabIndex = 5;
            this.lab_fanpai.Text = "翻牌追加文案：";
            // 
            // txt_fanpai
            // 
            this.txt_fanpai.Location = new System.Drawing.Point(33, 199);
            this.txt_fanpai.Multiline = true;
            this.txt_fanpai.Name = "txt_fanpai";
            this.txt_fanpai.Size = new System.Drawing.Size(499, 104);
            this.txt_fanpai.TabIndex = 6;
            // 
            // lab_roommsg_delay
            // 
            this.lab_roommsg_delay.AutoSize = true;
            this.lab_roommsg_delay.Location = new System.Drawing.Point(331, 26);
            this.lab_roommsg_delay.Name = "lab_roommsg_delay";
            this.lab_roommsg_delay.Size = new System.Drawing.Size(89, 12);
            this.lab_roommsg_delay.TabIndex = 7;
            this.lab_roommsg_delay.Text = "房间信息延时：";
            // 
            // txt_roommsg_delay
            // 
            this.txt_roommsg_delay.Location = new System.Drawing.Point(422, 22);
            this.txt_roommsg_delay.Name = "txt_roommsg_delay";
            this.txt_roommsg_delay.Size = new System.Drawing.Size(68, 21);
            this.txt_roommsg_delay.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(497, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "ms(毫秒)";
            // 
            // lab_thread_action
            // 
            this.lab_thread_action.AutoSize = true;
            this.lab_thread_action.Location = new System.Drawing.Point(55, 88);
            this.lab_thread_action.Name = "lab_thread_action";
            this.lab_thread_action.Size = new System.Drawing.Size(65, 12);
            this.lab_thread_action.TabIndex = 10;
            this.lab_thread_action.Text = "线程操作：";
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(117, 83);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 12;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "信息更新：";
            // 
            // btn_reget
            // 
            this.btn_reget.Location = new System.Drawing.Point(117, 114);
            this.btn_reget.Name = "btn_reget";
            this.btn_reget.Size = new System.Drawing.Size(75, 23);
            this.btn_reget.TabIndex = 14;
            this.btn_reget.Text = "刷新";
            this.btn_reget.UseVisualStyleBackColor = true;
            this.btn_reget.Click += new System.EventHandler(this.btn_reget_Click);
            // 
            // lab_roommsg_count
            // 
            this.lab_roommsg_count.AutoSize = true;
            this.lab_roommsg_count.Location = new System.Drawing.Point(307, 56);
            this.lab_roommsg_count.Name = "lab_roommsg_count";
            this.lab_roommsg_count.Size = new System.Drawing.Size(113, 12);
            this.lab_roommsg_count.TabIndex = 15;
            this.lab_roommsg_count.Text = "房间信息获取次数：";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 353);
            this.Controls.Add(this.lab_roommsg_count);
            this.Controls.Add(this.btn_reget);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.lab_thread_action);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_roommsg_delay);
            this.Controls.Add(this.lab_roommsg_delay);
            this.Controls.Add(this.txt_fanpai);
            this.Controls.Add(this.lab_fanpai);
            this.Controls.Add(this.lab_thread_id);
            this.Controls.Add(this.lab_thread_status);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flexlive.CQP.CSharpPlugins.Demo 插件设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lab_thread_status;
        private System.Windows.Forms.Label lab_thread_id;
        private System.Windows.Forms.Label lab_fanpai;
        private System.Windows.Forms.TextBox txt_fanpai;
        private System.Windows.Forms.Label lab_roommsg_delay;
        private System.Windows.Forms.TextBox txt_roommsg_delay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lab_thread_action;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_reget;
        private System.Windows.Forms.Label lab_roommsg_count;
    }
}