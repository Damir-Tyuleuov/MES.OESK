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
using System.Collections.Specialized;
using System.Configuration;
using System.Net.NetworkInformation;
using MES.OESK.ViewForm;
using MES.OESK.Class;
using System.Net;
using System.Net.Sockets;

namespace DispetcherRes
{
    public partial class AuthorForm : Form
    {
        public AuthorForm()
        {
            InitializeComponent();
        }

        Blank BL = new Blank();
        Employee EM = new Employee();
        Logs Logs = new Logs();

        List<Blank> ListSlujba = new List<Blank>();
        List<Blank> ListSlujba2 = new List<Blank>();
        List<Employee> ListEmployee = new List<Employee>();
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
     

        private void AuthorForm_Load(object sender, EventArgs e)
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            try
            { 
                //MessageBox.Show(ConfigurationManager.AppSettings["SlujbaID"]);
                EM.SelectRegionID = config.AppSettings.Settings["RegionID"].Value;
                BL.GetSlujba(ListSlujba, EM.SelectRegionID);
                comboBox1.DataSource = ListSlujba;
                comboBox1.ValueMember = "ID";
                comboBox1.DisplayMember = "Name";
                comboBox1.SelectedValue = config.AppSettings.Settings["SlujbaID"].Value.ToLower();
                ProcessorId = GetMacAddress();

                EM.EmployeePCInfo = Environment.UserDomainName + "/" + Environment.MachineName + "/" + Environment.UserName + "/";
                EM.EmployeeIPInfo = Dns.GetHostAddresses(Dns.GetHostName()).Where(address =>
                address.AddressFamily == AddressFamily.InterNetwork).First().ToString();
            }
            catch
            {
                MessageBox.Show("Отсутвует связь с сервером");
                this.Close();
                return;
            }
        }

        static string ProcessorId { get; set; }

        private string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }


        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ListEmployee.Clear();
            BL.GetShiftSheduleEmployee(ListEmployee, (comboBox1.SelectedItem as Blank).ID);

            comboBox2.DataSource = null;
            comboBox2.DataSource = ListEmployee;
            comboBox2.DisplayMember = "EmployeeName";

            if (ListEmployee.Count > 0)
            {
                BL.GetShiftSheduleAction(ListEmployee[0].ShiftSheduleID);
                label17.Text = (comboBox1.SelectedItem as Blank).Name + " (cмена: " + new ShiftShedule().ListShiftShedule[0].ShiftID + "-ая)";
            }
            else
            {
                label17.Text = (comboBox1.SelectedItem as Blank).Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Entrance();
        }

        private void Entrance()
        {
            try
            {
                if (Convert.ToByte(BL.Author(ProcessorId, (comboBox2.SelectedItem as Employee).EmployeeID, textBox1.Text)) == 1)
                {

                    var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


                    EM.SelectIDSlujba = (comboBox1.SelectedItem as Blank).ID;
                    EM.SelectNameSlujba = (comboBox1.SelectedItem as Blank).Name;
                    EM.SelectIDEmployee = (comboBox2.SelectedItem as Employee).EmployeeID;
                    EM.SelectNameEmployee = (comboBox2.SelectedItem as Employee).EmployeeName;
                    EM.SelectShiftShedule = (comboBox2.SelectedItem as Employee).ShiftSheduleID;
                    EM.EmployeePCInfo = Environment.UserDomainName + "/" + Environment.MachineName + "/" + Environment.UserName + "/";
                    EM.EmployeeIPInfo = Dns.GetHostAddresses(Dns.GetHostName()).Where(address =>
                    address.AddressFamily == AddressFamily.InterNetwork).First().ToString();

                    BL.GetShiftSheduleAction(ListEmployee[0].ShiftSheduleID);


                    if (radioButton1.Checked == true)
                    {

                        config.AppSettings.Settings["RegionID"].Value = "BFAF5261-B97A-4083-8AEF-36068B0558EE";
                    }

                    if (radioButton2.Checked == true)
                    {
                        config.AppSettings.Settings["RegionID"].Value = "4576468C-25E7-4509-9324-B2A1B5BCC36F";
                    }

                    config.AppSettings.Settings["SlujbaID"].Value = EM.SelectIDSlujba;


                    config.Save(ConfigurationSaveMode.Modified);


                    Logs.RecordLogs("Авторизация", "00000000-0000-0000-0000-000000000000");

                    this.DialogResult = DialogResult.OK;

                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка авторизации");
            }

        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                radioButton2.Checked = false;
                EM.SelectRegionID = "BFAF5261-B97A-4083-8AEF-36068B0558EE";
                config.AppSettings.Settings["RegionID"].Value = "BFAF5261-B97A-4083-8AEF-36068B0558EE";
                BL.GetSlujba(ListSlujba, EM.SelectRegionID);

                comboBox1.DataSource = ListSlujba;
                comboBox1.ValueMember = "Name";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                radioButton1.Checked = false;

                EM.SelectRegionID = "4576468C-25E7-4509-9324-B2A1B5BCC36F";
                config.AppSettings.Settings["RegionID"].Value = "4576468C-25E7-4509-9324-B2A1B5BCC36F";
                BL.GetSlujba(ListSlujba2, EM.SelectRegionID);

                comboBox1.DataSource = ListSlujba2;
                comboBox1.ValueMember = "Name";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Entrance();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            panel3.Visible = false;
            panel4.Visible = true;
            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = true;

            ListEmployee.Clear();
            BL.GetShiftSheduleEmployee(ListEmployee, (comboBox1.SelectedItem as Blank).ID);
            //BL.GetEmployee(ListEmployee, (comboBox1.SelectedItem as Blank).ID);

            comboBox2.DataSource = null;
            comboBox2.DataSource = ListEmployee;
            comboBox2.DisplayMember = "EmployeeName";

            return;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EM.SelectIDSlujba = (comboBox1.SelectedItem as Blank).ID;
            ShiftSheduleForm SSF = new ShiftSheduleForm();
            SSF.ShowDialog();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
