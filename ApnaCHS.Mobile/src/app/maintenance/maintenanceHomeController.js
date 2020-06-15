app.controller('maintenanceHomeController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS' , '$http','$state' , 
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $state) {

    'use strict';
    $scope.onBackClick = function () {
        $window.history.back();
    }


}]);