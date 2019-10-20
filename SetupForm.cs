using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metropolis
{
    public partial class SetupForm : System.Windows.Forms.Form
    {
        public SetupForm()
        {
            InitializeComponent();
            if (Program.EditMode)  EditModeCBox.Checked = true;
        }



        private string SelectFile()
        {

            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            // folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                //   string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                
            }
            if (folderBrowser.FileName == null || folderBrowser.FileName.Length < 1)
                return String.Empty;
            else return folderBrowser.FileName;
        }

        private void ExpStations_Click_1(object sender, EventArgs e)
        {
            XmlTextWriter textWriter = new XmlTextWriter(".\\StationXmFile.xml", null);
            textWriter.WriteStartDocument();
            textWriter.WriteComment("Выгрузка станций от " + DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
            var connection = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (connection != null)
            {
                using (connection)
                {
                    string expr = "select id, line_id, line_id_inner,name,coordX,coordY,type,cities_id,status, "
                       +  " coalesce((select text from InfoTable where st_id = s.id and line_id = s.line_id and lang = \'Ru\'),\".\") RuTxt ,"
                       +  " coalesce((select text from InfoTable where st_id = s.id and line_id = s.line_id and lang = \'En\'),\".\") EnTxt "
                       + "from Stations s";

                    var command = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, connection);
                    command.CommandText = expr;

                    var rd = prvCommon.f_GetDataReader(prvCommon.curDB, command, connection);
                    if (rd.HasRows) // если в таблице базы есть данные, то начнем зачитывать
                    {
                        textWriter.WriteStartElement("root");
                        while (rd.Read()) // построчно считываем данные и пишем в файл
                        {
                            textWriter.WriteString("\r\n");
                            textWriter.WriteStartElement("Станция");
                            textWriter.WriteString("\r\n\t");
                            //---------------------------------
                            textWriter.WriteElementString( "id", rd.GetInt32(0).ToString());
                                    textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("line_id", rd.GetInt32(1).ToString());
                                if (rd.GetValue(2).ToString().Length > 0)
                                    textWriter.WriteElementString("line_id_inner", rd.GetInt32(2).ToString());
                                else
                                    textWriter.WriteElementString("line_id_inner", "0");
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("name", rd.GetString(3));
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("coordX", rd.GetInt32(4).ToString());
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("coordY", rd.GetInt32(5).ToString());
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("type", rd.GetInt32(6).ToString());
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("cities_id", rd.GetInt32(7).ToString());
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("status", rd.GetInt32(8).ToString());
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("RuTxt", rd.GetString(9).ToString());
                                textWriter.WriteString("\r\n\t");
                                textWriter.WriteElementString("EnTxt", rd.GetString(10).ToString());
                                textWriter.WriteString("\r\n");
                            //---------------------------------
                            textWriter.WriteEndElement();  // Станция
                            textWriter.WriteString("\r\n");

                        }
                        textWriter.WriteEndElement();  // root
 
                    }
                    rd.Close();
                    connection.Close();
                }
               // return 1;
            }

 
            // Ends the document.
            textWriter.WriteEndDocument();
            // close writer
            textWriter.Close();
        }

        private void ImpStationsExp()  // импорт расширенной информации о станции
        {
            string filename = String.Empty;
            string line_name = String.Empty;
            int id = 0, line_id=0, line_id_inner, RuCnt=0,EnCnt=0;
            string  name, RuTxt = String.Empty, EnTxt = String.Empty;
            int coordX, coordY, type, cities_id, status;
            string errMsg = String.Empty;

            filename = SelectFile();   // выбираем файл для импорта. там должна быть версия xml 1.0!!!

            XmlDocument xDoc = new XmlDocument();
            if (filename.Length < 1 || filename == String.Empty) { MessageBox.Show("Не выбрали файл. Импорт невозможен"); return;}
            xDoc.Load(filename);
            XmlElement xRoot = xDoc.DocumentElement;            // получим корневой элемент
            foreach (XmlNode xnode in xRoot)            // обход всех узлов в корневом элементе
            {
                bool b_Ok = true;
                foreach (XmlNode childnode in xnode.ChildNodes) // обходим все дочерние узлы элемента 
                {
                    if (childnode.Name == "id")     // если узел - id
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out id);
                        if (!b_Ok) { errMsg = "Неверный Id для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "line_id" )     //  
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out line_id);
                        if (!b_Ok) { errMsg = "Неверный line_id для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "line_id_inner")     //  
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out line_id_inner);
                        if (!b_Ok) { errMsg = "Неверный line_id_inner для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "name")
                    {
                        name = childnode.InnerText;
                        if (name.Length == 0)
                        {
                            b_Ok = false;
                            errMsg = "Пустое наименование станции для для узла " + xnode.InnerText + "\r\n"; break;
                        }
                    }
                    if (childnode.Name == "coordX")     //  
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out coordX);
                        if (!b_Ok) { errMsg = "Неверный coordX для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "coordY")     //  
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out coordY);
                        if (!b_Ok) { errMsg = "Неверный coordY для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "type")     //  
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out type);
                    if (!b_Ok) { errMsg = "Неверный type для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "cities_id")     //  
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out cities_id);
                        if (!b_Ok) { errMsg = "Неверный cities_id для узла " + xnode.InnerText + "\r\n"; break; }
                    }
                    if (childnode.Name == "RuTxt") RuTxt = childnode.InnerText;
                    if (childnode.Name == "EnTxt") EnTxt = childnode.InnerText;
                    if (childnode.Name == "status")     // если узел - ID
                    {
                        if (childnode.InnerText == null || childnode.InnerText.Length < 1) status = 1;   // если не определили, то действующая
                        else
                        {
                            b_Ok = Int32.TryParse(childnode.InnerText, out status);
                            if (!b_Ok) { errMsg = "Неверный status для узла " + xnode.InnerText + "\r\n"; break; }
                        }
                    }
                    //    if (b_Ok && xnode.Name == "array") InsertLines(id, 1, 1, line_name, status);
                }
                if (!b_Ok) MessageBox.Show(errMsg);
                else
                {
                    // данные считали, теперь можно их закинуть в таблицу станций и в InfoTable, если есть RuTxt или EnTxt
                    // пока станции пропустим, для них другой механизЬм работает, а вот описания закинем
                    if (RuTxt.Length > 1)
                        if (DbTables.Up_InfoTable(line_id, id, 0 /*type*/, RuTxt, "Ru") > 0) RuCnt++;
                    if (EnTxt.Length > 1)
                        if (DbTables.Up_InfoTable(line_id, id, 0 /*type*/, EnTxt, "En") > 0) EnCnt++;
                }
            }
            MessageBox.Show("Импортировано " + RuCnt.ToString() + " русских и " + EnCnt.ToString() +" английских описаний станций");
        }
 
        private void ImpLines_Click(object sender, EventArgs e)
        {
            string filename = String.Empty;
            string line_name = String.Empty;
            int id = 0, status = 1;

            filename = SelectFile();   // выбираем файл для импорта. там должна быть версия xml 1.0!!!

            XmlDocument xDoc = new XmlDocument();
            if (filename.Length < 1 || filename == String.Empty)
            {
                MessageBox.Show("Не выбрали файл. Импорт невозможен");
                return;
            }
            xDoc.Load(filename);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                bool b_Ok = true;
                foreach (XmlNode childnode in xnode.ChildNodes) // обходим все дочерние узлы элемента 
                {
                    string errMsg = String.Empty;

                    if (childnode.Name == "ID")     // если узел - ID
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out id);
                        if (!b_Ok) errMsg = "Неверный Id для узла " + xnode.InnerText + "\r\n";
                    }
                    if (childnode.Name == "Line")     // если узел - ID
                    {
                        line_name = childnode.InnerText;
                        if (line_name == null || line_name.Length < 1)
                        {
                            b_Ok = false;
                            errMsg = "Неверное назв.линии для узла " + xnode.InnerText + "\r\n";
                        }
                    }
                    if (childnode.Name == "Status")     // если узел - ID
                    {
                        if (childnode.InnerText == null || childnode.InnerText.Length < 1)
                        {
                            status = 1;   // если не определили, то действующая
                        }
                        else if (childnode.InnerText != "действует") status = 0;
                    }
                }
                if (b_Ok && xnode.Name == "array" ) InsertLines(id, 1, 1, line_name, status);
            }
        }

        private void InsertLines(int l_id, int l_type_id, int l_color_id, string l_name, int l_status)
        {
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con != null)
            {
                using (l_con)
                {
                    try
                    {
                        // SQLiteCommand cmd = new SQLiteCommand(l_con);
                        var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                        bool    b_Ok = false;
         
                        // проверим, что такой линии нет
                        cmd.CommandText = "select count() from lines ln, setup su where id = @l_id and ln.cities_id = su.cities_id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@l_id", (object)l_id, "int"); //(int)DbType.Int32);
                        object cnt = cmd.ExecuteScalar();
                        if (cnt != null && Convert.ToInt32(cnt.ToString()) > 0)
                            MessageBox.Show( "Импорт невозможен. Есть линия с id=" + l_id.ToString());
                        else b_Ok = true;

                        if (b_Ok)
                        {
                            cmd.CommandText = "insert into lines(id,type_id,name,color_id, cities_id, status) " +
                                "Values(@id,@type_id,@name,@color_id, (select max(cities_id) from setup), @status)";
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type_id", l_type_id, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@color_id", l_color_id, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", l_name, "string");
                      //      prvCommon.f_AddParm(prvCommon.curDB, cmd, "@cities_id", "", "string");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@status", l_status, "int");
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(" Ошибка импорта " + ex.Message);
                    }
                    finally { }
                }

            }
        }

        private void ImpStations_click(object sender, EventArgs e)
        {
            string filename = String.Empty;
            string st_name = String.Empty;
            string line_name = String.Empty;
            int id = 0, status = 1;

            filename = SelectFile();   // выбираем файл для импорта. там должна быть версия xml 1.0!!!

            XmlDocument xDoc = new XmlDocument();
            if (filename.Length < 1 || filename == String.Empty)
            {
                MessageBox.Show("Не выбрали файл. Импорт невозможен");
                return;
            }
            xDoc.Load(filename);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                bool b_Ok = true;
                foreach (XmlNode childnode in xnode.ChildNodes) // обходим все дочерние узлы элемента 
                {
                    string errMsg = String.Empty;

                     if (childnode.Name == "ID")     // если узел - ID
                    {
                        b_Ok = Int32.TryParse(childnode.InnerText, out id);
                        if (!b_Ok) errMsg = "Неверный Id для узла " + xnode.InnerText + "\r\n";
                    }
                    if (childnode.Name == "Line")     // если узел - ID
                    {
                        line_name = childnode.InnerText;
                        if (line_name == null || line_name.Length < 1)
                        {
                            b_Ok = false;
                            errMsg = "Неверное назв.линии для узла " + xnode.InnerText + "\r\n";
                        }
                    }
                    if (childnode.Name == "Station")     // если узел - ID
                    {
                        st_name = childnode.InnerText;
                        if (st_name == null || st_name.Length < 1)
                        {
                            b_Ok = false;
                            errMsg = "Неверное назв.станции для узла " + xnode.InnerText + "\r\n";
                        }
                    }
                    ProgressLabel.Text = "В обработке станция " + childnode.InnerText;

                    if (childnode.Name == "Status")     // если узел - ID
                    {
                        if (childnode.InnerText == null || childnode.InnerText.Length < 1)
                        {
                            status = 1;   // если не определили, то действующая
                        }
                        else if (childnode.InnerText != "действует") status = 0;
                    }
                }
                if (b_Ok && xnode.Name == "array") InsertStations(id, line_name, st_name, status);
            }
            ProgressLabel.Text = "Усе!";
        }
        private void InsertStations(int l_id, string line_name, string st_name, int status)
        {
            int l_line_id = -1;
            int l_coordX , l_coordY;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con != null)
            {
                using (l_con)
                {
                    try
                    {
                        // SQLiteCommand cmd = new SQLiteCommand(l_con);
                        var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                        bool b_Ok = false;

                        // проверим, что такой станции нет
                        cmd.CommandText = "select count() from stations ln, setup su where id = @l_id and ln.cities_id = su.cities_id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@l_id", (object)l_id, "int"); //(int)DbType.Int32);
                        object cnt = cmd.ExecuteScalar();
                        if (cnt != null && Convert.ToInt32(cnt.ToString()) > 0)
                            MessageBox.Show("Импорт невозможен. Есть станция с id=" + l_id.ToString());
                        else b_Ok = true;
                        // найдем код линии по ее наименованию
                        cmd.CommandText = "select coalesce(ln.id,-1) from setup su, lines ln where ln.name = @line_name and ln.cities_id = su.cities_id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@l_id", (object)l_id, "int"); //(int)DbType.Int32);
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_name", (object)line_name, "string"); //(int)DbType.Int32);
                        cnt = cmd.ExecuteScalar();
                        l_line_id = Convert.ToInt32(cnt.ToString());
                        if (l_line_id < 0)
                            MessageBox.Show("Импорт невозможен. Нет линии с названием=" + line_name);
                        else b_Ok = true;

                        ProgressLabel.Text = "Импорт станции " + st_name + " линия " + l_line_id.ToString();
                        ProgressLabel.Update();

                        if (b_Ok)
                        {
                            int k = 1;   // расположим станции на экране поровнее
                            if (l_id < 30) k = 30;
                            if (l_id >= 30 && k < 60) k = 15;
                            if (l_id >= 60 && k < 90) k = 10;
                            if (l_id >= 90 && k < 120) k = 8;
                            if (l_id >= 120 && k < 150) k = 6;
                            if (l_id >= 150 && k < 180) k = 4;
                            if (l_id >= 180 && k < 260) k = 3;
                            if (l_id >= 260 && k < 320) k = 1;
                            l_coordY = l_id * k; // координаты от дуба, чтобы станции развести на экране
                            l_coordX = l_line_id * 55;
                            // для Москвы сделал примочку на базе координат
                            // select min(y),     max(y) - min(y),   min(x),      max(x)-min(x) from TempCoord
                            //      55.567759   0.327280999999999   37.35909    0.497256999999998
                            cmd.CommandText = "insert into stations(id,line_id,name,coordX,coordY,status,cities_id) " +
                                "Values(@id,@line_id,@name,@coordX,@coordY,@status,(select max(cities_id) from setup))";
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", l_coordX, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", l_coordY, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", l_line_id, "int");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", st_name, "string");
                            prvCommon.f_AddParm(prvCommon.curDB, cmd, "@status", status, "int");
                            cmd.ExecuteNonQuery();
                            if (st_name.Length > 0)  //   имя метки=станции ибо импорт if можно и убрать                            {
                            {  
                                cmd.CommandText = "insert into labels(station_id, name,coordX,coordY,cities_id) " +
                                        "Values(@id,@name,@coordX,@coordY,(select max(cities_id) from setup))";
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", l_id, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", l_coordX + 20, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", l_coordY, "int");
                                prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", st_name, "string");
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(" Ошибка импорта " + ex.Message);
                    }
                    finally { }
                }

            }
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {   // тут разрешаем или запрещаем редактить карту
            if (EditModeCBox.Checked) Program.EditMode = true;
            else Program.EditMode = false;
        }

        private void ImpStationsExpBt_Click(object sender, EventArgs e)
        {
            ImpStationsExp();
        }

        private void St2LangRes_Click(object sender, EventArgs e)
        {
            XmlTextWriter textWriter = new XmlTextWriter(".\\LangResXmFile.xml", null);
            textWriter.WriteStartDocument();     // Opens the document
            textWriter.WriteComment("Выгрузка Labels от " + DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "\r\n");  // Write comments

            var connection = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (connection != null)
            {
                using (connection)
                {
                    // шобы выгрузить шо угодно можно просто поменять sql. Order by, чтобы рядом стояли одинакошные
                    string expr = "select 'Labels','name','En', station_id, name from Labels order by name";
                    var command = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, connection);
                    command.CommandText = expr;
                    var rd = prvCommon.f_GetDataReader(prvCommon.curDB, command, connection);
                    if (rd.HasRows) // если в таблице базы есть данные, то начнем зачитывать
                    {
                        textWriter.WriteStartElement("root");
                        textWriter.WriteString("\r\n");
                        while (rd.Read()) // построчно считываем данные и пишем в файл
                        {
                            textWriter.WriteStartElement("Res");
                                textWriter.WriteElementString("value", rd.GetString(4)); textWriter.WriteString("\t");
                                textWriter.WriteElementString("tbl", rd.GetString(0));   textWriter.WriteString("\t");
                                textWriter.WriteElementString("fld", rd.GetString(1));   textWriter.WriteString("\t");
                                textWriter.WriteElementString("lang", rd.GetString(2));  textWriter.WriteString("\t");
                                textWriter.WriteElementString("id", rd.GetInt32(3).ToString());
                            textWriter.WriteEndElement();
                            textWriter.WriteString("\r\n");  // стрка завершена, сделаем перевод строки
                        }
                        textWriter.WriteEndElement();  // root
                    }
                    rd.Close();
                    connection.Close();
                }
                // return 1;
            }
            textWriter.WriteEndDocument();      // Ends the document.
            textWriter.Close();                 // close writer
        }

        private void LangResImp_Click(object sender, EventArgs e)
        {
            string filename = String.Empty;
            filename = SelectFile();   // выбираем файл для импорта. там должна быть версия xml 1.0!!!

            XmlDocument xDoc = new XmlDocument();
            if (filename.Length < 1 || filename == String.Empty)  { MessageBox.Show("Не выбрали файл. Импорт невозможен"); return;}
            xDoc.Load(filename);    
            XmlElement xRoot = xDoc.DocumentElement;    // получим корневой элемент
            bool b_Ok = true;
            string errMsg = String.Empty;
            string tbl=String.Empty, fld = String.Empty, lang = String.Empty, value = String.Empty;
            int id=0, count = 0;
            foreach (XmlNode xnode in xRoot)     // обход всех узлов в корневом элементе
            {
                foreach (XmlNode childnode in xnode.ChildNodes) // обходим все дочерние узлы элемента 
                {
                    if (childnode.Name == "id")     // если узел - id
                        { b_Ok = Int32.TryParse(childnode.InnerText, out id);
                        if (!b_Ok) { errMsg = "Неверный Id для узла " + xnode.InnerText + "\r\n"; break; }; }  
                    if (childnode.Name == "tbl")    tbl  = childnode.InnerText;
                    if (childnode.Name == "fld")    fld  = childnode.InnerText;
                    if (childnode.Name == "lang")   lang = childnode.InnerText;
                    if (childnode.Name == "value")  value = childnode.InnerText;
                }
                if (DbTables.Up_LangRes(tbl, fld, lang, id, value) < 0)
                {
                    errMsg = "Сохранение в Up_LangRes не прошло. \r\n Параметры: \r\n tbl=" + tbl + "\r\nfld=" + fld
                        + "\r\nlang=" + lang + "\r\nid=" + id.ToString() + "\r\nvalue=" + value;
                    b_Ok = false;
                    break;
                }
                count++;            }
            if (!b_Ok) MessageBox.Show("Импорт завершен неполностью. " + errMsg);
            else MessageBox.Show("Импортировано/изменено " + count.ToString() + " записей");
        }
    }
}
// создание xml-файла
/*
 *             // Create a new file in C:\\ dir
            XmlTextWriter textWriter = new XmlTextWriter(".\\StationXmFile.xml", null);
            // Opens the document
            textWriter.WriteStartDocument();
            // Write comments
            textWriter.WriteComment("Выгрузка станций от " + DateTime.Now.ToString("DD/MM/YYYY hh:mm"));
            // Write first element
            textWriter.WriteStartElement("Student");
            textWriter.WriteStartElement("r", "RECORD", "urn:record");
            // Write next element
            textWriter.WriteStartElement("Name", "");
            textWriter.WriteString("Student");
            textWriter.WriteEndElement();
            // Write one more element
            textWriter.WriteStartElement("Address", ""); textWriter.WriteString("Colony");
            textWriter.WriteEndElement();
            // WriteChars
            char[] ch = new char[3];
            ch[0] = 'a';
            ch[1] = 'r';
            ch[2] = 'c';
            textWriter.WriteStartElement("Char");
            textWriter.WriteChars(ch, 0, ch.Length);
            textWriter.WriteEndElement();
            // Ends the document.
            textWriter.WriteEndDocument();
            // close writer
            textWriter.Close();
 */
