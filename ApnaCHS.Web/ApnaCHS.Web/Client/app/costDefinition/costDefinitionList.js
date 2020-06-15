app.controller('costDefinitionListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$filter', 'appConstant',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $filter, appConstant) {

    'use strict';
    $scope.recordList = [];
    $scope.facilityTypes = appConstant.FacilityType;
    getList();

    function getList() {

        var promise = $http.post(DATA_URLS.LIST_COSTDEFINATION);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
                angular.forEach($scope.recordList, function (v) {
                    v.facilityType = $filter('filter')($scope.facilityTypes, { id: v.facilityType }, true)[0];

                });
            }
        });
    }
        

    $scope.new = function (item) {
        $location.path('costdefinition/new');
    }
     
    
    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_COSTDEFINATION, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("success", "success");
                $scope.recordList.splice($scope.selectedItem.index, 1);
                $('#cancelConfirmDelete').click();
            }
        });

    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.editFormData);
        if (fdata.facilityType != null) {
            fdata.facilityType = fdata.facilityType.id;
        }

        var promise = $http.post(DATA_URLS.UPDATE_COSTDEFINATION, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Cost Dfinition updated successfully", "Success");
                $('#cancelEditCostDefinitionBox').click();
                getList();
            }
        });
    }

    $scope.edit = function (item) {
        
        $scope.editFormData = angular.copy(item);
        $scope.editFormData.isEdit = true;
    }

}]);