app.controller('newUserController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', 'appConstant',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, appConstant) {

    'use strict';
    $scope.formData = {};
    $scope.formData.bBlocked = false;
    $scope.formData.bChangePass = false;

    $scope.userGroups = [];
    $scope.appSettings = [];

    getUserGroups();

    function getUserGroups() {

        var promise = $http.post(DATA_URLS.List_APPLICATIONROLELIST);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.userGroups = [];

                angular.forEach(data, function (v) {
                    
                    if (v.id != 5 && v.id != 6 && v.id != 7 && v.id != 8 && v.id != 9) {
                        $scope.userGroups.push(v);
                    }
                });
            }
        });
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        fdata.userroles = [];
        fdata.userroles.push($scope.formData.role)
        fdata.isback = true;

        var promise = $http.post(DATA_URLS.NEW_USER, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("New user created successfully", "Success");
                $location.path('/users');
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

}]);