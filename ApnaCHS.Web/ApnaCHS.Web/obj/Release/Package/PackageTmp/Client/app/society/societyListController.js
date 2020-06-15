app.controller('societyListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http) {

    'use strict';
    $scope.recordList = [];

    getList();

    function getList() {

        var promise = $http.post(DATA_URLS.LIST_SOCIETIES);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.new = function (item) {
        $location.path('society/new');
    }

    $scope.edit = function (item) {
        $location.path('society/details/' + item.id);
    }

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_USER, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society deleted successfully", "Success");
                $scope.recordList.splice($scope.selectedItem.index, 1);
                $('#cancelConfirmDelete').click();
            }
        });

    }

    $scope.exportToExcel = function () {

        if ($scope.recordList.length == 0) {
            toastr.success('No result found to export.', 'Success', {});
            return;
        }

        toastr.success('Export started please wait.', 'Success', {});

        //$("#exportReport").table2xlsx({
        //    filename: "Society List" + ".xlsx",
        //    sheetName: "SocietyList"
        //});

        var toexport = [];
        angular.forEach($scope.recordList, function (item) {

            toexport.push({
                "ID": item.id,
                "Name": item.name
            });
        });

        data2xlsx({
            filename: "Society List" + ".xlsx",
            sheetName: "SocietyList",
            data: toexport
        });
    }

}]);
