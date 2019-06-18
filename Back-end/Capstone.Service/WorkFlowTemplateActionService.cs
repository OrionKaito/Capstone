using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IWorkFlowTemplateActionService
    {
        IEnumerable<WorkFlowTemplateAction> GetAll();
        WorkFlowTemplateAction GetByID(Guid ID);
        WorkFlowTemplateAction GetByName(string name);
        void Create(WorkFlowTemplateAction workflowAction);
        void Save();
    }

    public class WorkFlowTemplateActionService : IWorkFlowTemplateActionService
    {
        private readonly IWorkFlowTemplateActionRepository _workFlowTemplateActionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkFlowTemplateActionService(IWorkFlowTemplateActionRepository workFlowTemplateActionRepository, IUnitOfWork unitOfWork)
        {
            _workFlowTemplateActionRepository = workFlowTemplateActionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(WorkFlowTemplateAction workflowAction)
        {
            _workFlowTemplateActionRepository.Add(workflowAction);
            _unitOfWork.Commit();
        }

        public IEnumerable<WorkFlowTemplateAction> GetAll()
        {
            return _workFlowTemplateActionRepository.GetAll().Where(w => w.IsDeleted == false);
        }

        public WorkFlowTemplateAction GetByID(Guid ID)
        {
            return _workFlowTemplateActionRepository.GetById(ID);
        }

        public WorkFlowTemplateAction GetByName(string name)
        {
            return _workFlowTemplateActionRepository.GetByName(name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
