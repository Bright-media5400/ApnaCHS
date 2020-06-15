var app = angular.module('app', ['LocalStorageModule', 'toastr', 'ngRoute', 'ui.router', 'bt.multiselect']);

app.directive('moveFocus', function () {
    return {
        restrict: "A",
        link: function (scope, elem, attrs) {
            elem.on('input', function (e) {
                var l = elem.val().length;
                var num = elem.context.value;
                if ((l == 0 && num == '') || (num.includes('.'))) {
                    elem.context.value = num.split('.').join('');
                }
                num = elem.context.value;
                if (l >= elem.attr("maxlength")) {
                    if (!isNaN(num) && Number.isInteger(+num) && num >= 0 && num < 100) {
                        if (elem.attr("id") == "saleNumber1")
                            document.querySelector('#saleNumber2').focus();
                        else {
                            if (elem.attr("id") == "saleNumber2") {
                                if (document.getElementById("saleNumber3"))
                                    document.querySelector('#saleNumber3').focus();
                                else
                                    document.querySelector('#amtNumber').focus();
                            }
                            else
                                document.querySelector('#amtNumber').focus();
                        }
                    }
                    else {
                        elem.context.value = '';
                    }
                }
            });
        }
    };
});

app.run(function (authService) {
    //authService.fillAuthData();
});

app.run(function ($rootScope,$location) {
  $rootScope.$on('$routeChangeStart', function (e) {
    console.log($location.path());
  });
});

app.config(function ($httpProvider, $locationProvider) {
    $locationProvider.hashPrefix('');
    $httpProvider.interceptors.push('authInterceptorService');
});
