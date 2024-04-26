using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DispetcherRes.Class;
using MES.OESK.Class;

namespace MES.OESK.ViewForm
{

    public partial class AddEvent : Form
    {
        Blank BL = new Blank();
        Employee EM = new Employee();
        Logs Logs = new Logs();
        OperationalRecord OR = new OperationalRecord();

        // OperationalRecord OR = new OperationalRecord();

        List<Blank> ListEventCategory = new List<Blank>();
        List<Blank> ListObjects = new List<Blank>();
        List<Blank> ListClassVL = new List<Blank>();
        List<Blank> ListLineOfVL = new List<Blank>();
        List<Blank> ListProtects = new List<Blank>();
        List<Blank> ListReporteds = new List<Blank>();
        List<Blank> ListOrganizations = new List<Blank>();

        List<Employee> ListEmployeesOfSlujba = new List<Employee>();
        List<Employee> ListEmployeesOfSlujba2 = new List<Employee>();

        List<Blank> ListPharaserOfEventsCategory = new List<Blank>();

        List<OperationalRecord> finded = new List<OperationalRecord>();

        List<OperationalRecord> ListAddOperationalRecord = new List<OperationalRecord>();

        byte stImportant = 0, stNewEquipment = 0, stDefend = 0, stNar = 0;


        int Zam1 = 0;
        int Zam2 = 0;
        int Zam3 = 0;
        //int whoIND = 0;
        public AddEvent()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);

            InitializeComponent();
        }


        //\\\\----Параметр важное
        private byte StImportant
        {
            get { return stImportant; }
            set
            {
                stImportant = value;

                if (0 == value)
                {
                    button8.BackColor = Color.Transparent;
                }

                if (1 == value)
                {
                    button8.BackColor = Color.Yellow;
                }
            }
        }


        //\\\\----Параметр ввод нового оборудрования
        private byte StNewEquipment
        {
            get { return stNewEquipment; }
            set
            {

                stNewEquipment = value;

                if (value == 0)
                {
                    button9.BackColor = Color.Transparent;

                }

                if (value == 1)
                {
                    button9.BackColor = Color.Yellow;
                }
            }
        }


        //\\\\----Параметр защита
        private byte StDefend
        {
            get { return stDefend; }
            set
            {
                stDefend = value;

                if (value == 0)
                {
                    button10.BackColor = Color.Transparent;
                }

                if (value == 1)
                {
                    button10.BackColor = Color.Yellow;
                }
            }
        }


        //\\\\----Параметр защита
        private byte StNar
        {
            get { return stNar; }
            set
            {
                stNar = value;

                if (value == 0)
                {
                    button7.BackColor = Color.Transparent;
                }

                if (value == 1)
                {
                    button7.BackColor = Color.Yellow;
                }
            }
        }


        //\\\\----Переменная линий
        string strLineName;
        string strLineID;
        private string StrLineID
        {
            get
            { return strLineID; }
            set
            { strLineID = value; }
        }


        //\\\\----Переменная событий
        string strEventID;
        private string StrEventID
        {
            get
            { return strEventID; }
            set
            { strEventID = value; }
        }


        //\\\\----Переменная даты отработки
        DateTime strDT2;
        private DateTime StrDT2
        {
            get
            { return strDT2; }
            set
            { strDT2 = value; }
        }


        //\\\\----Переменная объектов
        string strObjectName;
        string strObjectID;
        private string StrObjectID
        {
            get
            { return strObjectID; }
            set
            { strObjectID = value; }
        }


        //\\\\----Переменная номинала напряжения
        string strCVID;
        private string StrCVID
        {
            get
            { return strCVID; }
            set
            { strCVID = value; }
        }


        //\\\\----Переменная Сообщено
        string strReportedName;
        string strReportedID;
        private string StrReportedID
        {
            get
            { return strReportedID; }
            set
            { strReportedID = value; }
        }



        //\\\\----Переменная объектов
        byte stateLoad = 1;
        private byte StateLoad
        {
            get
            { return stateLoad; }
            set
            { stateLoad = value; }
        }


        //\\\\----Переменная Сообщено
        string strWhoName;
        string strWhoID;
        private string StrWhoID
        {
            get
            { return strWhoID; }
            set
            { strWhoID = value; }
        }


        //\\\\----Переменная принятых
        string strWhomName;
        string strWhomID;
        private string StrWhomID
        {
            get
            { return strWhomID; }
            set
            { strWhomID = value; }
        }


        //\\\\----Переменная защита
        string strProtectsName;
        string strProtectsID;
        private string StrProtectsID
        {
            get
            { return strProtectsID; }
            set
            { strProtectsID = value; }
        }


        //\\\\----Организация
        string strOrganizationName;
        string strOrganizationID;
        private string StrOrganizationID
        {
            get
            { return strOrganizationID; }
            set
            { strOrganizationID = value; }
        }


        //\\\\----Выгрузка основных параметров
        private void AddEvent_Load(object sender, EventArgs e)
        {


            if (new OperationalRecord().SelectedFunc == 0)
            {
                label15.Text = "Добавление записи";
            }

            if (new OperationalRecord().SelectedFunc == 1)
            {
                label15.Text = "Редактирование записи";
            }

            if (textBox6.Text == "" || textBox6.Text == "0")
            {
                button7.Enabled = false;
            }
            else
            {
                button7.Enabled = true;
            }

            BL.GetEventsCategory(ListEventCategory);
            comboBox2.DataSource = ListEventCategory;
            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "Name";
            comboBox2.Text = "";


            BL.GetEmployeesOfSlujba(ListEmployeesOfSlujba);
            comboBox3.DataSource = ListEmployeesOfSlujba;
            comboBox3.ValueMember = "EmployeeID";
            comboBox3.DisplayMember = "EmployeeName";
            comboBox3.Text = "";
            richTextBox1.Text = "";

            BL.GetEmployeesOfSlujba(ListEmployeesOfSlujba2);
            comboBox4.DataSource = ListEmployeesOfSlujba2;
            comboBox4.ValueMember = "EmployeeID";
            comboBox4.DisplayMember = "EmployeeName";
            comboBox4.Text = "";
            richTextBox2.Text = "";

            BL.GetClassVL(ListClassVL);
            comboBox5.DataSource = ListClassVL;
            comboBox5.ValueMember = "ID";
            comboBox5.DisplayMember = "Name";
            comboBox5.Text = "";

            BL.GetProtects(ListProtects);
            comboBox8.DataSource = ListProtects;
            comboBox8.ValueMember = "ID";
            comboBox8.DisplayMember = "Name";

            BL.GetReporteds(ListReporteds);
            comboBox9.DataSource = ListReporteds;
            comboBox9.ValueMember = "ID";
            comboBox9.DisplayMember = "Name";
            comboBox9.ForeColor = Color.Blue;

            BL.GetOrganizations(ListOrganizations);
            comboBox10.DataSource = ListOrganizations;
            comboBox10.ValueMember = "ID";
            comboBox10.DisplayMember = "Name";

            textBox1.Text = Zam1.ToString();
            textBox2.Text = Zam2.ToString();
            textBox3.Text = Zam3.ToString();

            BL.GetServerTime();
            dateTimePicker1.Value = new ShiftShedule().ServerTime;

            dateTimePicker2.CustomFormat = "''";
            dateTimePicker2.Checked = false;
            //StateLoad = 1;

            if (new OperationalRecord().SelectedFunc == 1)
            {
                dateTimePicker2.Checked = true;

                finded = new OperationalRecord().ListOfOperationalRecord.Where(item => item.ID == new OperationalRecord().SelectedID).ToList();



                var LC = ListEmployeesOfSlujba.Where(u => u.EmployeeID == finded[0].WhoID.ToString()).ToList();
                if (LC.Count > 0)
                {
                    comboBox3.SelectedValue = finded[0].WhoID;
                }

                LC = ListEmployeesOfSlujba2.Where(u => u.EmployeeID == finded[0].WhomID.ToString()).ToList();
                if (LC.Count > 0)
                {
                    comboBox4.SelectedValue = finded[0].WhomID;
                }


                //MessageBox.Show(finded[0].ClassVLID);
                var LC2 = ListEventCategory.Where(u => u.ID == finded[0].EventID).ToList();
                if (LC2.Count > 0)
                {
                    comboBox2.SelectedValue = finded[0].EventID;
                }

                LC2 = ListProtects.Where(u => u.ID == finded[0].ProtectsID).ToList();
                if (LC2.Count > 0)
                {
                    comboBox8.SelectedValue = finded[0].ProtectsID;
                }


                LC2 = ListReporteds.Where(u => u.ID == finded[0].ReportedID).ToList();
                if (LC2.Count > 0)
                {
                    comboBox9.SelectedValue = finded[0].ReportedID;
                }

                var LC3 = ListOrganizations.Where(u => u.ID == finded[0].OrganizationID).ToList();
                if (LC3.Count > 0)
                {
                    comboBox10.SelectedValue = finded[0].OrganizationID;
                }

                LC3 = ListObjects.Where(u => u.ID == finded[0].ObjectID).ToList();
                if (LC3.Count > 0)
                {
                    comboBox1.SelectedValue = finded[0].ObjectID;
                }

                LC3 = ListClassVL.Where(u => u.ID == finded[0].ClassVLID).ToList();
                if (LC3.Count > 0)
                {
                    comboBox5.SelectedValue = finded[0].ClassVLID;
                }

                LC3 = ListLineOfVL.Where(u => u.ID == finded[0].LineID).ToList();
                if (LC3.Count > 0)
                {
                    comboBox6.SelectedValue = finded[0].LineID;
                }

                if (finded[0].Zazem1 > 0 || finded[0].Zazem2 > 0)
                {
                    button5.Enabled = false;
                }

                richTextBox1.Text = finded[0].WhoText;
                richTextBox2.Text = finded[0].WhomText;
                richTextBox3.Text = finded[0].Message;
                richTextBox4.Text = finded[0].Note;
                textBox1.Text = finded[0].Zazem1.ToString();
                textBox2.Text = finded[0].Zazem2.ToString();
                textBox3.Text = finded[0].Zazem3.ToString();

                Zam1 = Convert.ToInt32(finded[0].Zazem1);
                Zam2 = Convert.ToInt32(finded[0].Zazem2);
                Zam3 = Convert.ToInt32(finded[0].Zazem3);

                StDefend = finded[0].StDefend;
                StImportant = finded[0].StImportant;
                StNewEquipment = finded[0].StNewEquipment;
                StNar = finded[0].StNar;

                textBox4.Text = finded[0].NarUser;
                textBox5.Text = finded[0].NarObject;
                textBox6.Text = finded[0].NarNum;

                dateTimePicker1.Value = finded[0].DateBegin;
                dateTimePicker2.Value = finded[0].DateEnd;


                if (dateTimePicker2.Value == Convert.ToDateTime("1991.01.01 00:00"))
                {
                    BL.GetServerTime();
                    dateTimePicker2.Value = new ShiftShedule().ServerTime;
                    dateTimePicker2.CustomFormat = "''";
                    dateTimePicker2.Checked = false;
                }

                BL.AddLocked(new OperationalRecord().SelectedID);

                //whoIND = 1;
                //MessageBox.Show((comboBox3.SelectedItem as Employee).EmployeeName);
            }


        }


        //\\\\----Защита от неверного ввода
        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox2.FindString(comboBox2.Text) < 0)
            {
                comboBox2.SelectedIndex = 0;
            }
        }


        //\\\\----Защита от неверного ввода
        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox1.FindString(comboBox1.Text) < 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }


        //\\\\----Защита от неверного ввода
        private void comboBox3_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox3.FindString(comboBox3.Text) < 0)
            {
                comboBox3.SelectedIndex = 0;
            }

            if (comboBox3.Text == "")
            {
                richTextBox1.Text = "";
            }
        }


        //\\\\----Защита от неверного ввода
        private void comboBox4_TextUpdate(object sender, EventArgs e)
        {
            if (comboBox4.FindString(comboBox4.Text) < 0)
            {
                comboBox4.SelectedIndex = 0;
            }

            if (comboBox4.Text == "")
            {
                richTextBox2.Text = "";
            }
        }


        //\\\\----Выставление параметров в текстовое поле
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox3.SelectedIndex != 0)
            {
                richTextBox1.Text = (comboBox3.SelectedItem as Employee).SlujbaName + ": " + (comboBox3.SelectedItem as Employee).EmployeeName;
                richTextBox3.Text = richTextBox3.Text + " " + (comboBox3.SelectedItem as Employee).EmployeeName;
            }
            else
            {
                richTextBox1.Text = "";
            }

        }


        //\\\\----Выставление параметров в текстовое поле
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (whoIND == 1)
            //{
            richTextBox2.Text = (comboBox4.SelectedItem as Employee).SlujbaName + ": " + (comboBox4.SelectedItem as Employee).EmployeeName;

            if (comboBox4.SelectedIndex == 0)
            {
                richTextBox2.Text = "";
            }
            //}
        }


        //\\\\----Выставление параметров в текстовое поле
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedValue != null && comboBox10.SelectedValue != null)
                {
                    comboBox6.DataSource = null;
                    BL.GetLineOfVL(ListLineOfVL, (comboBox5.SelectedItem as Blank).ID, (comboBox1.SelectedItem as Blank).ID);
                    comboBox6.DataSource = ListLineOfVL;
                    comboBox6.ValueMember = "ID";
                    comboBox6.DisplayMember = "Name";

                    if (new OperationalRecord().SelectedFunc == 1)
                    {

                        var finded = new OperationalRecord().ListOfOperationalRecord.Where(item => item.ID == new OperationalRecord().SelectedID).ToList();
                        comboBox6.SelectedValue = finded[0].LineID;

                    }
                }
            }
            catch
            {

            }
        }


        //\\\\----Защита от неверного ввода
        private void comboBox6_TextUpdate(object sender, EventArgs e)
        {

            if (comboBox6.FindString(comboBox6.Text) < 0)
            {
                comboBox6.Text = "";
            }
        }


        //\\\\----Выставление параметров в текстовое поле
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StateLoad == 1)
            {
                //MessageBox.Show("111");
                BL.GetPharaserOfEventsCategory(ListPharaserOfEventsCategory, (comboBox2.SelectedItem as Blank).ID);
                comboBox7.DataSource = null;
                comboBox7.DataSource = ListPharaserOfEventsCategory;
                comboBox7.ValueMember = "ID";
                comboBox7.DisplayMember = "Name";

                //listBox1.DataSource = null;
                //listBox1.DataSource = ListPharaserOfEventsCategory;
                //listBox1.ValueMember = "ID";
                //listBox1.DisplayMember = "Name";

            }

        }


        //\\\\----Выставление параметров в текстовое поле
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                richTextBox3.Text = (comboBox7.SelectedItem as Blank).Name;
            }
            catch
            {
                richTextBox3.Text = "";
            }
        }


        //\\\\----Заземление 1
        private void button3_Click(object sender, EventArgs e)
        {
            Zam1 = Zam1 + 1;
            textBox1.Text = Zam1.ToString();
        }


        //\\\\----Заземление 2
        private void button4_Click(object sender, EventArgs e)
        {
            if (Zam1 > 0)
            {
                Zam2 = Zam2 + 1;
                Zam1 = Zam1 - 1;
                textBox1.Text = Zam1.ToString();
                textBox2.Text = Zam2.ToString();
            }
        }


        //\\\\----Заземление 3
        private void button5_Click(object sender, EventArgs e)
        {
            Zam3 = Zam3 + 1;
            textBox3.Text = Zam3.ToString();
        }


        //\\\\----Обнуление всех заземлений
        private void button6_Click(object sender, EventArgs e)
        {
            Zam1 = 0;
            Zam2 = 0;
            Zam3 = 0;

            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
        }


        //\\\\----Параметр наряда нереализован
        private void button7_Click(object sender, EventArgs e)
        {
            if (StNar == 0)
            {
                StNar = 1;
                return;
            }

            if (StNar == 1)
            {
                StNar = 0;
                return;
            }
        }


        //\\\\----Пометка важное
        private void button8_Click(object sender, EventArgs e)
        {
            if (StImportant == 0)
            {
                StImportant = 1;
                return;
            }

            if (StImportant == 1)
            {
                StImportant = 0;
                return;
            }
        }


        //\\\\----Пометка ввода нового оборудования 
        private void button9_Click(object sender, EventArgs e)
        {
            if (StNewEquipment == 0)
            {
                StNewEquipment = 1;
                return;
            }

            if (StNewEquipment == 1)
            {
                StNewEquipment = 0;
                return;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void AddEvent_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (new OperationalRecord().SelectedFunc == 1)
            {
                BL.DeleteLocked(new OperationalRecord().SelectedID);
            }
        }

        private void comboBox2_MeasureItem(object sender, MeasureItemEventArgs e)
        {

        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void comboBox8_TextUpdate(object sender, EventArgs e)
        {

        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = true;
            panel2.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = false;
            panel2.Visible = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(comboBox1.SelectedIndex.ToString());
            try
            {
                ListClassVL.Clear();
                BL.GetClassVL(ListClassVL);
                comboBox5.DataSource = null;
                comboBox5.DataSource = ListClassVL;
                comboBox5.ValueMember = "ID";
                comboBox5.DisplayMember = "Name";
                //comboBox5.Text = "";
                comboBox5.SelectedValue = finded[0].ClassVLID;
                //ListClassVL.Clear();
                //comboBox5.DataSource = null;
                //BL.GetClassVL(ListClassVL);
                //comboBox5.DataSource = ListClassVL;
                //comboBox5.ValueMember = "ID";
                //comboBox5.DisplayMember = "Name";
            }
            catch
            {
                comboBox5.SelectedIndex = 0;
            }
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (StateLoad == 1)
            {
                //MessageBox.Show("111");
                if ((comboBox10.SelectedItem as Blank).ID != "00000000-0000-0000-0000-000000000000")
                {
                    BL.GetObjects(ListObjects, EM.SelectIDSlujba, (comboBox10.SelectedItem as Blank).ID);
                    comboBox1.DataSource = null;
                    comboBox1.DataSource = ListObjects;
                    comboBox1.ValueMember = "ID";
                    comboBox1.DisplayMember = "Name";
                    comboBox1.Text = "";
                }
                else
                {
                    //ListObjects.Clear();
                    //ListObjects.Add(new Blank
                    //{
                    //    ID = "00000000-0000-0000-0000-000000000000",
                    //    Name = ""
                    //});
                    //comboBox1.DataSource = null;
                    //comboBox1.DataSource = ListObjects;
                }


                if (new OperationalRecord().SelectedFunc == 1)
                {
                    try
                    {
                        var finded = new OperationalRecord().ListOfOperationalRecord.Where(item => item.ID == new OperationalRecord().SelectedID).ToList();
                        comboBox1.SelectedValue = finded[0].LineID;
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Checked == true &&
                dateTimePicker1.Value > dateTimePicker2.Value && dateTimePicker2.Value.Year != 1991)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddSeconds(+1);
            }

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox6.Text == "0")
            {
                button7.Enabled = false;
            }
            else
            {
                button7.Enabled = true;
            }

        }

        private void comboBox7_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            //var lbox = (ComboBox)sender;
            //// var text = lbox.Items[e.Index].ToString();
            //var text = comboBox7.GetItemText(lbox.Items[e.Index]);
            //var width = lbox.ClientSize.Width;
            //var size = e.Graphics.MeasureString(text, lbox.Font, width);
            //e.ItemHeight = (int)size.Height;
        }

        private void comboBox8_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex != 0)
            {
                richTextBox3.Text = richTextBox3.Text + " " + (comboBox8.SelectedItem as Blank).Name;
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            BL.GetServerTime();
            dateTimePicker1.Value = new ShiftShedule().ServerTime;

        }

        private void button14_Click(object sender, EventArgs e)
        {
            BL.GetServerTime();
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddSeconds(+1);
            }
            else
            {
                dateTimePicker2.Value = new ShiftShedule().ServerTime;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            var dtp = sender as DateTimePicker;
            if (!dtp.ShowCheckBox || dtp.Checked)
            {
                dtp.CustomFormat = "dd.MM.yyyy  HH:mm";

            }
            else
            {
                dtp.CustomFormat = "''";
            }

            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddSeconds(+1);
                dtp.CustomFormat = "''";
                dateTimePicker2.Checked = false;
            }

        }


        //\\\\----Пометка защита
        private void button10_Click(object sender, EventArgs e)
        {
            if (StDefend == 0)
            {
                StDefend = 1;
                return;
            }

            if (StDefend == 1)
            {
                StDefend = 0;
                return;
            }
        }


        //\\\\----Функция записи или добавления записи.
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            DateTime DTEND = new DateTime();

            if (comboBox2.SelectedValue == null)
            {
                StrEventID = "00000000-0000-0000-0000-000000000000";
            }
            else
            {
                StrEventID = (comboBox2.SelectedItem as Blank).ID;
            }


            if (comboBox3.SelectedValue == null)
            {
                StrWhoID = "00000000-0000-0000-0000-000000000000";
                strWhoName = "";
            }
            else
            {
                StrWhoID = (comboBox3.SelectedItem as Employee).EmployeeID;
                strWhoName = (comboBox3.SelectedItem as Employee).EmployeeName;
            }


            if (comboBox4.SelectedValue == null)
            {
                StrWhomID = "00000000-0000-0000-0000-000000000000";
                strWhomName = "";
            }
            else
            {
                StrWhomID = (comboBox4.SelectedItem as Employee).EmployeeID;
                strWhomName = (comboBox4.SelectedItem as Employee).EmployeeName;
            }


            if (comboBox5.SelectedValue == null)
            {
                StrCVID = "00000000-0000-0000-0000-000000000000";
            }
            else
            {
                StrCVID = (comboBox5.SelectedItem as Blank).ID;
            }


            if (comboBox6.SelectedValue == null)
            {
                StrLineID = "00000000-0000-0000-0000-000000000000";
                strLineName = "";
            }
            else
            {
                StrLineID = (comboBox6.SelectedItem as Blank).ID;
                strLineName = (comboBox6.SelectedItem as Blank).Name;
            }

            if (comboBox8.SelectedValue == null)
            {
                StrProtectsID = "00000000-0000-0000-0000-000000000000";
                strProtectsName = "";
            }
            else
            {
                StrProtectsID = (comboBox8.SelectedItem as Blank).ID;
                strProtectsName = (comboBox8.SelectedItem as Blank).Name;
            }

            if (comboBox9.SelectedValue == null)
            {
                StrReportedID = "00000000-0000-0000-0000-000000000000";
                strReportedName = "";
            }
            else
            {
                StrReportedID = (comboBox9.SelectedItem as Blank).ID;
                strReportedName = (comboBox9.SelectedItem as Blank).Name;
            }

            if (comboBox10.SelectedValue == null)
            {
                StrOrganizationID = "00000000-0000-0000-0000-000000000000";
                strOrganizationName = "";
            }
            else
            {
                StrOrganizationID = (comboBox10.SelectedItem as Blank).ID;
                strOrganizationName = (comboBox10.SelectedItem as Blank).Name;
            }


            if (comboBox1.SelectedValue == null || comboBox1.Text == "")
            {
                StrObjectID = "00000000-0000-0000-0000-000000000000";
                StrLineID = "00000000-0000-0000-0000-000000000000";
                strLineName = "";
                strObjectName = "";
            }
            else
            {
                StrObjectID = (comboBox1.SelectedItem as Blank).ID;
                strObjectName = (comboBox1.SelectedItem as Blank).Name;
            }

            if (dateTimePicker2.Checked == false)
            {
                dateTimePicker2.Value = Convert.ToDateTime("01.01.1991");
                DTEND = Convert.ToDateTime("01.01.1991");
            }
            else
            {
                DTEND = dateTimePicker2.Value;
            }


            try
            {
                ListAddOperationalRecord.Clear();

                ListAddOperationalRecord.Add(new OperationalRecord
                {
                    ObjectID = StrObjectID,
                    EventID = StrEventID,
                    LineID = StrLineID,
                    WhoID = StrWhoID,
                    WhoText = richTextBox1.Text,
                    WhomID = StrWhomID,
                    WhomText = richTextBox2.Text,
                    Message = richTextBox3.Text,
                    Note = richTextBox4.Text,
                    DateBegin = dateTimePicker1.Value,
                    DateEnd = DTEND,
                    EmployeeID = new Employee().SelectIDEmployee,
                    ProtectsID = StrProtectsID,
                    StImportant = StImportant,
                    StNewEquipment = StNewEquipment,
                    StDefend = StDefend,
                    ReportedID = StrReportedID,
                    Zazem1 = Convert.ToByte(textBox1.Text),
                    Zazem2 = Convert.ToByte(textBox2.Text),
                    Zazem3 = Convert.ToByte(textBox3.Text),
                    SlujbaID = new Employee().SelectIDSlujba,
                    NarUser = textBox4.Text,
                    NarObject = textBox5.Text,
                    NarNum = textBox6.Text,
                    StNar = StNar,
                    ShiftSheduleID = EM.SelectShiftShedule,
                    OrganizationID = StrOrganizationID,
                });



                if (new OperationalRecord().SelectedFunc == 0)
                {
                    //BL.operationalRecordNum = BL.operationalRecordNum + 1;
                    BL.AddOperationalRecord(ListAddOperationalRecord);
                    Logs.RecordLogs("Добавление новой записи", "00000000-0000-0000-0000-000000000000");

                    if (ListAddOperationalRecord[0].DateEnd.Year == Convert.ToDateTime("01.01.1991 00:00").Year)
                    {
                        ListAddOperationalRecord[0].DateEnd2 = "";
                    }
                    else
                    {
                        ListAddOperationalRecord[0].DateEnd2 = Convert.ToDateTime(ListAddOperationalRecord[0].DateEnd2).ToShortDateString() +
                        " " + Convert.ToDateTime(ListAddOperationalRecord[0].DateEnd2).ToShortTimeString();
                    }

                }


                if (new OperationalRecord().SelectedFunc == 1)
                {
                    BL.operationalRecordNum = BL.operationalRecordNum + 1;
                    BL.EditOperationalRecord(ListAddOperationalRecord);

                    int index = OR.ListOfOperationalRecord.IndexOf(OR.ListOfOperationalRecord.First(x => x.ID.ToLower() == new OperationalRecord().SelectedID.ToLower()));

                    String DTEND2 = "";
                    if (DTEND.Year == Convert.ToDateTime("01.01.1991 00:00").Year)
                    {
                        DTEND2 = "";
                    }
                    else
                    {
                        DTEND2 = Convert.ToDateTime(DTEND).ToShortDateString() +
                        " " + Convert.ToDateTime(DTEND).ToShortTimeString();
                    }

                    OR.ListOfOperationalRecord[index] = new OperationalRecord
                    {
                        ID = new OperationalRecord().SelectedID,
                        ObjectID = StrObjectID,
                        ObjectName = strObjectName + "\n" + strLineName,
                        EventID = StrEventID,
                        LineID = StrLineID,
                        LineName = strLineName,
                        WhoID = StrWhoID,
                        ClassVLID = StrCVID,
                        WhoName = strWhoName,
                        WhoText = richTextBox1.Text,
                        WhomID = StrWhomID,
                        WhomName = strWhomName,
                        WhomText = richTextBox2.Text,
                        Message = richTextBox3.Text,
                        Note = richTextBox4.Text,
                        DateBegin = dateTimePicker1.Value,
                        DateEnd = DTEND,
                        DateEnd2 = DTEND2,
                        EmployeeID = new Employee().SelectIDEmployee,
                        ProtectsID = StrProtectsID,
                        StImportant = StImportant,
                        StNewEquipment = StNewEquipment,
                        StDefend = StDefend,
                        ReportedID = StrReportedID,
                        ReportedName = strReportedName,
                        Zazem1 = Convert.ToByte(textBox1.Text),
                        Zazem2 = Convert.ToByte(textBox2.Text),
                        Zazem3 = Convert.ToByte(textBox3.Text),
                        SlujbaID = new Employee().SelectIDSlujba,
                        NarUser = textBox4.Text,
                        NarObject = textBox5.Text,
                        NarNum = textBox6.Text,
                        StNar = StNar,
                        ShiftSheduleID = EM.SelectShiftShedule,
                        OrganizationID = StrOrganizationID,
                        Nar = textBox4.Text + "\n" + textBox5.Text + "\n" + textBox6.Text,
                        EmployeeName = new Employee().SelectNameEmployee,
                    };

                }

                button1.Enabled = true;
                button2.Enabled = true;
                BL.StLocal = 1;

                this.Close();
            }

            catch
            {
                this.Close();
            }

        }


    }
}
