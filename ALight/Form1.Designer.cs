namespace ALight
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SPP = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.Resolution_Input = new System.Windows.Forms.TextBox();
            this.SPP_Input = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(55, 177);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(267, 58);
            this.button1.TabIndex = 0;
            this.button1.Text = "渲染";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 426);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(357, 61);
            this.progressBar1.TabIndex = 1;
            // 
            // SPP
            // 
            this.SPP.AutoSize = true;
            this.SPP.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SPP.Location = new System.Drawing.Point(57, 348);
            this.SPP.Name = "SPP";
            this.SPP.Size = new System.Drawing.Size(0, 65);
            this.SPP.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(55, 241);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(267, 58);
            this.button2.TabIndex = 3;
            this.button2.Text = "保存";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Resolution_Input
            // 
            this.Resolution_Input.Location = new System.Drawing.Point(105, 31);
            this.Resolution_Input.Name = "Resolution_Input";
            this.Resolution_Input.Size = new System.Drawing.Size(217, 25);
            this.Resolution_Input.TabIndex = 4;
            this.Resolution_Input.Text = "512";
            // 
            // SPP_Input
            // 
            this.SPP_Input.Location = new System.Drawing.Point(105, 62);
            this.SPP_Input.Name = "SPP_Input";
            this.SPP_Input.Size = new System.Drawing.Size(217, 25);
            this.SPP_Input.TabIndex = 5;
            this.SPP_Input.Text = "1024";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "分辨率";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "SPP";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 499);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SPP_Input);
            this.Controls.Add(this.Resolution_Input);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SPP);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.Text = "ALight";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label SPP;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox Resolution_Input;
        private System.Windows.Forms.TextBox SPP_Input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

