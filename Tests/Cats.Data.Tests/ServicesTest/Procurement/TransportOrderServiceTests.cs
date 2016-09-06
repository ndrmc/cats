using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cats.Data.Repository;
using Cats.Data.UnitWork;
using Cats.Models.ViewModels;
using Cats.Services.Common;
using Moq;
using NUnit.Framework;
using Cats.Services.Procurement;
using Cats.Models;
namespace Cats.Data.Tests.ServicesTest.Procurement
{
    [TestFixture]
    public class TransportOrderServiceTests
    {
        #region SetUp / TearDown

        private IList<ReliefRequisition> _reliefRequisitions;
        private IList<TransportOrder> _transportOrders;
        private TransportOrderService _transportOrderService;
        private IList<BidWinner> _transportBidWinners;
        private IList<ApplicationSetting> _applicationSettings;
        private INotificationService _notificationService;
         [SetUp]
        public void Init()
        {
            var unitOfWorkNotify = new Mock<IUnitOfWork>();

            _transportBidWinners = new List<BidWinner>()
                                       {
                                           new BidWinner()
                                               {
                                                   SourceID= 1,
                                                   DestinationID= 1,
                                                   Tariff= 100,
                                                   TransporterID = 1,
                                                   BidID = 1
                                               }
                                       };
            
            _reliefRequisitions = new List<ReliefRequisition>()
                                         {

                                             new ReliefRequisition()
                                                 {
                                                     RegionID = 1,
                                                     ProgramID = 1,
                                                     CommodityID = 1,
                                                     ZoneID = 2,
                                                     RequisitionNo = "REQ-001",
                                                     Round = 1,
                                                     RegionalRequestID = 1,
                                                     RequisitionID = 1,
                                                     Status = 4,
                                                     ReliefRequisitionDetails=new List<ReliefRequisitionDetail>()
                                                                                  {
                                                                                     new ReliefRequisitionDetail
                                                                                         {
                                                                                             RequisitionID=1,
                                                                                             RequisitionDetailID=1,
                                                                                             FDPID=1,
                                                                                             DonorID=1,
                                                                                             CommodityID=1,
                                                                                             Amount=10,
                                                                                             BenficiaryNo=12,
                                                                                             ReliefRequisition = new ReliefRequisition()
                                                                                                      {   
                                                                        
                                                                                                             RequisitionID = 1,
            RegionID = 1,
                                                                                                             RegionalRequestID = 1,
                                                                                                             TransportRequisitionDetails = new List<TransportRequisitionDetail>()
                                                                                                                 {
                                                                                                                     new TransportRequisitionDetail()
                                                                                                                         {
                                                                                                                             RequisitionID = 1,
                                                                                                                             TransportRequisitionDetailID = 1,
                                                                                                                             TransportRequisitionID = 1
                                                                                                                            
                                                                                                                             
                                                                                                                         }
                                                                                                                         
                                                                                                                 }

                                                                                                        },
                                                                                             FDP=new FDP
                                                                                                     {
                                                                                                         Name="XYX",
                                                                                                         AdminUnitID=1,
                                                                                                         FDPID=1
                                                                                                     },
                                                                                                     
                                                                                            
                                                                                         }
                                                                                  }
                                                 }
                                         };
            var _tranportRequisitionDetail = new List<TransportRequisitionDetail>()
                                                   {
                                                       new TransportRequisitionDetail
                                                           {
                                                               RequisitionID=1,
                                                               TransportRequisitionID=1
                                                           }
                                                   };
            _transportOrders = new List<TransportOrder>()
                                   {
                                       new TransportOrder()
                                           {
                                               TransporterID=1,
                                               TransportOrderID=1,
                                               StatusID = 1,
                                               StartDate=DateTime.Today,
                                               EndDate=DateTime.Today,
                                               BidDocumentNo="xyz"
                                           }
                                   };

            var _transportRequisitons = new List<TransportRequisition>
                                            {
                                                new TransportRequisition
                                                    {
                                                        TransportRequisitionID = 1,
                                                        Status = 1,
                                                        TransportRequisitionNo = "001"
                                                    }
                                            };

            var _hubAllocation = new List<HubAllocation>
                                     {
                                         new HubAllocation
                                             {
                                                 RequisitionID = 1,
                                                 HubID = 1
                                             }
                                     };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockReliefRequisitionRepository = new Mock<IGenericRepository<ReliefRequisition>>();
            mockReliefRequisitionRepository.Setup(
                t => t.Get(It.IsAny<Expression<Func<ReliefRequisition, bool>>>(), It.IsAny<Func<IQueryable<ReliefRequisition>, IOrderedQueryable<ReliefRequisition>>>(), It.IsAny<string>())).Returns(
                    (Expression<Func<ReliefRequisition, bool>> perdicate, Func<IQueryable<ReliefRequisition>, IOrderedQueryable<ReliefRequisition>> obrderBy, string prop) =>
                    {
                        var
                            result = _reliefRequisitions.AsQueryable();
                        return result;
                    }
                );
            mockReliefRequisitionRepository.Setup(t => t.FindById(It.IsAny<int>())).Returns((int id) => _reliefRequisitions
                                                                                                          .ToList().
                                                                                                          Find
                                                                                                          (t =>
                                                                                                           t.
                                                                                                               RequisitionID ==
                                                                                                           id));

            var mockReliefRequisionDetailRepository = new Mock<IGenericRepository<ReliefRequisitionDetail>>();
            mockReliefRequisionDetailRepository.Setup(
                 t => t.Get(It.IsAny<Expression<Func<ReliefRequisitionDetail, bool>>>(), It.IsAny<Func<IQueryable<ReliefRequisitionDetail>, IOrderedQueryable<ReliefRequisitionDetail>>>(), It.IsAny<string>())).Returns(
                 (Expression<Func<ReliefRequisitionDetail, bool>> predicate, Func<IQueryable<ReliefRequisitionDetail>, IOrderedQueryable<ReliefRequisitionDetail>> filter, string str) =>
                 {
                     return _reliefRequisitions.First().ReliefRequisitionDetails.AsQueryable();

                 }
        );

            mockUnitOfWork.Setup(t => t.ReliefRequisitionDetailRepository).Returns(
                mockReliefRequisionDetailRepository.Object);

            var mockTransportBidWinnerDetailRepository = new Mock<IGenericRepository<BidWinner>>();
            mockTransportBidWinnerDetailRepository.Setup(t => t.Get(It.IsAny<Expression<Func<BidWinner, bool>>>(), It.IsAny<Func<IQueryable<BidWinner>, IOrderedQueryable<BidWinner>>>(), It.IsAny<string>())).Returns(_transportBidWinners.AsQueryable());

            mockUnitOfWork.Setup(t => t.BidWinnerRepository).Returns(
                mockTransportBidWinnerDetailRepository.Object);

            mockUnitOfWork.Setup(t => t.ReliefRequisitionRepository).Returns(mockReliefRequisitionRepository.Object);

            var hubAllocationRepository = new Mock<IGenericRepository<HubAllocation>>();
            hubAllocationRepository.Setup(t => t.FindBy(It.IsAny<Expression<Func<HubAllocation, bool>>>())).Returns(_hubAllocation);
            mockUnitOfWork.Setup(t => t.HubAllocationRepository).Returns(hubAllocationRepository.Object);
            var transportOrderRepository =
                new Mock<IGenericRepository<TransportOrder>>();
             transportOrderRepository.Setup(
                 t => t.Get(It.IsAny<Expression<Func<TransportOrder, bool>>>(), It.IsAny<Func<IQueryable<TransportOrder>, IOrderedQueryable<TransportOrder>>>(), It.IsAny<string>())).Returns(
                     _transportOrders.AsQueryable());
                       
            transportOrderRepository.Setup(t => t.Add(It.IsAny<TransportOrder>())).Returns(true);
            mockUnitOfWork.Setup(t => t.TransportOrderRepository).Returns(transportOrderRepository.Object);

             var transportOrderDetailRepository = new Mock<IGenericRepository<TransportOrderDetail>>();
             transportOrderDetailRepository.Setup(
                 t =>
                 t.Get(It.IsAny<Expression<Func<TransportOrderDetail, bool>>>(),
                       It.IsAny<Func<IQueryable<TransportOrderDetail>, IOrderedQueryable<TransportOrderDetail>>>(),
                       It.IsAny<string>())).Returns(new List<TransportOrderDetail>(){new TransportOrderDetail(){CommodityID=1,FdpID=1,DonorID = 1,RequisitionID = 1,QuantityQtl = 1,SourceWarehouseID=1,
                       ReliefRequisition=new ReliefRequisition
                                             {
                                                 ProgramID=1,
                                                 Round=1,
                                                 Month=1,
                                                 RequisitionID=1,
                                                 RequisitionNo= "1"
                                             }}});
             mockUnitOfWork.Setup(t => t.TransportOrderDetailRepository).Returns(transportOrderDetailRepository.Object);
           
            var transportRequisition = new Mock<IGenericRepository<TransportRequisition>>();

            transportRequisition.Setup(
                t =>
                t.Get(It.IsAny<Expression<Func<TransportRequisition, bool>>>(),
                      It.IsAny<Func<IQueryable<TransportRequisition>, IOrderedQueryable<TransportRequisition>>>(),
                      It.IsAny<string>())).Returns(_transportRequisitons);
            
            mockUnitOfWork.Setup(t => t.TransportRequisitionRepository).Returns(transportRequisition.Object);
            mockUnitOfWork.Setup(t => t.Save());

            var transportRequisitionDetailRepository = new Mock<IGenericRepository<TransportRequisitionDetail>>();
            transportRequisitionDetailRepository.Setup(t => t.Get(It.IsAny<Expression<Func<TransportRequisitionDetail, bool>>>(), It.IsAny<Func<IQueryable<TransportRequisitionDetail>, IOrderedQueryable<TransportRequisitionDetail>>>(), It.IsAny<string>())).Returns(
                (Expression<Func<TransportRequisitionDetail, bool>> predicate, Func<IQueryable<TransportRequisition>, IOrderedQueryable<TransportRequisition>> filter, string includeProp) =>
                {
                    return _tranportRequisitionDetail;
                });
            mockUnitOfWork.Setup(t => t.TransportRequisitionDetailRepository).Returns(
                transportRequisitionDetailRepository.Object);
             var applicationSettingRepository = new Mock<IGenericRepository<ApplicationSetting>>();
             applicationSettingRepository.Setup(t=>t.FindBy(It.IsAny<Expression<Func<ApplicationSetting, bool>>>())).Returns(new List<ApplicationSetting>()
                                                                                                                                 {
                                                                                                                                     new ApplicationSetting()
                                                                                                                                         {
                                                                                                                                             SettingID=1,
                                                                                                                                            SettingName="CurrentBid",
                                                                                                                                            SettingValue = "1"
                                                                                                                                         }
                                                                                                                                 });
             mockUnitOfWork.Setup(t => t.ApplicationSettingRepository).Returns(applicationSettingRepository.Object);
             ;
             var bidRepository = new Mock<IGenericRepository<Bid>>();
             bidRepository.Setup(t => t.FindById(It.IsAny<int>())).Returns(new Bid() {BidID = 1, BidNumber = "Bid-001"});
           

             mockUnitOfWork.Setup(t => t.BidRepository).Returns(bidRepository.Object);
             var transporterRepository = new Mock<IGenericRepository<Transporter>>();
             transporterRepository.Setup(t => t.FindById(It.IsAny<int>())).Returns(new Transporter()
                                                                                       {
                                                                                           TransporterID = 1,
                                                                                           Name = "TRANS"
                                                                                       });
             mockUnitOfWork.Setup(t => t.TransporterRepository).Returns(transporterRepository.Object);
             var dispatchAllocationRepository = new Mock<IGenericRepository<DispatchAllocation>>();
             dispatchAllocationRepository.Setup(t => t.Add(It.IsAny<DispatchAllocation>())).Returns(true);
             mockUnitOfWork.Setup(t => t.DispatchAllocationRepository).Returns(dispatchAllocationRepository.Object);

             var sipcAllocationRepository = new Mock<IGenericRepository<SIPCAllocation>>();
             sipcAllocationRepository.Setup(t => t.FindBy(It.IsAny<Expression<Func<SIPCAllocation, bool>>>())).Returns(new List<SIPCAllocation>());
             mockUnitOfWork.Setup(t => t.SIPCAllocationRepository).Returns(sipcAllocationRepository.Object);
             var transporterService = new Mock<ITransporterService>();
            transporterService.Setup(t => t.GetBidWinner(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new List<BidWinner>()
                                                                                                               {
                                                                                                                   new BidWinner(){
                                                                                                                   BidID=1,
                                                                                                                   DestinationID=1,
                                                                                                                   Position=1,
                                                                                                                   SourceID=1,
                                                                                                                   Tariff=1,
                                                                                                                   TransporterID=1
                                                                                                                   }
                                                                                                               });
            _notificationService = new NotificationService(unitOfWorkNotify.Object);
            _transportOrderService = new TransportOrderService(mockUnitOfWork.Object, transporterService.Object, _notificationService,null);
            //Act 
        }

        [TearDown]
        public void Dispose()
        {
            _transportOrderService.Dispose();
        }

        #endregion

        #region Tests

        [Test]
        public void CanGetAllTransportRequisions()
        {
            var assignedRequisitons = new List<TransportRequisition>()
                                       {
                                           new TransportRequisition()
                                               {
                                                   CertifiedBy= 1,
                                                   CertifiedDate= DateTime.Today,
                                                   Remark= "",
                                                   RequestedBy = 1,
                                                   RequestedDate = DateTime.Today,
                                                   Status = 1,
                                                   TransportRequisitionNo = "009",
                                                   TransportRequisitionID = 1
                                               }
                                       };


            //Act 

            //TODO:Please mock the transportrequisitionrepository and test here


            //Assert

            Assert.IsInstanceOf<List<TransportRequisition>>(assignedRequisitons);
            Assert.IsNotEmpty(assignedRequisitons);





        }


        [Test]

        public void ShouldCreateTransportOrders()
        {

            //Act 

           
            var result = _transportOrderService.CreateTransportOrder(1,1,"",1);

            //Assert

            Assert.IsTrue(result);
        }
        [Test]
        public  void ShouldGenerateDispatchAllocation()
        {
            var result = _transportOrderService.GeneratDispatchPlan(1);

            Assert.IsTrue(result);
        }
        //[Test]
        //public void Should_Raise_Error_If_NoTransporter()
        //{
        //  //Arrange
        //    _transportBidWinners = new List<TransportBidWinnerDetail>()
        //                               {
        //                                   new TransportBidWinnerDetail()
        //                                       {
        //                                           HubID=2,
        //                                           WoredaID=2,
        //                                           TariffPerQtl=100,
        //                                           TransporterID = 1
        //                                       }
        //                               };


        //    Assert.Throws<Exception>(Create_Transport_Orders);
        //}
        //private void Create_Transport_Orders()
        //{
        //    var requisitionToDispatch = _transportOrderService.GetRequisitionToDispatch().ToList();
        //   _transportOrderService.CreateTransportOrder(requisitionToDispatch);
        //}
        #endregion
    }
}
