app.directive('newSecuritystaffBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/securitystaff/directive/newSecurityStaffBox.html',
            replace: true,
            transclude: true,
            scope: {
                societyId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.formData = {};
                scope.todayDate = new Date();
                getUDFList();

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.dateOfBirth = $filter('date')(fdata.dateOfBirth, appConstant.ConversionDateFormat);
                    fdata.joiningDate = $filter('date')(fdata.joiningDate, appConstant.ConversionDateFormat);
                    fdata.lastWorkingDay = $filter('date')(fdata.lastWorkingDay, appConstant.ConversionDateFormat);
                    fdata.society = { id: scope.societyId };

                    var promise = $http.post(DATA_URLS.NEW_SECURITYSTAFF, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status === 200) {
                            $rootScope.$broadcast("onSecurityStaffCreate", data);
                        }
                    });
                };

                function getUDFList() {
                    var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 3);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.udfList = data;
                        }
                    });
                };
            }
        };
    }]);