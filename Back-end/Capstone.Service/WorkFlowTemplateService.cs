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
        IEnumerable<WorkFlowTemplate> GetByPermissionToUse(Guid permissionID);
        IEnumerable<WorkFlowTemplate> SearchByNameOnPermissionToUse(string search, Guid permissionID);
        WorkFlowTemplate GetByID(Guid ID);
        WorkFlowTemplate GetByName(string name);
        int CountByIsDeleted();
        void Create(WorkFlowTemplate workflow);
        void Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }

    public class WorkFlowTemplateService : IWorkFlowTemplateService
    {
        private readonly IWorkFlowTemplateRepository _workFlowTemplateRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkFlowTemplateService(IWorkFlowTemplateRepository workFlowTemplateRepository, IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _workFlowTemplateRepository = workFlowTemplateRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(WorkFlowTemplate workflow)
        {
            _workFlowTemplateRepository.Add(workflow);
            _unitOfWork.Commit();
        }

        public IEnumerable<WorkFlowTemplate> GetAll()
        {
            return _workFlowTemplateRepository.GetAll().Where(w => w.IsDeleted == false);
        }

        public WorkFlowTemplate GetByID(Guid ID)
        {
            return _workFlowTemplateRepository.GetById(ID);
        }

        public WorkFlowTemplate GetByName(string name)
        {
            return _workFlowTemplateRepository.GetByName(name);
        }

        public IEnumerable<WorkFlowTemplate> GetByPermissionToUse(Guid permissionID)
        {
            return _workFlowTemplateRepository.GetByPermissionToUse(permissionID).OrderByDescending(w => w.CreateDate);
        }

        public IEnumerable<WorkFlowTemplate> SearchByNameOnPermissionToUse(string search, Guid permissionID)
        {
            return _workFlowTemplateRepository.SearchByNameOnPermissionToUse(permissionID, search);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _unitOfWork.CommitTransaction();
        }

        public void RollBack()
        {
            _unitOfWork.RollBack();
        }

        public int CountByIsDeleted()
        {
            return _workFlowTemplateRepository.GetAll().Where(w => w.IsDeleted == false).Count();
        }
    }
}
