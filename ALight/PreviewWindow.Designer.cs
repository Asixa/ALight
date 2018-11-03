namespace ALight
{
    partial class PreviewWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewWindow));
            this.TimeLabel = new System.Windows.Forms.Label();
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.DenoiseButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flip = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.spp_input = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MST_input = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.height_input = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.width_input = new System.Windows.Forms.TextBox();
            this.renderermode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Renderer = new System.Windows.Forms.Button();
            this.topanel = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.Loading = new System.Windows.Forms.Label();
            this.Canvas_denoise = new System.Windows.Forms.PictureBox();
            this.MenuPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.topanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas_denoise)).BeginInit();
            this.SuspendLayout();
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TimeLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.TimeLabel.Location = new System.Drawing.Point(9, 502);
            this.TimeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(49, 19);
            this.TimeLabel.TabIndex = 3;
            this.TimeLabel.Text = "TIME";
            // 
            // MenuPanel
            // 
            this.MenuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.MenuPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MenuPanel.Controls.Add(this.DenoiseButton);
            this.MenuPanel.Controls.Add(this.groupBox2);
            this.MenuPanel.Controls.Add(this.groupBox1);
            this.MenuPanel.Controls.Add(this.Renderer);
            this.MenuPanel.Location = new System.Drawing.Point(1, 45);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(200, 447);
            this.MenuPanel.TabIndex = 4;
            // 
            // DenoiseButton
            // 
            this.DenoiseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DenoiseButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DenoiseButton.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.DenoiseButton.Location = new System.Drawing.Point(2, 336);
            this.DenoiseButton.Name = "DenoiseButton";
            this.DenoiseButton.Size = new System.Drawing.Size(192, 32);
            this.DenoiseButton.TabIndex = 13;
            this.DenoiseButton.Text = "Denoise";
            this.DenoiseButton.UseVisualStyleBackColor = false;
            this.DenoiseButton.Click += new System.EventHandler(this.DenoiseButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flip);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Location = new System.Drawing.Point(2, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 102);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Debuging";
            // 
            // flip
            // 
            this.flip.AutoSize = true;
            this.flip.Location = new System.Drawing.Point(9, 20);
            this.flip.Name = "flip";
            this.flip.Size = new System.Drawing.Size(81, 19);
            this.flip.TabIndex = 1;
            this.flip.Text = "flip image";
            this.flip.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.spp_input);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.MST_input);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.height_input);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.width_input);
            this.groupBox1.Controls.Add(this.renderermode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(2, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 165);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Renderer Setting";
            // 
            // spp_input
            // 
            this.spp_input.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.spp_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.spp_input.ForeColor = System.Drawing.SystemColors.Window;
            this.spp_input.Location = new System.Drawing.Point(62, 90);
            this.spp_input.Name = "spp_input";
            this.spp_input.Size = new System.Drawing.Size(116, 14);
            this.spp_input.TabIndex = 11;
            this.spp_input.Text = "64";
            this.spp_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(6, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "SPP";
            // 
            // MST_input
            // 
            this.MST_input.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MST_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MST_input.ForeColor = System.Drawing.SystemColors.Window;
            this.MST_input.Location = new System.Drawing.Point(62, 125);
            this.MST_input.Name = "MST_input";
            this.MST_input.Size = new System.Drawing.Size(116, 14);
            this.MST_input.TabIndex = 13;
            this.MST_input.Text = "8";
            this.MST_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(6, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "MAX_SCATTER_TIME";
            // 
            // height_input
            // 
            this.height_input.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.height_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.height_input.ForeColor = System.Drawing.SystemColors.Window;
            this.height_input.Location = new System.Drawing.Point(62, 70);
            this.height_input.Name = "height_input";
            this.height_input.Size = new System.Drawing.Size(116, 14);
            this.height_input.TabIndex = 2;
            this.height_input.Text = "1080";
            this.height_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Scanner";
            // 
            // width_input
            // 
            this.width_input.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.width_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.width_input.ForeColor = System.Drawing.SystemColors.Window;
            this.width_input.Location = new System.Drawing.Point(62, 51);
            this.width_input.Name = "width_input";
            this.width_input.Size = new System.Drawing.Size(116, 14);
            this.width_input.TabIndex = 0;
            this.width_input.Text = "1920";
            this.width_input.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // renderermode
            // 
            this.renderermode.BackColor = System.Drawing.SystemColors.GrayText;
            this.renderermode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.renderermode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.renderermode.ForeColor = System.Drawing.SystemColors.Window;
            this.renderermode.FormattingEnabled = true;
            this.renderermode.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.renderermode.Items.AddRange(new object[] {
            "Shaded",
            "NormalMap"});
            this.renderermode.Location = new System.Drawing.Point(62, 24);
            this.renderermode.Name = "renderermode";
            this.renderermode.Size = new System.Drawing.Size(116, 23);
            this.renderermode.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Height";
            // 
            // Renderer
            // 
            this.Renderer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Renderer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Renderer.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Renderer.Location = new System.Drawing.Point(3, 19);
            this.Renderer.Name = "Renderer";
            this.Renderer.Size = new System.Drawing.Size(192, 32);
            this.Renderer.TabIndex = 8;
            this.Renderer.Text = "Renderer";
            this.Renderer.UseVisualStyleBackColor = true;
            this.Renderer.Click += new System.EventHandler(this.Renderer_Click);
            // 
            // topanel
            // 
            this.topanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topanel.Controls.Add(this.button4);
            this.topanel.Controls.Add(this.button6);
            this.topanel.Controls.Add(this.button5);
            this.topanel.Controls.Add(this.button3);
            this.topanel.Controls.Add(this.button1);
            this.topanel.Location = new System.Drawing.Point(1, 2);
            this.topanel.Name = "topanel";
            this.topanel.Size = new System.Drawing.Size(404, 40);
            this.topanel.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Red;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Arial Black", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button4.Location = new System.Drawing.Point(100, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(30, 30);
            this.button4.TabIndex = 6;
            this.button4.Text = "R";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Visible = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Blue;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Arial Black", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button6.Location = new System.Drawing.Point(160, 5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(30, 30);
            this.button6.TabIndex = 8;
            this.button6.Text = "B";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Visible = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Green;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Arial Black", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button5.Location = new System.Drawing.Point(130, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(30, 30);
            this.button5.TabIndex = 7;
            this.button5.Text = "G";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Visible = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button3.Image = global::ALight.Properties.Resources.save;
            this.button3.Location = new System.Drawing.Point(40, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(38, 40);
            this.button3.TabIndex = 2;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.Save_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.Image = global::ALight.Properties.Resources.Menu;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 40);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Menu_Click);
            // 
            // Canvas
            // 
            this.Canvas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Canvas.BackgroundImage")));
            this.Canvas.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Canvas.Location = new System.Drawing.Point(2, 28);
            this.Canvas.Margin = new System.Windows.Forms.Padding(2);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(685, 467);
            this.Canvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragScene);
            this.Canvas.DragEnter += new System.Windows.Forms.DragEventHandler(this.Canvas_DragEnter);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            // 
            // Loading
            // 
            this.Loading.BackColor = System.Drawing.Color.Transparent;
            this.Loading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Loading.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Loading.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Loading.Location = new System.Drawing.Point(356, 64);
            this.Loading.Name = "Loading";
            this.Loading.Size = new System.Drawing.Size(302, 169);
            this.Loading.TabIndex = 6;
            this.Loading.Text = "Loading Scene";
            this.Loading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Loading.Visible = false;
            // 
            // Canvas_denoise
            // 
            this.Canvas_denoise.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Canvas_denoise.BackgroundImage")));
            this.Canvas_denoise.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Canvas_denoise.Location = new System.Drawing.Point(1, 28);
            this.Canvas_denoise.Margin = new System.Windows.Forms.Padding(2);
            this.Canvas_denoise.Name = "Canvas_denoise";
            this.Canvas_denoise.Size = new System.Drawing.Size(685, 467);
            this.Canvas_denoise.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Canvas_denoise.TabIndex = 7;
            this.Canvas_denoise.TabStop = false;
            // 
            // PreviewWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(686, 523);
            this.Controls.Add(this.Loading);
            this.Controls.Add(this.topanel);
            this.Controls.Add(this.MenuPanel);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.Canvas_denoise);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PreviewWindow";
            this.Text = "ALight - Ray Tracing Renderer by Xingyu Chen";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PreviewWindow_FormClosed);
            this.SizeChanged += new System.EventHandler(this.PreviewWindow_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragScene);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Canvas_DragEnter);
            this.MenuPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.topanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas_denoise)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Panel topanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox height_input;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox width_input;
        private System.Windows.Forms.Button Renderer;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox renderermode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox spp_input;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MST_input;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox flip;
        public System.Windows.Forms.Label Loading;
        private System.Windows.Forms.PictureBox Canvas_denoise;
        private System.Windows.Forms.Button DenoiseButton;
    }
}