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

        /// Конструктор вершины
        /// <param name="vertexName">Название вершины</param>
        public GraphVertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
        }

        /// Добавить ребро
        /// <param name="newEdge">Ребро</param>
        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }

        /// Добавить ребро
        /// <param name="vertex">Вершина</param>
        /// <param name="edgeWeight">Вес</param>
        public void AddEdge(GraphVertex vertex, int edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }

        public override string ToString() => Name;      // Преобразование в строку <returns>Имя вершины</returns>
    }
 
    public class Graph                                          // Граф
    {
        public List<GraphVertex> Vertices { get; }              // Список вершин графа
        public Graph() {Vertices = new List<GraphVertex>(); }   // Конструктор графа

        public void AddVertex(string vertexName)    // Добавление вершины <param name="vertexName">Имя вершины</param>
            {Vertices.Add(new GraphVertex(vertexName)); }

        /// Поиск вершины
        /// <param name="vertexName">Название вершины</param>
        /// <returns>Найденная вершина</returns>
        public GraphVertex FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
                {if (v.Name.Equals(vertexName)) return v;}
            return null;
        }

        /// Добавление ребра
        /// <param name="firstName">Имя первой вершины</param>
        /// <param name="secondName">Имя второй вершины</param>
        /// <param name="weight">Вес ребра соединяющего вершины</param>
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
 
        public GraphVertexInfo(GraphVertex vertex)          // Конструктор <param name="vertex">Вершина</param>
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
    }

    /// Алгоритм Дейкстры
    public class Dijkstra
    {
        Graph graph;
        List<GraphVertexInfo> infos;
        /// Конструктор
        /// <param name="graph">Граф</param>
        public Dijkstra(Graph graph) { this.graph = graph;}

        /// Инициализация информации
        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();
            foreach (var v in graph.Vertices)
                {infos.Add(new GraphVertexInfo(v));}
        }

        /// Получение информации о вершине графа
        /// <param name="v">Вершина</param>
        /// <returns>Информация о вершине</returns>
        GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (var i in infos)
                { if (i.Vertex.Equals(v)) return i;}
            return null;
        }

        /// Поиск непосещенной вершины с минимальным значением суммы
        /// <returns>Информация о вершине</returns>
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
        /// Поиск кратчайшего пути по названиям вершин
        /// <param name="startName">Название стартовой вершины</param>
        /// <param name="finishName">Название финишной вершины</param>
        /// <returns>Кратчайший путь</returns>
        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName));
        }
        /// Поиск кратчайшего пути по вершинам
        /// <param name="startVertex">Стартовая вершина</param>
        /// <param name="finishVertex">Финишная вершина</param>
        /// <returns>Кратчайший путь</returns>
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

        /// Вычисление суммы весов ребер для следующей вершины
        /// <param name="info">Информация о текущей вершине</param>
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

        /// Формирование пути
        /// <param name="startVertex">Начальная вершина</param>
        /// <param name="endVertex">Конечная вершина</param>
        /// <returns>Путь</returns>
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