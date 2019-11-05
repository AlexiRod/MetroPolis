using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.VisualBasic.PowerPacks;
using Metropolis.Properties;
using System.Data;
using System.Data.Common;

namespace Metropolis
{
    public partial class SchemeEditor : Form
    {

        #region Variables


        public static List<Image> back = new List<Image>();
        public static int timeForBack = 15;
        public static Color MyColor = Color.Red; // цвета для персонализации
        public static Color DefColor = Color.Gainsboro;

        public ShapeContainer shpRoute = new ShapeContainer() { Location = new Point(0, 0) }; // Контейнер для линий
        public RoundButton rbFrom;
        public RoundButton rbTo;
        public RoundButton rb;
        public StationLabel lb;
        public Label ActualStation;

        public static bool isEditor = false; // режим редактирования
        public static bool showInfrastructure = false; // показывать ли инфраструктуру

        public static int CountOfStations = 0;
        public int zoomFactor = 1;
        public int ActualSize = 0;

        public static string timeLang;
        public static string Lang = "Ru";

        public static Dictionary<LineShape, Dictionary<string, RoundButton>> lineDict = new Dictionary<LineShape, Dictionary<string, RoundButton>>();
        // Для Линии есть словарь пар хранящаих ключ-значение: Имя (начало или конец) - Кнопка к которой привязано начало или конец


        bool map = false;  // флаг, показывающий, надо ли перемещать карту как в старом варианте
        bool wheel = false;   //  флаг, показывающий, надо ли увеличивать карту как в старом варианте
        bool lb_MapCreated = false;

        int minZoom = 0;
        int maxZoom = 3;


        Button pointer_st;
        Rectangle screenRectangle; // = RectangleToScreen(this.ClientRectangle);
        RoundButton MovingButton, st_from, st_to;
        Point startPoint = new Point(30, 150);
        Point cpoint_MouseDown;             // мышка нажата
        Label ShowConnDesc = new Label();   // показываем описание коннектора
        ContextMenuStrip ActionMenu;

        int X, Y;
        int X_center = 0, Y_center = 0;     // точка, где начали крутить мышиное колесико. От нее раздвигаем/сдвигаем 
        int X_move = 0, Y_move = 0;         // координаты сдвига, если мы перетаскиваем карту
        int ShftX = 0, ShftY = 0;           // храним сдвиг карты, чтобы корректировать координаты при сохранении
        int PtX, PtY;   // когда передвигаем станцию или метку запомним ее стартовую позицию, чтобы посчитать сдвиг
        int DeltaLblX, DeltaLblY;   // разница координат станции и ее метки.Исп.при перетаскивании станции
        int BrdWdth = 4;                    // толщина линии, соединяющая станции
        int startPx, startPy;              // Начальные координаты панели

        double kTotal = 1; // хранит итоговый коэфф.умножения
        Single LFontSize = 9.75F;
        char ConnSt = 'N';

        List<Label> stationLabels = new List<Label>();
        List<RoundButton> stationButtons = new List<RoundButton>();

        #endregion




        #region Load

        public SchemeEditor() // Конструктор 
        {
            InitializeComponent();

            // тут можно поподменять сообщения по ресурсам
            string tmpStr;
            if (DbTables.f_GetLangResValue("SchemeEditor", "FormText", Program.Lang, 999999, out tmpStr) > 0)
                if (tmpStr.Length > 0) this.Text = tmpStr;

        }


        private void SchemeEditor_Load(object sender, EventArgs e) // Загрузка 
        {
            zoomFactor = 1;
            button3.Left = 1800;
            button3.Top = 1500;
            ActualStation = label4;
            startPx = pRoute.Left;
            startPy = pRoute.Top;


            //Добавление лейблов для отображения подробностей маршрута. Чтобы не ломался формат, добавим так
            stationLabels.Add(label4);
            stationLabels.Add(label5);
            stationLabels.Add(label6);
            stationLabels.Add(label7);
            stationLabels.Add(label8);
            stationLabels.Add(label9);
            stationLabels.Add(label10);
            stationLabels.Add(label11);
            stationLabels.Add(label12);
            stationLabels.Add(label13);
            stationLabels.Add(label14);
            stationLabels.Add(label15);
            stationLabels.Add(label16);
            stationLabels.Add(label17);
            stationLabels.Add(label18);
            stationLabels.Add(label19);
            stationLabels.Add(label20);
            stationLabels.Add(label21);
            stationLabels.Add(label22);
            stationLabels.Add(label23);
            stationLabels.Add(label24);
            stationLabels.Add(label25);
            stationLabels.Add(label26);
            stationLabels.Add(label27);
            stationLabels.Add(label28);
            stationLabels.Add(label29);
            stationLabels.Add(label30);
            stationLabels.Add(label31);
            stationLabels.Add(label32);
            stationLabels.Add(label33);
            stationLabels.Add(label34);
            stationLabels.Add(label35);
            stationLabels.Add(label36);
            stationLabels.Add(label37);
            stationLabels.Add(label38);
            stationLabels.Add(label39);
            stationLabels.Add(label40);
            stationLabels.Add(label41);
            stationLabels.Add(label42);
            stationLabels.Add(label43);
            stationLabels.Add(label44);
            stationLabels.Add(label45);
            stationLabels.Add(label46);
            stationLabels.Add(label47);
            stationLabels.Add(label48);
            stationLabels.Add(label49);
            stationLabels.Add(label50);
            stationLabels.Add(label51);
            stationLabels.Add(label52);
            stationLabels.Add(label53);
            stationLabels.Add(label54);
            stationLabels.Add(label55);
            stationLabels.Add(label56);
            stationLabels.Add(label57);
            stationLabels.Add(label58);
            stationLabels.Add(label59);
            stationLabels.Add(label60);
            
            Controls.Add(shpRoute);


            // Добавление фонов
            back.Add(Resources.back1);
            back.Add(Resources.back2);
            back.Add(Resources.back3);
            back.Add(Resources.back4);
            back.Add(Resources.back5);
            back.Add(Resources.back6);
            back.Add(Resources.back7);
            back.Add(Resources.back8);
            back.Add(Resources.back9);
            back.Add(Resources.back10);
            back.Add(Resources.back11);
            back.Add(Resources.back12);
            back.Add(Resources.back13);
            back.Add(Resources.back14);
            back.Add(Resources.back15);
            back.Add(Resources.back16);
            back.Add(Resources.back17);
            back.Add(Resources.back18);
            back.Add(Resources.back19);
            back.Add(Resources.back20);


            Languagesetup();
            f_RecreateStations(); // стартовая отрисовка кнопок
            AddZoomIfItIsnt(); // Проверка для каждой станции, есть ли такие координаты в базе
            f_DrawAllConnectors();  // стартовая отрисовка соединителей
            Zoom();

            menuStrip1.Focus();


            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con != null)
            {
                using (l_con)
                {
                    // Создаем объект DataAdapter
                    var l_adapter = prvCommon.f_GetDBAdapter(prvCommon.curDB, "select s.id, (coalesce(s.name ,'-') || ' (' || (select name from lines where id=s.line_id) || ')') as st_name  from stations s, lines l where s.line_id = l.id", l_con as DbConnection);
                    // Создаем объект Dataset
                    DataSet l_ds = new DataSet();
                    DataSet l_ds1 = new DataSet();
                    // Заполняем Dataset
                    l_adapter.Fill(l_ds);
                    l_adapter.Fill(l_ds1);
                    // Отображаем данные
                    cbFrom.DataSource = l_ds.Tables[0];
                    cbFrom.DisplayMember = "st_name";
                    cbFrom.ValueMember = "id";

                    cbTo.DataSource = l_ds1.Tables[0];
                    cbTo.DisplayMember = "st_name";
                    cbTo.ValueMember = "id";

                    AutoCompleteStringCollection acomp = new AutoCompleteStringCollection();
                    for (int i = 0; i < l_ds1.Tables[0].Rows.Count; i++)
                        acomp.Add(l_ds1.Tables[0].Rows[i][1].ToString());

                    AutoCompleteStringCollection acomp1 = new AutoCompleteStringCollection();
                    for (int i = 0; i < l_ds1.Tables[0].Rows.Count; i++)
                        acomp1.Add(l_ds1.Tables[0].Rows[i][1].ToString());

                    cbFrom.AutoCompleteCustomSource = acomp;
                    cbTo.AutoCompleteCustomSource = acomp1;
                }
            }
            cbFrom.Text = "";
            cbTo.Text = "";
        }


        public void Languagesetup() // Чтение и установка языка из базы 
        {
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null) { MessageBox.Show("Read станций default невозможно. Ошибка установления соединения"); return; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select lang from Setup";
                    cmd.ExecuteNonQuery();
                    System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())
                        {
                            Lang = reader.GetString(0);
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка set default: " + ex.Message); }


            if (Lang == "En")
            {
                языкToolStripMenuItem.Text = "Language";
                помощьToolStripMenuItem.Text = "Help";
                настройкиToolStripMenuItem.Text = "Settings";
                режимыToolStripMenuItem.Text = "Modes";
                историческоеПредставлениеToolStripMenuItem.Text = "Historical reference";
                викторинаToolStripMenuItem.Text = "Quiz";
                lblMarshroute.Text = "Route";
                lblFrom.Text = "From";
                lblTo.Text = "To";
                lblTimeRoute.Text = "Travel time: ";
                lblStationsRoute.Text = "Stations on the way:";
                btnPath.Text = "Make a path";

            }
            else
            {
                языкToolStripMenuItem.Text = "Язык";
                помощьToolStripMenuItem.Text = "Помощь";
                настройкиToolStripMenuItem.Text = "Настройки";
                режимыToolStripMenuItem.Text = "Режимы";
                историческоеПредставлениеToolStripMenuItem.Text = "Историческое представление";
                викторинаToolStripMenuItem.Text = "Образование";
                lblMarshroute.Text = "Маршрут";
                lblFrom.Text = "Откуда";
                lblTo.Text = "Куда";
                lblTimeRoute.Text = "Время в пути: ";
                lblStationsRoute.Text = "Станции в пути:";
                btnPath.Text = "Построить";
            }

            // создадим пункт меню
            ToolStripMenuItem getAction = new ToolStripMenuItem(Lang == "Ru" ? "Информация" : "Information", null,
                    new System.EventHandler(OnShowStInfo));
            ToolStripMenuItem getAction1 = new ToolStripMenuItem(Lang == "Ru" ? "Историческая справка" : "Historical Information", null,
                    new System.EventHandler(OnShowStInfo2));

            ToolStripMenuItem getAction2 = new ToolStripMenuItem(Lang == "Ru" ? "Oтсюда" : "From", null,
                    new System.EventHandler(OnShowFrom));
            ToolStripMenuItem getAction3 = new ToolStripMenuItem(Lang == "Ru" ? "Cюда" : "To", null,
                    new System.EventHandler(OnShowTo));

            ToolStripMenuItem getAction4 = new ToolStripMenuItem(Lang == "Ru" ? "Дополнительно" : "More", null,
                    new System.EventHandler(OnShowStInfo1));

            ActionMenu = new ContextMenuStrip();   // Это объект нашего меню. Далее мы к нему добавим созданные ранее пункты меню 
            ActionMenu.Items.AddRange(new[] { getAction, getAction1, getAction2, getAction3 });

            timeLang = lblTimeRoute.Text;
        }


        public string getNameOfLine(int lineId)
        {
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            string ret = "";

            if (l_con == null)
            {
                MessageBox.Show("Чтение информации о станиции невозможно. Ошибка установления соединения"); return "";
            }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select name from Lines where id = @id";
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", lineId, "int");

                    System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                    if (reader.HasRows) // если есть данные
                    {
                        // Разбиваем текст по трем разделам
                        string[] text = new string[3];
                        while (reader.Read())
                        {
                            ret += reader.GetString(0);
                        }
                    }

                    
                    reader.Close();
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка при чтении информации о станиции. " + ex.Message); }

            return " (" + ret + ")";
        }

        public void OnShowFrom(object sender, System.EventArgs e) // Откуда 
        {
            var source = ActionMenu.SourceControl as RoundButton;

            if (source != null)
            {
                rbFrom = source as RoundButton;

                cbFrom.Text = rbFrom.st_name + getNameOfLine(rbFrom.st_line_id); 
            }
        }

        public void OnShowTo(object sender, System.EventArgs e) // Куда 
        {
            var source = ActionMenu.SourceControl as RoundButton;

            if (source != null)
            {
                rbTo = source as RoundButton;

                cbTo.Text = rbTo.st_name + getNameOfLine(rbTo.st_line_id);
            }
        }

        private void OnShowStInfo(object sender, System.EventArgs e) // Историческая справка (работает почти так же как информация) 
        {       // создадим станцию и заполним ее данные. Затем вызовем форму
            if (sender is ToolStripMenuItem)
            {
                int h = this.HorizontalScroll.Value; // Запоминаем положение ползунков
                int v = this.VerticalScroll.Value;

                RoundButton tmpButton;
                var menu = (ToolStripDropDownItem)sender;  // достучимся до исходного контрола,
                var strip = (ContextMenuStrip)menu.Owner;  //  из которого вызвали меню strip.SourceControl
                if (strip.SourceControl is RoundButton)
                {
                    tmpButton = (RoundButton)strip.SourceControl;
                    Station StInfo = new Station(tmpButton, tmpButton.st_name);

                    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

                    if (l_con == null)
                    {
                        MessageBox.Show("Чтение информации о станиции невозможно. Ошибка установления соединения"); return;
                    }
                    try
                    {
                        using (l_con)
                        {
                            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                            cmd.CommandText = "select text from InfoTable where st_id = @id and lang = '" + Lang + "'";
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");

                            System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                            if (reader.HasRows) // если есть данные
                            {
                                string[] text = new string[3];
                                while (reader.Read())
                                {
                                    text = reader.GetString(0).Split('~');
                                }

                                StInfo.Info = text[0];
                                StInfo.History = text[1];
                                StInfo.Route = text[2];

                            }
                            reader.Close();


                            for (int i = 1; i <= 5; i++)
                            {
                                int li_rc = 0;
                                Image img = null;

                                li_rc = Program.f_ReadImgFile(".\\Фото\\St" + tmpButton.UniqueId + "_" + i + ".jpg", ref img);
                                if (li_rc != 1)
                                {
                                    li_rc = Program.f_ReadImgFile(".\\Фото\\St" + tmpButton.UniqueId + "_" + i + ".jpeg", ref img);
                                    if (li_rc != 1)
                                        li_rc = Program.f_ReadImgFile(".\\Фото\\St" + tmpButton.UniqueId + "_" + i + ".png", ref img);
                                }

                                if (img != null)
                                    StInfo.Images.Add(img);
                            }


                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show("Ошибка при чтении информации о станиции. " + ex.Message); }


                    InformationForm informationForm = new InformationForm(StInfo);
                    informationForm.ShowDialog();

                    // Устанавливаем ползунки обратно (лучше дважды, так как не всегда работает один раз)
                    this.HorizontalScroll.Value = h;
                    this.HorizontalScroll.Value = h;
                    this.VerticalScroll.Value = v;
                    this.VerticalScroll.Value = v;


                }
            }
        }

        private void OnShowStInfo1(object sender, System.EventArgs e) // Получение актуальной для режима разработчика информации 
        {
            if (sender is ToolStripMenuItem && isEditor)
            {
                int h = this.HorizontalScroll.Value;
                int v = this.VerticalScroll.Value;
                RoundButton tmpButton;
                var menu = (ToolStripDropDownItem)sender;  // достучимся до исходного контрола,
                var strip = (ContextMenuStrip)menu.Owner;  //  из которого вызвали меню strip.SourceControl
                if (strip.SourceControl is RoundButton)
                {
                    tmpButton = (RoundButton)strip.SourceControl;

                    MessageBox.Show("Coordinates: " + tmpButton.coordX + "; " + tmpButton.coordY + "\nLabel: " + tmpButton.LblX + " - " + tmpButton.LblY + ";\n\nId: " + tmpButton.st_id);
                }

                this.HorizontalScroll.Value = h;
                this.HorizontalScroll.Value = h;
                this.VerticalScroll.Value = v;
                this.VerticalScroll.Value = v;
            }

        }

        private void OnShowStInfo2(object sender, System.EventArgs e) // Историческая информация о станции 
        {
            if (sender is ToolStripMenuItem)
            {
                int h = this.HorizontalScroll.Value; // Запоминаем положение ползунков
                int v = this.VerticalScroll.Value;

                RoundButton tmpButton;
                var menu = (ToolStripDropDownItem)sender;  // достучимся до исходного контрола,
                var strip = (ContextMenuStrip)menu.Owner;  //  из которого вызвали меню strip.SourceControl
                if (strip.SourceControl is RoundButton)
                {
                    tmpButton = (RoundButton)strip.SourceControl;
                    Station StInfo = new Station(tmpButton, tmpButton.st_name);

                    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

                    if (l_con == null)
                    {
                        MessageBox.Show("Чтение информации о станиции невозможно. Ошибка установления соединения"); return;
                    }
                    try
                    {
                        using (l_con)
                        {
                            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                            cmd.CommandText = "select text from InfoTable where st_id = @id and lang = '" + Lang + "'";
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");

                            System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                            if (reader.HasRows) // если есть данные
                            {
                                // Разбиваем текст по трем разделам
                                string[] text = new string[3];
                                while (reader.Read())
                                {
                                    text = reader.GetString(0).Split('~');
                                }

                                StInfo.Info = text[0];
                                StInfo.History = text[1];
                                StInfo.Route = text[2];

                            }


                            for (int i = 1; i <= 5; i++)
                            {
                                int li_rc = 0;
                                Image img = null;

                                li_rc = Program.f_ReadImgFile(".\\Фото\\St" + tmpButton.UniqueId + "_" + i + ".jpg", ref img);
                                if (li_rc != 1)
                                {
                                    li_rc = Program.f_ReadImgFile(".\\Фото\\St" + tmpButton.UniqueId + "_" + i + ".jpeg", ref img);
                                    if (li_rc != 1)
                                        li_rc = Program.f_ReadImgFile(".\\Фото\\St" + tmpButton.UniqueId + "_" + i + ".png", ref img);
                                }

                                if (img != null)
                                    StInfo.Images.Add(img);
                            }

                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show("Ошибка при чтении информации о станиции. " + ex.Message); }


                    HistoryForm historyForm = new HistoryForm(StInfo);
                    historyForm.ShowDialog();

                    // Устанавливаем ползунки обратно (лучше дважды, так как не всегда работает один раз)
                    this.HorizontalScroll.Value = h;
                    this.HorizontalScroll.Value = h;
                    this.VerticalScroll.Value = v;
                    this.VerticalScroll.Value = v;

                }
            }
        }


        private void OnSizeChanged(object sender, EventArgs e) // При изменении размеров формы: 
        {
            screenRectangle = RectangleToScreen(this.ClientRectangle);
            if ((this.WindowState == FormWindowState.Normal) || (this.WindowState == FormWindowState.Maximized)) { }
        }

        #endregion




        #region Zoom

        public void AddZoomIfItIsnt() // Если станции нет в таблице координат, то добавим ее 
        {
            int iKey;
            RoundButton tmpButton;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null) { MessageBox.Show("Set станций default невозможно. Ошибка установления соединения"); return; }
            try
            {
                using (l_con)
                {
                    foreach (var rb in Controls.OfType<RoundButton>())
                    {
                        iKey = Convert.ToInt32(rb.Name.Substring(2)); // получаем ключ. Имя кнопки = "St" + id, поэтому просто обрезаем "St"
                        if (iKey > 0)
                        {
                            CountOfStations++;
                            Program.StationDict.TryGetValue(iKey, out tmpButton);
                            if (tmpButton != null)
                            {
                                for (int i = minZoom; i <= maxZoom; i++)
                                {
                                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                                    cmd.CommandText = "select count() from ZoomCoordinates where id=@id AND zoom = @zoom";   // проверяем, была ли такая станция
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@zoom", i, "int");
                                    cmd.ExecuteNonQuery();
                                    object cnt = cmd.ExecuteScalar();


                                    if (cnt != null && Convert.ToInt32(cnt.ToString()) == 0)   // если такой станции еще не было
                                    {
                                        cmd.CommandText = "INSERT INTO ZoomCoordinates (id, zoom, name) VALUES(@id, @zoom, (select name from Stations where id = @id))";
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
                                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@zoom", i, "int");
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка set default: " + ex.Message); }
        }


        public void Zoom() // Масштабирование 
        {
            // очистка окна маршрута:
            foreach (var item in stationLabels)
                item.Visible = false;

            foreach (var item in stationButtons)
                item.Visible = false;

            shpRoute.Shapes.Clear();

            cbFrom.SelectedItem = null;
            cbTo.SelectedItem = null;
            
            label4.Location = startPoint;
            ActualStation = label4;


            // Масштабирование:
            int iKey;
            string lblText = "";
            RoundButton tmpButton;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null) { MessageBox.Show("Увеличение невозможно. Ошибка установления соединения"); return; }
            try
            {
                using (l_con)
                {
                    this.HorizontalScroll.Value = 0;
                    this.HorizontalScroll.Value = 0;
                    this.VerticalScroll.Value = 0;
                    this.VerticalScroll.Value = 0;

                    int size = 30, radius = 20;
                    float font = 10;

                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "SELECT size, radius, font from Zoom where zoom = @zoom";
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@zoom", zoomFactor, "int");
                    System.Data.Common.DbDataReader reader1 = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                    if (reader1.HasRows) // если есть данные
                    {
                        while (reader1.Read()) // построчно считываем данные 
                        {
                            size = reader1.GetInt32(0);
                            radius = reader1.GetInt32(1);
                            font = reader1.GetFloat(2);
                        }
                    }
                    reader1.Close();

                    ActualSize = size;

                    foreach (var rb in Controls.OfType<RoundButton>())
                    {
                        iKey = Convert.ToInt32(rb.Name.Substring(2)); // получаем ключ. Имя кнопки = "St" + id, поэтому просто обрезаем "St"
                        if (iKey > 0)
                        {
                            Program.StationDict.TryGetValue(iKey, out tmpButton);
                            if (tmpButton != null)
                            {

                                int x = 1, y = 1;
                                int lblx = 1, lbly = 1;

                                // выберем нужные при данном ZoomFactor-е координаты из базы
                                cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                                cmd.CommandText = "SELECT coordX, coordY, lblX, lblY from ZoomCoordinates where id = @id AND zoom = @zoom";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@zoom", zoomFactor, "int");
                                System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);

                                if (reader.HasRows) // если есть данные
                                {
                                    while (reader.Read())
                                    {
                                        x = reader.GetInt32(0);
                                        y = reader.GetInt32(1);
                                        lblx = reader.GetInt32(2);
                                        lbly = reader.GetInt32(3);
                                    }
                                }
                                reader.Close();



                                // установим размер кнопки, ее радиус, шрифт
                                tmpButton.Size = new Size(size, size);
                                tmpButton.ButtonRoundRadius = radius;

                                // установим координаты
                                tmpButton.coordX = x;
                                tmpButton.coordY = y;
                                tmpButton.Location = new Point(tmpButton.coordX, tmpButton.coordY);

                                // запишем актуальные координаты в таблицу Stations
                                cmd.CommandText = "update Stations set  coordX = @coordX, coordY = @coordY where id=@id";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", tmpButton.coordX, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", tmpButton.coordY, "int");
                                cmd.ExecuteNonQuery();

                                if (tmpButton.LbBtCtl != null)
                                {
                                    tmpButton.LblX = lblx;
                                    tmpButton.LblY = lbly;
                                    tmpButton.LbBtCtl.Location = new Point(tmpButton.LblX, tmpButton.LblY);
                                    tmpButton.LbBtCtl.Font = new Font(tmpButton.LbBtCtl.Font.FontFamily, font, tmpButton.LbBtCtl.Font.Style);

                                    lblText = tmpButton.LbBtCtl.Text; if (lblText == null) lblText = "";
                                    cmd.CommandText = "update labels set coordX = @coordX, coordY = @coordY where station_id=@id";
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", tmpButton.LblX, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", tmpButton.LblY, "int");
                                    cmd.ExecuteNonQuery();
                                }
                                else MessageBox.Show("There is NO label.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка увеличения " + ex.Message); }
            MoveLines(); // Передвинем коннекторы

        }

        public void MoveLines() // Передвижение коннектороы между станциями 
        {
            int iKey;
            RoundButton tmpButton;

            foreach (var rb in Controls.OfType<RoundButton>())
            {
                iKey = Convert.ToInt32(rb.Name.Substring(2)); // получаем ключ. Имя кнопки = "St" + id, поэтому просто обрезаем "St"
                if (iKey > 0)
                {
                    Program.StationDict.TryGetValue(iKey, out tmpButton);
                    if (tmpButton != null)
                    {
                        foreach (var line in tmpButton.ConLine) // из листа коннекторов выберем сущетсвующие
                        {
                            if (line != null) // из словаря возьмем нужную кнопку, к которой привязана данная линия и установим ее координаты для линии
                            {
                                line.X1 = lineDict[line]["Start"].coordX + ActualSize / 2;
                                line.Y1 = lineDict[line]["Start"].coordY + ActualSize / 2;


                                line.X2 = lineDict[line]["End"].coordX + ActualSize / 2;
                                line.Y2 = lineDict[line]["End"].coordY + ActualSize / 2;
                            }
                        }
                    }
                }
            }
        }


        #endregion




        #region Drawing

        private void f_DrawAllConnectors()  // рисует коннекторы между станциями. Устанавливает ссылки на коннекторы в RoundButton
        {
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null)
            {
                MessageBox.Show("Сохранение коннектора невозможно. Ошибка установления соединения"); return;
            }
            try
            {
                using (l_con)
                {
                    // Берет из базы нужные коннекторы
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select st_id_from, st_id_to, line_id, line2, name, coalesce(Description,\"\") " +
                        "from ConnectStations where show ='1'";   // считаем отображаемые коннекторы
                    System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                    if (reader.HasRows)
                    {
                        int id_from, id_to, line_from, line_to;
                        string lnClr, desc;
                        while (reader.Read())
                        {
                            id_from = reader.GetInt32(0);
                            id_to = reader.GetInt32(1);
                            line_from = reader.GetInt32(2);
                            line_to = reader.GetInt32(3);
                            lnClr = reader.GetString(4);  // цвет линии from
                            desc = reader.GetString(5);  // описание


                            RoundButton CStationFr = null, CStationTo = null;
                            // найдем начальную и конечную станции для данного коннектора. Отрисуем его и поместим ссылки в класс RoundButton
                            Program.StationDict.TryGetValue(line_from * 10000 + id_from, out CStationFr);
                            Program.StationDict.TryGetValue(line_to * 10000 + id_to, out CStationTo);
                            if (CStationFr != null && CStationTo != null)
                            {
                                Color LnClr = Program.f_DefColor(lnClr);
                                int HBt = (int)(CStationFr.Height / 2);  // половина высоты кнопки для коррекции положения линии
                                f_DrawConnector(CStationFr, CStationTo, LnClr, desc, HBt, BrdWdth, true, shapeContainer2);
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(" Ошибка отрисовки соединителей. " + ex.Message); }
        }

        private void f_DrawConnector(RoundButton CStationFr, RoundButton CStationTo, Color lnClr, string desc, int HBt, int BorderWidth, bool bAddConn, ShapeContainer ShpCnt) // рисует коннектор от одной станции к другой
        {
            LineShape lineShapeConn;

            lineShapeConn = new LineShape(CStationFr.coordX + HBt, CStationFr.coordY + HBt, CStationTo.coordX + HBt, CStationTo.coordY + HBt); // HBt - половина высоты кнопки для рисования

            // Зададим свойства и события
            lineShapeConn.BorderColor = lnClr;
            lineShapeConn.Name = desc;
            lineShapeConn.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
            lineShapeConn.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);

            if (lineShapeConn.BorderWidth != BorderWidth) lineShapeConn.BorderWidth = BorderWidth;
            lineShapeConn.Parent = ShpCnt;
            if (bAddConn)
            {
                // Добавление кнопки в словарь в нужный раздел
                lineDict[lineShapeConn] = new Dictionary<string, RoundButton>();
                lineDict[lineShapeConn]["Start"] = CStationFr;
                lineDict[lineShapeConn]["End"] = CStationTo;


                f_AddLineToSt(lineShapeConn, CStationFr);  // добавляем инфу о соединителе к нужным объектам
                f_AddLineToSt(lineShapeConn, CStationTo);  // RoundButton, чтобы они двигались при перемещении кнопки
            }
            if (kTotal != 1 || ShftX != 0 || ShftY != 0)
            {
                lineShapeConn.X1 = CStationFr.ScurrX + HBt; lineShapeConn.Y1 = CStationFr.ScurrY + HBt;
                lineShapeConn.X2 = CStationTo.ScurrX + HBt; lineShapeConn.Y2 = CStationTo.ScurrY + HBt;
            }


            ShpCnt.Shapes.Add(lineShapeConn); // Добавление в контейнер
        }

        private void f_RecreateStations()  // перерисовывает кнопки станций 
        {
            int cX = 30, cY = 25;
            string tmp_StationText, tmp_ButtonName;
            Color tmp_Color = Color.AliceBlue;

            Program.LineBase CurLine = new Program.LineBase();
            foreach (KeyValuePair<int, RoundButton> kvp in Program.StationDict)
            {
                RoundButton CurrStation = kvp.Value;
                if (CurrStation != null)
                {
                    if (Lang == "En")
                    {
                        var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
                        string engName = "";

                        if (l_con == null) { MessageBox.Show("Чтение станций (Eng) невозможно. Ошибка установления соединения"); return; }
                        try
                        {
                            using (l_con)
                            {
                                // Считываем английские названия станций и устанавливаем их
                                var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);

                                cmd.CommandText = "select value from LangRes where id = @id";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", CurrStation.st_id, "int");
                                cmd.ExecuteNonQuery();

                                System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                if (reader.HasRows) // если есть данные
                                {

                                    while (reader.Read())
                                    {
                                        engName = reader.GetString(0);
                                    }
                                }
                                reader.Close();
                            }
                            CurrStation.st_name = engName;
                        }
                        catch (Exception ex)
                        { MessageBox.Show(" Ошибка чтения " + ex.Message); }
                    }

                    // Установим нужное станции имя
                    tmp_StationText = CurrStation.st_name;
                    tmp_ButtonName = "St" + Convert.ToString(CurrStation.st_line_id * 10000 + CurrStation.st_id);
                    Program.LineDict.TryGetValue(CurrStation.st_line_id, out CurLine);
                    tmp_Color = CurLine.line_color;
                    if (CurrStation.coordX > 0 && CurrStation.coordY > 0) // установка координат
                    {
                        CreateStationBt(CurrStation, CurrStation.coordX, CurrStation.coordY,
                            CurrStation.LblX, CurrStation.LblY, CurrStation.lbl_name,
                            tmp_ButtonName, tmp_StationText, CurrStation.st_id, CurrStation.st_line_id, tmp_Color);
                    }
                    else
                    {
                        CreateStationBt(CurrStation, cX, cY,
                            CurrStation.LblX, CurrStation.LblY, CurrStation.lbl_name,
                            tmp_ButtonName, tmp_StationText, CurrStation.st_id, CurrStation.st_line_id, tmp_Color);
                        cX += 20; cY += 5;  // следующую кнопку немного сдвинем
                    }
                }
            }
            lb_MapCreated = true;  // контролы на схеме. больше не добавляем
            X_center = 0; Y_center = 0;    // занулим координаты центра после расчета. 
        }

        private void CreateStationBt(RoundButton a_rb, int ai_X, int ai_Y, int albl_X, int albl_Y, string alblText, string as_name, string as_text, int ai_st_id, int ai_line_id, Color argb_Color) // Создание станции 
        {
            // создаем кнопку с заданными параметрами

            int StX = a_rb.ScurrX, StY = a_rb.ScurrY, LbX = a_rb.LcurrX, LbY = a_rb.LcurrY;   // если нужен пересчет координат из-за Shift, то тут 

            // устанавливаем свойства и события
            a_rb.BackColor = argb_Color; a_rb.BackColor2 = argb_Color;
            a_rb.ButtonRoundRadius = Program.ButtonRndRadius;
            a_rb.SetXY(X, Y);
            a_rb.Location = new System.Drawing.Point(StX, StY);
            a_rb.Name = as_name;
            a_rb.Size = new System.Drawing.Size(Program.ButtonSize, Program.ButtonSize);
            a_rb.TabIndex = ai_st_id; a_rb.Text = "";  // текст отображается на label
            a_rb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            a_rb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            a_rb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            a_rb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.Controls.Add(a_rb);
            a_rb.BringToFront();

            a_rb.ContextMenuStrip = ActionMenu;  // цепляем меню к станции


            // построим рядом с кнопками метки с названиями станций и заданными свойствами
            if (!lb_MapCreated)
            {
                StationLabel label1 = new StationLabel();
                label1.SetXY(X, Y); label1.Text = alblText;
                label1.Location = new System.Drawing.Point(LbX, LbY);
                label1.Name = "Lb" + Convert.ToString(ai_line_id * 10000 + ai_st_id);
                label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
                label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
                label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);

                this.Controls.Add(label1);
                label1.BringToFront();
                a_rb.LbBtCtl = label1;          // запомним в классе станций этот контрол, чтоб не искать потом
                label1.ParentStation = a_rb;    // и встречно запомним станцию-родителя, чтобы легко искать было
            }
            else
            {   // если есть прикрепленная к станции метка, то устанавливаем нужное положение
                if (a_rb.LbBtCtl != null) a_rb.LbBtCtl.Location = new System.Drawing.Point(LbX, LbY);
            }
        }

        private void f_AddLineToSt(LineShape Ln, RoundButton St) // Добавим линию в список линий конкретной станции 
        {
            // добавляет линию Ln в массив контролов для станции St (для масштабирования/отрисовки нужной части)
            int max_ind = 0;
            foreach (var line in St.ConLine)
            {  // ищем размер заполненного массива и проверяем есть ли такая линия уже
                if (line != null)
                {
                    if (line == Ln) return; //  раз такая линия есть, то просто выходим
                    else max_ind++;         //  копим данные
                }
                else // если такой линии нет, то добавим и выйдем
                { St.ConLine[max_ind] = Ln; return; }
            }
        }

        #endregion




        #region Buttons

        private void btnPlus_Click(object sender, EventArgs e) // Масштаб + 
        {
            //устанавливаем нулевые значения дважды (так надо, иначе может не установиться)

            this.HorizontalScroll.Value = 0;
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            //pRoute.Location = new Point(startPx, startPy);
            //Button btn = new Button()
            //{
            //    BackColor = Color.White,
            //    Location = new Point(0, 0),
            //    Width = this.Width,
            //    Height = this.Height,
            //    Text = Lang == "Ru" ? "Подождите, идет масштабирование" : "Wait for zooming",
            //    Font = new Font("Microsoft Sans Serif", 18)
            //};
            //Controls.Add(btn);
            //btn.BringToFront();

            zoomFactor++;
            Zoom(); // метод по масштабированию и сдвигу

            if (zoomFactor >= maxZoom)
                btnPlus.Enabled = false;

            btnMinus.Enabled = true;

            // Очистка ненужных для маршрутов данных
            this.Refresh();
            lblTimeRoute.Text = timeLang;
            foreach (var item in stationLabels)
                item.Visible = false;

            foreach (var item in stationButtons)
                item.Visible = false;

            shpRoute.Shapes.Clear();   // очистка маршрута
            lblCoordinates.Location = new Point(420, 1000);

            //tbFrom.Text = "";
            //tbTo.Text = "";
            label4.Location = startPoint;
            ActualStation = label4;

            //rbFrom = null;
            //rbTo = null;
            this.Refresh();
            

            this.HorizontalScroll.Value = 0;
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0; // снова устанавливаем дважды

            MakePath(false);
            if (rbFrom != null)
                cbFrom.Text = rbFrom.st_name + getNameOfLine(rbFrom.st_line_id);
            if (rbTo != null)
                cbTo.Text = rbTo.st_name + getNameOfLine(rbTo.st_line_id);
            //Controls.Remove(btn);
        }

        private void btnMinus_Click(object sender, EventArgs e) // Масштаб - 
        {
            // снова устанавливаем дважды
            this.HorizontalScroll.Value = 0;
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            pRoute.Location = new Point(startPx, startPy);

            zoomFactor--;
            Zoom(); // масштабирование

            if (zoomFactor <= minZoom)
                btnMinus.Enabled = false;

            btnPlus.Enabled = true;

            // удаление ненужной инфы
            this.Refresh();
            lblTimeRoute.Text = timeLang;
            foreach (var item in stationLabels)
                item.Visible = false;

            foreach (var item in stationButtons)
                item.Visible = false;

            shpRoute.Shapes.Clear();   // очистка маршрута
            lblCoordinates.Location = new Point(420, 1000);


            //cbFrom.SelectedItem = null;
            //cbTo.SelectedItem = null;
            label4.Location = startPoint;
            ActualStation = label4;

            //rbFrom = null;
            //rbTo = null;
            this.Refresh();

            this.HorizontalScroll.Value = 0; // снова устанавливаем дважды
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            MakePath(false);
            if (rbFrom != null)
                cbFrom.Text = rbFrom.st_name + getNameOfLine(rbFrom.st_line_id);
            if(rbTo != null)
                cbTo.Text = rbTo.st_name + getNameOfLine(rbTo.st_line_id);
        }


        private void SchemeEditor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            this.HorizontalScroll.Value = 0;
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0;

            pRoute.Left = Cursor.Position.X - this.Left + this.HorizontalScroll.Value + h;
            pRoute.Top = Cursor.Position.Y - this.Top - 25 + this.VerticalScroll.Value  + v;
            pRoute.BringToFront();
            pRoute.Refresh();


            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;

            h = this.HorizontalScroll.Value;
            v = this.VerticalScroll.Value;
            this.HorizontalScroll.Value = 0;
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0;

            pRoute.Left = Cursor.Position.X - this.Left + this.HorizontalScroll.Value + h;
            pRoute.Top = Cursor.Position.Y - this.Top - 25 + this.VerticalScroll.Value + v;
            pRoute.BringToFront();
            pRoute.Refresh();


            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }

        private void cbFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFrom.SelectedValue != null)
            {
                foreach (var r1 in Controls.OfType<RoundButton>())
                {
                    if (r1.st_id.ToString() == cbFrom.SelectedValue.ToString())
                    {
                        rbFrom = r1;
                    }
                }
            }
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            cbFrom.Enabled = false;
            cbFrom.Enabled = true;
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }

        private void cbTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTo.SelectedValue != null)
            {
                foreach (var r in Controls.OfType<RoundButton>())
                {
                    if (r.st_id.ToString() == cbTo.SelectedValue.ToString())
                    {
                        rbTo = r;
                    }
                }
            }
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            cbTo.Enabled = false;
            cbTo.Enabled = true;
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }


        private void btnPath_Click(object sender, EventArgs e) // Построение нового маршрута 
        {
            MakePath(true);
        }

        private void MakePath(bool f)
        {
            this.Refresh();
            b7.Enabled = true;
            //RoundButton prevFrom = rbFrom;
            //RoundButton prevTo = rbTo;

            // Проверка кореектности данных

            
            if (rbFrom == null || rbTo == null)
            {
                if (f == true)
                {
                    MessageBox.Show("Невозможно построить такой маршрут!");
                }
                return;
                //if (rbFrom == null)
                //{
                //    foreach (var rb in Controls.OfType<RoundButton>())
                //    {
                //        if (rb.st_name == tbFrom.Text)
                //        {
                //            rbFrom = rb;
                //        }
                //    }
                //}

                //if (rbTo == null)
                //{
                //    foreach (var rb in Controls.OfType<RoundButton>())
                //    {
                //        if (rb.st_name == tbTo.Text)
                //        {
                //            rbTo = rb;
                //        }
                //    }
                //}

             
                //else
                //if (rbFrom == prevFrom || rbTo == prevTo)
                //{
                //    tbFrom.Text = rbFrom.Text;
                //    tbTo.Text = rbTo.Text;
                //}
            }
            else
            {
                //foreach (var rb in Controls.OfType<RoundButton>())
                //{
                //    if (rb.st_name == tbFrom.Text && rb.st_id == rbFrom.st_id)
                //    {
                //        rbFrom = rb;
                //    }
                //}
                
                //foreach (var rb in Controls.OfType<RoundButton>())
                //{
                //    if (rb.st_name == tbTo.Text && rb.st_id == rbTo.st_id)
                //    {
                //        rbTo = rb;
                //    }
                //}
                
            }

            if (rbFrom != rbTo)
            {
                //tbFrom.Text = rbFrom.st_name;
                //tbTo.Text = rbTo.st_name;
                foreach (var item in stationLabels)
                    item.Visible = false;

                foreach (var item in stationButtons)
                    item.Visible = false;

                shpRoute.Shapes.Clear();   // очистка предыдущего маршрута
                lblCoordinates.Location = new Point(420, 1000);

                label4.Location = startPoint; // стартовая позиция и лейбл для подробностей
                ActualStation = label4;

                st_from = rbFrom;
                st_to = rbTo;

                int i = 0;
                int last = 0;
                var dijkstra = new Dijkstra(Program.g);
                var path = dijkstra.FindShortestPath(st_from.UniqueId.ToString(), st_to.UniqueId.ToString()); // рассчет маршрута
                // теперь построим маршрут
                if (path.Length > 0)
                {
                    string[] words = path.Split(new char[] { ' ' });
                    List<string> times = new List<string>();
                    int itogs = 0;
                    int StFr = 0, StTo = Convert.ToInt32(words[0]);

                    foreach (string s in words)
                    {
                        // разделили маршрут на станции, теперь начинаем строить
                        StTo = Convert.ToInt32(s);
                        if (StFr > 0)
                        {
                            Program.StationDict.TryGetValue(StFr, out st_from);
                            Program.StationDict.TryGetValue(StTo, out st_to);
                            if (st_from != null && st_to != null) // строим маршрут по заданным станциям на карте
                                f_DrawConnector(st_from, st_to, Color.Black, "Route" + StFr.ToString() + StTo.ToString(), (int)(st_from.Height / 2), 5, false, shpRoute /*Это контейнер, где храним маршрут*/);
                            else MessageBox.Show("Сюда попадать не должны, так как все станции должны быть");

                            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

                            string addForTime = "";

                            if (l_con == null) { MessageBox.Show("Чтение станций невозможно. Ошибка установления соединения"); return; }
                            try
                            {
                                using (l_con)
                                {
                                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);

                                    // берем время маршрута из базы, записываем его в лейблы
                                    cmd.CommandText = "select time from Graph where st_id_from = @st_id_from and st_id_to = @st_id_to or st_id_from = @st_id_to and st_id_to = @st_id_from";
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_from", st_from.st_id, "int");
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_to", st_to.st_id, "int");
                                    cmd.ExecuteNonQuery();

                                    System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                    if (reader.HasRows)
                                    {
                                        int time;
                                        while (reader.Read())
                                        {
                                            time = reader.GetInt32(0);

                                            if (Lang == "Ru")
                                                addForTime += " (" + time + " мин)";
                                            else
                                                addForTime += " (" + time + " min)";
                                            itogs += time;
                                        }
                                    }
                                    reader.Close();


                                    // Информация о посадке в вагон, если она нужна

                                    int id1 = 0, id2 = 0;

                                    cmd.CommandText = "select line_id from Stations where id = @st_id_from";
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_from", st_from.st_id, "int");
                                    reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            id1 = reader.GetInt32(0);
                                        }
                                    }
                                    reader.Close();


                                    cmd.CommandText = "select line_id from Stations where id = @st_id_to";
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_to", st_to.st_id, "int");
                                    reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            id2 = reader.GetInt32(0);
                                        }
                                    }
                                    reader.Close();



                                    if (id1 != id2) // Если переход
                                    {
                                        for (int j = words.Length - 1; j > 0; j--)
                                        {
                                            if (words[j] == st_from.Name.Substring(2))
                                            {
                                                string name_prev = words[j - 1]; // Станция от которой мы попали на переход
                                                RoundButton rbPrev = new RoundButton();
                                                foreach (var roundB in Controls.OfType<RoundButton>())  //  
                                                {
                                                    if (roundB.Name.Substring(2) == name_prev)
                                                    {
                                                        rbPrev = roundB;
                                                        break;
                                                    }
                                                }
                                                // Нашли предыдущую станцию, известсен переход, сделаем запрос 

                                                if (rbPrev.st_id != 0)
                                                {
                                                    cmd.CommandText = "select st_prev, num from WaggonNums where st_from = @st_id_from and st_to = @st_id_to";
                                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_from", st_from.st_id, "int");
                                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_to", st_to.st_id, "int");

                                                    cmd.ExecuteNonQuery();

                                                    System.Data.Common.DbDataReader reader1 = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                                    if (reader1.HasRows)
                                                    {
                                                        while (reader1.Read())
                                                        {
                                                            int prev = reader1.GetInt32(0);
                                                            int num = reader1.GetInt32(1);

                                                            foreach (var r in Controls.OfType<RoundButton>())  //  
                                                            {
                                                                if (r.st_id == prev)
                                                                {
                                                                    if (r.st_line_id == rbPrev.st_line_id)
                                                                    {
                                                                        if (prev != rbPrev.st_id)
                                                                            num *= -1;


                                                                        if (Lang == "Ru")
                                                                            switch (num)
                                                                            {
                                                                                case 1:
                                                                                    addForTime += "[Садитесь ближе к началу]";
                                                                                    break;
                                                                                case 2:
                                                                                    addForTime += "[Садитесь в начало]";
                                                                                    break;
                                                                                case -1:
                                                                                    addForTime += "[Садитесь ближе к концу]";
                                                                                    break;
                                                                                case -2:
                                                                                    addForTime += "[Садитесь в конец]";
                                                                                    break;
                                                                                default:
                                                                                    addForTime += "[Садитесь посередине]";
                                                                                    break;
                                                                            }
                                                                        
                                                                        else
                                                                            switch (num)
                                                                            {
                                                                                case 1:
                                                                                    addForTime += "[Take car closer to front]";
                                                                                    break;
                                                                                case 2:
                                                                                    addForTime += "[Take first car]";
                                                                                    break;
                                                                                case -1:
                                                                                    addForTime += "[Take car closer to back]";
                                                                                    break;
                                                                                case -2:
                                                                                    addForTime += "[Take last car]";
                                                                                    break;
                                                                                default:
                                                                                    addForTime += "[Take central car]";
                                                                                    break;
                                                                            }


                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Невозможно найти предыдущую станцию!");
                                                }

                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            { MessageBox.Show(" Ошибка чтения " + ex.Message); }
                            times.Add(addForTime);
                        }

                        // визуализация подробностей поездки с помощью RoundButton и Label
                        Label newStation = stationLabels[last];
                        last++;

                        if (StFr > 0)
                            newStation.Text = st_to.st_name;
                        else
                            newStation.Text = st_from.st_name;

                        if (Lang == "En")
                        {
                            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
                            string engName = "";

                            if (l_con == null) { MessageBox.Show("Чтение станций (Eng) невозможно. Ошибка установления соединения"); return; }
                            try
                            {
                                using (l_con) // берем английское название станции, если выбран английский язык
                                {
                                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);

                                    cmd.CommandText = "select value from LangRes where id = @id";
                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", StFr > 0 ? st_to.st_id : st_from.st_id, "int");
                                    cmd.ExecuteNonQuery();

                                    System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                    if (reader.HasRows) // если есть данные
                                    {

                                        while (reader.Read())
                                        {
                                            engName = reader.GetString(0);
                                        }
                                    }
                                    reader.Close();
                                }

                                newStation.Text = engName;
                            }
                            catch (Exception ex)
                            { MessageBox.Show(" Ошибка чтения " + ex.Message); }
                        }


                        if (StFr > 0) // записываем данные о времени
                        {
                            newStation.Text += times[i];
                            i++;
                        }

                        //StTo = Convert.ToInt32(s); 

                        // Добавление подробностей на панель со сдвигом координат
                        newStation.Visible = true;
                        newStation.Location = new Point(ActualStation.Left, ActualStation.Top + 20);
                        ActualStation = newStation;
                        RoundButton rb = new RoundButton()
                        {
                            Size = new Size(20, 20),
                            ButtonRoundRadius = 10,
                            BackColor = StFr > 0 ? st_to.BackColor : st_from.BackColor,
                            BackColor2 = StFr > 0 ? st_to.BackColor2 : st_from.BackColor2,
                            Location = new Point(newStation.Left - 21, newStation.Top)
                        };

                        StFr = StTo;

                        stationButtons.Add(rb);
                        pRoute.Controls.Add(rb);
                    }

                    // Итоговое время
                    lblTimeRoute.Text = timeLang + itogs;
                    if (Lang == "Ru")
                        lblTimeRoute.Text += " мин";
                    else
                        lblTimeRoute.Text += " min";


                }
                else MessageBox.Show("Невозможно построить такой маршрут!");
            }
        }
        


        private void button2_Click(object sender, EventArgs e)  // Очистка маршрута с карты 
        {
            shpRoute.Shapes.Clear();
            lblCoordinates.Location = new Point(420, 1000);
        }

        private void btnZoomSave_Click(object sender, EventArgs e) // Сохраняем координаты станций при данном зуме 
        {
            this.HorizontalScroll.Value = 0; // снова устанавливаем дважды
            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            this.VerticalScroll.Value = 0;
            // Лучше по 2 раза

            int iKey;
            RoundButton tmpButton;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null) { MessageBox.Show("Сохранение станций невозможно. Ошибка установления соединения"); return; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    foreach (var rb in Controls.OfType<RoundButton>())  //  
                    {
                        iKey = Convert.ToInt32(rb.Name.Substring(2)); // получаем ключ. Имя кнопки = "St" + id, поэтому просто обрезаем "St"
                        if (iKey > 0)
                        {
                            Program.StationDict.TryGetValue(iKey, out tmpButton);
                            if (tmpButton != null)
                            {
                                // Перезаписываем координаты кнопок и лейблов при днном ZoomFactor
                                tmpButton.LblX = tmpButton.LbBtCtl.Left;
                                tmpButton.LblY = tmpButton.LbBtCtl.Top;
                                cmd.CommandText = "update ZoomCoordinates set  coordX = @coordX, coordY = @coordY, lblX = @lblX, lblY = @lblY where id=@id and zoom = @zoom";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", tmpButton.coordX, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", tmpButton.coordY, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lblX", tmpButton.LblX, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lblY", tmpButton.LblY, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@zoom", zoomFactor, "int");
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(" Ошибка сохранения " + ex.Message); }
        }
        
        private void btnDeleteRoute_Click(object sender, EventArgs e) // Очистка маршрута
        {
            this.Refresh();
            lblTimeRoute.Text = timeLang;
            foreach (var item in stationLabels)
                item.Visible = false;

            foreach (var item in stationButtons)
                item.Visible = false;

            shpRoute.Shapes.Clear();   // очистка маршрута
            lblCoordinates.Location = new Point(420, 1000);

            cbFrom.SelectedItem = null;
            cbTo.SelectedItem = null;

            label4.Location = startPoint;
            ActualStation = label4;

            rbFrom = null;
            rbTo = null;
            this.Refresh();
        }
        
        private void CoonectSt_Click(object sender, EventArgs e) // Режим конектора 
        {
            if (ConnSt == 'N')
            {
                ConnSt = 'Y';    // будем рисовать соединители станций (заполняем таблицу Graph)
                ConnectorPanel.Enabled = true; ConnectorPanel.Visible = true;
                st_from_lbl.Text = "От";
            }
            else
            {
                ConnSt = 'N';    // не будем рисовать соединители станций (заполняем таблицу Graph)
                ConnectorPanel.Enabled = false; ConnectorPanel.Visible = false;
            }
        }



        private void StationBt_Click(object sender, EventArgs e) // форма станций 
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            StationsForm StForm = new StationsForm();
            StForm.ShowDialog();
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        } 

        private void Setup_Click(object sender, EventArgs e) // форма настроек 
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            SetupForm StUpForm = new SetupForm();
            StUpForm.ShowDialog();
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }

        private void LineBt_Click(object sender, EventArgs e) // форма линий 
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            LinesForm LnForm = new LinesForm();
            LnForm.ShowDialog();
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }

        

        
        private void языкToolStripMenuItem_Click(object sender, EventArgs e) // Смена языка 
        {
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

            if (l_con == null) { MessageBox.Show("Set станций default невозможно. Ошибка установления соединения"); return; }
            try
            {
                using (l_con)
                {

                    if (Lang == "En")
                    {
                        Lang = "Ru";
                        языкToolStripMenuItem.Text = "Язык";
                        помощьToolStripMenuItem.Text = "Помощь";
                        настройкиToolStripMenuItem.Text = "Настройки";
                        режимыToolStripMenuItem.Text = "Режимы";
                        историческоеПредставлениеToolStripMenuItem.Text = "Историческое представление";
                        MessageBox.Show("Перезапустите приложение!");
                    }
                    else
                    {
                        Lang = "En";
                        языкToolStripMenuItem.Text = "Language";
                        помощьToolStripMenuItem.Text = "Help";
                        настройкиToolStripMenuItem.Text = "Settings";
                        режимыToolStripMenuItem.Text = "Modes";
                        историческоеПредставлениеToolStripMenuItem.Text = "Historical reference";
                        MessageBox.Show("Restart the application!");
                    }

                    // перезаписывание в базу
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "update Setup set lang = '" + Lang + "'";
                    cmd.ExecuteNonQuery();
                    this.Close();
                }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка set default: " + ex.Message); }
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e) // Справка 
        {
            MessageBox.Show("Нажмите на станцию правой кнопкой мыши. Выберите раздел 'Информация', если хотите узнать информацию о станции. Выберите разделы 'Сюда','Отсюда' для построения маршрута. Информация о маршруте находится в правом верхнем углу", "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e) // форма настроек 
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            SettingsForm s = new SettingsForm();
            s.ShowDialog();
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;

            foreach (var b in this.Controls.OfType<Button>())
                b.BackColor = DefColor;
            

            foreach (var b in pRoute.Controls.OfType<Button>())
                b.BackColor = DefColor;
            
        }

        private void викторинаToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void викторинаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            QuizForm quizForm = new QuizForm();
            quizForm.ShowDialog();
        }

        private void режимыToolStripMenuItem_Click(object sender, EventArgs e) // форма Разработчика 
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;

            EditForm editForm = new EditForm();
            editForm.ShowDialog();

            if (isEditor)
            {

                this.HorizontalScroll.Value = 0;
                this.HorizontalScroll.Value = 0;
                this.VerticalScroll.Value = 0;
                this.VerticalScroll.Value = 0;
                c1.Visible = true;
                //c2.Visible = true;
                b1.Visible = true;
                b2.Visible = true;
                b3.Visible = true;
                b4.Visible = true;
                b5.Visible = true;
                b6.Visible = true;
                b8.Visible = true;
                b7.Visible = true;
            }


            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }
        
        private void историческоеПредставлениеToolStripMenuItem_Click(object sender, EventArgs e) // форма Исторического представления 
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;
            HistoricalViewForm form = new HistoricalViewForm();
            form.ShowDialog();
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
        }


        #endregion




        #region Mouse & Keys

        private void btn_MouseEnter(object sender, EventArgs e) // персонализация 
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                btn.BackColor = MyColor;
            }

            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem btn = sender as ToolStripMenuItem;
                btn.BackColor = MyColor;
            }
        }

        private void btn_MouseLeave(object sender, EventArgs e) // персонализация 
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                btn.BackColor = DefColor;
            }

            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem btn = sender as ToolStripMenuItem;
                btn.BackColor = Color.White;
            }
        }


        //int px = 0, py = 0;

        private void OnMouseDown(object sender, MouseEventArgs e)  
        {
            int h = this.HorizontalScroll.Value;
            int v = this.VerticalScroll.Value;

            //if (sender is Panel)
            //{
            //    Panel panel = sender as Panel;
            //    if (panel.Name == "pRoute")
            //    {
            //        px = Cursor.Position.X + Cursor.Position.X - panel.Left - this.Left;
            //        py = Cursor.Position.Y + Cursor.Position.Y - panel.Top - this.Top;

            //        panel.Location = new Point(px,py);
            //    }
            //}


            if (c1.Checked)
            {
               
                Point p = this.PointToClient(Cursor.Position);
                if (e.Button == MouseButtons.Right)
                {
                    // чтобы не было лишней фокусировки
                    btnMinus.Enabled = false;
                    btnPlus.Enabled = false;
                    b3.Enabled = false;
                    b1.Enabled = false;
                    b2.Enabled = false;
                    b4.Enabled = false;
                    b5.Enabled = false;
                    b6.Enabled = false;
                    b7.Enabled = false;
                    this.b8.Enabled = false;
                    ConnectorPanel.Enabled = false;
                   
                    c2.Enabled = false;
                    c1.Enabled = false;

                    if (sender is RoundButton)
                    {
                        //  если мы находимся в режиме создания соединителей, то запомним станцию st_id_from
                        if (ConnSt == 'Y')
                        {
                            st_from = sender as RoundButton;
                            // создадим временный объект, чтобы было куда рисовать линию соединителя
                            this.pointer_st = new System.Windows.Forms.Button();
                            pointer_st.Location = new System.Drawing.Point(p.X, p.Y); pointer_st.Name = "pointer_st";
                            pointer_st.BackColor = System.Drawing.Color.White;
                            pointer_st.Size = new System.Drawing.Size(4, 4); pointer_st.Text = "";  // текст отображается на label
                            this.Controls.Add(pointer_st);
                            pointer_st.BringToFront();
                        }
                        if (ConnSt == 'N')
                        {
                            st_from = sender as RoundButton;
                        }
                    }
                }
                if (e.Button == MouseButtons.Left)
                {
                    // чтобы не было лишней фокусировки
                    btnMinus.Enabled = false;
                    btnPlus.Enabled = false;
                    b3.Enabled = false;
                    b1.Enabled = false;
                    b2.Enabled = false;
                    b4.Enabled = false;
                    b5.Enabled = false;
                    b6.Enabled = false;
                    b7.Enabled = false;
                    this.b8.Enabled = false;
                    ConnectorPanel.Enabled = false;
                    c2.Enabled = false;
                    c1.Enabled = false;

                    if (sender is LineShape)  // если даванули на коннектор, то покажем описание
                    {
                        ShowConnDesc.AutoSize = true;
                        ShowConnDesc.Location = new System.Drawing.Point((int)(((sender as LineShape).X1 + (sender as LineShape).X2) / 2), (int)(((sender as LineShape).Y1 + (sender as LineShape).Y2) / 2));
                        ShowConnDesc.Name = "labet";
                        ShowConnDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        ShowConnDesc.Size = new System.Drawing.Size(29, 13);
                        ShowConnDesc.Text = (sender as LineShape).Name;
                        Controls.Add(ShowConnDesc);
                        ShowConnDesc.BringToFront();
                    }
                    if (sender is RoundButton)
                    {
                        if (Program.EditMode)
                        {
                            MovingButton = sender as RoundButton;
                            //    if (sender is RoundButton && f_FindStLabel(MovingButton, out MovingLabel) == 1)
                            if (sender is RoundButton && (sender as RoundButton).LbBtCtl != null)
                            {
                                StationLabel MovingLabel = (sender as RoundButton).LbBtCtl;
                                // определим смещение метки относительно станции, чтобы таскалась за ней
                                DeltaLblX = MovingButton.Location.X - MovingLabel.Location.X;
                                DeltaLblY = MovingButton.Location.Y - MovingLabel.Location.Y;
                                PtX = MovingButton.Location.X;  // запомним, где была станция в момент начала движения, чтобы посчитать смещение
                                PtY = MovingButton.Location.Y;
                            }
                        }
                      
                    }
                    if (Program.EditMode && sender is StationLabel)
                    {
                        PtX = (sender as StationLabel).Location.X;  // запомним, где была метка в момент начала движения, чтобы посчитать смещение
                        PtY = (sender as StationLabel).Location.Y;
                        StationLabel stationLabel = sender as StationLabel;
                        stationLabel.Focus();
                    }
                    if (sender is SchemeEditor)
                    { // запомним координаты тыка для дальнешего подсчета сдвига, чтобы передвинуть карту
                        cpoint_MouseDown.X = e.Location.X; cpoint_MouseDown.Y = e.Location.Y;
                    }
                }


            }
            
            
            this.HorizontalScroll.Value = h;
            this.HorizontalScroll.Value = h;
            this.VerticalScroll.Value = v;
            this.VerticalScroll.Value = v;
            // Восстановление ползунков
        }
        
        private void OnMouseUp(object sender, MouseEventArgs e) 
        {
            if (c1.Checked)
            {
                int h = this.HorizontalScroll.Value;
                int v = this.VerticalScroll.Value;

                if (ConnSt == 'N' && (e.Button & MouseButtons.Right) != 0) // прошлый рассчет маршрута путем зажатия ПКМ
                {
                    // проверим, что координаты попадают на станцию
                    Point p = this.PointToClient(Cursor.Position);
                    st_to = null;
                    bool bFound = false;
                    foreach (var rb in this.Controls.OfType<RoundButton>())
                    {
                        if (rb != null && st_from != null)
                        {
                            if (rb.Location.X <= p.X && rb.Location.X + st_from.Height >= p.X)
                                if (rb.Location.Y <= p.Y && rb.Location.Y + st_from.Height >= p.Y)
                                {
                                    bFound = true; st_to = rb;
                                }
                            if (bFound) break;
                        }
                    }
                    //             st_to = sender as RoundButton;  // если есть обе станции для построения маршрута, то забуцкаем 
                    if (st_from != null && st_to != null && st_from != st_to)
                    {
                        int i = 0;
                        int last = 0;
                        var dijkstra = new Dijkstra(Program.g);
                        var path = dijkstra.FindShortestPath(st_from.UniqueId.ToString(), st_to.UniqueId.ToString());
                        // теперь построим маршрут
                        if (path.Length > 0)
                        {
                            string[] words = path.Split(new char[] { ' ' });
                            List<string> times = new List<string>();
                            int StFr = Convert.ToInt32(words[0]), StTo = 0;
                            foreach (string s in words)
                            {  // разделили маршрут на станции, теперь начинаем строить
                                
                                StFr = Convert.ToInt32(s);
                                if (StTo > 0)
                                {
                                    Program.StationDict.TryGetValue(StFr, out st_from);
                                    Program.StationDict.TryGetValue(StTo, out st_to);
                                    if (st_from != null && st_to != null)
                                        f_DrawConnector(st_from, st_to, Color.Black, "Route" + StFr.ToString() + StTo.ToString(), (int)(st_from.Height / 2), 5, false, shpRoute /*Это контейнер, где храним маршрут*/);
                                    else MessageBox.Show("Сюда попадать не должны, т.к.все станции должны быть");

                                    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

                                    if (l_con == null) { MessageBox.Show("Чтение станций невозможно. Ошибка установления соединения"); return; }
                                    try
                                    {
                                        using (l_con)
                                        {
                                            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);

                                            cmd.CommandText = "select time from Graph where st_id_from = @st_id_from and st_id_to = @st_id_to or st_id_from = @st_id_to and st_id_to = @st_id_from";
                                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_from", st_from.st_id, "int");
                                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id_to", st_to.st_id, "int");
                                            cmd.ExecuteNonQuery();

                                            System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
                                            if (reader.HasRows) // если есть данные
                                            {
                                                int time;
                                                while (reader.Read()) // построчно считываем данные st_id_from и st_id_to
                                                {
                                                    time = reader.GetInt32(0);
                                                    
                                                    if (Lang == "Ru")
                                                        times.Add(" (" + time + " мин)");
                                                    else
                                                        times.Add(" (" + time + " min)");
                                                }
                                            }
                                            reader.Close();
                                        }

                                    }
                                    catch (Exception ex)
                                    { MessageBox.Show(" Ошибка чтения " + ex.Message); }

                                }

                                Label newStation = stationLabels[last];
                                last++;

                                newStation.Text = st_from.st_name;

                                if (StTo > 0)
                                {
                                    newStation.Text += times[i];
                                    i++;
                                }

                                StFr = StTo;
                                StTo = Convert.ToInt32(s); // if (Int32.TryParse(s, out StTo)));


                                newStation.Visible = true;
                                newStation.Location = new Point(ActualStation.Left, ActualStation.Top + 20);
                                ActualStation = newStation;
                                RoundButton rb = new RoundButton()
                                {
                                    Size = new Size(20, 20),
                                    ButtonRoundRadius = 10,
                                    BackColor = st_from.BackColor,
                                    BackColor2 = st_from.BackColor2,
                                    Location = new Point(newStation.Left - 21, newStation.Top)
                                };





                                stationButtons.Add(rb);
                                pRoute.Controls.Add(rb);
                            }

                        }
                        else MessageBox.Show("Невозможно построить маршрут");
                    }
                }
                if (ConnSt == 'Y' && (e.Button & MouseButtons.Right) != 0) // если в режиме коннектора
                {
                    //  если есть запомненная станция и кнопу отпустили, то  станцию st_id_to
                    if (ConnSt == 'Y' && st_from != null)
                    {
                        if (ConnSt == 'Y' && (e.Button & MouseButtons.Right) != 0)
                        {
                            // проверим, что координаты попадают на станцию
                            Point p = this.PointToClient(Cursor.Position);
                            bool bFound = false;
                            foreach (var rb in this.Controls.OfType<RoundButton>())
                            {
                                if (rb.Location.X <= p.X && rb.Location.X + st_from.Height >= p.X)
                                    if (rb.Location.Y <= p.Y && rb.Location.Y + st_from.Height >= p.Y)
                                    {
                                        bFound = true; // чтобы прервать цикл
                                                       // добавим новый коннектор, предварительно проверив существование такого же
                                        if (st_from.st_id > 0 && rb.st_id > 0 && st_from.st_line_id > 0 && rb.st_line_id > 0)
                                        {
                                            int GrShow = 0, GrTime = 0, li_tmp;
                                            string desc = String.Empty;
                                            if (GraphShow.Checked) GrShow = 1;
                                            if (Int32.TryParse(GraphTime.Text, out li_tmp)) GrTime = Math.Abs(li_tmp);
                                            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
                                            if (l_con == null)
                                            {
                                                MessageBox.Show("Сохранение коннектора невозможно. Ошибка установления соединения"); return;
                                            }
                                            try
                                            {
                                                using (l_con)
                                                {
                                                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                                                    cmd.CommandText = "select count() from Graph where st_id_from=@st_from_id and st_id_to=@st_to_id";   // формируем строку запроса
                                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_from_id", st_from.st_id, "int");  // задаем значения параметров
                                                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_to_id", rb.st_id, "int");
                                                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                                                    object cnt = cmd.ExecuteScalar();

                                                    if (textBox1.Text.Length > 0) desc = textBox1.Text;
                                                    if (cnt != null && Convert.ToInt32(cnt.ToString()) == 0)   // если такого коннектора еще не было
                                                    {
                                                        DbTables.Up_Graph('I', st_from.st_id, rb.st_id, GrTime, GrShow, desc);
                                                    }
                                                    if (cnt != null && Convert.ToInt32(cnt.ToString()) > 0)    // такой коннектор уже есть
                                                    {
                                                        DbTables.Up_Graph('U', st_from.st_id, rb.st_id, GrTime, GrShow, desc);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(" Ошибка сохранения " + ex.Message);
                                            }
                                            // подрисуем коннектор на схеме
                                            RoundButton Bt1, Bt2;   // определим реальные экземпляры класса по контролу 
                                            Program.StationDict.TryGetValue(Convert.ToInt32(st_from.Name.Substring(2)), out Bt1);
                                            Program.StationDict.TryGetValue(Convert.ToInt32(rb.Name.Substring(2)), out Bt2);
                                            if (Bt1 != null && Bt2 != null)
                                            {
                                                int HBt = (int)Bt1.Height / 2;
                                                f_DrawConnector(Bt1, Bt2, rb.BackColor, desc, HBt, BrdWdth, true, shapeContainer2);
                                                //    Invalidate();  // покажем линию
                                            }
                                        }
                                        else { MessageBox.Show("Коннектор создать нельзя. Неверные значения id"); }
                                    }
                                if (bFound) break;
                            }
                        }
                    }

                    pointer_st.Dispose();  // удалим, ставшую ненужной кнопку
                }
                if (e.Button == MouseButtons.Left)
                {
                    btnMinus.Enabled = true;
                    btnPlus.Enabled = true;
                    b3.Enabled = true;
                    b1.Enabled = true;
                    b2.Enabled = true;
                    b4.Enabled = true;
                    b5.Enabled = true;
                    b6.Enabled = true;
                    b7.Enabled = true;
                    this.b8.Enabled = true;
                    ConnectorPanel.Enabled = true;
                    //c2.Enabled = true;
                    c1.Enabled = true;

                    if (sender is Microsoft.VisualBasic.PowerPacks.LineShape)  // если даванули на коннектор, то покажем описание
                    {
                        if (Controls.Contains(ShowConnDesc)) Controls.Remove(ShowConnDesc); // метка больше не нужна, емсли была
                    }
                    if (Program.EditMode && sender is RoundButton)  // Перемещение кнопки
                    {
                        RoundButton CStation = null;
                        Program.StationDict.TryGetValue((sender as RoundButton).st_line_id * 10000 + (sender as RoundButton).st_id, out CStation);
                        if (CStation != null)
                        {
                            // координата в реале сместится пропорционально смещению на карте 
                            CStation.coordX = CStation.coordX + (int)((CStation.ScurrX - PtX) / kTotal);
                            CStation.coordY = CStation.coordY + (int)((CStation.ScurrY - PtY) / kTotal);
                            CStation.ScurrX = CStation.Location.X; CStation.ScurrY = CStation.Location.Y;
                        }
                        if (CStation.LbBtCtl != null)
                        {
                            CStation.LblX = CStation.LblX + (int)((CStation.ScurrX - PtX) / kTotal);
                            CStation.LblY = CStation.LblY + (int)((CStation.ScurrY - PtY) / kTotal);
                            CStation.LcurrX = CStation.LbBtCtl.Location.X; CStation.LcurrY = CStation.LbBtCtl.Location.Y;
                        }
                    }
                    else if (Program.EditMode && sender is StationLabel) // Перемещение лейбла
                    {
                        StationLabel lbl = (sender as StationLabel), lblCtrl = null;
                        RoundButton CStation = null;
                        string LbName = lbl.Name;
                        if (this.Controls.ContainsKey(LbName))     // если есть такая метка, то возьмем ее координаты и Text для сохранения в файл
                        {
                            if (this.Controls[LbName] is StationLabel)
                            {
                                lblCtrl = this.Controls[LbName] as StationLabel;
                            }
                        }
                        // вычленим из имени метки st_line_id * 10000 + st_id
                        LbName = lbl.Name.Substring(2);
                        Program.StationDict.TryGetValue(Convert.ToInt32(LbName), out CStation);
                        if (CStation != null)
                        {
                            CStation.LblX = CStation.LblX + (int)((lbl.Location.X - PtX) / kTotal);
                            CStation.LblY = CStation.LblY + (int)((lbl.Location.Y - PtY) / kTotal);
                        }
                    }
                    MovingButton = null;  // занулим значения ссылок, т.к.движение окончено 
                }

                this.HorizontalScroll.Value = h;
                this.HorizontalScroll.Value = h;
                this.VerticalScroll.Value = v;
                this.VerticalScroll.Value = v;
            }
        }
        
        private void OnMouseWheel(object sender, MouseEventArgs e) 
        {
            // устаревшее увеличение
            //if (c1.Checked && wheel) 
            //{
            //    Point p = this.PointToClient(Cursor.Position);
            //    X_center = p.X; Y_center = p.Y;  // зафиксируем центр, относительно которого будем увеличивать  
            //    if (e.Delta > 0) CalcCoord(X_center, Y_center, +1);
            //    if (e.Delta < 0) CalcCoord(X_center, Y_center, -1);
            //    // это чтобы не исчезали кусочки контролов на экране (появляются, когда сверху мышой проведешь)
            //    // надо также глянуть Refresh & Update, т.к.тут не особо кузяво помогает
            //    screenRectangle = RectangleToScreen(this.ClientRectangle);
            //    Invalidate(screenRectangle, true);
            //}
        }

        private void OnMouseMove(object sender, MouseEventArgs e) 
        {
            //if ((e.Button & MouseButtons.Left) != 0)
            //{
            //    if (sender is Panel)
            //    {
            //        Panel panel = sender as Panel;
            //        if (panel.Name == "pRoute")
            //        {
            //            px = Cursor.Position.X + Cursor.Position.X - panel.Left - this.Left;
            //            py = Cursor.Position.Y + Cursor.Position.Y - panel.Top - this.Top;

            //            panel.Location = new Point(px, py);
            //            panel.BringToFront();
            //        }
            //    }
            //}

                if (c1.Checked)
            {
                Point p = this.PointToClient(Cursor.Position);
                if (ConnSt == 'Y' && (e.Button & MouseButtons.Right) != 0)  // если рисуем соединитель
                    if (pointer_st != null) pointer_st.Location = new System.Drawing.Point(p.X, p.Y);
                if ((e.Button & MouseButtons.Left) != 0)
                {

                    if (sender is SchemeEditor && map)  // если на форме зажата левая кнопа, то надо двигать карту
                    {
                        X_move = e.X - cpoint_MouseDown.X; Y_move = e.Y - cpoint_MouseDown.Y;
                        //  передвигаем станции/метки на нужную величину (X_move, Y_move)  f_RecreateStations();   // подвинем карту
                        foreach (KeyValuePair<int, RoundButton> kvp in Program.StationDict)
                        {
                            RoundButton CurrStation = kvp.Value;
                            if (CurrStation != null)
                            {
                                CurrStation.ScurrX += X_move; CurrStation.ScurrY += Y_move;
                                CurrStation.Location = new System.Drawing.Point(CurrStation.ScurrX, CurrStation.ScurrY);
                                if (CurrStation.LbBtCtl != null)   // передвинем метку, если она есть
                                {
                                    CurrStation.LcurrX += X_move; CurrStation.LcurrY += Y_move;
                                    CurrStation.LbBtCtl.Location = new System.Drawing.Point(CurrStation.LcurrX, CurrStation.LcurrY);
                                }
                            }
                        }
                        foreach (LineShape item in shapeContainerR.Shapes.OfType<LineShape>())   // надо учесть смену координат и для маршрута
                        { item.X1 += X_move; item.X2 += X_move; item.Y1 += Y_move; item.Y2 += Y_move; }
                        ShftX += X_move; ShftY += Y_move; // храним данные по сдвигу для расчета реальных координат
                        X_move = 0; Y_move = 0;           // сбросим координаты сдвига
                        screenRectangle = RectangleToScreen(this.ClientRectangle);
                        Invalidate(screenRectangle, true);
                        // подсчитаем величину, на которую сдвинули начало координат
                        cpoint_MouseDown = p;
                    }

                    if (Program.EditMode && sender is RoundButton)  // Перемещение кнопки
                    {
                        RoundButton CStation = null;
                        Program.StationDict.TryGetValue((sender as RoundButton).st_line_id * 10000 + (sender as RoundButton).st_id, out CStation);
                        if (CStation != null) CStation.Location = new System.Drawing.Point(p.X, p.Y);
                        CStation.ScurrX = p.X; CStation.ScurrY = p.Y;
                        if (CStation.LbBtCtl != null)
                        {
                            CStation.LcurrX = p.X - DeltaLblX; CStation.LcurrY = p.Y - DeltaLblY;
                            CStation.LbBtCtl.Location = new System.Drawing.Point(CStation.LcurrX, CStation.LcurrY);
                        }
                    }
                }
            }
        }
        
        private void OnKeyPress(object sender, KeyPressEventArgs e) 
        {
            if (c2.Checked) // перемещение с помощью клавиш (сейчас неактивно)
            {
                if (sender is RoundButton)
                {
                    switch (e.KeyChar)
                    {
                        case 'ц':
                        case 'w':
                            rb.Top--;
                            rb.coordY--;
                            rb.LbBtCtl.Top--;
                            lblCoordinates.Text = rb.Location.ToString();
                            //f_DrawAllConnectors();
                            break;
                        case 'ы':
                        case 's':
                            rb.Top++;
                            rb.coordY++;
                            rb.LbBtCtl.Top++;
                            lblCoordinates.Text = rb.Location.ToString();
                            //f_DrawAllConnectors();
                            break;
                        case 'ф':
                        case 'a':
                            rb.Left--;
                            rb.coordX--;
                            rb.LbBtCtl.Left--;
                            lblCoordinates.Text = rb.Location.ToString();
                            //f_DrawAllConnectors();
                            break;
                        case 'в':
                        case 'd':
                            rb.Left++;
                            rb.coordX++;
                            rb.LbBtCtl.Left++;
                            lblCoordinates.Text = rb.Location.ToString();
                            //f_DrawAllConnectors();
                            break;
                    }
                }
            }
        }

        #endregion




        #region Comments

        //private void ToRealSize_Click(object sender, EventArgs e) // Устаревший метод сброса
        //{ 
        //    foreach (KeyValuePair<int, RoundButton> kvp in Program.StationDict)    // заполним окошко с нераскиданными станциями
        //    {
        //        RoundButton CurrStation = kvp.Value;
        //        CurrStation.ScurrX = CurrStation.coordX; CurrStation.ScurrY = CurrStation.coordY; // установим начальные координаты станциям 
        //        CurrStation.Location = new System.Drawing.Point(CurrStation.ScurrX, CurrStation.ScurrY);
        //        CurrStation.LcurrX = CurrStation.LblX; CurrStation.LcurrY = CurrStation.LblY;    // и меткам
        //        if (CurrStation.LbBtCtl != null) CurrStation.LbBtCtl.Location = new System.Drawing.Point(CurrStation.LcurrX, CurrStation.LcurrY);
        //    }
        //    ShftX = 0; ShftY = 0; // сдвига теперь нет, сбросим данные сдвига
        //    kTotal = 1;   // сброс счетчика увеличения
        //    LFontSize = 9.75F;
        //}



        //private void buttonSave_Click(object sender, EventArgs e)  // Сохранение (устаревшее)
        //{
        //    this.HorizontalScroll.Value = 0;
        //    this.HorizontalScroll.Value = 0;
        //    this.VerticalScroll.Value = 0;
        //    this.VerticalScroll.Value = 0;
        //    // Лучше по 2 раза

        //    int iKey;
        //    string lblText = "";
        //    RoundButton tmpButton;
        //    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

        //    if (l_con == null) { MessageBox.Show("Сохранение станций невозможно. Ошибка установления соединения"); return; }
        //    try
        //    {
        //        using (l_con)
        //        {
        //            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
        //            foreach (var rb in Controls.OfType<RoundButton>())  //  
        //            {
        //                iKey = Convert.ToInt32(rb.Name.Substring(2)); // получаем ключ. Имя кнопки = "St" + id, поэтому просто обрезаем "St"
        //                if (iKey > 0)
        //                {
        //                    Program.StationDict.TryGetValue(iKey, out tmpButton);
        //                    if (tmpButton != null)
        //                    {
        //                        cmd.CommandText = "update stations set  coordX = @coordX, coordY = @coordY where id=@id";
        //                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
        //                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", tmpButton.coordX, "int");
        //                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", tmpButton.coordY, "int");
        //                        cmd.ExecuteNonQuery();
        //                        //   if (f_FindStLabel(tmpButton, out lbl) == 1)
        //                        if (tmpButton.LbBtCtl != null)
        //                        {
        //                            lblText = tmpButton.LbBtCtl.Text; if (lblText == null) lblText = "";
        //                            cmd.CommandText = "update labels set coordX = @coordX, coordY = @coordY where station_id=@id";
        //                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
        //                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", tmpButton.LblX, "int");
        //                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", tmpButton.LblY, "int");
        //                            cmd.ExecuteNonQuery();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { MessageBox.Show(" Ошибка сохранения " + ex.Message); }
        //}


        //private void OnLocationChanged(object sender, EventArgs e) // устаревшая смена положения
        //{
        //    Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
        //    Point new_p;
        //    new_p = PointToClient(new Point(MousePosition.X, MousePosition.Y));
        //    X = this.Location.X + (screenRectangle.Top - this.Top); X = this.Location.X + 32;
        //    Y = this.Location.Y + (screenRectangle.Left - this.Left); Y = this.Location.Y + 68;
        //}


        //private void CalcCoord(int X_cent, int Y_cent, int Direction)  // устаревший метод рассчета сдвига с помощью колеса мыши
        //{
        //    // расчет новых координат относительно центра X_cent Y_cent 
        //    // отношение Direction/ ScaleKoeff - дает увеличение или уменьшение.  

        //    double k;
        //    if (Direction > 0) k = 1.2;
        //    else k = 0.8;
        //    kTotal *= k;   // тут копим итоговую величину масштабирования для перерасчета реальных координат
        //    LFontSize = (Single)(LFontSize * k);

        //    this.Invalidate();

        //    foreach (KeyValuePair<int, RoundButton> kvp in Program.StationDict)
        //    {
        //        RoundButton CurrStation = kvp.Value;
        //        if (CurrStation != null)
        //        {
        //            CurrStation.ScurrX = (int)((CurrStation.ScurrX - X_cent) * k) + X_cent;
        //            CurrStation.ScurrY = (int)((CurrStation.ScurrY - Y_cent) * k) + Y_cent;
        //            CurrStation.Location = new System.Drawing.Point(CurrStation.ScurrX, CurrStation.ScurrY);
        //            //  можно увеличивать размеры кнопок, но нужно следить за отрисовкой коннекторов
        //            //    CurrStation.Height = (int)(Program.ButtonSize * kTotal);
        //            //   CurrStation.Width = (int)(Program.ButtonSize * kTotal);
        //            //    CurrStation.StBtCtl.Size = new System.Drawing.Size((int)(Program.ButtonSize*kTotal), (int)(Program.ButtonSize * kTotal));
        //            //    CurrStation.ButtonRoundRadius = (int)(Program.ButtonSize * kTotal);
        //            // теперь подтянем входящие/выходящие коннекторы до нужной точки
        //            if (CurrStation.LbBtCtl != null)   // передвинем метку, если она есть
        //            {
        //                CurrStation.LcurrX = (int)((CurrStation.LcurrX - X_cent) * k) + X_cent;
        //                CurrStation.LcurrY = (int)((CurrStation.LcurrY - Y_cent) * k) + Y_cent;
        //                CurrStation.LbBtCtl.Location = new System.Drawing.Point(CurrStation.LcurrX, CurrStation.LcurrY);
        //                CurrStation.LbBtCtl.Font = new System.Drawing.Font("Microsoft Sans Serif", LFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        //            }

        //        }
        //    }
        //    foreach (LineShape item in shapeContainerR.Shapes.OfType<LineShape>())   // надо учесть смену координат и для маршрута
        //    {
        //        item.X1 = (int)((item.X1 - X_cent) * k) + X_cent;
        //        item.Y1 = (int)((item.Y1 - Y_cent) * k) + Y_cent;
        //        item.X2 = (int)((item.X2 - X_cent) * k) + X_cent;
        //        item.Y2 = (int)((item.Y2 - Y_cent) * k) + Y_cent;
        //    }
        //    this.Refresh();
        //}




        //public void UpdateZoomCoordinates() // Второстепенный метод, рассчитывающий координаты по коэфициентам. Еще может пригодиться
        //{
        //    int iKey;
        //    RoundButton tmpButton;
        //    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

        //    if (l_con == null) { MessageBox.Show("Set станций default невозможно. Ошибка установления соединения"); return; }
        //    try
        //    {
        //        using (l_con)
        //        {
        //            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);

        //            foreach (var rb in Controls.OfType<RoundButton>())  //  
        //            {
        //                iKey = Convert.ToInt32(rb.Name.Substring(2)); // получаем ключ. Имя кнопки = "St" + id, поэтому просто обрезаем "St"
        //                if (iKey > 0)
        //                {
        //                    Program.StationDict.TryGetValue(iKey, out tmpButton);
        //                    if (tmpButton != null)
        //                    {
        //                        for (int i = minZoom; i <= maxZoom; i++)
        //                        {
        //                            cmd.CommandText = "update ZoomCoordinates set coordX = CAST((SELECT coordX from DefaultCoordinates where id = @id) * (SELECT kof from Zoom where zoom = @zoom) AS INT)," +
        //                             "coordY = CAST((SELECT coordY from DefaultCoordinates where id = @id) * (SELECT kof from Zoom where zoom = @zoom) AS INT)," +
        //                                 "lblX = CAST((SELECT lblX from DefaultCoordinates where id = @id) * (SELECT kof from Zoom where zoom = @zoom) AS INT)," +
        //                                 "lblY = CAST((SELECT lblY from DefaultCoordinates where id = @id) * (SELECT kof from Zoom where zoom = @zoom) AS INT)," +
        //                                 "size = (SELECT size from Zoom where zoom = @zoom)," +
        //                                 "radius = (SELECT radius from Zoom where zoom = @zoom)," +
        //                                 "font = (SELECT font from Zoom where zoom = @zoom)" +
        //                                 "where zoom = @zoom and id = @id;";

        //                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", tmpButton.st_id, "int");
        //                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@zoom", i, "int");
        //                            cmd.ExecuteNonQuery();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    { MessageBox.Show("Ошибка set default: " + ex.Message); }
        //}









        //private void OnMouseDown(object sender, MouseEventArgs e)
        //{
        //    if (sender is RoundButton)
        //    {
        //        rb = sender as RoundButton;
        //        tBEditor.Text = rb.Name;
        //        lblCoordinates.Text = rb.Location.ToString();
        //        rb.Focus();
        //    }
        //}
        //private void OnMouseUp(object sender, MouseEventArgs e) { }
        //private void OnMouseMove(object sender, MouseEventArgs e) { }
        //private void OnMouseWheel(object sender, MouseEventArgs e) { }

        //     int         li_rc;
        //   ArrayList UncStArray = new ArrayList();

        //       foreach (KeyValuePair<int, Program.StationBase > kvp in Program.UncStDict)    // заполним окошко с нераскиданными станциями
        //       {
        //            Program.StationBase CurrStation = kvp.Value;
        //            if (CurrStation != null)
        //            {
        //            
        //            UncStArray.Add(new  CurrStation(st_id, CurrStation.st_name));
        //           }
        //       }

        //    lbox_Stations.BeginUpdate();
        //    lbox_Stations.DataSource = new BindingSource(Program.UncStDict, null);
        //    lbox_Stations.DisplayMember = "Value";  // отображаем название станции
        //    lbox_Stations.ValueMember   = "Key"  ;  // получаем ее код при выделении (для поиска)
        //    lbox_Stations.EndUpdate();













        //if (tmpButton.st_id == 149)
        //{
        //     int www = 229;
        //}


        //if (isStart)
        //{
        //    if (line.X1 >= tmpButton.coordX && line.X1 <= tmpButton.coordX + tmpButton.Width && line.Y1 >= tmpButton.coordY && line.Y1 <= tmpButton.coordY + tmpButton.Height) // Point 1
        //    {
        //        line.X1 = x + tmpButton.Width / 2;
        //        line.Y1 = y + tmpButton.Height / 2;
        //    }

        //    if (line.X2 >= tmpButton.coordX && line.X2 <= tmpButton.coordX + tmpButton.Width && line.Y2 >= tmpButton.coordY && line.Y2 <= tmpButton.coordY + tmpButton.Height) // Point 2
        //    {
        //        line.X2 = x + tmpButton.Width / 2;
        //        line.Y2 = y + tmpButton.Height / 2;
        //    }
        //}
        //else
        //{
        //    int deltax = line.X2 - line.X1;
        //    int deltay = line.Y2 - line.Y1;


        //    if (line.X1 >= tmpButton.coordX && line.X1 <= tmpButton.coordX + tmpButton.Width && line.Y1 >= tmpButton.coordY && line.Y1 <= tmpButton.coordY + tmpButton.Height) // Point 1
        //    {
        //        line.X1 = x + tmpButton.Width / 2;
        //        line.Y1 = y + tmpButton.Height / 2;
        //        line.X2 = line.X1 + deltax;
        //        line.Y2 = line.Y1 + deltay;
        //    }
        //    else
        //    if (line.X2 >= tmpButton.coordX && line.X2 <= tmpButton.coordX + tmpButton.Width && line.Y2 >= tmpButton.coordY && line.Y2 <= tmpButton.coordY + tmpButton.Height) // Point 2
        //    {
        //        line.X2 = x + tmpButton.Width / 2;
        //        line.Y2 = y + tmpButton.Height / 2;
        //        line.X1 = line.X2 + deltax;
        //        line.Y1 = line.Y2 + deltay;
        //    }
        //    else
        //    {
        //        //MessageBox.Show("Zoom Problemz");
        //    }
        //}




        //if (tmpButton.st_id == 149)
        //{
        //    Debug.WriteLine("\nZooming: " + tmpButton.st_name);
        //    foreach (var item in tmpButton.ConLine)
        //    {
        //        if (item != null)
        //            Debug.WriteLine(item.X1 + " " + item.Y1 + " " + item.X2 + " " + item.Y2 + " ");
        //    }

        //}


        //public void MoveShapes()
        //{
        //    var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);

        //    if (l_con == null) { MessageBox.Show("Движение коннекторов невозможно. Ошибка установления соединения"); return; }
        //    try
        //    {
        //        using (l_con)
        //        {
        //            var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
        //            List<int> from = new List<int>();
        //            List<int> to = new List<int>();

        //            cmd.CommandText = "select st_id_from, st_id_to from Graph";
        //            System.Data.Common.DbDataReader reader = prvCommon.f_GetDataReader(prvCommon.curDB, cmd, l_con);
        //            if (reader.HasRows) // если есть данные
        //            {
        //                while (reader.Read()) // построчно считываем данные 
        //                {
        //                    from.Add(reader.GetInt32(0));
        //                    to.Add(reader.GetInt32(1));
        //                }
        //            }
        //            reader.Close();

        //        }
        //    }
        //    catch (Exception ex)
        //    { MessageBox.Show(" Ошибка движения коннекторов " + ex.Message); }



        //    foreach (LineShape item in shapeContainerR.Shapes.OfType<LineShape>())
        //    {
        //        item.X1 += X_move;
        //        item.X2 += X_move;
        //        item.Y1 += Y_move;
        //        item.Y2 += Y_move;
        //    }
        //}
        //btnWait = new Button()
        //{
        //    Location = new Point(0, 0),
        //    Width = this.Width,
        //    Height = this.Height,
        //    Font = new Font("Century Gothic", 20F, FontStyle.Regular, GraphicsUnit.Point, 0),
        //    Text = "Wait",
        //    Visible = false,
        //};

        //Controls.Add(btnWait);


        //// посчитаем сдвиг по X и Y для передачи в RoundButton
        //screenRectangle = RectangleToScreen(this.ClientRectangle);
        //Point new_p;
        //new_p = PointToClient(new Point(MousePosition.X, MousePosition.Y));
        //X = this.Location.X + (screenRectangle.Top - this.Top); Y = this.Location.Y + (screenRectangle.Left - this.Left);
        //X = this.Location.X + 20; Y = this.Location.Y + 39;
        //UpdateZoomCoordinates();  


        #endregion

    }
}