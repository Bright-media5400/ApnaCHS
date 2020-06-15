'use strict';
app.factory('authService', ['$q', '$injector', 'localStorageService', 'appConstant', '$rootScope', 'DATA_URLS',
function ($q, $injector, localStorageService, appConstant, $rootScope, DATA_URLS) {

    'use strict';
    var authServiceFactory = {}
    var _isRefreshToken = false;

    var authentication = localStorageService.get('authentication') ||
    {
        isAuth: false,
        loginError: '',
        loginData: {}
    }

    var authStatusChanged = function () {
        $rootScope.$broadcast('authStatusChanged');
        var phase = $rootScope.$$phase;
        if (!(phase === '$apply' || phase === '$digest')) {
            $rootScope.$apply();
        }
    };

    var clearAuthData = function () {
        authentication.isAuth = false;
        authentication.loginError = '';
        authentication.loginData = {};

        localStorageService.remove('authentication');
    }

    var login = function (formdata) {

        var loginData = {
            grant_type: 'password',
            UserName: formdata.username + (formdata.localLogin ? ",Local" : ""),
            password: formdata.password,
        };

        //if (formdata.instance) {
        //    loginData.instance = formdata.instance.id;
        //}

        var promise = $.ajax({
            type: 'POST',
            url: DATA_URLS.ROOT_PATH + 'Token',
            data: loginData
        });

        promise
            .done(function (data) {

                authentication.isAuth = true;
                authentication.loginData = {
                    access_token: data.access_token,
                    expires_in: data.expires_in,
                    userName: data.userName,
                    instance:data.instance,
                    userid: data.userid,
                    refresh_token: data.refresh_token,
                    userFullName: data.userFullName,
                    type: data.type,
                    societies: data.societies,
                    complexes: data.complexes
                };

                localStorageService.set('authentication', authentication);
                authStatusChanged();
                //console.log('authentication.loginData -', authentication.loginData);
            })
            .fail(function (jqXHR) {
                clearAuthData();
                console.log(jqXHR.responseText);
                if (jqXHR.responseText === undefined) {
                    authentication.loginError = 'Could not connect. Check your internet connection or try again later';
                } else {
                    authentication.loginError = JSON.parse(jqXHR.responseText).error_description;
                }
                authStatusChanged();
            });
    }

    var logout = function () {

        var headers = {};
        var accesstoken = authentication.isAuth ? authentication.loginData.access_token : null;
        if (accesstoken) {
            headers.Authorization = 'Bearer ' + authentication.loginData.access_token;
        }

        clearAuthData();
        authStatusChanged();

        $.ajax({
            type: 'POST',
            url: DATA_URLS.USER_LOGOUT,
            headers: headers
        });
    }

    var refreshToken = function () {
        var deferred = $q.defer();

        var authData = authentication.loginData;
        var data = "grant_type=refresh_token&refresh_token=" + authData.refresh_token + "&client_id=" + appConstant.ClientId;


        var $http = $injector.get('$http');
        if (_isRefreshToken) return;
        _isRefreshToken = true;

        $http.post(DATA_URLS.ROOT_PATH + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
            .success(function (response) {

                _isRefreshToken = false;

                authentication.isAuth = true;
                authentication.loginData = {
                    access_token: response.access_token,
                    expires_in: response.expires_in,
                    permissions: response.permissions,
                    userName: response.userName,
                    userid: response.userid,
                    refresh_token: response.refresh_token,
                    userFullName: data.userFullName,
                    type: data.type,
                    societies: data.societies,
                    complexes: data.complexes
                };

                localStorageService.set('authentication', authentication);
                deferred.resolve(response);

            }).error(function (err, status) {
                _isRefreshToken = false;
                logout();
                deferred.reject(err);
            });

        return deferred.promise;
    };

    var refreshStatus = function () {
        return _isRefreshToken;
    }

    authServiceFactory.authentication = authentication;
    authServiceFactory.login = login;
    authServiceFactory.logout = logout;
    authServiceFactory.refreshToken = refreshToken;
    authServiceFactory.refreshStatus = refreshStatus;
    return authServiceFactory;

}]);