using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.OESK.Class
{
    public class OperationalRecord
    {

        public OperationalRecord()
        {

        }


      

        static BindingList<OperationalRecord> listOfOperationalRecordHistory = new BindingList<OperationalRecord>();

        public BindingList<OperationalRecord> ListOfOperationalRecordHistory
        {
            get { return listOfOperationalRecordHistory; }
            set { listOfOperationalRecordHistory = value; }
        }


        static BindingList<OperationalRecord> listOfOperationalRecord = new BindingList<OperationalRecord>();

       public BindingList<OperationalRecord> ListOfOperationalRecord
        {
            get { return listOfOperationalRecord; }
            set { listOfOperationalRecord = value; }
        }


        static BindingList<OperationalRecord> listOfOperationalRecordDuble = new BindingList<OperationalRecord>();

        public BindingList<OperationalRecord> ListOfOperationalRecordDuble
        {
            get { return listOfOperationalRecordDuble; }
            set { listOfOperationalRecordDuble = value; }
        }

        public string ID { get; set; }
        public Bitmap Lock { get; set; }
        public DateTime DateBegin { get; set; }
  
        public string WhoText { get; set; }
        public string ObjectName { get; set; }
        public string Message { get; set; }
        public string WhomText { get; set; }
        public string DateEnd2 { get; set; }
        public string ReportedName { get; set; }
        public byte AllZazem { get; set; }
        public Bitmap StImportantImage { get; set; }
        public string StNewEquipmentString { get; set; }
        public string StDefendString { get; set; }
        public string Nar  { get; set; }
        public byte StNar { get; set; }
        public string Note { get; set; }
        public string EmployeeName { get; set; }

        public byte Zazem1 { get; set; }
        public byte Zazem2 { get; set; }
        public byte Zazem3 { get; set; }

        public byte StNewEquipment { get; set; }
        public byte StDefend { get; set; }

        public byte StImportant { get; set; }
        public DateTime DateEnd { get; set; }
        public string EventID { get; set; }
        //public string EventName { get; set; }
        public string LineID { get; set; }
        public string LineName { get; set; }
        public string WhoID { get; set; }
        public string WhoName { get; set; }
        public string WhomID { get; set; }
        public string WhomName { get; set; }
        public string ProtectsID { get; set; }
        //public string ProtectsName { get; set; }
        public string ReportedID { get; set; }

        public string EmployeeID { get; set; }
        public string SlujbaID { get; set; }
        public string ClassVLID { get; set; }
        public string NarObject { get; set; }
        public string NarUser { get; set; }
        public string NarNum { get; set; }
        public string ObjectID { get; set; }
        public string ShiftSheduleID { get; set; }

        public string OrganizationID { get; set; }

        static string selectedID;
        public string SelectedID
        {
            get { return selectedID; }
            set { selectedID = value; }
        }

        static byte selectedFunc = 0;
        public byte SelectedFunc
        {
            get { return selectedFunc; }
            set { selectedFunc = value; }
        }

    }
}
