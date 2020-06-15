app.controller('newComplexController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', 'appConstant', '$filter',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, appConstant, $filter) {

    'use strict';
    $scope.uploadedSocietyList = [];
    $scope.complexType = appConstant.ComplexType;
    $scope.FacilityTypes = appConstant.FacilityType;
    $scope.buildingList = null;
    $scope.gateList = null;
    $scope.wingList = null;
    $scope.societyList = null;

    $scope.formData = {};
    $scope.formData.withLift = false;

    getCityList();
    getStateList();


    $scope.todayDate = new Date();

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        fdata.dateOfIncorporation = $filter('date')(fdata.dateOfIncorporation, appConstant.ConversionDateFormat);
        fdata.dateOfRegistration = $filter('date')(fdata.dateOfRegistration, appConstant.ConversionDateFormat);
        fdata.type = fdata.type.id;
        fdata.facilities = [];

        var isbreak = false;
        angular.forEach($scope.wingList, function (v) {
            if (fdata.type == 1) {

                //single society
                fdata.noOfSocieties = 1;

                if (v.building) {
                    fdata.facilities.push({ wing: v.wing, name: v.building.name + '|' + fdata.societyName, type: 1, noOfLifts: v.noOfLifts, noOfFloors: v.noOfFloors, noOfFlats: v.noOfFlats });
                }
                else {
                    alert('For all wings, building name should be selected');
                    isbreak = true;
                    return;
                }
            }
            else {

                //multiple society
                if (v.building && v.society) {
                    fdata.facilities.push({ wing: v.wing, name: v.building.name + '|' + v.society.name, type: 1, noOfLifts: v.noOfLifts, noOfFloors: v.noOfFloors, noOfFlats: v.noOfFlats });
                }
                else {
                    alert('For all wings, building and society name should be selected');
                    isbreak = true;
                    return;
                }
            }
        });

        if (isbreak) return;

        fdata.societyAssets = [];
        angular.forEach(fdata.facilities, function (v) {
            if (v.noOfLifts > 0) {
                for (var i = 1; i <= v.noOfLifts; i++) {
                    fdata.societyAssets.push({ name: "Lift " + i, facility: v });
                }
            }

        });

        angular.forEach($scope.FacilityTypes, function (v) {
            if (v.selected && v.count > 0) {
                angular.forEach(v.List, function (v1) {
                    fdata.facilities.push({ name: v1.name, type: v.id });
                });
            }
        });



        fdata.societies = [];
        angular.forEach($scope.societyList, function (v) {
            fdata.societies.push({ name: v.name });
        });

        angular.forEach($scope.gateList, function (v) {
            fdata.societyAssets.push({ name: v.name });
        });

        fdata.noOfBuilding = fdata.noOfWings;

        console.log('fdata - ', fdata);
        //return;

        $scope.formSubmitted = true;
        var promise = $http.post(DATA_URLS.NEW_COMPLEX, fdata);

        promise.then(function (data) {

            $scope.formSubmitted = false;

            if (!data.status || data.status == 200) {
                toastr["success"]("Complex registered successfully", "Success");

                if (fdata.bulkIndex == null) {
                    $location.path('/complex/list');

                    angular.forEach($scope.FacilityTypes, function (v) {
                        v.selected = false;
                        v.count = null;
                    });
                }
                else {

                    $scope.uploadedSocietyList.splice(fdata.bulkIndex, 1);

                    $scope.buildingList = null;
                    $scope.gateList = null;
                    $scope.wingList = null;
                    $scope.societyList = null;
                    $scope.formData = {};
                    $scope.formData.withLift = false;

                    angular.forEach($scope.FacilityTypes, function (v) {
                        v.selected = false;
                        v.count = null;
                    });
                }
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    function getCityList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 5);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.cityList = data;
            }
        });
    };

    function getStateList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 6);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.stateList = data;
            }
        });
    };

    //$scope.onUploadExcel = function () {
    //    $('#file-upload').click();
    //}

    $scope.uploadComplexList = function (files) {

        // Get The File From The Input
        var oFile = files[0];
        var sFilename = oFile.name;
        // Create A File Reader HTML5
        var reader = new FileReader();


        // Ready The Event For When A File Gets Selected
        reader.onload = function (e) {
            var data = e.target.result;
            var cfb = XLSX.read(data, { type: 'binary', cellDates: true, cellNF: false, cellText: false });

            cfb.SheetNames.forEach(function (sheetName) {

                var oJS = XLS.utils.sheet_to_json(cfb.Sheets[sheetName]);
                console.log('oJS - ', oJS);

                var i = 1;
                angular.forEach(oJS, function (v) {
                    v.type = v['Complex Type'];
                    v.societyName = v['Society Name'];
                    v.contactPerson = v['Contact Person'];
                    v.phoneNo = v['Mobile No'];
                    v.email = v['Email'];
                    v.registrationNo = v['Registration No'];
                    v.name = v['Complex Name'];
                    v.address = v['Address'];
                    v.pincode = v['Pincode'];
                    v.area = v['Area'];
                    v.city = v['City'];
                    v.state = v['State'];
                    v.dateOfIncorporation = v['Date Of Incorporation (MM/DD/YYYY)'] && typeof v['Date Of Incorporation (MM/DD/YYYY)'].getMonth === 'function' ? v['Date Of Incorporation (MM/DD/YYYY)'].setDate(v['Date Of Incorporation (MM/DD/YYYY)'].getDate() + 1) : v['Date Of Incorporation (MM/DD/YYYY)'];
                    v.dateOfRegistration = v['Date Of Registration  (MM/DD/YYYY)'] && typeof v['Date Of Registration  (MM/DD/YYYY)'].getMonth === 'function' ? v['Date Of Registration  (MM/DD/YYYY)'].setDate(v['Date Of Registration  (MM/DD/YYYY)'].getDate() + 1) : v['Date Of Registration  (MM/DD/YYYY)'];
                    v.noOfSocieties = v['No Of Societies'];
                    v.societynames = v['Society Names'];
                    v.noOfBuilding = v['No Of Building'];
                    v.buildingnames = v['Building Names'];
                    v.noOfWings = v['No Of Wings'];
                    v.wingnames = v['Wing Names'];
                    v.noOfAmenities = v['No Of Amenities'];
                    v.noOfGate = v['No Of Gates'];
                    v.gatenames = v['Gate Names'];
                    v.id = i++;
                });

                $scope.uploadedSocietyList = oJS;
                $scope.resultAvailable = false;

                var phase = $rootScope.$$phase;
                if (!(phase === '$apply' || phase === '$digest')) {
                    $rootScope.$apply();
                }
            });
        };

        // Tell JS To Start Reading The File.. You could delay this if desired
        reader.readAsBinaryString(oFile);
    }

    $scope.uploadExcel = function () {

        var fdata = angular.copy($scope.uploadedSocietyList);
        var promise = $http.post(DATA_URLS.IMPORT_SOCIETIES, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Import successfull", "Success");

                angular.forEach($scope.uploadedSocietyList, function (v) {
                    var rowItem = $filter('filter')(data, { id: v.id })[0];

                    if (rowItem) {
                        v.result = rowItem.result;
                        v.isSuccess = rowItem.isSuccess;
                    }
                });
            }
        });
    }

    $scope.onSelectClick = function (item, $index) {
        item.selected = !item.selected;

        $scope.buildingList = null;
        $scope.gateList = null;
        $scope.wingList = null;
        $scope.societyList = null;


        var fdata = angular.copy(item);

        fdata.type = $filter('filter')($scope.complexType, { text: fdata.type }, true)[0];
        fdata.city = $filter('filter')($scope.cityList, { text: fdata.city }, true)[0];
        fdata.state = $filter('filter')($scope.stateList, { text: fdata.state }, true)[0];

        //fdata.dateOfIncorporation = new Date(fdata.dateOfIncorporation.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        fdata.dateOfIncorporation = fdata.dateOfIncorporation.getMonth === 'function' ? fdata.dateOfIncorporation : new Date($filter('date')(fdata.dateOfIncorporation, appConstant.ConversionDateFormat))
        //fdata.dateOfRegistration = new Date(fdata.dateOfRegistration.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        fdata.dateOfRegistration = fdata.dateOfRegistration.getMonth === 'function' ? fdata.dateOfRegistration : new Date($filter('date')(fdata.dateOfRegistration, appConstant.ConversionDateFormat))

        fdata.noOfSocieties = parseInt(fdata.noOfSocieties);
        if (fdata.noOfSocieties > 0) {
            $scope.societyList = [];
            $scope.societynames = fdata.societynames.split(',');
            for (var i = 0; i < fdata.noOfSocieties; i++) {
                var name = typeof $scope.societynames[i] === 'undefined' ? 'Society' + (i + 1).toString() : $scope.societynames[i];
                $scope.societyList.push({ name: name, display: name });
            }
            for (var i = 0; i < $scope.societyList.length; i++) {
                $scope.societyList[i].srno = i + 1;
            }
        }

        fdata.noOfBuilding = parseInt(fdata.noOfBuilding);
        if (fdata.noOfBuilding > 0) {
            $scope.buildingList = [];
            $scope.buildingnames = fdata.buildingnames.split(',');
            for (var i = 0; i < fdata.noOfBuilding; i++) {
                var name = typeof $scope.buildingnames[i] === 'undefined' ? 'Building ' + (i + 1).toString() : $scope.buildingnames[i];
                $scope.buildingList.push({ name: name, display: name, society: null });
            }
            for (var i = 0; i < $scope.buildingList.length; i++) {
                $scope.buildingList[i].srno = i + 1;
            }
        }

        fdata.noOfWings = parseInt(fdata.noOfWings);
        if (fdata.noOfWings > 0) {
            $scope.wingList = [];
            $scope.wingnames = fdata.wingnames.split(',');
            for (var i = 0; i < fdata.noOfWings; i++) {

                if (typeof $scope.wingnames[i] === 'undefined') {
                    var name = 'Wing' + (i + 1).toString();
                    $scope.wingList.push({ wing: name, display: name, society: null });
                }
                else {
                    var name = $scope.wingnames[i].split(':');

                    var building = null, society = null, wing = null, lifts = null, floors = null, flats = null;

                    if (typeof name[0] !== 'undefined') {
                        wing = name[0];
                    }

                    if (typeof name[1] !== 'undefined') {
                        society = $filter('filter')($scope.societyList, { name: name[1] }, true)[0];
                    }

                    if (typeof name[2] !== 'undefined') {
                        building = $filter('filter')($scope.buildingList, { name: name[2] }, true)[0];
                    }

                    if (typeof name[3] !== 'undefined') {
                        lifts = parseInt(name[3]);
                    }

                    if (typeof name[4] !== 'undefined') {
                        floors = parseInt(name[4]);
                    }

                    if (typeof name[5] !== 'undefined') {
                        flats = parseInt(name[5]);
                    }

                    $scope.wingList.push({ society: society, building: building, wing: wing, display: wing, noOfLifts: lifts, noOfFloors: floors, noOfFlats: flats });
                }
            }
            for (var i = 0; i < $scope.wingList.length; i++) {
                $scope.wingList[i].srno = i + 1;
            }
        }

        fdata.noOfAmenities = parseInt(fdata.noOfAmenities);

        fdata.noOfGate = parseInt(fdata.noOfGate);
        if (fdata.noOfGate > 0) {
            $scope.gateList = [];
            $scope.gatenames = fdata.gatenames.split(',');
            for (var i = 0; i < fdata.noOfGate; i++) {
                var name = typeof $scope.gatenames[i] === 'undefined' ? 'Gate' + (i + 1).toString() : $scope.gatenames[i];
                $scope.gateList.push({ name: name, display: name, society: null });
            }
            for (var i = 0; i < $scope.gateList.length; i++) {
                $scope.gateList[i].srno = i + 1;
            }
        }

        fdata.bulkIndex = $index;
        $scope.formData = fdata;
    }

    $scope.onAmenitiesChanged = function (facility) {
        if (facility.selected) {
            facility.count = 1;
            $scope.onAmenitiesCountChanged(facility);
        }
        else {
            facility.count = null;
            facility.List = null;
        }
    }

    $scope.onAmenitiesCountChanged = function (facility) {

        var cnt = 0;
        angular.forEach($scope.FacilityTypes, function (v) {
            cnt = cnt + (v.count == undefined ? 0 : v.count);
        });

        if (cnt > $scope.formData.noOfAmenities) {

            alert('Max amenities allowed is ' + $scope.formData.noOfAmenities);

            facility.count = null;
            facility.selected = false;
        }
        else {

            if (!facility.List) {

                facility.List = [];

                for (var i = 0; i < facility.count; i++) {
                    facility.List.push({ name: facility.text, display: facility.text });
                }
            }
            else {
                if (facility.List.length < facility.count) {
                    for (var i = facility.List.length; i < facility.count; i++) {
                        facility.List.push({ name: facility.text, display: facility.text });
                    }
                }
                else {
                    facility.List.splice(facility.count, facility.List.length - facility.count);
                }
            }

            for (var i = 0; i < facility.List.length; i++) {
                facility.List[i].srno = i + 1;
                facility.List[i].name = facility.List[i].display + ' ' + facility.List[i].srno
            }
        }
    }

    $scope.onSelectFacility = function (facility) {
        $scope.selectedFacility = facility;
    }

    $scope.onBuildingChanged = function () {
        if (!$scope.buildingList) {

            $scope.buildingList = [];

            for (var i = 0; i < $scope.formData.noOfBuilding; i++) {
                $scope.buildingList.push({ name: 'Building', display: 'Building', society: null });
            }
        }
        else {
            if ($scope.buildingList.length < $scope.formData.noOfBuilding) {
                for (var i = $scope.buildingList.length; i < $scope.formData.noOfBuilding; i++) {
                    $scope.buildingList.push({ name: 'Building', display: 'Building', society: null });
                }
            }
            else {
                $scope.buildingList.splice($scope.formData.noOfBuilding, $scope.buildingList.length - $scope.formData.noOfBuilding);
            }
        }

        for (var i = 0; i < $scope.buildingList.length; i++) {
            $scope.buildingList[i].srno = i + 1;
            $scope.buildingList[i].name = $scope.buildingList[i].display + ' ' + $scope.buildingList[i].srno
        }
    }

    $scope.onGateChanged = function () {
        if (!$scope.gateList) {

            $scope.gateList = [];

            for (var i = 0; i < $scope.formData.noOfGate; i++) {
                $scope.gateList.push({ name: 'Gate', display: 'Gate', society: null });
            }
        }
        else {
            if ($scope.gateList.length < $scope.formData.noOfGate) {
                for (var i = $scope.gateList.length; i < $scope.formData.noOfGate; i++) {
                    $scope.gateList.push({ name: 'Gate', display: 'Gate', society: null });
                }
            }
            else {
                $scope.gateList.splice($scope.formData.noOfGate, $scope.gateList.length - $scope.formData.noOfGate);
            }
        }

        for (var i = 0; i < $scope.gateList.length; i++) {
            $scope.gateList[i].srno = i + 1;
            $scope.gateList[i].name = $scope.gateList[i].display + ' ' + $scope.gateList[i].srno
        }
    }

    $scope.onWingChanged = function () {
        if (!$scope.wingList) {

            $scope.wingList = [];

            for (var i = 0; i < $scope.formData.noOfWings; i++) {
                $scope.wingList.push({ wing: 'Wing', display: 'Wing', society: null });
            }
        }
        else {
            if ($scope.wingList.length < $scope.formData.noOfWings) {
                for (var i = $scope.wingList.length; i < $scope.formData.noOfWings; i++) {
                    $scope.wingList.push({ wing: 'Wing', display: 'Wing', society: null });
                }
            }
            else {
                $scope.wingList.splice($scope.formData.noOfWings, $scope.wingList.length - $scope.formData.noOfWings);
            }
        }

        for (var i = 0; i < $scope.wingList.length; i++) {
            $scope.wingList[i].srno = i + 1;
            $scope.wingList[i].wing = $scope.wingList[i].display + ' ' + $scope.wingList[i].srno
        }
    }

    $scope.onSocietyChanged = function () {
        if (!$scope.societyList) {

            $scope.societyList = [];

            for (var i = 0; i < $scope.formData.noOfSocieties; i++) {
                $scope.societyList.push({ name: 'Society', display: 'Society' });
            }
        }
        else {
            if ($scope.societyList.length < $scope.formData.noOfSocieties) {
                for (var i = $scope.societyList.length; i < $scope.formData.noOfSocieties; i++) {
                    $scope.societyList.push({ name: 'Society', display: 'Society' });
                }
            }
            else {
                $scope.societyList.splice($scope.formData.noOfSocieties, $scope.societyList.length - $scope.formData.noOfSocieties);
            }
        }

        for (var i = 0; i < $scope.societyList.length; i++) {
            $scope.societyList[i].srno = i + 1;
            $scope.societyList[i].name = $scope.societyList[i].display + ' ' + $scope.societyList[i].srno
        }
    }

    $scope.onTypeChange = function () {

        $scope.societyList = [];

        if ($scope.formData.type.id == 1) {
            //single society
            $scope.formData.noOfSocieties = 1;
            $scope.societyList.push({ name: $scope.formData.societyName, srno: 1 });
            $scope.formData.name = $scope.formData.societyName;
        }
        else {
            //multiple society
            $scope.formData.noOfSocieties = null;
        }

    }

    $scope.areaData = {};
    $scope.areaList = [];

    $scope.searchArea = function () {
        if ($scope.formData.pincode.length != 6) {
            $scope.areaData = {};
            $scope.areaList = [];
            $scope.formData.area = null;
            $scope.formData.city = null;
            $scope.formData.state = null;
            return;
        }


        var promise = $http.post(DATA_URLS.READ_AREA, $scope.formData.pincode);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                if (data.length == 0) {
                    toastr["info"]("No information found with entered pincode. Please contact administrator for further details", "Success");
                    return;
                }

                if (data.length == 1) {

                    $scope.areaList = [];
                    $scope.areaData = data[0];
                    $scope.formData.area = $scope.areaData.officeName;

                    var city = $filter('filter')($scope.cityList, { text: $scope.areaData.districtName });
                    if (city)
                        $scope.formData.city = city[0];


                    var state = $filter('filter')($scope.stateList, { text: $scope.areaData.stateName });
                    if (state)
                        $scope.formData.state = state[0];

                }
                else {
                    $scope.areaList = data;
                    $scope.areaList.push({ officeName: 'Other' });
                    $scope.areaData = null;

                    var city = $filter('filter')($scope.cityList, { text: data[0].districtName });
                    if (city)
                        $scope.formData.city = city[0];


                    var state = $filter('filter')($scope.stateList, { text: data[0].stateName });
                    if (state)
                        $scope.formData.state = state[0];
                }
            }
        });
    }

    $scope.onAreaChange = function () {
        if ($scope.formData.areaName.officeName == 'Other') {
            $scope.formData.area = null;
            //$scope.formData.city = null;
            //$scope.formData.state = null;
        }
        else {
            var areaData = $scope.formData.areaName;
            $scope.formData.area = areaData.officeName;

            var city = $filter('filter')($scope.cityList, { text: areaData.districtName });
            if (city)
                $scope.formData.city = city[0];


            var state = $filter('filter')($scope.stateList, { text: areaData.stateName });
            if (state)
                $scope.formData.state = state[0];
        }
    }

}]);