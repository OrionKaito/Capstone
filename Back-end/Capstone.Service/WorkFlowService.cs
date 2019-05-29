using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;

namespace Capstone.Service
{
    public interface IWorkFlowService
    {
        IEnumerable<WorkFlow> GetAll();
        WorkFlow GetByID(Guid ID);
        void Create(WorkFlow workflow);
        void Update(WorkFlow workflow);
        void Delete(WorkFlow workflow);
        void Save();
    }

    public class WorkFlowService : IWorkFlowService
    {
        private readonly IWorkFlowRepository _workFlowRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkFlowService(IWorkFlowRepository workFlowRepository, IUnitOfWork unitOfWork)
        {
            _workFlowRepository = workFlowRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(WorkFlow workflow)
        {
            _workFlowRepository.Add(workflow);
            _unitOfWork.Commit();
        }

        public void Delete(WorkFlow workflow)
        {
            _workFlowRepository.Delete(workflow);
            _unitOfWork.Commit();
        }

        public IEnumerable<WorkFlow> GetAll()
        {
            return _workFlowRepository.GetAll();
        }

        public WorkFlow GetByID(Guid ID)
        {
            return _workFlowRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(WorkFlow workflow)
        {
            _workFlowRepository.Update(workflow);
            _unitOfWork.Commit();
        }
    }
}
