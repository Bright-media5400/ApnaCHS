app.constant('appConstant', {
    appBaseurl: '/',
    DateFormat: 'dd/MM/yyyy',
    ExcelFormat: 'dd/MMM/yyyy',
    ConversionDateFormat: 'MM/dd/yyyy',
    ClientId: 'ngAuthApp',
    MasterTypeList: [
        { id: 1, text: 'Flat Types' },
        { id: 3, text: 'Staff Shift Type ' },
        { id: 4, text: 'Staff Type' },
        { id: 5, text: 'City' },
        { id: 6, text: 'State' },
        { id: 7, text: 'Maintenance Cost Definition' },
    ],
    MasterType: {
        FlatTypes: 1,
        ParkingType: 2,
        StaffShiftType: 3,
        StaffType: 4,
        City: 5,
        State: 6,
        MaintenanceCostDefinition: 7
    },
    ComplexType: [
        { id: 1, text: 'Single Society' },
        { id: 2, text: 'Group of Societies' }
    ],
    FacilityType: [
        { id: 1, text: 'Building', imageclass: 'fa fa-building' },
        { id: 2, text: 'Row Houses', imageclass: 'fa fa-home' },
        { id: 3, text: 'Open Parking', imageclass: 'fa fa-motorcycle' },
        { id: 4, text: 'Parking Tower', imageclass: 'fa fa-car' },
        { id: 5, text: 'Garden', imageclass: 'fa fa-tree' },
        { id: 6, text: 'Club House', imageclass: 'fa fa-building' },
        { id: 7, text: 'Gym', imageclass: 'fa fa-building' },
        { id: 8, text: 'Swimming Pool', imageclass: 'fa fa-building' },
        { id: 9, text: 'Community Hall', imageclass: 'fa fa-building' },
        { id: 10, text: 'Play Ground', imageclass: 'fa fa-building' },
        { id: 11, text: 'Play Area', imageclass: 'fa fa-street-view' },
        { id: 12, text: 'Commercial Space', imageclass: 'fa fa-building' },
        { id: 13, text: 'School', imageclass: 'fa fa-building' },
        { id: 14, text: 'Hospital', imageclass: 'fa fa-hospital-o' },
        { id: 15, text: 'Temple', imageclass: 'fa fa-building' },
        { id: 16, text: 'Mosque', imageclass: 'fa fa-building' },
        { id: 17, text: 'Curch', imageclass: 'fa fa-church' },
        { id: 18, text: 'Gurudwada', imageclass: 'fa fa-building' },
    ],
    FloorType: [
        { id: 1, text: 'Floor', imageclass: 'fa fa-building' },
        { id: 2, text: 'Parkings', imageclass: 'fa fa-car' },
        { id: 3, text: 'Shops', imageclass: 'fa fa-building-o' },
        { id: 4, text: 'Offices', imageclass: 'fa fa-building-o' },

    ],
    ParkingType: [
        { id: 1, text: 'Two Wheeler' },
        { id: 2, text: 'Four Wheeler' },
        { id: 3, text: 'Two/Four Wheeler' }
    ],
    VechileType: [
        { id: 1, text: 'Two Wheeler' },
        { id: 2, text: 'Four Wheeler' },
        { id: 3, text: 'Other' }
    ],
    PaymentMode: [
        { id: 1, text: 'Cash' },
        { id: 2, text: 'Cheque' },
        { id: 3, text: 'NEFT' },
        { id: 4, text: 'Paytm' },
        { id: 5, text: 'Google Pay' },
        { id: 6, text: 'Bill Desk' },
        { id: 7, text: 'Debit/Credit Card' }
    ]

});



app.constant('DATA_URLS', (function () {
    'use strict';

    var URLS,
        //baseUrl = 'http://localhost:29479/';
        baseUrl = 'http://13.71.116.32:2052/';


    URLS = {
        ROOT_PATH: baseUrl,
        DEFAULT_PATH: '/dashboard',

        REPORT_FLATS: baseUrl + 'api/Flat/Report',
        GET_FLAT: baseUrl + 'api/Flat/Read',
        GET_SOCIETY: baseUrl + 'api/Society/Read',
        LIST_BILLING: baseUrl + 'api/Billing/List',
        LIST_BILLINGTRANSACTIONS: baseUrl + 'api/BillingTransaction/List',
        MYHISTORY_BILLINGTRANSACTIONS: baseUrl + 'api/BillingTransaction/MyHistory',
        CURRENTDUES_BILLING: baseUrl + 'api/Billing/MyCurrentDues',
        BILLING_MYBILLS: baseUrl + 'api/Billing/MyBills',
        GET_BILLING: baseUrl + 'api/Billing/Read',
        LIST_SECURITYSTAFF: baseUrl + 'api/SecurityStaff/List',
        LIST_SOCIETYSTAFF: baseUrl + 'api/SocietyStaff/List',
        REPORT_FLATS: baseUrl + 'api/Flat/Report',
        SOCIETYWISELIST_FACILITIES: baseUrl + 'api/Facility/SocietyWiseList',
        GET_FACILITY: baseUrl + 'api/Facility/Read',
        GET_FLOOR: baseUrl + 'api/Floor/Read',
        LIST_SOCIETYASSETS: baseUrl + 'api/SocietyAsset/List',
        LIST_VEHICLES: baseUrl + 'api/Vehicle/List',
        LIST_FLATOWNERFAMILIES: baseUrl + 'api/FlatOwnerFamily/List'
   };

    return URLS;
}()))