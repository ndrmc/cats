﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels.HRD;

namespace Cats.Services.EarlyWarning
{
    public class HRDService : IHRDService
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public HRDService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool AddHRD(HRD hrd)
        {
            _unitOfWork.HRDRepository.Add(hrd);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteHRD(HRD hrd)
        {

            if (hrd == null) return false;
            var hrdDetails = _unitOfWork.HRDDetailRepository.FindBy(h => h.HRDID == hrd.HRDID);
            foreach (var hrdDetail in hrdDetails)
            {
                _unitOfWork.HRDDetailRepository.Delete(hrdDetail);
            }
            _unitOfWork.HRDRepository.Delete(hrd);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.HRDRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.HRDRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditHRD(HRD hrd)
        {
            _unitOfWork.HRDRepository.Edit(hrd);
            _unitOfWork.Save();
            return true;
        }

        public HRD FindById(int id)
        {
            return _unitOfWork.HRDRepository.FindById(id);
        }

        public List<HRD> GetAllHRD()
        {
            return _unitOfWork.HRDRepository.GetAll();
        }

        public List<HRD> FindBy(Expression<Func<HRD, bool>> predicate)
        {
            return _unitOfWork.HRDRepository.FindBy(predicate);
        }

        public IEnumerable<HRDDetail> GetHRDDetailByHRDID(int hrdID)
        {
            return _unitOfWork.HRDDetailRepository.Get(t => t.HRDID == hrdID);
        }
        public IEnumerable<HRD> Get(Expression<Func<HRD, bool>> filter = null,
                                    Func<IQueryable<HRD>, IOrderedQueryable<HRD>> orderBy = null,
                                    string includeProperties = "")
        {
            return _unitOfWork.HRDRepository.Get(filter, orderBy, includeProperties);
        }

        public void PublishHrd(int hrdId)
        {
            // Get the Current HRD and the one to set as current
            var newHrd = _unitOfWork.HRDRepository.FindById(hrdId);
            var currentHrd = this.Get(m => m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Published").FirstOrDefault();

            try
            {
                //newHrd.Status = 3;
                newHrd.PublishedDate = DateTime.Now;
                //// If the currentHrd is null it means we don't have a current HRD in the database.
                //if (null !=currentHrd)
                //    currentHrd.Status = 4;
                //_unitOfWork.Save();
               
            }
            catch (Exception ex)
            {
                // TODO: Log the current exception and throw domain specific error.
            }

        }
        public List<Plan> GetPlans()
        {
            return _unitOfWork.PlanRepository.FindBy(m => m.Program.Name == "Relief");
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }


        public Plan GetPlan(int id)
        {
            return _unitOfWork.PlanRepository.FindById(id);
        }


        public bool AddHRDFromAssessment(HRD hrd)
        {
            _unitOfWork.HRDRepository.Add(hrd);
            _unitOfWork.Save();
            var plan=_unitOfWork.PlanRepository.FindById(hrd.PlanID);
            plan.Status = (int) PlanStatus.HRDCreated;
             
            _unitOfWork.PlanRepository.Edit(plan);
            _unitOfWork.Save();
            return true;

        }


        public bool AddHRD(int year, int userID, int seasonID, int rationID, int planID, int businessProcessId)
        {
            var plan = _unitOfWork.PlanRepository.FindById(planID);
            var woredas = _unitOfWork.AdminUnitRepository.FindBy(m => m.AdminUnitTypeID == 4);
            var hrd = new HRD()
                {
                    Year = year,
                    CreatedBY = userID,
                    SeasonID = seasonID,
                    RationID = rationID,
                    PlanID = planID,
                    CreatedDate = DateTime.Now,
                    PublishedDate = DateTime.Now,
                Status = (int?)HRDStatus.Draft,
                BusinessProcessId = businessProcessId
                };
            var hrdDetails = (from detail in woredas
                              select new HRDDetail
                              {
                                  WoredaID = detail.AdminUnitID,
                                  StartingMonth = plan.StartDate.Month,
                                  NumberOfBeneficiaries = 0,
                                  DurationOfAssistance = 0
                              }).ToList();

            hrd.HRDDetails = hrdDetails;
            _unitOfWork.HRDRepository.Add(hrd);
            _unitOfWork.Save();
            return true;

        }
       public int GetWoredaBeneficiaryNumber(int hrdID, int woredaID)
       {
           var hrdDetail=_unitOfWork.HRDDetailRepository.FindBy(m => m.HRDID == hrdID && m.WoredaID == woredaID).FirstOrDefault();
           if(hrdDetail!=null)
           {
               return hrdDetail.NumberOfBeneficiaries;
           }
           return 0;
       }

        public IEnumerable<HRDViewModel> GetHrds()
        {
            var hrds = GetAllHRD();
            return (from hrd in hrds
                    let ration = hrd.Ration
                    where ration != null
                    select new HRDViewModel
                    {
                        HRDID = hrd.HRDID,
                        Season = hrd.Season.Name,
                        Year = hrd.Year,
                        Ration = ration.RefrenceNumber,
                        CreatedDate = hrd.CreatedDate,
                        CreatedBy = hrd.UserProfile.FirstName + " " + hrd.UserProfile.LastName,
                        PublishedDate = hrd.PublishedDate,
                        //StatusID = hrd.Status,
                        Plan = hrd.Plan.PlanName,
                    });
        }
    }
}