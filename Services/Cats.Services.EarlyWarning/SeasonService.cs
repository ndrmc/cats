using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public class SeasonService:ISeasonService
    {   
         private readonly IUnitOfWork _unitOfWork;


         public SeasonService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool AddSeason(Season season)
        {
            _unitOfWork.SeasonRepository.Add(season);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteSeason(Season season)
        {
            if (season == null) return false;
            _unitOfWork.SeasonRepository.Delete(season);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.SeasonRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.SeasonRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditSeason(Season season)
        {
            _unitOfWork.SeasonRepository.Edit(season);
            _unitOfWork.Save();
            return true;
        }

        public Season FindById(int id)
        {
            return _unitOfWork.SeasonRepository.FindById(id);
        }

        public List<Season> GetAllSeason()
        {
            return _unitOfWork.SeasonRepository.GetAll();
        }

        public List<Season> FindBy(Expression<Func<Season, bool>> predicate)
        {
            return _unitOfWork.SeasonRepository.FindBy(predicate);
        }

        public IEnumerable<Season> Get(Expression<Func<Season, bool>> filter = null, Func<IQueryable<Season>, IOrderedQueryable<Season>> orderBy = null, string includeProperties = "")
        {
            return _unitOfWork.SeasonRepository.Get(filter, orderBy, includeProperties);
        }

        public List<Season> GetListOfSeasonsInRegion(List<string> regions )
        {
            var sesons =  _unitOfWork.SeasonRepository.GetAll();
            var seasonsInRegion = _unitOfWork.NeedAssessmentRepository.GetAll().Select(s=>s.Season1.Name);

            var filteredSeasons = from seasonList in sesons
                                  where seasonsInRegion.Contains(seasonList.Name)
                                  select seasonList;
            return filteredSeasons.ToList();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
