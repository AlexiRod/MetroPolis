using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SQLite;
using System.Data.Common;

using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace Metropolis
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //    f_ReadLines(LineFile);
            f_ReadCfg(ConfigFile);  // считаем настройки из файла конфигурации по маршруту ConfigFile
            f_ReadLinesDB();
            f_ReadStationsDB();
            f_ReadConnectorsDB();  // Дийкстра. эта процедура заполняет ребра графа
            string s_Str;
            if (DbTables.f_GetSetupValue("lang", out s_Str) > 0) Lang = s_Str;  // определим установленный язык
         //   f_ReadStations(StationFile);   // прочитаем файлы данных
                   Application.Run(new SchemeEditor());

        }

        static public string ConfigFile = ".\\metropolis.cfg";
        static public string DbName = ".\\TranspBase.sqlite";
        static public string StationFile = "c:\\distr\\stations.txt";
        static public string LineFile = "c:\\distr\\lines.txt";
        static public int ButtonSize = 20;          // диаметр станционного кружка. Нужен для коррекции сдвига линий.
        static public int ButtonRndRadius = 10;     // радиус закругления станционного кружка.
        static public bool EditMode = false;        // в режиме редактирования (true) можем менять карту
        static public string Lang = "Ru";           // по умолчанию язык в ситеме русский

        static public Graph g = new Graph();       // создадим статичный экземпляр графа

        public class LineBase
        {

            public int line_id;            // id линии
            public int line_style;         // стиль линии (пока не исп.)
            public Color line_color;       // цвет линии. по названию определяем цвет в f_DefColor()     
            public string line_name;       // название линии
 
        }

        static public Dictionary<int, LineBase> LineDict        = new Dictionary<int, LineBase>();
        static public Dictionary<int, RoundButton> StationDict  = new Dictionary<int, RoundButton>();

        // эти словари для поиска линий, которые нужно подвинуть, если подвинули станцию или сменили масштаб
        //static public Dictionary<RoundButton, StConnector> StationFrom = new Dictionary<RoundButton, StConnector>();
        //static public Dictionary<RoundButton, StConnector> StationTo   = new Dictionary<RoundButton, StConnector>();


        public static int f_ReadCfg(string as_FileName)
        // Чтение параметров конфигурации из текстового файла. 
        {
            int li_rc, li_int;
            string s_FileName = as_FileName, CfgStr = "";
            ArrayList arrText = new ArrayList();

            if (as_FileName != null && as_FileName.Length > 0) { s_FileName = as_FileName; }
            li_rc = Program.f_ReadTxtFile(s_FileName, arrText);
            if (li_rc == 1)
            {
                foreach (string sOutput in arrText)
                {
                    if (sOutput.Length > 0) CfgStr = (sOutput.Substring(0, sOutput.IndexOf('=') - 1));
                    if (CfgStr.ToUpper() == "STATIONSIZE")
                    {
                        CfgStr = sOutput.Substring(sOutput.IndexOf('=') + 1, sOutput.IndexOf(';') - sOutput.IndexOf('=') - 1);
                        if (Int32.TryParse(CfgStr, out li_int)) ButtonSize = li_int;
                    }
                    if (sOutput.Length > 0) CfgStr = (sOutput.Substring(0, sOutput.IndexOf('=') - 1));
                    if (CfgStr.ToUpper() == "STATIONRADIUS")
                    {
                        CfgStr = sOutput.Substring(sOutput.IndexOf('=') + 1, sOutput.IndexOf(';') - sOutput.IndexOf('=') - 1);
                        if (Int32.TryParse(CfgStr, out li_int)) ButtonRndRadius = li_int;
                    }
                //   if (CfgStr.ToUpper() == "SCALEINC")
                //    {
                //        CfgStr = sOutput.Substring(sOutput.IndexOf('=') + 1, sOutput.IndexOf(';') - sOutput.IndexOf('=') - 1);
                //        if (Int32.TryParse(CfgStr, out li_int)) ScaleInc = li_int;
                //    }
                }
            }
            return li_rc;
        } // f_ReadCfg
        public static int f_ReadConnectorsDB()  // Дийкстра. эта процедура заполняет ребра графа
        {
            var connection = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (connection != null)
            {
                using (connection)
                {
                    int st_id_from, st_id_to, time, line_from, line_to;
                    string expr = "select st_id_from, st_id_to, time, line_id, line2 from ConnectStations";
                    var command = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, connection);
                    command.CommandText = expr;

                    var rd = prvCommon.f_GetDataReader(prvCommon.curDB, command, connection);
                    if (rd.HasRows) // если в таблице базы есть данные, то начнем зачитывать
                    {
                        while (rd.Read()) // построчно считываем данные
                        { 
                            st_id_from = rd.GetInt32(0);      
                            st_id_to = rd.GetInt32(1);   
                            time = rd.GetInt32(2);
                            line_from = rd.GetInt32(3) * 10000 + st_id_from;
                            line_to = rd.GetInt32(4) * 10000 + st_id_to;
                            g.AddEdge(line_from.ToString(), line_to.ToString(), time); // Дийкстра. добавим ребраю time - вес                                          // Добавим эту станцию (вершину) для алгоритма Дийкстры
                        }
                    }
                    rd.Close();
                    connection.Close();
                }
                return 1;
            }
            return -1;
        }
        public static int f_ReadStationsDB()
        {
            var connection = prvCommon.f_GetDBConnection( prvCommon.curDB );
            if (connection != null)
            { 
                using (connection)
                {
                    string expr = "select * from StLb";
                    var command = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, connection);
                    command.CommandText = expr;
            
                    var rd = prvCommon.f_GetDataReader(prvCommon.curDB, command, connection);
                    if (rd.HasRows) // если в таблице базы есть данные, то начнем зачитывать
                    {
                        while (rd.Read()) // построчно считываем данные
                        {
                            //    object id = SQLiteReader.GetValue(0);
                            RoundButton CurrStation = new RoundButton();               // создадим станцию и зададим значения экземпляра
                            CurrStation.st_id = rd.GetInt32(0);       // Convert.ToInt32(words[0]);
                            CurrStation.st_line_id = rd.GetInt32(1);   // Convert.ToInt32(words[1]);
                            CurrStation.UniqueId = CurrStation.st_line_id * 10000 + CurrStation.st_id;
                            CurrStation.st_name = rd.GetString(2); // words[2];
                            CurrStation.coordX = rd.GetInt32(3);  // Convert.ToInt32(words[3]);
                            CurrStation.coordY = rd.GetInt32(4);  // Convert.ToInt32(words[4]);
                            CurrStation.ScurrX = CurrStation.coordX; // приравняем координаты схемы и координаты на экране, т.к.
                            CurrStation.ScurrY = CurrStation.coordY;  // пока не было мастабирования и они равны
                            if (rd.GetString(6).Length > 0)
                            {
                                CurrStation.lbl_name = rd.GetString(6);
                                if (rd.GetInt32(7) != 0 && rd.GetInt32(8) != 0)
                                {
                                    CurrStation.LblX = rd.GetInt32(7);CurrStation.LblY = rd.GetInt32(8);
                                    CurrStation.LcurrX = CurrStation.LblX; CurrStation.LcurrY = CurrStation.LblY;
                                }
                            }
                            StationDict.Add(CurrStation.UniqueId, CurrStation);   // заполним словарь ссылкой на экземпляр 
   // if (CurrStation.UniqueId>10010 && CurrStation.UniqueId<10016)  // ЭТО УБРАТЬ ПОСЛЕ СОЕДИНЕИЯ ВСЕЙ СХЕМЫ
                            g.AddVertex(CurrStation.UniqueId.ToString());         // Дийкстра. станция - это вершина графа для Дийкстры                                          // Добавим эту станцию (вершину) для алгоритма Дийкстры
                        }
                    }
                    rd.Close();
                    connection.Close();
                }
                return 1;
            }
            return -1;
        }
        public static int f_ReadLinesDB()
        {
            //SQLiteConnection connection = f_getConnection();
            //string expr = "select l.id, l.type_id, c.name, l.name from Lines l, Colors c where l.Color_id = c.id";
            //   SQLiteCommand command = new SQLiteCommand( "select * from Stations", connection);
            // SQLiteCommand command = new SQLiteCommand(expr, connection);
            // SQLiteDataReader rd = command.ExecuteReader();
            //command.ExecuteReader();
            // текст базовой вьюшки
            // create view StLb as select s.id,s.Line_id,s.name, c.name clr_name, s.coordx,s.coordy, l.name, l.coordx,l.coordY 
            //  from Stations s, Colors c, Lines Ln
            //  Left JOIN Labels l on s.id = l.Station_id
            //  where s.line_id = ln.id and ln.color_id = c.id



            var connection = prvCommon.f_GetDBConnection(prvCommon.curDB);
            string expr = "select l.id, l.type_id, c.name, l.name from Lines l, Colors c where l.Color_id = c.id";
            var command = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, connection);
            command.CommandText = expr;
 
            var rd = prvCommon.f_GetDataReader(prvCommon.curDB, command, connection);
            using (connection)
            {
                if (rd.HasRows) // если в таблице базы есть данные, то начнем зачитывать    
                {
                    while (rd.Read())
                    {
                        LineBase CurrLine = new LineBase();               // создадим станцию и зададим значения экземпляра
                        CurrLine.line_id = rd.GetInt32(0);
                        CurrLine.line_style = rd.GetInt32(1);
                        CurrLine.line_color = f_DefColor(rd.GetString(2));
                        CurrLine.line_name = rd.GetString(3);
                        LineDict.Add(CurrLine.line_id, CurrLine);   // заполним словарь ссылкой на экземпляр 
                    }
                }
            }
             //   rd.Close();
             //   connection.Close();
            return 1;
        } // f_ReadLinesDB



        public static int f_ReadStations(string as_FileName)
        // это вариант чтения станций из текстового файла. сейчас не используется
        { 
            int li_rc;
            string s_FileName = StationFile;
            ArrayList arrText = new ArrayList();

            if (as_FileName != null && as_FileName.Length > 0)  { s_FileName = as_FileName; }
            li_rc = Program.f_ReadTxtFile(s_FileName, arrText);
            if (li_rc == 1)
            {
                foreach (string sOutput in arrText)
                {           // сюда бы потом воткнуть проверку пустых значений, чтоб не свалиться
                    string[] words = sOutput.Split('\x9');
                    RoundButton CurrStation = new RoundButton();               // создадим станцию и зададим значения экземпляра
                    CurrStation.st_id       = Convert.ToInt32(words[0]);       
                    CurrStation.st_line_id  = Convert.ToInt32(words[1]);
                    CurrStation.UniqueId = CurrStation.st_line_id * 10000 + CurrStation.st_id;
                    CurrStation.st_name = words[2];
                    if (words.GetUpperBound(0) >= 4)                               // если координаты станции заданы
                    {
                        CurrStation.coordX = Convert.ToInt32(words[3]);
                        CurrStation.coordY = Convert.ToInt32(words[4]);
                        CurrStation.ScurrX  = Convert.ToInt32(words[3]); // приравняем координаты схемы и координаты на экране, т.к.
                        CurrStation.ScurrY = Convert.ToInt32(words[4]);  // пока не было мастабирования и они равны
                    }
                    if (words.GetUpperBound(0) >= 6)                               // если заданы координаты метки
                    {
                        CurrStation.LblX = Convert.ToInt32(words[5]);
                        CurrStation.LblY = Convert.ToInt32(words[6]);
                        CurrStation.LcurrX = Convert.ToInt32(words[5]);
                        CurrStation.LcurrY = Convert.ToInt32(words[6]);
                    }
                    if (words.GetUpperBound(0) >= 7)
                            CurrStation.lbl_name = words[7];
                    else    CurrStation.lbl_name = "" ;  // если название станции не задано 

                    StationDict.Add(CurrStation.UniqueId, CurrStation);   // заполним словарь ссылкой на экземпляр 
                }
            }
            else
            {
                MessageBox.Show("Не удалось считать файл станций");
            }
            return li_rc;
         } // f_ReadStatiobs


        public static int f_ReadTxtFile(string as_FileName, ArrayList arrText)
        // функция читает текстовый файл и записывает его в массив
        // возврат: - код ошибки или успешное чтение = 1
        //          -1 - файл не найден
        {
            if (as_FileName == null) return 0;
            int li_rc = -2;

            FileInfo fileInf = new FileInfo(as_FileName);
            if (fileInf.Exists)
            {
                try
                {

                    StreamReader objReader = new StreamReader(as_FileName, System.Text.Encoding.Default);
                    string sLine = "";
                    // ArrayList arrText = new ArrayList();

                    while (sLine != null)
                    {
                        sLine = objReader.ReadLine();
                        if (sLine != null)
                            arrText.Add(sLine);
                    }
                    objReader.Close();
                    li_rc = 1;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка обработки файла " + as_FileName, e.Message);
                }
            }
            else { return -1; }
            return li_rc;
        }   // f_ReadTxtFile

        public static int f_ReadImgFile(string as_FileName,ref Image a_Img)
        // функция читает img файл и вертает его в ref
        // возврат: - код ошибки или успешное чтение = 1
        //          -1 - файл не найден
        {
            if (as_FileName == null) return -1;
            int li_rc = -1;

            FileInfo fileInf = new FileInfo(as_FileName);
            if (fileInf.Exists)
            {
                try
                {
                    a_Img = Image.FromFile(as_FileName);
                    li_rc = 1;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка обработки файла " + as_FileName, e.Message);
                }
            }
            else { return li_rc; }
            return li_rc = 1;
        }   // f_ReadImgFile

        public static int f_WriteTxtFile(string as_FileName, ArrayList arrText, bool ab_DontRewrite)
        // функция записывает текстовый файл и записывает его в массив
        // ab_DontRewrite = false - файл будет ПЕРЕЗАПИСАН с нуля. true - дописывание в файл 
        // возврат: - код ошибки или успешное чтение = 1
        //          -1 - файл не найден
        {


            if (as_FileName == null) return 0;
            int li_rc = -2;

            FileInfo fileInf = new FileInfo(as_FileName);
            if (fileInf.Exists)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(as_FileName, ab_DontRewrite, System.Text.Encoding.UTF8))
                    {
                        foreach (string text in arrText)
                        {
                            if (text != null) sw.WriteLine(text);
                        }

                    }
                    li_rc = 1;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка обработки файла " + as_FileName, e.Message);
                }
            }
            else { return -1; }
            return li_rc;
        }   // f_WriteTxtFile
 
        public static int f_ReadLines(string as_FileName)
        {
            int li_rc;
            string s_FileName = StationFile;
            ArrayList arrText = new ArrayList();

            if (as_FileName != null && as_FileName.Length > 0) { s_FileName = as_FileName; }
            li_rc = Program.f_ReadTxtFile(s_FileName, arrText);
            if (li_rc == 1)
            {
                foreach (string sOutput in arrText)
                {           // сюда бы потом воткнуть проверку пустых значений, чтоб не свалиться
                    string[] words = sOutput.Split('\x9');
                    LineBase CurrLine = new LineBase();               // создадим станцию и зададим значения экземпляра
                    CurrLine.line_id = Convert.ToInt32(words[0]);
                    CurrLine.line_style = Convert.ToInt32(words[1]);
                    CurrLine.line_color = f_DefColor(words[2]);
                    CurrLine.line_name = words[3];
                    LineDict.Add(CurrLine.line_id, CurrLine);   // заполним словарь ссылкой на экземпляр 
                }
            }
            else
            {
                MessageBox.Show("Не удалось считать файл станций");
            }
            return li_rc;
        } // f_ReadLines
        public static Color f_DefColor(string as_Color)
        {  // определим цвет по названию
            switch (as_Color)
            {
                case "Gold": return Color.Gold;
                case "Gray": return Color.Gray;
                case "DeepPink": return Color.DeepPink;
                case "DarkViolet": return Color.DarkViolet;
                case "DarkSeaGreen": return Color.DarkSeaGreen;
                case "DarkRed": return Color.DarkRed;
                case "DarkOrange": return Color.DarkOrange;
                case "DarkOliveGreen": return Color.DarkOliveGreen;
                case "DarkMagenta": return Color.DarkMagenta;
                case "DarkGreen": return Color.DarkGreen;
                case "DarkGray": return Color.DarkGray;
                case "DarkGoldenrod": return Color.DarkGoldenrod;
                case "DarkCyan": return Color.DarkCyan;
                case "DarkBlue": return Color.DarkBlue;
                case "Cyan": return Color.Cyan;
                case "Coral": return Color.Coral;
                case "Chocolate": return Color.Chocolate;
                case "Brown": return Color.Brown;
                case "Black": return Color.Black;
                case "Green": return Color.Green;
                case "GreenYellow": return Color.GreenYellow;
                case "LightBlue": return Color.LightBlue;
                case "LightCyan": return Color.LightCyan;
                case "LightGoldenrodYellow": return Color.LightGoldenrodYellow;
                case "LightGray": return Color.LightGray;
                case "LightGreen": return Color.LightGreen;
                case "LightYellow": return Color.LightYellow;
                case "Lime": return Color.Lime;
                case "LimeGreen": return Color.LimeGreen;
                case "Magenta": return Color.Magenta;
                case "MediumPurple": return Color.MediumPurple;
                case "Orange": return Color.Orange;
                case "OrangeRed": return Color.OrangeRed;
                case "Pink": return Color.Pink;
                case "Plum": return Color.Plum;
                case "Purple": return Color.Purple;
                case "Red": return Color.Red;
                case "Silver": return Color.Silver;
                case "SkyBlue": return Color.SkyBlue;
                case "Transparent": return Color.Transparent;
                case "Violet": return Color.Violet;
                case "Yellow": return Color.Yellow;
                case "YellowGreen": return Color.YellowGreen;
                case "White": return Color.White;
                default: return Color.LightPink;    // не нашли подходящий - будет LightPink
            }
        } // f_DefColor
    }   
    
}           // namespase
 
