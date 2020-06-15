app.controller('userDetailsController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter',
    function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter) {

        'use strict';
        $scope.editFormData = null;
        $scope.userGroups = [];

        if ($routeParams.uid != undefined) {
            get($routeParams.uid);
            getUserGroups();
        }

        function getUserGroups() {

            var promise = $http.post(DATA_URLS.List_APPLICATIONROLELIST);

            promise.then(function (data) {
                if (!data.status || data.status == 200) {
                    $scope.userGroups = data;

                    selectUserGroup();
                }
            });
        }

        function get(id) {

            var promise = $http.post(DATA_URLS.GET_USER, id);

            promise.then(function (data) {
                if (!data.status || data.status == 200) {
                    $scope.editFormData = data;

                    selectUserGroup();
                    selectAppSettings();
                }
            });
        }

        function selectUserGroup() {
            if ($scope.editFormData != null) {
                var roleid = $scope.editFormData.userroles[0].id;
                $scope.editFormData.role = $filter('filter')($scope.userGroups, { id: roleid })[0];
            }
        }

        function selectAppSettings() {
            if ($scope.editFormData != null && $scope.appSettings.length > 0) {
                $scope.editFormData.appSettingsId = $filter('filter')($scope.appSettings, { id: $scope.editFormData.appSettings.id })[0];
            }
        }

        $scope.submit = function () {

            var fdata = angular.copy($scope.editFormData);

            fdata.userroles = [];
            fdata.userroles.push($scope.editFormData.role)
            fdata.appSettingsId = fdata.appSettingsId.id;

            var promise = $http.post(DATA_URLS.UPDATE_USER, fdata);

            promise.then(function (data) {
                if (!data.status || data.status == 200) {
                    toastr["success"]("Usuario actualizado satisfactoriamente", "&#201;xito");
                    $location.path('/users');
                }
            });
        }

        $scope.onBackClick = function () {
            $window.history.back();
        }

    }]);