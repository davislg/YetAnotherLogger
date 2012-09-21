using System;
using System.Windows.Forms;

namespace YAL.Demo
{
    public partial class Form1 : Form
    {
        string fileName;
        public Form1()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;
            YAL.Logger.AppName = "YAL Demo Log";
            YAL.FileLogger.BaseDirectory = string.Empty;
            fileName = DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            YAL.Logger.FileName = fileName + ".log";
            YAL.FileLogger.Create();
            Log();
        }

        private void Log()
        {
            LoggType loggType;
            Exception exception = null;

            if (this.comboBox2.SelectedIndex > 0)
            {
                switch (this.comboBox2.SelectedIndex)
                {
                    case 1:
                        exception = new System.IO.IOException();
                        break;
                    case 2:
                        exception = new Exception();
                        break;
                }
            }

            if (Enum.TryParse<LoggType>(this.comboBox1.Text, out loggType))
            {
                YAL.Logger.Default.Log(loggType, this.textBox1.Text, exception);
            }
            else
            {
                YAL.Logger.Default.Log(LoggType.General, this.textBox1.Text, exception);
            }

            MessageBox.Show("Error logged");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            YAL.Logger.FileName = fileName + ".xml";
            YAL.XmlLogger.Create();
            Log();
        }
    }
}
