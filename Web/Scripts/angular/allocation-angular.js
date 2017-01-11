/// <reference path="jquery-1.9.1.js" />
/// <reference path="hello-angular.js" />
/// <reference path="angular.js" />

var $$scope;
var timer;
var HubId, StoreId;
// Create app Module
function onsaveAllocation() {
    var result = $$scope.saveAllocation();
    //if(result ===true)
  //  {
        window.location.href = backBtn;

}

var app = angular.module("dragDrop", ['ngResource']);

// Declaring a Service
app.factory("dragDropService", function ($resource)
{

    return {

        getRequisitions: $resource(Url + "?regionId=" + regionId)


    };






});
app.factory("hubService", function ($resource)
{

    return {

        getSWarehouse: $resource("Logistics/DispatchAllocation/ReadSWarehouse"  + 1)


    };






});
app.factory("savefactory", function ($http) {

    return {
        save: function (hubAllocated) {

            $http.post(UrlPOST, { allocation: hubAllocated }).success(function (responseData) {
                var msg = '<div class="cats_success">Success: Hub allocation saved successfully.</div>';
                $(".message-window").html(msg).delay(800).hide().fadeIn(); // Don't change this line.
                clearTimeout(timer); // Don't change this line.
                timer = setTimeout(function () { // Don't change this line.
                    $(".message-window").fadeOut("normal", function () { $(this).html(''); }); // Don't change this line.
                }, 5000); // Here is the millisecond duration in which the message will be displayed. Can be changed.
                return true;
            }).error(function (responseData) {
                var msg = '<div class="cats_error">Error: Hub allocation couldn\'t be saved.</div>';
                $(".message-window").html(msg).delay(800).hide().fadeIn(); // Don't change this line.
                clearTimeout(timer); // Don't change this line.
                timer = setTimeout(function () { // Don't change this line.
                    $(".message-window").fadeOut("normal", function () { $(this).html(''); }); // Don't change this line.
                }, 5000); // Here is the millisecond duration in which the message will be displayed. Can be changed.
                return false;

            });
        }
    };

});

app.controller("DragDroController", function ($scope, $http ,dragDropService, savefactory)
{
    $scope.showModal = false;
    $scope.WarehouseName = "";


    $scope.Warehouse;
    $scope.WarehouseList = [];
    $scope.SWarehouseName ;
    $scope.handleDrop = function (index) {


        HubId = index;
        StoreId = 0;
        if (index.indexOf(' ') === -1) {


        } else {
            var spaceIndex = index.indexOf(' ');

            HubId = index.substring(1, spaceIndex - 1);
            StoreId = index.substring(spaceIndex + 2);
        }


        $scope.allocated[0].StoreId = StoreId;
        $scope.allocated[0].HubId = HubId;
       // $scope.showModal = !$scope.showModal;



      $scope.GetWarehouseList1(index);


        $scope.showModal = !$scope.showModal;
        $scope.allocated[0].HubId = index;




    };
    $scope.saveWarehouse = function (SWarehouse) {


        $scope.showModal = !$scope.showModal;
        $scope.allocated[0].SatelliteWarehouseID = SWarehouse;

    };



    $scope.GetWarehouseList1 = function (index) {

        $http({ method: 'GET', url: '../DispatchAllocation/ReadSWarehouse?hubId=' + index })
            .success(function (data, status, headers, config) { $scope.WarehouseList = data; })
            .error(function (data, status, headers, config) {

            });


    };
    $scope.saveAllocation = function () {

        var result = savefactory.save($scope.allocated);
        return result;
    };

    $scope.RemoveRequisitionFromStore = function(requisition)
    {
        //Get parent
        var req = document.getElementById(requisition);
        //Remove from parent div

        req.parentElement.removeChild(req);
        //Remove req from allocations
        for (var i = 0; i < $$scope.allocated.length; i++) {
            if ($$scope.allocated[i].reqId == requisition) {
                $$scope.allocated.splice(i, 1);//Removes the list from the allocaated array

            }
        }
        //hide remove btn

        req.children[0].style.display = 'none';
        //Add req to $scope.Requisitions
        var container = document.getElementById("container");

        container.appendChild(req);


    }
    $scope.Requisitions = dragDropService.getRequisitions.query({}, isArray = true);
    $scope.allocated = [];


    $scope.newRequisitions = {

        0: "    No requisitions in " + RegionName +" region ",
        other: "{} requisitions  in " +  RegionName
    };

    $$scope = $scope;

});


app.directive('draggable', function () {


    return function (scope, element) {

        // this gives us the native JS object
        var el = element[0];

        el.draggable = true;

        el.addEventListener(
            'dragstart',
            function (e) {
                e.dataTransfer.effectAllowed = 'move';
                e.dataTransfer.setData('Text', this.id);
                this.classList.add('drag');
                return false;
            },
            false
        );

        el.addEventListener(
            'dragend',
            function (e) {

                this.classList.remove('drag');
                return false;
            },
            false
        );
    }
});


app.directive('droppable', function () {
    return {
        scope: {
            drop: '&',
            allocated: "="
        },
        link: function(scope, element) {


            // again we need the native object
            var el = element[0];

            el.addEventListener(
                'dragover',
                function(e) {
                    e.dataTransfer.dropEffect = 'move';
                    // allows us to drop
                    if (e.preventDefault) e.preventDefault();
                    this.classList.add('over');
                    return false;
                },
                false
            );
            el.addEventListener(
                'dragenter',
                function(e) {
                    this.classList.add('over');
                    return false;
                },
                false
            );

            el.addEventListener(
                'dragleave',
                function(e) {
                    this.classList.remove('over');
                    return false;
                },
                false
            );


            el.addEventListener(
                'drop',
                function(e) {
                    // Stops some browsers from redirecting.
                    if (e.stopPropagation) e.stopPropagation();

                    this.classList.remove('over');

                    var item = document.getElementById(e.dataTransfer.getData('Text'));
                    this.appendChild(item);


                    for (var i = 0; i < $$scope.allocated.length; i++) {
                        if ($$scope.allocated[i].reqId == item.id) {
                            $$scope.allocated.splice(i, 1);//Removes the list from the allocaated array

                        }
                    }
                    $$scope.allocated.splice(0, 0, { reqId: item.id, HubId: 'index',StoreId: StoreId });

                    scope.$apply('drop()');

                    //show Remove Button
                    item.children[0].style.display = 'inline';
                    //record parent
                    return false;
                },
                false
            );

        }
    };
});

app.directive('modal', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog">' +
              '<div class="modal-content">' +
                '<div class="modal-header">' +
                  '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                  '<h4 class="modal-title">{{ title }}</h4>' +
                '</div>' +
                '<div class="modal-body" ng-transclude></div>' +
              '</div>' +
            '</div>' +
          '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.title = attrs.title;

            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});