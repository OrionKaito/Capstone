using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IWorkFlowTemplateActionConnectionService
    {
        IEnumerable<WorkFlowTemplateActionConnection> GetAll();
        WorkFlowTemplateActionConnection GetByID(Guid ID);
        void Create(WorkFlowTemplateActionConnection connection);
        void Save();
    }

    public class WorkFlowTemplateActionConnectionService : IWorkFlowTemplateActionConnectionService
    {
        private readonly IWorkFlowTemplateActionConnectionRepository _workFlowTemplateActionConnectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkFlowTemplateActionConnectionService(IWorkFlowTemplateActionConnectionRepository workFlowTemplateActionConnectionRepository,
            IUnitOfWork unitOfWork)
        {
            _workFlowTemplateActionConnectionRepository = workFlowTemplateActionConnectionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(WorkFlowTemplateActionConnection connection)
        {
            _workFlowTemplateActionConnectionRepository.Add(connection);
            _unitOfWork.Commit();
        }

        public IEnumerable<WorkFlowTemplateActionConnection> GetAll()
        {
            return _workFlowTemplateActionConnectionRepository.GetAll().Where(w => w.IsDeleted == false);
        }

        public WorkFlowTemplateActionConnection GetByID(Guid ID)
        {
            return _workFlowTemplateActionConnectionRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }

    public class Graph
    {
        private int Vertices; //Tổng số đỉnh
        private List<int>[] adjency; //Các cạnh của các đỉnh

        public Graph(int vertices)
        {
            Vertices = vertices;
            adjency = new List<int>[vertices];
            for (int i = 0; i < vertices; i++)
            {
                adjency[i] = new List<int>();
            }
        }

        public void AddEdge(int vertice, int edge)
        {
            adjency[vertice].Add(edge);
        }

        public string DepthFirstSearch(int index)
        {
            string message = "";
            bool[] visited = new bool[Vertices];

            Stack<int> stack = new Stack<int>();
            visited[index] = true;
            stack.Push(index);

            while (stack.Count != 0)
            {
                index = stack.Pop();
                message += "next->" + index + "/n";
                foreach (int i in adjency[index]) //lấy tất cả các cạnh của đỉnh hiện tại mà chưa visit
                {
                    if (!visited[i])
                    {
                        visited[i] = true;
                        stack.Push(i);
                    }
                }
            }

            return message;
        }
    }
}
