app.controller('selectcomplexController', ['$scope', '$rootScope', '$window', '$location', '$http', 'authService', 'DATA_URLS', '$filter',
function ($scope, $rootScope, $window, $location, $http, authService, DATA_URLS, $filter) {

    'use strict';
    $scope.complexRecords = distinctByName(JSON.parse(authService.authentication.loginData.complexes));


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