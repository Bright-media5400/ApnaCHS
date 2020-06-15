app.controller('uploadVehiclesController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService', '$sce', 'societyService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService, $sce, societyService) {

    'use strict';
    $scope.flatList = [];
    $scope.uploadList = [];
    $scope.searchFormData = {};
    $scope.resultAvailable = false;

    getRelationshipList();
    getGenderList();

    function clearSearchFilters() {
        $scope.searchFilters = {};
    }

    function setFilters() {
        clearSearchFilters();

        var soc = societyService.getSociety();
        $scope.searchFilters.societyId = soc;
    }

    function getGenderList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 8);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {

                var str = [];
                angular.forEach(data, function (v) {
                    str.push(v.text);
                });

                $scope.gendersString = str.toString();
            }
        });
    };

    function getRelationshipList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 9);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {

                var str = [];
                angular.forEach(data, function (v) {
                    str.push(v.text);
                });

                $scope.relationsString = str.toString();
            }
        });
    };

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

        var promise = $http.post(DATA_URLS.EXPORT_FLATOWNERSVEHICLES, filters);
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
                "Society": item.society,
                "Building": item.building,
                "Wing": item.wing,
                "Floor": item.floor,
                "Flat": item.flat,
                "FlatOwner": item.flatOwner,
                "FlatOwnerType": item.flatOwnerType,
                "Name": '',
                "Make": '',
                "Number": '',
                "Type": '',
            });

            //srno = srno + 1;
        });

        var str = [];
        angular.forEach(appConstant.VechileType, function (v) {
            str.push(v.text);
        });


        if (str.length > 0) {
            toexport.push({ "Society": str.toString() });
        }

        data2xlsx({
            filename: "Flat Owners Vehicles History Template" + ".xlsx",
            sheetName: "FlatOwnersVehicles",
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
            var cfb = XLSX.read(data, { type: 'binary' });

            cfb.SheetNames.forEach(function (sheetName) {

                var oJS = XLS.utils.sheet_to_json(cfb.Sheets[sheetName]);
                console.log('oJS - ', oJS);

                var i = 1;
                var upload = [];
                angular.forEach(oJS, function (v) {
                    upload.push({
                        srno: i++,
                        society: v['Society'],
                        building: v['Building'],
                        wing: v['Wing'],
                        floor: v['Floor'],
                        flat: v['Flat'],
                        flatOwner: v['FlatOwner'],
                        flatOwnerType: v['FlatOwnerType'],
                        name: v['Name'],
                        make: v['Make'],
                        number: v['Number'],
                        type: v['Type'],
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

                if (f.type) {
                    f.type = $filter('filter')(appConstant.VechileType, { text: f.type })[0].id;
                }

                list.push(f);
            }
        });

        var fdata = { vehicles: list };
        var promise = $http.post(DATA_URLS.UPLOAD_FLATOWNERSVEHICLE, fdata);

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
                "FlatOwner": item.flatOwner,
                "FlatOwnerType": item.flatOwnerType,
                "Name": item.name,
                "Make": item.make,
                "Number": item.number,
                "Type": item.type,
                "Result": item.message
            });
        });

        data2xlsx({
            filename: "Vehicles All Export Result" + ".xlsx",
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
                    "FlatOwner": item.flatOwner,
                    "FlatOwnerType": item.flatOwnerType,
                    "Name": item.name,
                    "Make": item.make,
                    "Number": item.number,
                    "Type": item.type,
                    "Result": item.message
                });
            }
        });

        data2xlsx({
            filename: "Vehicles Failed Export Result" + ".xlsx",
            sheetName: "Failed",
            data: toexport
        });
    }
}]);