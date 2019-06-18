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
}
