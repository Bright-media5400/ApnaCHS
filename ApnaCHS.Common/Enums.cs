using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.AppCommon
{
    public enum EnModuleList : int
    {
        User = 1,
        Roles = 2,
        Permission = 3,
        CustomField = 4,
        UserDefinedList = 5,
        Society = 6
    }

    public enum EnActionList : int
    {
        ViewOrList = 1,
        Add = 2,
        PrintOrExport = 3,
        EditOwn = 4,
        EditAll = 5,
        DeleteOwn = 6,
        DeleteAll = 7
    }

    public enum EnFloorType
    {
        Floor = 1,
        Parkings = 2,
        CommercialSpace = 3,
    }

    public enum EnFacilityType
    {
        Building = 1,
        RowHouses = 2,
        OpenParking= 3,
        ParkingTower = 4,
        Garden = 5,
        ClubHouse = 6,
        Gym = 7,
        SwimmingPool = 8,
        CommunityHall = 9,
        PlayGround = 10,
        PlayArea = 11,
        CommercialSpace = 12,
        School = 13,
        Hospital = 14,
        Temple = 15,
        Mosque = 16,
        Curch = 17,
        Gurudwada = 18,
        
    }

    public enum EnComplexType
    {
        SingleSociety = 1,
        GroupOfSocieties = 2
    }

    public enum EnMasterTypeList
    {
        FlatTypes = 1,
        ParkingType = 2,
        StaffShiftType = 3,
        StaffType = 4,
        City = 5,
        State = 6,
        MaintenanceCostDefinition = 7
    }

    public enum EnParkingType : byte
    {
        TwoWheeler = 1,
        FourWheeler = 2,
        TwoFourWheeler = 3
    }
    public enum EnOwnerType : byte
    {
        Owner = 1,
        Tenant = 2,
        PayingGuest = 3
    }

    public enum EnBillType : byte
    {
        Monthly = 1,
        Opening = 2
    }
}
