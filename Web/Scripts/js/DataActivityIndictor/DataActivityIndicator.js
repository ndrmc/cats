/*
 * TODO:
 * I need to refactor this  file to separate  js files of service , factory and directive.
 * refactor the variable names,make mnemonic
 * Add comments
 *
 */


DataEntryIndicatorApp = angular.module("DataEntryIndicatorApp", []).controller('WorkflowActController', function WorkflowActController($scope, DataServices, $http, $timeout) {




    $scope.displayedUserNames = [];//ROW STARTERS
    $scope.displayedWorkflowNames = [];//FOR THE COLUMNS
    $scope.displayedWorkflows = [];
    $scope.displayedWorkflowContent = [];
    $scope.displayedWorkflowContentColumns = [];
    $scope.displayedWorkflowList = [];
    $scope.delay = 1;
    $scope.ctrlStatus =
    {
        loadingUser: true,
        loadingActivity: true,
        loadingDocument: true
    };
    $scope.viewGraphXAxis = [];

    $scope.searching = false;

    $scope.isBusy = false;
    $scope.subtitleText;

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

    $scope.openDetail = function(userName, indexOfActivity)
    {

        var selectedActivityName = $scope.displayedWorkflowNames[indexOfActivity];

        $.each($scope.displayedWorkflowList, function (index, workflowListObj) {

            var name = workflowListObj.name.trim().toCamel();;

            $.each(workflowListObj.dashboardDataEntries, function (index, dashBoardEntry) {

                if (dashBoardEntry.activityName == selectedActivityName && name == userName) {
                    console.log(buildDetailMsg(dashBoardEntry.detail));
                    return false;
                }

            })
        })


    }

    function buildDetailMsg(detailData)
    {

        var index = 1;
        var msg = "";
        var breakIndicator = "\n *------------------------------------------------------------* \n";
        var separator = " => ";

        angular.forEach(detailData, function (valueDetail, detail) {
            msg = msg+(index++).toString()+") ";
            angular.forEach(valueDetail, function (value, key) {
                msg = msg + key + separator;
                msg = msg + value + " , ";
            });
            msg = msg + breakIndicator;

        });

        return msg;

      }

    function getStartDate() {

        return $("#startDateDataIndicator").val();
    }

    function getEndDate() {


        return $("#endDateDataIndicator").val();
    }

    $scope.calcColumnTotal = function (columnName) {
        console.log(columnName);
        var sum = 0;
        angular.forEach($scope.displayedWorkflowList, function (users) {
            angular.forEach(users.dashboardDataEntries, function (wf) {
                var userActivityname = wf.activityName.trim().toCamel();
                if (columnName == userActivityname) {

                    sum = sum + parseInt(wf.activityCount);

                }


            });


        });
        return sum;
    };

    $scope.getSubTitleText = function () {

        if ($scope.selectedStartDate == undefined && $scope.selectedEndDate != undefined)
            return "After Date " + $scope.selectedEndDate;

        if ($scope.selectedStartDate != undefined && $scope.selectedEndDate == undefined)
            return "Before Date " + $scope.selectedStartDate;

        if ($scope.selectedStartDate == undefined && $scope.selectedEndDate == undefined)
            return "";

        var result = $scope.selectedStartDate + " - " + $scope.selectedEndDate;
        return result;
    }

    $scope.init = function () {

        var initializeControls = function () {


            $scope.ctrlStatus.loadingUser = true;
            DataServices.getAllTeamUsers().then(function (result) {

                $scope.controlData.lookupUsers = result.data;

                $scope.ctrlStatus.loadingUser = false;

                useLabels: false
                $("#User").Xdropdown({
                    useLabels: false
                });


            });




            $scope.ctrlStatus.loadingDocument = true;

            DataServices.getAllWorkflows().then(function (result) {

                $scope.controlData.lookupDocument = result.data;

                $scope.ctrlStatus.loadingDocument = false;

                if ($scope.controlData.lookupDocument.length > 0) {
                    $scope.filterData.selectedDocument = $scope.controlData.lookupDocument[0].name;


                    populateActivityCombo($scope.filterData.selectedDocument);
                }



                var dd = $("#Document").Xdropdown({
                    useLabels: false
                });

                dd.Xdropdown('set text', $scope.filterData.selectedDocument);
                dd.Xdropdown('set value', $scope.filterData.selectedDocument);

            });



        };

        var setFilterControlDefaultValue = function () {

            var now = new Date();

            var aWeekBefore = new Date(new Date().setDate(new Date().getDate() - 5));

            $scope.filterData.selectedStartDate = aWeekBefore.toLocaleDateString();

            $scope.filterData.selectedEndDate = now.toLocaleDateString();

        };


        initializeControls();

        setFilterControlDefaultValue();

        // getDataEntryStat_();



    }

    var populateActivityCombo = function (selectedWorkflow) {

        if (selectedWorkflow == null) return;

        $scope.ctrlStatus.loadingActivity = true;

        DataServices.getAllStateTemplate(selectedWorkflow).then(function (result) {
            $scope.controlData.lookupActivities = result.data;

            $scope.filterData.selectedActivities = [];


            $("#Activity").Xdropdown({
                //placeholder: 'Your placeholder',
                useLabels: false
            });
            $("#Activity").Xdropdown("restore defaults");

            $scope.ctrlStatus.loadingActivity = false;

        });


    };

    $scope.InvalidFilter = "DEFAULT";

    var getDataEntryStat = function () {

        if (filterIsValid()) {

            $scope.InvalidFilter = "DEFAULT";
            $scope.filterData.selectedStartDate = getStartDate();
            $scope.filterData.selectedEndDate = getEndDate();

            DataServices.getDataEntryStat($scope.filterData.selectedStartDate, $scope.filterData.selectedEndDate, $scope.filterData.selectedDocument,
            $scope.filterData.selectedUsers, $scope.filterData.selectedActivities).then(function (result) {

                $scope.InvalidFilter = "VALID";

                $scope.displayedWorkflowList = [];
                $scope.displayedWorkflowList = result.data;


            });

        }
        else {
            $scope.InvalidFilter = "INVALID";

        }


    }

    var filterIsValid = function () {

        if ($scope.filterData.selectedStartDate.trim() == "" && $scope.filterData.selectedEndDate.trim() == "")
            return false;
        if ($scope.filterData.selectedDocument.trim() == "" || $scope.filterData.selectedUsers.length == 0 || $scope.filterData.selectedActivities.length == 0)
            return false;

        return true;
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
            $scope.subtitleText = $scope.getSubTitleText();

            $scope.viewGraphXAxis = [];
            $.each($scope.filterData.selectedActivities, function (index, workflowActObj) {

                name = workflowActObj.trim().toCamel();
                $scope.viewGraphXAxis.push(name);
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

                        if (headerActivityName.activityName == userActivityname) {

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
                        name: name,
                        data: rowCount

                    };

                $scope.displayedWorkflowContent.push(rowObj);



            });
        }

        PopulateColumns();

        PopulateRows();

    }

    $scope.applyFilter = function () {

        $scope.searching = true;

        getDataEntryStat_();

        //$scope.drawGraph();


    }

    $scope.drawGraph = function () {
        var chart;
        var container = "containerGraph";
        chart = new Highcharts.Chart({
            chart: {
                renderTo: container,
                type: 'column'
            },


            title: {
                text: 'Data Entry Users Comparison'
            },
            subtitle: {
                text: $scope.subtitleText
            },
            credits: {
                enabled: false
            },
            exporting: {
                enabled: true
            },
            xAxis: {
                categories: $scope.viewGraphXAxis
            },
            yAxis: {
                allowDecimals: false,
                min: 0,
                title: {
                    text: 'Activity count'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y} </b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    stacking: null

                }
            },
            series:
               $scope.displayedWorkflowContent

        });

    }

    var getDataEntryStat_ = function () {

        if (filterIsValid()) {


            $scope.InvalidFilter = "DEFAULT";

            $scope.filterData.selectedStartDate = getStartDate();
            $scope.filterData.selectedEndDate = getEndDate();


            DataServices.getDataEntryStat($scope.filterData.selectedStartDate, $scope.filterData.selectedEndDate,
            $scope.filterData.selectedDocument, $scope.filterData.selectedUsers, $scope.filterData.selectedActivities)
            .then(function (result) {

                $scope.InvalidFilter = "VALID";
                $scope.displayedWorkflowList = [];
                $scope.displayedWorkflowList = result.data;

                buildTableStructure();

                $scope.drawGraph();

                $scope.searching = false;



            });
        }
        else {
            $scope.InvalidFilter = "INVALID";

            $scope.searching = false;

        }

    }

    $scope.onWorkflowChange = function () {
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
                    getDataEntryStat: function (_startDate, _endDate, _workflowDefs, _wfusers, _activities) {

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
                            (i = $scope.displayedWorkflowNames.indexOf(name)) < 0 ?
                            $scope.displayedWorkflowNames.push(name) : $scope.displayedWorkflowNames.splice(i, 1);

                        });
                    }
                };


            }]).filter('columnSum', function () {
                return function (data, columnName) {
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
                    angular.forEach(data, function (value) {
                        sum = sum + parseInt(value);
                    });
                    return sum;
                };
            })

.directive('drawChart', function () {
    return function (scope, element, attrs) {

        var container = $(element).attr("id");

        scope.$watch('displayedworkflowcontent', function () {
            drawplot();

        }, true);

        var drawplot = function () {
            var chart;
            chart = new Highcharts.Chart({
                chart: {
                    renderTo: container,
                    type: 'column'
                },


                title: {
                    text: 'Data Entry Users Comparison'
                },
                subtitle: {
                    text: scope.subtitleText
                },
                credits: {
                    enabled: false
                },
                exporting: {
                    enabled: true
                },
                xAxis: {
                    categories: scope.viewGraphXAxis
                },
                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    title: {
                        text: 'Activity count'
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y} </b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        stacking: null

                    }
                },
                series:
                   scope.displayedWorkflowContent

            });
        };
    };
}).directive('spinner',
            function ($timeout) {
                return {
                    restrict: 'E',
                    template: '<i class="fa fa-cog fa-spin"></i>',
                    scope: {
                        searching: '=',
                        delay: '@'
                    },
                    link: function (scope, elem, attrs) {
                        var showTimer;

                        scope.$watch("searching", function (newVal) {
                            newVal ? showSpinner() : hideSpinner();
                        });

                        function showSpinner() {

                            if (showTimer) return;

                            showTimer = $timeout(showElement.bind(this, true), getDelay());
                        }

                        function hideSpinner() {

                            if (showTimer) {
                                $timeout.cancel(showTimer);
                            }

                            showTimer = null;

                            showElement(false);
                        }

                        function showElement(show) {
                            show ? elem.css({ display: '' }) : elem.css({ display: 'none' });
                        }

                        function getDelay() {
                            var delay = parseInt(scope.delay);

                            return isNaN(delay) ? 200 : delay;
                        }
                    }
                };
            }
);