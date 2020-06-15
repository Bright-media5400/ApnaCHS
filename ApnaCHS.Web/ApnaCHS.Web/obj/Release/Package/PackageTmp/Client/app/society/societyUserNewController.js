app.controller('societyUserNewController', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService', 'DATA_URLS', '$filter', 'localStorageService',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter, localStorageService) {

    'use strict';
    $scope.formData = {};
    $scope.userGroups = [];

    getUserGroups();

    function getUserGroups() {

        var promise = $http.post(DATA_URLS.GROUP_APPLICATIONROLELIST);

        promise.then(function (data) {

            if (!data.status || data.status == 200) {
                $scope.userGroups = [];

                angular.forEach(data, function (v) {
                    $scope.userGroups.push(v);
                });
            }
        });
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        fdata.password = fdata.phoneNumber;
        fdata.userroles = [];
        fdata.userroles.push($scope.formData.role)

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        fdata.societyId = soc.id;

        var promise = $http.post(DATA_URLS.USER_NEWSOCIETY, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("New user created successfully", "Success");
                $location.path('/society/admins');
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

}]);