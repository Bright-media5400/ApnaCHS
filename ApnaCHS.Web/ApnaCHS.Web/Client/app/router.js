app.config(['$routeProvider', 'DATA_URLS', '$locationProvider', function ($routeProvider, DATA_URLS, $locationProvider) {

    $routeProvider.
    when('/', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/dashboard/dashboard.html',
        controller: 'dashboardController'
    })
    .when('/dashboard', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/dashboard/dashboard.html',
        controller: 'dashboardController'
    })
    .when('/unauthorized',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/common/unauthorized.html',
    })
    .when('/users',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/user/userList.html',
        controller: 'userListController'
    })
    .when('/user/new',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/user/newUser.html',
        controller: 'newUserController'
    })
    .when('/user/:uid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/user/userDetails.html',
        controller: 'userDetailsController'
    })
    .when('/roles',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/usergroup/userGroupList.html',
        controller: 'userGroupListController'
    })
    .when('/role/new',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/usergroup/newUserGroup.html',
        controller: 'newUserGroupController'
    })
    .when('/role/:uid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/usergroup/userGroupDetails.html',
        controller: 'userGroupDetailsController'
    })
    .when('/society/list',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/societyList.html',
        controller: 'societyListController'
    })
    .when('/society/new',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/newSociety.html',
        controller: 'newSocietyController'
    })
    .when('/society/details/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/societyDetails.html',
        controller: 'societyDetailsController'
    })
    .when('/society/:sid/flats',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flat/flatList.html',
        controller: 'flatListController'
    })
    .when('/complex/list',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/complex/complexList.html',
        controller: 'complexListController'
    })
    .when('/complex/new',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/complex/newComplex.html',
        controller: 'newComplexController'
    })
    .when('/complex/details/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/complex/complexDetails.html',
        controller: 'complexDetailsController'
    })
    .when('/complex/payments',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/complex/paymentList.html',
        controller: 'paymentListController'
    })
    .when('/facility/details/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/facility/facilityDetails.html',
        controller: 'facilityDetailsController'
    })
    .when('/floor/details/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/floor/floorDetails.html',
        controller: 'floorDetailsController'
    })
    .when('/flat/details/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flat/flatDetails.html',
        controller: 'flatDetailsController'
    })
    .when('/societystaff/list/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/societystaff/societystaffList.html',
        controller: 'societystaffListController'
    })
    .when('/securitystaff/list/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/securitystaff/securitystaffList.html',
        controller: 'securitystaffListController'
    })
    .when('/flat/details/:sid/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flat/flatDetails.html',
        controller: 'flatDetailsController'
    })
    .when('/flatowner/details/:oid/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flatowner/flatownerDetails.html',
        controller: 'flatownerDetailsController'
    })
    .when('/tenant/details/:tid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/tenant/tenantDetails.html',
        controller: 'tenantDetailsController'
    })
    .when('/flat/details/:sid/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/flat/flatDetails.html',
        controller: 'flatDetailsController'
    })
    .when('/masters',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/mastervalue/mastervalueList.html',
        controller: 'mastervalueListController'
    })
    .when('/costs',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/usergroup/maintenanceCostList.html',
        controller: 'maintenanceCostListController'
    })
    .when('/complex/list',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/complex/complexList.html',
        controller: 'complexListController'
    })
     .when('/maintainance/receipt/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/receipt/receiptList.html',
        controller: 'receiptListController'
    })
        .when('/costdefinition/list',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/costdefinition/costDefinitionList.html',
        controller: 'costDefinitionListController'
    })
        .when('/costdefinition/new',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/costdefinition/newCostDefinition.html',
        controller: 'newCostDefinitionController'
    })
    .when('/billingtransaction/new/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/billingtransaction/newBillingTransaction.html',
        controller: 'newBillingTransactionController'
    })
     .when('/billingtransaction/flat/:fid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/billingtransaction/newBillingTransaction.html',
        controller: 'newBillingTransactionController'
    })
     .when('/society/payment/history/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/billingtransaction/billingHistory.html',
        controller: 'billingHistoryController'
    })
     .when('/society/pendingbills/:sid',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/billingtransaction/pendingBill.html',
        controller: 'pendingBillController'
    })
    .when('/complex/select',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/complex/selectcomplex.html',
        controller: 'selectcomplexController'
    })
    .when('/society/select',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/selectsociety.html',
        controller: 'selectsocietyController'
    })
    .when('/society/members',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/societymembers.html',
        controller: 'societymembersController'
    })
    .when('/society/tenants',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/societytenants.html',
        controller: 'societytenantsController'
    })
    .when('/society/admins',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/societyadmins.html',
        controller: 'societyadminsController'
    })
    .when('/society/user/new',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/society/societyUserNew.html',
        controller: 'societyUserNewController'
    })
    .when('/uploadflats',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/bulkuploads/uploadFlats.html',
        controller: 'uploadFlatsController'
    })
    .when('/uploadfamilies',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/bulkuploads/uploadFamilies.html',
        controller: 'uploadFamiliesController'
    })
    .when('/uploadflatowners',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/bulkuploads/uploadFlatOwners.html',
        controller: 'uploadFlatOwnersController'
    })
    .when('/uploadflattenants',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/bulkuploads/uploadFlatTenants.html',
        controller: 'uploadFlatTenantsController'
    })
    .when('/uploadvehicles',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/bulkuploads/uploadVehicles.html',
        controller: 'uploadVehiclesController'
    })
    .when('/uploadopeningbills',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/bulkuploads/uploadOpeningBills.html',
        controller: 'uploadOpeningBillsController'
    })
    .when('/approvals/maintenancecosts',
    {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/approvals/approveMaintenancecosts.html',
        controller: 'approveMaintenancecostsController'
    })
    .when('/approvals/flats', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/approvals/approveFlats.html',
        controller: 'approveFlatsController'
    })
    .when('/approvals/flatowners', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/approvals/approveFlatowners.html',
        controller: 'approveFlatownersController'
    })
    .when('/approvals/flatownerFamilies', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/approvals/approveFlatownerFamilies.html',
        controller: 'approveFlatownerFamiliesController'
    })
    .when('/approvals/vehicles', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/approvals/approveVehicles.html',
        controller: 'approveVehiclesController'
    })
    .when('/approvals/tenats', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/approvals/approveTenants.html',
        controller: 'approveTenantsController'
    })
    .when('/maintenancecost/list', {
        templateUrl: DATA_URLS.ROOT_PATH + 'Client/app/maintenancecost/maintenanceCostList.html',
        controller: 'maintenanceListController'
    })
     .otherwise({
         redirectTo: '/'
     });

    // use the HTML5 History API
    //$locationProvider.html5Mode(true);
}]);


