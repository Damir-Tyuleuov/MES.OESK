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
    public partial class MailForm : Form
    {
        Blank BL = new Blank();
        Logs Logs = new Logs();
        List<Blank> ListSlujba = new List<Blank>();
        public MailForm()
        {
            InitializeComponent();
        }

        private void MailForm_Load(object sender, EventArgs e)
        {
            BL.GetSlujba(ListSlujba, new Employee().SelectRegionID);
            comboBox1.DataSource = ListSlujba;
            comboBox1.ValueMember = "Name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length != 0)
            {
                BL.GiveMail(
                    new Employee().SelectIDSlujba,
                    new Employee().SelectIDEmployee,
                    richTextBox1.Text, 
                    (comboBox1.SelectedItem as Blank).ID);
                Logs.RecordLogs("Отправка письма - "+(comboBox1.SelectedItem as Blank).Name, "00000000-0000-0000-0000-000000000000");
                this.Close();
            }
            else
            {
                MessageBox.Show("Сообщение пустое");
            }
        }


    }
}
