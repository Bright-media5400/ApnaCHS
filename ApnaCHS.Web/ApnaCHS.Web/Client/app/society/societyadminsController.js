app.controller('societyadminsController', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService', 'DATA_URLS', '$filter', 'localStorageService', 'societyService',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter, localStorageService, societyService) {

    'use strict';
    $scope.recordList = [];

    getList();

    function getList() {

        var soc = societyService.getSociety();
        var promise = $http.post(DATA_URLS.LIST_ADMINS_SOCIETY, soc.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    $scope.removeAdmin = function (user, index) {

        var soc = societyService.getSociety();
        var fdata = { userId: user.id, societyId: soc.id };
        var promise = $http.post(DATA_URLS.UPDATE_USER_REMOVEADMIN, fdata);

        promise.then(function (data) {
            $('#cancelConfirmDelete').click();
            if (!data.status || data.status == 200) {
                $scope.recordList.splice(index, 1);
                toastr.success(user.name + ' removed as admin', 'Success', {});
            }
        });
    }

    $scope.onRemoveAdmin = function (user, index) {
        $scope.selectedItem = user;
        $scope.selectedItem.index = index;
    }

    $scope.onMembersClick = function () {
        $location.path('/society/members');
    }

    $scope.onTenantsClick = function () {
        $location.path('/society/tenants');
    }

    $scope.onNewClick = function () {
        $location.path('/society/user/new');
    }

    $scope.onBlockUser = function (user, index) {
        $scope.selectedUser = user;
        $scope.selectedUser.index = index;
    }

    $scope.blockUser = function (user, index) {
        var soc = societyService.getSociety();
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