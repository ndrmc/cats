DataEntryIndicatorApp=angular.module("DataEntryIndicatorApp", [])    .controller('WorkflowActController', function WorkflowActController($scope, DataServices, $http) {




    $scope.displayedUserNames = [];//ROW STARTERS
    $scope.displayedWorkflowNames = [];//FOR THE COLUMNS
    $scope.displayedWorkflows = [];
    $scope.displayedWorkflowContent = [];
    $scope.displayedWorkflowContentColumns = [];
    $scope.displayedWorkflowList = [];

    $scope.filterData = {
        selectedUsers: [],
        selectedActivities: [],
        selectedDocument: '',
        selectedStartDate: '',
        selectedEndDate: ''
    }

    $scope.controlData = {
        lookupUsers: [],
        lookupActivities: [],
        lookupDocument: [],
        mappingDocumentToActivities: []

    };

    $scope.calcColumnTotal = function (columnName) {
        console.log(columnName);
        var sum = 0;
        angular.forEach($scope.displayedWorkflowList, function ( users) {
            angular.forEach(users.dashboardDataEntries, function (wf) {
                var userActivityname = wf.activityName.trim().toCamel();
                if (columnName == userActivityname) {

                    sum = sum + parseInt(wf.activityCount);

                }


            });


        });
        return sum;
        };
    $scope.init = function () {


        var initializeControls = function () {

            DataServices.getAllTeamUsers().then(function (result) {

                $scope.controlData.lookupUsers = result.data;


            //    $('#user').multiselect({

            //        templates: {

            //            li: '<li><div class="checkbox"><label></label></div></li>'
            //        }

            //    });


            //    $('.multiselect-container div.checkbox').each(function (index) {

            //        var id = 'multiselect-' +index,

            //            $input = $(this).find('input');

            //        // Associate the label and the input

            //        $(this).find('label').attr('for', id);

            //        $input.attr('id', id);

            //        // Remove the input from the label wrapper

            //        $input.detach();

            //        // Place the input back in before the label

            //        $input.prependTo($(this));

            //        $(this).click(function (e) {

            //// Prevents the click from bubbling up and hiding the dropdown

            //e.stopPropagation();

            //});


            //});


            });



            DataServices.getAllWorkflows().then(function (result) {

                $scope.controlData.lookupDocument = result.data;


                if ($scope.controlData.lookupDocument.length > 0)
                { $scope.filterData.selectedDocument = $scope.controlData.lookupDocument[0].name; }


                populateActivityCombo($scope.filterData.selectedDocument);



            //    buildTableStructure();
            //    $('#Document').multiselect({

            //        templates: {

            //            li: '<li><div class="checkbox"><label></label></div></li>'
            //        }

            //    });
            });


            DataServices.getAllStateTemplate().then(function (result) {

                $scope.controlData.lookupActivities = result.data;


            //    $('#Activities').multiselect({

            //        templates: {

            //            li: '<li><div class="checkbox"><label></label></div></li>'
            //        }

            //    });
            });







        };

        var setFilterControlDefaultValue = function () {


            var now = new Date();
            var aWeekBefore = new Date(new Date().setDate(new Date().getDate() - 5));



            $scope.filterData.selectedStartDate = now.toLocaleDateString();
            $scope.filterData.selectedEndDate = aWeekBefore.toLocaleDateString();




        };


        initializeControls();

        setFilterControlDefaultValue();

        //getDataEntryStat();



    }

    var populateActivityCombo=function(selectedWorkflow){
        DataServices.getAllStateTemplate(selectedWorkflow).then(function (result) {
            $scope.controlData.lookupActivities = result.data;
            buildTableStructure();
        });
        };

         $scope.InvalidFilter="DEFAULT";

    var getDataEntryStat = function () {

        if(filterIsValid())
        {

         $scope.InvalidFilter= "DEFAULT";

            DataServices.getDataEntryStat($scope.filterData.selectedStartDate, $scope.filterData.selectedEndDate, $scope.filterData.selectedDocument,
            $scope.filterData.selectedUsers, $scope.filterData.selectedActivities).then(function (result) {
             $scope.InvalidFilter="VALID";

             $scope.displayedWorkflowList= [];
            $scope.displayedWorkflowList = result.data;


        });
        }
                else
                {
                 $scope.InvalidFilter="INVALID";

                }


                }


                    var filterIsValid = function() {
                        if ($scope.filterData.selectedStartDate.trim() =="" && $scope.filterData.selectedEndDate.trim()=="")
            return false;
    if($scope.filterData.selectedDocument.trim() =="" ||$scope.filterData.selectedUsers.length==0|| $scope.filterData.selectedActivities.length==0)
{
return false;
}
        return true;
                    }

    var buildTableStructure = function () {

        var PopulateColumns = function () {
            if ($scope.displayedWorkflowList != undefined) {
                if ($scope.displayedWorkflowList.length > 0) {
            //Reterive all columns
                //first Clear columns list
                    if ($scope.displayedWorkflowNames.length > 0)
                        $scope.displayedWorkflowNames =[];


    }
    }
            //populate columns list
            var name;
            var i;
            $.each($scope.filterData.selectedActivities, function (index, workflowActObj) {

                name = workflowActObj.trim().toCamel();

                //Check if already exists or not?
                (i = $scope.displayedWorkflowNames.indexOf(name)) < 0 ? $scope.displayedWorkflowNames.push(name) : $scope.displayedWorkflowNames.splice(i, 1);
            });

        };

        var PopulateRows = function () {
            if ($scope.displayedWorkflowContent != undefined) {
                if ($scope.displayedWorkflowContent.length > 0) {

                    $scope.displayedWorkflowContent = [];

                }
            }

            var name;

            var tableIndex = 1;

            var userActivityname;

            $.each($scope.displayedWorkflowList, function (index, workflowListObj) {

                var name = workflowListObj.name.trim().toCamel();;


                var userActivityCountArray = [];
                var rowCount = [];
                var found = false;


                $.each($scope.filterData.selectedActivities, function (index, activityCount) {
                    //BUILD USER X ACTIVITY COUNT
                    //iterate the activity lookup
                    found = false;
                    userActivityname = activityCount.trim().toCamel();
                    $.each(workflowListObj.dashboardDataEntries, function (index, headerActivityName) {
                        //iterate activity count

                        //check if the selected header activity name is equal to the activitycount name


                        if(headerActivityName.activityName == userActivityname) {
                            rowCount.push(headerActivityName.activityCount);
                            found = true;
                        }


                    });
                    if (!found)
                    { rowCount.push(0); }
                });


                userActivityCountArray.push(rowCount);

                var rowObj =
                    {
                        index: tableIndex++,
                        userName: name,
                        activityAndCount: rowCount

                    };

                $scope.displayedWorkflowContent.push(rowObj);


                console.log(rowObj)

            });
        }

        PopulateColumns();

        PopulateRows();

    }

    $scope.applyFilter = function () {

        getDataEntryStat_();

    }


    var getDataEntryStat_ = function () {

        if(filterIsValid())
        {


                        $scope.InvalidFilter= "DEFAULT";

        DataServices.getDataEntryStat($scope.filterData.selectedStartDate, $scope.filterData.selectedEndDate, $scope.filterData.selectedDocument,
$scope.filterData.selectedUsers, $scope.filterData.selectedActivities).then(function (result) {

                        $scope.InvalidFilter="VALID";

     $scope.displayedWorkflowList=[];
        $scope.displayedWorkflowList = result.data;

        buildTableStructure();

});
}
else
{
                            $scope.InvalidFilter="INVALID";


                }


}

    $scope.onWorkflowChange= function()
    {
        populateActivityCombo($scope.filterData.selectedDocument);


    }

    $scope.init();

})


            .factory('DataServices', ["$http", function ($http) {


                return {


                    getAllTeamUsers: function () {

                        return $http.get(urlGetAllTeamUsers);



                    },
                    getAllWorkflows: function () {


                        return $http.get(urlGetAllWorkflows);



                    },
                    getAllStateTemplates: function () {


                        return $http.get(urlGetAllStateTemplate);



                    },

                    getAllStateTemplate: function (workflowName) {


                        return $http.get(urlGetAllStateTemplate_ + workflowName);



                    },
                    getDataEntryStat: function ( _startDate,  _endDate, _workflowDefs, _wfusers,  _activities) {

                        var config = {
                            params: {
                                startDate: _startDate,
                                endDate: _endDate,
                                workflowDefs: _workflowDefs,
                                wfusers: _wfusers,
                                activities: _activities
                            }
                        };                        return $http.get(urlGetDataEntryStat, config);



                    }


                }

            }])

            .service('PopulateColumnsAndHeaderNames', [function ($scope) {

                var PopulateColumns = function () {
                    if ($scope.displayedWorkflowList.length > 0) {
                        //Reterive all columns
                        //Clear columns list
                        $scope.displayedWorkflowNames.splice(0, A.length);
                        var i;


                        //populate columns list
                        var name;
                        $.each($scope.displayedWorkflowList, function (index, workflowActObj) {

                            name = workflowActObj.Activity.Trim().toCamel();

                            //Check if already exists or not?
                            (i = $scope.displayedWorkflowNames.indexOf(name)) < 0 ? $scope.displayedWorkflowNames.push(name) : $scope.displayedWorkflowNames.splice(i, 1);
                        });
                    }
                };


            }]).filter('columnSum', function () {
                return function (data,columnName) {
                    var sum = 0;

                    angular.forEach(data[0].dashboardDataEntries, function (value) {
                        if (value.activityName == columnName)
                      
                            sum = sum + parseInt(value.activityCount);
                    });
               

                    return sum;
                };
            }).filter('rowSum', function () {
                return function (data) {        
                    if (angular.isUndefined(data) && angular.isUndefined(key))
                        return 0;        
                    var sum = 0;        
                    angular.forEach(data,function(value){
                        sum = sum + parseInt(value);
                    });        
                    return sum;
                };
            });


