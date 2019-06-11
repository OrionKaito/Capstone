using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;


namespace Capstone.Service
{

    public interface IRequestService
    {
        IEnumerable<Request> GetAll();
        IEnumerable<Request> GetByUserID(string ID);
        Request GetByID(Guid ID);
        void Create(Request request);
        void Save();
    }
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestService(IRequestRepository requestRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(Request request)
        {
            _requestRepository.Add(request);
            _unitOfWork.Commit();
        }

        public IEnumerable<Request> GetAll()
        {
            return _requestRepository.GetMany(r => r.IsDeleted == false);
        }

        public Request GetByID(Guid ID)
        {
            return _requestRepository.GetById(ID);
        }

        public IEnumerable<Request> GetByUserID(string ID)
        {
            return _requestRepository.GetMany(r => r.InitiatorID.Equals(ID));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
