app.controller('societyDetailsController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $routeParams, $state,localStorageService) {

    'use strict';
     $scope.society =  JSON.parse(localStorageService.get('onLogSoc'));
}]);
                
        
        
                
    
