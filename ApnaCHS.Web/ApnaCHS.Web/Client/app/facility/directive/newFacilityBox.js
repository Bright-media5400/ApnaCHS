app.directive('newFacilityBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', 'appConstant', 'localStorageService', 'societyService',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, appConstant, localStorageService, societyService) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/facility/directive/newFacilityBox.html',
            replace: true,
            transclude: true,
            scope: {
                societyId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.formData.noOfWings = 1;

                scope.facilityTypes = appConstant.FacilityType;

                scope.isWing = false;
                scope.isNoOfFloors = false;
                scope.isNoOfFlats = false;
                scope.isNoOfLifts = false;
                scope.isNoOfParkinglots = false;
                scope.wings = null;


                scope.submit = function () {

                    var facilities = []
                    var soc = societyService.getSociety();

                    angular.forEach(scope.wings, function (v) {
                        var obj = angular.copy(scope.formData);
                        obj.type = obj.facilityType.id;
                        obj.complex = { id: soc.complex.id };

                        obj.wing = v.wing;
                        obj.noOfFloors = v.noOfFloors;
                        obj.noOfFlats = v.noOfFlats;
                        obj.noOfLifts = v.noOfLifts;
                        obj.noOfParkinglots = v.noOfParkinglots;

                        //PATCH for opening parking to handle the scenario for different parking types
                        if (obj.type == 3) { 
                            obj.wing = v.noOf2Parkinglots + ':' + v.noOf4Parkinglots + ':' + v.noOf24Parkinglots;
                        }
                        facilities.push(obj);
                    });

                    var fdata = { societyId: scope.societyId, facilities: facilities };
                    var promise = $http.post(DATA_URLS.NEW_FACILITY, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            clearForm();
                            $rootScope.$broadcast("onFacilityCreate", data);
                        }
                    });
                };

                function clearForm() {
                    scope.isWing = false;
                    scope.isNoOfFloors = false;
                    scope.isNoOfFlats = false;
                    scope.isNoOfLifts = false;
                    scope.isNoOfParkinglots = false;
                    scope.wings = null;

                    scope.formData = {};
                    scope.formData.noOfWings = 1;
                    scope.onNoOfWings();
                }

                scope.onFacilityType = function () {
                    scope.formData.noOfWings = 1;
                    scope.onNoOfWings();

                    if (scope.formData.facilityType.id == 1) {
                        //Building 
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Flats";
                    }
                    else if (scope.formData.facilityType.id == 2) {
                        //Row Houses
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Row Houses";
                    }
                    else if (scope.formData.facilityType.id == 3) {
                        //Open Parking
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = true;
                    }
                    else if (scope.formData.facilityType.id == 4) {
                        //Parking Tower
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 5) {
                        //Garden
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 6) {
                        //Club House
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Rooms";
                    }
                    else if (scope.formData.facilityType.id == 7) {
                        //Gym
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Rooms";
                    }
                    else if (scope.formData.facilityType.id == 8) {
                        //Swimming Pool
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 9) {
                        //Community Hall
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Rooms";
                    }
                    else if (scope.formData.facilityType.id == 10) {
                        //Play Ground
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 11) {
                        //Play Area
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 12) {
                        //Commercial Space
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Shops";
                    }
                    else if (scope.formData.facilityType.id == 13) {
                        //School
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Rooms";
                    }
                    else if (scope.formData.facilityType.id == 14) {
                        //Hospital
                        scope.isWing = true;
                        scope.isNoOfFloors = true;
                        scope.isNoOfFlats = true;
                        scope.isNoOfLifts = true;
                        scope.isNoOfParkinglots = false;

                        scope.formData.flatLabel = "Rooms";
                    }
                    else if (scope.formData.facilityType.id == 15) {
                        //Temple
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 16) {
                        //Mosque
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 17) {
                        //Curch
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                    else if (scope.formData.facilityType.id == 18) {
                        //Gurudwada
                        scope.isWing = false;
                        scope.isNoOfFloors = false;
                        scope.isNoOfFlats = false;
                        scope.isNoOfLifts = false;
                        scope.isNoOfParkinglots = false;
                    }
                }

                scope.onNoOfWings = function () {
                    scope.wings = [];

                    for (var i = 0; i < scope.formData.noOfWings; i++) {
                        scope.wings.push({});
                    }
                }

                scope.clearNewFacilityBox = function () {
                    scope.formData = null;
                    scope.isWing = false;
                    scope.isNoOfFloors = false;
                    scope.isNoOfFlats = false;
                    scope.isNoOfLifts = false;
                    scope.isNoOfParkinglots = false;
                    scope.wings = null;
                }
            }
        };
    }]);