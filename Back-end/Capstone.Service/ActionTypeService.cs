using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Service
{
    public interface IActionTypeService
    {
        IEnumerable<ActionType> GetAll();
        ActionType GetByID(Guid ID);
        void Create(ActionType actionType);
        void Update(ActionType actionType);
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
        }

        public void Delete(ActionType actionType)
        {
            _actionTypeRepository.Delete(actionType);
        }

        public IEnumerable<ActionType> GetAll()
        {
            return _actionTypeRepository.GetAll();
        }

        public ActionType GetByID(Guid ID)
        {
            return _actionTypeRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ActionType actionType)
        {
            _actionTypeRepository.Update(actionType);
        }
    }
}
