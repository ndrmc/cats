using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.Procurement
{
    public class TransportBidQuotationService : ITransportBidQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransportBidQuotationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool AddTransportBidQuotation(TransportBidQuotation item)
        {
            _unitOfWork.TransportBidQuotationRepository.Add(item);
            _unitOfWork.Save();
            return true;
        }


        public bool AddWoreda(TransportBidQuotation transportBidQuotation)
        {
            //var detail = _unitOfWork.TransportBidQuotationRepository.FindBy(
            //        m => m.TransportBidQuotationHeaderID == transportBidQuotation.TransportBidQuotationHeaderID
            //        && m.DestinationID == transportBidQuotation.DestinationID).FirstOrDefault();
            var bid =
                _unitOfWork.BidRepository.Get(b => b.BidID == transportBidQuotation.BidID).FirstOrDefault();
            var bidPlanDetail =
                _unitOfWork.TransportBidPlanDetailRepository.Get(t => t.BidPlanID == bid.TransportBidPlanID).FirstOrDefault();

            //if (detail == null)
            //{
                if (bidPlanDetail != null)
                {
                    var newBidPlanDetail = new TransportBidPlanDetail
                    {
                        BidPlanID = bidPlanDetail.BidPlanID,
                        DestinationID = transportBidQuotation.DestinationID,
                        ProgramID = bidPlanDetail.ProgramID,
                        Quantity = bidPlanDetail.Quantity,
                        SourceID = transportBidQuotation.SourceID
                    };
                    _unitOfWork.TransportBidPlanDetailRepository.Add(newBidPlanDetail);
                    _unitOfWork.TransportBidQuotationRepository.Add(transportBidQuotation);
                    _unitOfWork.Save();
                    return true;
                }

            //}
            return false;
        }

        public bool DeleteWoreda(int transportBidQuotationId)
        {
            try
            {
                var transportBidQuotation = _unitOfWork.TransportBidQuotationRepository.FindById(transportBidQuotationId);

                var bid = _unitOfWork.BidRepository.Get(b => b.BidID == transportBidQuotation.BidID).FirstOrDefault();

                var bidPlanDetail = _unitOfWork.TransportBidPlanDetailRepository.Get(t => t.BidPlanID == bid.TransportBidPlanID).FirstOrDefault();

                if (bidPlanDetail != null)
                    _unitOfWork.TransportBidPlanDetailRepository.Delete(bidPlanDetail);


                _unitOfWork.TransportBidQuotationRepository.Delete(transportBidQuotation);

                _unitOfWork.Save();
            }
            catch
            {

            }

            return true;
        }

        public bool UpdateTransportBidQuotation(TransportBidQuotation item)
        {
            if (item == null) return false;
            _unitOfWork.TransportBidQuotationRepository.Edit(item);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteTransportBidQuotation(TransportBidQuotation item)
        {
            if (item == null) return false;
            _unitOfWork.TransportBidQuotationRepository.Delete(item);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var item = _unitOfWork.TransportBidQuotationRepository.FindById(id);
            return DeleteTransportBidQuotation(item);
        }

        public TransportBidQuotation FindById(int id)
        {
            return _unitOfWork.TransportBidQuotationRepository.FindById(id);
        }

        public List<TransportBidQuotation> GetAllTransportBidQuotation()
        {
            return _unitOfWork.TransportBidQuotationRepository.GetAll();

        }

        public List<TransportBidQuotation> FindBy(Expression<Func<TransportBidQuotation, bool>> predicate)
        {
            return _unitOfWork.TransportBidQuotationRepository.FindBy(predicate);

        }

        public IEnumerable<TransportBidQuotation> Get(
            System.Linq.Expressions.Expression<Func<TransportBidQuotation, bool>> filter = null,
            Func<IQueryable<TransportBidQuotation>, IOrderedQueryable<TransportBidQuotation>> orderBy = null,
            string includeProperties = "")
        {
            return _unitOfWork.TransportBidQuotationRepository.Get(filter, orderBy, includeProperties);
        }

        private List<Cats.Models.Transporter> GetDrmfssTransporters()
        {
            return _unitOfWork.TransporterRepository.FindBy(t => t.OwnedByDRMFSS);

        }
        public List<TransportBidQuotation> GetSecondWinner(int transporterId, int woredaId, string bidDocumentNo)// BidId should also be used to filter the data
        {
            var secondTransporter = 2;
            const int firstTransporter = 1;
            var bidQoutation =
                _unitOfWork.TransportBidQuotationRepository.FindBy(
                    t => t.DestinationID == woredaId && t.TransporterID != transporterId && t.Bid.BidNumber == bidDocumentNo);
            var secondWinner = new List<TransportBidQuotation>();
            var winnerTransporter = new List<TransportBidQuotation>();
            var selectedTransportersId = new List<int>();
            if (bidQoutation.Count > 0)
            {
                if (bidQoutation.Count == 1)
                    secondTransporter = 1;

                //secondWinner = bidQoutation
                //    .GroupBy(g => g.TransportBidQuotationID)
                //    .OrderByDescending(o => o.Key)
                //    .Select(s => new { Qoutation = s.ToList() })
                //    .ToList()[secondTransporter - 1].Qoutation;
                var topwinners = bidQoutation.OrderBy(g => g.Tariff).ToList();


                int count = 0;
                foreach (var topwinner in topwinners)
                {
                    if (count == 5) break;
                    if (!selectedTransportersId.Contains(topwinner.TransporterID))
                    {
                        var transportBidQuotation = new TransportBidQuotation{TransporterID = topwinner.TransporterID};
                        var transporterHeaderInfo = new TransportBidQuotationHeader
                        {
                            TransporterId = topwinner.TransporterID,
                            Transporter = _unitOfWork.TransporterRepository.FindById(topwinner.TransporterID)
                        };
                        transportBidQuotation.TransportBidQuotationHeader = transporterHeaderInfo;
                        secondWinner.Add(transportBidQuotation);
                        selectedTransportersId.Add(topwinner.TransporterID);
                        count++;
                    }
                }

            }

            //DRMFSS
            var drmfssTransporters =
                GetDrmfssTransporters()
                    .Where(m => m.TransporterID != transporterId && !selectedTransportersId.Contains(m.TransporterID));
            foreach (var transporter in drmfssTransporters)
            {


                var drmfssTransporter = new TransportBidQuotation { TransporterID = transporter.TransporterID};
                var drmfssTransporterHeaderInfo = new TransportBidQuotationHeader()
                                                      {
                                                          TransporterId = transporter.TransporterID,
                                                          Transporter = _unitOfWork.TransporterRepository.FindById(transporter.TransporterID)
                                                      };

                drmfssTransporter.TransportBidQuotationHeader = drmfssTransporterHeaderInfo;
                secondWinner.Add(drmfssTransporter);
            }



            return secondWinner;

        }



        //public
    }
}