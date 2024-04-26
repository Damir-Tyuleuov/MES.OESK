using System;
using System.Collections.Generic;

using System.Data.SqlClient;

using DispetcherRes.Class;
using MES.OESK.Class;
using System.ComponentModel;
using System.Configuration;


namespace DispetcherRes.SqlLayer
{
    public class SqlLayer
    {
        public SqlLayer()
        {
            Sqlconnection= "Data Source = " + config.AppSettings.Settings["ConnectionServer"].Value + "; Initial Catalog = MES.OESK.DISP; User ID = disp; Password=disp2023";
        }
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
        //\\\\----Строки подключения к базе
        private string Sqlconnection;
        SqlConnection Sql = new SqlConnection();
        SqlCommand Cmd = new SqlCommand();
        SqlDataReader DR;

        public void OpenConnection(SqlConnection Sql)
        {
            try
            {
                if (Sql.ConnectionString=="")
                {
                    Sql.ConnectionString = Sqlconnection;
                }
                if (Cmd.Connection == null)
                {
                    Cmd.Connection = Sql;
                }
                Sql.Open();
            }
            catch
            {
                new Employee().SqlError = 1;
                Sql.Close();
            }
        }

        private SqlDataReader ExecuteReader()
        {
            try
            {
                DR = Cmd.ExecuteReader();
                if (DR ==null)
                {
                    new Employee().SqlError = 1;
                }
                return DR;
            }
            catch
            {
                new Employee().SqlError = 1;
                DR = null;
                return DR;
            }
        }

        public void CloseConnection(SqlConnection Sql)
        {
            try
            {
                Sql.Close();
            }
            catch
            {

            }
        }

        public void GetServerTime (string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            DR = ExecuteReader();
            while (DR.Read())
            {
               new ShiftShedule().ServerTime= DR.GetDateTime(0);
            }
            CloseConnection(Sql);

        }

        public void AddSamplesForOR(string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", new Employee().SelectIDSlujba);
            Cmd.Parameters.AddWithValue("@Employee", new Employee().SelectIDEmployee);
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", new Employee().SelectShiftShedule);
            Cmd.ExecuteNonQuery();
            CloseConnection(Sql);
        }


        public List<Blank> GetBlank(List<Blank> ListBlank, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new Blank
                {
                    ID = DR.GetValue(0).ToString(),
                    Name = DR.GetValue(1).ToString(),
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<AllChanges> GetChanges(List<AllChanges> ListBlank, string SP)
        {
            try
            {
                OpenConnection(Sql);
                Cmd = new SqlCommand(SP, Sql);
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@SlujbaID", new Employee().SelectIDSlujba);
                DR = ExecuteReader();
                while (DR.Read())
                {
                    ListBlank.Add(new AllChanges
                    {
                        OperationalRecordNum = DR.GetInt32(0),
                        LockedNum = DR.GetInt32(1),
                        Mail = DR.GetInt32(2),
                        ShiftSheduleID = DR.GetValue(3).ToString()
                    });
                }

                CloseConnection(Sql);
                return ListBlank;
            }
            catch
            {
                ListBlank.Clear();
                new Employee().SqlError = 1;
                Sql.Close();
                return ListBlank;
            }
        }


        public List<AllChanges> GetChangesAppeals(List<AllChanges> ListBlank, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", new Employee().SelectIDSlujba);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new AllChanges
                {
                    OperationalRecordNum = DR.GetInt32(0),
                    LockedNum = DR.GetInt32(1),
                    Mail = DR.GetInt32(2),
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public void AddDeleteLocked(string ID, string SP)
        {
                OpenConnection(Sql);
                Cmd = new SqlCommand(SP, Sql);
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@ID", ID);
                Cmd.ExecuteNonQuery();
                CloseConnection(Sql);
        }


        public void DeleteOperationRecordOfID(string SP, string ID, string SlujbaID)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@ID", ID);
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            Cmd.ExecuteNonQuery();
            CloseConnection(Sql);

        }


        public List<Appeals> GetBlankOfOneParam(List<Appeals> AppealsList, string SP, string IDParam, string NameParam)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@" + NameParam, IDParam);
            DR = ExecuteReader();
            while (DR.Read())
            {
                AppealsList.Add(new Appeals
                {
                    ID = DR.GetValue(0).ToString(),
                    Datetime = DR.GetDateTime(1),
                    SlujbaID = DR.GetValue(2).ToString(),
                    SlujbaName = DR.GetValue(3).ToString(),
                    AuthorID = DR.GetValue(4).ToString(),
                    AuthorName = DR.GetValue(5).ToString(),
                    Text = DR.GetValue(6).ToString(),
                    WhomsSlujbaID = DR.GetValue(7).ToString(),
                    WhomsSlujbaName = DR.GetValue(8).ToString(),
                    WhomsEmployeeID = DR.GetValue(9).ToString(),
                    WhomsEmployeeName = DR.GetValue(10).ToString(),
                    StateRead = DR.GetByte(11)
                });
            }

            CloseConnection(Sql);
            return AppealsList;
        }


        public List<Blank> GetBlankOfOneParam(List<Blank> ListBlank, string SP, string IDParam, string NameParam)
        {
            try
            {
                OpenConnection(Sql);
                Cmd = new SqlCommand(SP, Sql);
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.Parameters.AddWithValue("@" + NameParam, IDParam);
                DR = ExecuteReader();
                while (DR.Read())
                {
                    ListBlank.Add(new Blank
                    {
                        ID = DR.GetValue(0).ToString(),
                        Name = DR.GetValue(1).ToString(),
                    });
                }

                CloseConnection(Sql);
                return ListBlank;
            }
            catch {
                ListBlank.Clear();
                return ListBlank;
            }
        }


        public List<Blank> GetBlankOfTwoParam(List<Blank> ListBlank, string SP,
            string IDParam, string NameParam, string IDParam2, string NameParam2)
        {

            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@" + NameParam, IDParam);
            Cmd.Parameters.AddWithValue("@" + NameParam2, IDParam2);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new Blank
                {
                    ID = DR.GetValue(0).ToString(),
                    Name = DR.GetValue(1).ToString(),
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<Blank> GetOrganizations(List<Blank> ListBlank, string SP,
           string IDParam, string NameParam, string IDParam2, string NameParam2)
        {

            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@" + NameParam, IDParam);
            Cmd.Parameters.AddWithValue("@" + NameParam2, IDParam2);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new Blank
                {
                    ID = DR.GetValue(0).ToString(),
                    Name = DR.GetValue(1).ToString(),
                    IDOrganizations = DR.GetValue(2).ToString(),
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<Blank> GetBlankOfTreeParam(List<Blank> ListBlank, string SP,
        string IDParam, string NameParam, string IDParam2, 
        string NameParam2, string IDParam3, string NameParam3)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@" + NameParam, IDParam);
            Cmd.Parameters.AddWithValue("@" + NameParam2, IDParam2);
            Cmd.Parameters.AddWithValue("@" + NameParam3, IDParam3);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new Blank
                {
                    ID = DR.GetValue(0).ToString(),
                    Name = DR.GetValue(1).ToString(),
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public void AddEditBlankParam(string SP,
        string IDParam, string NameParam,
        string IDParam2, string NameParam2,
        string IDParam3, string NameParam3,
        string IDParam4, string NameParam4,
        string IDParam5, string NameParam5)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;

            Cmd.Parameters.AddWithValue("@" + NameParam, IDParam);

            if (NameParam2 != "")
            {
                Cmd.Parameters.AddWithValue("@" + NameParam2, IDParam2);
            }

            if (NameParam3 != "")
            {
                Cmd.Parameters.AddWithValue("@" + NameParam3, IDParam3);
            }

            if (NameParam4 != "")
            {
                Cmd.Parameters.AddWithValue("@" + NameParam4, IDParam4);
            }

            if (NameParam5 != "")
            {
                Cmd.Parameters.AddWithValue("@" + NameParam5, IDParam5);
            }

            Cmd.ExecuteNonQuery();
            CloseConnection(Sql);
        }


        public List<Employee> GetEmployee(List<Employee> ListBlank, string SlujbaID, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new Employee
                {
                    EmployeeID = DR.GetValue(0).ToString(),
                    EmployeeName = DR.GetValue(1).ToString(),
                    Pass = DR.GetValue(2).ToString(),
                    SlujbaID = DR.GetValue(3).ToString(),
                    SlujbaName = DR.GetValue(4).ToString(),
                    Lvl = DR.GetByte(5),
                    //ShiftSheduleID= DR.GetValue(6).ToString()
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<Employee> GetEmployeeShiftShedule(List<Employee> ListBlank, string SlujbaID, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new Employee
                {
                    EmployeeID = DR.GetValue(0).ToString(),
                    EmployeeName = DR.GetValue(1).ToString(),
                    Pass = DR.GetValue(2).ToString(),
                    SlujbaID = DR.GetValue(3).ToString(),
                    SlujbaName = DR.GetValue(4).ToString(),
                    Lvl = DR.GetByte(5),
                    ShiftSheduleID= DR.GetValue(6).ToString(),
                    SelectShiftID = DR.GetInt32(7),
                    SelectShiftState = DR.GetInt32(8),
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<ShiftShedule> GetPrintShiftShedule(List<ShiftShedule> ListBlank, string ShiftSheduleID, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", ShiftSheduleID);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new ShiftShedule
                {
                    ShiftSheduleID = DR.GetValue(0).ToString(),
                    ShiftID = DR.GetValue(1).ToString(),
                    ShiftSheduleDate = DR.GetDateTime(2),
                    IsClosed = DR.GetInt32(3),
                    ShiftSheduleStartDate = DR.GetValue(4).ToString(),
                    ShiftSheduleEndDate = DR.GetValue(5).ToString(),
                    SlujbaID = DR.GetValue(6).ToString(),
                    ShiftSheduleEmployee = DR.GetValue(7).ToString(),
                    ShiftSheduleEmployeeName = DR.GetValue(8).ToString()
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<ShiftShedule> GetShiftShedule(List<ShiftShedule> ListBlank, string SlujbaID, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new ShiftShedule
                {
                    ShiftSheduleID = DR.GetValue(0).ToString(),
                    ShiftID = DR.GetValue(1).ToString(),
                    ShiftSheduleDate = DR.GetDateTime(2),
                    IsClosed = DR.GetInt32(3)
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<ShiftShedule> GetShiftSheduleAction(List<ShiftShedule> ListBlank, string ShiftSheduleID, string SP)
        {
            ListBlank.Clear();
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", ShiftSheduleID);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new ShiftShedule
                {
                    ShiftSheduleID = DR.GetValue(0).ToString(),
                    ShiftID = DR.GetValue(1).ToString(),
                    ShiftSheduleDate = DR.GetDateTime(2),
                    IsClosed = DR.GetInt32(3)
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<ShiftShedule> GetShiftSheduleHistory(List<ShiftShedule> ListBlank, string SlujbaID, string SP, DateTime DateBegin, DateTime DateEnd)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            Cmd.Parameters.AddWithValue("@DateBegin", DateBegin);
            Cmd.Parameters.AddWithValue("@DateEnd", DateEnd);
            DR = ExecuteReader();
            while (DR.Read())
            {
                ListBlank.Add(new ShiftShedule
                {
                    ShiftSheduleID = DR.GetValue(0).ToString(),
                    ShiftID = DR.GetValue(1).ToString(),
                    ShiftSheduleDate = DR.GetDateTime(2),
                    IsClosed = DR.GetInt32(3),
                    ShiftSheduleStartDate = DR.GetValue(4).ToString(),
                    ShiftSheduleEndDate = DR.GetValue(5).ToString(),
                    ShiftSheduleEmployee = DR.GetValue(6).ToString()

                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<Employee> GetEmployeesOfShiftShedule(List<Employee> ListBlank, string ShiftSheduleID, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", ShiftSheduleID);
            DR = ExecuteReader();

            while (DR.Read())
            {
                ListBlank.Add(new Employee
                {
                    ShiftSheduleID = DR.GetValue(0).ToString(),
                    EmployeeID = DR.GetValue(1).ToString(),
                    EmployeeName= DR.GetValue(2).ToString()
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public List<Employee> GetEmployeesOfSlujba(List<Employee> ListBlank, string SlujbaID, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            DR = ExecuteReader();

            while (DR.Read())
            {
                ListBlank.Add(new Employee
                {
                    EmployeeID = DR.GetValue(0).ToString(),
                    EmployeeName = DR.GetValue(1).ToString(),
                    SlujbaName = DR.GetValue(2).ToString()
                });
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public string Author(string SP, string MacAdress, string EmployeeID, string Pass, string Rezult)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand("SP_Author", Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@MacAdress", MacAdress);
            Cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            Cmd.Parameters.AddWithValue("@Pass", Pass);
            DR = ExecuteReader();
            while (DR.Read())
            {
                Rezult = DR.GetValue(0).ToString();
            }

            CloseConnection(Sql);
            return Rezult;
        }


        public void RecordLogs(List<Logs> ListLogs)
        {
            try
            {
                OpenConnection(Sql);
                Cmd = new SqlCommand("SP_RecordLogs", Sql);
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;

                Cmd.Parameters.AddWithValue("@Actions", ListLogs[0].Actions);
                Cmd.Parameters.AddWithValue("@InfoID", ListLogs[0].InfoID);
                Cmd.Parameters.AddWithValue("@EmployeeID", ListLogs[0].EmployeeID);

                Cmd.Parameters.AddWithValue("@ShiftShedule", ListLogs[0].ShiftSheduleID);
                Cmd.Parameters.AddWithValue("@InfoPC", ListLogs[0].InfoPC);
                Cmd.Parameters.AddWithValue("@InfoIP", ListLogs[0].InfoIP);


                Cmd.ExecuteNonQuery();
                CloseConnection(Sql);
            }
            catch { }

        }


        public void AddOperationalRecord(List<OperationalRecord> ListOperationalRecord, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;

            Cmd.Parameters.AddWithValue("@ObjectID", ListOperationalRecord[0].ObjectID);
            Cmd.Parameters.AddWithValue("@EventID", ListOperationalRecord[0].EventID);
            Cmd.Parameters.AddWithValue("@LineID", ListOperationalRecord[0].LineID);

            Cmd.Parameters.AddWithValue("@WhoID", ListOperationalRecord[0].WhoID);
            Cmd.Parameters.AddWithValue("@WhoText", ListOperationalRecord[0].WhoText.Trim());
            Cmd.Parameters.AddWithValue("@WhomID", ListOperationalRecord[0].WhomID);
            Cmd.Parameters.AddWithValue("@WhomText", ListOperationalRecord[0].WhomText.Trim());

            Cmd.Parameters.AddWithValue("@Message", ListOperationalRecord[0].Message.Trim());
            Cmd.Parameters.AddWithValue("@Note", ListOperationalRecord[0].Note.Trim());

            Cmd.Parameters.AddWithValue("@DateBegin", ListOperationalRecord[0].DateBegin);
            Cmd.Parameters.AddWithValue("@DateEnd", ListOperationalRecord[0].DateEnd);

            Cmd.Parameters.AddWithValue("@Employee", ListOperationalRecord[0].EmployeeID);
            Cmd.Parameters.AddWithValue("@Protects", ListOperationalRecord[0].ProtectsID);

            Cmd.Parameters.AddWithValue("@StImportant", ListOperationalRecord[0].StImportant);
            Cmd.Parameters.AddWithValue("@StNewEquipment", ListOperationalRecord[0].StNewEquipment);
            Cmd.Parameters.AddWithValue("@StDefend", ListOperationalRecord[0].StDefend);

            Cmd.Parameters.AddWithValue("@ReportedID", ListOperationalRecord[0].ReportedID);

            Cmd.Parameters.AddWithValue("@Zazem1", ListOperationalRecord[0].Zazem1);
            Cmd.Parameters.AddWithValue("@Zazem2", ListOperationalRecord[0].Zazem2);
            Cmd.Parameters.AddWithValue("@Zazem3", ListOperationalRecord[0].Zazem3);
            Cmd.Parameters.AddWithValue("@SlujbaID", ListOperationalRecord[0].SlujbaID);

            Cmd.Parameters.AddWithValue("@NarObject ", ListOperationalRecord[0].NarObject.Trim());
            Cmd.Parameters.AddWithValue("@NarUser ", ListOperationalRecord[0].NarUser.Trim());
            Cmd.Parameters.AddWithValue("@NarNum", ListOperationalRecord[0].NarNum.Trim());

            Cmd.Parameters.AddWithValue("@StNar", ListOperationalRecord[0].StNar);
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", ListOperationalRecord[0].ShiftSheduleID);
            Cmd.Parameters.AddWithValue("@OrganizationID", ListOperationalRecord[0].OrganizationID);
            Cmd.ExecuteNonQuery();
            CloseConnection(Sql);

        }


        public void EditOperationalRecord(List<OperationalRecord> ListOperationalRecord, string SP)
        {
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@ID", new OperationalRecord().SelectedID);
            Cmd.Parameters.AddWithValue("@ObjectID", ListOperationalRecord[0].ObjectID);
            Cmd.Parameters.AddWithValue("@EventID", ListOperationalRecord[0].EventID);
            Cmd.Parameters.AddWithValue("@LineID", ListOperationalRecord[0].LineID);
            Cmd.Parameters.AddWithValue("@WhoID", ListOperationalRecord[0].WhoID);
            Cmd.Parameters.AddWithValue("@WhoText", ListOperationalRecord[0].WhoText.Trim());
            Cmd.Parameters.AddWithValue("@WhomID", ListOperationalRecord[0].WhomID);
            Cmd.Parameters.AddWithValue("@WhomText", ListOperationalRecord[0].WhomText.Trim());
            Cmd.Parameters.AddWithValue("@Message", ListOperationalRecord[0].Message.Trim());
            Cmd.Parameters.AddWithValue("@Note", ListOperationalRecord[0].Note.Trim());
            Cmd.Parameters.AddWithValue("@DateBegin", ListOperationalRecord[0].DateBegin);
            Cmd.Parameters.AddWithValue("@DateEnd", ListOperationalRecord[0].DateEnd);
            Cmd.Parameters.AddWithValue("@Employee", ListOperationalRecord[0].EmployeeID);

            Cmd.Parameters.AddWithValue("@Protects", ListOperationalRecord[0].ProtectsID);
            Cmd.Parameters.AddWithValue("@StImportant", ListOperationalRecord[0].StImportant);
            Cmd.Parameters.AddWithValue("@StNewEquipment", ListOperationalRecord[0].StNewEquipment);
            Cmd.Parameters.AddWithValue("@StDefend", ListOperationalRecord[0].StDefend);

            Cmd.Parameters.AddWithValue("@ReportedID", ListOperationalRecord[0].ReportedID);

            Cmd.Parameters.AddWithValue("@Zazem1", ListOperationalRecord[0].Zazem1);
            Cmd.Parameters.AddWithValue("@Zazem2", ListOperationalRecord[0].Zazem2);
            Cmd.Parameters.AddWithValue("@Zazem3", ListOperationalRecord[0].Zazem3);

            Cmd.Parameters.AddWithValue("@SlujbaID", ListOperationalRecord[0].SlujbaID);

            Cmd.Parameters.AddWithValue("@NarObject ", ListOperationalRecord[0].NarObject.Trim());
            Cmd.Parameters.AddWithValue("@NarUser ", ListOperationalRecord[0].NarUser.Trim());
            Cmd.Parameters.AddWithValue("@NarNum", ListOperationalRecord[0].NarNum.Trim());

            Cmd.Parameters.AddWithValue("@StNar", ListOperationalRecord[0].StNar);

            Cmd.Parameters.AddWithValue("@ShiftSheduleID", ListOperationalRecord[0].ShiftSheduleID);
            Cmd.Parameters.AddWithValue("@OrganizationID", ListOperationalRecord[0].OrganizationID);
            Cmd.ExecuteNonQuery();
            CloseConnection(Sql);

        }


        public BindingList<OperationalRecord> GetOperationalRecord(BindingList<OperationalRecord> ListBlank, string SP)
        {

            string LineName = "";
            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", new Employee().SelectIDSlujba);
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", new Employee().SelectShiftShedule);
            DR = ExecuteReader();
            int i = 0;
            while (DR.Read())
            {
                LineName = "";
                if (DR.GetValue(25).ToString()!= "00000000-0000-0000-0000-000000000000")
                {
                    LineName = "\n (" +DR.GetValue(32).ToString()+")";
                }

                ListBlank.Add(new OperationalRecord
                {
                    ID = DR.GetValue(0).ToString(),
                    DateBegin = DR.GetDateTime(1),
                    WhoID = DR.GetValue(2).ToString(),
                    WhoName = DR.GetValue(3).ToString(),
                    ObjectID = DR.GetValue(4).ToString(),
                    ObjectName = DR.GetValue(5).ToString() + LineName,
                    Message = DR.GetValue(6).ToString(),
                    WhomID = DR.GetValue(7).ToString(), 
                    WhomName = DR.GetValue(8).ToString(),
                    DateEnd = DR.GetDateTime(9),
                    ReportedName = DR.GetValue(10).ToString(),
                    Zazem1 = DR.GetByte(11),
                    Zazem2 = DR.GetByte(12),
                    Zazem3 = DR.GetByte(13),
                    StImportant = DR.GetByte(14),
                    StDefend = DR.GetByte(15),
                    StNewEquipment = DR.GetByte(16),
                    Note = DR.GetValue(17).ToString(),
                    EmployeeID = DR.GetValue(18).ToString(),
                    EmployeeName = DR.GetValue(19).ToString(),
                    EventID = DR.GetValue(20).ToString(),
                    WhoText = DR.GetValue(21).ToString(), 
                    WhomText = DR.GetValue(22).ToString(),
                    ClassVLID = DR.GetValue(23).ToString(),
                    ProtectsID = DR.GetValue(24).ToString(),
                    LineID = DR.GetValue(25).ToString(),
                    ReportedID = DR.GetValue(26).ToString(),
                    NarObject = DR.GetValue(27).ToString(),
                    NarUser = DR.GetValue(28).ToString(),
                    NarNum = DR.GetValue(29).ToString(),
                    StNar = DR.GetByte(30),
                    OrganizationID = DR.GetValue(31).ToString(),
                    AllZazem = Convert.ToByte(DR.GetByte(11) + DR.GetByte(12) + DR.GetByte(13)),
                    Nar = DR.GetValue(29).ToString() + "\n" + DR.GetValue(27).ToString() + "\n" + DR.GetValue(28).ToString(),
                    DateEnd2 = DR.GetDateTime(9).ToString(),
                    LineName = DR.GetValue(32).ToString()
                });

                if (ListBlank[i].DateEnd.Year == Convert.ToDateTime("01.01.1991 00:00").Year)
                {
                    ListBlank[i].DateEnd2 = "";
                }
                else
                {
                    ListBlank[i].DateEnd2 = Convert.ToDateTime(ListBlank[i].DateEnd2).ToShortDateString()+
                    " "+Convert.ToDateTime(ListBlank[i].DateEnd2).ToShortTimeString();
                }


                i++;
            }

            CloseConnection(Sql);
            return ListBlank;
        }


        public BindingList<OperationalRecord> GetOperationalRecordHistory(BindingList<OperationalRecord> ListBlank, string SP, string SlujbaID, string ShiftSheduleID)
        {
            string LineName = "";

            OpenConnection(Sql);
            Cmd = new SqlCommand(SP, Sql);
            Cmd.CommandType = System.Data.CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@SlujbaID", SlujbaID);
            Cmd.Parameters.AddWithValue("@ShiftSheduleID", ShiftSheduleID);
            DR = ExecuteReader();
            int i = 0;
            while (DR.Read())
            {

                LineName = "";
                if (DR.GetValue(25).ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    LineName = "\n (" + DR.GetValue(32).ToString() + ")";
                }
                ListBlank.Add(new OperationalRecord
                {
                    ID = DR.GetValue(0).ToString(),
                    DateBegin = DR.GetDateTime(1),
                    WhoID = DR.GetValue(2).ToString(),
                    WhoName = DR.GetValue(3).ToString(),
                    ObjectID = DR.GetValue(4).ToString(),
                    ObjectName = DR.GetValue(5).ToString()+LineName,
                    Message = DR.GetValue(6).ToString(),
                    WhomID = DR.GetValue(7).ToString(),
                    WhomName = DR.GetValue(8).ToString(),
                    DateEnd = DR.GetDateTime(9),
                    ReportedName = DR.GetValue(10).ToString(),
                    Zazem1 = DR.GetByte(11),
                    Zazem2 = DR.GetByte(12),
                    Zazem3 = DR.GetByte(13),
                    StImportant = DR.GetByte(14),
                    StDefend = DR.GetByte(15),
                    StNewEquipment = DR.GetByte(16),
                    Note = DR.GetValue(17).ToString(),
                    EmployeeID = DR.GetValue(18).ToString(),
                    EmployeeName = DR.GetValue(19).ToString(),
                    EventID = DR.GetValue(20).ToString(),
                    WhoText = DR.GetValue(21).ToString(),
                    WhomText = DR.GetValue(22).ToString(),
                    ClassVLID = DR.GetValue(23).ToString(),
                    ProtectsID = DR.GetValue(24).ToString(),
                    LineID = DR.GetValue(25).ToString(),
                    ReportedID = DR.GetValue(26).ToString(),
                    NarObject = DR.GetValue(27).ToString(),
                    NarUser = DR.GetValue(28).ToString(),
                    NarNum = DR.GetValue(29).ToString(),
                    StNar = DR.GetByte(30),
                    AllZazem = Convert.ToByte(DR.GetByte(11) + DR.GetByte(12) + DR.GetByte(13)),
                    Nar = DR.GetValue(28).ToString() + "\n" + DR.GetValue(27).ToString() + "\n" + DR.GetValue(29).ToString(),
                    DateEnd2 = DR.GetDateTime(9).ToString()
                   ,LineName = DR.GetValue(32).ToString()
                });

                if (ListBlank[i].DateEnd.Year == Convert.ToDateTime("01.01.1991 00:00").Year)
                {
                    ListBlank[i].DateEnd2 = "";
                }


                i++;
            }

            CloseConnection(Sql);
            return ListBlank;
        }


    }
}
