using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace BasicGUI
{
    public partial class Form1 : Form
    {
        // State
        string algorithm;
        string filecontent;
        string fullpath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            this.fullpath = openFileDialog1.FileName;
            string nameonly = System.IO.Path.GetFileName(this.fullpath);
            List<List<string>> data = Form1.parsingFile(this.fullpath);
            Graph gg = new Graph(data.Count);
            foreach (List<string> vertices in data)
            {
                gg.addEdge(vertices.First(), vertices.Last());
            }

            if (openFileDialog1.FileName.Contains(".txt"))
            {
                this.filecontent = File.ReadAllText(openFileDialog1.FileName);
                richTextBox1.Text = this.filecontent;
                label7.Text = nameonly;
            }
            else
            {
                MessageBox.Show("The file you've chosen is not a text file");
            }
            /// testing doang
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.AppendText(gg.friendRecommendationDFS("B"));
            
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }
        static List<List<string>> parsingFile(string path)
        {
            List<List<string>> res = new List<List<string>>();

            try
            {
                List<string> lines = System.IO.File.ReadAllLines(path).ToList();

                lines = lines.Where((val, idx) => idx != 0).ToList();
                foreach (string line in lines)
                {
                    res.Add(line.Split(" ").ToList());
                }

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return res;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
