using System;
using System.Collections.Generic;
using Cats.Models;

namespace Cats.Services.Logistics
{
    public interface ITransportRequisitionDetailService : IDisposable
    {
        TransportRequisitionDetail FindById(int id);
        List<TransportRequisitionDetail> GetAllTransportRequisitionDetail();   
    }
}