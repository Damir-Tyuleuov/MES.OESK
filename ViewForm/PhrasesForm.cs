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

    public partial class PhrasesForm : Form
    {
        Blank BL = new Blank();
        Employee EM = new Employee();
        List<Blank> ListEventCategory = new List<Blank>();
        List<Blank> ListPharaserOfEventsCategory = new List<Blank>();
        List<Blank> SearchListPharaserOfEventsCategory = new List<Blank>();
        byte StateUpdate = 0;

        public PhrasesForm()
        {
            InitializeComponent();
        }


        private void PhrasesForm_Load(object sender, EventArgs e)
        {
            GetEventsCategory();
            StateUpdate = 1;
        }


        private void Zapolnenie(string ID)
        {
            try
            {
                if (StateUpdate == 1 && ListEventCategory.Count > 0 && dataGridView2[1, dataGridView2.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
                {
                    BL.GetPharaserOfEventsCategory(ListPharaserOfEventsCategory, ID);
                    dataGridView1.Rows.Clear();
                    for (int i = 0; i < ListPharaserOfEventsCategory.Count; i++)
                        dataGridView1.Rows.Add(ListPharaserOfEventsCategory[i].ID, ListPharaserOfEventsCategory[i].Name);
                    //dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
                }
                else
                {
                    ListPharaserOfEventsCategory.Clear();
                    dataGridView1.Rows.Clear();

                }
            }
            catch { }
        }


        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column1"].Value = Guid.NewGuid();
            e.Row.Cells["Column2"].Value = "Новое имя";
        }


        private void Search()
        {
            SearchListPharaserOfEventsCategory = ListPharaserOfEventsCategory.Where(u => u.Name.ToUpper().Contains(
            textBox1.Text.ToUpper())).ToList();

            Zapolnenie(dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex].Value.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ListPharaserOfEventsCategory.Count > 1)
            {
                Search();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
                {
                    BL.AddEditPhrases(
                        dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString(),
                         dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString(),
                         dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                    MessageBox.Show("Запись сохранена");
                }
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
                {
                    if (ListPharaserOfEventsCategory.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " +
                              dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            BL.DeletePhrases(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                            Zapolnenie(dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex].Value.ToString());
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
            e.Row.Cells["Column3"].Value = Guid.NewGuid();
            e.Row.Cells["Column4"].Value = "Новое имя";
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Zapolnenie(dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex].Value.ToString());
            }
            catch { }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2[e.ColumnIndex, e.RowIndex].Value != null)
            {
                Save(dataGridView2[0, e.RowIndex].Value.ToString(),
                     dataGridView2[1, e.RowIndex].Value.ToString());
            }
        }

        private void Save(string ID, string Name)
        {
            try
            {

                if (Name != "Новое имя" || Name != "")
                {
                    BL.AddEditEventsCategory(
                         ID,
                         Name,
                         EM.SelectIDSlujba);
                    MessageBox.Show("Запись '" + Name + "' сохранена");
                    BL.GetPharaserOfEventsCategory(ListPharaserOfEventsCategory,
                        dataGridView2[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                    Zapolnenie(dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex].Value.ToString());
                }
            }
            catch
            {

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[e.ColumnIndex, e.RowIndex].Value != null)
            {
                Save2(dataGridView1[0, e.RowIndex].Value.ToString(),
                     dataGridView1[1, e.RowIndex].Value.ToString());
            }
        }

        private void Save2(string ID, string Name)
        {
            try
            {

                if (dataGridView2[1, dataGridView2.CurrentCell.RowIndex].Value.ToString() != "Новое имя" &&
                    (Name != "Новое имя" || Name != ""))
                {
                    BL.AddEditPhrases(
                        ID,
                        Name,
                        dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString());
                    MessageBox.Show("Запись '" + Name + "' сохранена");
                    Zapolnenie(dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString());
                }
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2[1, dataGridView2.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
            {
                if (dataGridView2.Rows.Count > 0)
                {

                    DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView2[1, dataGridView2.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        BL.DeleteEventsCategory(dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString());

                        GetEventsCategory();
                        if (ListEventCategory.Count != 0)
                        {
                            Zapolnenie(dataGridView2[0, dataGridView2.SelectedCells[0].RowIndex].Value.ToString());
                        }
                    }

                }
            }
        }

        private void GetEventsCategory()
        {
            BL.GetEventsCategory(ListEventCategory);
            ListEventCategory.RemoveAt(0);

            dataGridView2.Rows.Clear();
            for (int i = 0; i < ListEventCategory.Count; i++)
            {
                dataGridView2.Rows.Add(ListEventCategory[i].ID, ListEventCategory[i].Name);
            }
        } 


    }
}
