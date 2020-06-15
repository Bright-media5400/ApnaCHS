app.controller('newSocietyController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', 'appConstant','$filter',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, appConstant, $filter) {

    'use strict';
    $scope.uploadedSocietyList = [];
    $scope.complexList = [];
    $scope.formData = {};
    getComplexList();

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        fdata.dateOfIncorporation = $filter('date')(fdata.dateOfIncorporation, appConstant.ConversionDateFormat);
        fdata.dateOfRegistration = $filter('date')(fdata.dateOfRegistration, appConstant.ConversionDateFormat);
        var promise = $http.post(DATA_URLS.NEW_SOCIETY, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society registered successfully", "Success");
                $location.path('/society/list');
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    //$scope.onUploadExcel = function () {
    //    $('#file-upload').click();
    //}

    $scope.uploadSocietyList = function (files) {

        // Get The File From The Input
        var oFile = files[0];
        var sFilename = oFile.name;
        // Create A File Reader HTML5
        var reader = new FileReader();


        // Ready The Event For When A File Gets Selected
        reader.onload = function (e) {
            var data = e.target.result;
            var cfb = XLSX.read(data, { type: 'binary' });

            cfb.SheetNames.forEach(function (sheetName) {

                var oJS = XLS.utils.sheet_to_json(cfb.Sheets[sheetName]);
                //console.log(oJS);

                var i = 1;
                angular.forEach(oJS, function (v) {
                    v.name = v['Society Name*'];
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
    function getComplexList() {

        var promise = $http.post(DATA_URLS.LIST_COMPLEX);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.complexList = data;
            }
        });
    }
}]);