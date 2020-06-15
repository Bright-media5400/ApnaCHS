app.controller('flatListController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http,$routeParams, $state,localStorageService) {

    'use strict';
    $scope.recordList = [];
    getList();

    function getList() {

        var user = authService.authentication.loginData.userName;        
        var soc =  JSON.parse(localStorageService.get('onLogSoc'));

        var fdata = { username: user,societyId : soc.id };
        var promise = $http.post(DATA_URLS.REPORT_FLATS,fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;

                if($scope.recordList.length ==1){
                $location.path('flat/details/'+ $scope.recordList[0].flatId);
                  
                }
            }
        });
    }
}]);  
    

                   
                
