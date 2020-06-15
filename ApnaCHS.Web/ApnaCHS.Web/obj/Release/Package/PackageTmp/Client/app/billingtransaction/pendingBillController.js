app.controller('pendingBillController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS'
    , '$http', 'appConstant', '$routeParams',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, appConstant, $routeParams) {

    'use strict';
    $scope.recordList = [];

    if ($routeParams.sid != undefined) {
        getList($routeParams.sid);
    }

    function getList(socId) {

        var fdata = { societyId: socId };
        var promise = $http.post(DATA_URLS.PENDING_BILLING, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.showMonth = function (month) {
        var monthNames = ["January", "February", "March", "April", "May", "June",
          "July", "August", "September", "October", "November", "December"
        ];

        return monthNames[month - 1];
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

}]);