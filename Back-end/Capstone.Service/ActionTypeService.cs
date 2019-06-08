using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IActionTypeService
    {
        IEnumerable<ActionType> GetAll();
        ActionType GetByID(Guid ID);
        ActionType GetByName(string Name);
        void Create(ActionType actionType);
        void Delete(ActionType actionType);
        void Save();
    }

    public class ActionTypeService : IActionTypeService
    {
        private readonly IActionTypeRepository _actionTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActionTypeService(IActionTypeRepository actionTypeRepository, IUnitOfWork unitOfWork)
        {
            _actionTypeRepository = actionTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(ActionType actionType)
        {
            _actionTypeRepository.Add(actionType);
            _unitOfWork.Commit();
        }

        public void Delete(ActionType actionType)
        {
            _actionTypeRepository.Delete(actionType);
            _unitOfWork.Commit();
        }

        public IEnumerable<ActionType> GetAll()
        {
            return _actionTypeRepository.GetAll().Where(a => a.IsDeleted == false);
        }

        public ActionType GetByID(Guid ID)
        {
            return _actionTypeRepository.GetById(ID);
        }

        public ActionType GetByName(string Name)
        {
            return _actionTypeRepository.GetByName(Name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
