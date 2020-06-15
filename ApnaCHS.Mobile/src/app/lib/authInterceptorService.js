'use strict';
app.service('authInterceptorService', ['$q', '$injector', '$location', 'localStorageService', 'authService', function ($q, $injector, $location, localStorageService, authService) {

    var service = this;

    service.request = function (config) {

        var accesstoken = authService.authentication.isAuth ? authService.authentication.loginData.access_token : null;
        if (accesstoken) {
            config.headers.authorization = 'Bearer ' + accesstoken;
        }
        return config;
    };

    service.response = function (response) {

        if (response.headers()['content-type'] === "application/json; charset=utf-8") {
            return response.data;
        } else {
            return response;
        }
    };

    service.responseError = function (rejection) {

        var deferred = $q.defer();

        if (rejection.status === 401) {
            console.log('rejection.status === 401 -', rejection);
            if (authService.authentication.isAuth) {
                if (!authService.refreshStatus()) {

                    authService
                        .refreshToken()
                        .then(function (response) {
                            _retryHttpRequest(rejection.config, deferred);
                        }, function () {

                            authService.logout();
                            //$location.path('/login');
                            deferred.reject(_returnResponse(rejection));
                        });
                }
                else {

                    var myVar = setInterval(function () {

                        console.log('setInterval');

                        if (!authService.refreshStatus()) {

                            clearInterval(myVar);
                            if (authService.authentication.isAuth) {
                                _retryHttpRequest(rejection.config, deferred);
                            }
                        }

                        //deferred.reject(_returnResponse(rejection));
                    }, 1000);
                }
            }
        }
        else if (rejection.status === 200) {

            deferred.resolve(_returnResponse(rejection));
            //return response;
        }
        else if (rejection.status === 400) {

            var msg = !!rejection.data.message ? rejection.data.message : rejection.data;

            if (typeof msg === 'string') {
                toastr["error"](msg, "Error");
                deferred.resolve(_returnResponse(rejection));
            }
            else {
                toastr["error"]('Invalid or expired session. Re-login again', "Error");
                deferred.reject(_returnResponse(rejection));
            }
        }
        else if (rejection.status === 500) {
            toastr["error"](rejection.data.exceptionMessage, "Error");
            
            deferred.resolve(_returnResponse(rejection));
        }
        else if (rejection.status === 403) {
            toastr["error"](rejection.statusText, "Error");
            deferred.resolve(_returnResponse(rejection));
        }
        else {
            toastr["error"](rejection.statusText, "Error");
            deferred.resolve(_returnResponse(rejection));
        }

        return deferred.promise;
    };

    var _retryHttpRequest = function (config, deferred) {
        var $http = $injector.get('$http');
        $http(config).then(function (response) {
            deferred.resolve(response);
        }, function (response) {
            deferred.reject(response);
        });
    }

    var _returnResponse = function (response) {
        return response;
        //if (response.headers()['content-type'] === "application/json; charset=utf-8") {
        //    return response.data;
        //} else {
        //    return response;
        //}
    }
}]);
