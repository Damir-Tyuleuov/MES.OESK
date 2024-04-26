using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.OESK.Class
{
    public class ShiftShedule
    {
        static List<ShiftShedule> listShiftShedule = new List<ShiftShedule>();
        public List<ShiftShedule> ListShiftShedule
        {
            get { return listShiftShedule; }
            set { listShiftShedule = value; }
        }


        static List<ShiftShedule> listShiftSheduleHistory = new List<ShiftShedule>();
        public List<ShiftShedule> ListShiftSheduleHistory
        {
            get { return listShiftSheduleHistory; }
            set { listShiftSheduleHistory = value; }
        }

        static DateTime serverTime;
       public DateTime ServerTime
        {
            set { serverTime = value; }
            get
            { return serverTime; }
        }



        static Byte historyMod;
        public Byte HistoryMod
        {
            set { historyMod = value; }
            get
            { return historyMod; }
        }


        static string historyshiftShedule;
        public string HistoryshiftShedule
        {
            set { historyshiftShedule = value; }
            get
            { return historyshiftShedule; }
        }


        string shiftSheduleID;
        public string ShiftSheduleID
        {
            set { shiftSheduleID = value; }
            get { return shiftSheduleID; }
        }

        string shiftID;
        public string ShiftID
        {
            set { shiftID = value; }
            get { return shiftID; }
        }

        DateTime shiftSheduleDate;
        public DateTime ShiftSheduleDate
        {
            set { shiftSheduleDate = value; }
            get { return shiftSheduleDate; }
        }

        int isClosed;
        public int IsClosed
        {
            set { isClosed = value; }
            get { return isClosed; }
        }

        string shiftSheduleStartDate;
        public string ShiftSheduleStartDate
        {
            set { shiftSheduleStartDate = value; }
            get { return shiftSheduleStartDate; }
        }

        string shiftSheduleEndDate;
        public string ShiftSheduleEndDate
        {
            set { shiftSheduleEndDate = value; }
            get { return shiftSheduleEndDate; }
        }

        string slujbaID;
        public string SlujbaID
        {
            set { slujbaID = value; }
            get { return slujbaID; }
        }

        string shiftSheduleEmployee;
        public string ShiftSheduleEmployee
        {
            set { shiftSheduleEmployee = value; }
            get { return shiftSheduleEmployee; }
        }

        string shiftSheduleEmployeeName;
        public string ShiftSheduleEmployeeName
        {
            set { shiftSheduleEmployeeName = value; }
            get { return shiftSheduleEmployeeName; }
        }



    }
}
