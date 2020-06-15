app.directive('userGroupBox', ['$http', 'DATA_URLS', '$location', '$anchorScroll', '$filter', 'toastr', '$stateParams', '$rootScope', '$routeParams', 'appConstant',
    function ($http, DATA_URLS, $location, $anchorScroll, $filter, toastr, $stateParams, $rootScope, $routeParams, appConstant) {
        'use strict';

        return {
            restrict: 'E',
            templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/usergroup/directive/userGroupBox.html',
            replace: true,
            transclude: true,
            scope: {
                societyId: '='
            },
            link: function (scope, element, attrs, location, path) {

                'use strict';
                scope.recordList = [];
                scope.formData = {};
                scope.facilityRecords = [];
                scope.definitionList = [];
                scope.todayDate = new Date();

                //if ($routeParams.sid != undefined) {
                //    scope.societyId = $routeParams.sid;
                //    getList();
                //}

                getList();

                //scope.submit = function () {
                //    var fdata = angular.copy(scope.formData);
                //    fdata.society = { id: scope.societyId };

                //    var promise = $http.post(DATA_URLS.ADD_GROUP, fdata);

                //    promise.then(function (data) {
                //        if (!data.status || data.status === 200) {
                //            $rootScope.$broadcast("onUserGroupCreate", data);
                //            scope.formData = {};
                            
                //            getList($routeParams.sid);
                //        }
                //    });
                //};
                scope.edit = function (item) {
                    scope.formData = angular.copy(item);
                    
                    scope.formData.isEdit = true;
                }

                scope.submit = function () {

                    var fdata = angular.copy(scope.formData);
                    fdata.society = { id: scope.societyId };
                    var promise = $http.post(fdata.isEdit ? DATA_URLS.ADD_GROUP : DATA_URLS.ADD_GROUP, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {

                            toastr["success"](scope.formData.isEdit ? "Group updated" : "New Group created successfully", "Success");
                            $('#cancelAddNew').click();
                            getList();
                        }
                    });
                }

                function getList() {

                    var fdata = { societyId: scope.societyId };
                    var promise = $http.post(DATA_URLS.GROUP_APPLICATIONROLELIST, fdata);

                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            scope.recordList = data;
                        }
                    });
                }
            }
        };
    }]);


                
                
                
            