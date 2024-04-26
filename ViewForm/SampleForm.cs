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
    public partial class SampleForm : Form
    {
        Blank BL = new Blank();
        List<Blank> SampleList = new List<Blank>();

        public SampleForm()
        {
            InitializeComponent();
        }

        private void SampleForm_Load(object sender, EventArgs e)
        {
            BL.GetSamples(SampleList, new Employee().SelectIDSlujba);
            Zapolnenie();
          
        }

        private void Zapolnenie()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < SampleList.Count; i++)
            {
                dataGridView1.Rows.Add(SampleList[i].ID, SampleList[i].Name, new Employee().SelectIDSlujba);
            }
            //dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
        }


        private void Save(string ID, string Name)
        {
            try
            {

                if (Name != "Новое имя" || Name!="")
                {
                    BL.AddEditSamples(
                        ID,
                        Name);
                    MessageBox.Show("Запись '" + Name + "' сохранена");
                }
            }
            catch
            {

            }
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column1"].Value = Guid.NewGuid();
            e.Row.Cells["Column2"].Value = "Новое имя";
            e.Row.Cells["Column3"].Value = new Employee().SelectIDSlujba;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1[1, dataGridView1.SelectedCells[0].RowIndex].Value.ToString() != "Новое имя")
                {
                    if (SampleList.Count > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " +
                            dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            BL.DeleteSamples(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                            BL.GetSamples(SampleList, new Employee().SelectIDSlujba);
                            Zapolnenie();
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
