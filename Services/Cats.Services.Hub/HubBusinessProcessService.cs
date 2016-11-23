using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.Hub.UnitWork;
using Cats.Models.Hubs;

namespace Cats.Services.Hub
{
    public class HubBusinessProcessService :IHubBusinessProcessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HubBusinessProcessService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
         public bool CheckPlanBeforeReject(BusinessProcessState st)
         {
             var state =
                 _unitOfWork.HubStateTemplateRepository.FindBy(s => s.StateTemplateID == st.StateID).FirstOrDefault();
             return state != null && state.Name == "Rejected";
         }

        public bool PromotWorkflow(BusinessProcessState state)
        {
            BusinessProcess item = this.FindById(state.ParentBusinessProcessID);
            if(item!=null)
            {

                _unitOfWork.HubBusinessProcessStateRepository.Add(state);
                _unitOfWork.Save();
                item.CurrentStateID = state.BusinessProcessStateID;
                _unitOfWork.HubBusinessProcessRepository.Edit(item);
                _unitOfWork.Save();
               
                return true;    
            }
            return false;
        }

        public BusinessProcess CreateBusinessProcess(int templateID, int documentID, string documentType, BusinessProcessState startingState)
        {
            var startingTemplate=_unitOfWork.HubStateTemplateRepository.FindBy(s => s.ParentProcessTemplateID == templateID && s.StateType == 0).FirstOrDefault();
            if (startingTemplate==null)
                return null;
            var bp = new BusinessProcess { ProcessTypeID = templateID, DocumentID = documentID, DocumentType = documentType };
            Add(bp);
            startingState.ParentBusinessProcessID = bp.BusinessProcessID;
            startingState.StateID = startingTemplate.StateTemplateID;
            PromotWorkflow(startingState);
            return bp;
        }
        public bool Add(BusinessProcess item)
        {
            _unitOfWork.HubBusinessProcessRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Update(BusinessProcess item)
        {
            if (item == null) return false;
            _unitOfWork.HubBusinessProcessRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Delete(BusinessProcess item)
        {
            if (item == null) return false;
            _unitOfWork.HubBusinessProcessRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.HubBusinessProcessRepository.FindById(id);
            return Delete(item);
        }
        public BusinessProcess FindById(int id)
        {
            return _unitOfWork.HubBusinessProcessRepository.FindById(id);
        }
        public List<BusinessProcess> GetAll()
        {
            return _unitOfWork.HubBusinessProcessRepository.GetAll();

        }
        public List<BusinessProcess> FindBy(Expression<Func<BusinessProcess, bool>> predicate)
        {
            return _unitOfWork.HubBusinessProcessRepository.FindBy(predicate);

        }
        /*
          BusinessProcessState createdstate = new BusinessProcessState
                        {
                            DatePerformed = DateTime.Now,
                            PerformedBy = "System",
                            Comment = "Created workflow for Payment Request"

                        };
                        _PaymentRequestservice.Create(request);

                        BusinessProcess bp = _BusinessProcessService.CreateBusinessProcess(BP_PR,request.PaymentRequestID,
                                                                                           "PaymentRequest", createdstate);
                        request.BusinessProcessID = bp.BusinessProcessID;
         */
        public BusinessProcess CreateBusinessProcessForObject(int templateID, int DocumentID, string DocumentType,bool save=false)
        {
            StateTemplate startingTemplate = _unitOfWork.HubStateTemplateRepository.FindBy(s => s.ParentProcessTemplateID == templateID && s.StateType == 0).Single();
            BusinessProcess bp = new BusinessProcess { ProcessTypeID = templateID, DocumentID = DocumentID, DocumentType = DocumentType };
            BusinessProcessState StartingState = new BusinessProcessState
            {
                DatePerformed = DateTime.Now,
                PerformedBy = "System",
                Comment = "Created workflow for" + DocumentType

            };         
             _unitOfWork.HubBusinessProcessRepository.Add(bp);

            StartingState.ParentBusinessProcess = bp;
            StartingState.StateID = startingTemplate.StateTemplateID;

            _unitOfWork.HubBusinessProcessStateRepository.Add(StartingState);
            bp.CurrentStateID = StartingState.BusinessProcessStateID;

         //   _unitOfWork.BusinessProcessRepository.Edit(bp);
          //  PromotWorkflow(StartingState);

            if (save)
            {
                _unitOfWork.Save();

            }
            return bp;
        }


        public bool Save()
        {
            _unitOfWork.Save();
            return true;
        }
    }
 }