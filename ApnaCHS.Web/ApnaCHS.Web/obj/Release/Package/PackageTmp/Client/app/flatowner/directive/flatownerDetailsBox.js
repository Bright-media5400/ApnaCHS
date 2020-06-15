app.directive('flatownerDetailsBox', ['$http', 'DATA_URLS', '$window', '$location', '$anchorScroll', '$filter', '$stateParams', '$rootScope', 'appConstant', 'localStorageService', '$timeout',
    function ($http, DATA_URLS, $window, $location, $anchorScroll, $filter, $stateParams, $rootScope, appConstant, localStorageService, $timeout) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flatowner/directive/flatownerDetailsBox.html',
            replace: true,
            transclude: true,
            scope: {
                editFormData: '=', //this object stores flat owner 
                flatId: '=',
                flatOwnerType: '=',
                memberSinceDate: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.genderList = [];
                scope.relationshipList = [];
                scope.recordList = [];
                scope.vehicleList = [];


                getRelationshipList();
                getGenderList();

                scope.todayDate = new Date();

                scope.$watch(function (scope) { return scope.editFormData },
                              function (newValue, oldValue) {
                                  if (!!newValue && newValue != oldValue) {
                                      getFamilyList();
                                      getVehicleList();
                                  }
                              });

                function getFamilyList() {
                    var soc = JSON.parse(localStorageService.get('onLogSoc'));
                    var fdata = { flatOwnerId: scope.editFormData.id, societyId: soc.id };
                    var promise = $http.post(DATA_URLS.LIST_FLATOWNERFAMILIES, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.recordList = data;
                        }
                    });
                }

                function getVehicleList() {
                    var soc = JSON.parse(localStorageService.get('onLogSoc'));
                    var fdata = { flatId: scope.flatId, flatOwnerId: scope.editFormData.id, societyId: soc.id };
                    var promise = $http.post(DATA_URLS.LIST_VEHICLES, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.vehicleList = data;
                        }
                    });
                }

                function getGenderList() {
                    var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 8);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.genderList = data;
                        }
                    });
                };

                function getRelationshipList() {
                    var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 9);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.relationshipList = data;
                        }
                    });
                };

                scope.onBackClick = function () {
                    $window.history.back();
                }

                //Edit Current Owner Family
                scope.editFlatOwnerFamily = function (flatOwnerFamiliy) {
                    scope.editOwnerFamily = angular.copy(flatOwnerFamiliy);

                    if (scope.editOwnerFamily.dateOfBirth)
                        scope.editOwnerFamily.dateOfBirth = new Date($filter('date')(scope.editOwnerFamily.dateOfBirth, appConstant.ConversionDateFormat));
                    scope.editOwnerFamily.gender = $filter('filter')(scope.genderList, { id: scope.editOwnerFamily.gender.id })[0];
                    scope.editOwnerFamily.relationship = $filter('filter')(scope.relationshipList, { id: scope.editOwnerFamily.relationship.id })[0];
                    scope.editOwnerFamily.isEdit = true;
                    scope.editOwnerFamily.comments = null;
                }

                scope.editCurrentOwnerFamily = function () {

                    var fdata = angular.copy(scope.editOwnerFamily);
                    var promise = $http.post(DATA_URLS.UPDATE_FLATOWNERFAMILY, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            toastr["success"]("Flat Owner Family updated successfully", "Success");
                            $('#cancelEditFlatOwnerFamilyBox').click();
                            $rootScope.$broadcast("onOwnerFamilyChanged", data);
                            getFamilyList();
                        }
                    });
                }

                //clear Family Member Function

                scope.cancelFamilyMember = function () {
                    scope.formData = null;
                };

                // Add current Flat Owner Family Member

                scope.submitFlatOwnerFamily = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.flatOwner = { id: scope.editFormData.id };
                    fdata.dateOfBirth = $filter('date')(fdata.dateOfBirth, appConstant.ConversionDateFormat);

                    var promise = $http.post(DATA_URLS.NEW_FLATOWNERFAMILY, fdata);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            toastr["success"]("Family Member registered successfully", "Success");
                            $('#cancelFlatOwnerFamility').click();
                            $rootScope.$broadcast("onOwnerFamilyChanged", data);
                            getFamilyList();
                            scope.formData = null;
                        }
                    });
                }

                //delete vehicle

                scope.onDelete = function (vehicle) {
                    scope.selectedVehicle = vehicle;
                }

                scope.delete = function (vehicle) {
                    var promise = $http.post(DATA_URLS.DELETE_VEHILCE, scope.selectedVehicle.id);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            toastr["success"]("Vehicle deleted successfully", "Success");
                            $('#cancelRemoveVehicleBox').click();
                            $rootScope.$broadcast("onVehicleDelete", data);
                            getVehicleList();
                        }
                    });
                }

                //delete Owner family member

                scope.onDeleteOwnerMember = function (flatOwnerFamiliy) {
                    scope.selectedOwnerMember = flatOwnerFamiliy;
                }

                scope.deleteOwnerMember = function (flatOwnerFamiliy) {
                    var promise = $http.post(DATA_URLS.DELETE_FLATOWNERFAMILY, scope.selectedOwnerMember.id);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            toastr["success"]("Owner Family Member deleted successfully", "Success");
                            $('#cancelRemoveOwnerFamilyBox').click();
                            $rootScope.$broadcast("onOwnerFamiliyDelete", data);
                            getFamilyList();
                        }
                    });
                }

                scope.approve = function () {
                    var soc = JSON.parse(localStorageService.get('onLogSoc'));
                    var fdata = { flatOwner: scope.editFormData.id, societyId: soc.id, note: scope.note };
                    var promise = $http.post(DATA_URLS.APPROVE_FLATOWNER, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $('#cancelConfirmApproveBox').click();
                            toastr["success"]("Flat owner approved", "Success");
                            scope.editFormData.isApproved = true;
                        }
                    });
                }

            }
        }
    }]);