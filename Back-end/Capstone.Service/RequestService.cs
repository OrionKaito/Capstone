﻿using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{

    public interface IRequestService
    {
        IEnumerable<Request> GetAll();
        IEnumerable<Request> GetRequestToApproveByPermissions(List<Guid> permissions);
        IEnumerable<Request> GetRequestToApproveByLineManager();
        IEnumerable<Request> GetRequestToApproveByInitiator(string userID);
        IEnumerable<Request> GetByUserID(string ID);
        IEnumerable<Request> GetRequestNotAbleToHandleByPermission();
        IEnumerable<Request> GetRequestNotAbleToHandleByLineManager();
        Request GetByID(Guid ID);
        int CountMyRequest(string ID);
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
        private readonly IPermissionOfGroupRepository _permissionOfGroupRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestService(IRequestRepository requestRepository
            , IRequestActionRepository requestActionRepository
            , IPermissionOfGroupRepository permissionOfGroupRepository
            , IUserGroupRepository userGroupRepository
            , IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _requestActionRepository = requestActionRepository;
            _permissionOfGroupRepository = permissionOfGroupRepository;
            _userGroupRepository = userGroupRepository;
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

        public IEnumerable<Request> GetRequestNotAbleToHandleByPermission()
        {
            List<Request> requestsNoPermisison = new List<Request>();

            var requestsActionPermission = _requestActionRepository.GetMany(r => r.Status == StatusEnum.Pending
            && r.NextStep.PermissionToUse != null);

            foreach (var requestAction in requestsActionPermission)
            {
                //Lấy permissionOfGroup của nhưng request action vừa rồi
                var permissionOfGroups = _permissionOfGroupRepository.GetMany(p => p.PermissionID == requestAction.NextStep.PermissionToUse.ID);

                //Lấy những permissionOfGroup theo permissionID
                var permissionInGroup = _permissionOfGroupRepository.GetByPermission(requestAction.NextStep.PermissionToUse.ID);
                //Kiểm tra permission đó có group chưa
                if (permissionInGroup == null)
                {
                    requestsNoPermisison.Add(requestAction.Request);
                }

                foreach (var permissionOfGroup in permissionOfGroups)
                {
                    //Kiểm tra usergroup có tồn tại không
                    var userGroup = _userGroupRepository.GetMany(u => u.GroupID == permissionOfGroup.GroupID);
                    if (userGroup.Count() == 0)
                    {
                        requestsNoPermisison.Add(requestAction.Request);
                    }
                }
            }
            
            return requestsNoPermisison;
        }

        public IEnumerable<Request> GetRequestNotAbleToHandleByLineManager()
        {
            //Lấy các request action mà duyệt bằng linemanager mà không có
            var requestActions = _requestActionRepository.GetMany(r => r.Status == StatusEnum.Pending
            && r.NextStep.IsApprovedByLineManager == true
            && r.Request.Initiator.LineManagerID == null);

            //Lấy các request action mà duyệt bằng permission 
            List<Request> requestsNoLineManager = new List<Request>();

            foreach (var requestAction in requestActions)
            {
                requestsNoLineManager.Add(requestAction.Request);
            }

            return requestsNoLineManager;
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

        public int CountMyRequest(string ID)
        {
            return _requestRepository.CountMyRequest(ID);
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
