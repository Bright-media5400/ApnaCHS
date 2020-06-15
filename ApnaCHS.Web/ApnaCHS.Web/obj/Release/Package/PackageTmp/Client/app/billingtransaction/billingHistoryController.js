app.controller('billingHistoryController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService) {
    'use strict';
    $scope.recordList = [];
    $scope.paymentModes = appConstant.PaymentMode;
    getList();

    $scope.onBackClick = function () {
        $window.history.back();
    }

    function getList() {

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var fdata = { societyId: soc.id };

        var promise = $http.post(DATA_URLS.LIST_BILLINGTRANSACTIONS, fdata);

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
}]);
           

