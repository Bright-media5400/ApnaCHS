app.directive('vehicleBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', '$stateParams', '$rootScope', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, $stateParams, $rootScope, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/vehicle/directive/vehicleBox.html',
            replace: true,
            transclude: true,
            scope: {
                flatOwnerId: '=',
                flatId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.vehicleTypes = appConstant.VechileType;

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);

                    fdata.flatOwner = { id: scope.flatOwnerId };
                    fdata.flat = { id: scope.flatId }
                    fdata.type = fdata.type.id;
                    var promise = $http.post(DATA_URLS.NEW_VEHICLE, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onVehicleCreate", data);
                            $('#cancelVehicleBox').click();
                            scope.formData = null;
                            getVehicleList();
                        }
                    });
                };

                //clear Family Member Function

                scope.cancelVehicleBox = function () {
                    scope.formData = null;
                };
            }
        }
    }]);