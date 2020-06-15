app.controller('uploadFlatOwnersController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService', '$sce',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService, $sce) {

    'use strict';
    $scope.flatList = [];
    $scope.uploadList = [];
    $scope.searchFormData = {};
    $scope.resultAvailable = false;
    $scope.colDOB = 'Date Of Birth (MM/DD/YYYY)';
    $scope.colMSD = 'Member Since Date (MM/DD/YYYY)';
    $scope.colMTD = 'Member Till Date (MM/DD/YYYY)';

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

        var promise = $http.post(DATA_URLS.REPORT_FLATS, filters);
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
                //"SrNo": srno,
                "RegistrationNo": item.registrationNo,
                "Society": item.society,
                "Building": item.building,
                "Wing": item.wing,
                "Floor": item.floor,
                "Flat": item.flat,
                "Name": '',
                "Mobile No": '',
                "Email": '',
                "Gender": '',
                "Aadhaar Card No": '',
                "Date Of Birth (MM/DD/YYYY)": '',
                "Member Since Date (MM/DD/YYYY)": '',
                "Member Till Date (MM/DD/YYYY)": ''
            });

            //srno = srno + 1;
        });

        data2xlsx({
            filename: "Flat Owners History Template" + ".xlsx",
            sheetName: "FlatOwners",
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
                        "flat": v['Flat'],
                        "name": v['Name'],
                        "mobileNo": v['Mobile No'],
                        "emailId": v['Email'],
                        "gender": v['Gender'],
                        "aadhaarCardNo": v['Aadhaar Card No'],
                        "dateOfBirth": v[$scope.colDOB] && typeof v[$scope.colDOB].getMonth === 'function' ? v[$scope.colDOB].setDate(v[$scope.colDOB].getDate() + 1) : v[$scope.colDOB],
                        "memberSinceDate": v[$scope.colMSD] && typeof v[$scope.colMSD].getMonth === 'function' ? v[$scope.colMSD].setDate(v[$scope.colMSD].getDate() + 1) : v[$scope.colMSD],
                        "memberTillDate": v[$scope.colMTD] && typeof v[$scope.colMTD].getMonth === 'function' ? v[$scope.colMTD].setDate(v[$scope.colMTD].getDate() + 1) : v[$scope.colMTD]
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
            var f = angular.copy(v);

            f.dateOfBirth = $filter('date')(f.dateOfBirth, 'MM/dd/yyyy');
            f.memberSinceDate = $filter('date')(f.memberSinceDate, 'MM/dd/yyyy');
            f.memberTillDate = $filter('date')(f.memberTillDate, 'MM/dd/yyyy');

            list.push(f);
        });

        var fdata = { flatowners: list };
        //console.log('fdata - ', fdata);

        var promise = $http.post(DATA_URLS.UPLOAD_FLATOWNERS, fdata);

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