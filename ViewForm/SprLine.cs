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
    public partial class SprLine : Form
    {
        Blank BL = new Blank();
        Employee EM = new Employee();
        List<Blank> ListClassVL = new List<Blank>();
        List<Blank> ListLineOfVL = new List<Blank>();
        List<Blank> SearchListLineOfVL = new List<Blank>();
        List<Blank> ListObjects = new List<Blank>();
        List<Blank> ListOrganizations = new List<Blank>();

        public SprLine()
        {
            InitializeComponent();
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Column1"].Value = Guid.NewGuid();
            e.Row.Cells["Column2"].Value = "Новое имя";
        }

        private void Zapolnenie(List<Blank> List)
        {
            dataGridView1.Rows.Clear();
            ListLineOfVL.RemoveAt(0);
            for (int i = 0; i < List.Count; i++)
            {
                dataGridView1.Rows.Add(List[i].ID, List[i].Name);
            }
            //dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells.OfType<DataGridViewCell>().First(c => c.Visible);
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedItem != null)
            {
                ListLineOfVL.Clear();
                dataGridView1.Rows.Clear();

                BL.GetLineOfVL(ListLineOfVL, (comboBox1.SelectedItem as Blank).ID, (comboBox2.SelectedItem as Blank).ID);
                Zapolnenie(ListLineOfVL);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ListLineOfVL.Count > 1)
            {
                Search();

                if (textBox1.Text=="")
                {
                    if (comboBox2.SelectedItem != null)
                    {
                        BL.GetLineOfVL(ListLineOfVL, (comboBox1.SelectedItem as Blank).ID, (comboBox2.SelectedItem as Blank).ID);
                        Zapolnenie(ListLineOfVL);
                    }
                }
            }


        }


        private void Search ()
        {
                SearchListLineOfVL = ListLineOfVL.Where(u =>u.Name.ToUpper().Contains(
                textBox1.Text.ToUpper())).ToList();

            Zapolnenie(SearchListLineOfVL); 
        }


        private void Save(string ID, string Name)
        {
            try
            {
                if (ListObjects.Count > 0 && (Name != "Новое имя" || Name != ""))
                {
                    //MessageBox.Show((comboBox1.SelectedItem as Blank).ID + " n/ "
                    //    +ID + " n/ "
                    //    + Name + " n/ "
                    //    + (comboBox2.SelectedItem as Blank).ID)
                    //    ;
                    BL.AddEditLine(
                        (comboBox1.SelectedItem as Blank).ID,
                        ID,
                        Name,
                        (comboBox2.SelectedItem as Blank).ID);

                    MessageBox.Show("Запись '" + Name + "' сохранена");
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
                if (dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString() != "Новое имя")
                {
                    if (ListLineOfVL.Count > 0)
                    {

                        DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить " + dataGridView1[1, dataGridView1.CurrentCell.RowIndex].Value.ToString(), "Удаление", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            BL.DeleteLineOfSlujb(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());

                            BL.GetLineOfVL(ListLineOfVL, (comboBox1.SelectedItem as Blank).ID, (comboBox2.SelectedItem as Blank).ID);

                            Zapolnenie(ListLineOfVL);
                        }

                    }
                }

            }
            catch
            {
                MessageBox.Show("Выберите запись");
            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedItem != null)
            {
                BL.GetLineOfVL(ListLineOfVL, (comboBox1.SelectedItem as Blank).ID, (comboBox2.SelectedItem as Blank).ID);

                Zapolnenie(ListLineOfVL);

            }

        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListObjects.Clear();
            BL.GetObjects(ListObjects, EM.SelectIDSlujba, (comboBox3.SelectedItem as Blank).ID);
            ListObjects.RemoveAt(0);
            comboBox2.DataSource = null;
            comboBox2.DataSource = ListObjects;
            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "Name";
            if (ListObjects.Count>0)
            {
                comboBox2.SelectedIndex = 0;
            }
            if (ListObjects.Count==0)
            {
                comboBox1.Enabled = false;
                dataGridView1.Rows.Clear();
            }
            else
            {
                comboBox1.Enabled = true;
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

        private void SprLine_Load(object sender, EventArgs e)
        {
            BL.GetClassVL(ListClassVL);
            ListClassVL.RemoveAt(0);
            comboBox1.DataSource = ListClassVL;
            comboBox1.ValueMember = "ID";
            comboBox1.DisplayMember = "Name";

            BL.GetOrganizations(ListOrganizations);
            ListOrganizations.RemoveAt(0);
            comboBox3.DataSource = ListOrganizations;
            comboBox3.ValueMember = "ID";
            comboBox3.DisplayMember = "Name";
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

    }
}
