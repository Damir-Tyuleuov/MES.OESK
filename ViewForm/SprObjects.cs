using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DispetcherRes.Class;

namespace MES.OESK.ViewForm
{
    public partial class SprObjects : Form
    {
        Blank BL = new Blank();
        Employee EM = new Employee();
        List<Blank> ListObjects = new List<Blank>();
        List<Blank> ListSearchObjects = new List<Blank>();
        List<Blank> ListOrganizations = new List<Blank>();
        byte StateUpdate = 0;

        public SprObjects()
        {
            InitializeComponent();
        }

        private void SprObjects_Load(object sender, EventArgs e)
        {
            GetOrganization();
            StateUpdate = 1;
            //dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
        }


        private void GetOrganization()
        {
            dataGridView1.Rows.Clear();
            ListOrganizations.Clear();
            BL.GetOrganizations(ListOrganizations);
            ListOrganizations.RemoveAt(0);

           
            if (ListOrganizations.Count > 0)
            {
                for (int i = 0; i < ListOrganizations.Count; i++)
                {
                    dataGridView1.Rows.Add(
                   ListOrganizations[i].ID,
                   ListOrganizations[i].Name);
                }
            }
        }


        private void GetObject(string ID)
        {
            if (StateUpdate==1 && dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
            {
                BL.GetObjects(ListObjects, EM.SelectIDSlujba, ID);
                ListObjects.RemoveAt(0);
                dataGridView2.Rows.Clear();
                if (ListObjects.Count > 0)
                {
                    for (int i = 0; i < ListObjects.Count; i++)
                    {
                        dataGridView2.Rows.Add(
                        ListObjects[i].ID,
                        ListObjects[i].Name);
                    }
                    //dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
                }
            }
            else
            {
                ListObjects.Clear();
                dataGridView2.Rows.Clear();

            }
        }


        private void Save2(string ID, string Name)
        {
            try
            {

                if (dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString()!= "Новое имя" &&
                    (Name != "Новое имя" || Name!=""))
                {
                    BL.AddEditObject(
                        ID,
                        Name,
                        dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                    MessageBox.Show("Запись '" + Name + "' сохранена");
                    GetObject(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                }
            }
            catch
            {

            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ListObjects.Count > 1)
            {
                Search();

                if (textBox1.Text == "")
                {
                    Zapolnenie(ListObjects);
                }
            }
        }

        private void Search()
        {
            ListSearchObjects = ListObjects.Where(u => u.Name.ToUpper().Contains(
            textBox1.Text.ToUpper())).ToList();

            Zapolnenie(ListSearchObjects);
        }

        private void Zapolnenie(List<Blank> List)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < List.Count; i++)
            {
                dataGridView2.Rows.Add(List[i].ID, List[i].Name);
            }
            if (List.Count > 0)
            {
                //dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2[1, dataGridView2.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
                {
                    if (dataGridView2.Rows.Count > 0)
                    {

                        DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView2[1, dataGridView2.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            BL.DeleteObject(dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString());

                            GetObject(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                        }

                    }
                }

            }
            catch
            {
                MessageBox.Show("Выберите запись");
            }
        }

        private void dataGridView2_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column1"].Value = Guid.NewGuid();
            e.Row.Cells["Column2"].Value = "Новое имя";
        }


        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column3"].Value = Guid.NewGuid();
            e.Row.Cells["Column4"].Value = "Новое имя";
        }

        private void Save(string ID, string Name)
        {
            try
            {

                if (Name != "Новое имя" || Name!="")
                {
                    BL.AddEditObjectOrganizations(
                         ID,
                         Name,
                         EM.SelectIDSlujba);
                    MessageBox.Show("Запись '" + Name + "' сохранена");
                    GetObject(ID);
                }
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
            {

                DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    BL.DeleteObject(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                    GetOrganization();
                }

            }
            
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null || 
                dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString() != "Новое имя" ||
                dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString() != "")
            {
                Save(dataGridView1[0, e.RowIndex].Value.ToString(),
                     dataGridView1[1, e.RowIndex].Value.ToString());
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2[e.ColumnIndex, e.RowIndex].Value != null || 
                dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString() != "Новое имя"||
                dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString() != "")
            {
                Save2(dataGridView2[0, e.RowIndex].Value.ToString(),
                     dataGridView2[1, e.RowIndex].Value.ToString());
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                GetObject(dataGridView1[0, e.RowIndex].Value.ToString());
            }
            catch { }
        }
    }
}
