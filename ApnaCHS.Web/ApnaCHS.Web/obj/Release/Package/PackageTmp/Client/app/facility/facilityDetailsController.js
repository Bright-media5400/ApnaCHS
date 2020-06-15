app.controller('facilityDetailsController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService) {

    'use strict';
    $scope.editFormData = {};
    $scope.parkingFormData = {};
    $scope.floorFormData = {};
    $scope.recordList = [];
    $scope.totalFlatsInFacility = 0;

    $scope.flatTypes = [];
    $scope.floorRecords = [];
    $scope.facilityId = null;
    $scope.floorId = null;
    $scope.parkingTypes = appConstant.ParkingType;
    $scope.floorList = null;

    $scope.newFloorClick = false;

    getUDFList();
    getFlatTypes();

    $scope.isFloorVisible = false;
    $scope.FloorTypes = appConstant.FloorType;
    $scope.FacilityType = appConstant.FacilityType;

    if ($routeParams.fid != undefined) {
        $scope.facilityId = $routeParams.fid;

        get($routeParams.fid);
        getList();
    }

    function getUDFList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 2);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.udfList = data;
            }
        });
    };

    function getList() {
        var fdata = { facilityId: $scope.facilityId };
        var promise = $http.post(DATA_URLS.LIST_SOCIETYASSETS, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.editFormData);
        //patchy code. Do not remove
        fdata.societies = null;
        fdata.floors = null;
        fdata.flatParkings = null;
        fdata.societyAssets = null;

        var promise = $http.post(DATA_URLS.UPDATE_FACILITY, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Facility updated successfully", "Success");
                get($scope.editFormData.id);
            }
        });
    }

    $scope.edit = function () {
        $scope.editFormData.isEdit = true;
    }


    $scope.cancelEdit = function () {
        $scope.editFormData.isEdit = false;
    }

    $scope.submitParking = function () {

        var fdata = angular.copy($scope.selectedParking);
        fdata.type = fdata.type.id;
        var promise = $http.post(DATA_URLS.UPDATE_FLATPARKING, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Parking updated successfully", "Success");
                $('#cancelEditFlatParkingBox').click();

                $scope.editFormData.flatParkings[$scope.selectedParking.index].type = $scope.selectedParking.type;
                $scope.editFormData.flatParkings[$scope.selectedParking.index].name = $scope.selectedParking.name;
                $scope.selectedParking = null;
            }
        });
    }

    $scope.onSelectParking = function (flatParking, index) {
        $scope.selectedParking = angular.copy(flatParking);
        if ($scope.selectedParking.type) {
            $scope.selectedParking.type = $filter('filter')($scope.parkingTypes, { id: $scope.selectedParking.type.id })[0];
        }
        $scope.selectedParking.isEdit = true;
        $scope.selectedParking.index = index;
    }


    function get(id) {

        var promise = $http.post(DATA_URLS.GET_FACILITY, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;

                getTotalFlats(data.id);

                if ($scope.editFormData.type == 1) {

                    var sdata = angular.copy($scope.editFormData.societies[0].society);
                    sdata['complex'] = $scope.editFormData.complex;

                    localStorageService.set('onLogSoc', JSON.stringify(sdata));
                }
                
                angular.forEach($scope.editFormData.floors, function (v) {
                    v.floorType = $filter('filter')($scope.FloorTypes, { id: v.type }, true)[0];
                });
                angular.forEach($scope.editFormData.flatParkings, function (v) {
                    if (v.type) {
                        v.type = $filter('filter')($scope.parkingTypes, { id: v.type }, true)[0];
                    }
                });
                $scope.facilityType = $filter('filter')($scope.FacilityType, { id: data.type }, true)[0];
                $scope.isFloorVisible = isFloor($scope.editFormData.type);
            }
        });
    }

    function getTotalFlats(id) {

        var promise = $http.post(DATA_URLS.GET_FACILITY_FLATCOUNT, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.totalFlatsInFacility = data;
            }
        });
    }

    var onFlatParkingCreate = $rootScope.$on('onFlatParkingCreate', function (e, d) {
        get($scope.editFormData.id);
    });

    $scope.onBackClick = function () {
        $window.history.back();
    }

    function isFloor(type) {
        return type == 1 || type == 4 || type == 6 || type == 7 || type == 9 || type == 12 || type == 13 || type == 14 || type == 15;
    }

    $scope.createMultipleParkings = function () {
        var fdata = angular.copy($scope.parkingFormData);
        fdata.facilityId = $scope.editFormData.id;

        var promise = $http.post(DATA_URLS.MULTIPLE_FLATPARKING, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.parkingFormData = {};
                get($scope.editFormData.id);
                $('#cancelnewFlatParkingBox').click();
            }
        });
    }

    //$scope.createMultipleFloors = function () {
    //    var fdata = angular.copy($scope.floorFormData);
    //    fdata.facilityId = $scope.editFormData.id;
    //    fdata.type = fdata.type.id;

    //    var promise = $http.post(DATA_URLS.MULTIPLE_FLOOR, fdata);

    //    promise.then(function (data) {
    //        if (!data.status || data.status == 200) {
    //            $scope.floorFormData = {};
    //            get($scope.editFormData.id);
    //            $('#cancelNewFloorBox').click();
    //        }
    //    });
    //}

    $scope.createMultipleFloors = function () {

        var floors = [];
        var errormsg = null;

        if ($scope.floorFormData.totalFloors != $scope.floorList.length) {
            errormsg = errormsg + 'Total count mismatch';
        }

        var totalFlats = 0;
        angular.forEach($scope.floorList, function (v) {
            totalFlats = totalFlats + (v.flats + v.commercialspaces);

            var flats = 0
            var inerrormsg = null;

            angular.forEach(v.flatList, function (v1) {
                flats = flats + v1.noOfFlats;
            });
            if (v.flats && flats != v.flats) {
                inerrormsg = 'Flats';
            }

            var cs = 0
            angular.forEach(v.commercialspaceList, function (v1) {
                cs = cs + v1.noOfFlats;
            });
            if (v.commercialspaces && cs != v.commercialspaces) {
                if (inerrormsg)
                    inerrormsg = inerrormsg + ',Commercial Spaces';
                else {
                    inerrormsg = 'Commercial Spaces';
                }
            }

            var parkings = 0
            angular.forEach(v.parkingList, function (v1) {
                parkings = parkings + v1.noofparkings;
            });
            if (v.parkings && parkings != v.parkings) {
                if (inerrormsg)
                    inerrormsg = inerrormsg + ',Parkings';
                else {
                    inerrormsg = 'Parkings';
                }
            }

            if (inerrormsg) {
                if (errormsg) {
                    errormsg = errormsg + '<br />' + inerrormsg + ' configuration mismatch for floor - ' + v.name;
                }
                else {
                    errormsg = inerrormsg + ' configuration mismatch for floor - ' + v.name;
                }
            }


            var fdata = angular.copy(v);
            fdata.facility = { id: $scope.editFormData.id };
            fdata.type = fdata.type.id;
            fdata.flats = [];
            fdata.flatParkings = [];

            if (v.flatList && v.flatList.length > 0) {
                angular.forEach(v.flatList, function (f) {
                    for (var i = 0; i < f.noOfFlats; i++) {

                        var fcopy = angular.copy(f);
                        fcopy.flatTypeId = fcopy.flatTypeId.id;
                        fdata.flats.push(fcopy);
                    }
                });
            }

            if (v.commercialspaceList && v.commercialspaceList.length > 0) {
                angular.forEach(v.commercialspaceList, function (cs) {
                    for (var i = 0; i < cs.noOfFlats; i++) {

                        var cscopy = angular.copy(cs);
                        cscopy.flatTypeId = cscopy.flatTypeId.id;
                        cscopy.isCommercialSpace = true;
                        fdata.flats.push(cscopy);
                    }
                });
            }

            if (v.parkingList && v.parkingList.length > 0) {
                angular.forEach(v.parkingList, function (p) {
                    for (var i = 0; i < p.noofparkings; i++) {

                        var pcopy = angular.copy(p);
                        pcopy.type = pcopy.type.id;
                        fdata.flatParkings.push(pcopy);
                    }
                });
            }

            floors.push(fdata);
        });

        if (totalFlats > $scope.editFormData.noOfFlats) {
            if (errormsg) {
                errormsg = errormsg + '<br />Total flats count mismatch';
            }
            else {
                errormsg = 'Total count mismatch';
            }
        }

        $scope.errormsg = errormsg;

        if ($scope.errormsg) { return; }

        //console.log('floors - ', floors);
        var promise = $http.post(DATA_URLS.MULTIPLE_FLOOR, { floors: floors });

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Floor added successfully", "Success");
                $scope.onCancelNewFloorBox();
                get($scope.editFormData.id);
            }
        });
    }

    $scope.onLinkSociety = function () {
        getSocities();
    }

    function getSocities() {
        var fdata = { complexId: $scope.editFormData.complex.id };
        var promise = $http.post(DATA_URLS.LIST_SOCIETIES, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.societyList = data;

                angular.forEach($scope.societyList, function (v) {
                    var soc = $filter('filter')($scope.editFormData.societies, { society: { id: v.id } }, true)[0];
                    if (soc) {
                        v.selected = true;
                    }
                });

            }
        });
    }

    $scope.toggleSocietySelection = function (society) {
        society.selected = !society.selected;
    }

    $scope.updateSocieties = function () {

        var linkSocieties = [];
        var unlinkSocieties = [];

        angular.forEach($scope.societyList, function (v) {

            var soc = $filter('filter')($scope.editFormData.societies, { society: { id: v.id } }, true)[0];
            if (!soc && v.selected) {
                linkSocieties.push(v.id);
            }
        });

        angular.forEach($scope.societyList, function (v) {
            var soc = $filter('filter')($scope.editFormData.societies, { society: { id: v.id } }, true)[0];
            if (soc && !v.selected) {
                unlinkSocieties.push(v.id);
            }
        });

        var fdata = { facilityId: $scope.editFormData.id, linkSocieties: linkSocieties, unlinkSocieties: unlinkSocieties };
        var promise = $http.post(DATA_URLS.LINKSOCIETIES_FACILITIES, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {

                get($scope.editFormData.id);
                $('#cancelLinkSocietiesBox').click();
            }
        });
    }

    var onSocirtyAssetCreate = $rootScope.$on('onSocirtyAssetCreate', function (e, d) {
        toastr["success"]("Asset created successfully", "Success");
        $('#cancelNewAssetBox').click();
        getList()
    });

    $scope.editAsset = function (item) {
        $scope.editSocietyAsset = angular.copy(item);

        if ($scope.editSocietyAsset.purchaseDate)
            $scope.editSocietyAsset.purchaseDate = new Date($filter('date')($scope.editSocietyAsset.purchaseDate, appConstant.ConversionDateFormat));
    }

    $scope.submitAsset = function () {

        var fdata = angular.copy($scope.editSocietyAsset);
        var promise = $http.post(DATA_URLS.UPDATE_SOCIETYASSET, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Asset updated successfully", "Success");
                $('#cancelAssetBox').click();
                getList();
            }
        });
    }

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_SOCIETYASSET, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Assets deleted successfully", "Success");
                $scope.recordList.splice($scope.selectedItem.index, 1);
                $('#cancelConfirmDelete').click();
            }
        });

    }

    $scope.$on('$destroy', function () {

        onFlatParkingCreate();
        onSocirtyAssetCreate();
    });

    $scope.onFloorsChanged = function () {

        if (!$scope.floorList) {

            if (($scope.editFormData.noOfFloors - $scope.editFormData.floors.length + 1) < $scope.floorFormData.totalFloors) return;

            $scope.floorList = [];
            var noOfFloors = $scope.floorFormData.totalFloors;

            for (var i = 0; i < noOfFloors; i++) {
                $scope.floorList.push({});
            }
        }
        else {

            if (($scope.editFormData.noOfFloors - $scope.editFormData.floors.length + 1) < $scope.floorFormData.totalFloors) return;

            var noOfFloors = $scope.floorFormData.totalFloors;
            var floorno = $scope.floorList.length + $scope.editFormData.floors.length - 1;

            if (floorno < noOfFloors + $scope.editFormData.floors.length) {
                for (var i = 0; i < noOfFloors - $scope.floorList.length; i++) {
                    $scope.floorList.push({});
                }
            }
            else {
                $scope.floorList.splice(noOfFloors, $scope.floorList.length - noOfFloors);
            }
        }

        for (var i = 0; i < $scope.floorList.length; i++) {

            var no = $scope.editFormData.floors.length + i;

            $scope.floorList[i].floorNumber = no;
            $scope.floorList[i].name = no == 0 ? 'Ground Floor' : 'Floor ' + no.toString();
            //$scope.floorList[i].wing = $scope.floorList[i].display + ' ' + $scope.floorList[i].srno
        }
    }

    $scope.onNewClick = function () {
        $scope.newFloorClick = true;
        $scope.errormsg = null;
    }

    $scope.onCancelNewFloorBox = function () {
        $scope.newFloorClick = false;
        $scope.errormsg = null;

        $scope.floorList = [];
        $scope.floorFormData = {};
    }

    $scope.selectedFloor = null;
    $scope.onFlatsChanged = function (floor) {
        if (floor.flats <= 0) {
            floor.flatList = [];
        }
    }

    $scope.onFlatsOpened = function (item, index) {

        $scope.selectedFloor = angular.copy(item);
        $scope.selectedFloor.index = index;

        if (!$scope.selectedFloor.flatList) {
            $scope.selectedFloor.flatList = [];
        }
        else {
            angular.forEach($scope.selectedFloor.flatList, function (v) {
                v.flatTypeId = $filter('filter')($scope.flatTypes, { id: v.flatTypeId.id }, true)[0];
            });
        }

        $scope.selectedFloor.valid = true;
    }

    $scope.onCommercialSpacesChanged = function (floor) {
        if (floor.commercialspaces <= 0) {
            floor.commercialspaceList = [];
        }
    }

    $scope.onCommercialSpacesOpened = function (item, index) {

        $scope.selectedFloor = angular.copy(item);
        $scope.selectedFloor.index = index;

        if (!$scope.selectedFloor.commercialspaceList) {
            $scope.selectedFloor.commercialspaceList = [];
        }
        else {
            angular.forEach($scope.selectedFloor.commercialspaceList, function (v) {
                v.flatTypeId = $filter('filter')($scope.flatTypes, { id: v.flatTypeId.id }, true)[0];
            });
        }

        $scope.selectedFloor.valid = true;
    }

    $scope.onParkingsChanged = function (floor) {

        if (floor.parkings <= 0) {
            floor.parkingList = [];
        }
    }

    $scope.onParkingsOpened = function (item, index) {

        $scope.selectedFloor = angular.copy(item);
        $scope.selectedFloor.index = index;

        if (!$scope.selectedFloor.parkingList) {
            $scope.selectedFloor.parkingList = [];
        }
        else {
            angular.forEach($scope.selectedFloor.parkingList, function (v) {
                v.type = $filter('filter')($scope.parkingTypes, { id: v.type.id }, true)[0];
            });
        }

        $scope.selectedFloor.valid = true;
    }

    $scope.onFloorTypeChange = function (item) {
        item.flats = 0;
        item.flatList = null;
        item.commercialspaces = 0;
        item.commercialspaceList = null;
        item.parkings = 0;
        item.parkingList = null;
    }

    $scope.onCancelConfigureFlats = function (selectedFloor) {
        selectedFloor.flatList = null;
    }

    $scope.validateConfigureFlats = function (selectedFloor) {
        selectedFloor.valid = true;

        var flats = 0
        angular.forEach(selectedFloor.flatList, function (v) {
            flats = flats + v.noOfFlats;
        });

        if (flats == selectedFloor.flats) {
            $scope.floorList[selectedFloor.index].flatList = selectedFloor.flatList;
            $('#cancelConfigureFlats').click();
        }
        else {
            selectedFloor.valid = false;
        }

    }

    $scope.onAddConfigureFlats = function (selectedFloor) {
        selectedFloor.flatList.push({});
    }

    $scope.onDeleteConfigureFlats = function (selectedFloor,index) {
        selectedFloor.flatList.splice(index, 1);
    }


    $scope.onCancelCommercialSpaces = function (selectedFloor) {
        selectedFloor.commercialspaceList = null;
    }

    $scope.validateConfigureCommercialSpaces = function (selectedFloor) {

        selectedFloor.valid = true;

        var flats = 0
        angular.forEach(selectedFloor.commercialspaceList, function (v) {
            flats = flats + v.noOfFlats;
        });

        if (flats == selectedFloor.commercialspaces) {
            $scope.floorList[selectedFloor.index].commercialspaceList = selectedFloor.commercialspaceList;
            $('#cancelConfigureCommercialSpaces').click();
        }
        else {
            selectedFloor.valid = false;
        }
    }

    $scope.onAddConfigureCommercialSpaces = function (selectedFloor) {
        selectedFloor.commercialspaceList.push({});
    }

    $scope.onDeleteConfigureCommercialSpaces = function (selectedFloor, index) {
        selectedFloor.commercialspaceList.splice(index, 1);
    }


    $scope.onCancelParkings = function (selectedFloor) {
        selectedFloor.parkingList = null;
    }

    $scope.validateParkings = function (selectedFloor) {
        selectedFloor.valid = true;

        var flats = 0
        angular.forEach(selectedFloor.parkingList, function (v) {
            flats = flats + v.noofparkings;
        });

        if (flats == selectedFloor.parkings) {
            $scope.floorList[selectedFloor.index].parkingList = selectedFloor.parkingList;
            $('#cancelConfigureParkings').click();
        }
        else {
            selectedFloor.valid = false;
        }
    }

    $scope.onAddConfigureParkings = function (selectedFloor) {
        selectedFloor.parkingList.push({});
    }

    $scope.onDeleteConfigureParkings = function (selectedFloor, index) {
        selectedFloor.parkingList.splice(index, 1);
    }

    $scope.copyFloor = function (currIndex) {
        if (currIndex != 0) {

            $scope.floorList[currIndex].type = $scope.floorList[currIndex - 1].type;
            $scope.floorList[currIndex].flats = $scope.floorList[currIndex - 1].flats;
            $scope.floorList[currIndex].flatList = $scope.floorList[currIndex - 1].flatList;
            $scope.floorList[currIndex].commercialspaces = $scope.floorList[currIndex - 1].commercialspaces;
            $scope.floorList[currIndex].commercialspaceList = $scope.floorList[currIndex - 1].commercialspaceList;
            $scope.floorList[currIndex].parkings = $scope.floorList[currIndex - 1].parkings;
            $scope.floorList[currIndex].parkingList = $scope.floorList[currIndex - 1].parkingList;
        }
    }

    function getFlatTypes() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 1);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.flatTypes = data;
            }
        });
    };

    $scope.onSocietyDashboard = function () {
        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        $location.path('/society/details/' + soc.id);
    }
}]);;