using System;
using System.Collections.Generic;
using Cats.Models;

namespace Cats.Services.Transaction
{
    public interface ITranscationTypeService : IDisposable
    {
        Models.TransactionType FindById(int id);
        List<Models.TransactionType> GetAllTranscationType();
    }
}