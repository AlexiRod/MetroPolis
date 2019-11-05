using static System.Linq.Enumerable;
using static System.String;
using static System.Console;
using System.Collections.Generic;
using System;

namespace Metropolis
{
    public class GraphEdge      /// Ребро графа 
    {
        public GraphVertex ConnectedVertex { get; } // Связанная вершина
        public int EdgeWeight { get; }  // Вес ребра
        public GraphEdge(GraphVertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;  //Связанная вершина
            EdgeWeight = weight;  // Вес ребра
        }
    }

    public class GraphVertex // Вершина графа
    {
        public string Name { get; }                                 // Название вершины
        public string RealName { get; }                             // Название вершины
        public List<GraphEdge> Edges { get; }                       // Список ребер
        
        public GraphVertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
        }
        
        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }
        
        public void AddEdge(GraphVertex vertex, int edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }

        public override string ToString() => Name;     
    }
 
    public class Graph                                          // Граф
    {
        public List<GraphVertex> Vertices { get; }              // Список вершин графа
        public Graph() {Vertices = new List<GraphVertex>(); }   // Конструктор графа

        public void AddVertex(string vertexName)    
            {Vertices.Add(new GraphVertex(vertexName)); }
        

        public GraphVertex FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
                {if (v.Name.Equals(vertexName)) return v;}
            return null;
        }
        
        public void AddEdge(string firstName, string secondName, int weight)
        {
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }

    public class GraphVertexInfo                            // Информация о вершине
    {
        public GraphVertex Vertex { get; set; }             // Вершина
        public bool IsUnvisited { get; set; }               // Не посещенная вершина
        public int EdgesWeightSum { get; set; }             // Сумма весов ребер
        public GraphVertex PreviousVertex { get; set; }     // Предыдущая вершина
 
        public GraphVertexInfo(GraphVertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
    }
    
    public class Dijkstra
    {
        Graph graph;
        List<GraphVertexInfo> infos;
        public Dijkstra(Graph graph) { this.graph = graph;}
        
        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();
            foreach (var v in graph.Vertices)
                {infos.Add(new GraphVertexInfo(v));}
        }
        
        GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (var i in infos)
                { if (i.Vertex.Equals(v)) return i;}
            return null;
        }
        
        public GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            GraphVertexInfo minVertexInfo = null;
            foreach (var i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }
            return minVertexInfo;
        }
        
        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName));
        }
        
        public string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null) break;
                SetSumToNextVertex(current);
            }
            return GetPath(startVertex, finishVertex);
        }

        void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        
        string GetPath(GraphVertex startVertex, GraphVertex endVertex)
        {
            var path = endVertex.ToString();
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                path = endVertex.ToString() + " " + path;
            }
            return path;
        }
    }
}