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
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using Color = Microsoft.Msagl.Drawing.Color;
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
        Microsoft.Msagl.Drawing.Graph graph;
        List<string> res;
        // List<Microsoft.Msagl.Drawing.Edge> Edgess;
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
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.graph = new Microsoft.Msagl.Drawing.Graph("graph");
            foreach (List<string> vertices in data)
            {
                var Edge = this.graph.AddEdge(vertices.First(), vertices.Last());
                Edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                Edge.Attr.ArrowheadAtSource = ArrowStyle.None;
                this.g.addEdge(vertices.First(), vertices.Last());
                this.graph.FindNode(vertices.First()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                this.graph.FindNode(vertices.Last()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                // Edgess.Add(Edge);
            }
            gViewer1.Graph = this.graph;
            this.SuspendLayout();
            this.ResumeLayout();
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
                this.filecontent = File.ReadAllText(openFileDialog1.FileName);
                richTextBox1.Text = this.filecontent;
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
            List<List<string>> data = Form1.parsingFile(this.fullpath);
            this.graph = new Microsoft.Msagl.Drawing.Graph("graph");
            foreach (List<string> vertices in data)
            {
                var Edge = this.graph.AddEdge(vertices.First(), vertices.Last());
                Edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                Edge.Attr.ArrowheadAtSource = ArrowStyle.None;
                this.graph.FindNode(vertices.First()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                this.graph.FindNode(vertices.Last()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
            }


            if (this.source != "" && this.destination != "" && this.fullpath != "" && this.algorithm != "")
            {
                string filename = System.IO.Path.GetFileName(this.fullpath);
                label7.Text = filename;

                if (this.algorithm == "BFS")
                {
                    richTextBox1.Clear();
                    richTextBox1.Focus();
                    this.res = new List<string>();
                    richTextBox1.AppendText(this.g.friendRecommendationBFS(this.source));
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText(this.g.exploreFriendBFS(this.source, this.destination, ref this.res));
                    if (res.Any())
                    {
                        this.graph = new Microsoft.Msagl.Drawing.Graph("graph");
                        foreach (List<string> vertices in data)
                        {
                            bool colorEdge = false;
                            for(int i = 0; i < this.res.Count() - 1; i++)
                            {
                                if ((this.res[i] == vertices.First() && this.res[i + 1] == vertices.Last()) || (this.res[i] == vertices.Last() && this.res[i + 1] == vertices.First()))
                                {
                                    colorEdge = true;
                                    break;
                                }
                            }
                            var Edge = this.graph.AddEdge(vertices.First(), vertices.Last());
                            if(colorEdge == true)
                            {
                                Edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Magenta;
                            }
                            Edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                            Edge.Attr.ArrowheadAtSource = ArrowStyle.None;
                            this.graph.FindNode(vertices.First()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                            this.graph.FindNode(vertices.Last()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                        }

                        for (int i = 0; i < this.res.Count(); i++)
                        {
                            this.graph.FindNode(this.res[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Plum;
                        }
                    }
                }

                else if (this.algorithm == "DFS")
                {
                    richTextBox1.Clear();
                    richTextBox1.Focus();
                    this.res = new List<string>();
                    richTextBox1.AppendText(this.g.friendRecommendationDFS(this.source));
                    richTextBox1.AppendText(Environment.NewLine);
                    richTextBox1.AppendText(this.g.exploreFriendsDFS(this.source, this.destination, ref this.res));
                    if (res.Any())
                    {
                        foreach(var x in res)
                        {
                            richTextBox1.AppendText(x);
                        }

                        this.graph = new Microsoft.Msagl.Drawing.Graph("graph");
                        foreach (List<string> vertices in data)
                        {
                            bool colorEdge = false;
                            for (int i = 0; i < this.res.Count() - 1; i++)
                            {
                                if ((this.res[i] == vertices.First() && this.res[i + 1] == vertices.Last()) || (this.res[i] == vertices.Last() && this.res[i + 1] == vertices.First()))
                                {
                                    colorEdge = true;
                                    break;
                                }
                            }
                            var Edge = this.graph.AddEdge(vertices.First(), vertices.Last());
                            if (colorEdge == true)
                            {
                                Edge.Attr.Color = Microsoft.Msagl.Drawing.Color.Magenta;
                            }
                            Edge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                            Edge.Attr.ArrowheadAtSource = ArrowStyle.None;
                            this.graph.FindNode(vertices.First()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                            this.graph.FindNode(vertices.Last()).Attr.Shape = Microsoft.Msagl.Drawing.Shape.Circle;
                        }

                        for (int i = 0; i < this.res.Count(); i++)
                        {
                            this.graph.FindNode(this.res[i]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Plum;
                        }
                    }
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

            gViewer1.Graph = this.graph;
            this.SuspendLayout();
            this.ResumeLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}