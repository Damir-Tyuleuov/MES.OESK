using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispetcherRes.SqlLayer;
using MES.OESK.Class;

namespace DispetcherRes.Class
{
    public class Blank

    {
        SqlLayer.SqlLayer SL = new SqlLayer.SqlLayer();
        Logs Logs = new Logs();
        public Blank()
        {

        }

        static int OperationalRecordNum = 0;
        public int operationalRecordNum
        {
            set { OperationalRecordNum = value; }
            get
            { return OperationalRecordNum; }
        }

        static byte stLocal = 0;
        public byte StLocal
        {
            get
            {
                return stLocal;
            }
            set
            {
                stLocal = value;
            }
        }

        BindingList<OperationalRecord> ListSynksOperationalRecord = new BindingList< OperationalRecord >();

        string SP;
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime Data { get; set; }

        public string IDOrganizations { get; set; }

        static List<Blank> listObjects = null;
        public List<Blank> ListObjectsCech
        {
            get { return listObjects; }
            set { listObjects = value; }
        }


        static List<Blank> listOfReporteds;
        public List<Blank> ListOfReporteds
        {
            get { return listOfReporteds; }
            set { listOfReporteds = value; }
        }

        static BindingList<Blank> listBOfReporteds = new BindingList<Blank>();
        public BindingList<Blank> ListBOfReporteds
        {
            get { return listBOfReporteds; }
            set { listBOfReporteds = value; }
        }


        static BindingList<Blank> listBOfOrganizations = new BindingList<Blank>();
        public BindingList<Blank> ListBOfOrganizations
        {
            get { return listBOfOrganizations; }
            set { listBOfOrganizations = value; }
        }


        //\\\\----Получение серверного времени
        public void GetServerTime() 
        {
            SP = "SP_GetServerTime";
            SL.GetServerTime(SP);
        
        }


        //\\\\----Открыть смену
        public void OpenShiftShedule(string ShiftSheduleID)
        {
            SP = "SP_OpenShiftShedule";
            SL.AddEditBlankParam(SP,
                ShiftSheduleID, "ShiftSheduleID",
                "", "",
                "", "",
                "", "",
                "", ""
                );
            Logs.RecordLogs("Открытие смены", ShiftSheduleID);
        }


        //\\\\----Закрыть смену
        public void CloseShiftShedule(string ShiftSheduleID)
        {
            SP = "SP_CloseShiftShedule";
            SL.AddEditBlankParam(SP,
                ShiftSheduleID, "ShiftSheduleID",
                "", "",
                "", "",
                "", "",
                "", ""
                );
            Logs.RecordLogs("Закрытие смены", ShiftSheduleID);
        }


        //\\\\----Добавление персонала смены
        public void AddEmployeeShiftShedule(string ShiftSheduleID, string EmployeeID)
        {
            SP = "SP_AddEmployeeShiftShedule";
            SL.AddEditBlankParam(SP,
                ShiftSheduleID, "ShiftSheduleID",
                EmployeeID,"EmployeeID",
                "", "",
                "", "",
                 "", ""
                );

                Logs.RecordLogs("Установка/редактирование дежурных смены", ShiftSheduleID);
        }


        //\\\\----Очистка смен
        public void ClearShiftShedule(string ShiftSheduleID)
        {
            SP = "SP_ClearShiftShedule";
            SL.AddEditBlankParam(SP,
                ShiftSheduleID, "ShiftSheduleID",
                "", "",
                "", "",
                "", "",
                 "", ""
                );
        }

        //\\\\----Получение шаблонов записей
        public void GetAppeals(string SlujbaID)
        {
            List<Appeals> AS = new List<Appeals>();
            SP = "SP_GetSamples";
            SL.GetBlankOfOneParam(AS, "SP_GetAppeals", SlujbaID, "SlujbaID");
            new Appeals().AppealsList = AS;
        }


        //\\\\----Получение шаблонов записей
        public List<Blank> GetSamples(List<Blank> SamplesList, string SlujbaID)
        {
            SamplesList.Clear();
            SP = "SP_GetSamples";
            SL.GetBlankOfOneParam(SamplesList, SP, SlujbaID, "SlujbaID");
            return SamplesList;
        }


        //\\\\----Добавление шаблонов записей в смену
        public void AddSamplesForOR()
        {
            SP = "SP_AddSamplesForOR";
            SL.AddSamplesForOR(SP);
        }


        //\\\\----Удалить запись
        public void DeleteOperationRecordOfID(string ID)
        {
            SP = "SP_DeleteOperationalRecordOfID";
            SL.DeleteOperationRecordOfID(SP, ID, new Employee().SelectIDSlujba);
        }


        //\\\\----Получение списка служб по региону
        public List<Blank> GetSlujba(List<Blank> ListSlujba,string IDRegion)
        {
            ListSlujba.Clear();
            SP = "SP_GetSlujba";
            SL.GetBlankOfOneParam(ListSlujba,SP, IDRegion, "IDRegion");
            return ListSlujba;
        }


        //\\\\----Получение объектов службы
        public List<Blank> GetObjects(List<Blank> ListObjects, string IDSlujba, string IDOrganazations)
        {
            ListObjects.Clear();

            ListObjects.Add(new Blank{ID= "00000000-0000-0000-0000-000000000000"});
            SP = "SP_GetObjectsOfSlujba";
            SL.GetOrganizations(ListObjects, SP, IDSlujba, "IDSlujba", IDOrganazations,"IDOrganizations");
            return ListObjects;
        }


        //\\\\----Получение линий по классу ВЛ
        public List<Blank> GetLineOfVL(List<Blank> ListLineOfVL, string IDClassVL, string ObjectID)
        {
            ListLineOfVL.Clear();
            SP = "SP_GetLineOfVL";
            ListLineOfVL.Add(new Blank { ID = "00000000-0000-0000-0000-000000000000" });
            SL.GetBlankOfTreeParam(ListLineOfVL, SP, IDClassVL, 
                "IDClassVL",new Employee().SelectIDSlujba, "SlujbaID"
                , ObjectID, "ObjectID");
            
            return ListLineOfVL;
        }


        //\\\\----Получение шаблонов фраз
        public List<Blank> GetPharaserOfEventsCategory(List<Blank> ListPharaserOfEventsCategory, string IDEventsCategory)
        {
            ListPharaserOfEventsCategory.Clear();
            SP = "SP_GetPharaserOfEventsCategory";
            SL.GetBlankOfOneParam(ListPharaserOfEventsCategory, SP, 
                IDEventsCategory, "IDEventsCategory"
                );
            return ListPharaserOfEventsCategory;
        }


        //\\\\----Получение классов напряжения
        public List<Blank> GetClassVL(List<Blank> ListClassVL)
        {
            ListClassVL.Clear();
            SP = "SP_GetClassVL";
            SL.GetBlank(ListClassVL, SP);
            return ListClassVL;
        }


        //\\\\----Получение классов напряжения объектов
        public List<Blank> GetObjectClassVL(List<Blank> ListClassVL, string ObjectID)
        {
            ListClassVL.Clear();
            SP = "SP_GetObjectClassVL";
            SL.GetBlankOfOneParam(ListClassVL, SP, ObjectID, "ObjectID");
            return ListClassVL;
        }


        //\\\\----Получение списка заблокированных
        public List<Blank> GetLocked (List<Blank> ListLocked)
        {
            ListLocked.Clear();
            SP = "SP_GetLocked";
            SL.GetBlank(ListLocked, SP);
            return ListLocked;
        }


        //\\\\----Получение классов напряжения
        public List<Blank> GetProtects (List<Blank> ListProtects)
        {
            ListProtects.Clear();
            SP = "SP_GetProtects";
            SL.GetBlank(ListProtects, SP);
            return ListProtects;
        }


        //\\\\----Добавление, редактирование сообщения
        public void AddEditReported(string ID, string Name)
        {
            SP = "SP_AddEditReported";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                Name.Trim(), "Name",
                "","",
                "", "",
                 "", ""
                );
        }


        //\\\\----Прочтение сообщения
        public void MailSTRead(string ID, string EmployeeID)
        {
            SP = "SP_MailSTRead";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                EmployeeID, "EmployeeID",
                "", "",
                "", "",
                 "", ""
                );
        }


        //\\\\----Отправка почты
        public void GiveMail(string SlujbaID, string Author, string Text, string WhomsSlujbaID)
        {
            SP = "SP_GiveMail";
            SL.AddEditBlankParam(SP,
                SlujbaID, "SlujbaID",
                Author, "Author",
                Text, "Text",
                WhomsSlujbaID, "WhomsSlujbaID",
                "",""
                );
        }


        //\\\\----Добавление, редактирование линий по классу ВЛ
        public void AddEditLine(string IDClassVL, string ID, string Name, string ObjectID)
        {
            SP = "SP_AddEditLineOfSlujb";
            SL.AddEditBlankParam(SP,
                new Employee().SelectIDSlujba, "SlujbaID",
                IDClassVL, "IDClassVL",
                ID,"ID",
                Name.Trim(), "Name",
                ObjectID, "ObjectID"
                );
            Logs.RecordLogs("Добавление/редактирование оборудования", ID);
        }


        //\\\\----Добавление, редактирование персонала
        public void AddEditPersonal(string ID, string Name, string SlujbaID)
        {
            SP = "SP_AddEditSprEmployeeOfSlujba";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                Name.Trim(), "Name",
                SlujbaID, "SlujbaID",
                "","",
                "",""
                );
            Logs.RecordLogs("Редактирование общего персонала", ID);
        }


        //\\\\----Добавление, редактирование линий по классу ВЛ
        public void AddEditObject(string ID, string Name, string ParrentID)
        {
            SP = "SP_AddEditObject";
            SL.AddEditBlankParam(SP,
                ID,"ID",
                Name.Trim(), "Name",
                ParrentID, "ParrentID",
                "", "",
                "",""
                );
            Logs.RecordLogs("Добавление/Редактирование объекта", ID);
        }


        //\\\\----Добавление, редактирование категорий
        public void AddEditEventsCategory(string ID, string Name, string SlujbaID)
        {
            SP = "SP_AddEditEventsCategory";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                Name.Trim(), "Name",
                SlujbaID, "SlujbaID",
                "", "",
                "", ""
                );
            Logs.RecordLogs("Добавление/Редактирование категорий фраз", ID);
        }


        //\\\\----Добавление, редактирование линий по классу ВЛ
        public void AddEditObjectOrganizations(string ID, string Name, string SlujbaID)
        {
            SP = "SP_AddEditOrganizations";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                Name.Trim(), "Name",
                SlujbaID, "SlujbaID",
                "", "",
                "", ""
                );
            Logs.RecordLogs("Добавление/Редактирование организации", ID);
        }


        //\\\\----Добавление пользователя в службу
        public void AddEmployeeForSPR(string ID,string SlujbaID, string Bind)
        {
            SP = "SP_AddEmployeeForSPR";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                SlujbaID, "SlujbaID",
                Bind, "Bind",
                "", "",
                 "", ""
                );
            Logs.RecordLogs("Привязка персонала к службе", ID);
        }

        //\\\\----Удаление пользователя  служб
        public void DeleteEmployeeSPR(string ID)
        {
            SP = "SP_DeleteSprEmployeeOfSlujba";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", "",
                "", "",
                 "", ""
                );
            Logs.RecordLogs("Удаление персонала из общего списка", ID);
        }


        //\\\\----Удаление пользователя  служб
        public void DeleteEmployeeForSPR(string ID)
        {
            SP = "SP_DeleteEmployeeForSPR";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", "",
                "", "",
                 "", ""
                );
            Logs.RecordLogs("Отвязка персонала от службы", ID);
        }

        //\\\\----Добавление, редактирование фраз
        public void AddEditPhrases(string ID, string Name, string IDEventsCategory)
        {
            SP = "SP_AddEditPhrases";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                Name.Trim(), "Name",
                IDEventsCategory, "IDEventsCategory",
                 "", "",
                 "",""
                );
        }


        //\\\\----Добавление, редактирование шаблонов записей
        public void AddEditSamples(string ID, string Name)
        {
            SP = "SP_AddEditSamples";
            SL.AddEditBlankParam(SP,
                new Employee().SelectIDSlujba, "SlujbaID",
                ID, "ID",
                Name.Trim(), "Name",
                "","",
                 "", ""
                );
        }


        //\\\\----Добавление, редактирование сообщения входящих в службу
        public void AddEditReportedInfo(string SlujbaID, string IDReported)
        {
            SP = "SP_AddEditReportedInfo";
            SL.AddEditBlankParam(SP,
                SlujbaID, "SlujbaID",
                IDReported, "IDReported",
                "","",
                "","",
                 "", ""
                );
        }


        //\\\\----Добавление, редактирование сообщения входящих в службу
        public void AddShiftShedule(string ShiftSheduleID, Int16 ShiftID , DateTime ShiftSheduleDate, string SlujbaID)
        {
            SP = "SP_AddShiftShedule";
            SL.AddEditBlankParam(SP,
                ShiftSheduleID, "ShiftSheduleID",
                ShiftID.ToString(), "ShiftID",
                ShiftSheduleDate.ToString(), "ShiftSheduleDate", 
                SlujbaID, "SlujbaID",
                 "", ""
                );
        }


        //\\\\----Удаление линий
        public void DeleteLineOfSlujb(string ID)
        {
            SP = "SP_DeleteLineOfSlujb";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", ""
                , "", "",
                 "", ""
                );
            Logs.RecordLogs("Удаление оборудования", ID);
        }


     


        //\\\\----Удаление Объекторв
        public void DeleteObject(string ID)
        {
            SP = "SP_DeleteObject";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", ""
                , "", "",
                 "", ""
                );
            Logs.RecordLogs("Удаление организации/объекта", ID);
        }


        //\\\\----Удаление Объекторв
        public void DeleteEventsCategory(string ID)
        {
            SP = "SP_DeleteEventsCategory";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", ""
                , "", "",
                 "", ""
                );
            Logs.RecordLogs("Удаление категорий фраз", ID);
        }


        //\\\\----Удаление фразы
        public void DeletePhrases(string ID)
        {
            SP = "SP_DeletePhrases";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", ""
                , "", "",
                 "", ""
                );
        }


        //\\\\----Удаление шаблона записи
        public void DeleteSamples(string ID)
        {
            SP = "SP_DeleteSamples";
            SL.AddEditBlankParam(SP,
                ID, "ID",
                "", "",
                "", "", 
                "", "",
                "", ""
                );
        }

        //\\\\----Получение смен
        public List<ShiftShedule> GetShiftShedule(List<ShiftShedule> ListShiftShedule)
        {
            ListShiftShedule.Clear();
            SP = "SP_GetShiftShedule";
            SL.GetShiftShedule(ListShiftShedule, new Employee().SelectIDSlujba, SP);
            return ListShiftShedule;
        }


        //\\\\----Получение активной смены
        public void GetShiftSheduleAction(string ShiftSheduleID)
        {
            //new ShiftShedule().ListShiftShedule.Clear();
            SP = "SP_GetShiftSheduleAction";
            SL.GetShiftSheduleAction(new ShiftShedule().ListShiftShedule, ShiftSheduleID, SP);
        }


        //\\\\----Получение активной смены
        public void GetShiftSheduleSelectHistory(string ShiftSheduleID)
        {
           //new ShiftShedule().ListShiftSheduleHistory.Clear();
            SP = "SP_GetShiftSheduleAction";
            SL.GetShiftSheduleAction(new ShiftShedule().ListShiftSheduleHistory, ShiftSheduleID, SP);
        }


        //\\\\----Получение смен
        public List<ShiftShedule> GetPrintShiftShedule(List<ShiftShedule> ListShiftShedule, string ShiftSheduleID)
        {
            ListShiftShedule.Clear();
            SP = "SP_GetPrintShiftShedule";
            SL.GetPrintShiftShedule(ListShiftShedule, ShiftSheduleID, SP);
            return ListShiftShedule;
        }


        //\\\\----Получение историй смен
        public List<ShiftShedule> GetShiftSheduleHistory(List<ShiftShedule> ListShiftShedule, DateTime DateStart, DateTime DateEnd)
        {
            ListShiftShedule.Clear();
            SP = "SP_GetShiftSheduleHistory";
            SL.GetShiftSheduleHistory(ListShiftShedule, new Employee().SelectIDSlujba, SP, DateStart, DateEnd);
            return ListShiftShedule;
        }


        //\\\\----Получение Сообщений
        public List<Blank> GetReporteds(List<Blank> ListReporteds)
        {
           
            ListReporteds.Clear();
            SP = "SP_GetReporteds";
            SL.GetBlankOfOneParam(ListReporteds, SP, new Employee().SelectIDSlujba, "SlujbaID");

            ListBOfReporteds.Clear();
            for (int i=0;i< ListReporteds.Count;i++)
            {
                ListBOfReporteds.Add(new Blank
                {
                    ID = ListReporteds[i].ID,
                    Name = ListReporteds[i].Name,
                });
            }
            ListOfReporteds = ListReporteds;
            return ListReporteds;
        }


        //\\\\----Получение Организации
        public List<Blank> GetOrganizations(List<Blank> ListOrganizations)
        {
            ListBOfOrganizations.Clear();
            SP = "SP_GetOrganizations";
            ListOrganizations.Add(new Blank {
                ID = "00000000-0000-0000-0000-000000000000",
                IDOrganizations = "00000000-0000-0000-0000-000000000000" });
            SL.GetBlankOfOneParam(ListOrganizations, SP, new Employee().SelectIDSlujba, "IDSlujba");
            return ListOrganizations;
        }


        //\\\\----Получение Сообщений
        public List<Blank> GetSprReporteds(List<Blank> ListReporteds)
        {
            ListReporteds.Clear();
            SP = "SP_GetSprReporteds";
            SL.GetBlank(ListReporteds, SP);
            return ListReporteds;
        }


        //\\\\----Получение всех служб
        public List<Blank> GetAllSlujb(List<Blank> ListAllSlujb)
        {
            ListAllSlujb.Clear();
            SP = "SP_GetAllSlujb";
            SL.GetBlank(ListAllSlujb, SP);
            return ListAllSlujb;
        }


        //\\\\----Получение дежурных смены
        public List<Employee> GetEmployeeShiftShedule(List<Employee> ListEmployeeShiftShedule, string ShiftSheduleID)
        {
            SP = "SP_GetEmployeeShiftShedule";
            SL.GetEmployeesOfShiftShedule(ListEmployeeShiftShedule, ShiftSheduleID, SP);
            return ListEmployeeShiftShedule;
        }

        //\\\\----Получение сотрудников службы
        public List<Employee> GetEmployee (List<Employee> ListEmployee, string SlujbaID)
        {
            SP = "SP_GetEmployee";
            SL.GetEmployee(ListEmployee, SlujbaID, SP);
            return ListEmployee;
        }


        //\\\\----Получение персонала смены
        public List<Employee> GetShiftSheduleEmployee(List<Employee> ListEmployee, string SlujbaID)
        {
            SP = "SP_GetShiftSheduleEmployeeToday";
            SL.GetEmployeeShiftShedule(ListEmployee, SlujbaID, SP);
            return ListEmployee;
        }


        //\\\\----Проверка авторизаций
        public string Author(string MacAdress, string EmployeeID, string Pass)
        {
            string Rezult="0";
            SP = "SP_Author";
            Rezult = SL.Author(SP, MacAdress, EmployeeID, Pass, Rezult);
            return Rezult;
        }


        //\\\\----Получение списка событий
        public List<Blank> GetEventsCategory(List<Blank> ListEventsCategory)
        {
            ListEventsCategory.Clear();
            ListEventsCategory.Add(new Blank { ID = "00000000-0000-0000-0000-000000000000" });
            SP = "SP_GetEventsCategory";
            SL.GetBlankOfOneParam(ListEventsCategory, SP,new Employee().SelectIDSlujba, "SlujbaID");
            return ListEventsCategory;
        }


        //\\\\----Получение сотрудников привязанных к службе.
        public List<Employee> GetEmployeesOfSlujba(List<Employee> ListEmployeesOfSlujba)
        {
            ListEmployeesOfSlujba.Clear();
            ListEmployeesOfSlujba.Add(new Employee { EmployeeID= "00000000-0000-0000-0000-000000000000" });

            SP = "SP_GetEmployeeOfSlujba";
            SL.GetEmployeesOfSlujba(ListEmployeesOfSlujba, new Employee().SelectIDSlujba, SP);
            return ListEmployeesOfSlujba;
        }


        //\\\\----Получение сотрудников всех служб
        public List<Employee> GetSprEmployeesOfSlujba(List<Employee> ListEmployeesOfSlujba,string SlujbaID)
        {
            ListEmployeesOfSlujba.Clear();
            SP = "SP_GetSprEmployeeOfSlujba";
            SL.GetEmployeesOfSlujba(ListEmployeesOfSlujba, SlujbaID, SP);
            return ListEmployeesOfSlujba;
        }


        //\\----Добавление
        public void AddOperationalRecord(List<OperationalRecord> ListOperationalRecord)
        {
            SP = "SP_AddOperationalRecord";
            SL.AddOperationalRecord(ListOperationalRecord, SP);
        }

        //\\----Редактирование
        public void EditOperationalRecord(List<OperationalRecord> ListOperationalRecord)
        {
                    SP = "SP_EditOperationalRecord";
                    SL.EditOperationalRecord(ListOperationalRecord, SP);
                    Logs.RecordLogs("Редактирование записи", ListOperationalRecord[0].ID);
        }


        public void GetOperationalRecordReserv()
        {
            new OperationalRecord().ListOfOperationalRecord.Clear();
            SP = "SP_GetOperationalRecord";
            SL.GetOperationalRecord(new OperationalRecord().ListOfOperationalRecord, SP);
        }


        public void GetOperationalRecord()
        {

            try
            {
                ListSynksOperationalRecord.Clear();
                SP = "SP_GetOperationalRecord";
                SL.GetOperationalRecord(ListSynksOperationalRecord, SP);

                //new OperationalRecord().ListOfOperationalRecord.Clear();
                //SL.GetOperationalRecord(new OperationalRecord().ListOfOperationalRecord, SP);

                for (int i = 0; i < ListSynksOperationalRecord.Count; i++)
                {
                    if (new OperationalRecord().ListOfOperationalRecord.Any(c => c.ID.ToLower() == ListSynksOperationalRecord[i].ID.ToLower()))
                    {
                        int index = new OperationalRecord().ListOfOperationalRecord.IndexOf(new OperationalRecord().ListOfOperationalRecord.First(x => x.ID.ToLower() == ListSynksOperationalRecord[i].ID.ToLower()));
                        new OperationalRecord().ListOfOperationalRecord[index] = ListSynksOperationalRecord[i];
                    }
                    else
                    {
                        new OperationalRecord().ListOfOperationalRecord.Add(ListSynksOperationalRecord[i]);
                    }
                }

                var result = new OperationalRecord().ListOfOperationalRecord.Except(ListSynksOperationalRecord);

                foreach (var item in result)
                {
                    int index = new OperationalRecord().ListOfOperationalRecord.IndexOf(new OperationalRecord().ListOfOperationalRecord.First(x => x.ID.ToLower() == item.ID.ToLower()));
                    new OperationalRecord().ListOfOperationalRecord.Remove(item);
                }

                for (int i = 0; i < ListSynksOperationalRecord.Count; i++)
                {
                    new OperationalRecord().ListOfOperationalRecord[i] = ListSynksOperationalRecord[i];
                }

            }
            catch
            {
                GetOperationalRecordReserv();
            }


        }





        public BindingList<OperationalRecord> GetOperationalRecordHistory(string SlujbaID, string ShiftSheduleID)
        {
            new OperationalRecord().ListOfOperationalRecordHistory.Clear();
            SP = "SP_GetOperationalRecordHistory";
            SL.GetOperationalRecordHistory(new OperationalRecord().ListOfOperationalRecordHistory, SP, SlujbaID, ShiftSheduleID);
            return new OperationalRecord().ListOfOperationalRecordHistory;
        }


        public List<AllChanges> GetChanges (List<AllChanges> ListChanges)
        {
            ListChanges.Clear();
            SP = "SP_GetChanges";
            SL.GetChanges(ListChanges, SP);
            return ListChanges;
        }

        public List<AllChanges> ValidMail(List<AllChanges> ListChanges)
        {
            ListChanges.Clear();
            SP = "SP_ValidAppeals";
            SL.GetChangesAppeals(ListChanges, SP);
            return ListChanges;
        }

        public void AddLocked(string ID)
        {
            SP = "SP_AddLocked";
            SL.AddDeleteLocked(ID, SP);
        }

        public void DeleteLocked(string ID)
        {
            SP = "SP_DeleteLocked";
            SL.AddDeleteLocked(ID, SP);
        }

    }
}
