app.directive('newFloorBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/floor/directive/newFloorBox.html',
            replace: true,
            transclude: true,
            scope: {
                facilityId: '=',
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.FloorTypes = appConstant.FloorType;

                scope.submit = function () {
                    var fdata = angular.copy(scope.formData);
                    fdata.facility = { id: scope.facilityId };
                    fdata.type = fdata.type.id;

                    var promise = $http.post(DATA_URLS.NEW_FLOOR, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onFloorCreate", data);
                            scope.formData = {};
                        }
                    });
                };
            }
        };
    }]);