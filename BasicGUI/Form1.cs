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
        string source = "";
        string destination = "";
        string algorithm = "";
        string filecontent = "";
        string fullpath = "";
        Graph g;

        public Form1()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            openFileDialog1.ShowDialog();

            this.fullpath = openFileDialog1.FileName;
            List<List<string>> data = Form1.parsingFile(this.fullpath);
            this.g = new Graph(data.Count);

            foreach (List<string> vertices in data)
            {
                this.g.addEdge(vertices.First(), vertices.Last());
            }

            foreach (var x in this.g.getVertices())
            {
                comboBox1.Items.Add(x);
                comboBox2.Items.Add(x);
            }

            if (!openFileDialog1.FileName.Contains(".txt"))
            {
                MessageBox.Show("The file you've chosen is not a text file");
            }
            else
            {
                string filename = System.IO.Path.GetFileName(this.fullpath);
                label7.Text = filename;
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.algorithm = "DFS";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.algorithm = "BFS";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.source = comboBox1.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.destination = comboBox2.SelectedItem.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(this.source != "" && this.destination != "" && this.fullpath != "" && this.algorithm != "")
            {
                string filename = System.IO.Path.GetFileName(this.fullpath);
                this.filecontent = File.ReadAllText(openFileDialog1.FileName);
                richTextBox1.Text = this.filecontent;
                label7.Text = filename;

                if(this.algorithm == "BFS")
                {
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText(this.g.friendRecommendationBFS(this.source));
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText(this.g.exploreFriendBFS(this.source, this.destination));
                }
                else if(this.algorithm == "DFS")
                {
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText(this.g.friendRecommendationDFS(this.source));
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText(this.g.exploreFriendsDFS(this.source, this.destination));
                }
                else
                {
                    MessageBox.Show("Please input valid algorithm");
                }

            }
            else
            {
                MessageBox.Show("Please complete an input");
            }
        }
    }
}
