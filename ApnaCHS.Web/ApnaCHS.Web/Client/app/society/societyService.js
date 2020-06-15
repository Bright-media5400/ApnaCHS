'use strict';
app.factory('societyService', ['$q', '$injector', 'localStorageService', 'appConstant', '$rootScope',
function ($q, $injector, localStorageService, appConstant, $rootScope) {

    'use strict';
    var societyServiceFactory = {}

    var setSociety = function (society) {

        localStorageService.set('onLogSoc', JSON.stringify(society));
    }

    var getSociety = function () {

        var soc = JSON.parse(localStorageService.get('onLogSoc'));

        return soc;
    }

    societyServiceFactory.setSociety = setSociety;
    societyServiceFactory.getSociety = getSociety;

    return societyServiceFactory;

}]);