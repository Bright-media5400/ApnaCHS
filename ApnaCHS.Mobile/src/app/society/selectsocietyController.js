app.controller('selectsocietyController', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService', 'DATA_URLS', '$filter','commonService',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter,commonService) {

    'use strict';
    $scope.societyRecords = commonService.distinctByName(JSON.parse(authService.authentication.loginData.societies));

    $scope.select = function(s){
        commonService.selectSociety(s);
        commonService.navigateUser();
    }
}]);