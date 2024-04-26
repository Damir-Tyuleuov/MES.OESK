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
    public partial class SprTask : Form
    {
        Blank BL = new Blank();
        List<Blank> ListSprReporteds = new List<Blank>();
        List<Blank> ListReporteds = new List<Blank>();

        public SprTask()
        {
            InitializeComponent();
        }

        private void SprTask_Load(object sender, EventArgs e)
        {
            GetReporteds();

            GetReportedsInfo();

        }


        private void GetReporteds()
        {
            ListSprReporteds.Clear();
            dataGridView1.Rows.Clear();
            BL.GetSprReporteds(ListSprReporteds);

            for (int i = 0; i < ListSprReporteds.Count; i++)
            {
                dataGridView1.Rows.Add(ListSprReporteds[i].ID, ListSprReporteds[i].Name);
            }
        }

        private void GetReportedsInfo()
        {
            ListReporteds.Clear();
            dataGridView2.Rows.Clear();
            BL.GetReporteds(ListReporteds);

            for (int i = 1; i < ListReporteds.Count; i++)
            {
                dataGridView2.Rows.Add(ListReporteds[i].ID, ListReporteds[i].Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var LC = ListReporteds.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0)
                {
                    BL.AddEditReportedInfo(new Employee().SelectIDSlujba, dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                    GetReportedsInfo();
                    MessageBox.Show("Запись добавлена");
                }
                else
                {
                    MessageBox.Show("Такая запись уже добавлена");
                }
            }
            catch
            {
                MessageBox.Show("Выберите запись");
            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column1"].Value = Guid.NewGuid();
            e.Row.Cells["Column2"].Value = "Новое имя";

        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView2[1, dataGridView2.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    BL.AddEditReportedInfo(new Employee().SelectIDSlujba, dataGridView2[0, dataGridView2.CurrentCell.RowIndex].Value.ToString());
                    GetReportedsInfo();
                }
            }
            catch
            {
                MessageBox.Show("Выберите запись");
            }

        }


        private void Save (string ID, string Name)
        {
            try
            {

                if (Name != "Новое имя" || Name !="")
                {
                    BL.AddEditReported(ID, Name);
                    MessageBox.Show("Запись '" + Name + "' сохранена");
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
                Save(dataGridView1[0, e.RowIndex].Value.ToString(),
                     dataGridView1[1, e.RowIndex].Value.ToString());
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
