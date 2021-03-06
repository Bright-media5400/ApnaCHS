﻿app.controller('uploadFlatsController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService', '$sce',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService, $sce) {

    'use strict';
    $scope.flatList = [];
    $scope.uploadList = [];
    $scope.searchFormData = {};
    $scope.resultAvailable = false;
    $scope.flatTypeList = [];

    getFlatTypeList();

    function getFlatTypeList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 1);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.flatTypeList = data;
            }
        });
    }

    function clearSearchFilters() {
        $scope.searchFilters = {};
    }

    function setFilters() {
        clearSearchFilters();

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        $scope.searchFilters.societyId = soc;
    }

    $scope.clearSearch = function () {
        clearSearchFilters();
        $scope.search();
    }

    $scope.search = function () {

        setFilters();
        var filters = angular.copy($scope.searchFilters);

        if (filters.societyId)
            filters.societyId = filters.societyId.id;

        $scope.flatList = [];
        toastr.success('Export started for flat owner history template.', 'Success', {});

        var promise = $http.post(DATA_URLS.EXPORT_FLOORS, filters);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.flatList = data;
                $scope.downloadExcel();
            }
        });
    }

    $scope.clear = function () {
        $scope.uploadList = [];
        $('#file-upload').val('');
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    $scope.downloadExcel = function () {

        if ($scope.flatList.length == 0) {
            toastr.info('No result found to export.', 'Info', {});
            return;
        }

        var toexport = [];
        //var srno = 1;
        angular.forEach($scope.flatList, function (item) {
            toexport.push({
                "RegistrationNo": item.registrationNo,
                "Society": item.society,
                "Building": item.building,
                "Wing": item.wing,
                "Floor": item.floor,
                "Floor Type": item.floorType,
                "Flat Type": '',
                "Name": '',
                "Total Area": '',
                "Carpet Area": '',
                "BuildUp Area": '',
                "Is Commercial Space": '',
                "Have Parking": ''
            });
        });

        var str = [];
        angular.forEach($scope.flatTypeList, function (v) {
            str.push(v.text);
        });


        if (str.length > 0) {
            toexport.push({ "Society": str.toString() });
        }


        data2xlsx({
            filename: "Flat Upload Template" + ".xlsx",
            sheetName: "Floors",
            data: toexport
        });

    }

    $scope.uploadExcel = function () {
        // Get The File From The Input
        var oFile = $scope.files[0];
        var sFilename = oFile.name;
        // Create A File Reader HTML5
        var reader = new FileReader();


        // Ready The Event For When A File Gets Selected
        reader.onload = function (e) {
            var data = e.target.result;
            var cfb = XLSX.read(data, { type: 'binary', cellDates: true, cellNF: false, cellText: false });

            cfb.SheetNames.forEach(function (sheetName) {

                var oJS = XLS.utils.sheet_to_json(cfb.Sheets[sheetName]);
                var upload = [];
                console.log('oJS - ', oJS);

                var i = 1;
                angular.forEach(oJS, function (v) {

                    upload.push({
                        "srno": i++,
                        "society": v['Society'],
                        "registrationNo": v['RegistrationNo'],
                        "building": v['Building'],
                        "wing": v['Wing'],
                        "floor": v['Floor'],
                        "floorType": v['Floor Type'],
                        "flatType": v['Flat Type'],	
                        "name": v['Name'],	
                        "totalArea": v['Total Area'],	
                        "carpetArea": v['Carpet Area'],	
                        "buildUpArea": v['BuildUp Area'],
                        "isCommercialSpace": v['Is Commercial Space'] && v['Is Commercial Space'].toLowerCase() == 'yes' ? true : false,
                        "haveParking": v['Have Parking'] && v['Have Parking'].toLowerCase() == 'yes' ? true : false
                    });
                });

                $scope.uploadList = upload;
                $scope.resultAvailable = false;

                var phase = $rootScope.$$phase;
                if (!(phase === '$apply' || phase === '$digest')) {
                    $rootScope.$apply();
                }
            });
        };

        // Tell JS To Start Reading The File.. You could delay this if desired
        reader.readAsBinaryString(oFile);
        $scope.resultAvailable = false;
    }

    $scope.saveExcel = function (files) {
        $scope.files = files;
    }

    $scope.save = function () {
        if ($scope.uploadStarted) return;

        $scope.uploadStarted = true;
        $scope.resultAvailable = false;

        var list = []
        angular.forEach($scope.uploadList, function (v) {
            if (v.building) {
                var f = angular.copy(v);
                list.push(f);
            }
        });

        var fdata = { flats: list };
        //console.log('fdata - ', fdata);

        var promise = $http.post(DATA_URLS.UPLOAD_FLATS, fdata);

        promise.then(function (data) {
            $scope.uploadStarted = false;
            $scope.resultAvailable = true;
            if (!data.status || data.status == 200) {
                toastr.success('Upload completed.', 'Success', {});

                angular.forEach(data, function (v) {
                    angular.forEach($scope.uploadList, function (v1) {
                        if (v.srno == v1.srno) {
                            v1.isSuccess = v.isSuccess;
                            v1.message = $sce.trustAsHtml(v.message);
                        }
                    });
                });
            }
        });
    }

    $scope.onAllExportResult = function () {

        if ($scope.uploadList.length == 0) {
            toastr.info('No result found to export.', 'Info', {});
            return;
        }

        var toexport = [];

        angular.forEach($scope.uploadList, function (item) {

            toexport.push({
                "SrNo": item.srno,
                "Society": item.society,
                "Building": item.building,
                "Wing": item.wing,
                "Floor": item.floor,
                "Flat": item.flat,
                "Name": item.name,
                "Mobile No": item.mobileNo,
                "Email": item.emailId,
                "Gender": item.gender,
                "Aadhaar Card No": item.aadhaarCardNo,
                "Date Of Birth": item.dateOfBirth,
                "Member Since Date": item.memberSinceDate,
                "Result": item.message
            });
        });

        data2xlsx({
            filename: "Flat Owners All Export Result" + ".xlsx",
            sheetName: "All",
            data: toexport
        });

    }

    $scope.onFailedExportResult = function () {

        if ($scope.uploadList.length == 0) {
            toastr.info('No result found to export.', 'Info', {});
            return;
        }

        var toexport = [];

        angular.forEach($scope.uploadList, function (item) {
            if (!item.isSuccess) {
                toexport.push({
                    "SrNo": item.srno,
                    "Society": item.society,
                    "Building": item.building,
                    "Wing": item.wing,
                    "Floor": item.floor,
                    "Flat": item.flat,
                    "Name": item.name,
                    "Mobile No": item.mobileNo,
                    "Email": item.emailId,
                    "Gender": item.gender,
                    "Aadhaar Card No": item.aadhaarCardNo,
                    "Date Of Birth": item.dateOfBirth,
                    "Member Since Date": item.memberSinceDate,
                    "Result": item.message
                });
            }
        });

        data2xlsx({
            filename: "Flat Owners Failed Export Result" + ".xlsx",
            sheetName: "Failed",
            data: toexport
        });
    }
}]);