using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.OESK.Class
{
    public  class AllChanges
    {
        public AllChanges()
        {

        }

        static Int32 operationalRecordNum;
        public Int32 OperationalRecordNum
        {
            get { return operationalRecordNum; }
            set { operationalRecordNum = value; }
        }

        static Int32 lockedNum;
        public Int32 LockedNum
        {
            get { return lockedNum; }
            set { lockedNum = value; }
        }

        static Int32 mail;
        public Int32 Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        static string shiftSheduleID;
        public string ShiftSheduleID
        {
            get { return shiftSheduleID; }
            set { shiftSheduleID = value; }
        }

    }
}
