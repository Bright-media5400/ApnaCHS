app.controller('costDefinitionController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant) {

    'use strict';
    $scope.formData = {};
    $scope.facilityTypes = appConstant.FacilityType;
  
        $scope.submit = function () {

            var fdata = angular.copy($scope.formData);
            fdata.facilityType = fdata.facilityType.id;

        var promise = $http.post(DATA_URLS.NEW_COSTDEFINATION, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {

                toastr["success"]("New cost defination created successfully", "Success");
                $('#cancelAddNew').click();
            }
        });
    };

}]);

