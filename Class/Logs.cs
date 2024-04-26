using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispetcherRes.Class;
using DispetcherRes.SqlLayer;
using MES.OESK.Class;

namespace MES.OESK.Class
{
 
    public class Logs
    {
        SqlLayer SL = new SqlLayer();
        Employee EM = new Employee();
        public string Actions { get; set; }
        public string InfoID { get; set; }
        public string EmployeeID { get; set; }
        public string ShiftSheduleID { get; set; }
        public string InfoPC { get; set; }
        public string InfoIP { get; set; }

        static List<Logs> listLogs = new List<Logs>();
        public List<Logs> ListLogs
        {
            get { return listLogs; }
            set { listLogs = value; }
        }

        public void RecordLogs(string Actions, string InfoID)
        {
            ListLogs.Clear();
          
                ListLogs.Add(new Logs
                {
                    Actions = Actions,
                    InfoID = InfoID,
                    EmployeeID = EM.SelectIDEmployee,
                    ShiftSheduleID = EM.SelectShiftShedule,
                    InfoPC = EM.EmployeePCInfo,
                    InfoIP = EM.EmployeeIPInfo
                });
          

            SL.RecordLogs(ListLogs);
        }
    }
}
