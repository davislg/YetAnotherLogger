using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YAL.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            YAL.BaseLogger.AppName = "YAL Demo Log";
            YAL.FileLogger.BaseDirectory = string.Empty;
            string filename = DateTime.Now + ".log";
            YAL.BaseLogger.FileName = filename;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoggType loggType;
            if (Enum.TryParse<LoggType>(this.comboBox1.Text, out loggType))
            {
                YAL.FileLogger.Default.Log(loggType, this.textBox1.Text);
            }
            else
            {
                YAL.FileLogger.Default.Log(LoggType.General, this.textBox1.Text);
            }

            MessageBox.Show("Error logged");
        }
    }
}
