using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Service
{
  public interface IHangfireService
    {
        void checkAndChange();
    }
    public class HangfireService : IHangfireService
    {
        
        private readonly IRequestValueRepository _requestValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HangfireService(IRequestValueRepository requestValueRepository, IUnitOfWork unitOfWork)
        {
            _requestValueRepository = requestValueRepository;
            _unitOfWork = unitOfWork;
        }

        public void checkAndChange()
        {

            IEnumerable<RequestValue> allRequestValue = _requestValueRepository.GetAll();

            DateTime timeNow = DateTime.Now;
            DateTime timeNow5min = timeNow.AddMinutes(-5);
            foreach (RequestValue i in allRequestValue)
            {
                
                RequestValue rv = new RequestValue();
                rv.data = "chien dep trai vkl ha ha ha bonus them nhieu thu nua";
                rv.ID = new Guid();
                rv.timeStamp = DateTime.Now;

                
                
                if ((DateTime.Compare(timeNow, i.timeStamp) >0)&& (DateTime.Compare(timeNow5min, i.timeStamp) < 0)) {

                    _requestValueRepository.Add(rv);


                } else {
                    rv.data = "chien galang ahihi <3";
                    _requestValueRepository.Add(rv);
                }

            }
            Save();
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
    
}
