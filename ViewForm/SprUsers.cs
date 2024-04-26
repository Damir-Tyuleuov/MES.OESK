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

namespace MES.OESK.ViewForm
{
    public partial class SprUsers : Form
    {

        Blank BL = new Blank();
        List<Blank> ListAllSlujb = new List<Blank>();
        List<Employee> ListUserOfSlujba = new List<Employee>();
        List<Employee> ListUserOfSlujba2 = new List<Employee>();

        public SprUsers()
        {
            InitializeComponent();
        }

        private void SprUsers_Load(object sender, EventArgs e)
        {
            BL.GetAllSlujb(ListAllSlujb);
            comboBox1.DataSource = ListAllSlujb;
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "Name";
            label3.Text = "Пользователи " + new Employee().SelectNameSlujba;
            UpdateEmpolyeeSlujb();
         
        }


        private void UpdateEmpolyeeSlujb()
        {
            ListUserOfSlujba2.Clear();
            BL.GetEmployeesOfSlujba(ListUserOfSlujba2);
            ListUserOfSlujba2.RemoveAt(0);

            dataGridView2.Rows.Clear();
            for (int i = 0; i < ListUserOfSlujba2.Count; i++)
            {
                if (ListUserOfSlujba2[i].EmployeeName != "")
                    dataGridView2.Rows.Add(
                    ListUserOfSlujba2[i].EmployeeID,
                    ListUserOfSlujba2[i].EmployeeName,
                    ListUserOfSlujba2[i].SlujbaName);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSprEmployee();
        }


        private void GetSprEmployee()
        {
                BL.GetSprEmployeesOfSlujba(ListUserOfSlujba, (comboBox1.SelectedItem as Blank).ID);
                dataGridView1.Rows.Clear();

                for (int i = 0; i < ListUserOfSlujba.Count; i++)
                {
                    if (ListUserOfSlujba[i].EmployeeName != "")
                        dataGridView1.Rows.Add(
                        ListUserOfSlujba[i].EmployeeID,
                        ListUserOfSlujba[i].EmployeeName,
                        ListUserOfSlujba[i].SelectIDSlujba);
                }
               // dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
        }

        //\\---- Привязка пользователей
        private void button1_Click(object sender, EventArgs e)
        {
            var LC = ListUserOfSlujba2.Where(U => U.EmployeeID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

            if (LC.Count==0)
            {
                BL.AddEmployeeForSPR(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString(), 
                (comboBox1.SelectedItem as Blank).ID, 
                new Employee().SelectIDSlujba);

                UpdateEmpolyeeSlujb();

                MessageBox.Show("Пользователь " + dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString() + " успешно добавлен");
            }
            else
            {
                MessageBox.Show("Данный пользователь уже добавлен");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
                DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView2[1, dataGridView2.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    BL.DeleteEmployeeForSPR(dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString());
                    UpdateEmpolyeeSlujb();
                }
            //}
            //catch
            //{
            //    MessageBox.Show("Выберите пользователя");
            //}
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column1"].Value = Guid.NewGuid();
            e.Row.Cells["Column2"].Value = "Новое имя";
        }


        private void Save(string ID, string Name)
        {
            try
            {

                if (Name != "Новое имя" || Name !="")
                {
                    BL.AddEditPersonal(
                        ID,
                        Name,
                        (comboBox1.SelectedItem as Blank).ID);
                    GetSprEmployee();
                  
                    MessageBox.Show("Запись '"+ Name +"' сохранена");
                }
            }
            catch
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
                {
                    if (ListUserOfSlujba.Count > 0)
                    {

                        DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            BL.DeleteEmployeeSPR(dataGridView1[0, dataGridView1.SelectedCells[0].RowIndex].Value.ToString());
                            GetSprEmployee();
                            UpdateEmpolyeeSlujb();
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("Выберите запись");
            }
        }


        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
            {
                Save(dataGridView1[0, e.RowIndex].Value.ToString(),
                     dataGridView1[1, e.RowIndex].Value.ToString());
            }
        }


    }
}
