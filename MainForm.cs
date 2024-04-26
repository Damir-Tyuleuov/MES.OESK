using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DispetcherRes.Class;
using MES.OESK.Class;
using MES.OESK.ViewForm;



namespace DispetcherRes
{

    
    public partial class MainForm : Form
    {


        Image image = new Bitmap(MES.OESK.Properties.Resources.IsWarning);
        Image imageLock = new Bitmap(MES.OESK.Properties.Resources._lock);



        Microsoft.Office.Interop.Excel.Workbook ExcelWorkbook;
        Microsoft.Office.Interop.Excel.Worksheet ExcelWorksheet;
        Microsoft.Office.Interop.Excel.Range Range;

        String dir = Environment.CurrentDirectory;

        string ShiftSheduleID = null;

        Blank BL = new Blank();
        Logs Logs = new Logs();
        OperationalRecord OR = new OperationalRecord();

        List<ShiftShedule> ListShiftShedulePrint = new List<ShiftShedule>();
        List<OperationalRecord> ListSelectOperationalRecord = new List<OperationalRecord>();
        List<OperationalRecord> ListCopyOperationalRecord = new List<OperationalRecord>();
        List<OperationalRecord> ListSampleOperationalRecord = new List<OperationalRecord>();
        List<AllChanges> ListAllChanges = new List<AllChanges>();
        List<Blank> ListLocked = new List<Blank>();

        List<Blank> ListUnLocked = new List<Blank>();
        List<Blank> ListReporteds = new List<Blank>();

        DateTime DT = new DateTime();
        string ShiftID = "";
        string ShiftDt = "";

        int rowExcel;

        //int OperationalRecordNum = 0;
        public int OperationalRecordNum
        {
            set { BL.operationalRecordNum = value; }
            get
            { return BL.operationalRecordNum; }
        }

        Int32 SelectDW1 = -1;
        byte StHistory = 1;

        byte zazem1 = 0;
        byte zazem2 = 0;

        byte NumError = 1;

        public MainForm()
        {
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            InitializeComponent();

        }

        int lockedNum = 0;


        private int LockedNum
        {
            get
            { return lockedNum; }
            set
            { lockedNum = value; }
        }


        //\\\\----Переменная отвечающая за unlock.
        byte stLock = 0;
        private byte StLock
        {
            get
            { return stLock; }
            set
            { stLock = value; }
        }


        //\\\\----Функция добавления записи в оперативный журнал.
        private void добавитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddOperationRecord();
        }


        //\\\\----Отработка основных функция при запуске.
        private void MainForm_Load(object sender, EventArgs e)
        {
            // new Task( () =>
            //{
            //        this.Invoke( (MethodInvoker) (() =>
            //        {

           BL.GetServerTime();
           DT = new ShiftShedule().ServerTime;
           timer2.Enabled = true;

            dataGridView1.DataSource = OR.ListOfOperationalRecord;
                       ShiftSheduleID = new Employee().SelectShiftShedule;


                       dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 170, 33, 51);

                       dataGridView2.DataSource = OR.ListOfOperationalRecordHistory;
                       dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 170, 33, 51);

                       DGWPaint();


                       for (int i = 0; i < dataGridView1.Columns.Count; i++)
                       {

                if (i > 19)
                           {
                               dataGridView1.Columns[i].Visible = false;
                           }
                       }


                       for (int i = 0; i < dataGridView2.Columns.Count; i++)
                       {
                if (i > 19)
                           {
                               dataGridView2.Columns[i].Visible = false;
                           }
                       }

                       dataGridView1.Columns[17].DisplayIndex = 9;
                       dataGridView1.Columns[18].DisplayIndex = 10;
                       dataGridView1.Columns[19].DisplayIndex = 11;

                       dataGridView2.Columns[17].DisplayIndex = 9;
                       dataGridView2.Columns[18].DisplayIndex = 10;
                       dataGridView2.Columns[19].DisplayIndex = 11;


                       Zapolnenie();
                       BL.GetChanges(ListAllChanges);
                       OperationalRecordNum = ListAllChanges[0].OperationalRecordNum;
                       timer1.Enabled = true;
                       label3.Text = label3.Text + new Employee().SelectNameEmployee;

                       BL.GetReporteds(ListReporteds);
                       comboBox1.DataSource = new Blank().ListBOfReporteds;
                       comboBox1.ValueMember = "ID";
                       comboBox1.DisplayMember = "Name";

                       dataGridView2.Columns[0].Visible = false;

            ShiftID = "Текущая смена: " + new ShiftShedule().ListShiftShedule[0].ShiftID + "-ая (";
            ShiftDt = new ShiftShedule().ListShiftShedule[0].ShiftSheduleDate.ToShortDateString() + ") ";

            dataGridView1.ClearSelection();

        }


        //\\\\----Параметры для таблицы.
        private void DGWPaint()
        {

            this.dataGridView1.Columns[1].DefaultCellStyle.ForeColor = Color.White;


            this.dataGridView1.Columns[8].HeaderCell.Style.ForeColor = Color.Blue;

            this.dataGridView1.Columns[8].DefaultCellStyle.ForeColor = Color.Blue;

            this.dataGridView1.Columns[10].HeaderCell.Style.ForeColor = Color.Red;

            dataGridView1.Columns[10].HeaderCell.Style.Font = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);
            //dataGridView1.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dataGridView1.Columns[11].HeaderCell.Style.ForeColor = Color.Blue;
            dataGridView1.Columns[11].HeaderCell.Style.Font = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);
            //dataGridView1.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dataGridView1.Columns[12].HeaderCell.Style.ForeColor = Color.Blue;
            dataGridView1.Columns[12].HeaderCell.Style.Font = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);
            //dataGridView1.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.dataGridView1.Columns[13].HeaderCell.Style.ForeColor = Color.Red;

            this.dataGridView2.Columns[1].DefaultCellStyle.ForeColor = Color.White;

            this.dataGridView2.Columns[8].HeaderCell.Style.ForeColor = Color.Blue;

            this.dataGridView2.Columns[8].DefaultCellStyle.ForeColor = Color.Blue;

            this.dataGridView2.Columns[10].HeaderCell.Style.ForeColor = Color.Red;

            dataGridView2.Columns[10].HeaderCell.Style.Font = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);

            this.dataGridView2.Columns[11].HeaderCell.Style.ForeColor = Color.Blue;
            dataGridView2.Columns[11].HeaderCell.Style.Font = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);

            this.dataGridView2.Columns[12].HeaderCell.Style.ForeColor = Color.Blue;
            dataGridView2.Columns[12].HeaderCell.Style.Font = new System.Drawing.Font("Times New Roman", 16, FontStyle.Bold);

        }

        private void SelectRows(string ID)
        {
            try
            {
                int i = 0;
                i = OR.ListOfOperationalRecord.IndexOf(
                OR.ListOfOperationalRecord.First(x => x.ID == ID));
                dataGridView1.ClearSelection();
                dataGridView1.Rows[i].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];

                dataGridView1.FirstDisplayedScrollingRowIndex = i;
                dataGridView1.Focus();
            }
            catch
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                dataGridView1.Focus();
            }
        }

        //\\\\----Заполнение данными таблицу.
        private void Zapolnenie()
        {
            new Task(() =>
            {

                this.Invoke((MethodInvoker)(() =>
                {
                    try
                    {
                        BL.GetOperationalRecord();

                        if (OR.ListOfOperationalRecord.Count > 0)
                        {

                            Zakras();
                            LockedNum = 0;
                            label2.Text = "Кол-во записей - " + OR.ListOfOperationalRecord.Count.ToString();

                            SelectRows(new OperationalRecord().SelectedID);

                            Razmetka();

                            if (textBox1.Text != "")
                            {
                                SelectDW1 = -1;
                                dataGridView1.DataSource = OR.ListOfOperationalRecord.Where(x =>
                                x.Message.ToUpper().Contains(textBox1.Text.ToUpper())
                                || x.ObjectName.Contains(textBox1.Text.ToUpper())
                                ).ToList();
                                Zakras();
                            }


                        }
                }
                    catch
                {
                    BL.GetOperationalRecordReserv();
                        Zakras();
                    }
            }));

            }).Start();
        }


        //\\\\----Оформление
        private void Zakras()
        {
            try
            {
                //new Task(() =>
                //{
                //    this.Invoke((MethodInvoker)(() =>
                //    {

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {

                    if (OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, i].Value.ToString()).ToList()[0].StImportant == 1)
                    {

                        dataGridView1[10, i].Value = image;
                    }

                    if (OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, i].Value.ToString()).ToList()[0].StDefend == 1)
                    {
                        dataGridView1[12, i].Style.BackColor = Color.Blue;
                        dataGridView1[12, i].Style.ForeColor = Color.White;
                        dataGridView1[12, i].Value = "З";
                    }
                    else
                    {
                        dataGridView1[12, i].Style.BackColor = Color.White;
                        dataGridView1[12, i].Value = "";
                    }


                    if (OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, i].Value.ToString()).ToList()[0].StNewEquipment == 1)
                    {
                        dataGridView1[11, i].Style.BackColor = Color.Blue;
                        dataGridView1[11, i].Style.ForeColor = Color.White;
                        dataGridView1[11, i].Value = "Н";

                    }
                    else
                    {
                        dataGridView1[11, i].Style.BackColor = Color.White;
                        dataGridView1[11, i].Value = "";
                    }


                    if (dataGridView1[13, i].Value.ToString().Trim() != "" &&
                       dataGridView1[7, i].Value.ToString().Trim() != "")
                    {
                        dataGridView1[13, i].Style.ForeColor = Color.Blue;
                    }
                    else
                    {
                        dataGridView1[13, i].Style.ForeColor = Color.Red;
                    }

                }

                dataGridView1.Update();
            }
            catch
            {

            }
            //    }));
            //}).Start();

        }


        private void ZakrasHistory()
        {
            new Task(() =>
            {
                this.Invoke(new Action(() =>
                {
                    label6.Text = "Архивная смена: " + new ShiftShedule().ListShiftSheduleHistory[0].ShiftID + "-ая (" +
                    new ShiftShedule().ListShiftSheduleHistory[0].ShiftSheduleDate.ToShortDateString() + ")";
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {

                        if (OR.ListOfOperationalRecordHistory.Where(x => x.ID == dataGridView2[0, i].Value.ToString()).ToList()[0].StImportant == 1)
                        {
                            dataGridView2[10, i].Value = image;
                        }

                        if (OR.ListOfOperationalRecordHistory.Where(x => x.ID == dataGridView2[0, i].Value.ToString()).ToList()[0].StDefend == 1)
                        {
                            dataGridView2[12, i].Style.BackColor = Color.Blue;
                            dataGridView2[12, i].Style.ForeColor = Color.White;
                            dataGridView2[12, i].Value = "З";
                        }
                        else
                        {
                            dataGridView2[12, i].Style.BackColor = Color.White;
                            dataGridView2[12, i].Value = "";
                        }

                        if (OR.ListOfOperationalRecordHistory.Where(x => x.ID == dataGridView2[0, i].Value.ToString()).ToList()[0].StNewEquipment == 1)
                        {
                            dataGridView2[11, i].Style.BackColor = Color.Blue;
                            dataGridView2[11, i].Style.ForeColor = Color.White;
                            dataGridView2[11, i].Value = "Н";

                        }

                        if (dataGridView2[13, i].Value.ToString().Trim() != "" &&
                           dataGridView2[7, i].Value.ToString().Trim() != "")// && OR.ListOfOperationalRecordHistory.Where(x => x.ID.ToUpper() == dataGridView2[0, i].Value.ToString().ToUpper()).ToList()[0].StImportant != 1)
                        {
                            dataGridView2[13, i].Style.ForeColor = Color.Blue;
                            //dataGridView2[13, i].Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                        }

                        //dataGridView2[17, i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dataGridView2[18, i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dataGridView2[19, i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }));
            }).Start();

        }


        //\\\\----Отрисовка таблицы.
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {


            if (e.ColumnIndex == 17 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundRed, e.CellBounds);
                e.Handled = true;
            }

            if (e.ColumnIndex == 18 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundRedBlue, e.CellBounds);
                e.Handled = true;
            }

            if (e.ColumnIndex == 19 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundBlue, e.CellBounds);
                e.Handled = true;
            }

            if (e.ColumnIndex == 1 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources._lock, e.CellBounds);
                e.Handled = true;
            }


            var StNar = new OperationalRecord().ListOfOperationalRecord.Where(X => X.NarNum.ToString().Length > 0).ToList();
            int index = 0;

            if (StNar.Count > 0)
            {
                for (int i = 0; i < StNar.Count; i++)
                {
                    index = OR.ListOfOperationalRecord.IndexOf(OR.ListOfOperationalRecord.First(x => x.ID.ToLower() == StNar[i].ID.ToString().ToLower()));
                    if (e.RowIndex == index)
                    {
                        // dataGridView1[13, index].Style.Alignment = DataGridViewContentAlignment.MiddleLeft; //BottomCenter;
                        if (dataGridView1[13, index].Value.ToString().Trim() != "" && dataGridView1[14, index].Value.ToString() == "1")
                        {
                            if (e.ColumnIndex == 13 && e.RowIndex == index)
                            {
                                e.Paint(e.CellBounds, e.PaintParts);

                                e.Graphics.DrawLine(new Pen(Color.Blue, 3),
                                    new Point(e.CellBounds.Left, e.CellBounds.Top),
                                    new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 3),
                                    new Point(e.CellBounds.Right, e.CellBounds.Top),
                                    new Point(e.CellBounds.Left, e.CellBounds.Bottom));


                                //--\\\\\Рамка
                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                               new Point(e.CellBounds.Left, e.CellBounds.Top),
                               new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Left, e.CellBounds.Top));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                       new Point(e.CellBounds.Right, e.CellBounds.Bottom),
                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));


                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Paint(e.CellBounds, e.PaintParts);
                            if (e.ColumnIndex == 13 && dataGridView1[7, index].Value.ToString().Trim() == "" && e.RowIndex == index)
                            {

                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                               new Point(e.CellBounds.Left, e.CellBounds.Top),
                               new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Left, e.CellBounds.Top));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                       new Point(e.CellBounds.Right, e.CellBounds.Bottom),
                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));
                                
                            }
                           else  if (e.ColumnIndex == 13 && dataGridView1[7, index].Value.ToString().Trim() != "" && e.RowIndex == index)
                            {

                                e.Graphics.DrawLine(new Pen(Color.Blue, 2),
                               new Point(e.CellBounds.Left, e.CellBounds.Top),
                               new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 4),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 2),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Left, e.CellBounds.Top));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 4),
                       new Point(e.CellBounds.Right, e.CellBounds.Bottom),
                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                            }
                            e.Handled = true;
                        }

                    }
                }
            }
        }


        //\\\\----Редактирование выбранной записи.
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }


        //\\\\----Мониторинг.
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                BL.GetChanges(ListAllChanges);

                if (new Employee().SqlError == 1)
                {
                    timer1.Enabled = false;
                    if (NumError != 4)
                    {
                        System.Threading.Thread.Sleep(5000);
                        MessageBox.Show("Отсутствует связь с сервером, попытка переподключения " +
                            NumError + " из 3");
                        new Employee().SqlError = 0;
                        NumError = Convert.ToByte(NumError + 1);
                        timer1.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Отсутствует связь с сервером, попробуйте перезайти позже или свяжитесь с системным администратором");
                        this.Close();
                    }

                    return;
                }

                if (ShiftSheduleID != ListAllChanges[0].ShiftSheduleID)
                {
                    timer1.Enabled = false;
                    PrintOtchet();
                    MessageBox.Show("Данная смена закрыта");
                    this.Close();
                    return;
                }

                if (OperationalRecordNum != ListAllChanges[0].OperationalRecordNum)
                {
                    OperationalRecordNum = ListAllChanges[0].OperationalRecordNum;
                    Zapolnenie();
                }

                if (ListAllChanges[0].LockedNum != 0)
                {
                    BL.GetLocked(ListLocked);
                    Locked();
                }

                if (ListAllChanges[0].LockedNum == 0)
                {
                    UnLocked();
                }

                if (ListAllChanges[0].Mail != 0)
                {
                    связьToolStripMenuItem.Text = "Связь (" + ListAllChanges[0].Mail + ")";
                    связьToolStripMenuItem.ForeColor = Color.Red;
                }
                else
                {
                    связьToolStripMenuItem.Text = "Связь";
                    связьToolStripMenuItem.ForeColor = Color.White;
                }

                if (panel3.Visible == true)
                {
                    panel3.Visible = false;
                    label3.Visible = true;
                    panel4.Visible = true;
                }

                if (new ShiftShedule().HistoryMod == 1)
                {
                    if (StHistory == 1)
                    {
                        panel5.Visible = true;
                        dataGridView2.Visible = true;
                        вернутьсяКТекущейСменеToolStripMenuItem.Visible = true;
                        StHistory = 0;
                        ZakrasHistory();
                    }
                }

                if (BL.StLocal == 1)
                {
                    Zakras();
                    BL.StLocal = 0;
                }
            }
            catch
            {
                 return;
            }
        }


        //\\\\----Разблокирование записей.
        private void UnLocked()
        {
            try
            {
                new Task(() =>
                {
                    this.Invoke(new Action(() =>
                    {

                        if (StLock == 0)
                        {

                            for (int k = 0; k < dataGridView1.Rows.Count; k++)
                            {
                                dataGridView1[1, k].Value = null;
                            }
                            StLock = 1;
                            LockedNum = 0;
                            ListLocked.Clear();
                        }
                    }));
                }).Start();
            }
            catch { }
        }


        //\\\\----Блокировка записей.
        private void Locked()
        {
            new Task(() =>
            {
                this.Invoke(new Action(() =>
                {

                    if (ListLocked.Count != LockedNum)
                    {
                        for (int k = 0; k < dataGridView1.Rows.Count; k++)
                        {
                            dataGridView1[1, k].Value = null;
                        }

                        for (int i = 0; i < ListLocked.Count; i++)
                        {
                            dataGridView1[1, OR.ListOfOperationalRecord.IndexOf(
                            OR.ListOfOperationalRecord.First(x => x.ID == ListLocked[i].ID.ToString()))].Value = imageLock;
                        }

                        StLock = 0;
                        LockedNum = ListLocked.Count;
                    }
                }));
            }).Start();
        }


        private void AddOperationRecord()
        {
            new OperationalRecord().SelectedFunc = 0;
            AddEvent AE = new AddEvent();
            AE.ShowDialog();
        }


        private void DeleteOperationRecord()
        {
            try
            {
                if (dataGridView1.SelectedRows.Count != 0)
                {

                    DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить запись", "Удаление", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {

                        var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                        if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                        {
                            Logs.RecordLogs("Удаление записи", dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                            BL.DeleteOperationRecordOfID(dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString());
                        }

                        if (OR.ListOfOperationalRecord.Count < 1)
                        {
                            MessageBox.Show("Запись не выбрана");
                        }

                        if (LC.Count != 0)
                        {
                            MessageBox.Show("Нельзя удалить редактируемую запись");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Запись не выбрана");
                }

            }
            catch
            {

            }
        }


        private void CopyOperationRecord()
        {
            try
            {
                new Task(() =>
                {
                    this.Invoke(new Action(() =>
                    {

                        if (dataGridView1.SelectedRows.Count != 0)
                        {

                            var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                            if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                            {
                                var ListCopyOperationalRecord = OR.ListOfOperationalRecord.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();


                                ListCopyOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                                ListCopyOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;
                                ListCopyOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;


                                BL.AddOperationalRecord(ListCopyOperationalRecord);
                                Logs.RecordLogs("Копирование записи", ListCopyOperationalRecord[0].ID);
                                ListCopyOperationalRecord.Clear();
                            }

                            if (OR.ListOfOperationalRecord.Count < 1)
                            {
                                MessageBox.Show("Запись не выбрана");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Запись не выбрана");
                        }

                    }));
                }).Start();


            }
            catch
            {

            }
        }


        private void SampleOperationRecord()
        {
            try
            {
                new Task(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        ListCopyOperationalRecord = OR.ListOfOperationalRecord.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                        ListCopyOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                        BL.AddOperationalRecord(ListCopyOperationalRecord);
                        ListCopyOperationalRecord.Clear();

                    }));
                }).Start();

            }
            catch
            {

            }
        }

        private void EditOperationRecord()
        {
            try
            {
                if (dataGridView1.SelectedRows.Count != 0 && SelectDW1 != -1)
                {

                    var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                    if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                    {
                        new OperationalRecord().SelectedFunc = 1;
                        AddEvent AE = new AddEvent();
                        AE.ShowDialog();
                    }

                    if (LC.Count != 0)
                    {
                        MessageBox.Show("Запись в данный момент используется");
                    }

                    if (OR.ListOfOperationalRecord.Count < 1)
                    {
                        MessageBox.Show("Запись не выбрана");
                    }
                }
                else
                {
                    MessageBox.Show("Запись не выбрана");
                }

            }
            catch
            {
                Zapolnenie();

                BL.GetChanges(ListAllChanges);

                if (ListAllChanges[0].LockedNum != 0)
                {
                    BL.GetLocked(ListLocked);
                    Locked();
                }

                if (ListAllChanges[0].LockedNum == 0)
                {
                    UnLocked();
                }
            }
        }


        private void редактироватьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectDW1 != -1)
            {
                EditOperationRecord();
            }
            else
            {
                MessageBox.Show("Запись не выбрана");
            }
        }


        private void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteOperationRecord();
        }


        private void выходИхПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void добавитьЗаписьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddOperationRecord();
        }


        private void редактироватьЗаписьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EditOperationRecord();
        }


        private void удалитьЗаписьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteOperationRecord();
        }


        private void задачисообщеноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SprTask ST = new SprTask();
            ST.ShowDialog();
        }

        private void классЛинийлинийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SprLine SL = new SprLine();
            SL.ShowDialog();
        }

        private void шаблоныФразToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PhrasesForm PF = new PhrasesForm();
            PF.ShowDialog();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SprUsers SU = new SprUsers();
            SU.ShowDialog();
        }

        private void копироватьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyOperationRecord();
        }

        private void добавитьШаблоныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите добавить шаблоны записей в смену "
                          , "Внимание", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                BL.AddSamplesForOR();
                Logs.RecordLogs("Добавление шаблонов записей смены", "00000000-0000-0000-0000-000000000000");
            }


        }

        private void шаблоныЗаписейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SampleForm SF = new SampleForm();
            SF.ShowDialog();
        }


        private void письмаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppealForm AF = new AppealForm();
            AF.ShowDialog();
        }

        private void копироватьЗаписьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CopyOperationRecord();
        }

        private void графикСменToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShiftSheduleForm SSF = new ShiftSheduleForm();
            SSF.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                    BL.GetServerTime();
                    ListSelectOperationalRecord[0].DateBegin = new ShiftShedule().ServerTime;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;

                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }

                    //BL.operationalRecordNum = BL.operationalRecordNum + 1;
                    BL.EditOperationalRecord(ListSelectOperationalRecord);
                    //int index = new OperationalRecord().ListOfOperationalRecord.IndexOf(new OperationalRecord().ListOfOperationalRecord.First(x => x.ID.ToLower() == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString().ToLower()));
                    //new OperationalRecord().ListOfOperationalRecord[index] = ListSelectOperationalRecord[0];
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                    BL.GetServerTime();
                    if (new ShiftShedule().ServerTime < ListSelectOperationalRecord[0].DateBegin)
                    {
                        MessageBox.Show("Время отработки не может быть, меньше времени начала");
                        return;
                    }
                    ListSelectOperationalRecord[0].DateEnd = new ShiftShedule().ServerTime;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;

                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }
                    //BL.operationalRecordNum = BL.operationalRecordNum + 1;
                    BL.EditOperationalRecord(ListSelectOperationalRecord);
                    //int index = new OperationalRecord().ListOfOperationalRecord.IndexOf(new OperationalRecord().ListOfOperationalRecord.First(x => x.ID.ToLower() == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString().ToLower()));
                    //new OperationalRecord().ListOfOperationalRecord[index] = ListSelectOperationalRecord[0];
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search();
        }


        private void Search()
        {

            if (textBox1.Text != "")
            {
                SelectDW1 = -1;
                dataGridView1.DataSource = OR.ListOfOperationalRecord.Where(x =>
                x.Message.ToUpper().Contains(textBox1.Text.ToUpper())
                || x.ObjectName.Contains(textBox1.Text.ToUpper())
                ).ToList();
                Zakras();
                label2.Text = "Кол-во записей - " + dataGridView1.Rows.Count.ToString();

            }
            else
            {
                dataGridView1.DataSource = OR.ListOfOperationalRecord;
                label2.Text = "Кол-во записей - " + OR.ListOfOperationalRecord.Count.ToString();
                Zakras();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;
                    ListSelectOperationalRecord[0].Zazem1 = Convert.ToByte(ListSelectOperationalRecord[0].Zazem1 + 1);

                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }

                    BL.EditOperationalRecord(ListSelectOperationalRecord);
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    zazem1 = Convert.ToByte(ListSelectOperationalRecord[0].Zazem1);
                    zazem2 = Convert.ToByte(ListSelectOperationalRecord[0].Zazem2);
                    if (zazem1==0 && zazem2==0)
                    {
                        ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                        ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                        ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;
                        ListSelectOperationalRecord[0].Zazem3 = Convert.ToByte(ListSelectOperationalRecord[0].Zazem3 + 1);

                        if (ListSelectOperationalRecord[0].WhoID == "")
                        {
                            ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                        }
                        if (ListSelectOperationalRecord[0].WhomID == "")
                        {
                            ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                        }
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                    }
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    zazem1 = Convert.ToByte(ListSelectOperationalRecord[0].Zazem1);
                    zazem2 = Convert.ToByte(ListSelectOperationalRecord[0].Zazem2);
                    if (zazem1 > 0)
                    {
                        zazem2 = Convert.ToByte(zazem2 + 1);
                        zazem1 = Convert.ToByte(zazem1 - 1);

                        ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                        ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                        ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;
                        ListSelectOperationalRecord[0].Zazem2 = zazem2;
                        ListSelectOperationalRecord[0].Zazem1 = zazem1;

                        if (ListSelectOperationalRecord[0].WhoID == "")
                        {
                            ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                        }
                        if (ListSelectOperationalRecord[0].WhomID == "")
                        {
                            ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                        }
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                    }
                   
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;
                    ListSelectOperationalRecord[0].Zazem1 = 0;
                    ListSelectOperationalRecord[0].Zazem2 = 0;
                    ListSelectOperationalRecord[0].Zazem3 = 0;

                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }
                    BL.EditOperationalRecord(ListSelectOperationalRecord);
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;
                    ListSelectOperationalRecord[0].ReportedID = (comboBox1.SelectedItem as Blank).ID;

                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }
                    BL.EditOperationalRecord(ListSelectOperationalRecord);
                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;


                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }

                    if (ListSelectOperationalRecord[0].StImportant == 1)
                    {
                        ListSelectOperationalRecord[0].StImportant = 0;
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                        return;
                    }

                    if (ListSelectOperationalRecord[0].StImportant == 0)
                    {
                        ListSelectOperationalRecord[0].StImportant = 1;
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                        return;
                    }

                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;


                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }

                    if (ListSelectOperationalRecord[0].StDefend == 1)
                    {
                        ListSelectOperationalRecord[0].StDefend = 0;
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                        return;
                    }

                    if (ListSelectOperationalRecord[0].StDefend == 0)
                    {
                        ListSelectOperationalRecord[0].StDefend = 1;
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                        return;
                    }

                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (SelectDW1 > -1)
            {
                var LC = ListLocked.Where(U => U.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();
                if (LC.Count == 0 && OR.ListOfOperationalRecord.Count > 0)
                {
                    ListSelectOperationalRecord = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList();

                    ListSelectOperationalRecord[0].SlujbaID = new Employee().SelectIDSlujba;
                    ListSelectOperationalRecord[0].EmployeeID = new Employee().SelectIDEmployee;
                    ListSelectOperationalRecord[0].ShiftSheduleID = new Employee().SelectShiftShedule;


                    if (ListSelectOperationalRecord[0].WhoID == "")
                    {
                        ListSelectOperationalRecord[0].WhoID = "00000000-0000-0000-0000-000000000000";
                    }
                    if (ListSelectOperationalRecord[0].WhomID == "")
                    {
                        ListSelectOperationalRecord[0].WhomID = "00000000-0000-0000-0000-000000000000";
                    }

                    if (ListSelectOperationalRecord[0].StNewEquipment == 1)
                    {
                        ListSelectOperationalRecord[0].StNewEquipment = 0;
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                        return;
                    }

                    if (ListSelectOperationalRecord[0].StNewEquipment == 0)
                    {
                        ListSelectOperationalRecord[0].StNewEquipment = 1;
                        BL.EditOperationalRecord(ListSelectOperationalRecord);
                        return;
                    }

                }

                if (LC.Count != 0)
                {
                    MessageBox.Show("Запись в данный момент используется");
                }
            }
        }

        private void закрытьСменуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Внимание закрытие смены происходит в определённые временные рамки     (I смена 18:00 - 21:00; II смена 6:00 - 9:00) ", "Закрытие смены", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                BL.GetServerTime();
                if (new Employee().SelectShiftID == 1)
                {
                    if (new ShiftShedule().ServerTime.Hour > 17 && new ShiftShedule().ServerTime.Hour < 23)
                    {
                        BL.CloseShiftShedule(new Employee().SelectShiftShedule);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Первую смену можно закрыть c 18-00 по 21-00");

                    }
                    return;
                }


                if (new Employee().SelectShiftID == 2)
                {
                    if (new ShiftShedule().ServerTime.Hour > 5 && new ShiftShedule().ServerTime.Hour < 11)
                    {
                        BL.CloseShiftShedule(new Employee().SelectShiftShedule);

                    }
                    else
                    {
                        MessageBox.Show("Вторую смену можно закрыть c 6-00 по 9-00");
                    }
                    return;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Razmetka()
        {

            if (OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].StDefend == 0)
            {
                button10.BackColor = Color.Transparent;
            }
            else
            {
                button10.BackColor = Color.Yellow;
            }

            if (OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].StNewEquipment == 0)
            {
                button9.BackColor = Color.Transparent;
            }
            else
            {
                button9.BackColor = Color.Yellow;
            }


            if (OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].StImportant == 0)
            {
                button8.BackColor = Color.Transparent;
            }
            else
            {
                button8.BackColor = Color.Yellow;
            }

            comboBox1.SelectedValue = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].ReportedID;
            textBox2.Text = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].Zazem1.ToString();
            textBox3.Text = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].Zazem2.ToString();
            textBox4.Text = OR.ListOfOperationalRecord.Where(x => x.ID == dataGridView1[0, dataGridView1.CurrentCell.RowIndex].Value.ToString()).ToList()[0].Zazem3.ToString();

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logs.RecordLogs("Выход из программы", "00000000-0000-0000-0000-000000000000");
        }

        private void историяСменToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new ShiftShedule().HistoryMod == 1)
            {
                new ShiftShedule().HistoryMod = 0;
                dataGridView2.Visible = false;
                вернутьсяКТекущейСменеToolStripMenuItem.Visible = false;
                panel5.Visible = false;
                StHistory = 1;
                label6.Text = "Текущая смена: " + new ShiftShedule().ListShiftShedule[0].ShiftID + "-ая (" +
                 new ShiftShedule().ListShiftShedule[0].ShiftSheduleDate.ToShortDateString() + ")";
            }

            ShiftSheduleHistory SSH = new ShiftSheduleHistory();
            SSH.ShowDialog();
        }

        private void вернутьсяКТекущейСменеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ShiftShedule().HistoryMod = 0;
            dataGridView2.Visible = false;
            вернутьсяКТекущейСменеToolStripMenuItem.Visible = false;
            panel5.Visible = false;
            StHistory = 1;
            label6.Text = "Текущая смена: " + new ShiftShedule().ListShiftShedule[0].ShiftID + "-ая (" +
             new ShiftShedule().ListShiftShedule[0].ShiftSheduleDate.ToShortDateString() + ")";
        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.ColumnIndex == 9 && e.RowIndex == -1)
            //{
            //    e.PaintBackground(e.ClipBounds, false);
            //    e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundRedBlue, e.CellBounds);
            //    e.Handled = true;
            //}

            if (e.ColumnIndex == 17 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundRed, e.CellBounds);
                e.Handled = true;
            }

            if (e.ColumnIndex == 18 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundRedBlue, e.CellBounds);
                e.Handled = true;
            }

            if (e.ColumnIndex == 19 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources.IsGroundBlue, e.CellBounds);
                e.Handled = true;
            }


            if (e.ColumnIndex == 1 && e.RowIndex == -1)
            {
                e.PaintBackground(e.ClipBounds, false);
                e.Graphics.DrawImage(MES.OESK.Properties.Resources._lock, e.CellBounds);
                e.Handled = true;
            }
            var StNar = new OperationalRecord().ListOfOperationalRecordHistory.Where(X => X.NarNum.ToString().Length > 0).ToList();
            int index = 0;

            if (StNar.Count > 0)
            {
                for (int i = 0; i < StNar.Count; i++)
                {
                    index = OR.ListOfOperationalRecordHistory.IndexOf(OR.ListOfOperationalRecordHistory.First(x => x.ID.ToLower() == StNar[i].ID.ToString().ToLower()));
                    if (e.RowIndex == index)
                    {
                        // dataGridView1[13, index].Style.Alignment = DataGridViewContentAlignment.MiddleLeft; //BottomCenter;
                        if (dataGridView2[13, index].Value.ToString().Trim() != "" && dataGridView2[14, index].Value.ToString() == "1")
                        {
                            if (e.ColumnIndex == 13 && e.RowIndex == index)
                            {
                                e.Paint(e.CellBounds, e.PaintParts);

                                e.Graphics.DrawLine(new Pen(Color.Blue, 3),
                                    new Point(e.CellBounds.Left, e.CellBounds.Top),
                                    new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 3),
                                    new Point(e.CellBounds.Right, e.CellBounds.Top),
                                    new Point(e.CellBounds.Left, e.CellBounds.Bottom));


                                //--\\\\\Рамка
                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                               new Point(e.CellBounds.Left, e.CellBounds.Top),
                               new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Left, e.CellBounds.Top));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                       new Point(e.CellBounds.Right, e.CellBounds.Bottom),
                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));


                                e.Handled = true;
                            }
                        }
                        else
                        {
                            e.Paint(e.CellBounds, e.PaintParts);
                            if (e.ColumnIndex == 13 && dataGridView2[7, index].Value.ToString().Trim() == "" && e.RowIndex == index)
                            {

                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                               new Point(e.CellBounds.Left, e.CellBounds.Top),
                               new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Red, 2),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Left, e.CellBounds.Top));

                                e.Graphics.DrawLine(new Pen(Color.Red, 4),
                       new Point(e.CellBounds.Right, e.CellBounds.Bottom),
                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                            }
                            else if (e.ColumnIndex == 13 && dataGridView2[7, index].Value.ToString().Trim() != "" && e.RowIndex == index)
                            {

                                e.Graphics.DrawLine(new Pen(Color.Blue, 2),
                               new Point(e.CellBounds.Left, e.CellBounds.Top),
                               new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 4),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Right, e.CellBounds.Bottom));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 2),
                              new Point(e.CellBounds.Right, e.CellBounds.Top),
                              new Point(e.CellBounds.Left, e.CellBounds.Top));

                                e.Graphics.DrawLine(new Pen(Color.Blue, 4),
                       new Point(e.CellBounds.Right, e.CellBounds.Bottom),
                       new Point(e.CellBounds.Left, e.CellBounds.Bottom));

                            }
                            e.Handled = true;
                        }

                    }

                }
            }
        }

        private void объектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SprObjects SO = new SprObjects();
                SO.ShowDialog();
        }

        public void PrintOtchet()
        {
            try
            {
              

                if (new ShiftShedule().HistoryMod == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Сформировать отчёт смены", "Фомирование отчёта", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        BL.GetPrintShiftShedule(ListShiftShedulePrint, new Employee().SelectShiftShedule);
                        
               
                        //\\\\----Создание новой книги
                        Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

                        ExcelApp.Visible = false;
                        ExcelApp.DisplayAlerts = false;

                        ExcelWorkbook = ExcelApp.Workbooks.Open(dir + @"\PrintTemplate\LogBookTemplate.xls",
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        ExcelWorksheet = ExcelWorkbook.Sheets[1];

                        ExcelApp.Cells[4, "A"] = "АО 'ОЭСК' " + new Employee().SelectNameSlujba;
                        rowExcel = 6;
                        ExcelApp.Cells[rowExcel, "C"] = "C " + ListShiftShedulePrint[0].ShiftSheduleStartDate;
                        rowExcel = rowExcel + 1;
                        if (ListShiftShedulePrint[0].ShiftSheduleEndDate.ToString().Length > 0)
                        {
                            ExcelApp.Cells[rowExcel, "C"] = "По " + ListShiftShedulePrint[0].ShiftSheduleEndDate;
                        }
                        else
                        {
                            ExcelApp.Cells[rowExcel, "C"] = "";
                        }
                        rowExcel = rowExcel + 1;

                        for (int i = 0; i < ListShiftShedulePrint.Count; i++)
                        {
                            Range = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Rows[rowExcel];
                            Range.Insert();

                            ExcelApp.Cells[rowExcel, "B"] = "Диспетчер:";
                            ExcelApp.Cells[rowExcel, "C"] = ListShiftShedulePrint[i].ShiftSheduleEmployeeName;
                            rowExcel = rowExcel + 1;
                        }

                        Range.Delete();
                        rowExcel = rowExcel + 2;


                        for (int i = 0; i < OR.ListOfOperationalRecord.Count; i++)
                        {
                            Range = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Rows[rowExcel];
                            Range.Insert();

                            ExcelApp.Cells[rowExcel, "A"] = OR.ListOfOperationalRecord[i].DateBegin;

                            ExcelApp.Cells[rowExcel, "B"] = OR.ListOfOperationalRecord[i].ObjectName;
           

                            if (OR.ListOfOperationalRecord[i].Note == "")
                            {
                                ExcelApp.Cells[rowExcel, "C"] = OR.ListOfOperationalRecord[i].Message;
                            }
                            else
                            {
                                ExcelApp.Cells[rowExcel, "C"] = OR.ListOfOperationalRecord[i].Message +
                                    "\n" + "\n" + OR.ListOfOperationalRecord[i].Note;
                            }


                            if (OR.ListOfOperationalRecord[i].WhoText != "")
                            {
                                ExcelApp.Cells[rowExcel, "D"] = OR.ListOfOperationalRecord[i].WhoText;
                            }

                            if (OR.ListOfOperationalRecord[i].WhomText != "")
                            {
                                ExcelApp.Cells[rowExcel, "E"] = OR.ListOfOperationalRecord[i].WhomText;
                            }

                            if (OR.ListOfOperationalRecord[i].DateEnd != Convert.ToDateTime("1991.01.01 00:00"))
                            {
                                ExcelApp.Cells[rowExcel, "F"] = OR.ListOfOperationalRecord[i].DateEnd;
                            }
                            else
                            {
                                ExcelApp.Cells[rowExcel, "F"] = "";
                            }

                            if (OR.ListOfOperationalRecord[i].StImportant == 1)
                            {
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "G"];
                                //ExcelApp.Cells[rowExcel, "G"].HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsWarning.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                            }

                            if (OR.ListOfOperationalRecord[i].StDefend == 1)
                            {
                                ExcelApp.Cells[rowExcel, "H"].font.color = Color.Blue;
                                ExcelApp.Cells[rowExcel, "H"].font.size = 14;
                                ExcelApp.Cells[rowExcel, "H"] = "З";
                            }

                            if (OR.ListOfOperationalRecord[i].StNewEquipment == 1)
                            {
                                ExcelApp.Cells[rowExcel, "I"].font.color = Color.Blue;
                                ExcelApp.Cells[rowExcel, "I"].font.size = 14;
                                ExcelApp.Cells[rowExcel, "I"] = "Н";
                            }
                            ExcelApp.Cells[rowExcel, "J"].font.size = 14;
                            ExcelApp.Cells[rowExcel, "J"].font.color = Color.Blue;
                            ExcelApp.Cells[rowExcel, "J"] = OR.ListOfOperationalRecord[i].ReportedName;

                            if (OR.ListOfOperationalRecord[i].Zazem1 != 0)
                            {
                                ExcelApp.Cells[rowExcel, "K"].font.size = 10;
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "K"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsGroundRed.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                                ExcelApp.Cells[rowExcel, "K"] = OR.ListOfOperationalRecord[i].Zazem1;
                            }

                            if (OR.ListOfOperationalRecord[i].Zazem3 != 0)
                            {
                                ExcelApp.Cells[rowExcel, "L"].font.size = 10;
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "L"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsGroundBlue.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                                ExcelApp.Cells[rowExcel, "L"] = OR.ListOfOperationalRecord[i].Zazem3;
                            }

                            if (OR.ListOfOperationalRecord[i].Zazem2 != 0)
                            {
                                ExcelApp.Cells[rowExcel, "M"].font.size = 10;
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "M"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsGroundRedBlue.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                                ExcelApp.Cells[rowExcel, "M"] = OR.ListOfOperationalRecord[i].Zazem2;
                            }
                            ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Black;

                            if (OR.ListOfOperationalRecord[i].NarNum.ToString().Length > 0)
                            {
                                ExcelApp.Cells[rowExcel, "N"].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                ExcelApp.Cells[rowExcel, "N"].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                if (OR.ListOfOperationalRecord[i].DateEnd2=="" && OR.ListOfOperationalRecord[i].StNar==0)
                                {

                                    ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Red;
                                    ExcelApp.Cells[rowExcel, "N"].font.color = Color.Red;

                                    ExcelApp.Cells[rowExcel, "N"] = OR.ListOfOperationalRecord[i].NarUser + "\n" +
                                    OR.ListOfOperationalRecord[i].NarObject + "\n " + OR.ListOfOperationalRecord[i].NarNum;


                                }

                                if (OR.ListOfOperationalRecord[i].DateEnd2 == "" && OR.ListOfOperationalRecord[i].StNar == 1)
                                {
                                    ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Red;
                                    ExcelApp.Cells[rowExcel, "N"].font.color = Color.Red;
                                    ExcelApp.Cells[rowExcel, "N"] = OR.ListOfOperationalRecord[i].NarUser + "\n" +
                                    OR.ListOfOperationalRecord[i].NarObject + "\n " + OR.ListOfOperationalRecord[i].NarNum;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).Color = Color.Blue;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).Color = Color.Blue;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                }


                                if (OR.ListOfOperationalRecord[i].DateEnd2 != "" && OR.ListOfOperationalRecord[i].StNar == 0)
                                {
                                    ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Blue;
                                    ExcelApp.Cells[rowExcel, "N"].font.color = Color.Blue;

                                    ExcelApp.Cells[rowExcel, "N"] = OR.ListOfOperationalRecord[i].NarUser + "\n" +
                                    OR.ListOfOperationalRecord[i].NarObject + "\n " + OR.ListOfOperationalRecord[i].NarNum;
                                }

                                if (OR.ListOfOperationalRecord[i].DateEnd2 != "" && OR.ListOfOperationalRecord[i].StNar == 1)
                                {
                                    ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Red;
                                    ExcelApp.Cells[rowExcel, "N"].font.color = Color.Red;

                                    ExcelApp.Cells[rowExcel, "N"] = OR.ListOfOperationalRecord[i].NarUser + "\n" +
                                    OR.ListOfOperationalRecord[i].NarObject + "\n " + OR.ListOfOperationalRecord[i].NarNum;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).Color = Color.Blue;
                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).Color = Color.Blue;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
                                    ExcelApp.Cells[rowExcel, "N"].Borders(Microsoft.Office.Interop.Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                }

                            }
                            

                            ExcelApp.Cells[rowExcel, "O"] = OR.ListOfOperationalRecord[i].EmployeeName;

                            rowExcel++;
                        }
                        Range.Delete();

                        rowExcel++;
                        rowExcel++;
                        ExcelWorksheet.Protect("qqqq`1", true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);

                        ExcelApp.Visible = true;
                    }
                }

                if (new ShiftShedule().HistoryMod == 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Сформировать отчёт смены", "Фомирование отчёта", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        BL.GetPrintShiftShedule(ListShiftShedulePrint, new ShiftShedule().HistoryshiftShedule);

                        //\\\\----Создание новой книги
                        Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();

                        ExcelApp.Visible = false;
                        ExcelApp.DisplayAlerts = false;

                        ExcelWorkbook = ExcelApp.Workbooks.Open(dir + @"\PrintTemplate\LogBookTemplate.xls",
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        ExcelWorksheet = ExcelWorkbook.Sheets[1];

                        ExcelApp.Cells[4, "A"] = "АО 'ОЭСК' " + new Employee().SelectNameSlujba;
                        rowExcel = 6;
                        ExcelApp.Cells[rowExcel, "C"] = "C " + ListShiftShedulePrint[0].ShiftSheduleStartDate;
                        rowExcel = rowExcel + 1;
                        if (ListShiftShedulePrint[0].ShiftSheduleEndDate.ToString().Length > 0)
                        {
                            ExcelApp.Cells[rowExcel, "C"] = "По " + ListShiftShedulePrint[0].ShiftSheduleEndDate;
                        }
                        else
                        {
                            ExcelApp.Cells[rowExcel, "C"] = "";
                        }
                        rowExcel = rowExcel + 1;

                        for (int i = 0; i < ListShiftShedulePrint.Count; i++)
                        {
                            Range = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Rows[rowExcel];
                            Range.Insert();

                            ExcelApp.Cells[rowExcel, "B"] = "Диспетчер:";
                            ExcelApp.Cells[rowExcel, "C"] = ListShiftShedulePrint[i].ShiftSheduleEmployeeName;
                            rowExcel = rowExcel + 1;
                        }

                        Range.Delete();
                        rowExcel = rowExcel + 2;


                        for (int i = 0; i < OR.ListOfOperationalRecordHistory.Count; i++)
                        {
                            Range = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Rows[rowExcel];
                            Range.Insert();

                            ExcelApp.Cells[rowExcel, "A"] = OR.ListOfOperationalRecordHistory[i].DateBegin;

                            ExcelApp.Cells[rowExcel, "B"] = OR.ListOfOperationalRecordHistory[i].ObjectName;

                            if (OR.ListOfOperationalRecordHistory[i].Note == "")
                            {
                                ExcelApp.Cells[rowExcel, "C"] = OR.ListOfOperationalRecordHistory[i].Message;
                            }
                            else
                            {
                                ExcelApp.Cells[rowExcel, "C"] = OR.ListOfOperationalRecordHistory[i].Message +
                                    "\n" + "\n" + OR.ListOfOperationalRecordHistory[i].Note;
                            }


                            if (OR.ListOfOperationalRecordHistory[i].WhoText != "")
                            {
                                ExcelApp.Cells[rowExcel, "D"] = OR.ListOfOperationalRecordHistory[i].WhoText;
                            }

                            if (OR.ListOfOperationalRecordHistory[i].WhomText != "")
                            {
                                ExcelApp.Cells[rowExcel, "E"] = OR.ListOfOperationalRecordHistory[i].WhomText;
                            }

                            if (OR.ListOfOperationalRecordHistory[i].DateEnd != Convert.ToDateTime("1991.01.01 00:00"))
                            {
                                ExcelApp.Cells[rowExcel, "F"] = OR.ListOfOperationalRecordHistory[i].DateEnd;
                            }
                            else
                            {
                                ExcelApp.Cells[rowExcel, "F"] = "";
                            }

                            if (OR.ListOfOperationalRecordHistory[i].StImportant == 1)
                            {
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "G"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsWarning.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);

                            }

                            if (OR.ListOfOperationalRecordHistory[i].StDefend == 1)
                            {
                                ExcelApp.Cells[rowExcel, "H"].font.color = Color.Blue;
                                ExcelApp.Cells[rowExcel, "H"].font.size = 14;
                                ExcelApp.Cells[rowExcel, "H"] = "З";
                            }

                            if (OR.ListOfOperationalRecordHistory[i].StNewEquipment == 1)
                            {
                                ExcelApp.Cells[rowExcel, "I"].font.color = Color.Blue;
                                ExcelApp.Cells[rowExcel, "I"].font.size = 14;
                                ExcelApp.Cells[rowExcel, "I"] = "Н";
                            }
                            ExcelApp.Cells[rowExcel, "J"].font.size = 14;
                            ExcelApp.Cells[rowExcel, "J"].font.color = Color.Blue;
                            ExcelApp.Cells[rowExcel, "J"] = OR.ListOfOperationalRecordHistory[i].ReportedName;

                            if (OR.ListOfOperationalRecordHistory[i].Zazem1 != 0)
                            {
                                ExcelApp.Cells[rowExcel, "K"].font.size = 10;
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "K"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsGroundRed.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                                ExcelApp.Cells[rowExcel, "K"] = OR.ListOfOperationalRecordHistory[i].Zazem1;
                            }

                            if (OR.ListOfOperationalRecordHistory[i].Zazem3 != 0)
                            {
                                ExcelApp.Cells[rowExcel, "L"].font.size = 10;
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "L"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsGroundBlue.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                                ExcelApp.Cells[rowExcel, "L"] = OR.ListOfOperationalRecordHistory[i].Zazem3;
                            }

                            if (OR.ListOfOperationalRecordHistory[i].Zazem2 != 0)
                            {
                                ExcelApp.Cells[rowExcel, "M"].font.size = 10;
                                Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ExcelWorksheet.Cells[rowExcel, "M"];
                                float Left = (float)((double)oRange.Left) + 2;
                                float Top = (float)((double)oRange.Top) + 0;
                                const float ImageSize = 12;
                                ExcelWorksheet.Shapes.AddPicture(dir + @"\Images\IsGroundRedBlue.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                                ExcelApp.Cells[rowExcel, "M"] = OR.ListOfOperationalRecordHistory[i].Zazem2;
                            }
                            ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Black;
                            if (OR.ListOfOperationalRecordHistory[i].NarNum.ToString().Length > 0)
                            {

                                ExcelApp.Cells[rowExcel, "N"].Borders.Color = Color.Red;
                                ExcelApp.Cells[rowExcel, "N"].font.color = Color.Red;

                                ExcelApp.Cells[rowExcel, "N"] = OR.ListOfOperationalRecordHistory[i].NarUser + "\n" +
                                OR.ListOfOperationalRecordHistory[i].NarObject + "\n " + OR.ListOfOperationalRecordHistory[i].NarNum;
                            }

                            ExcelApp.Cells[rowExcel, "O"] = OR.ListOfOperationalRecordHistory[i].EmployeeName;

                            rowExcel++;
                        }
                        Range.Delete();

                        rowExcel++;
                        rowExcel++;
                        ExcelWorksheet.Protect("qqqq`1", true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);

                        ExcelApp.Visible = true;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Для печати отчета необходим Microsoft Excel. Microsoft Excel не установлен.", "Ошибка");
            }

            }
        private void отчётToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintOtchet();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (OR.ListOfOperationalRecord.Count > 0)
                {
                    EditOperationRecord();
                }
            }
            catch
            {

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          DT=DT.AddSeconds(+1);
           label6.Text = ShiftID+ShiftDt+ DT.ToLongTimeString();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Size.Width < 1400)
                {
                    dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[15].Visible = false;
                    dataGridView1.Columns[16].Visible = false;

                    dataGridView2.Columns[8].Visible = false;
                    dataGridView2.Columns[15].Visible = false;
                    dataGridView2.Columns[16].Visible = false;

                }
                if (this.Size.Width > 1450)
                {
                    dataGridView1.Columns[8].Visible = true;
                    dataGridView1.Columns[15].Visible = true;
                    dataGridView1.Columns[16].Visible = true;

                    dataGridView2.Columns[8].Visible = true;
                    dataGridView2.Columns[15].Visible = true;
                    dataGridView2.Columns[16].Visible = true;
                }
            }
            catch
            {

            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (OR.ListOfOperationalRecord.Count > 0)
                {
                    new OperationalRecord().SelectedID = dataGridView1[0, e.RowIndex].Value.ToString();
                    SelectDW1 = e.RowIndex;
                    Razmetka();
                }
            }
            catch
            {
                SelectDW1 = -1;
            }
        }
    }
}
