﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Models.Hubs;
using Cats.Services.Hub;
using Cats.Models.Hubs.ViewModels;
using Cats.Models.Hubs.ViewModels.Dispatch;
using Cats.Web.Hub;
using Cats.Web.Hub.Helpers;
using Telerik.Web.Mvc;
using Cats.Alert;

namespace Cats.Areas.Hub.Controllers.Allocations
{
    public class DispatchAllocationController : BaseController 
    {
        //
        // GET: /DispatchAllocation/
        
        private readonly IUserProfileService _userProfileService;
        private readonly IDispatchAllocationService _dispatchAllocationService;
        private readonly IShippingInstructionService _shippingInstructionService;
        private readonly IProjectCodeService _projectCodeService;
        private readonly IOtherDispatchAllocationService _otherDispatchAllocationService;
        private readonly ITransporterService _transporterService;
        private readonly ICommonService _commonService;
        private readonly IAdminUnitService _adminUnitService;
        private readonly IFDPService _fdpService;
        private readonly IHubService _hubService;
        private readonly ICommodityTypeService _commodityTypeService;
        private readonly IDonorService _donorService;
        private readonly IUnitService _unitService;
        public DispatchAllocationController(IDispatchAllocationService dispatchAllocationService,
                                            IUserProfileService userProfileService,
                                            IOtherDispatchAllocationService otherDispatchAllocationService,
                                            IShippingInstructionService shippingInstructionService,
                                            IProjectCodeService projectCodeService,
                                            ITransporterService transporterService,
                                            ICommonService commonService,
                                            IAdminUnitService adminUnitService,
                                            IFDPService fdpService,
                                            IHubService hubService,
                                             ICommodityTypeService commodityTypeService, IDonorService donorService, IUnitService unitService)
            : base(userProfileService)
        {
            this._dispatchAllocationService = dispatchAllocationService;
            this._userProfileService = userProfileService;
            this._otherDispatchAllocationService = otherDispatchAllocationService;
            this._projectCodeService = projectCodeService;
            this._shippingInstructionService = shippingInstructionService;
            this._transporterService = transporterService;
            this._adminUnitService = adminUnitService;
            this._fdpService = fdpService;
            this._commonService = commonService;
            this._hubService = hubService;
            this._commodityTypeService = commodityTypeService;
            this._donorService = donorService;
            this._unitService = unitService;
        }
        public ActionResult Index()
        {
            var user = _userProfileService.GetUser(User.Identity.Name);
            return View(_dispatchAllocationService.GetAvailableRequisionNumbers(user.DefaultHub.Value, true));
        }

        public ActionResult AllocationList()
        {
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            Cats.Models.Hubs.Hub hub = user.DefaultHubObj;
            var list = _dispatchAllocationService.GetUncommitedAllocationsByHub(hub.HubID);
            var listViewModel = (from item in list select BindDispatchAllocationViewModelDto(item));

            return PartialView("AllocationList", listViewModel);
        }
        private DispatchAllocationViewModelDto BindDispatchAllocationViewModelDto(DispatchAllocation dispatchAllocation)
        {
            var target = new DispatchAllocationViewModelDto();
            target.Amount = dispatchAllocation.Amount;
            target.AmountInUnit = dispatchAllocation.AmountInUnit;
            target.BidRefNo = dispatchAllocation.BidRefNo;
            target.CommodityID = dispatchAllocation.CommodityID;
            target.CommodityName = dispatchAllocation.Commodity.Name;
            target.DispatchAllocationID = dispatchAllocation.DispatchAllocationID;
            target.DispatchedAmount = dispatchAllocation.DispatchedAmount;
            target.DispatchedAmountInUnit = dispatchAllocation.DispatchedAmountInUnit;
            target.FDPName = dispatchAllocation.FDP.Name;
            target.IsClosed = dispatchAllocation.IsClosed;
            target.ProjectCodeID = dispatchAllocation.ProjectCodeID;
            //TODO:Check Region,zone,woreda Name
            target.Region = dispatchAllocation.FDP.AdminUnit.AdminUnit2.Name;
            target.RemainingQuantityInQuintals = dispatchAllocation.RemainingQuantityInQuintals;
            target.RemainingQuantityInUnit = dispatchAllocation.RemainingQuantityInUnit;
            target.RequisitionNo = dispatchAllocation.RequisitionNo;
            target.ShippingInstructionID = dispatchAllocation.ShippingInstructionID;
            target.TransporterName = dispatchAllocation.Transporter.Name;
            target.Woreda = dispatchAllocation.FDP.AdminUnit.Name;
            target.Zone = dispatchAllocation.FDP.AdminUnit.AdminUnit2.Name;
            return target;
        }
            [HttpPost]
        public ActionResult CommitAllocation(string[] checkedRecords, int? SINumber, int? ProjectCode, string Sitext, string ProjectCodeText )
        {
            var user = _userProfileService.GetUser(User.Identity.Name);
            if(checkedRecords != null && SINumber != -1 && ProjectCode != -1)
            {
                if(SINumber == 0 || ProjectCode == 0 )
                {
                    SINumber =_shippingInstructionService.GetSINumberIdWithCreate(Sitext).ShippingInstructionID;
                    ProjectCode = _projectCodeService.GetProjectCodeIdWIthCreate(ProjectCodeText).ProjectCodeID;
                }
                _dispatchAllocationService.CommitDispatchAllocation(checkedRecords, SINumber.Value, user, ProjectCode.Value);
            }
            
            return RedirectToAction("Index","Dispatch");
        }

        public ActionResult GetAvailableSINumbers(int? CommodityID)
        {
          UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            if (CommodityID.HasValue)
            {
                var nums = from si in _dispatchAllocationService.GetAvailableSINumbersWithUncommitedBalance(CommodityID.Value,user.DefaultHub.Value)
                           select new { Name = si.Value, Id = si.ShippingInstructionID };

                return Json(new SelectList(nums, "Id", "Name"), JsonRequestBehavior.AllowGet);
            }
            return Json(new SelectList(Enumerable.Empty<SelectListItem>()),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCommodities(string RequisitionNo)
        {
            if (!string.IsNullOrEmpty(RequisitionNo))
            {
                var commodities = from c in _dispatchAllocationService.GetAvailableCommodities(RequisitionNo)
                           select new { Text = c.Name, Value = c.CommodityID };
                return Json(commodities, JsonRequestBehavior.AllowGet);
            }
            return new EmptyResult();
        }

        public ActionResult SiBalances(string requisition)
        {
            // THIS only considers the first commodity
            // TODO: check if has to be that way.
           var user = _userProfileService.GetUser(User.Identity.Name);
            Commodity commodity = _dispatchAllocationService.GetAvailableCommodities(requisition).FirstOrDefault();
            List<SIBalance> sis = _dispatchAllocationService.GetUncommitedSIBalance(UserProfile.DefaultHub.Value, commodity.CommodityID,user.PreferedWeightMeasurment);
            ViewBag.UnitType = commodity.CommodityTypeID;
            return PartialView("SIBalance", sis);
        }

        public ActionResult GetBalance(int? siNumber, int? commodityId, string siNumberText)
        {
            if (siNumber.HasValue && commodityId.HasValue)
            {
                UserProfile user = _userProfileService.GetUser(User.Identity.Name);
                List<SIBalance> si = (from v in _dispatchAllocationService.GetUncommitedSIBalance(
                                              UserProfile.DefaultHub.Value,
                                              commodityId.Value,user.PreferedWeightMeasurment)
                                      select v).ToList();
                
               
                SIBalance sis = new SIBalance();
                    if(siNumber.Value == 0 )
                        sis = si.FirstOrDefault(v1 => v1.SINumber == siNumberText); 
                    else
                        sis = si.FirstOrDefault(v1 => v1.SINumberID == siNumber.Value);
                    
               
                decimal balance = sis.Dispatchable;// +ReaminingExpectedReceipts;
                return Json(balance, JsonRequestBehavior.AllowGet);
            }
            return Json(new EmptyResult());
        }

        public ActionResult GetAllocations(string RquisitionNo, int? CommodityID, bool Uncommited)
        {
            ViewBag.req = RquisitionNo;
            ViewBag.Com = CommodityID;
            ViewBag.Uncommited = Uncommited;
            var list = new List<DispatchAllocation>();
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            if (!string.IsNullOrEmpty(RquisitionNo) && CommodityID.HasValue)
            {
                list = _dispatchAllocationService.GetAllocations(RquisitionNo, CommodityID.Value, user.DefaultHub.Value, Uncommited, user.PreferedWeightMeasurment);
            }
            else if (!string.IsNullOrEmpty(RquisitionNo))
            {
                list = _dispatchAllocationService.GetAllocations(RquisitionNo, user.DefaultHub.Value, Uncommited);
            }

            List<DispatchAllocationViewModelDto> FDPAllocations = new List<DispatchAllocationViewModelDto>();
            foreach (var dispatchAllocation in list)
            {
                var DAVMD = new DispatchAllocationViewModelDto();
                string preferedWeightMeasurment = user.PreferedWeightMeasurment.ToUpperInvariant();
                //if (preferedWeightMeasurment == "MT" && dispatchAllocation.Commodity.CommodityTypeID == 1) //only for food
                //{
                //    DAVMD.Amount = dispatchAllocation.Amount / 10;
                //    DAVMD.DispatchedAmount = dispatchAllocation.DispatchedAmount / 10;
                //    DAVMD.RemainingQuantityInQuintals = dispatchAllocation.RemainingQuantityInQuintals / 10;
                //}
                //else
                {
                    DAVMD.Amount = dispatchAllocation.Amount;
                    DAVMD.DispatchedAmount = dispatchAllocation.DispatchedAmount;
                    DAVMD.RemainingQuantityInQuintals = dispatchAllocation.RemainingQuantityInQuintals;
                }
                DAVMD.DispatchAllocationID = dispatchAllocation.DispatchAllocationID;
                DAVMD.CommodityName = dispatchAllocation.Commodity.Name;
                DAVMD.CommodityID = dispatchAllocation.CommodityID;
                DAVMD.RequisitionNo = dispatchAllocation.RequisitionNo;
                DAVMD.BidRefNo = dispatchAllocation.BidRefNo;
                DAVMD.ProjectCodeID = dispatchAllocation.ProjectCodeID;
                DAVMD.ShippingInstructionID = dispatchAllocation.ShippingInstructionID;

                DAVMD.Region = dispatchAllocation.FDP.AdminUnit.AdminUnit2.AdminUnit2.Name;
                DAVMD.Zone = dispatchAllocation.FDP.AdminUnit.AdminUnit2.Name;
                DAVMD.Woreda = dispatchAllocation.FDP.AdminUnit.Name;
                DAVMD.FDPName = dispatchAllocation.FDP.Name;
                DAVMD.TransporterName = dispatchAllocation.Transporter.Name;
                DAVMD.IsClosed = dispatchAllocation.IsClosed;


                DAVMD.AmountInUnit = DAVMD.Amount;
                DAVMD.DispatchedAmountInUnit = dispatchAllocation.DispatchedAmountInUnit;
                DAVMD.RemainingQuantityInUnit = dispatchAllocation.RemainingQuantityInUnit;

                FDPAllocations.Add(DAVMD);

            }

            return PartialView("AllocationList", FDPAllocations);

        }


        [GridAction]
        public ActionResult GetAllocationsGrid(string RquisitionNo, int? CommodityID, bool? Uncommited)
        {
            bool commitStatus = true;
            if (Uncommited.HasValue) commitStatus = Uncommited.Value;
            var list = new List<DispatchAllocation>();
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            if (!string.IsNullOrEmpty(RquisitionNo) && CommodityID.HasValue)
            {
                list = _dispatchAllocationService.GetAllocations(RquisitionNo, CommodityID.Value, user.DefaultHub.Value, commitStatus, user.PreferedWeightMeasurment);
            }
            else if (!string.IsNullOrEmpty(RquisitionNo))
            {
                list = _dispatchAllocationService.GetAllocations(RquisitionNo, user.DefaultHub.Value, commitStatus);
            }
            List<DispatchAllocationViewModelDto> FDPAllocations = new List<DispatchAllocationViewModelDto>();
            foreach (var dispatchAllocation in list)
            {
                var DAVMD = new DispatchAllocationViewModelDto();
                string preferedWeightMeasurment = user.PreferedWeightMeasurment.ToUpperInvariant();
                //if (preferedWeightMeasurment == "MT" && dispatchAllocation.Commodity.CommodityTypeID == 1) //only for food
                //{
                //    DAVMD.Amount = dispatchAllocation.Amount / 10;
                //    DAVMD.DispatchedAmount = dispatchAllocation.DispatchedAmount / 10;
                //    DAVMD.RemainingQuantityInQuintals = dispatchAllocation.RemainingQuantityInQuintals / 10;
                //}
                //else
                {
                    DAVMD.Amount = dispatchAllocation.Amount;
                    DAVMD.DispatchedAmount = dispatchAllocation.DispatchedAmount;
                    DAVMD.RemainingQuantityInQuintals = dispatchAllocation.RemainingQuantityInQuintals;
                }
                DAVMD.DispatchAllocationID = dispatchAllocation.DispatchAllocationID;
                DAVMD.CommodityName = dispatchAllocation.Commodity.Name;
                DAVMD.CommodityID = dispatchAllocation.CommodityID;
                DAVMD.RequisitionNo = dispatchAllocation.RequisitionNo;
                DAVMD.BidRefNo = dispatchAllocation.BidRefNo;
                DAVMD.ProjectCodeID = dispatchAllocation.ProjectCodeID;
                DAVMD.ShippingInstructionID = dispatchAllocation.ShippingInstructionID;

                DAVMD.Region = dispatchAllocation.FDP.AdminUnit.AdminUnit2.AdminUnit2.Name;
                DAVMD.Zone = dispatchAllocation.FDP.AdminUnit.AdminUnit2.Name;
                DAVMD.Woreda = dispatchAllocation.FDP.AdminUnit.Name;
                DAVMD.FDPName = dispatchAllocation.FDP.Name;
                DAVMD.TransporterName = dispatchAllocation.Transporter.Name;
                DAVMD.IsClosed = dispatchAllocation.IsClosed;


                DAVMD.AmountInUnit = DAVMD.Amount;
                DAVMD.DispatchedAmountInUnit = dispatchAllocation.DispatchedAmountInUnit;
                DAVMD.RemainingQuantityInUnit = dispatchAllocation.RemainingQuantityInUnit;

                FDPAllocations.Add(DAVMD);

            }

            return View(new GridModel(FDPAllocations));

        }

        public ActionResult GetSIBalances()
        {
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            var list = _dispatchAllocationService.GetSIBalances(user.DefaultHub.Value);
            return PartialView("SIBalance", list);
        }

        public ActionResult AvailableRequistions(bool UnCommited)
        {
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            return Json(_dispatchAllocationService.GetAvailableRequisionNumbers(user.DefaultHub.Value, UnCommited), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.CommodityTypes = new SelectList(_commonService.GetAllCommodityType(), "CommodityTypeID", "Name");
            ViewBag.Commodities = new SelectList(_commonService.GetAllParents(), "CommodityID", "Name");
            ViewBag.Donors = new SelectList(_commonService.GetAllDonors(), "DonorID", "Name");
            ViewBag.Regions = new SelectList(_commonService.GetRegions(), "AdminUnitID", "Name");
            ViewBag.Zones = new SelectList(Enumerable.Empty<SelectListItem>(), "AdminUnitID", "Name");
            ViewBag.Woredas = new SelectList(Enumerable.Empty<SelectListItem>(), "AdminUnitID", "Name");
            ViewBag.FDPS = new SelectList(Enumerable.Empty<SelectListItem>(), "FDPID", "Name");
            ViewBag.Years = new SelectList(_commonService.GetYears().Select(y => new { Name = y, Id = y }), "Id", "Name");
            ViewBag.Months = new SelectList(MonthHelper.GetMonthList(), "Id", "Name");
            ViewBag.Transporters = new SelectList(_transporterService.GetAllTransporter().OrderBy(t=>t.Name), "TransporterID", "Name");
            ViewBag.Programs = new SelectList(_commonService.GetAllProgram(), "ProgramID", "Name");
            ViewBag.Units = new SelectList(_commonService.GetAllUnit(), "UnitID", "Name");
            return PartialView("Create", new DispatchAllocationViewModel());
        }

        [HttpPost]
        public ActionResult Create(DispatchAllocationViewModel allocation)
        {
            if (ModelState.IsValid)
            {
                var user = _userProfileService.GetUser(User.Identity.Name);
                var alloc = GetAllocationModel(allocation);
                alloc.DispatchAllocationID = Guid.NewGuid();
                alloc.HubID = user.DefaultHub.Value;
                _dispatchAllocationService.AddDispatchAllocation(alloc);
                if (this.Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
                else return RedirectToAction("Index");
            }
            PrepareEdit(allocation);
            return PartialView("Create",allocation);
        }


        public ActionResult Edit(string id)
        {
            DispatchAllocation allocation = _dispatchAllocationService.FindById(Guid.Parse(id));
           DispatchAllocationViewModel alloc = GetAllocationModel(allocation);
            alloc.CommodityTypeID = allocation.Commodity.CommodityTypeID;
            PrepareEdit(alloc);
            return PartialView("Edit", alloc);
            
            
        }

        private void PrepareCreate()
        {
            ViewBag.CommodityTypes = new SelectList(_commonService.GetAllCommodityType(), "CommodityTypeID", "Name");
            ViewBag.Commodities = new SelectList(_commonService.GetAllParents(), "CommodityID", "Name");
            ViewBag.Donors = new SelectList(_commonService.GetAllDonors(), "DonorID", "Name");
            ViewBag.Regions = new SelectList(_commonService.GetRegions(), "AdminUnitID", "Name");
            ViewBag.Zones = new SelectList(Enumerable.Empty<SelectListItem>(), "AdminUnitID", "Name");
            ViewBag.Woredas = new SelectList(Enumerable.Empty<SelectListItem>(), "AdminUnitID", "Name");
            ViewBag.FDPS = new SelectList(Enumerable.Empty<SelectListItem>(), "FDPID", "Name");
            ViewBag.Years = new SelectList(_commonService.GetYears().Select(y => new { Name = y, Id = y }), "Id", "Name");
            ViewBag.Months = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
            ViewBag.Transporters = new SelectList(_transporterService.GetAllTransporter(), "TransporterID", "Name");
            ViewBag.Programs = new SelectList(_commonService.GetAllProgram(), "ProgramID", "Name");
            ViewBag.Units = new SelectList(_commonService.GetAllUnit(), "UnitID", "Name");
        }

        [HttpPost]
        public ActionResult Edit(DispatchAllocationViewModel allocation)
        {
            if (ModelState.IsValid)
            {
                DispatchAllocation alloc = GetAllocationModel(allocation);
                _dispatchAllocationService.EditDispatchAllocation(alloc);
                if (this.Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
                else return RedirectToAction("Index");
                //return Json(true, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }
            PrepareEdit(allocation);
            return PartialView(allocation);
        }

        private void PrepareEdit(DispatchAllocationViewModel allocation)
        {
            ViewBag.Commodities = new SelectList(_commonService.GetAllParents(), "CommodityID", "Name", allocation.CommodityID);

            ViewBag.CommodityTypes = new SelectList(_commonService.GetAllCommodityType(), "CommodityTypeID", "Name",allocation.CommodityTypeID);

            ViewBag.Donors = new SelectList(_commonService.GetAllDonors(), "DonorID", "Name", allocation.DonorID);
            
            ViewBag.Years = new SelectList(_commonService.GetYears().Select(y => new { Name = y, Id = y }), "Id", "Name", allocation.Year);
            if (allocation.Year.HasValue)
            {
                ViewBag.Months = new SelectList(_commonService.GetMonths(allocation.Year.Value).Select(p => new{Id = p, Name = p }), "Id", "Name", allocation.Month);
            }
            else
            {
                ViewBag.Months = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
            }
            ViewBag.Transporters = new SelectList(_transporterService.GetAllTransporter(), "TransporterID", "Name",allocation.TransporterID);
            ViewBag.Programs = new SelectList(_commonService.GetAllProgram(), "ProgramID", "Name",allocation.ProgramID);
            ViewBag.Units = new SelectList(_commonService.GetAllUnit(), "UnitID", "Name",allocation.Unit);

           // TODO we can use the line below for debugging and server side validation
            PrepareFDPForEdit(allocation.FDPID);

        }

        private void PrepareFDPForEdit(int? fdpid)
        {

           AdminUnitModel unitModel = new AdminUnitModel();
            FDP fdp;
            if (fdpid != null)
                fdp = _fdpService.FindById(fdpid.Value);
            else
                fdp = null;
            if (fdp != null)
            {
                unitModel.SelectedWoredaId = fdp.AdminUnitID;
                if (fdp.AdminUnit.ParentID != null) unitModel.SelectedZoneId = fdp.AdminUnit.ParentID.Value;

                unitModel.SelectedRegionId = _adminUnitService.GetRegionByZoneId(unitModel.SelectedZoneId);
                ViewBag.Regions =
                    new SelectList(
                        _adminUnitService.GetRegions().Select(p => new {Id = p.AdminUnitID, Name = p.Name}).OrderBy(
                            o => o.Name), "Id", "Name", unitModel.SelectedRegionId);
                ViewBag.Zones =
                    new SelectList(this.GetChildren(unitModel.SelectedRegionId).OrderBy(o => o.Name), "Id", "Name",
                                   unitModel.SelectedZoneId);
                ViewBag.Woredas =
                    new SelectList(this.GetChildren(unitModel.SelectedZoneId).OrderBy(o => o.Name), "Id", "Name",
                                   unitModel.SelectedWoredaId);
                ViewBag.FDPS = new SelectList(this.GetFdps(unitModel.SelectedWoredaId).OrderBy(o => o.Name), "Id",
                                               "Name", fdp.FDPID);
            }
            else
            {
                ViewBag.SelectedRegionId = new SelectList(unitModel.Regions, "Id", "Name");
                ViewBag.SelectedWoredaId = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
                ViewBag.FDPID = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
                ViewBag.SelectedZoneId = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Name");
            }

        }

        public List<AdminUnitItem> GetFdps(int woredaId)
        {
            var fdps = from p in _fdpService.GetFDPsByWoreda(woredaId)
                       select new AdminUnitItem() { Id = p.FDPID, Name = p.Name };
            return fdps.ToList();
        }

        public List<AdminUnitItem> GetChildren(int parentId)
        {
            var units = from item in _adminUnitService.GetChildren(parentId)
                        select new AdminUnitItem
                        {
                            Id = item.AdminUnitID,
                            Name = item.Name
                        };
            return units.ToList();
        }

        public ActionResult SelectionHeader( string requisition)
        {
            DispatchAllocation dispatchAllocation =
                _dispatchAllocationService.GetAllocations(requisition).FirstOrDefault();
           return PartialView(dispatchAllocation);
        }


        private DispatchAllocationViewModel GetAllocationModel(DispatchAllocation dispatch)
        {
            //TODO:Make sure if includeproperties are loaded correctly
            var fdp = _fdpService.FindById(dispatch.FDPID);
            var donorid = (dispatch.DonorID) ?? -1;
            var donor = (donorid != -1) ? _donorService.FindById(donorid).Name : "-";
            var monthid = (dispatch.Month) ?? -1;
            var month = (monthid != -1) ? Cats.Helpers.RequestHelper.MonthName(monthid) : "-";
           DispatchAllocationViewModel model = new DispatchAllocationViewModel(fdp);
            model.Amount = dispatch.Amount;
            model.Beneficiery = dispatch.Beneficiery;
            model.BidRefNo = dispatch.BidRefNo;
            model.CommodityID = dispatch.CommodityID;
            model.DispatchAllocationID = dispatch.DispatchAllocationID;
            model.DonorID = dispatch.DonorID;
            model.FDPID = dispatch.FDPID;
            model.HubID = dispatch.HubID;
            model.Month = dispatch.Month;
            model.PartitionId = dispatch.PartitionId;
            model.ProgramID = dispatch.ProgramID;
            model.ProjectCodeID = dispatch.ProjectCodeID;
            model.RequisitionNo = dispatch.RequisitionNo;
            model.Round = dispatch.Round;
            model.ShippingInstructionID = dispatch.ShippingInstructionID;
            model.TransporterID = dispatch.TransporterID;
            model.Unit = dispatch.Unit;
            model.Year = dispatch.Year;
            model.CommodityTypeID = dispatch.Commodity.CommodityTypeID;
            model.ProgramName = dispatch.Program.Name;
            model.CommodityTypeName = dispatch.Commodity.CommodityType.Name;
            model.CommodityName = dispatch.Commodity.Name;
            model.DonorName = donor;
            model.UnitName = _unitService.FindById(dispatch.Unit) == null ? "-" : _unitService.FindById(dispatch.Unit).Name;
            model.MonthName = month;
            model.TransporterName = dispatch.Transporter.Name;
            return model;
        }

        /// <summary>
        /// Gets the allocation model.
        /// </summary>
        /// <param name="dispatch">The dispatch.</param>
        /// <returns></returns>
        private DispatchAllocation GetAllocationModel(DispatchAllocationViewModel dispatch)
        {
            DispatchAllocation model = new DispatchAllocation();
            model.Amount = dispatch.Amount;
            model.Beneficiery = dispatch.Beneficiery;
            model.BidRefNo = dispatch.BidRefNo;
            model.CommodityID = dispatch.CommodityID;
            if(dispatch.DispatchAllocationID.HasValue)
            {
                model.DispatchAllocationID = dispatch.DispatchAllocationID.Value;    
            }
            
            model.DonorID = dispatch.DonorID;
            model.FDPID = dispatch.FDPID;
            model.HubID = dispatch.HubID;
            model.Month = dispatch.Month;
            model.PartitionId = dispatch.PartitionId;
            model.ProgramID = dispatch.ProgramID;
            model.ProjectCodeID = dispatch.ProjectCodeID;
            model.RequisitionNo = dispatch.RequisitionNo;
            model.Round = dispatch.Round;
            model.ShippingInstructionID = dispatch.ShippingInstructionID;
            model.TransporterID = dispatch.TransporterID;
            model.Unit = dispatch.Unit;
            model.Year = dispatch.Year;
            return model;
        }

        /// <summary>
        /// Allocate dispatch To the other owners.
        /// </summary>
        /// <returns></returns>
        public ActionResult ToOtherOwners()
        {
            var model = _otherDispatchAllocationService.GetAllToOtherOwnerHubs(_userProfileService.GetUser(User.Identity.Name));
            return View(model);
            
        }

        /// <summary>
        /// Allocate dispatch To hubs of the same owner ( Transfer and Swap) 
        /// </summary>
        /// <returns></returns>
        public ActionResult ToHubs()
        {
            var model = _otherDispatchAllocationService.GetAllToCurrentOwnerHubs(_userProfileService.GetUser(User.Identity.Name));
            return View(model);
        }

        public ActionResult CreateTransfer()
        {
            var model = new OtherDispatchAllocationViewModel();
           
            return PartialView("EditTransfer", InitTransfer(model));
        }
        private OtherDispatchAllocationViewModel InitTransfer(OtherDispatchAllocationViewModel otherDispatchAllocationViewModel)
        {
            var user = _userProfileService.GetUser(User.Identity.Name);
            var tohubs = _hubService.GetOthersHavingSameOwner(user.DefaultHubObj);

            var commodities = _commonService.GetAllParents();
            var commodityTypes = _commodityTypeService.GetAllCommodityType();
            var programs = _commonService.GetAllProgram();
            var units = _commonService.GetAllUnit();

            otherDispatchAllocationViewModel.InitTransfer(user, tohubs, commodities, commodityTypes, programs, units);
            return otherDispatchAllocationViewModel;
        }
        public ActionResult EditTransfer2(string id)
        {
            var model = new OtherDispatchAllocationViewModel();
            if (id != null && id != "")
            {
                model = _otherDispatchAllocationService.GetViewModelByID(Guid.Parse(id));
            }
           // var model = _otherDispatchAllocationService.GetViewModelByID(Guid.Parse(id));
            // model.InitTransfer(_userProfileService.GetUser(User.Identity.Name), repository);
            return PartialView("EditOthers", InitTransfer(model));
        }

        public ActionResult EditTransfer(string id)
        {
            var model = _otherDispatchAllocationService.GetViewModelByID( Guid.Parse(id));
           // model.InitTransfer(_userProfileService.GetUser(User.Identity.Name), repository);
            return PartialView("EditTransfer", InitTransfer(model));
        }

        // Only do the save if this has been a post.
        [HttpPost]
        public ActionResult SaveTransfer(OtherDispatchAllocationViewModel model)
        {
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            model.FromHubID = user.DefaultHub.Value;
            if (ModelState.IsValid)
            {
                _otherDispatchAllocationService.Save(model);
                if (this.Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
                else return RedirectToAction("ToHubs");
                //return RedirectToAction("ToHubs");
            }
            else
            {
               // model.InitTransfer(_userProfileService.GetUser(User.Identity.Name), repository);
                return PartialView("EditTransfer", InitTransfer(model));
            }
        }


        public ActionResult CreateLoan()
        {
            var model = new OtherDispatchAllocationViewModel();
           // model.InitLoan(_userProfileService.GetUser(User.Identity.Name), repository);
            return PartialView("EditLoans",InitLoan(model));
        }

        private OtherDispatchAllocationViewModel InitLoan(OtherDispatchAllocationViewModel otherDispatchAllocationViewModel)
        {
            var user = _userProfileService.GetUser(User.Identity.Name);
            var tohubs = _hubService.GetOthersWithDifferentOwner(user.DefaultHubObj);

            var commodities = _commonService.GetAllParents();
            var commodityTypes = _commodityTypeService.GetAllCommodityType();
            var programs = _commonService.GetAllProgram();
            var units = _commonService.GetAllUnit();

           otherDispatchAllocationViewModel.InitLoan(user,tohubs,commodities,commodityTypes,programs,units);
            return otherDispatchAllocationViewModel;
        }
        public ActionResult EditLoan2(string id)
        {
            var model = new OtherDispatchAllocationViewModel();
            if (id != null && id != "")
            {
                model = _otherDispatchAllocationService.GetViewModelByID(Guid.Parse(id));
            }
                // model.InitLoan(_userProfileService.GetUser(User.Identity.Name), repository);
            return PartialView("EditOthers", InitLoan(model));
        }
        public ActionResult EditLoan(string id)
        {
            var model = _otherDispatchAllocationService.GetViewModelByID( Guid.Parse(id));
           // model.InitLoan(_userProfileService.GetUser(User.Identity.Name), repository);
            return PartialView("EditLoans", InitLoan(model));
        }
        public ActionResult SaveTransferAjax(OtherDispatchAllocationViewModel model)
        {
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            model.FromHubID = user.DefaultHub.Value;
            if (ModelState.IsValid)
            {
                _otherDispatchAllocationService.Save(model);
                return Json("{status:1}", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("{status:0}", JsonRequestBehavior.AllowGet);

                //return PartialView("EditLoans", InitLoan(model));
            }
        }
        public ActionResult SaveFDPAllocationAjax(DispatchAllocationViewModel allocation)
        {
            if (ModelState.IsValid)
            {
                DispatchAllocation alloc = GetAllocationModel(allocation);
                _dispatchAllocationService.EditDispatchAllocation(alloc);

      
                WorkflowCommon.EnterEditWorkflow(alloc, "FDP Allocation Has been Saved.");


                return Json("{status:1}", JsonRequestBehavior.AllowGet);
            }
            return Json("{status:0}", JsonRequestBehavior.AllowGet);
        }
        // Only do the save if this has been a post.
        [HttpPost]
        public ActionResult SaveLoan(OtherDispatchAllocationViewModel model)
        {
           UserProfile user = _userProfileService.GetUser(User.Identity.Name);
           model.FromHubID = user.DefaultHub.Value;
            if (ModelState.IsValid)
            {
                _otherDispatchAllocationService.Save(model);
                if (this.Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
                else return RedirectToAction("ToOtherOwners");
                //return RedirectToAction("ToOtherOwners");
            }
            else
            {
                
                return PartialView("EditLoans", InitLoan(model));
                //return PartialView("EditLoans", _otherDispatchAllocationService.GetViewModelByID((Guid)model.OtherDispatchAllocationID));
            }
        }

        // remote validations
        public ActionResult IsProjectValid(string ProjectCode)
        {
            var count = _projectCodeService.GetProjectCodeId(ProjectCode);
            var result = (count > 0);
            return (Json(result, JsonRequestBehavior.AllowGet));
        }

        public ActionResult IsSIValid(string ShippingInstruction)
        {
            bool result = false;
            result = _shippingInstructionService.GetShipingInstructionId(ShippingInstruction) > 0;
            return (Json(result, JsonRequestBehavior.AllowGet));
        }

        private DispatchAllocationViewModel GetAllocationModelForClose(DispatchAllocation dispatch)
        {
            //TODO:Make sure if includeproperties are loaded correctly
            var fdp = _fdpService.FindById(dispatch.FDPID);
            var donorid = (dispatch.DonorID) ?? -1;
            var donor = (donorid != -1) ? _donorService.FindById(donorid).Name : "-";
            var monthid = (dispatch.Month) ?? -1;
            var month = (monthid != -1) ? Cats.Helpers.RequestHelper.MonthName(monthid) : "-";
            var projectcodeId = (dispatch.ProjectCodeID) ?? -1;
            var projectcodevalue = (projectcodeId != -1) ? _projectCodeService.GetProjectCodeValueByProjectCodeId(projectcodeId) : "-";
            DispatchAllocationViewModel model = new DispatchAllocationViewModel(fdp);
           model.Amount = dispatch.Amount;
           model.BidRefNo = dispatch.BidRefNo;
           model.FDPName = dispatch.FDP.Name;
           model.RequisitionNo = dispatch.RequisitionNo;
           model.Round = dispatch.Round;
           model.Year = dispatch.Year;
           model.ProgramName = dispatch.Program.Name;
           model.CommodityName = dispatch.Commodity.Name;
           model.MonthName = month;
           model.ShippingInstruction = dispatch.ShippingInstruction;
           model.ProjectCodeValue = projectcodevalue;
            model.RemainingQuantityInQuintals = dispatch.RemainingQuantityInQuintals;
            return model;
        }

        public ActionResult Close(string id)
        {
            //var closeAllocation = _dispatchAllocationService.FindById(Guid.Parse(id));
            //return PartialView("Close", closeAllocation);
            DispatchAllocation allocation = _dispatchAllocationService.FindById(Guid.Parse(id));
            DispatchAllocationViewModel alloc = GetAllocationModelForClose(allocation);

            WorkflowCommon.EnterDelteteWorkflow(allocation);
            
            return PartialView("Close", alloc);
        }

        [HttpPost, ActionName("Close")]
        public ActionResult CloseConfirmed(string DispatchAllocationID)
        {
            var closeAllocation = _dispatchAllocationService.FindById(Guid.Parse(DispatchAllocationID));
            if (closeAllocation != null)
            {
                _dispatchAllocationService.CloseById(Guid.Parse(DispatchAllocationID));
                return Json(new { status=1,gridNum = 1 }, JsonRequestBehavior.AllowGet);
                //if (this.Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
                //else return RedirectToAction("Index");
            }
            return this.Close(DispatchAllocationID);
        }

        public ActionResult OtherClose(string id)
        {
            var closeAllocation = _otherDispatchAllocationService.FindById(Guid.Parse(id));
            return PartialView("CloseOther", closeAllocation);
        }

        [HttpPost, ActionName("OtherClose")]
        public ActionResult OtherCloseConfirmed(string OtherDispatchAllocationID)
        {
            var closeAllocation = _otherDispatchAllocationService.FindById(Guid.Parse(OtherDispatchAllocationID));
            UserProfile user = _userProfileService.GetUser(User.Identity.Name);
            if (closeAllocation != null)
            {
                _otherDispatchAllocationService.CloseById(Guid.Parse(OtherDispatchAllocationID));
                int? gridNum = null;
                if (closeAllocation.Hub1.HubOwnerID == user.DefaultHubObj.HubOwnerID)
                {
                    gridNum = 3;
                }
                else
                {
                    gridNum = 2;
                }

                return Json(new { gridNum = 3,status=1 }, JsonRequestBehavior.AllowGet);
                //if (this.Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
                //else return RedirectToAction("Index");
            }
            return this.Close(OtherDispatchAllocationID);
        }


        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                     _userProfileService.Dispose();
                     _dispatchAllocationService.Dispose();
                     _shippingInstructionService.Dispose();
                     _projectCodeService.Dispose();
                     _otherDispatchAllocationService.Dispose();
                     _transporterService.Dispose();
                     _commonService.Dispose();
                     _adminUnitService.Dispose();
                     _fdpService.Dispose();
                     _hubService.Dispose();
                     _commodityTypeService.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
    }
}
