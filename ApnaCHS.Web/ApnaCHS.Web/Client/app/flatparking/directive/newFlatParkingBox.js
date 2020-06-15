app.directive('newFlatParkingBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', '$stateParams', '$rootScope', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, $stateParams, $rootScope, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flatparking/directive/newFlatParkingBox.html',
            replace: true,
            transclude: true,
            scope: {
                floorId: '=',
                facilityId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.parkingTypes = appConstant.ParkingType;

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);
                    if (scope.floorId != null) {
                        fdata.floor = { id: scope.floorId };
                    }
                    fdata.facility = { id: scope.facilityId };
                    fdata.type = fdata.type.id;

                    var promise = $http.post(DATA_URLS.NEW_FLATPARKING, { flatParking: JSON.stringify(fdata), count: fdata.noofparkings });

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onFlatParkingCreate", data);
                            toastr["success"]("Flat Parking created successfully", "Success");
                            $('#cancelNewFlatParkingBox').click();
                            scope.formData = {};
                        }
                    });
                }

            }
        };
    }]);