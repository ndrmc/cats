
angular.module(AngularAppName, []).controller('WorkflowActController', function WorkflowActController($scope, DataServices, $http) {




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

    $scope.init = function () {


        var initializeControls = function () {

            DataServices.getAllTeamUsers().then(function (result) {

                $scope.controlData.lookupUsers = result.data;


                //$('#User').multiselect({

                //    templates: {

                //        li: '<li><div class="checkbox"><label></label></div></li>'
                //    }

                //});



            });



            DataServices.getAllWorkflows().then(function (result) {

                $scope.controlData.lookupDocument = result.data;


                if ($scope.controlData.lookupDocument.length > 0)
                { $scope.filterData.selectedDocument = $scope.controlData.lookupDocument[0].name; }


                populateActivityCombo($scope.filterData.selectedDocument);



                buildTableStructure();
                //$('#Document').multiselect({

                //    templates: {

                //        li: '<li><div class="checkbox"><label></label></div></li>'
                //    }

                //});
            });


            DataServices.getAllStateTemplate().then(function (result) {

                $scope.controlData.lookupActivities = result.data;


                //$('#Activities').multiselect({

                //    templates: {

                //        li: '<li><div class="checkbox"><label></label></div></li>'
                //    }

                //});
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

        getDataEntryStat();



    }

    var populateActivityCombo=function(selectedWorkflow){
        DataServices.getAllStateTemplate(selectedWorkflow).then(function (result) {
            $scope.controlData.lookupActivities = result.data;
            buildTableStructure();
        });
    };

    var getDataEntryStat = function () {

        DataServices.getDataEntryStat($scope.filterData.selectedStartDate, $scope.filterData.selectedEndDate, $scope.filterData.selectedDocument,
$scope.filterData.selectedUsers, $scope.filterData.selectedActivities).then(function (result) {

    $scope.displayedWorkflowList = result.data;

});
    }

    var buildTableStructure = function () {

        var PopulateColumns = function () {
            if ($scope.displayedWorkflowList != undefined) {
                if ($scope.displayedWorkflowList.length > 0) {
                    //Reterive all columns
                    //first Clear columns list
                    if ($scope.displayedWorkflowNames.length > 0)
                        $scope.displayedWorkflowNames = [];


                }
            }
            //populate columns list
            var name;
            var i;
            $.each($scope.controlData.lookupActivities, function (index, workflowActObj) {

                name = workflowActObj.name.trim().toCamel();

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


                $.each(workflowListObj.dashboardDataEntries, function (index, activityCount) {
                    //BUILD USER X ACTIVITY COUNT
                    //iterate the activity lookup
                    found = false;
                    userActivityname = activityCount.activityName.trim().toCamel();
                    $.each($scope.controlData.lookupActivities, function (index, headerActivityName) {
                        //iterate activity count

                        //check if the selected header activity name is equal to the activitycount name


                        if (headerActivityName.name == userActivityname) {
                            rowCount.push(activityCount.activityCount);
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

        getDataEntryStat();

        buildTableStructure();
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
                        };
                        return $http.get(urlGetDataEntryStat, config);



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


            }]);


