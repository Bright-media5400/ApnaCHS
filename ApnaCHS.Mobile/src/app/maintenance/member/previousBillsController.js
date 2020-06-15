app.controller('previousBillsController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS'
    , '$http', '$filter', 'appConstant', '$routeParams','localStorageService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $filter, appConstant, $routeParams,localStorageService) {

    'use strict';
    $scope.recordList = [];
    var months = ['January', 'February', 'March', 'April', 'May',
        'June', 'July', 'August', 'September',
        'October', 'November', 'December'];
    

    getList();

    function getList() {

        var user = authService.authentication.loginData.userName;        
        var soc =  JSON.parse(localStorageService.get('onLogSoc'));
        var fdata = { username: user,societyId : soc.id };

        var promise = $http.post(DATA_URLS.BILLING_MYBILLS, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.showMonth = function(bill) {
        return months[bill.month - 1] || '';
    }

}]);