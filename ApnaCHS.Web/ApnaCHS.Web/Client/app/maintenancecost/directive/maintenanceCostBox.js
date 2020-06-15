app.directive('maintenanceCostBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', '$stateParams', '$rootScope', 'appConstant', 'localStorageService', '$window', 'authService', 'societyService',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, $stateParams, $rootScope, appConstant, localStorageService, $window, authService, societyService) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/maintenancecost/directive/maintenanceCostBox.html',
            replace: true,
            transclude: true,
            scope: {
                societyId: '=',
                isDash: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.recordList = [];
                scope.formData = {};
                scope.facilityRecords = [];
                scope.definitionList = [];
                scope.todayDate = new Date();
                scope.searchFormData = {};
                scope.note = null;
                scope.commentTableName = 'MC';
                getList();
                getUDFList();

                //function getUDFList() {

                //    var promise = $http.post(DATA_URLS.GET_MASTERVALUELIST, appConstant.MasterType.MaintenanceCostDefinition);

                //    promise.then(function (data) {
                //        if (!data.status || data.status == 200) {
                //            scope.definitionList = data;
                //        }
                //    });
                //}

                function getUDFList() {

                    var promise = $http.post(DATA_URLS.LIST_COSTDEFINATION);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.definitionList = data;
                        }
                    });
                }

                scope.search = function () {
                    getList();
                }

                function setfilters() {
                    var searchFilter = {};

                    var soc = societyService.getSociety();
                    searchFilter.societyId = soc.id;

                    if (scope.isDash) {
                        searchFilter.isApproved = true;
                        searchFilter.isActive = true;
                        searchFilter.isDeleted = false;
                    }
                    else {
                        searchFilter.isApproved = scope.searchFormData.isApproved;
                        searchFilter.isRejected = scope.searchFormData.isRejected;
                    }
                    return searchFilter;
                }

                scope.clearSearch = function () {
                    scope.searchFormData = {};
                    scope.searchFormData.isApproved = "";

                    scope.search();
                }

                function getList() {

                    var filters = setfilters();
                    var promise = $http.post(DATA_URLS.LIST_MAINTENANCECOSTS, filters);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.recordList = data;

                            angular.forEach(scope.recordList, function (v1) {
                                v1.userApproved = !!($filter('filter')(v1.approvals, { approvedBy: authService.authentication.loginData.userName }, true)[0]);
                            });
                        }
                    });
                }

                scope.new = function () {
                    scope.selectedItem = null;
                    scope.formData = null;

                    if (scope.isDash) {
                        $location.path('/maintenancecost/list');
                    }
                    else {
                        $('#hidddenNew').click();
                    }
                }

                scope.editMC = function (item) {
                    scope.formData = angular.copy(item);
                    scope.formData.definition = $filter('filter')(scope.definitionList, { id: scope.formData.definition.id })[0];
                    if (scope.formData.fromDate) {
                        scope.formData.fromDate = new Date($filter('date')(scope.formData.fromDate, appConstant.ConversionDateFormat));
                    }
                    if (scope.formData.toDate) {
                        scope.formData.toDate = new Date($filter('date')(scope.formData.toDate, appConstant.ConversionDateFormat));
                    }
                    scope.formData.date = new Date($filter('date')(scope.formData.date, appConstant.ConversionDateFormat));

                    scope.formData.isEdit = true;
                }

                scope.cancelEdit = function () {
                    scope.formData.isEdit = false;
                    scope.formData = null;
                }

                scope.onDelete = function (item, index) {
                    scope.selectedItem = angular.copy(item);
                    scope.selectedItem.index = index;
                }

                scope.delete = function (item, index) {

                    var promise = $http.post(DATA_URLS.DELETE_MAINTENANCECOST, item.id);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            toastr["success"]("Maintenance cost deleted", "Success");
                            if (!scope.recordList[index].isApproved) {
                                scope.recordList.splice(scope.selectedItem.index, 1);
                            }
                            else {
                                scope.recordList[index].deleted = true;
                            }
                            $('#cancelConfirmDeleteMCL').click();
                        }
                    });
                }

                scope.onRestore = function (item, index) {
                    scope.selectedItem = angular.copy(item);
                    scope.selectedItem.index = index;
                }

                scope.restore = function (item, index) {

                    var promise = $http.post(DATA_URLS.RESTORE_MAINTENANCECOST, scope.selectedItem.id);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            toastr["success"]("Maintenance cost restored", "Success");
                            scope.recordList[scope.selectedItem.index].deleted = false;
                            $('#cancelConfirmRestore').click();
                        }
                    });
                }

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.society = { id: scope.societyId };
                    fdata.fromDate = $filter('date')(fdata.fromDate, appConstant.ConversionDateFormat);

                    if (fdata.toDate) {
                        fdata.toDate = $filter('date')(fdata.toDate, appConstant.ConversionDateFormat);
                    }

                    var promise = $http.post(fdata.isEdit ? DATA_URLS.UPDATE_MAINTENANCECOST : DATA_URLS.NEW_MAINTENANCECOST, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            toastr["success"](scope.formData.isEdit ? "Maintenance cost updated" : "New Maintenance cost created successfully", "Success");
                            $('#cancelNewMainCostBox').click();
                            getList();
                        }
                    });
                }

                scope.submitEndDate = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.society = { id: scope.societyId };

                    fdata.toDate = $filter('date')(fdata.toDate, appConstant.ConversionDateFormat);
                    var promise = $http.post(DATA_URLS.UPDATEENDDATE_MAINTENANCECOST, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            toastr["success"]("Maintenance cost updated", "Success");
                            $('#cancelEditEndDateBox').click();
                            getList();
                        }
                    });
                }

                scope.onApprove = function (item, index) {
                    scope.selectedItem = item;
                    scope.selectedItem.index = index;
                }


                scope.approve = function () {

                    var fdata = { id: scope.selectedItem.id, note: scope.note };
                    var promise = $http.post(DATA_URLS.APPROVE_MAINTENANCECOST, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            // scope.recordList[scope.selectedItem.index].isApproved = data.isApproved;
                            scope.recordList[scope.selectedItem.index].isApproved = true;
                            toastr["success"]("Maintenance cost approved by you", "Success");
                            $('#cancelConfirmApproveBox').click();
                        }
                    });
                }

                scope.onReject = function (item, index) {
                    scope.selectedItem = item;
                    scope.selectedItem.index = index;
                }

                scope.reject = function (item, index) {
                    var fdata = { id: scope.selectedItem.id, note: scope.note };
                    var promise = $http.post(DATA_URLS.REJECT_MAINTENANCECOST, fdata);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            scope.recordList[scope.selectedItem.index].isRejected = true;
                            toastr["success"]("Maintenance cost rejected", "Success");
                            $('#cancelConfirmReject').click();
                        }
                        else {
                            scope.recordList[index].isRejected = false;
                        }
                    });
                }

                scope.assignFlats = function (item, index) {
                    scope.selectedMCLine = item;
                    loadFacilities(scope.societyId);
                }

                function loadFacilities(id) {

                    var promise = $http.post(DATA_URLS.LOADFLATS_FACILITIES, { societyId: id });

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.facilityRecords = data;

                            angular.forEach(scope.facilityRecords, function (v1) {
                                angular.forEach(v1.floors, function (v2) {
                                    angular.forEach(v2.flats, function (v3) {

                                        var flat = $filter('filter')(scope.selectedMCLine.flats, { id: v3.id }, true)[0];
                                        if (flat) {
                                            v3.selected = true;
                                        }
                                    });
                                });
                            });
                        }
                    });
                }

                scope.allFlatsInFacility = function (item) {
                    angular.forEach(item.floors, function (v) {
                        angular.forEach(v.flats, function (v1) {
                            v1.selected = true;
                        });
                    });
                }

                scope.clearAllFlatsInFacility = function (item) {
                    angular.forEach(item.floors, function (v) {
                        angular.forEach(v.flats, function (v1) {
                            v1.selected = false;
                        });
                    });
                }

                scope.allFlatsInFloor = function (item) {
                    angular.forEach(item.flats, function (v1) {
                        v1.selected = true;
                    });
                }

                scope.clearAllFlatsInFloor = function (item) {
                    angular.forEach(item.flats, function (v1) {
                        v1.selected = false;
                    });
                }

                scope.updateFlats = function () {

                    scope.assignedFlats = [];
                    scope.unassignedFlats = [];
                    scope.updateFlatsProgress = true;

                    angular.forEach(scope.facilityRecords, function (v1) {
                        angular.forEach(v1.floors, function (v2) {
                            angular.forEach(v2.flats, function (v3) {

                                var flat = $filter('filter')(scope.selectedMCLine.flats, { id: v3.id }, true)[0];
                                if (v3.selected) {
                                    if (!flat) {
                                        scope.assignedFlats.push({ id: v3.id });
                                    }
                                }
                                else {
                                    if (flat) {
                                        scope.unassignedFlats.push({ id: v3.id });
                                    }
                                }
                            });
                        });
                    });

                    var fdata = {
                        mcid: scope.selectedMCLine.id,
                        assignedFlats: scope.assignedFlats,
                        unassignedFlats: scope.unassignedFlats
                    };

                    var promise = $http.post(DATA_URLS.ASSIGNED_FLATS, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            getList();
                            $('#cancelAssignFlatsBox').click();
                            toastr["success"]("Flat assigned to " + scope.selectedMCLine.definition.name, "Success");
                            scope.selectedMCLine = {};
                        }
                    });
                }

                scope.toggleFlatSelection = function (flat) {
                    flat.selected = !flat.selected;
                }

                scope.onBackClick = function () {
                    $window.history.back();
                    //$location.path('facility/details/' + $scope.editFormData.facility.id);
                }

                scope.selectedItem = null;
                scope.onCommentClick = function (mc) {
                    scope.selectedItem = angular.copy(mc);
                }
            }
        };
    }]);
