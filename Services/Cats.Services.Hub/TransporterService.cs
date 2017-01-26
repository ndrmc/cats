using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cats.Data.Hub;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;
using Cats.Services.Workflows;

namespace Cats.Services.Hub
{
    public class TransporterService : ITransporterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkflowActivityService _IWorkflowActivityService;

        public TransporterService(IUnitOfWork unitOfWork, IWorkflowActivityService _IWorkflowActivityService)
        {
            this._unitOfWork = unitOfWork;
            this._IWorkflowActivityService = _IWorkflowActivityService;
        }

        public bool AddTransporter(Transporter entity)
       {
            
           _unitOfWork.TransporterRepository.Add(entity);
            _IWorkflowActivityService.EnterCreateWorkflow(entity);
            _unitOfWork.Save();
           return true;
           
       }
       public bool EditTransporter(Transporter entity)
       {
           _unitOfWork.TransporterRepository.Edit(entity);
            _IWorkflowActivityService.EnterEditWorkflow(entity);

            _unitOfWork.Save();
           return true;

       }
         public bool DeleteTransporter(Transporter entity)
        {
             if(entity==null) return false;
            _IWorkflowActivityService.EnterDeleteWorkflow(entity);

            _unitOfWork.TransporterRepository.Edit(entity);

            _unitOfWork.Save();
            return true;
        }
       public  bool DeleteById(int id)
       {
           var entity = _unitOfWork.TransporterRepository.FindById(id);
           if(entity==null) return false;
            DeleteTransporter(entity);

            return true;
       }
       public List<Transporter> GetAllTransporter()
       {
         
            var allRecords = _unitOfWork.TransporterRepository.GetAll().Cast<IWorkflowHub>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(allRecords).Cast<Transporter>().ToList<Transporter>();

        }
        public Transporter FindById(int id)
       {
           var record= _unitOfWork.TransporterRepository.FindById(id);

            List<IWorkflowHub> lst = new List<IWorkflowHub>();
            lst.Add((IWorkflowHub)record);
            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(lst).Cast<Transporter>().FirstOrDefault<Transporter>();
             

        }
        public List<Transporter> FindBy(Expression<Func<Transporter, bool>> predicate)
       {
     

            var allRecords = _unitOfWork.TransporterRepository.FindBy(predicate).Cast<IWorkflowHub>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecordsHub(allRecords).Cast<Transporter>().ToList<Transporter>();

        }


        public void Dispose()
       {
           _unitOfWork.Dispose();
           
       }

        public bool IsNameValid(int? transporterID, string name)
        {
             
       
           var trans = _unitOfWork.TransporterRepository.FindBy(t=>t.Name == name && t.TransporterID!=transporterID)                ;

            List<IWorkflowHub> lst = new List<IWorkflowHub>();
            lst.Add((IWorkflowHub)trans);
            var tr = _IWorkflowActivityService.ExcludeDeletedRecordsHub(lst).Any();

            if (tr == false) return false;
           return true;

       
        }

       
    }
       
}




      
      