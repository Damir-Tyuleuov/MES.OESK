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
    public partial class AppealForm : Form
    {
        Blank BL = new Blank();
        Appeals AS = new Appeals();
        List<AllChanges> AC = new List<AllChanges>();
        int K1 = 0, x =0, y=0;
        public AppealForm()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MailForm MF = new MailForm();
            MF.ShowDialog();
        }

        private void AppealForm_Load(object sender, EventArgs e)
        {
            GetAppeals ();
            timer1.Enabled = true;
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        private void GetAppeals ()
        {
            BL.GetAppeals(new Employee().SelectIDSlujba);
            var GetAppeals = AS.AppealsList.Where(U => U.WhomsSlujbaID == new Employee().SelectIDSlujba).ToList();
            var GetAppeals2 = AS.AppealsList.Where(U => U.SlujbaID == new Employee().SelectIDSlujba).ToList();

            if (GetAppeals.Count > 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < GetAppeals.Count; i++)
                {
                    dataGridView1.Rows.Add(GetAppeals[i].ID,
                        GetAppeals[i].Datetime,
                        GetAppeals[i].SlujbaName,
                        GetAppeals[i].Text,
                        GetAppeals[i].AuthorName,
                        GetAppeals[i].StateRead
                        );
                    if (GetAppeals[i].StateRead == 0)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }

            if (GetAppeals2.Count > 0)
            {
                dataGridView2.Rows.Clear();
                for (int i = 0; i < GetAppeals2.Count; i++)
                {
                    dataGridView2.Rows.Add(GetAppeals2[i].ID,
                        GetAppeals2[i].Datetime,
                        GetAppeals2[i].WhomsSlujbaName,
                        GetAppeals2[i].Text,
                        GetAppeals2[i].AuthorName,
                        GetAppeals2[i].StateRead
                        );

                    if (GetAppeals2[i].StateRead == 0)
                    {
                        dataGridView2[6,i].Style.BackColor = Color.Red;
                        dataGridView2[6, i].Value = "Не прочтено";
                        dataGridView2[6, i].Style.ForeColor = Color.White;
                    }
                    else
                    {
                        dataGridView2[6, i].Style.BackColor = Color.Green;
                        dataGridView2[6, i].Value = "Прочтено";
                        dataGridView2[6, i].Style.ForeColor = Color.White;
                    }
                }
            }
            K1 = AS.AppealsList.Count;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BL.ValidMail(AC);
            if (AC[0].Mail != K1)
            {
                GetAppeals();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1[5, x].Value.ToString() == "0")
            {
                BL.MailSTRead(dataGridView1[0, x].Value.ToString(), new Employee().SelectIDEmployee);
                GetAppeals();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                x = dataGridView1.SelectedCells[0].RowIndex;
                y = dataGridView1.SelectedCells[0].ColumnIndex;
            }
            catch
            {

            }
        }


    }
}
