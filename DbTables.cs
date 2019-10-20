using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
//using System.Diagnostics;

namespace Metropolis
{
    class DbTables
    {

        static public int Up_Graph(char Action, int st_from_id, int st_to_id, int GrTime, int GrShow, string desc)
        {
            // добавление/изменение графа. Возврат: -1 - ошибка, 1 - успешно 
            int li_rc = -1;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            {
                MessageBox.Show("Сохранение коннектора невозможно. Ошибка установления соединения"); return li_rc;
            }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    if (Action == 'I')
                    {
                        cmd.CommandText = "insert into graph(st_id_from, st_id_to, time, show, description) " +
                        "Values(@st_from_id, @st_to_id, @time, @show, @desc)";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_from_id", st_from_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_to_id", st_to_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@time", GrTime, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@show", GrShow, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@desc", desc, "string");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else if (Action == 'U')
                    {
                        cmd.CommandText = "update graph set time=@time,show=@show, description=@desc " +
                            "where st_id_from=@st_from_id and st_id_to=@st_to_id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_from_id", st_from_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_to_id", st_to_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@time", GrTime, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@show", GrShow, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@desc", desc, "string");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else
                    {
                        MessageBox.Show("Up_Graph. Неверно указан Action ");
                        return li_rc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Ошибка сохранения " + ex.Message);
                return li_rc;
            }
            return li_rc;
        }

        static public int Up_Station(char Action, int id, int line_id, int line_id_inner, string name,
            int coordX, int coordY, int type, int cities_id, int status, int ways)
        {
            // добавление/изменение станций. Возврат: -1 - ошибка, 1 - успешно 
            int li_rc = -1;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            {
                MessageBox.Show("Сохранение станции невозможно. Ошибка установления соединения"); return li_rc;
            }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    if (Action == 'I')
                    {
                        cmd.CommandText = "insert into Stations(id,line_id,line_id_inner,name," +
                           " coordX, coordY, type, cities_id, status, ways) " +
                            "Values(@id,@line_id,@line_id_inner,@name, " +
                            " @coordX, @coordY, @type, @cities_id, @status, @ways )";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", line_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id_inner", line_id_inner, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", name, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", coordX, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", coordX, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type", type, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@cities_id", cities_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@status", status, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@ways", ways, "int");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else if (Action == 'U')
                    {
                        cmd.CommandText = "update Stations set line_id=@line_id,line_id_inner=@line_id_inner," +
                           "name=@name, coordX=@coordX, coordY=@coordY, type=@type, cities_id=@cities_id" +
                           "status=@status, ways=@ways where id=@id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", line_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id_inner", line_id_inner, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", name, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", coordX, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", coordX, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type", type, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@cities_id", cities_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@status", status, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@ways", ways, "int"); li_rc = 1;
                    }
                    else
                    {
                        MessageBox.Show("Up_Stations. Неверно указан Action ");
                        return li_rc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Up_Stations Ошибка сохранения " + ex.Message);
                return li_rc;
            }
            return li_rc;
        }


//-----------------------------------------------------------------------------------
        static public int Up_Labels(char Action, int station_id, string name, int coordX, int coordY, int cities_id)
        {
            // добавление/изменение графа. Возврат: -1 - ошибка, 1 - успешно 
            int li_rc = -1;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            {
                MessageBox.Show("Сохранение Labels невозможно. Ошибка установления соединения"); return li_rc;
            }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    if (Action == 'I')
                    {
                        cmd.CommandText = "insert into Labels(station_id, name, coordX, coordY, cities_id) " +
                        "Values(@station_id, @name, @coordX, @coordY, @cities_id)";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@station_id", station_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", name, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", coordX, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", coordY, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@cities_id", cities_id, "int");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else if (Action == 'U')
                    {
                        cmd.CommandText = "update Labels set name=@name,coordX=@coordX, coordY=@coordY," +
                            " cities_id=@cities_id where station_id=@station_id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@station_id", station_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@name", name, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordX", coordX, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@coordY", coordY, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@cities_id", cities_id, "int");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else
                    {
                        MessageBox.Show("Up_Labels. Неверно указан Action ");
                        return li_rc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Up_Labels Ошибка сохранения " + ex.Message);
                return li_rc;
            }
            return li_rc;
        }

        //-----------------------------------------------------------------------------------
        // добавление/изменение таблицы языковых ресурсов. Возврат: -1 - ошибка, 1 - успешно 
        // тут мы сначала проверяем наличие записей с такими параметрами и, в зависимости от этого Insert или Update
        // ВАЖНО! Параметр id здесь - это id из таблтицы tbl. Например, передаем value для станции, значит id будет stations.id для этой станции 
        static public int Up_LangRes(string tbl, string fld, string lang, int id, string value)
        {
            int li_rc = -1;
            char Action = 'N';
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (tbl.Length < 1) { MessageBox.Show("Сохранение LangRes невозможно. Параметр tbl пуст"); return li_rc; }
            if (fld.Length < 1) { MessageBox.Show("Сохранение LangRes невозможно. Параметр fld пуст"); return li_rc; }
            if (lang.Length < 1) { MessageBox.Show("Сохранение LangRes невозможно. Параметр lang пуст"); return li_rc; }
            if (value.Length < 1) { MessageBox.Show("Сохранение LangRes невозможно. Параметр value пуст"); return li_rc; }
            if (id == 0) { MessageBox.Show("Сохранение LangRes невозможно. Параметр id = 0"); return li_rc; }
            if (l_con == null)
            {
                MessageBox.Show("Сохранение LangRes невозможно. Ошибка установления соединения"); return li_rc;
            }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select count() from LangRes where tbl=@tbl and fld=@fld and lang=@lang and id=@id";   
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@tbl", tbl, "string");  // задаем значения параметров
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@fld", fld, "string");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    if (Convert.ToInt32(cnt.ToString()) == 0) Action = 'I';
                    else Action = 'U';

                    if (Action == 'I')
                    {
                        cmd.CommandText = "insert into LangRes(tbl, fld, lang, id, value) " +
                        "Values(@tbl, @fld, @lang, @id, @value)";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@tbl", tbl, "string");  // задаем значения параметров
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@fld", fld, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@value", value, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else if (Action == 'U')
                    {
                        cmd.CommandText = "update LangRes set value=@value  where tbl=@tbl and fld=@fld and lang=@lang and id=@id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@tbl", tbl, "string");  // задаем значения параметров
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@fld", fld, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@value", value, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else
                    {
                        MessageBox.Show("Up_Labels. Неверно указан Action ");
                        return li_rc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Up_Labels Ошибка сохранения " + ex.Message);
                return li_rc;
            }
            return li_rc;
        }

        //-----------------------------------------------------------------------------------
        // добавление/изменение InfoTable. Возврат: -1 - ошибка, 1 - успешно 
        static public int Up_InfoTable(int line_id, int st_id, int type, string text, string lang)
        {
            int li_rc = -1;
            char Action;  // I - insert   U- update
            if (line_id == 0) { MessageBox.Show("Сохранение InfoTable невозможно. Код линии = 0"); return li_rc; }
            if (st_id == 0) { MessageBox.Show("Сохранение InfoTable невозможно. Код станции = 0"); return li_rc; }
            if (lang.Length < 2) { MessageBox.Show("Сохранение InfoTable невозможно. Слишком короткий код языка=" + lang); return li_rc; }
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null) {MessageBox.Show("Сохранение InfoTable невозможно. Ошибка установления соединения"); return li_rc;}
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select count() from InfoTable where type=@type and st_id=@st_id and line_id=@line_id and lang=@lang";
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id", st_id, "int");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", line_id, "int");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type", type, "int");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    if (Convert.ToInt32(cnt.ToString()) == 0) Action = 'I';
                    else Action = 'U';
                    if (Action == 'I')
                    {
                        cmd.CommandText = "insert into InfoTable(line_id, st_id, type, text, lang) " +
                        "Values(@line_id, @st_id, @type, @text, @lang)";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", line_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id", st_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type", type, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@text", text, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else if (Action == 'U')
                    {
                        cmd.CommandText = "update InfoTable set text=@text " +
                            " where st_id=@st_id and lang=@lang and type=@type and line_id=@line_id";
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", line_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id", st_id, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type", type, "int");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@text", text, "string");
                        prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                        cmd.ExecuteNonQuery();  // выполняем сформированный запрос на добавление
                        li_rc = 1;
                    }
                    else
                    {
                        MessageBox.Show("Up_InfoTable. Неверно указан Action ");
                        return li_rc;
                    }
                }
            }
            catch (Exception ex)
                { MessageBox.Show(" Up_InfoTable Ошибка сохранения " + ex.Message);return li_rc;}
            return li_rc;
        }
        //--------------------------------------------------------------------------------------------------
        // фукция возвращает одно поле ColName в виде string из таблице TblName с id = параметру int id
        // возврат: -1 - ошибка;   1 - Ok Возможно есть смысл дополнительно обработать null
        static public int f_GetColValueById(string TblName, string ColName, int id, out string retStr)
        {
            int li_rc = -1;
            int WithCity = 0; // учет поля cities_id для таблиц, зависящих от города. для такой таблицы подмешиваем в sql доп.условие and t.cities_id=su.cities_id
            retStr = String.Empty;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            { MessageBox.Show("f_CheckExistById: Ошибка установления соединения"); return li_rc; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    if (prvCommon.curDB != prvCommon.DBName.SQLite) WithCity = f_CheckCities_Id(TblName);
                    if (WithCity < 0)
                    { MessageBox.Show("f_CheckExistById: Ошибка поиска поля cities_id"); return li_rc; }
                    else if (WithCity == 0)
                        cmd.CommandText = "select CAST(" + ColName + " as TEXT) from " + TblName + " where id=@id";   // формируем строку запроса
                    else if (WithCity > 0)
                        cmd.CommandText = "select CAST(" + ColName + " as TEXT) from " + TblName + " t,setup su where id=@id and t.cities_id=su.cities_id";   // формируем строку запроса
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    if (cnt == null) retStr = String.Empty;
                    else retStr = cnt.ToString();
                    li_rc = 1;
                }
            }
            catch (Exception ex)
            { MessageBox.Show("f_CheckExistById: Ошибка проверки наличия записи с кодом " + id.ToString() + " в таблице " + TblName + ex.Message); }
            return li_rc;
        }
        //--------------------------------------------------------------------------------------------------
        // фукция возвращает языковое описание из InfoTable (в retStr)
        // возврат: -1 - ошибка;   1 - Ok  
        static public int f_GetInfoTableText(int st_id, int line_id, int type, string lang, out string retStr)
        {
            int li_rc = -1;
            retStr = String.Empty;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            { MessageBox.Show("f_GetInfoTableText: Ошибка установления соединения"); return li_rc; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select Coalesce(text,' ') from InfoTable where st_id=@st_id and line_id=@line_id and type=@type  and lang=@lang"; 
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@st_id", st_id, "int");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@line_id", line_id, "int");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@type", type, "int");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    if (cnt == null) retStr = String.Empty;
                    else retStr = cnt.ToString();
                    li_rc = 1;
                }
            }
            catch (Exception ex)
            { MessageBox.Show("f_GetInfoTableText: Ошибка " + ex.Message); }
            return li_rc;
        }



        //---------------------------------------------------------------
        // фукция получает  языковой ресурс из LangRes (в retStr)
        // возврат: -1 - ошибка;   1 - Ok  
        static public int f_GetLangResValue(string tbl, string fld, string lang, int id, out string retStr)
        {
            int li_rc = -1;
            retStr = String.Empty;
            if (tbl.Length < 1) { MessageBox.Show("Получение данных из LangRes невозможно. Параметр tbl пуст"); return li_rc; }
            if (fld.Length < 1) { MessageBox.Show("Получение данных из  LangRes невозможно. Параметр fld пуст"); return li_rc; }
            if (lang.Length < 1) { MessageBox.Show("Получение данных из  LangRes невозможно. Параметр lang пуст"); return li_rc; }
            // если id = 999999, то это служебные данны интерфейса. Подписи форм, кнопок и т.п.
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            { MessageBox.Show("f_GetLangResValue: Ошибка установления соединения"); return li_rc; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select Coalesce(value,'') from LangRes where tbl=@tbl and fld=@fld and lang=@lang and id=@id";
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@tbl", tbl, "string");  // задаем значения параметров
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@fld", fld, "string");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@lang", lang, "string");
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    if (cnt == null) retStr = String.Empty;
                    else retStr = cnt.ToString();
                    li_rc = 1;
                }
            }
            catch (Exception ex)
            { MessageBox.Show("f_GetLangResValue: Ошибка " + ex.Message); }
            return li_rc;
        }
        //--------------------------------------------------------------------------------------------------
        // фукция возвращает значение поля fld из таблицы настроек Setup (в retStr)
        // возврат: -1 - ошибка;   1 - Ok  
        static public int f_GetSetupValue(string fld, out string retStr)
        {
            int li_rc = -1;
            retStr = String.Empty;
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
            { MessageBox.Show("f_GetSetupValue: Ошибка установления соединения"); return li_rc; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select Coalesce(" + fld + ",'') from setup ";
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    if (cnt == null) retStr = String.Empty;
                    else retStr = cnt.ToString();
                    li_rc = 1;
                }
            }
            catch (Exception ex) { MessageBox.Show("f_GetSetupValue: Ошибка " + ex.Message); }
            return li_rc;
        }
        //--------------------------------------------------------------------------------------------------
        // фукция проверяет наличие в таблице TblName записи с id = параметру int id
        // возврат: -1 - ошибка; 0 - нет записи с таким id; > 0 - есть записи с таким 
        static public int f_CheckExistById(string TblName, int id)
        {   
            int li_rc = -1;
            int WithCity = 0; // учет поля cities_id для таблиц, зависящих от города. для такой таблицы подмешиваем в sql доп.условие and t.cities_id=su.cities_id
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null)
                {MessageBox.Show("f_CheckExistById: Ошибка установления соединения"); return li_rc;}
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    if (prvCommon.curDB != prvCommon.DBName.SQLite) WithCity = f_CheckCities_Id(TblName);
                    if (WithCity < 0)
                        { MessageBox.Show("f_CheckExistById: Ошибка поиска поля cities_id"); return li_rc;}
                    else if (WithCity == 0)
                        cmd.CommandText = "select count() from " + TblName + " where id=@id";   // формируем строку запроса
                    else if (WithCity > 0)
                        cmd.CommandText = "select count() from " + TblName + " t,setup su where id=@id and t.cities_id=su.cities_id";   // формируем строку запроса
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@id", id, "int");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    li_rc = Convert.ToInt32(cnt.ToString());
                }
            }
            catch (Exception ex)
                {MessageBox.Show("f_CheckExistById: Ошибка проверки наличия записи с кодом " + id.ToString() + " в таблице " + TblName  + ex.Message);}
            return li_rc;
        }
    
        //--------------------------------------------------------------------------------------------------
        // фукция проверяет в таблице TblName поля cities_id Строго для SQLite
        // возврат: -1 - ошибка; 0 - нет поля ;  > 0 - есть поле cities_id 
        static public int f_CheckCities_Id(string TblName)
        {
            int li_rc = -1;
            if (prvCommon.curDB != prvCommon.DBName.SQLite)
                { MessageBox.Show("f_CheckCities_Id: Функция работает только с БД SQLite"); return li_rc; }
            var l_con = prvCommon.f_GetDBConnection(prvCommon.curDB);
            if (l_con == null) { MessageBox.Show("f_CheckExistById: Ошибка установления соединения"); return li_rc; }
            try
            {
                using (l_con)
                {
                    var cmd = prvCommon.f_GetSQLCommandVar(prvCommon.curDB, l_con);
                    cmd.CommandText = "select count() from sqlite_master where type='table' and upper(tbl_name) = upper(@TblName) and lower(sql) like '%cities_id%'";
                    prvCommon.f_AddParm(prvCommon.curDB, cmd, "@TblName", TblName, "string");
                    cmd.ExecuteNonQuery();  // посылаем запрос на выполнение
                    object cnt = cmd.ExecuteScalar();
                    li_rc = Convert.ToInt32(cnt.ToString());
                }
            }
            catch (Exception ex)
                { MessageBox.Show("f_CheckCities_Id: Ошибка: " + ex.Message); }
            return li_rc;
        }
//--------------------------------------------------------------------------------------------------

    }
}
