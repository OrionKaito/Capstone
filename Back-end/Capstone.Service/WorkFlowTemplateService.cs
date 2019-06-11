using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IWorkFlowTemplateService
    {
        IEnumerable<WorkFlowTemplate> GetAll();
        WorkFlowTemplate GetByID(Guid ID);
        WorkFlowTemplate GetByName(string name);
        void Create(WorkFlowTemplate workflow);
        void Update(WorkFlowTemplate workflow);
        void Delete(WorkFlowTemplate workflow);
        void Save();
    }

    public class WorkFlowTemplateService : IWorkFlowTemplateService
    {
        private readonly IWorkFlowTemplateRepository _workFlowTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkFlowTemplateService(IWorkFlowTemplateRepository workFlowRepository, IUnitOfWork unitOfWork)
        {
            _workFlowTemplateRepository = workFlowRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(WorkFlowTemplate workflow)
        {
            _workFlowTemplateRepository.Add(workflow);
            _unitOfWork.Commit();
        }

        public void Delete(WorkFlowTemplate workflow)
        {
            _workFlowTemplateRepository.Delete(workflow);
            _unitOfWork.Commit();
        }

        public IEnumerable<WorkFlowTemplate> GetAll()
        {
            return _workFlowTemplateRepository.GetAll().Where(w => w.IsDeleted == false && w.IsEnabled == true);
        }

        public WorkFlowTemplate GetByID(Guid ID)
        {
            return _workFlowTemplateRepository.GetById(ID);
        }

        public WorkFlowTemplate GetByName(string name)
        {
            return _workFlowTemplateRepository.GetByName(name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(WorkFlowTemplate workflow)
        {
            _workFlowTemplateRepository.Update(workflow);
            _unitOfWork.Commit();
        }
    }
}
