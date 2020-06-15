app.controller('societymembersController', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService', 'DATA_URLS', '$filter', 'localStorageService',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter, localStorageService) {

    'use strict';
    $scope.recordList = [];

    getList();

    function getList() {

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var promise = $http.post(DATA_URLS.LIST_MEMBERS_SOCIETY, soc.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    $scope.makeAdmin = function (user, index) {

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var fdata = { userId: user.id, societyId: soc.id };
        var promise = $http.post(DATA_URLS.UPDATE_USER_MAKEADMIN, fdata);

        promise.then(function (data) {
            $('#cancelConfirmDelete').click();
            if (!data.status || data.status == 200) {
                $scope.recordList.splice(index, 1);
                toastr.success(user.name + ' is a admin', 'Success', {});
            }
        });
    }

    $scope.onMakeAdmin = function (user, index) {
        $scope.selectedItem = user;
        $scope.selectedItem.index = index;
    }

    $scope.onAdminsClick = function () {
        $location.path('/society/admins');
    }

    $scope.onTenantsClick = function () {
        $location.path('/society/tenants');
    }

    $scope.onBlockUser = function (user, index) {
        $scope.selectedUser = user;
        $scope.selectedUser.index = index;
    }

    $scope.blockUser = function (user, index) {
        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var fdata = { userid: $scope.selectedUser.id, societyId: soc.id };
        var promise = $http.post(DATA_URLS.BLOCK_SOCIETYUSER, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society user blocked successfully", "Success");
                $('#cancelConfirmBlock').click();
                $scope.recordList[index].bBlocked = true;

            }
        });
    }
}]);