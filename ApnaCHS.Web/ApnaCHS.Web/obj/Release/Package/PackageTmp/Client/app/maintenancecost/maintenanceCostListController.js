app.controller('maintenanceListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter','localStorageService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter, localStorageService) {

    'use strict';
    var soc = JSON.parse(localStorageService.get('onLogSoc'));
    $scope.societyId = soc.id;
}]);
