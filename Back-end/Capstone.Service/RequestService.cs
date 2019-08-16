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
        IEnumerable<Request> GetRequestToApproveByPermissions(List<Guid> permissions);
        IEnumerable<Request> GetRequestToApproveByLineManager();
        IEnumerable<Request> GetRequestToApproveByInitiator(string userID);
        IEnumerable<Request> GetByUserID(string ID);
        Request GetByID(Guid ID);
        void Create(Request request);
        void Save();
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestActionRepository _requestActionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestService(IRequestRepository requestRepository, IRequestActionRepository requestActionRepository, IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _requestActionRepository = requestActionRepository;
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

        public IEnumerable<Request> GetRequestToApproveByPermissions(List<Guid> permissions)
        {
            //var requestNotComplete = _requestRepository.GetMany(r => r.IsCompleted == false);

            //List<Request> result = new List<Request>();

            //foreach (var request in requestNotComplete)
            //{
            //    var requestAction = _requestActionRepository.GetById(request.CurrentRequestActionID);
            //    var permisisonOfRequest = requestAction.NextStep.PermissionToUseID.GetValueOrDefault();
            //    if (permissions.Contains(permisisonOfRequest))
            //    {
            //        result.Add(request);
            //    }
            //}

            var pendingRequestAction = _requestActionRepository.GetMany(r => r.Status == StatusEnum.Pending);
            List<Request> result = new List<Request>();

            foreach (var requestAction in pendingRequestAction)
            {
                var permissionOfRequest = requestAction.NextStep.PermissionToUseID.GetValueOrDefault();
                if (permissions.Contains(permissionOfRequest))
                {
                    result.Add(requestAction.Request);
                }
            }

            return result;
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

        public IEnumerable<Request> GetRequestToApproveByLineManager()
        {
            var pendingRequestAction = _requestActionRepository.GetMany(r => r.Status == StatusEnum.Pending
            && r.NextStep.IsApprovedByLineManager == true);

            List<Request> result = new List<Request>();

            foreach (var requestAction in pendingRequestAction)
            {
                result.Add(requestAction.Request);
            }

            return result;
        }

        public IEnumerable<Request> GetRequestToApproveByInitiator(string userID)
        {
            var pendingRequestAction = _requestActionRepository.GetMany(r => r.Status == StatusEnum.Pending
            && r.NextStep.IsApprovedByInitiator == true && r.Request.InitiatorID == userID);

            List<Request> result = new List<Request>();

            foreach (var requestAction in pendingRequestAction)
            {
                result.Add(requestAction.Request);
            }

            return result;
        }
    }
}
