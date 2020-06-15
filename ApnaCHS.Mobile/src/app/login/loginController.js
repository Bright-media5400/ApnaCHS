app.controller('loginController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS' , '$http','$state',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $state) {

    'use strict';

    $scope.isSigningIn = { value: false };
    $scope.loginError = { value: '' }
    $scope.signinData = {};
    $scope.instances = [];
    $scope.isInstanceAllowed = false;

    //TODO: For testing only
    //$scope.signinData.username = '9898989898';
    //$scope.signinData.password = '9898989898';


    $rootScope.$on('authStatusChanged', function () {
        $scope.isSigningIn.value = false;

        if (!authService.authentication.isAuth) {
            $scope.loginError.value = authService.authentication.loginError;
            toastr["error"]($scope.loginError.value, "Error");
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