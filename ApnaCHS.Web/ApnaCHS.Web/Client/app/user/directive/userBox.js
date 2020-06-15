app.directive('userBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/user/directive/userBox.html',
            replace: true,
            transclude: true,
            scope: {
                societyId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.userList = [];
                scope.formData = {};

                getGroupList();
                getUserList();

                function getGroupList() {

                    var fdata = { societyId: scope.societyId };
                    var promise = $http.post(DATA_URLS.GROUP_APPLICATIONROLELIST, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.recordList = data;
                        }
                    });
                }

                scope.edit = function (item) {
                    scope.formData = angular.copy(item);

                    scope.formData.isEdit = true;
                }

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.societyId = scope.societyId;

                    fdata.userroles = [];
                    fdata.userroles.push(scope.formData.role)

                    var promise = $http.post(fdata.isEdit ? DATA_URLS.UPDATE_SOCIETYUSER : DATA_URLS.NEW_SOCIETYUSER, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            toastr["success"](scope.formData.isEdit ? "User updated" : "New User created successfully", "Success");
                            $('#cancelAddNewUser').click();
                            getList();
                        }
                    });
                }

                function getUserList() {

                    var promise = $http.post(DATA_URLS.LIST_USERS_SOCIETY, scope.societyId);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.userList = data;
                        }
                    });
                }
            }
        };
    }]);
