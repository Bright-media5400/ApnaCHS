using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.AppCommon
{
    public static class ProgramCommon
    {
        public static int MaxAttempts = 5;

        public static string SocietyRoleName = "{0}-S-{1}";
        public static string ComplexRoleName = "{0}-C-{1}";

        public static int BillingCycle = 5;
        public static int DueDays = 15;

        public static int Frontend = 2;
        public static int MemberRoleId = 5;
        public static int MemberFamily = 8;

        public static int SuperAdmin = 3;
        public static int Admin = 4;

        public static int TenantRoleId = 7;
        public static int TenantFamily = 9;

        public static string GetFormattedUsername(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                throw new InvalidOperationException("Value cannot be null");
            }

            return string.Format("{0}{1}", val.Replace(" ", string.Empty).ToLower(), DateTime.Now.ToString("ddMMyyyyhhmmss"));
        }

        public static string GetMonthName(int val)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(val);
        }
    }

    public enum EnMasterValueType : byte
    {
        FlatTypes = 1,
        ParkingType = 2,
        StaffShiftType = 3,
        StaffType = 4,
        City = 5,
        State = 6,
        MaintenanceCostDefinition = 7,
        Gender = 8,
        Relationship = 9
    }
}
