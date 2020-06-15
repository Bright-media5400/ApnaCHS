app.controller('mainController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS' , '$http','$state' , 'commonService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $state,commonService) {

    'use strict';
    $scope.isopen = false;
    $scope.isUserAuthenticated = authService.authentication.isAuth;
    $scope.userName = '';
    $scope.fullName = '';
    $scope.isHome = true;

    $rootScope.$on('$routeChangeSuccess', function (e) {
        
        $scope.isHome =  $location.path() == '/admin'
        || $location.path() == '/member'
        || $location.path() == '/tenant'
        || $location.path() == '/mfamily'
        || $location.path() == '/tfamily';

      });    

    $scope.togglesidenav = function() {
        $scope.isopen = !$scope.isopen;
    }

    $scope.closesidenav = function() {
        if($scope.isopen){
            $scope.isopen = false;
        }
    }    

    if ($scope.isUserAuthenticated) {
        $scope.userName = authService.authentication.loginData.userName;
        $scope.fullName = authService.authentication.loginData.userFullName;
        
        commonService.navigateUser();
        // if ($location.path().match('/unauthorized')) {
        //     $location.path(DATA_URLS.DEFAULT_PATH);
        // }
    }

    $rootScope.$on('authStatusChanged', function () {
        $scope.isUserAuthenticated = authService.authentication.isAuth;

        if ($scope.isUserAuthenticated) {
            $scope.userName = authService.authentication.loginData.userName;
            $scope.fullName = authService.authentication.loginData.userFullName;

            navigateUser();
            //if ($location.path().match('/unauthorized')) {
            //$location.path(DATA_URLS.DEFAULT_PATH);
            //}
        }
        else {

        }
    });

    function navigateUser() {

        var disC = commonService.distinctByName(JSON.parse(authService.authentication.loginData.complexes));
        var disS = commonService.distinctByName(JSON.parse(authService.authentication.loginData.societies));

        if (disC.length > 1) {
            $location.path('/complex/select');
        }
        else if (disS.length > 0) {
            if (disS.length == 1) {
                commonService.selectSociety(disS[0]);
                commonService.navigateUser();
            }
            else {
                $location.path('/society/select');
            }
        }
        else {
            $location.path('/unauthorized');
        }
    }

    $scope.navigateHome = function (){
        commonService.navigateUser();
    }
 
    $scope.logOut = function () {
        $scope.closesidenav();
        authService.logout();
    }
}]);