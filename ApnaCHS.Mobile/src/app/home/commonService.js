'use strict';
app.factory('commonService', ['$q', '$injector', 'authService', 'appConstant', '$rootScope', 'DATA_URLS',
'toastr','$location','$filter','$http','localStorageService',
function ($q, $injector, authService, appConstant, $rootScope, DATA_URLS,toastr,$location,$filter,$http,localStorageService) {

    'use strict';
    var commonServiceFactory = {}

    var navigateUser = function () {
        var role = authService.authentication.loginData.role

        if(role == "Admin"){
            $location.path('/admin');
        }
        else if(role == "Member"){
            $location.path('/member');
        }
        else if(role == "Tenant"){
            $location.path('/tenant');
        }
        else if(role == "MFamily"){
            $location.path('/mfamily');
        }
        else if(role == "TFamily"){
            $location.path('/tfamily');
        }
    }
    
    var distinctByName = function(jsonData) {

        var lookup = {};
        var items = jsonData;
        var lst = [];

        for (var item, i = 0; item = items[i++];) {
            var name = item.name;

            if (!(name in lookup)) {
                lookup[name] = 1;
                lst.push(name);
            }
        }
            
        var result = [];
        angular.forEach(lst, function (v) {
            var obj = $filter('filter')(items, { name: v }, true)[0];
            result.push(obj);
        });

        return result;
    }

    var selectSociety = function(soc) {

        var promise = $http.post(DATA_URLS.GET_SOCIETY, soc.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                var editFormData = data;

                editFormData.dateOfIncorporation = new Date($filter('date')(editFormData.dateOfIncorporation, appConstant.ConversionDateFormat));
                editFormData.dateOfRegistration = new Date($filter('date')(editFormData.dateOfRegistration, appConstant.ConversionDateFormat));
                
                localStorageService.set('onLogSoc', JSON.stringify(editFormData));
            }
        });
    }

    commonServiceFactory.selectSociety = selectSociety;
    commonServiceFactory.navigateUser = navigateUser;
    commonServiceFactory.distinctByName = distinctByName;
    return commonServiceFactory;
}]);