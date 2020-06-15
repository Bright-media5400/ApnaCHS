app.controller('flatDetailsController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', '$timeout', 'localStorageService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, $timeout, localStorageService) {

    'use strict';
    $scope.editFormData = {};
    $scope.formData = {};
    $scope.tenantsRecords = [];
    $scope.flatOwnersRecords = [];
    $scope.currentFlatOwner = null;
    $scope.currentTenant = null;
    $scope.societyId = null;
    $scope.flatId = null;
    $scope.recordList = [];
    $scope.selectedFlatOwner = null;
    $scope.todayDate = new Date();
    $scope.parkingTypes = appConstant.ParkingType;

    getUDFList();
    getGenderList();

    if ($routeParams.fid != undefined) {
        $scope.societyId = $routeParams.sid;
        $scope.flatId = $routeParams.fid;

        get($routeParams.fid);
    }

    //delete flat

    $scope.onDeleteFlat = function (editFormData) {
        $scope.selectedFlat = editFormData;
    }

    $scope.deleteFlat = function (editFormData) {
        var promise = $http.post(DATA_URLS.DELETE_FLAT, $scope.selectedFlat.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Flat deleted successfully", "Success");

                //$window.history.back();
                $timeout(function () {
                    $location.path('/floor/details/' + editFormData.floor.id);
                }, 500);
            }
        });
    }

    //delete flat owners

    $scope.onDelete = function (udf) {
        $scope.selectedOwner = udf;
    }

    $scope.delete = function (udf) {

        var promise = $http.post(DATA_URLS.DELETE_FLATOWNER, $scope.selectedOwner.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Flat Owner deleted successfully", "Success");
                $('#cancelRemoveOwnerBox').click();
                get($routeParams.fid);
            }
        });
    }

    //delete tenant

    $scope.onDeleteTenant = function (udf) {
        $scope.selectedTenant = udf;
    }

    $scope.deleteTenant = function (udf) {
        var promise = $http.post(DATA_URLS.DELETE_FLATOWNER, $scope.selectedTenant.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Tenant deleted successfully", "Success");
                $('#cancelRemoveTenantBox').click();
                get($routeParams.fid);
            }
        });
    }

    $scope.onParkingDelete = function (parking) {
        $scope.selectedParking = parking;
    }

    $scope.deleteParking = function () {
        var promise = $http.post(DATA_URLS.DELETE_FLATPARKING, $scope.selectedParking.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Parking unassigned successfully", "Success");
                $('#cancelRemoveParkingBox').click();
                $scope.facilityRecords = null;
                get($scope.editFormData.id);
            }
        });
    }

    function getUDFList() {

        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 1);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.udfList = data;

            }
        });
    };

    function getGenderList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 8);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.genderList = data;
            }
        });
    };

    //Edit Flat Onwer
    $scope.submitFlatOwner = function () {

        var fdata = angular.copy($scope.editFlatOwner);
        var promise = $http.post(DATA_URLS.UPDATE_FLATOWNER, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Flat Owner updated successfully", "Success");
                $('#cancelEditFlatOwnerBox').click();
                get($routeParams.fid);
            }
        });
    }

    $scope.editOwner = function (udf) {
        $scope.editFlatOwner = angular.copy(udf.flatOwner);
        $scope.editFlatOwner.dateOfBirth = new Date($filter('date')($scope.editFlatOwner.dateOfBirth, appConstant.ConversionDateFormat));
        //$scope.editFlatOwner.memberSinceDate = new Date($filter('date')($scope.editFlatOwner.memberSinceDate, appConstant.ConversionDateFormat));
        //$scope.editFlatOwner.memberTillDate = new Date($filter('date')($scope.editFlatOwner.memberTillDate, appConstant.ConversionDateFormat));
        $scope.editFlatOwner.gender = $filter('filter')($scope.genderList, { id: $scope.editFlatOwner.gender.id })[0];
        $scope.editFlatOwner.flatOwnerType = udf.flatOwnerType;
    }

    // Update Owner Till Date

    $scope.submitOwnerTillDate = function () {

        var fdata = angular.copy($scope.editFlatOwner);
        fdata = { flat: { id: $scope.flatId }, flatOwner: { id: $scope.editFlatOwner.id }, memberTillDate: $filter('date')($scope.editFlatOwner.memberTillDate, appConstant.ConversionDateFormat) };
        var promise = $http.post(DATA_URLS.UPDATE_OWNERTILLDATE, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Flat Owner Till Date updated successfully", "Success");
                $('#cancelFlatOwnerTillDateBox').click();
                get($routeParams.fid);
            }
        });
    }

    // update owner since date

    $scope.editSinceDate = function (udf) {
        $scope.editOwnerSinceDate = angular.copy(udf);
        $scope.editOwnerSinceDate.memberSinceDate = new Date($filter('date')($scope.editOwnerSinceDate.memberSinceDate, appConstant.ConversionDateFormat));
    }

    $scope.submitOwnerSinceDate = function () {

        var fdata = angular.copy($scope.editOwnerSinceDate);
        fdata = { flat: { id: $scope.flatId }, flatOwner: { id: $scope.editOwnerSinceDate.flatOwner.id }, memberSinceDate: $filter('date')($scope.editOwnerSinceDate.memberSinceDate, appConstant.ConversionDateFormat) };
        var promise = $http.post(DATA_URLS.UPDATE_OWNERSINCEDATE, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Member Since Date updated successfully", "Success");
                $('#cancelSinceDateBox').click();
                get($routeParams.fid);
            }
        });
    }

    //Edit Tenant

    $scope.submitCurrentTenat = function () {

        var fdata = angular.copy($scope.editCurrentTenant);
        var promise = $http.post(DATA_URLS.UPDATE_TENANT, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Tenant updated successfully", "Success");
                $('#cancelEditTenantBox').click();
                get($routeParams.fid);
            }
        });
    }

    //Update Tenant Till Date

    $scope.submitTenatTillDate = function () {

        var fdata = angular.copy($scope.editCurrentTenant);
        var promise = $http.post(DATA_URLS.UPDATE_TENANTTILLDATE, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Tenant Till Date updated successfully", "Success");
                $('#cancelTenantTillDateBox').click();
                get($routeParams.fid);
            }
        });
    }

    $scope.editTenant = function (udf) {
        $scope.editCurrentTenant = angular.copy(udf);
        $scope.editCurrentTenant.dateOfBirth = new Date($filter('date')($scope.editCurrentTenant.dateOfBirth, appConstant.ConversionDateFormat));
        $scope.editCurrentTenant.memberSinceDate = new Date($filter('date')($scope.editCurrentTenant.memberSinceDate, appConstant.ConversionDateFormat));
        $scope.editCurrentTenant.memberTillDate = new Date($filter('date')($scope.editCurrentTenant.memberTillDate, appConstant.ConversionDateFormat));
        $scope.editCurrentTenant.gender = $filter('filter')($scope.genderList, { id: $scope.editCurrentTenant.gender.id })[0];
        $scope.editCurrentTenant.isEdit = true;
    }

    //Edit Flat

    $scope.submit = function () {

        var fdata = angular.copy($scope.editFlat);
        var promise = $http.post(DATA_URLS.UPDATE_FLAT, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Flat updated successfully", "Success");
                $scope.cancelEdit();
                $scope.editFlat = null;
                get($routeParams.fid);
            }
        });
    }

    $scope.edit = function () {
        $scope.editFlat = angular.copy($scope.editFormData);
        if ($scope.editFlat.flatType != null) {
            $scope.editFlat.flatType = $filter('filter')($scope.udfList, { id: $scope.editFlat.flatType.id })[0];
        }
        $scope.editFlat.isEdit = true;
    }

    $scope.cancelEdit = function (editFormData) {
        $scope.editFlat = null;
    }


    function get(id) {

        var promise = $http.post(DATA_URLS.GET_FLAT, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;
                $scope.editFormData.flatOwners.sort(sort_by({ name: 'memberSinceDate', reverse: true }));

                var owners = $scope.editFormData.flatOwners.filter(function (e) {
                    return e.flatOwnerType == 1;
                });
                $scope.currentFlatOwner = owners[0];

                //var tenants = $scope.editFormData.flatOwners.filter(function (e) {
                //    return e.flatOwnerType == 2;
                //});
                //$scope.currentTenant = tenants[0];
            }
        });
    }

    var onVehicleCreate = $rootScope.$on('onVehicleCreate', function (e, d) {
        toastr["success"]("vehicle details created successfully", "Success");
        $.each($('[id=cancelvehicleBox]'), function (k, v) {
            v.click();
        });
        get($routeParams.fid);
    });

    var onFlatOwnerCreate = $rootScope.$on('onFlatOwnerCreate', function (e, d) {

        toastr["success"]("Flat owner created successfully", "Success");

        $.each($('[id=cancelNewFlatOwnerBox]'), function (k, v) {
            v.click();
        });
        get($routeParams.fid);
    });

    var onTenantCreate = $rootScope.$on('onTenantCreate', function (e, d) {

        toastr["success"]("Tenant created successfully", "Success");
        $('#cancelNewTenantBox').click();
        get($routeParams.fid);
    });

    var onOwnerFamilyChanged = $rootScope.$on('onOwnerFamilyChanged', function (e, d) {
        get($routeParams.fid);
    });

    var onTenantFamilyChanged = $rootScope.$on('onTenantFamilyChanged', function (e, d) {
        get($routeParams.fid);
    });

    var onVehicleDelete = $rootScope.$on('onVehicleDelete', function (e, d) {
        get($routeParams.fid);
    });

    var onTenantFamiliyDelete = $rootScope.$on('onTenantFamiliyDelete', function (e, d) {
        get($routeParams.fid);
    });

    var onOwnerFamiliyDelete = $rootScope.$on('onOwnerFamiliyDelete', function (e, d) {
        get($routeParams.fid);
    });

    $scope.$on('$destroy', function () {
        onVehicleCreate();
        onFlatOwnerCreate();
        onTenantCreate();
        onOwnerFamilyChanged();
        onTenantFamilyChanged();
        onVehicleDelete();
        onTenantFamiliyDelete();
        onOwnerFamiliyDelete();
    });

    $scope.onBackClick = function () {
        $window.history.back();
        //$location.path('floor/details/' + $scope.editFormData.floor.id);
    }

    $scope.assignParkings = function () {
        loadFacilities($scope.editFormData.id);
    }

    function loadFacilities(id) {

        var promise = $http.post(DATA_URLS.LOADPARKINGS_FACILITIES, { flatId: id });

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.facilityRecords = data;

                angular.forEach($scope.facilityRecords, function (v1) {
                    angular.forEach(v1.floors, function (v2) {
                        angular.forEach(v2.flatParkings, function (v3) {

                            v3.selected = (v3.flatId == $scope.editFormData.id);
                        });
                    });

                    angular.forEach(v1.flatParkings, function (v2) {
                        v2.selected = (v2.flatId == $scope.editFormData.id);
                    });

                });

            }
        });
    }

    $scope.allParkingsInFacility = function (item) {
        angular.forEach(item.floors, function (v) {
            angular.forEach(v.flatParkings, function (v1) {
                if (v1.flatId == null || v1.flatId == $scope.editFormData.id) {
                    v1.selected = true;
                }
            });
        });

        angular.forEach(item.flatParkings, function (v) {
            if (v.flatId == null || v.flatId == $scope.editFormData.id) {
                v.selected = true;
            }
        });
    }

    $scope.clearAllParkingsInFacility = function (item) {

        angular.forEach(item.floors, function (v) {
            angular.forEach(v.flatParkings, function (v1) {
                if (v1.flatId == null || v1.flatId == $scope.editFormData.id) {
                    v1.selected = false;
                }
            });
        });

        angular.forEach(item.flatParkings, function (v) {
            if (v.flatId == null || v.flatId == $scope.editFormData.id) {
                v.selected = false;
            }
        });
    }

    $scope.allParkingsInFloor = function (item) {
        angular.forEach(item.flatParkings, function (v1) {
            if (v1.flatId == null || v1.flatId == $scope.editFormData.id) {
                v1.selected = true;
            }
        });
    }

    $scope.clearAllParkingsInFloor = function (item) {
        angular.forEach(item.flatParkings, function (v1) {
            if (v1.flatId == null || v1.flatId == $scope.editFormData.id) {
                v1.selected = false;
            }
        });
    }

    $scope.toggleParkingSelection = function (parking) {

        if (parking.flatId == $scope.editFormData.id) return;
        parking.selected = !parking.selected;
    }

    $scope.updateFlats = function () {

        $scope.assignedParkings = [];
        $scope.updateFlatsProgress = true;

        angular.forEach($scope.facilityRecords, function (v1) {

            angular.forEach(v1.flatParkings, function (v2) {

                if (v2.selected) {
                    $scope.assignedParkings.push(v2.id);
                }
            });

            angular.forEach(v1.floors, function (v2) {
                angular.forEach(v2.flatParkings, function (v3) {

                    if (v3.selected) {
                        $scope.assignedParkings.push(v3.id);
                    }
                });
            });
        });

        if ($scope.assignedParkings.length == 0) {
            toastr["success"]("No parking selected", "Info");
            return;
        }
        var fdata = {
            flatid: $scope.editFormData.id,
            parkingids: $scope.assignedParkings
        };

        var promise = $http.post(DATA_URLS.ASSIGN_FLATPARKING, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $('#assignParkingBox').click();
                toastr["success"]("Parking assigned to " + $scope.editFormData.name, "Success");
                get($scope.editFormData.id);
            }
        });
    }

    $scope.onClickParking = function (parking) {
        $scope.selectedParking = parking;
    }

    $scope.removeParkingBox = function (parking) {

    }

    $scope.showParkingText = function (type) {
        //return text.substring(0, 12) + '...';
        if (type) {
            return $filter('filter')($scope.parkingTypes, { id: type })[0].text;
        }

    }

    $scope.approveFlatOwner = function () {

    }
}]);