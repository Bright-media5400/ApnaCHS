app.controller('billDetailsController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $routeParams, $state,localStorageService) {

    'use strict';
    $scope.editFormData = {};
    $scope.id = null;
    
    if ($routeParams.bid != undefined) {
        $scope.id = $routeParams.bid;
        get($routeParams.bid);
   }

    function get(id) {

        var promise = $http.post(DATA_URLS.GET_BILLING, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;
                
            }
        });
    }
}]);
                
        
        
                
    
