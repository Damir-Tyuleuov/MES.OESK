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
    public partial class ShiftSheduleForm : Form
    {
        Image image = new Bitmap(MES.OESK.Properties.Resources.lock_open);
        Blank BL = new Blank();
        Employee EM = new Employee();
        List<ShiftShedule> ListShiftShedule = new List<ShiftShedule>();

        List<Employee> ListEmployee = new List<Employee>();
        List<Employee> ListEmployeeShiftShedule = new List<Employee>();
        public ShiftSheduleForm()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);

            InitializeComponent();
        }

        private void ShiftSheduleForm_Load(object sender, EventArgs e)
        {

            BL.GetShiftShedule(ListShiftShedule);
            BL.GetEmployee(ListEmployee, EM.SelectIDSlujba);

            for(int i=0;i<ListEmployee.Count;i++)
            {
                checkedListBox1.Items.Add(ListEmployee[i].EmployeeName);
            }

            for (int i = 0; i < ListShiftShedule.Count; i++)
            {
                dataGridView1.Rows.Add(
                     ListShiftShedule[i].ShiftSheduleID,
                     ListShiftShedule[i].ShiftID,
                     ListShiftShedule[i].ShiftSheduleDate.ToLongDateString());
                if (ListShiftShedule[i].IsClosed==1)
                {
                    dataGridView1[3, i].Value = image;
                }
            }
            dataGridView1.ClearSelection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id = Guid.NewGuid().ToString();
            BL.GetServerTime();

            if (ListShiftShedule.Count == 0)
            {
                ListShiftShedule.Add(new ShiftShedule { ShiftSheduleID = id,ShiftID = "1", ShiftSheduleDate = new ShiftShedule().ServerTime });
                dataGridView1.Rows.Add(id, "1", new ShiftShedule().ServerTime.ToLocalTime().ToLongDateString());
                BL.AddShiftShedule(id, 1, new ShiftShedule().ServerTime.ToLocalTime(), EM.SelectIDSlujba);
                id = Guid.NewGuid().ToString();
            }
            if (ListShiftShedule.Count > 0)
            {
                var item = ListShiftShedule.Last();
                if (item.ShiftID == "1")
                {
                    ListShiftShedule.Add(new ShiftShedule { ShiftSheduleID = id, ShiftID = "2", ShiftSheduleDate = item.ShiftSheduleDate });
                    dataGridView1.Rows.Add(id, "2", item.ShiftSheduleDate.ToLongDateString());
                    BL.AddShiftShedule(id, 2, item.ShiftSheduleDate, EM.SelectIDSlujba);
                }
                if (item.ShiftID == "2")
                {
                    ListShiftShedule.Add(new ShiftShedule { ShiftSheduleID = id, ShiftID = "1", ShiftSheduleDate = item.ShiftSheduleDate.AddDays(+1) });
                    dataGridView1.Rows.Add(id, "1", item.ShiftSheduleDate.AddDays(+1).ToLongDateString());
                    BL.AddShiftShedule(id, 1, item.ShiftSheduleDate.AddDays(+1), EM.SelectIDSlujba);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ListShiftShedule.Count > 0)
            {
                if (checkedListBox1.CheckedItems.Count > 0)
                {
                    BL.ClearShiftShedule(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                    for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                    {

                        BL.AddEmployeeShiftShedule(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString(),
                        ListEmployee.Find(x => x.EmployeeName == checkedListBox1.CheckedItems[i].ToString()).EmployeeID);
                    }

                    MessageBox.Show("Дежурный персонал добавлен");
                }
                else
                {
                    MessageBox.Show("Выберите держурный персонал");
                }
            }
            else
            {
                MessageBox.Show("Смены отсутвуют");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                ListEmployeeShiftShedule.Clear();
                BL.GetEmployeeShiftShedule(ListEmployeeShiftShedule, dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                for (int y = 0; y < checkedListBox1.Items.Count; y++)
                {
                    checkedListBox1.SetItemChecked(y, false);
                }

                for (int i = 0; i < ListEmployeeShiftShedule.Count; i++)
                {
                     checkedListBox1.SetItemCheckState(ListEmployee.FindIndex(x => x.EmployeeID == ListEmployeeShiftShedule[i].EmployeeID), CheckState.Checked);
                }
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BL.GetServerTime();
            DateTime dt = new ShiftShedule().ServerTime;
            var item = ListShiftShedule.Where(x => x.ShiftSheduleID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

            if (ListShiftShedule.Where(x => x.IsClosed == 1).ToList().Count == 0 && ListShiftShedule.Count > 0)
            {

                //\\----1 Смена
                if (item[0].ShiftID == "1")
                {
                    if (dt.Day == ListShiftShedule.Find(x => x.ShiftSheduleID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ShiftSheduleDate.Day
                       && dt.Hour > 5
                       && dt.Hour < 22
                       )
                    {
                       
                            BL.OpenShiftShedule(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                        BL.GetShiftShedule(ListShiftShedule);
                        dataGridView1.Rows.Clear();
                        for (int i = 0; i < ListShiftShedule.Count; i++)
                        {
                            dataGridView1.Rows.Add(
                                 ListShiftShedule[i].ShiftSheduleID,
                                 ListShiftShedule[i].ShiftID,
                                 ListShiftShedule[i].ShiftSheduleDate.ToLongDateString());
                            if (ListShiftShedule[i].IsClosed == 1)
                            {
                                dataGridView1[3, i].Value = image;
                            }
                        }

                        MessageBox.Show("Смена открыта");
                            return;
                 
                    }
                    else
                    {
                        MessageBox.Show("I Смену возможно открыть в текущий день c 06-00 - до 9-00");
                        return;
                    }

                }


                //\\----2 Смена
                if (item[0].ShiftID == "2")
                {
                    if (ListShiftShedule.Find(x => x.ShiftSheduleID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ShiftSheduleDate.Day < dt.AddDays(+2).Day
                       && dt.Hour > 17 && dt.Hour < 23)
                    {
                        BL.OpenShiftShedule(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                        BL.GetShiftShedule(ListShiftShedule);

                        BL.GetShiftShedule(ListShiftShedule);
                        dataGridView1.Rows.Clear();
                        for (int i = 0; i < ListShiftShedule.Count; i++)
                        {
                            dataGridView1.Rows.Add(
                                 ListShiftShedule[i].ShiftSheduleID,
                                 ListShiftShedule[i].ShiftID,
                                 ListShiftShedule[i].ShiftSheduleDate.ToLongDateString());
                            if (ListShiftShedule[i].IsClosed == 1)
                            {
                                dataGridView1[3, i].Value = image;
                            }
                        }
                        MessageBox.Show("Смена открыта");

                        return;
                    }
                    else
                    {
                        MessageBox.Show("II Смену возможно открыть в текущий день c 18-00 - до 21-00");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Невозможно открыть несколько смен одновременно");
                return;
            }


        }

    }
}
