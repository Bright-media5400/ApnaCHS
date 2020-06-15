app.directive('newAssetBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', 'appConstant', 'localStorageService', 'societyService',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, appConstant, localStorageService, societyService) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/asset/directive/newAssetBox.html',
            replace: true,
            transclude: true,
            scope: {
                societyId: '=',
                facilityId: '=',
                floorId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.todayDate = new Date();
                var soc = societyService.getSociety();

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.complex = { id: soc.complex.id }
                    fdata.society = { id: soc.id }
                    fdata.facility = scope.facilityId ? { id: scope.facilityId } : null;
                    fdata.purchaseDate = $filter('date')(fdata.purchaseDate, appConstant.ConversionDateFormat);
                    // { id: scope.facilityId };
                    fdata.floor = null;

                    var promise = $http.post(DATA_URLS.NEW_SOCIETYASSET, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onSocirtyAssetCreate", data);
                        }
                    });
                };
            }
        };
    }]);