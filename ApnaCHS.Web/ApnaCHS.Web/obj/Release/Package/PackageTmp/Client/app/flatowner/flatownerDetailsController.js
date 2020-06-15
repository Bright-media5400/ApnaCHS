app.controller('flatownerDetailsController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant) {

    'use strict';
    $scope.flatOwnerId = null;
    $scope.flatId = null;
    $scope.mapObj = null;

    if ($routeParams.oid != undefined) {
        $scope.flatOwnerId = $routeParams.oid;
        $scope.flatId = $routeParams.fid;

        get($scope.flatOwnerId);
    }

    function get(id) {

        var promise = $http.post(DATA_URLS.GET_FLATOWNER, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;

                var flat = $filter('filter')($scope.editFormData.flats, { flat: { id: parseInt($scope.flatId) } }, true)[0];
                $scope.mapObj = flat;
            }
        });
    }
}]);