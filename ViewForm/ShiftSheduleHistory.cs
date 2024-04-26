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
    public partial class ShiftSheduleHistory : Form
    {

        Image image = new Bitmap(MES.OESK.Properties.Resources.lock_open);
        Image image2 = new Bitmap(MES.OESK.Properties.Resources._lock);

        Blank BL = new Blank();
        Employee EM = new Employee();
        List<ShiftShedule> ListShiftShedule = new List<ShiftShedule>();

        List<Employee> ListEmployee = new List<Employee>();
        List<Employee> ListEmployeeShiftShedule = new List<Employee>();

        public ShiftSheduleHistory()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);

            InitializeComponent();
        }

        private void ShiftSheduleHistory_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-7);
            dateTimePicker2.Value = DateTime.Now.ToLocalTime();
           
        }


        private void GetShiftShedule()
        {
            BL.GetShiftSheduleHistory(ListShiftShedule, dateTimePicker1.Value, dateTimePicker2.Value);

            dataGridView1.Rows.Clear();
            for (int i = 0; i < ListShiftShedule.Count; i++)
            {
                if (i < ListShiftShedule.Count - 1)
                {
                    if (ListShiftShedule[i].ShiftSheduleID == ListShiftShedule[i + 1].ShiftSheduleID)
                    {
                        ListShiftShedule[i + 1].ShiftSheduleEmployee =
                        ListShiftShedule[i + 1].ShiftSheduleEmployee + " , " +
                        ListShiftShedule[i].ShiftSheduleEmployee;
                        continue;
                    }
                }

                        dataGridView1.Rows.Add(
                        ListShiftShedule[i].ShiftSheduleID,
                        ListShiftShedule[i].ShiftID,
                        ListShiftShedule[i].ShiftSheduleDate.ToLongDateString(),
                        ListShiftShedule[i].ShiftSheduleStartDate.ToString() + " - " +
                        ListShiftShedule[i].ShiftSheduleEndDate.ToString(),
                        ListShiftShedule[i].ShiftSheduleEmployee);

              

                //if (ListShiftShedule[i].IsClosed == 2)
                //{
                //    // dataGridView1[5, i].Value = image2;
                //}



            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (ListShiftShedule.Find(x => x.ShiftSheduleID.ToLower() == dataGridView1[0, i].Value.ToString().ToLower()).IsClosed == 1)
                {
                    //MessageBox.Show("111");
                    dataGridView1[5, i].Value = image;
                }

                if (ListShiftShedule.Find(x => x.ShiftSheduleID.ToLower() == dataGridView1[0, i].Value.ToString().ToLower()).IsClosed == 2)
                {
                    dataGridView1[5, i].Value = image2;
                }

            }

            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                new ShiftShedule().HistoryMod = 1;
                new ShiftShedule().HistoryshiftShedule = dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString();
                BL.GetOperationalRecordHistory(new Employee().SelectIDSlujba, new ShiftShedule().HistoryshiftShedule);
                BL.GetShiftSheduleSelectHistory(new ShiftShedule().HistoryshiftShedule);
                this.Close();
            }
           
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value> dateTimePicker2.Value)
            {
                dateTimePicker1.Value = dateTimePicker2.Value.AddDays(-1);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(+1);
            }
            if (dateTimePicker2.Value>DateTime.Now.ToLocalTime())
            {
                dateTimePicker2.Value = DateTime.Now.ToLocalTime();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetShiftShedule();
        }
    }
}
