using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispetcherRes.Class
{
   public class Employee: SqlLayer.SqlLayer
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Pass { get; set; }
        public string SlujbaID { get; set; }
        public string SlujbaName { get; set; }
        public  byte Lvl { get; set; }
        public string ShiftSheduleID { get; set; }

        static string selectIDSlujba;
        public string SelectIDSlujba
        {
            get{ return selectIDSlujba; }
            set{ selectIDSlujba = value; }
        }

        static string selectRegionID;
        public string SelectRegionID
        {
            get { return selectRegionID; }
            set { selectRegionID = value; }
        }

        static string selectNameSlujba;
        public string SelectNameSlujba
        {
            get { return selectNameSlujba; }
            set { selectNameSlujba = value; }
        }

        static string selectIDEmployee = "00000000-0000-0000-0000-000000000000";
        public string SelectIDEmployee
        {
            get { return selectIDEmployee; }
            set { selectIDEmployee = value; }
        }


        static string selectNameEmployee;
        public string SelectNameEmployee
        {
            get { return selectNameEmployee; }
            set { selectNameEmployee = value; }
        }


        static string selectShiftShedule = "00000000-0000-0000-0000-000000000000";
        public string SelectShiftShedule
        {
            get { return selectShiftShedule; }
            set { selectShiftShedule = value; }
        }


        static string employeePCInfo = null;
        public string EmployeePCInfo
        {
            get { return employeePCInfo; }

            set { employeePCInfo = value; }
        }

        static string employeeIPInfo = null;
        public string EmployeeIPInfo
        {
            get { return employeeIPInfo; }
            set { employeeIPInfo = value; }
        }

        static int selectShiftID;
        public int SelectShiftID
        {
            get { return selectShiftID; }
            set { selectShiftID = value; }
        }


        static int selectShiftState;
        public int SelectShiftState
        {
            get { return selectShiftState; }
            set { selectShiftState = value; }
        }


        static byte sqlError =0;
        public byte SqlError
        {
            get { return sqlError; }
            set { sqlError = value; }
        }
    }
}
