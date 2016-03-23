using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
   public class CurrencyService:ICurrencyService
    {   
        private readonly IUnitOfWork _unitOfWork;

       public CurrencyService(IUnitOfWork unitOfWork)
       {
           _unitOfWork = unitOfWork;
       }

        public bool AddCurrency(Currency currency)
        {
            _unitOfWork.CurrencyRepository.Add(currency);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteCurrency(Currency currency)
        {
            if (currency == null) return false;
            _unitOfWork.CurrencyRepository.Delete(currency);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.CurrencyRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.CurrencyRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditCurrency(Currency currency)
        {
            _unitOfWork.CurrencyRepository.Edit(currency);
            _unitOfWork.Save();
            return true;
        }

        public Currency FindById(int id)
        {
            return _unitOfWork.CurrencyRepository.FindById(id);
        }

        public List<Currency> GetAllCurrency()
        {
            return _unitOfWork.CurrencyRepository.GetAll();
        }

        public List<Currency> FindBy(Expression<Func<Currency, bool>> predicate)
        {
            return _unitOfWork.CurrencyRepository.FindBy(predicate);
        }

        public IEnumerable<Currency> Get(Expression<Func<Currency, bool>> filter = null, Func<IQueryable<Currency>, IOrderedQueryable<Currency>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.CurrencyRepository.Get(filter, orderBy, includeProperties);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
