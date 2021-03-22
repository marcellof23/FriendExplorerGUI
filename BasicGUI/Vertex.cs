using System;
using System.Collections.Generic;
using System.Text;

namespace BasicGUI
{
    class Vertex
    {
        public string value;
        public List<string> edges;
        public Vertex(string _value)
        {
            this.value = _value;
            this.edges = new List<string>();
        }
    }
}
