﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cats.Services.Hub;
using Cats.Services.Hub.Interfaces;
using Kendo.Mvc.UI;
using NUnit.Framework;
using Moq;
using Cats.Areas.Logistics.Controllers;
using Cats.Models.Hubs;
using System.Linq.Expressions;
namespace Cats.Tests.ControllersTests
{
    [TestFixture]
   public class LogisticsStockStatusTests
    {
        

        private LogisticsStockStatusController _logisticsStockStatusController;
       
        [SetUp]
       public void Init()
        {
            var dispatch = new List<VWDispatchCommodity>()
                                                     {
                                                         new VWDispatchCommodity()
                                                             {
                                                                 DispatchedAmountInMT=20,
                                                                 DispatchedAmountInUnit=200,
                                                                 LedgerID=9,
                                                                 Amount=2000,
                                                                 RemainingInMT=1800,
                                                                 RemainingInUnit=1800,
                                                                 FDPID=12,
                                                                 Hub = "Adama",
                                                                 HubId = 1,
                                                                 ProjectCode ="4470/2562",
                                                                 ShippingInstruction = "00017512B",
                                                                 FDPName = "Dalol",
                                                                 AdminUnitTypeID=4,
                                                                 ParentID=13,
                                                                 IsClosed=false,
                                                                 Donor="UN - World Food Program",
                                                                 CommodityID=1,
                                                                 Commodity="Cerial",
                                                                 RequisitionNo="27977",
                                                                 BidRefNo="122/2004",
                                                                 Beneficiery = 14236,
                                                                 Unit=1,
                                                                 TransporterID=50,
                                                                 Name="PSNP",
                                                                 Round=1,
                                                                 Month=10,
                                                                 Year=2004,
                                                                 DonorID=1,                                                               
                                                                 ProgramID=2,
                                                                 ShippingInstructionID=93,
                                                                 ProjectCodeID=79,
                                                                 DispatchDate=new DateTime(19/07/2012),
                                                                 CreatedDate=new DateTime(19/07/2012),
                                                                 DispatchedByStoreMan = "Gizachew",
                                                                 GIN="0104026",
                                                                 Zone="Zone 2",
                                                                 Region="Afar",
                                                                 RegionId=2,
                                                                 ZoneId=13


                                                             }
                                                     };


            var woredas = new List<AdminUnit>()
                              {
                                  new AdminUnit()
                                      {
                                          Name = "Chefera",
                                          AdminUnitID = 56
                                      }
                              };
                                                                



       
           Mock<IStockStatusService> mockStatusService = new Mock<IStockStatusService>();
           Mock<IAdminUnitService> mockAdminUnitService = new Mock<IAdminUnitService>();


            mockStatusService.Setup(s => s.GetDispatchedCommodity(It.IsAny<Expression<Func<VWDispatchCommodity,bool>>>())).Returns(dispatch);
            mockAdminUnitService.Setup(a => a.GetAllAdminUnit()).Returns(woredas);



            _logisticsStockStatusController = new LogisticsStockStatusController(null, null, null, null, null,
                mockStatusService.Object, mockAdminUnitService.Object, null, null, null, null, null, null,null,null);



        }

        //[Test]
        //public void CanItGenerateDispatchedList()
        //{
        //    var request = new DataSourceRequest();
        //    ActionResult actual = _logisticsStockStatusController.DispatchCommodity();
        //    ViewResult result = actual as ViewResult;
        //    Assert.IsNotNull(result);
        //}
    }
}
