var app = angular.module('app', ['LocalStorageModule', 'toastr', 'ngRoute', 'ui.router']);

app.run(function (authService) {
    //authService.fillAuthData();
});

app.run(function ($rootScope,$location) {
  $rootScope.$on('$routeChangeStart', function (e) {
    console.log($location.path());
  });
});

app.config(function ($httpProvider, $locationProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});
