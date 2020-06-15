app.controller('homeController', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService','DATA_URLS','$filter',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter) {

    'use strict';
    //$scope.isUserAuthenticated = !!authService.authentication();
    $scope.Init = false;
    $scope.isUserAuthenticated = authService.authentication.isAuth;
    $scope.userName = '';
    $scope.fullName = '';

    setTimeout(function () {
        $scope.Init = true;
        $rootScope.$apply();
    }, 100);

    if ($scope.isUserAuthenticated) {
        $scope.userName = authService.authentication.loginData.userName;
        $scope.fullName = authService.authentication.loginData.userFullName;
        $scope.type = authService.authentication.loginData.type;

            if ($location.path().match('/unauthorized')) {
                $location.path(DATA_URLS.DEFAULT_PATH);
        }
    }

    $rootScope.$on('authStatusChanged', function () {
        $scope.isUserAuthenticated = authService.authentication.isAuth;

        if ($scope.isUserAuthenticated) {
            $scope.userName = authService.authentication.loginData.userName;
            $scope.fullName = authService.authentication.loginData.userFullName;
            $scope.type = authService.authentication.loginData.type;

            if (authService.authentication.loginData.type == 1) {
                //if ($location.path().match('/unauthorized')) {
                $location.path(DATA_URLS.DEFAULT_PATH);
                //}
            } else {

                var disC = distinctByName(JSON.parse(authService.authentication.loginData.complexes));
                var disS = distinctByName(JSON.parse(authService.authentication.loginData.societies));

                if (disC.length > 1) {
                    $location.path('/complex/select');
                }
                else if (disS.length > 0) {
                    if (disS.length == 1) {
                        $location.path('/society/details/' + disS[0].id);
                    }
                    else {
                        $location.path('/society/select');
                    }
                }
                else {
                    $location.path('/unauthorized');
                }
            }
        }
        else {

        }
    });

    $scope.logOut = function () {
        authService.logout();
    }

    $scope.$on('$routeChangeSuccess', function () {
    });

    $scope.$on('$routeChangeStart', function (scope, next, current) {
    });

    function distinctByName(jsonData) {

        var lookup = {};
        var items = jsonData;
        var lst = [];

        for (var item, i = 0; item = items[i++];) {
            var name = item.name;

            if (!(name in lookup)) {
                lookup[name] = 1;
                lst.push(name);
            }
        }
            
        var result = [];
        angular.forEach(lst, function (v) {
            var obj = $filter('filter')(items, { name: v }, true)[0];
            result.push(obj);
        });

        return result;
    }
}]);