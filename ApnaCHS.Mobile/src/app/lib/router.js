app.config(['$routeProvider', function ($routeProvider) {

    $routeProvider
    .when('/', {
        templateUrl: 'app/login/login.html'
    })
    .when('/admin', {
        templateUrl: 'app/home/super-admin-homepage.html'
    })
    .when('/member', {
        templateUrl: 'app/home/society-member-homepage.html'
    })
    .when('/tenant', {
        templateUrl: 'app/home/tenant-homepage.html'
    })
    .when('/mfamily', {
        templateUrl: 'app/home/owner-family-homepage.html'
    })
    .when('/tfamily', {
        templateUrl: 'app/home/tenant-homepage.html'
    })
    .when('/maintenancehome', {
        templateUrl:'app/maintenance/maintenanceHome.html',
        controller:'maintenanceHomeController'
    })
    .when('/currentDues', {
        templateUrl:'app/maintenance/member/currentDues.html',
        controller:'currentDuesController'
    })
    .when('/previousBills', {
        templateUrl:'app/maintenance/member/previousBills.html',
        controller:'previousBillsController'
    })
    .when('/transactionHistory', {
        templateUrl:'app/maintenance/member/transactionHistory.html',
        controller:'transactionHistoryController'
    })
    .when('/bill/details/:bid', {
    
        templateUrl:'app/maintenance/member/billDetails.html',
        controller: 'billDetailsController'
    })
    .when('/flat/list/member', {
    
        templateUrl:'app/flat/member/flatList.html',
        controller: 'flatListController'
    })
    .when('/flat/details/:fid', {
    
        templateUrl:'app/flat/member/flatDetails.html',
        controller: 'flatDetails'
    })
    .when('/complex/select',
    {
        templateUrl: 'app/complex/selectcomplex.html',
        controller: 'selectcomplexController'
    })
    .when('/society/select',
    {
        templateUrl: 'app/society/selectsociety.html',
        controller: 'selectsocietyController'
    })
    .when('/member/society/details',
    {
        templateUrl: 'app/society/member/societyDetails.html',
        controller: 'societyDetailsController'
    })
    .when('/admin/society/details',
    {
        templateUrl: 'app/society/admin/societyDetails.html',
        controller: 'societyDetailsController'
    })
    .when('/society/securitystafflist/:sid',
    {
        templateUrl: 'app/securitystaff/member/securityStaff.html',
        controller: 'securityStaffListController'
    })
    .when('/society/societystafflist/:sid',
    {
        templateUrl: 'app/societystaff/member/societyStaff.html',
        controller: 'societyStaffListController'
    })
    .when('/flat/flatreport/:sid',
    {
        templateUrl: 'app/flat/flatReport.html',
        controller: 'flatReportController'
    })
    .when('/facility/flacilitydetails/:sid',
    {
        templateUrl: 'app/facility/facilityDetails.html',
        controller: 'facilityDetailsController'
    })
    .when('/facility/flacilitylist/:sid',
    {
        templateUrl: 'app/facility/facilityList.html',
        controller: 'facilityListController'
    })
    .when('/floor/details/:sid',
    {
        templateUrl: 'app/floor/floorDetails.html',
        controller: 'floorDetailsController'
    })
    .when('/asset/list/:sid',
    {
        templateUrl: 'app/asset/assetList.html',
        controller: 'assetListController'
    })
    .otherwise({
        redirectTo: '/'
    });

    // use the HTML5 History API
    //$locationProvider.html5Mode(true);
}]);
