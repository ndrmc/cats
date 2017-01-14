using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public interface IBusinessProcessService
    {
         bool Add(BusinessProcess item);
         bool Update(BusinessProcess item);
         bool Delete(BusinessProcess item);
         bool DeleteById(int id);
         BusinessProcess FindById(int id);
         List<BusinessProcess> GetAll();
         List<BusinessProcess> FindBy(Expression<Func<BusinessProcess, bool>> predicate);
         bool PromotWorkflow(BusinessProcessState state); 
         bool PromotWorkflow_WoutUpdatingCurrentStatus(BusinessProcessState state); 
          BusinessProcess CreateBusinessProcess(int templateID, int documentID, string documentType, BusinessProcessState startingState);
         BusinessProcess CreateBusinessProcessForObject(int templateID, int DocumentID, string DocumentType, bool save = false);
         bool Save();
        bool CheckPlanBeforeReject(BusinessProcessState st);

        int GetGlobalEditStateTempId();
         int GetGlobalDeleteStateTempId();
        int GetGlobalPrintStateTempId();
        int GetGlobalCreatedStateTempId();
        int GetGlobalExportedStateTempId();
        BusinessProcess CreateBusinessProcessWithOutStateEntry(int bpPr, int i, string created);
        bool InsertBusinessProcessState(BusinessProcessState businessProcessState);
    }
}