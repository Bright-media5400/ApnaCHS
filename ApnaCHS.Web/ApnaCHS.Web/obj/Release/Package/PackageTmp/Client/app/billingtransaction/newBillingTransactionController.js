app.controller('newBillingTransactionController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS'
    , '$http', 'appConstant', '$routeParams', '$filter',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, appConstant, $routeParams, $filter) {

    'use strict';
    $scope.flatDues = null;
    $scope.formData = {};
    $scope.paymentMode = appConstant.PaymentMode;
    $scope.result = null;
    $scope.flatId = null;
    $scope.societyId = null;

    if ($routeParams.fid != undefined) {
        $scope.flatId = $routeParams.fid;
        get($routeParams.fid);
    }

    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;
    }

    function get(id) {

        var promise = $http.post(DATA_URLS.FLATCURRENTDUES_BILLING, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.flatDues = data;
                $scope.formData.receiptNo = $scope.flatDues.receiptNo
                $scope.formData.amount = $scope.flatDues.amount
            }
        });
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        fdata.mode = fdata.mode.id;
        fdata.society = { id: ($scope.flatDues ? $scope.flatDues.society.id : $routeParams.sid) };
        fdata.chequeDate = $filter('date')(fdata.chequeDate, appConstant.ExcelFormat);

        var promise = $http.post(DATA_URLS.NEW_BILLINGTRANSACTION, fdata);
        promise.then(function (data) {
            if (!data.status || data.status === 200) {
                $scope.formData = {};
                $scope.result = data;
                toastr["success"]("Payment completed successfully", "Success");
            }
        });
    };

    $scope.onPaymentModeChange = function () {
        if ($scope.formData.mode.id == 1) {
            $scope.formData.reference = 'By cash';
        }
        else {
            $scope.formData.reference = '';
        }
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

}]);