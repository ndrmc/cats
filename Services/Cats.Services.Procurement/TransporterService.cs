using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;
using System.Linq;
using Cats.Models.Constant;
using Cats.Services.Workflows;

namespace Cats.Services.Procurement
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
        #region Default Service Implementation
        public bool AddTransporter(Transporter transporter)
        {

            _unitOfWork.TransporterRepository.Add(transporter);

            _IWorkflowActivityService.EnterCreateWorkflow(transporter);

            _unitOfWork.Save();


            return true;

        }
        public bool EditTransporter(Transporter transporter)
        {
            _unitOfWork.TransporterRepository.Edit(transporter);

            _IWorkflowActivityService.EnterEditWorkflow(transporter);

            _unitOfWork.Save();
            return true;

        }
        public bool DeleteTransporter(Transporter transporter)
        {
            if (transporter == null) return false;

            _IWorkflowActivityService.EnterDeleteWorkflow(transporter);

            _unitOfWork.TransporterRepository.Edit(transporter);

            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.TransporterRepository.FindById(id);
            if (entity == null) return false;
            DeleteTransporter(entity);

            return true;
        }
        public List<Transporter> GetAllTransporter()
        {
            var allRecords = _unitOfWork.TransporterRepository.GetAll().Cast<IWorkflow>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecords(allRecords).Cast<Transporter>().ToList<Transporter>();
        }
        public Transporter FindById(int id)
        {

            List<IWorkflow> lst = new List<IWorkflow>();
            lst.Add(_unitOfWork.TransporterRepository.FindById(id));

            return _IWorkflowActivityService.ExcludeDeletedRecords(lst).Cast<Transporter>().FirstOrDefault<Transporter>();

        }
        public List<Transporter> FindBy(Expression<Func<Transporter, bool>> predicate)
        {

            var allRecords = _unitOfWork.TransporterRepository.FindBy(predicate).Cast<IWorkflow>().ToList();
            return _IWorkflowActivityService.ExcludeDeletedRecords(allRecords).Cast<Transporter>().ToList<Transporter>();

        }
        #endregion


        public List<BidWinner> GetBidWinner(int sourceID, int DestinationID, int bidId)
        {
            List<BidWinner> Winners = new List<BidWinner>();

            var bidWinner =
                _unitOfWork.BidWinnerRepository.Get(
                    t => t.SourceID == sourceID && t.DestinationID == DestinationID && t.Position == 1 &&
                        t.Bid.BidID == bidId).FirstOrDefault();

            if (bidWinner == null)
            {
                return Winners;
            }
            var bidIdstr = bidWinner.BidID.ToString();
            if (bidIdstr == "")
            {
                return Winners;
            }
            if (bidIdstr != "")
            {
                var currentBidId = int.Parse(bidIdstr);
                Winners = _unitOfWork.BidWinnerRepository.FindBy(q => q.BidID == currentBidId && q.SourceID == sourceID && q.DestinationID == DestinationID && q.Position == 1);
                Winners.OrderBy(t => t.Position);
            }
            return Winners.OrderBy(t => t.Position).ToList();


        }


        public BidWinner GetCurrentBidWinner(int sourceID, int DestincationID, int bidId)
        {
            var winners = GetBidWinner(sourceID, DestincationID, bidId);
            if (winners.Count < 1) return null;
            return winners[0];
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();

        }

    }
}


