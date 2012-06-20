using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorkflowWithBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var p = new Process
                        {
                            StartInfo = new ProcessStartInfo("iexplore.exe")
                                            {
                                                Arguments = "http://www.clean-code-advisors.de",
                                                WindowStyle = ProcessWindowStyle.Maximized
                                            }
                        };
            p.Start();
            p.WaitForExit();
            p.Close();

            textBox1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
