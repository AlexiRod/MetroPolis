using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Common;

//using System.ComponentModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;



namespace Metropolis
{
    public partial class StationsForm : System.Windows.Forms.Form
    {
        // параметры данных для формы

        DataSet ds;
        public StationsForm()
        {
            int li_rc;
            InitializeComponent();
            li_rc = FillData();
            if (li_rc < 0)
            {
                MessageBox.Show("Не удалось считать линии. Код ошибки " +  li_rc.ToString() );
                return;
            }

            InitializeContextMenu();
        }

        private int FillData()
        {
            int li_rc = -1;
            //    SQLiteConnection connection;
            //    SQLiteDataAdapter adapter;
            //   connection = Program.f_getConnection();
            var connection = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (connection == null) return li_rc;

            using (connection)
            {
           //     if ai_line_id < 1   
           // зачитаем в форму данные по станциям/линиям/меткам
                    string sql = "SELECT s.name, coalesce(l.name,'-') as lbl_name, " + 
                    "(select coalesce(ln.name, '-')from lines ln where ln.id = s.line_id) as line_name, " + 
                    "s.coordX, s.coordY, s.id , s.line_id, l.coordX as lblX,l.coordY as lblY " + 
                    "FROM Stations s Left JOIN Labels l on s.id = l.Station_id";  
                //      else
                //          string sql = "SELECT l.id , l.name, l.type_id, (select c.name from colors c where c.id=l.color_id) FROM Lines l";
                // Создаем объект DataAdapter 
                var adapter = prvCommon.f_GetDBAdapter(prvCommon.curDB, sql, connection);

                // Создаем объект Dataset
                if (ds == null) ds = new DataSet();
                else ds.Clear();

                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные и настраиваем имена колонок
                // эти колонки мы не отображаем
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["line_id"].Visible = false;
                dataGridView1.Columns["id"].Visible = false;
                dataGridView1.Columns["lblX"].Visible = false;
                dataGridView1.Columns["lblY"].Visible = false;
              
                // заголовки этих колонок переименовываем
                dataGridView1.Columns["id"].HeaderText = "Код станции";
                dataGridView1.Columns["name"].HeaderText = "Станция";
                dataGridView1.Columns["lbl_name"].HeaderText = "Обозн.на схеме";
                dataGridView1.Columns["line_name"].HeaderText = "Линия";
                dataGridView1.Columns["coordX"].HeaderText = "Коорд Х";
                dataGridView1.Columns["coordY"].HeaderText = "Коорд Y";
            }
            li_rc = 1;
            return li_rc;
        }

        private void StationsForm_Load(object sender, EventArgs e)
        {
            //    SQLiteConnection l_con = Program.f_getConnection(); 
            var l_con  = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con != null)
            { 
                using (l_con)
                {
                    // Создаем объект DataAdapter
                    var l_adapter = prvCommon.f_GetDBAdapter(prvCommon.curDB, "select id, coalesce(name,'-') as line_name from lines", l_con as DbConnection);
                    //var l_adapter = prvCommon.f_GetDBAdapter(prvCommon.curDB, "select s.id, (coalesce(s.name ,'-') || ' (' || (select name from lines where id=s.line_id) || ')') as st_name  from stations s, lines l where s.line_id = l.id", l_con as DbConnection);
                    // Создаем объект Dataset
                    DataSet l_ds = new DataSet();
                    // Заполняем Dataset
                    l_adapter.Fill(l_ds);
                    // Отображаем данные
                    SelectLineCombo.DataSource = l_ds.Tables[0]; 
                    SelectLineCombo.DisplayMember = "line_name";
                    SelectLineCombo.ValueMember = "id";
                }
            }
        }
        private void InitializeContextMenu()
        {
            // Create the menu item.
            ToolStripMenuItem getAction = new ToolStripMenuItem("Действия", null,
                new System.EventHandler(ShortcutMenuClick));

            // Add the menu item to the shortcut menu.
            ContextMenuStrip ActionMenu = new ContextMenuStrip();
            ActionMenu.Items.Add(getAction);

            // Set the shortcut menu for the first column.
            dataGridView1.Columns[1].ContextMenuStrip = ActionMenu;  // цепляем меню к ячейке названия линии
            dataGridView1.MouseDown += new MouseEventHandler(dataGridView1_MouseDown);
        }
        private void ShortcutMenuClick(object sender, System.EventArgs e)
        {

        }
        private DataGridViewCell clickedCell;
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // If the user right-clicks a cell, store it for use by the shortcut menu.
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = dataGridView1.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.Cell)
                {
                    clickedCell = dataGridView1.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                }
            }
        }
        private void ChangeLine_event(object sender, EventArgs e)
        {
        }

            private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
      //      MessageBox.Show("You are in the DataGridView.CellValueChanged event.");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            { 
                t_id.Text   =  dataGridView1.CurrentRow.Cells[5].Value.ToString(); 
                t_name.Text = (string) dataGridView1.CurrentRow.Cells[0].Value;
                t_X.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                t_Y.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                t_lblname.Text = (string)dataGridView1.CurrentRow.Cells[1].Value;
                // установим ComboBox. Найдем строку с назв.станции и ее индекс. Присвоим индекс ComboBox.SelectedIndex
                SelectLineCombo.DisplayMember = (string)dataGridView1.CurrentRow.Cells[2].Value ;
                int index = SelectLineCombo.FindString((string)dataGridView1.CurrentRow.Cells[2].Value);
                if (index > 0) SelectLineCombo.SelectedIndex = index;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (c_Action != 'N')
            {
                MessageBox.Show("Сохраните или отмените предыдущие изменения");
            }
            else
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    c_Action = 'D';
                    int l_id = 0;
                    bool b_Ok = false;
                    string s_erMsg = String.Empty;
                    // Проверять много чего надо будет, имхо. На удаление желат-но и подтверждающий MsgBx поставить 
                    /* 
                    l_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    if (l_id < 1) s_erMsg = "Введите id линии";
                    if (l_id > 0)   // id линии есть. проверяем станции
                    {
                        // SQLiteConnection l_con = Program.f_getConnection();
                        var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
                        if (l_con != null)
                        {
                            using (l_con)
                            {

                                var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                                //   SQLiteCommand cmd = new SQLiteCommand(l_con);
                                cmd.CommandText = "select count() from stations where line_id = @l_id";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@l_id", (object)l_id, "int"); //(int)DbType.Int32);
                                                                                                         //    cmd.Parameters.AddWithValue("@l_id", l_id);
                                object cnt = cmd.ExecuteScalar();
                                if (cnt != null && Convert.ToInt32(cnt.ToString()) > 0)
                                    s_erMsg = "Удаление невозможно. Есть станции этой линии. Количество = " + cnt.ToString() + ". Сначала удалите их";
                                else b_Ok = true;
                            }
                        }
                    }
                    */
                    if (b_Ok) dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                    else
                        if (s_erMsg.Length > 0) MessageBox.Show(s_erMsg);
                }
                else MessageBox.Show("Необходимо выбрать ряд для удаления");
            }
        }

        private char c_Action = 'N';
        private void Insert_Click(object sender, EventArgs e)
        {
            if (c_Action != 'N')
            {
                MessageBox.Show("Сохраните или отмените предыдущие изменения");
            }
            else
            {
                c_Action = 'I';             // устанавливаем режим добавления
                t_id.ReadOnly   = false;    // открываем редактирование
                t_X.ReadOnly = false; t_Y.ReadOnly = false;
                t_name.ReadOnly = false;
                SelectLineCombo.Enabled = true;
                t_lblname.ReadOnly = false;
                SelectLineCombo.Enabled = true;
                // НАЙДЕМ ДОПУСТИМЫЙ НОМЕР ЛИНИИ
                //SQLiteConnection l_con = Program.f_getConnection();
                    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB );
                    using (l_con)
                    {
                    //    SQLiteCommand cmd = new SQLiteCommand(l_con);
                        var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                        cmd.CommandText = "select Max(id) + 1 from stations";
                        object cnt = cmd.ExecuteScalar();
                        if (cnt != null)    t_id.Text = cnt.ToString();
                    }
                t_name.Text = ""; // SelectLineCombo.SelectedValue.ToString() ;
                SelectLineCombo.Text = "";
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            int     l_id,  l_coordX, l_coordY, l_line_id, l_lblX, l_lblY, status = 1;
            string  l_name, l_lblname, s_erMsg = String.Empty;
            bool    b_Ok = true;
            // проверяем данные. ошибки копим в строке s_erMsg
            l_id = Convert.ToInt32(t_id.Text);
            if (l_id < 1) s_erMsg = "Введите id станции.";
            l_name = t_name.Text;
            l_lblname = t_lblname.Text;
            if (l_name.Length == 0) s_erMsg += "Введите название линии.";
            l_line_id = Convert.ToInt32( SelectLineCombo.SelectedValue );
            if (l_line_id == 0) s_erMsg += "Задайте линию.";
            l_coordX = Convert.ToInt32(t_X.Text);
            if (l_coordX == 0) s_erMsg += "Задайте координату Х.";
            l_coordY = Convert.ToInt32(t_Y.Text);
            if (l_coordY == 0) s_erMsg += "Задайте координату Y.";
            
            if (s_erMsg.Length > 0)
            {
                MessageBox.Show(s_erMsg);
                b_Ok = false;
            }

            if (b_Ok)   // если у нас режим вставки, то проверим значения
            {
                
                // SQLiteConnection l_con = Program.f_getConnection();
                var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
                if (l_con != null)
                {
                    using (l_con)
                    {
                        try
                        {
                            // SQLiteCommand cmd = new SQLiteCommand(l_con);
                            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                            if (c_Action == 'I')
                            {
                                cmd.CommandText = "insert into stations(id,line_id,name,coordX,coordY,status,cities_id) " +
                                    "Values(@id,@line_id,@name,@coordX,@coordY,@status, (select max(cities_id) from setup))";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", l_coordX, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", l_coordY, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", l_line_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", l_name, "string");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@status", status, "int"); // пока считаем, что мы добавляем действующие станции
                                cmd.ExecuteNonQuery();
                                if (l_lblname.Length > 0)  // если ввели имя метки, то ее тоже добавим. коорд.с маленьким сдвигом
                                {
                                    cmd.CommandText = "insert into labels(station_id, name,coordX,coordY,cities_id) " +
                                            "Values(@id,@name,@coordX,@coordY, (select max(cities_id) from setup))";
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", l_coordX - 20, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", l_coordY - 20, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", l_lblname, "string");
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else if (c_Action == 'D')
                            {
                                // сначала нужно удалить метку, чтобы данные не разъехались
                                cmd.CommandText = "delete from labels where station_id=@id";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "delete from stations where id=@id";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                cmd.ExecuteNonQuery();
                            }
                            else if (c_Action == 'U')
                            {
                                cmd.CommandText = "update stations set name = @name, coordX = @coordX, coordY = @coordY, line_id = @line_id where id=@id";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", l_coordX, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", l_coordY, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", l_line_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", l_name, "string");
                                cmd.ExecuteNonQuery();
                                // теперь нужно разобраться с меткой.тут возможны варианты. поэтому определим есть ои она
                                cmd.CommandText = "select Max(name) + 1 from labels";
                                object cnt = cmd.ExecuteScalar();
                                if (cnt != null) 
                                {  // метка была. тогда меняем только имя или удаляем, если пользовател убрал имя
                                    if (l_lblname.Length > 0)  
                                    {
                                        cmd.CommandText = "update labels set name = @name where station_id=@id";
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", l_lblname, "string");
                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        cmd.CommandText = "delete from labels where station_id=@id";
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else  // метки не было
                                {
                                    if (l_lblname.Length > 0)  // если ввели имя метки, то ее тоже добавим. коорд.с маленьким сдвигом
                                    {
                                        cmd.CommandText = "insert into labels(station_id, name,coordX,coordY) " +
                                                "Values(@id,@name,@coordX,@coordY)";
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", l_coordX - 20, "int");
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", l_coordY - 20, "int");
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", l_lblname, "string");
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                            }
                            //cmd.CommandText = "COMMIT";
                            //cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(" Ошибка добавления " + ex.Message);
                        }
                        finally
                        {
                            c_Action = 'N'; // сбрасываем режим
                            t_id.ReadOnly = true;  // независимо от результата дизейблим конролы редактирования
                            t_name.ReadOnly = true;
                            t_X.ReadOnly = true; t_Y.ReadOnly = true;
                            SelectLineCombo.Enabled = false;
                            t_lblname.ReadOnly = true;
                         }
                    }

                    FillData();
                }
            }
        }

        private void Change_Click(object sender, EventArgs e)
        {
            if (c_Action != 'N')
            {
                MessageBox.Show("Сохраните или отмените предыдущие изменения");
            }
            else
            {
                c_Action = 'U';             // устанавливаем режим добавления
                t_name.ReadOnly = false;   // открываем редактирование
                SelectLineCombo.Enabled = true;
                t_X.ReadOnly = false; t_Y.ReadOnly = false;
                t_lblname.ReadOnly = false;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            c_Action = 'N'; // сбрасываем режим
            t_id.ReadOnly = true;  // независимо от результата дизейблим конролы редактирования
            t_name.ReadOnly = true;
            t_X.ReadOnly = true; t_Y.ReadOnly = true;
            SelectLineCombo.Enabled = false;
            t_lblname.ReadOnly = true;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox5_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox6_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
