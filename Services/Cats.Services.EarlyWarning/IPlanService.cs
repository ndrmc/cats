﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IPlanService : IDisposable
    {
        bool AddPlan(Plan plan);
        bool DeletePlan(Plan plan);
        bool DeleteById(int id);
        bool EditPlan(Plan plan);
        Plan FindById(int id);
        List<Plan> GetAllPlan();
        List<Plan> FindBy(Expression<Func<Plan, bool>> predicate);

        IEnumerable<Plan> Get(
             Expression<Func<Plan, bool>> filter = null,
             Func<IQueryable<Plan>, IOrderedQueryable<Plan>> orderBy = null,
             string includeProperties = "");

        List<Program> GetPrograms();
        List<Program> GetNonReliefProgram();
        void AddNeedAssessmentPlan(NeedAssessment needAssessment);
        void AddNeedAssessmentPlan(string planName, DateTime startDate, DateTime endDate, int businessProcessID);
        void AddPlan(string planName, DateTime startDate, DateTime endDate);
        void AddHRDPlan(string planName, DateTime startDate, DateTime endDate, int BusinessProcessID);
        List<NeedAssessment> PlannedNeedAssessment(int planID);
        List<HRD> PlannedHRD(int planID);
        void ChangePlanStatus(int planID);
        void AssessmentPlanStatus(Plan plan);
        HRD GetHrd(int id);
        IEnumerable<HrdDonorCoverage> GetDonorCoverage(
             Expression<Func<HrdDonorCoverage, bool>> filter = null,
             Func<IQueryable<HrdDonorCoverage>, IOrderedQueryable<HrdDonorCoverage>> orderBy = null,
             string includeProperties = "");
        string FindHrdDonorCoverage(List<HrdDonorCoverage> hrdDonorCoverages, int fdpID);
    }
}