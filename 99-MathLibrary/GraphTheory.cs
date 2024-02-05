namespace RST.Math
{
    public static class GraphTheory
    {
        // Za dano množico povezav, poiščimo maksimalno stopnjo grafa.
        public static int MaximumDegree(List<(int end1, int end2)> lstEdges)
        {
            if (lstEdges.Count == 0)
                return -1;

            // Compute vertex degrees for all endvertices over the edges
            Dictionary<int, int> dicVertexDegrees = new Dictionary<int, int>();
            foreach(var edge in lstEdges) 
            {
                IncreaseVertexDegree(dicVertexDegrees, edge.end1);
                IncreaseVertexDegree(dicVertexDegrees, edge.end2);
            }

            // Find a vertex with maximum degree
            int max = 0;
            foreach(var vertex in dicVertexDegrees.Keys)
            {
                if (dicVertexDegrees[vertex] >= max)
                    max = dicVertexDegrees[vertex];
            }
            return max;
        }

        // Za dano množico povezav, poiščimo maksimalno stopnjo grafa in dodatno vrnimo vozlišče z maksimalno stopnjo.
        public static int MaximumDegree(List<(int end1, int end2)> lstEdges, out int? maxDegreeVert)
        {
            // Preden zaključimo metodo, moramo nastaviti vse out parametre
            // int? pove, da lahko poleg celega števila vrnemo tudi null
            maxDegreeVert = null;

            if (lstEdges.Count == 0)
                return -1;

            // Compute vertex degrees for all endvertices over the edges
            Dictionary<int, int> dicVertexDegrees = new Dictionary<int, int>();
            foreach (var edge in lstEdges)
            {
                IncreaseVertexDegree(dicVertexDegrees, edge.end1);
                IncreaseVertexDegree(dicVertexDegrees, edge.end2);
            }

            // Find a vertex with maximum degree
            int max = 0;
            foreach (var vertex in dicVertexDegrees.Keys)
            {
                if (dicVertexDegrees[vertex] >= max)
                {
                    max = dicVertexDegrees[vertex];
                    maxDegreeVert = vertex;
                }
            }
            return max;
        }

        public static int MaximumDegree2(List<(int end1, int end2)> lstEdges)
        {
            if (lstEdges.Count == 0)
                return -1;

            // Compute vertex degrees for all endvertices over the edges
            Dictionary<int, Vertex> dicVertexDegrees = new Dictionary<int, Vertex>();
            foreach (var edge in lstEdges)
            {
                // Oglejmo si razliko med posredovanjem parametra dicVertexDegrees kot reference ali brez
                IncreaseVertexDegree(dicVertexDegrees, edge.end1);
                IncreaseVertexDegree(dicVertexDegrees, edge.end2);
            }

            // Find a vertex with maximum degree
            int max = 0;
            foreach (var vertex in dicVertexDegrees.Keys)
            {
                if (dicVertexDegrees[vertex].Degree >= max)
                    max = dicVertexDegrees[vertex].Degree;
            }
            return max;
        }
        private static void IncreaseVertexDegree(Dictionary<int, int> dicVertexDegrees, int vertex)
        {
            if (!dicVertexDegrees.ContainsKey(vertex))
                dicVertexDegrees.Add(vertex, 1);
            else
                dicVertexDegrees[vertex]++;
        }

        private static void IncreaseVertexDegree(Dictionary<int, Vertex> dicVertexDegrees, int vertex)
        {
            // Tale vrstica je samo za primerjavo delovanja ref
            //dicVertexDegrees = new Dictionary<int, Vertex>();
            if (!dicVertexDegrees.ContainsKey(vertex))
                dicVertexDegrees.Add(vertex, new Vertex(vertex) { Degree = 1 });
            else
                dicVertexDegrees[vertex].Degree++;
        }
    }

    public class Vertex
    {
        public Vertex(int id) 
        {
            this.ID = id;
        }

        public int ID { get; private set; }

        public int Degree { get; set; }

        public override string ToString()
        {
            return $"({this.ID},{this.Degree})";
        }
    }
}