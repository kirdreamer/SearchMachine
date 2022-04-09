using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SearchMachine
{
    public partial class Form1 : Form
    {
        bool stopFlag = false;
        string searchingLine;
        public Form1()
        {
            InitializeComponent();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string path = pathBox.Text;
            if (File.Exists(path))
            {
                using (FileStream fstream = File.OpenRead(path))
                {
                    stopFlag = false;
                    long endPos = 0;
                    long beginPos = 0;
                    AnswerLabel.Text = "";
                    searchingLine = textBox1.Text;
                    button1.Enabled = false;
                    pathOpen.Enabled = false;
                    stopButton.Enabled = true;

                    while (fstream.Position < fstream.Length)
                    {
                        if (stopFlag)
                            break;
                        bool flag = false;
                        beginPos = endPos;
                        int it = 0;

                        byte[] ch = new byte[1];
                        do
                        {
                            fstream.Read(ch, 0, 1);
                            if ((!Encoding.Default.GetString(ch).Equals("\n") &&
                                !Encoding.Default.GetString(ch).Equals("\r")) && !flag)
                            {
                                if (searchingLine.Length > 0)
                                {
                                    if (searchingLine[it].Equals(Encoding.Default.GetString(ch)[0]))
                                        ++it;
                                    if (it == searchingLine.Length)
                                        flag = true;
                                }
                            }
                        }
                        while ((!Encoding.Default.GetString(ch).Equals("\n") &&
                                !Encoding.Default.GetString(ch).Equals("\r")) && fstream.Position < fstream.Length);
                        endPos = fstream.Position;
                        if (flag)
                        {
                            fstream.Seek(beginPos - endPos, SeekOrigin.Current);
                            byte[] output = new byte[endPos - beginPos - 1];
                            fstream.Read(output, 0, output.Length);
                            string textfile = Encoding.Default.GetString(output);
                            AnswerLabel.Text += textfile + "\n";
                            panel1.Refresh();
                        }
                        fstream.Seek(endPos, SeekOrigin.Begin);
                        Application.DoEvents();

                    }
                    stopButton.Enabled = false;
                    button1.Enabled = true;
                    pathOpen.Enabled = true;
                }
            }
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void PathOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            pathBox.Text = "";
            pathBox.Text += openFileDialog1.FileName;
            MessageBox.Show("The following file was selected : " + openFileDialog1.FileName);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            stopFlag = true;
        }
    }
}
