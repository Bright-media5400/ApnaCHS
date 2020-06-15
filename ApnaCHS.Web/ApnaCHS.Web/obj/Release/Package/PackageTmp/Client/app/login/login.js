app.controller('login', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService', 'DATA_URLS', '$filter',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter) {

    'use strict';

    $scope.isSigningIn = { value: false };
    $scope.loginError = { value: '' }
    $scope.signinData = {};
    $scope.instances = [];
    $scope.isInstanceAllowed = false;

    //TODO: For testing only
    $scope.signinData.username = 'admin';
    $scope.signinData.password = 'admin2019';


    $rootScope.$on('authStatusChanged', function () {
        $scope.isSigningIn.value = false;

        if (!authService.authentication.isAuth) {
            $scope.loginError.value = authService.authentication.loginError;
        }
        else {
            $scope.signinData = {};
            if ($scope.isInstanceAllowed) {
                $scope.signinData.instance = $scope.instances[0];
            }
            $scope.signinForm.$setPristine();
        }
    });

    $scope.signIn = function () {
        $scope.isSigningIn.value = true;
        $scope.loginError.value = '';
        authService.login($scope.signinData);
    }
}]);