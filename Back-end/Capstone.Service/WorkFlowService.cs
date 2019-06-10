using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IWorkFlowService
    {
        IEnumerable<WorkFlowTemplate> GetAll();
        WorkFlowTemplate GetByID(Guid ID);
        WorkFlowTemplate GetByName(string name);
        void Create(WorkFlowTemplate workflow);
        void Update(WorkFlowTemplate workflow);
        void Delete(WorkFlowTemplate workflow);
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

        public void Create(WorkFlowTemplate workflow)
        {
            _workFlowRepository.Add(workflow);
            _unitOfWork.Commit();
        }

        public void Delete(WorkFlowTemplate workflow)
        {
            _workFlowRepository.Delete(workflow);
            _unitOfWork.Commit();
        }

        public IEnumerable<WorkFlowTemplate> GetAll()
        {
            return _workFlowRepository.GetAll().Where(w => w.IsDeleted == false && w.IsEnabled == true);
        }

        public WorkFlowTemplate GetByID(Guid ID)
        {
            return _workFlowRepository.GetById(ID);
        }

        public WorkFlowTemplate GetByName(string name)
        {
            return _workFlowRepository.GetByName(name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(WorkFlowTemplate workflow)
        {
            _workFlowRepository.Update(workflow);
            _unitOfWork.Commit();
        }
    }
}
