app.controller('maintenanceListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter','localStorageService', 'societyService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter, localStorageService, societyService) {

    'use strict';
    var soc = societyService.getSociety();
    $scope.societyId = soc.id;
}]);
