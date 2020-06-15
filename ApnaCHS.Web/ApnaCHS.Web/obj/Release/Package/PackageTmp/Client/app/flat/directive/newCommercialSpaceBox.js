app.directive('newCommercialSpaceBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flat/directive/newCommercialSpaceBox.html',
            replace: true,
            transclude: true,
            scope: {
                floorId: '=',
                societyId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                getUDFList();

                scope.submitCS = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.floor = { id: scope.floorId };
                    fdata.society = { id: scope.societyId };
                    fdata.isCommercialSpace = true;
                    var promise = $http.post(DATA_URLS.NEW_FLAT, { flat: JSON.stringify(fdata), count: fdata.noofflats });

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onCommercialSpaceCreate", data);
                            scope.formData = {};
                        }
                    });
                };

                function getUDFList() {
                    var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 1);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.udfList = data;
                        }
                    });
                };
            }
        };
    }]);