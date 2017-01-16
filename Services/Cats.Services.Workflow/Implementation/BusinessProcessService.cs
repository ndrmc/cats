﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public class BusinessProcessService :IBusinessProcessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BusinessProcessService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
         public bool CheckPlanBeforeReject(BusinessProcessState st)
         {
             var state =
                 _unitOfWork.StateTemplateRepository.FindBy(s => s.StateTemplateID == st.StateID).FirstOrDefault();
             return state != null && state.Name == "Rejected";
         }

        public bool PromotWorkflow(BusinessProcessState state)
        {
            BusinessProcess item = this.FindById(state.ParentBusinessProcessID);
            if(item!=null)
            {

                _unitOfWork.BusinessProcessStateRepository.Add(state);
                _unitOfWork.Save();
                item.CurrentStateID = state.BusinessProcessStateID;
                _unitOfWork.BusinessProcessRepository.Edit(item);
                _unitOfWork.Save();
               
                return true;    
            }
            return false;
        }
        /// <summary>
        /// For Recording entries of  : Edit , Print and Delete
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool PromotWorkflow_WoutUpdatingCurrentStatus(BusinessProcessState state)
        {
                _unitOfWork.BusinessProcessStateRepository.Add(state);

                _unitOfWork.Save();

                return true;
     
        }
        public BusinessProcess CreateBusinessProcessWithOutStateEntry(int templateID, int documentID, string documentType)
        {           
            var bp = new BusinessProcess { ProcessTypeID = templateID, DocumentID = documentID, DocumentType = documentType };
            Add(bp);       
            return bp;
        }
        public BusinessProcess CreateBusinessProcess(int templateID, int documentID, string documentType, BusinessProcessState startingState)
        {
            var startingTemplate =
                _unitOfWork.StateTemplateRepository.FindBy(
                    s => s.ParentProcessTemplateID == templateID && s.StateType == 0).FirstOrDefault();
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
            _unitOfWork.BusinessProcessRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Update(BusinessProcess item)
        {
            if (item == null) return false;
            _unitOfWork.BusinessProcessRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }
        public bool Delete(BusinessProcess item)
        {
            if (item == null) return false;
            _unitOfWork.BusinessProcessRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var item = _unitOfWork.BusinessProcessRepository.FindById(id);
            return Delete(item);
        }
        public BusinessProcess FindById(int id)
        {
            return _unitOfWork.BusinessProcessRepository.FindById(id);
        }
        public List<BusinessProcess> GetAll()
        {
            return _unitOfWork.BusinessProcessRepository.GetAll();

        }
        public List<BusinessProcess> FindBy(Expression<Func<BusinessProcess, bool>> predicate)
        {
            return _unitOfWork.BusinessProcessRepository.FindBy(predicate);

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
            StateTemplate startingTemplate = _unitOfWork.StateTemplateRepository.FindBy(s => s.ParentProcessTemplateID == templateID && s.StateType == 0).Single();
            BusinessProcess bp = new BusinessProcess { ProcessTypeID = templateID, DocumentID = DocumentID, DocumentType = DocumentType };
            BusinessProcessState StartingState = new BusinessProcessState
            {
                DatePerformed = DateTime.Now,
                PerformedBy = "System",
                Comment = "Created workflow for" + DocumentType

            };         
             _unitOfWork.BusinessProcessRepository.Add(bp);

            StartingState.ParentBusinessProcess = bp;
            StartingState.StateID = startingTemplate.StateTemplateID;

            _unitOfWork.BusinessProcessStateRepository.Add(StartingState);
            bp.CurrentStateID = StartingState.BusinessProcessStateID;

         //   _unitOfWork.BusinessProcessRepository.Edit(bp);
          //  PromotWorkflow(StartingState);

            if (save)
            {
                _unitOfWork.Save();

            }
            return bp;
        }

        private int GetGlobalProcessTemplateId()
        {
            var applicationSetting =
                _unitOfWork.ApplicationSettingRepository.Get(p => p.SettingName == "GlobalWorkflow")
                    .FirstOrDefault();
            int globalId = 0;
            if (applicationSetting != null)
            {
                var processTemplateId =
                    applicationSetting
                        .SettingValue;
                globalId = Convert.ToInt32(processTemplateId);
            }
            return globalId;
        }
        public int GetGlobalEditStateTempId()
        {
            int globalId = GetGlobalProcessTemplateId();
            var stateTemplate = _unitOfWork.StateTemplateRepository.Get(
                t => t.ParentProcessTemplateID == globalId && t.Name == "Edited")
                .FirstOrDefault();
            if (stateTemplate != null)
                return
                    stateTemplate
                        .StateTemplateID;
            return 0;
        }
        public int GetGlobalCreatedStateTempId()
        {
            int globalId = GetGlobalProcessTemplateId();
            var stateTemplate = _unitOfWork.StateTemplateRepository.Get(
                t => t.ParentProcessTemplateID == globalId && t.Name == "Created")
                .FirstOrDefault();
            if (stateTemplate != null)
                return
                    stateTemplate
                        .StateTemplateID;
            return 0;
        }
        public int GetGlobalDeleteStateTempId()
        {
            int globalId = GetGlobalProcessTemplateId();
            var stateTemplate = _unitOfWork.StateTemplateRepository.Get(
                t => t.ParentProcessTemplateID == globalId && t.Name == "Deleted")
                .FirstOrDefault();
            if (stateTemplate != null)
                return
                    stateTemplate
                        .StateTemplateID;
            return 0;
        }

        public int GetGlobalPrintStateTempId()
        {
            int globalId = GetGlobalProcessTemplateId();
            var stateTemplate = _unitOfWork.StateTemplateRepository.Get(
                t => t.ParentProcessTemplateID == globalId && t.Name == "Printed")
                .FirstOrDefault();
            if (stateTemplate != null)
                return
                    stateTemplate
                        .StateTemplateID;
            return 0;
        }
        public int GetGlobalExportedStateTempId()
        {
            int globalId = GetGlobalProcessTemplateId();
            var stateTemplate = _unitOfWork.StateTemplateRepository.Get(
                t => t.ParentProcessTemplateID == globalId && t.Name == "Exported")
                .FirstOrDefault();
            if (stateTemplate != null)
                return
                    stateTemplate
                        .StateTemplateID;
            return 0;
        }
        public bool Save()
        {
            _unitOfWork.Save();
            return true;
        }

        public bool InsertBusinessProcessState(BusinessProcessState businessProcessState)
        {
            _unitOfWork.BusinessProcessStateRepository.Add(businessProcessState);
            _unitOfWork.Save();
            return true;
        }
    }
 }