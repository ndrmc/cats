using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.Logistics;
using Cats.Services.Common;

namespace Cats.Services.Logistics
{
    public class TransportRequisitionDetailService : ITransportRequisitionDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly INotificationService _notificationService;
     

        public TransportRequisitionDetailService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            //_notificationService = notificationService;
        }

        #region Default Service Implementation

        public List<TransportRequisitionDetail> GetAllTransportRequisitionDetail()
        {
            return _unitOfWork.TransportRequisitionDetailRepository.GetAll();
        }
        public TransportRequisitionDetail FindById(int id)
        {
            return _unitOfWork.TransportRequisitionDetailRepository.FindById(id);
        }
     
        #endregion
        
        public void Dispose()
        {
            _unitOfWork.Dispose();

        }        
    }
}


