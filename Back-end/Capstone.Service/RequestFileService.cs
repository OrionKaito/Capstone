﻿using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IRequestFileService
    {
        IEnumerable<RequestFile> GetAll();
        IEnumerable<RequestFile> GetByRequestActionID(Guid ID);
        RequestFile GetByID(Guid ID);
        void Create(RequestFile requestFile);
        void Save();
    }
    public class RequestFileService : IRequestFileService
    {
        private readonly IRequestFileRepository _requestFileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestFileService(IRequestFileRepository requestFileRepository, IUnitOfWork unitOfWork)
        {
            _requestFileRepository = requestFileRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(RequestFile requestFile)
        {
            _requestFileRepository.Add(requestFile);
        }

        public IEnumerable<RequestFile> GetAll()
        {
            return _requestFileRepository.GetAll().Where(a => a.IsDeleted == false);
        }

        public RequestFile GetByID(Guid ID)
        {
            return _requestFileRepository.GetById(ID);
        }

        public IEnumerable<RequestFile> GetByRequestActionID(Guid ID)
        {
            return _requestFileRepository.GetMany(r => r.RequestActionID == ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
