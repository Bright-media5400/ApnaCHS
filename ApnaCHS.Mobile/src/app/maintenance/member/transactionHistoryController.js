app.controller('transactionHistoryController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS'
    , '$http', '$filter', 'appConstant', '$routeParams','localStorageService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $filter, appConstant, $routeParams,localStorageService) {

    'use strict';
    $scope.recordList = [];
    $scope.paymentModes = appConstant.PaymentMode;
    getList();

    function getList() {        

        var user = authService.authentication.loginData.userName;        
        var soc =  JSON.parse(localStorageService.get('onLogSoc'));
        var fdata = { username: user,societyId : soc.id };

        var promise = $http.post(DATA_URLS.MYHISTORY_BILLINGTRANSACTIONS,fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
                angular.forEach($scope.recordList, function (v) {
                    if (v.mode) {
                        v.mode = $filter('filter')($scope.paymentModes, { id: v.mode }, true)[0];
                    }
                });
            }
        });
    }

    //$scope.showMode = function(mode) {
      //  if(mode){
        //    return $filter('filter')(appConstant.PaymentMode, {},true)[0].text;
        //}
    //}
}]);