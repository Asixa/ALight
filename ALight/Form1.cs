﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcDx;

namespace ALight
{
    public partial class Form1 : Form
    {
        private readonly Render.Renderer renderer=new Render.Renderer();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            renderer.Init();
        }
    }
}
