app.directive('newFlatOwnerBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flatowner/directive/newFlatOwnerBox.html',
            replace: true,
            transclude: true,
            scope: {
                flatId: '=',
                ownerType: '=',
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.todayDate = new Date();

                getGenderList();

                scope.submit = function () {
                    var fdata = angular.copy(scope.formData);
                    fdata.dateOfBirth = $filter('date')(fdata.dateOfBirth, appConstant.ConversionDateFormat);
                    //fdata.memberSinceDate = $filter('date')(fdata.memberSinceDate, appConstant.ConversionDateFormat);
                    fdata.flats = [
                        {
                            flat: { id: scope.flatId },
                            flatOwnerType: scope.ownerType,
                            memberSinceDate: $filter('date')(fdata.memberSinceDate, appConstant.ConversionDateFormat)
                        }];
                    var promise = $http.post(DATA_URLS.NEW_FLATOWNER, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onFlatOwnerCreate", data);
                            scope.formData = {};
                        }
                    });
                }

                function getGenderList() {
                    var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 8);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.genderList = data;
                        }
                    });
                };
            }
        };
    }]);