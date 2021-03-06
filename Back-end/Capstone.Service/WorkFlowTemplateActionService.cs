﻿using Capstone.Data.Infrastructrure;
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
        WorkFlowTemplateAction GetStartByWorkFlowID(Guid ID);
        IEnumerable<WorkFlowTemplateAction> GetByWorkFlowID(Guid ID);
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

        public IEnumerable<WorkFlowTemplateAction> GetByWorkFlowID(Guid ID)
        {
            return _workFlowTemplateActionRepository.GetMany(x => x.WorkFlowTemplateID == ID);
        }

        public WorkFlowTemplateAction GetStartByWorkFlowID(Guid ID)
        {
            return _workFlowTemplateActionRepository.GetStartByWorFlowID(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
