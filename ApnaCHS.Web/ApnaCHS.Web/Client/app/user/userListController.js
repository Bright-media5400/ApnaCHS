app.controller('userListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http) {

    'use strict';
    $scope.recordList = [];

    getList();

    function getList() {

        var promise = $http.post(DATA_URLS.LIST_USERS_ADMIN);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.new = function (item) {
        $location.path('user/new');
    }

    $scope.edit = function (item) {
        $location.path('user/' + item.id);
    }

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_USER, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Usuario eliminado satisfactoriamente", "&#201;xito");
                $scope.recordList.splice($scope.selectedItem.index, 1);
                $('#cancelConfirmDelete').click();
            }
        });

    }


}]);