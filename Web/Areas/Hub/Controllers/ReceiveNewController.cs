using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cats.Areas.Hub.Models;
using Cats.Models.Constant;
using Cats.Models.Hubs;
using Cats.Models.Hubs.ViewModels;
using Cats.Services.Common;
using Cats.Services.Hub;
using Cats.Web.Hub;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CommoditySource = Cats.Models.Hubs.CommoditySource;


namespace Cats.Areas.Hub.Controllers
{
    public class ReceiveNewController : BaseController
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IReceiptAllocationService _receiptAllocationService;
        private readonly IReceiveService _receiveService;
        private readonly IReceiveDetailService _receiveDetailService;
        private readonly ICommodityService _commodityService;
        private readonly IUnitService _unitService;
        private readonly IStoreService _storeService;
        private readonly ITransactionService _transactionService;
        private readonly IDonorService _donorService;
        private readonly IHubService _hub;
        private readonly ITransporterService _transporterService;
        private readonly IShippingInstructionService _shippingInstructionService;
        private Guid _receiptAllocationId;
        private readonly IHubBusinessProcessService _businessProcessService;
        private readonly IHubStateTemplateService _stateTemplateService;
        private readonly IApplicationSettingService _applicationSettingService;

        public ReceiveNewController(IUserProfileService userProfileService,
            IReceiptAllocationService receiptAllocationService,
            IReceiveService receiveService,
            ICommodityService commodityService,
            IUnitService unitService,
            IStoreService storeService,
            ITransactionService transactionService,
            IDonorService donorService,
            IHubService hub,
            ITransporterService transporterService,
            IShippingInstructionService shippingInstructionService,
            IReceiveDetailService receiveDetailService,
            IHubBusinessProcessService businessProcessService,
            IHubStateTemplateService stateTemplateService,
            IApplicationSettingService applicationSettingService)
            : base(userProfileService)
        {
            _userProfileService = userProfileService;
            _receiptAllocationService = receiptAllocationService;
            _receiveService = receiveService;
            _commodityService = commodityService;
            _unitService = unitService;
            _storeService = storeService;
            _transactionService = transactionService;
            _donorService = donorService;
            _hub = hub;
            _transporterService = transporterService;
            _shippingInstructionService = shippingInstructionService;
            _receiveDetailService = receiveDetailService;
            _businessProcessService = businessProcessService;
            _stateTemplateService = stateTemplateService;
            _applicationSettingService = applicationSettingService;
        }

        #region Action

        public ReceiveNewViewModel ModeltoNewView(Receive receive) //string receiveId, string grn)
        {

            var receiptAllocation = _receiptAllocationService.FindById(receive.ReceiptAllocationID.GetValueOrDefault());

            var user = _userProfileService.GetUser(User.Identity.Name);

            var viewModel = _receiveService.ReceiptAllocationToReceive(receiptAllocation);
        
            viewModel.CurrentHub = user.DefaultHub.Value;
            viewModel.UserProfileId = user.UserProfileID;
            var hubOwner = _hub.FindById(user.DefaultHub.Value);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(receive.ReceiptAllocationID.GetValueOrDefault());
            //var commodities = _commodityService.GetAllCommodityViewModelsByParent(receiptAllocation.CommodityID);
            //ViewData["commodities"] = commodities;
            //ViewData["units"] = _unitService.GetAllUnitViewModels();

            viewModel.CreatedDate = receive.CreatedDate;
            viewModel.Grn = receive.GRN;
            viewModel.ReceiptDate = receive.ReceiptDate;
            viewModel.SiNumber = receiptAllocation.SINumber;
            viewModel.ReceiptDate = viewModel.ReceiptDate;
            viewModel.ReceiptAllocationId = receive.ReceiptAllocationID.GetValueOrDefault();
            viewModel.ReceiveId = receive.ReceiveID;

            //viewModel.StackNumber
            viewModel.WayBillNo = receive.WayBillNo;
            viewModel.SiNumber = receiptAllocation.SINumber;
            viewModel.ProjectCode = receiptAllocation.ProjectNumber;
            //viewModel.Program = .FindById(receiptAllocation.ProgramID).Name;
            viewModel.ProgramId = receiptAllocation.ProgramID;
            //viewModel.CommodityType = _CommodityTypeRepository.FindById(receiptAllocation.Commodity.CommodityTypeID).Name,
            viewModel.CommodityTypeId = receiptAllocation.Commodity.CommodityTypeID;
            viewModel.CommoditySourceTypeId = receiptAllocation.CommoditySourceID;


            if (CommoditySource.Constants.LOAN == receiptAllocation.CommoditySourceID
                || CommoditySource.Constants.SWAP == receiptAllocation.CommoditySourceID
                || CommoditySource.Constants.TRANSFER == receiptAllocation.CommoditySourceID
                || CommoditySource.Constants.REPAYMENT == receiptAllocation.CommoditySourceID)
            {
                if (receiptAllocation.SourceHubID.HasValue)
                {

                    viewModel.SourceHub = _hub.FindById(receiptAllocation.SourceHubID.GetValueOrDefault(0)).Name;
                }
            }

            if (CommoditySource.Constants.LOCALPURCHASE == receiptAllocation.CommoditySourceID)
            {
                viewModel.SupplierName = receiptAllocation.SupplierName;
                viewModel.PurchaseOrder = receiptAllocation.PurchaseOrder;
            }


            if (String.IsNullOrEmpty(viewModel.SupplierName))
                viewModel.SupplierName = receive.SupplierName;


            if (String.IsNullOrEmpty(viewModel.PurchaseOrder))
                viewModel.PurchaseOrder = receive.PurchaseOrder;

            viewModel.CommoditySource = receiptAllocation.CommoditySource.Name;
            viewModel.CommoditySourceTypeId = receiptAllocation.CommoditySourceID;
            viewModel.ReceivedByStoreMan = receive.ReceivedByStoreMan;
            ReceiveDetail receivedetail = receive.ReceiveDetails.FirstOrDefault();
            viewModel.StoreId = receive.StoreId.GetValueOrDefault();
            viewModel.StackNumber = receive.StackNumber.GetValueOrDefault();
            if (receivedetail != null)
                viewModel.ReceiveDetailNewViewModel = new ReceiveDetailNewViewModel
                {
                    CommodityId = receivedetail.CommodityID,
                    CommodityChildID = receivedetail.CommodityChildID,
                    ReceivedQuantityInMt =
                        receivedetail.QuantityInMT,
                    ReceivedQuantityInUnit =
                        receivedetail.QuantityInUnit,
                    SentQuantityInMt = receivedetail.SentQuantityInMT,
                    SentQuantityInUnit = receivedetail.SentQuantityInUnit,
                    UnitId = receivedetail.UnitID,
                    Description = receivedetail.Description??string.Empty,
                    ReceiveId = receivedetail.ReceiveID,
                    ReceiveDetailId = receivedetail.ReceiveDetailID,

                };
            viewModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel>();
            foreach (var receivedetai in receive.ReceiveDetails)
            {
            var    ReceiveDetailNewViewModl = new ReceiveDetailsViewModel()
            {
                CommodityId = receivedetai.CommodityID,
                CommodityChildID = receivedetai.CommodityChildID.GetValueOrDefault(),
                ReceivedQuantityInMt =
                                                             receivedetai.QuantityInMT,
                ReceivedQuantityInUnit =
                                                             receivedetai.QuantityInUnit,
                SentQuantityInMt = receivedetai.SentQuantityInMT,
                SentQuantityInUnit = receivedetai.SentQuantityInUnit,
                UnitId = receivedetai.UnitID,
                Description = receivedetai.Description??string.Empty,
                ReceiveDetailsId = receivedetai.ReceiveDetailID,
                

            };

                viewModel.ReceiveDetailsViewModels.Add(ReceiveDetailNewViewModl);
            }
            viewModel.WeightBridgeTicketNumber = receive.WeightBridgeTicketNumber;
            viewModel.WeightBeforeUnloading = receive.WeightBeforeUnloading;
            viewModel.WeightAfterUnloading = receive.WeightAfterUnloading;
            viewModel.TransporterId = receive.TransporterID;
            viewModel.DriverName = receive.DriverName;
            viewModel.PlateNoPrime = receive.PlateNo_Prime;
            viewModel.PlateNoTrailer = receive.PlateNo_Trailer;
            viewModel.PortName = receive.PortName;
            viewModel.Remark = receive.Remark;
            viewModel.ResponsibleDonorId = receive.ResponsibleDonorID;
            viewModel.SourceDonorId = receive.SourceDonorID;

            return viewModel;

        }


        public ActionResult Create(string receiptAllocationId, string grn)
        {
            ViewBag.isEditMode = false;
            var commodities =
               _commodityService.GetAllCommodity()
                   .Where(l => l.ParentID == null)
                   .Where(l => l.CommodityTypeID == 1)
                   .Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name })
                   .ToList();
            var commodity = new CommodityModel() { Id = 0, Name = string.Empty };
            commodities.Insert(0, commodity);
            ViewBag.Commodities = commodities;
            ViewBag.SubCommodities =
                _commodityService.GetAllSubCommodities()
                    .Where(l => l.ParentID != null)
                    .Where(l => l.CommodityTypeID == 1)
                    .Select(c => new SubCommodity() {Id = c.CommodityID, Name = c.Name})
                    .ToList();
            var units =
                _unitService.GetAllUnit().Select(u => new UnitModel() {Id = u.UnitID, Name = u.Name}).ToList();
            var unit = new UnitModel {Id = 0, Name = string.Empty};
            units.Insert(0,unit);
            ViewBag.Units = units;
            if (grn != null)
            {           
                ViewBag.isEditMode = true;
                return View(ModeltoNewView(_receiveService.FindById(Guid.Parse(receiptAllocationId))));
            }

            if (String.IsNullOrEmpty(receiptAllocationId)) return View();
            _receiptAllocationId = Guid.Parse(receiptAllocationId);

            var receiptAllocation = _receiptAllocationService.FindById(_receiptAllocationId);

            var user = _userProfileService.GetUser(User.Identity.Name);

            if (receiptAllocation == null ||
                (user.DefaultHub == null || receiptAllocation.HubID != user.DefaultHub.Value)) return View();



            var viewModel = _receiveService.ReceiptAllocationToReceive(receiptAllocation);
            viewModel.CurrentHub = user.DefaultHub.Value;
            viewModel.UserProfileId = user.UserProfileID;
            var hubOwner = _hub.FindById(user.DefaultHub.Value);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            //var commodities = _commodityService.GetAllCommodityViewModelsByParent(receiptAllocation.CommodityID);
            //ViewData["commodities"] = commodities;
            //ViewData["units"] = _unitService.GetAllUnitViewModels();
            //viewModel.ReceiveDetailNewViewModel.CommodityId = receiptAllocation.CommodityID;

            //since the commodity that comes from allocation is the child look for the parent for saving later.
            var parentCommodityId =
                _commodityService.FindById(receiptAllocation.CommodityID).ParentID ??
                receiptAllocation.CommodityID;
            
            viewModel.ReceiveDetailNewViewModel = new ReceiveDetailNewViewModel
            {
                CommodityId = parentCommodityId,
                CommodityChildID = receiptAllocation.CommodityID,
                //UnitId=receiptAllocation.UnitID.GetValueOrDefault(),
            };

            var listViewModels = new List<ReceiveDetailsViewModel>();
            if (viewModel.ReceiveDetailNewViewModel != null)
                listViewModels.Add(new ReceiveDetailsViewModel()
                {
                    CommodityChildID = Convert.ToInt32(viewModel.ReceiveDetailNewViewModel.CommodityChildID),
                    CommodityId = viewModel.ReceiveDetailNewViewModel.CommodityId,
                    Description = viewModel.ReceiveDetailNewViewModel.Description ?? string.Empty ,
                    ReceiveDetailsId = viewModel.ReceiveDetailNewViewModel.ReceiveDetailId,
                    ReceivedQuantityInMt = viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInMt,
                    SentQuantityInMt = viewModel.ReceiveDetailNewViewModel.SentQuantityInMt,
                    ReceivedQuantityInUnit = viewModel.ReceiveDetailNewViewModel.ReceivedQuantityInUnit,
                    SentQuantityInUnit = viewModel.ReceiveDetailNewViewModel.SentQuantityInUnit
                });
            viewModel.ReceiveDetailsViewModels = listViewModels;
            return View(viewModel);
        }

        public ActionResult Commodities(Guid? receiptAllocationId, string grn, Guid? receiveId)
        {
            ViewBag.receiveId = receiveId;
            //ViewBag.Commodities = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            //ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            var commodityModelList = new List<CommodityModel>
            {
                new CommodityModel() { Id = -1, Name = ""}
            };
            var commodityModels = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            commodityModelList.AddRange(commodityModels);

            var subCommodityModelList = new List<SubCommodity>
            {
                new SubCommodity() { Id = -1, CommodityId = -1, Name = ""}
            };
            var subCommodityModels =
                _commodityService.GetAllSubCommodities()
                    .Where(l => l.ParentID != null)
                    .Where(l => l.CommodityTypeID == 1)
                    .Select(c => new SubCommodity() {Id = c.CommodityID, CommodityId = c.ParentID ?? 0, Name = c.Name})
                    .ToList();
            subCommodityModelList.AddRange(subCommodityModels);

            var unitModelList = new List<UnitModel>
            {
                new UnitModel() { Id = -1, Name = ""}
            };
            var unitModels = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
            unitModelList.AddRange(unitModels);

            ViewBag.Commodities = commodityModelList;
            ViewBag.SubCommodities = subCommodityModelList;
            ViewBag.Units = unitModelList;
            ViewBag.SI =
                _shippingInstructionService.GetAllShippingInstruction()
                    .Select(
                        s =>
                            new Cats.Models.Hubs.ViewModels.ShippingInstructionModel()
                            {
                                Id = s.ShippingInstructionID,
                                Value = s.Value
                            })
                    .ToList();

            return View("Commodities");
        }

        public JsonResult GetCascadingData(int CommodityId)
        {
            var subCommodityModelList = new List<SubCommodity>
            {
                new SubCommodity() { Id = -1, CommodityId = -1, Name = ""}
            };

            var subCommodityModels = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null)
                .Where(l => l.CommodityTypeID == 1 && l.ParentID == CommodityId).Select(c => new SubCommodity()
                { Id = c.CommodityID, CommodityId = c.ParentID ?? 0, Name = c.Name }).ToList();

            subCommodityModelList.AddRange(subCommodityModels);

            return Json(subCommodityModelList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadCommoditiesFromReceive([DataSourceRequest] DataSourceRequest request, Guid? receiveId)
        {
            ViewBag.Commodities = _commodityService.GetAllCommodity().Where(l => l.ParentID == null).Where(l => l.CommodityTypeID == 1).Select(c => new CommodityModel() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
            ViewBag.SI = _shippingInstructionService.GetAllShippingInstruction().Select(s => new Cats.Models.Hubs.ViewModels.ShippingInstructionModel() { Id = s.ShippingInstructionID, Value = s.Value }).ToList();

            if (receiveId != null)
            {
                var receiveDetailsViewModels =
                    (from receives in _receiveService.FindById((Guid)receiveId).ReceiveDetails
                     where receives.TransactionGroup !=null && receives.TransactionGroup.Transactions !=null
                     let transaction =
                         receives.TransactionGroup.Transactions.FirstOrDefault(
                             p => p.QuantityInMT > 0 || p.QuantityInUnit > 0)
                     where transaction != null
                     let amount = transaction.QuantityInMT
                     select new ReceiveDetailsViewModel()
                     {

                         CommodityId = receives.CommodityID,
                         Description = receives.Description,
                         SentQuantityInMt = receives.SentQuantityInMT,
                         SentQuantityInUnit = receives.SentQuantityInUnit,
                         ReceivedQuantityInMt = transaction.QuantityInMT,
                         ReceivedQuantityInUnit = transaction.QuantityInUnit,
                         SiNumber = transaction.ShippingInstructionID,
                         CommodityChildID = receives.CommodityChildID ?? 0,
                         UnitId = receives.UnitID,
                         ReceiveDetailsId = receives.ReceiveDetailID,
                         ReceiveDetailsIdString = receives.ReceiveDetailID.ToString()
                     }).ToList();
               
                return Json(receiveDetailsViewModels.ToDataSourceResult(request));
            }
            else
            {
                return Json(new List<ReceiveDetailsViewModel>().ToDataSourceResult(request));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCommoditiesFromReceive([DataSourceRequest] DataSourceRequest request, ReceiveDetailsViewModel receiveDetailsViewModels, Guid receiveId)
        {
            var results = new List<ReceiveDetailsViewModel>();
            if (ModelState.IsValid)
            {
                var receiveModel = ModeltoNewView(_receiveService.FindById(receiveId));
                ReceiveDetailsViewModel rdvm;
                if (!receiveDetailsViewModels.ReceiveDetailsId.HasValue)
                {
                    rdvm = new ReceiveDetailsViewModel();
                    rdvm = receiveDetailsViewModels;
                    
                    rdvm.ReceiveDetailsId = Guid.NewGuid();
                    receiveModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel> { rdvm };
                    _transactionService.ReceiptDetailsTransaction(receiveModel);

                    results.Add(receiveDetailsViewModels);
                }
                else
                {
                    rdvm = new ReceiveDetailsViewModel();
                    rdvm = receiveDetailsViewModels;
                    
                    receiveModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel> { rdvm };
                    _transactionService.ReceiptDetailsTransaction(receiveModel, false, true);

                    results.Add(receiveDetailsViewModels);
                }

            }

         

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        public bool CreateCommoditiesFromReceive(List<ReceiveDetailsViewModel> receiveDetailsViewModels,
            Guid receiveId,ReceiveNewViewModel receiveNewViewModel)
        {
            var result = false;
            var recieve = _receiveService.FindById(receiveId);
            if (recieve == null) return false;
            var receiveModel = ModeltoNewView(recieve);


            receiveModel.Grn = receiveNewViewModel.Grn;
            receiveModel.CommodityTypeId = receiveNewViewModel.CommodityTypeId;
            receiveModel.SourceDonorId = receiveNewViewModel.SourceDonorId;
            receiveModel.ResponsibleDonorId = receiveNewViewModel.ResponsibleDonorId;
            receiveModel.TransporterId = receiveNewViewModel.TransporterId > 0 ? receiveNewViewModel.TransporterId : 1;
            receiveModel.PlateNoPrime = receiveNewViewModel.PlateNoPrime;
            receiveModel.PlateNoTrailer = receiveNewViewModel.PlateNoTrailer;
            receiveModel.DriverName = receiveNewViewModel.DriverName;
            receiveModel.WeightBridgeTicketNumber = receiveNewViewModel.WeightBridgeTicketNumber;
            receiveModel.WeightBeforeUnloading = receiveNewViewModel.WeightBeforeUnloading;
            receiveModel.WeightAfterUnloading = receiveNewViewModel.WeightAfterUnloading;
            receiveModel.VesselName = receiveNewViewModel.VesselName;
            receiveModel.PortName = receiveNewViewModel.PortName;
            receiveModel.ReceiptDate = receiveNewViewModel.ReceiptDate;
            receiveModel.CreatedDate = DateTime.Now;
            receiveModel.WayBillNo = receiveNewViewModel.WayBillNo;
            receiveModel.CommoditySourceTypeId = receiveNewViewModel.CommoditySourceTypeId;
            receiveModel.ReceivedByStoreMan = receiveNewViewModel.ReceivedByStoreMan;
            receiveModel.PurchaseOrder = receiveNewViewModel.PurchaseOrder;
            receiveModel.SupplierName = receiveNewViewModel.SupplierName;
            receiveModel.Remark = receiveNewViewModel.Remark;
            receiveModel.ReceiptAllocationId = receiveNewViewModel.ReceiptAllocationId;
            receiveModel.CurrentHub = receiveNewViewModel.CurrentHub;
            receiveModel.UserProfileId = receiveNewViewModel.UserProfileId;
            receiveModel.StoreId = receiveNewViewModel.StoreId;
            receiveModel.StackNumber = receiveNewViewModel.StackNumber;
            receiveModel.SourceDonorId = receiveNewViewModel.SourceDonorId;
            receiveModel.ResponsibleDonorId = receiveNewViewModel.ResponsibleDonorId;


            foreach (var receiveDetailViewModel in receiveDetailsViewModels)
            {
                ReceiveDetailsViewModel rdvm;
                if (receiveDetailViewModel.ReceiveDetailsId == Guid.Empty|| receiveDetailViewModel.ReceiveDetailsId== null)
                {
                    rdvm = receiveDetailViewModel;
                    rdvm.ReceiveDetailsId = Guid.NewGuid();
                    receiveModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel> {rdvm};
                    result = _transactionService.ReceiptDetailsTransaction(receiveModel);
                }
                else
                {
                    rdvm = receiveDetailViewModel;
                    receiveModel.ReceiveDetailsViewModels = new List<ReceiveDetailsViewModel> {rdvm};
                    result = _transactionService.ReceiptDetailsTransaction(receiveModel, false, true);
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult Create(ReceiveNewViewModel viewModel)
        {
            var commodities =
                _commodityService.GetAllCommodity()
                    .Where(l => l.ParentID == null)
                    .Where(l => l.CommodityTypeID == 1)
                    .Select(c => new CommodityModel() {Id = c.CommodityID, Name = c.Name})
                    .ToList();
            var commodity = new CommodityModel() {Id = 0, Name = string.Empty};
            commodities.Insert(0,commodity);
            ViewBag.Commodities = commodities;
            ViewBag.SubCommodities = _commodityService.GetAllSubCommodities().Where(l => l.ParentID != null).Where(l => l.CommodityTypeID == 1).Select(c => new SubCommodity() { Id = c.CommodityID, Name = c.Name }).ToList();
            ViewBag.Units = _unitService.GetAllUnit().Select(u => new UnitModel() { Id = u.UnitID, Name = u.Name }).ToList();
            //Todo: change to support multiple receive detail 
            ViewBag.isEditMode = false;
            if (!string.IsNullOrEmpty(viewModel.Grn))
            {
                ViewBag.isEditMode = true;
            }
            var user = _userProfileService.GetUser(User.Identity.Name);
            var hubOwner = _hub.FindById(user.DefaultHub.Value);

            if (viewModel.ReceiveId != Guid.Empty)
            {
                _receiptAllocationId =
                    _receiveService.FindById(viewModel.ReceiveId).ReceiptAllocationID.GetValueOrDefault();
                viewModel.ReceiptAllocationId = _receiptAllocationId;
            }
            else
            {

                _receiptAllocationId = viewModel.ReceiptAllocationId;
            }

            #region Fix to ModelState

            switch (viewModel.CommoditySourceTypeId)
            {
                case CommoditySource.Constants.DONATION:
                    ModelState.Remove("SourceHub");
                    ModelState.Remove("SupplierName");
                    ModelState.Remove("PurchaseOrder");
                    break;
                case CommoditySource.Constants.LOCALPURCHASE:
                    ModelState.Remove("SourceHub");
                    break;
                default:
                    ModelState.Remove("DonorID");
                    ModelState.Remove("ResponsibleDonorID");
                    ModelState.Remove("SupplierName");
                    ModelState.Remove("PurchaseOrder");
                    break;
            }

            #endregion

            //if (!ModelState.IsValid)
            //{
            //    viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            //    viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            //    return View(viewModel);
            //}

            var recieveUpdated = false;
            //check if the detail are not null 
            if (viewModel.ReceiveDetailsViewModels.Any())
            {
                var firstOrdefault = viewModel.ReceiveDetailsViewModels.FirstOrDefault();
                var viewNewModel =
                    viewModel.ReceiveDetailsViewModels.FirstOrDefault(
                        f => f.ReceiveDetailsId == viewModel.ReceiveDetailNewViewModel.ReceiveDetailId);
                if (viewNewModel != null)
                    firstOrdefault = viewNewModel;         
                if (firstOrdefault != null && firstOrdefault.ReceiveDetailsId == null)
                    firstOrdefault.ReceiveDetailsId = Guid.NewGuid();
                //viewModel.ReceiveDetailNewViewModel = new ReceiveDetailNewViewModel
                //{                  
                //    CommodityId = firstOrdefault.CommodityId,
                //    CommodityChildID = firstOrdefault.CommodityChildID,
                //    Description = firstOrdefault.Description,
                //    ReceiveDetailId =  (Guid) firstOrdefault.ReceiveDetailsId,
                //    ReceivedQuantityInMt = firstOrdefault.ReceivedQuantityInMt,
                //    ReceivedQuantityInUnit = firstOrdefault.ReceivedQuantityInUnit,
                //    SentQuantityInMt = firstOrdefault.SentQuantityInMt,
                //    SentQuantityInUnit = firstOrdefault.SentQuantityInUnit,
                //    UnitId = firstOrdefault.UnitId
                //};

                //if (viewModel.ReceiveDetailNewViewModel.ReceiveDetailId == Guid.Empty)
                //    firstOrdefault.ReceiveDetailsId =
                //        viewModel.ReceiveDetailNewViewModel.ReceiveDetailId = Guid.NewGuid();

                #region GRN validation

                if (!_receiveService.IsGrnUnique(viewModel.Grn))
                {
                    if (viewModel.ReceiveId == Guid.Empty || _receiveService.FindById(viewModel.ReceiveId).GRN != viewModel.Grn)
                    {
                        ModelState.AddModelError("GRN", @"GRN already existed");
                        viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                        viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                        return View(viewModel);
                    }
                }

                #endregion

                #region Validate receive amount

                //if (_receiveService.IsReceiveExcedeAllocation(viewModel.ReceiveDetailNewViewModel,
                //    viewModel.ReceiptAllocationId))
                //{
                //    viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                //    ModelState.AddModelError("ReceiveId", "you are trying to receive more than allocated");
                //    viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                //    return View(viewModel);
                //}

                #endregion

                #region Validate Receive Amount not excide Sent one 

                if (_receiveService.IsReceiveGreaterThanSent(viewModel.ReceiveDetailNewViewModel))
                {
                    viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
                    ModelState.AddModelError("ReceiveId", "You can't receive more than sent item");
                    viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
                    return View(viewModel);
                }

                #endregion

                //List<ReceiveDetailsViewModel> receiveDetailsViewModels = GetReceiveDetailsViewModels(viewModel);
                //viewModel.ReceiveDetailsViewModels = receiveDetailsViewModels;


                if (viewModel.ReceiveId != Guid.Empty)
                {
                    //reverse the transaction
                    Receive prevmodel = _receiveService.FindById((viewModel.ReceiveId));
                    recieveUpdated = _transactionService.ReceiptTransaction(ModeltoNewView(prevmodel), true);

                    if (recieveUpdated)
                    {
                        BusinessProcess bp = _businessProcessService.FindById(prevmodel.BusinessProcessID);
                        BusinessProcessState bps = bp.CurrentState;

                        var stateTemplate = _stateTemplateService.FindBy(p => p.Name == ConventionalAction.Edited &&
                                                                              p.ParentProcessTemplateID ==
                                                                              bps.BaseStateTemplate
                                                                                  .ParentProcessTemplateID)
                            .FirstOrDefault();

                        if (stateTemplate != null)
                        {
                            var businessProcessState = new BusinessProcessState()
                            {
                                StateID = stateTemplate.StateTemplateID, // mark as edited
                                PerformedBy = HttpContext.User.Identity.Name,
                                DatePerformed = DateTime.Now,
                                Comment = "Receive hub is edited, a system internally captured data.",
                                ParentBusinessProcessID = bps.ParentBusinessProcessID
                            };

                            _businessProcessService.PromotWorkflow(businessProcessState);
                        }
                    }
                }
                else
                {
                    viewModel.ReceiveId = Guid.NewGuid();

                    int BP_PR = _applicationSettingService.getReceiveHubWorkflow();

                    if (BP_PR != 0)
                    {
                        BusinessProcessState createdstate = new BusinessProcessState
                        {
                            DatePerformed = DateTime.Now,
                            PerformedBy = User.Identity.Name,
                            Comment = "Receive Hub workflow is created."
                        };

                        BusinessProcess bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0, "ReceiveHubWorkflow", createdstate);

                        if (bp != null)
                        {
                            viewModel.BusinessProcessID = bp.BusinessProcessID;
                        }
                    }
                }

                recieveUpdated =_transactionService.ReceiptTransaction(viewModel);
                if (recieveUpdated && viewModel.ReceiveDetailsViewModels.Any())
                {
                    CreateCommoditiesFromReceive(viewModel.ReceiveDetailsViewModels, viewModel.ReceiveId,viewModel);
                }

                //var receiveID =
                //    _receiveService.GetAllReceive()
                //        .Where(
                //            r =>
                //                r.ReceiptAllocationID == viewModel.ReceiptAllocationId &&
                //                r.WayBillNo == viewModel.WayBillNo && r.GRN == viewModel.Grn)
                //        .Select(r => r.ReceiveID)
                //        .LastOrDefault();
                //return RedirectToAction("Commodities", "ReceiveNew",
                //    new
                //    {
                //        @receiptAllocationId = viewModel.ReceiptAllocationId,
                //        @grn = viewModel.Grn,
                //        @receiveId = receiveID
                //    });
                //return RedirectToAction("Index", "Receive");
            }
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");
            ModelState.AddModelError("ReceiveDetails", "Please add at least one commodity");
            viewModel.AllocationStatusViewModel = _receiveService.GetAllocationStatus(_receiptAllocationId);
            viewModel.IsTransporterDetailVisible = !hubOwner.HubOwner.Name.Contains("WFP");

            //return Create(viewModel.ReceiptAllocationId.ToString(),viewModel.Grn);

            return RedirectToAction("Index", "Receive");
        }

        private List<ReceiveDetailsViewModel> GetReceiveDetailsViewModels(ReceiveNewViewModel viewModel)
        {
            List<ReceiveDetailsViewModel> result = new List<ReceiveDetailsViewModel>();
            if (viewModel.ReceiveId == Guid.NewGuid())
                return result;
            var receiveDetails = _receiveService.FindById((Guid)viewModel.ReceiveId);
            if (receiveDetails == null) return result;
            return (from receives in receiveDetails.ReceiveDetails
                where receives.TransactionGroup!=null && receives.TransactionGroup.Transactions!=null
                let transaction =
                    receives.TransactionGroup.Transactions.FirstOrDefault(
                        p => p.QuantityInMT > 0 || p.QuantityInUnit > 0)
                where transaction != null
                let amount = transaction.QuantityInMT
                select new ReceiveDetailsViewModel()
                {

                    CommodityId = receives.CommodityID,
                    CommodityName = receives.CommodityName,
                    Description = receives.Description,
                    SentQuantityInMt = receives.SentQuantityInMT,
                    SentQuantityInUnit = receives.SentQuantityInUnit,
                    ReceivedQuantityInMt = transaction.QuantityInMT,
                    ReceivedQuantityInUnit = transaction.QuantityInUnit,
                    SiNumber = transaction.ShippingInstructionID,
                    CommodityChildID = receives.CommodityChildID ?? 0,
                    UnitId = receives.UnitID,
                    ReceiveDetailsId = receives.ReceiveDetailID,
                    ReceiveDetailsIdString = receives.ReceiveDetailID.ToString()
                }).ToList();
        }

        public JsonResult AllocationStatus(string receiptAllocationId)
        {
            _receiptAllocationId = Guid.Parse(receiptAllocationId);
            return Json(_receiveService.GetAllocationStatus(_receiptAllocationId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReceiveDetails_Create([DataSourceRequest] DataSourceRequest request, ReceiveDetailsViewModel receiveDetailsViewModel, Guid receiveId)
        {
            if (receiveDetailsViewModel != null && ModelState.IsValid)
            {
                //SessionProductRepository.Insert(receiveDetailsViewModel);

            }

            return Json(new[] { receiveDetailsViewModel }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Combobox

        public JsonResult GetUnities()
        {
            return Json(_unitService.GetAllUnitViewModels(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCommodities(string receiptAllocationId)
        {
            _receiptAllocationId = Guid.Parse(receiptAllocationId);

            var receiptAllocation = _receiptAllocationService.FindById(_receiptAllocationId);

            return Json(_commodityService.GetAllCommodityViewModelsByParent(receiptAllocation.CommodityID),
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStroes(int hubId)
        {
            return Json((from c in _storeService.GetStoreByHub(hubId)
                         select new StoreViewModel
                         {
                             StoreId = c.StoreID,
                             Name = c.Name
                         }), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGRNList(string siNo)
        {
            var GRNs = new List<GRNViewModel>();
            GRNs.AddRange(_receiveService.FindBy(f => f.ReceiptAllocation.IsFalseGRN && f.ReceiptAllocation.SINumber == siNo).Select(s => new GRNViewModel
			{
                Name = s.GRN,
                Id = s.ReceiveID
            }));


            return Json(GRNs, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetStacks(int? storeId)
        {
            if (storeId == null)
                return Json(new SelectList(Enumerable.Empty<StackViewModel>()), JsonRequestBehavior.AllowGet);
            var store = _storeService.FindById(storeId.Value);
            var stacks = new List<StackViewModel>();
            stacks.AddRange(store.Stacks.Select(i => new StackViewModel { Name = i.ToString(), Id = i }));
            return Json(stacks, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetResponsibleDonor()
        {
            return Json(from c in _donorService.GetAllDonor()
                            .Where(p => p.IsResponsibleDonor == true)
                            .DefaultIfEmpty()
                            .OrderBy(p => p.Name)
                        select new DonorViewModel
                        {
                            DonorId = c.DonorID,
                            Name = c.Name
                        }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSourceDonor()
        {
            return Json(from c in _donorService.GetAllDonor()
                            .Where(p => p.IsSourceDonor == true)
                            .DefaultIfEmpty()
                            .OrderBy(p => p.Name)
                        select new DonorViewModel
                        {
                            DonorId = c.DonorID,
                            Name = c.Name
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTransporter()
        {
            return Json(from c in _transporterService.GetAllTransporter().DefaultIfEmpty().OrderBy(o => o.Name)
                        select new TransporterViewModel
                        {
                            TransporterId = c.TransporterID,
                            Name = c.Name
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AutoCompleteCommodity(string term)
        {
            var result = (from commodity in _commodityService.GetAllCommodity()
                          where commodity.Name.ToLower().StartsWith(term.ToLower())
                          select commodity.Name);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private Method

        private void PopulateCombox(int parentCommodity)
        {
            var commodities = _commodityService.GetAllCommodityViewModelsByParent(parentCommodity);
            ViewData["commodities"] = commodities;
            ViewData["units"] = _unitService.GetAllUnitViewModels();
        }

        #endregion

    }
}