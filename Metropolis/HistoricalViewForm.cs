﻿using Metropolis.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metropolis
{
    public partial class HistoricalViewForm : Form
    {
        Image img;

        public HistoricalViewForm()
        {
            InitializeComponent();
            img = pictureBox.Image;
            pictureBox.MouseWheel += PictureBox1_MouseWheel;
        }



        public int time = 0;

        List<string> texts = new List<string>();
        List<List<Image>> images = new List<List<Image>>();
        int startH, startW;
        public List<Image> back = SchemeEditor.back;

        private void HistoricalViewForm_Load(object sender, EventArgs e)
        {

            if (SchemeEditor.timeForBack != 0)
               timerBack.Enabled = true;

            Random random = new Random();
            this.BackgroundImage = back[random.Next(back.Count)];

            //this.WindowState = FormWindowState.Maximized;
            textBox.ReadOnly = true;
            pictureBox.Width = this.Width - pictureBox.Left - 50;
            pictureBox.Height = pictureBox.Width * 5 / 7 - 20;
            textBox.Height = pictureBox.Height;
            startH = pictureBox.Height;
            startW = pictureBox.Width;

            Information();
            if (SchemeEditor.Lang == "En")
            {
                lblYear.Text = "Select the Year:";
                btnToRealSize.Text = "Reset the scale";
                DialogResult result = MessageBox.Show("Sorry, text about history is only in Russian language! Do you want to see it?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);


                if (result == DialogResult.No)
                    for (int i = 0; i < texts.Count; i++)
                       texts[i] = "There is only historical text in Russian. Restart this form to see it.";
                    
            }

        }



        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                pictureBox.Focus();
                cbYears.Enabled = false;
                textBox.Enabled = false;
                lblYear.Enabled = false;
                if (e.Delta > 0)
                {
                    if (pictureBox.Width + pictureBox.Left + 50 < this.Width+this.Left + 140)
                    {
                        pictureBox.Width += 50;
                        pictureBox.Height += 50;
                    }
                }
                else
                {
                    if (pictureBox.Width * pictureBox.Height > 150000)
                    {
                        pictureBox.Width -= 50;
                        pictureBox.Height -= 50;
                    }

                }
                cbYears.Enabled = true;
                textBox.Enabled = true;
                lblYear.Enabled = true;
            }
        }

      

        public List<Image> yearImages;
        private void cbYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            ind = 0;
            textBox.Text = texts[cbYears.SelectedIndex];
            yearImages = images[cbYears.SelectedIndex];
            pictureBox.Image = yearImages[0];
        }

        public int ind = 0;
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (yearImages != null)
            {
                if (ind < yearImages.Count-1)
                {
                    ind++;
                    pictureBox.Image = yearImages[ind];
                }
                else
                {
                    pictureBox.Image = yearImages[0];
                    ind = 0;
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (yearImages != null)
            {
                if (ind > 0)
                {
                    ind--;
                    pictureBox.Image = yearImages[ind];
                }
                else
                {
                    pictureBox.Image = yearImages[yearImages.Count - 1];
                    ind = yearImages.Count-1;
                }
            }
        }

        private void btnToRealSize_Click(object sender, EventArgs e)
        {
            pictureBox.Width = startW;
            pictureBox.Height = startH;
        }


        private void timerBack_Tick(object sender, EventArgs e)
        {
            
            time++;
            if (time > SchemeEditor.timeForBack)
            {
                Random random = new Random();
                this.BackgroundImage = back[random.Next(back.Count)];
                time = 0;
            }
        }


        private void btn_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                btn.BackColor = SchemeEditor.MyColor;
            }

            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem btn = sender as ToolStripMenuItem;
                btn.BackColor = SchemeEditor.MyColor;
            }
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                btn.BackColor = SchemeEditor.DefColor;
            }

            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem btn = sender as ToolStripMenuItem;
                btn.BackColor = Color.White;
            }
        }


        private void Information()
        {

            texts.Add("Первый метрополитен в мире появился в Лондоне в 1863 года после изобретения Марком Брюнелем проходческого щита, что позволило прорыть тоннель под Темзой. Затем подземка пришла в Нью-Йорк (сначала в 1868 в виде надземного участка, который не сохранился, затем в 1904 году с появлением первой подземной линии), Глазго (Будапешт (1896), Париж (1900) и в Берлин (1902). Любопытно, что изначально Марк Брюнель работал над изобретением проходческого щита...в Петербурге при Александре I, однако его смерть не дала продолжить работу изобретателю. Он переработал проект вместо Невы под Темзу, и в 1842 году был проложен первый в мире подводный тоннель, что дало возможность в перспективе создать подземную систему общественного транспорта в Лондоне. В России вплоть до революции 1917 года не получалось построить свой собственный метрополитен по причине опасения крупных промышленников по поводу возможного земельного передела.");
            images.Add(new List<Image>());
            images[0].Add(Resources._1_1);
            images[0].Add(Resources._1_2);
            images[0].Add(Resources._1_3);
            images[0].Add(Resources._1_4);



            texts.Add("B конце 1920-х гг., когда население пролетарской столицы в Москве подошло к 4 миллионам жителей, к идее строительства метро на государственном уровне вернулись вновь во избежание угрозы транспортного коллапса. В июне 1931 года постановлением пленума ЦК ВКП(б) 'О Московском городском хозяйства и о развитии городского хозяйства СССР' дали старт работам. Главным консультантом Метростроя был американец Джордж Морган, специалист по тоннельным работам, чей опыт очень пригодился метростроевцам. Куратором стройки по партийной линии был непосредственно Лазарь Каганович, главный зачинатель сталинской реконструкции Москвы и архитектурного оформления 'пролетарской столицы', в честь которого в 1935-1955 гг. называлась московская подземка." +
                "\r\n\r\nНа открытии метрополитена Каганович говорил: \r\n'Мы боролись с природой, мы боролись с плохими грунтами под Москвой. Московская геология оказалась дореволюционной, старорежимной, она не сочувствовала большевикам, она шла против нас. Мы воевали за новое человеческое общество, мы воевали против эксплуатации, против рабства, за добровольный сознательный труд на благо всего коллектива, за труд, воодушевляющий людей, за труд, созидающий не только новые прекрасные вещи, но и нового человека'." +
                "\r\n\r\nЕщё бы, по оценки Джорджа Моргана, к январю 1934 года было проделано только 6 % всех работ, то в январе 1935 года план был выполнен полностью, установив рекорд скорости, не известный до сего момента в мировой практике метростроения." +
                "\r\nОбратите внимание на названия ряда станций: \r\n\r\nВместо 'Кропоткинской' обозначена станция 'Дворец Советов' на месте взорванного Храма Христа Спасителя в 1931 году, где предполагалось возведение самого высокого здания в мире высотой в 415 метров для проведения заседаний Верховного Совета СССР, имевшего символ как кульминация высотного строительства страны Советов;" +
                "\r\n\r\n'Дзержинская' на месте современной 'Лубянки' до переименования в 1990 году.В 1935 года она ещё не имела центральный зал и представляла собой два параллельных перронных тоннеля, объединённых лишь небольшим залом при сходе с эскалатора.В ходе реконструкции в 1973 - 1975 гг.станция обрела центральный зал вместе с переходом на станцию 'Кузнецкий мост'(в юго - западном торце «Лубянки» сохранился фрагмент первоначальной отделки станции)." +
                "\r\n\r\n'Кировская' вместо современного названия станции 'Чистые пруды' до переименования в 1990 году; " +
                "'Коминтерн' вместо нынешнего 'Александровского сада'.");
            images.Add(new List<Image>());
            images[1].Add(Resources._2_1);
            images[1].Add(Resources._2_2);




            texts.Add("Предполагаемое строительство 2 очереди метро на карте генерального плана развития Москвы ещё в 1935 году. Станция 'Киевская' нынешней Филёвской линии будет открыта в 1937 году.В 1938 году будет открыт участок от станции 'Сокол' до 'Площади Свердлова'(нынешняя 'Театральная'). Станция 'Тверская'(тогда ещё как 'Горьковская') будет открыта лишь в 1979 году между 'Театральной' и 'Маяковской' без прекращения движения поездов." +
            "\r\nТакже мы видим продолжение линии от 'Коминтерна' к открывшимся в 1938 году станциями 'Площадь Революции', 'Курской'.");
            images.Add(new List<Image>());
            images[2].Add(Resources._3_1);
            images[2].Add(Resources._3_2);
            images[2].Add(Resources._3_3);




            texts.Add("Схема метро 1937 года с открывшейся станцией 'Киевская'.\r\n\r\n" +
                "13 марта 1938 года была открыта вторая по хронологии линия метро — Арбатско - Покровская.Был построен участок «Улица Коминтерна» — «Курская» и соединён с ответвлением первой линии.Поезда проследовали по новой линии Московского метрополитена от станции «Киевская» до станции «Курская».\r\n\r\n" +
                "После попадания бомбы в тоннель мелкого заложения «Арбатская» — «Смоленская» в 1941 году стала очевидной незащищённость этого участка метро.После 1947 года было принято решение о замене этого участка новым, глубокого заложения.Поэтому к 1953 году был построен новый участок Арбатско - Покровской линии «Площадь Революции» — «Киевская», полностью дублировавший старый, при этом участок мелкого заложения «Калининская» (так называлась станция «Улица Коминтерна» с 1946 года) — «Киевская» был закрыт и открыт заново лишь в 1958 году в составе Филёвской линии.");
            images.Add(new List<Image>());
            images[3].Add(Resources._4_1);
            images[3].Add(Resources._4_2);
            images[3].Add(Resources._4_3);
            images[3].Add(Resources._4_4);





            texts.Add("Схемы линий 1940 года. На этой карте мы видим планируемый к постройке участок от 'Площади Свердлова' к 'Заводу им. Сталина' и от метро 'Курская' до 'Стадиона им. Сталина'.\r\n\r\n" +
                "Второй участок будущей Замоскворецкой линии начал строиться незадолго до начала Великой Отечественной войны, которое прерывалось только осенью 1941 года при подходе гитлеровские войска к Москве.Однако уже в мае 1942 года работы возобновились, и 1 января 1943 года состоялось открытие участка(первоначально без станций «Новокузнецкая» и «Павелецкая», которые были открыты позже уже в составе действующей линии в ноябре того же года).Трассировка продлила путь вдоль Красной площади и под Москва - рекой в Замоскворечье, через Павелецкий вокзал и дошла до станции «Завод имени Сталина» (нынешняя 'Автозаводская') рядом с находившимся заводом ЗИС(затем ЗИЛ).\r\n\r\n" +
                "Продолжение участка будущей Арбатско - Покровской линии включало в себя проектные названия станций 'Спартаковская'(ныне 'Бауманская'), Электрозаводская' и 'Стадион им. Сталина' (сейчас 'Партизанская').\r\n\r\n" +
                "Кстати, 'Стадион им. Сталина' изначально была рассчитана на большой пассажиропоток, поскольку спорткомплекс должен был стать самым крупным стадионом СССР, и поэтому на станции были уложены три пути.Предполагалось также строительство второго выхода(который и должен был вести к стадиону) с единственным в Москве шестиниточным эскалатором, поскольку на этой станции три пути, а не два; но стадион им.Сталина так и не был построен(сначала из-за войны, а затем из - за неблагоприятных гидрогеологических условий), и оформление станции было изменено.Трёхпутная структура у станции осталась, и в результате «Партизанская» является самой широкой станцией Московского метрополитена по ширине станционного зала. Все эти три станции стали доступны для пассажиров в 1944 году.");
            images.Add(new List<Image>());
            images[4].Add(Resources._5_1);
            images[4].Add(Resources._5_2);





            texts.Add("Схема Большого подземного кольца Московского метрополитена [будущей Кольцевой линии метро] 1946 года.\r\n\r\n" +
                "Первая очередь Кольца, открывшаяся в 1950 году, включала в себя такие станции, как 'Парк культуры', «Калужская» (ныне «Октябрьская»), «Серпуховская» (ныне «Добрынинская»), «Павелецкая», «Таганская», 'Курская'.Второй участок Кольца, открывшийся в 1952 году, включал в себя 'Комсомольскую', 'Проспект мира'(на момент открытия 'Ботанический сад'), 'Новослободскую' и 'Белорусскую'.Последняя очередь замкнула Кольцо со станциями 'Киевская', 'Краснопресненская' в 1954 году.\r\n\r\n" +
                "В 1946 году станция 'Коминтерн' будет переименована в 'Калининскую'.");
            images.Add(new List<Image>());
            images[5].Add(Resources._6_1);
            images[5].Add(Resources._6_2);
            images[5].Add(Resources._6_3);
            images[5].Add(Resources._6_4);




            texts.Add("Схема 1954 года, где Большое Кольцо ещё не замкнуто.\r\n\r\n" +
                "Обратите внимание, уже начинается проектирование северного участка будущей 'Калужско-Рижской линии'(Ботанический Сад, Рижская, Щербаковская и ВСХВ) и продолжение на юго - запад 'Сокольнической ветки'(Фрунзенская, Усачёвская, Лужниковская, Ленинские горы, Университет).\r\n\r\n" +
                "Открытая в 1954 году станция 'Киевская' стала одной из последних торжественных станций метро(её строительство Никита Хрущёв лично и курировал), после чего уже в 1955 году было принято постановления 'о борьбе с архитектурными излишествами'.Из - за этого не повезло Рижскому радиусу, так как изначально этим станциям планировалось гораздо более богатое оформление.\r\n\r\n" +
                "Зато станции 'Спортивная' и 'Фрунзенская' проектировались ещё до выхода Постановления Об устранении излишеств в проектировании и строительстве, поэтому, несмотря на дату открытия(1957 год), они относятся ещё к сталинской архитектуре и, таким образом, является одной из последних станций московского метро, построенных в стиле сталинского ампира.");
            images.Add(new List<Image>());
            images[6].Add(Resources._7_1);
            images[6].Add(Resources._7_3);




            texts.Add("Кольцевая линия оказалась ключевой в дальнейшем развитии Московского метрополитена. Часто радиальные линии строились «от кольца», лишь впоследствии соединяясь центральным участком. В настоящее время каждая из двенадцати станций на Кольцевой линии пересадочная (в 1954, при замыкании кольца, пересадочных было только шесть; последняя остававшаяся на линии станция без пересадки — «Новослободская» — получила переход на станцию «Менделеевская» в 1988 году).\r\n\r\n" +
                "\r\n\r\n" +
                "На фото - официальная схема линий на момент 1956 года.Здесь ещё интересный момент - на карте отображена станция 'Первомайская', которой сейчас нет(существовала в 1954 - 1961 гг.), сейчас там депо.\r\n\r\n" +
                "Ещё обратите внимание на нынешнюю станцию 'Охотный ряд', которая в 1955 - 1957 гг.носила название 'Им. Кагановича', когда в 1955 году Метрополитен в часть Лазаря Кагановича переименовали в честь Ленина, дав тому 'утешительный приз' в виде персонифицированного названия станции.В 1957 году Каганович как активный участник 'антипартийной группы', выступившей против нового курса Никиты Хрущёва, был выведен из состава Президиума ЦК и снят с высших государственных постов.\r\n" +
                "'Охотный ряд', кстати, переименовывали чаще других в московском метро: 1935 - 195 гг. - Охотный ряд, 1955 - 1957 гг. - Им.Кагановича, 1957 - 1961 гг. - вновь 'Охотный ряд', 1961 - 1990 гг.- 'Проспект Маркса', после вновь нынешнее название.");
            images.Add(new List<Image>());
            images[7].Add(Resources._8_1);



            texts.Add("Схема Московского метро им. Ленина 1957 года, когда уже откроется станция 'Спортивная', 'Фрунзенская'. В 1958 году появятся уже утилитарные 'Ботанический сад' (сейчас 'Проспект мира'), 'Рижская', 'Щербаковская' (ныне 'Алексеевская'),и ВСХВ (бывшая Всесоюзная сельскохозяйственная выставка, позже известная нам 'Выставка достижений народного хозяйства').\r\n\r\n" +
                "Кстати, на нынешней станции 'Проспект мира' Кольцевой линии в оформлении присутствуют растительные и биологические мотивы в орнаменте. Это её первое название как отражение идей селекционера - самоучки И.Мичурина, который в 1 половине 1950 - х гг.занимал ведущее положение в советской ботанике:\n'Мы не можем ждать милостей от природы, взять их у неё- наша задача!'");
            images.Add(new List<Image>());
            images[8].Add(Resources._9_1);
            images[8].Add(Resources._9_2);
            images[8].Add(Resources._9_3);




            texts.Add("Схемы линий от 1958 года.\r\n\r\n" +
                "Станции «Александровский сад» (на тот период «Калининская»), «Арбатская» и «Смоленская» мелкого заложения(нынешней Филевской линии) были закрыты и использовались под склады до открытия Филевской линии от «Киевской».\r\n\r\n" +
                "Проектное название станции 'Щербаковская' на Рижском радиусе изменено на официальную 'Мир' в момент открытия.\r\n\r\n" +
                "Также мы здесь видим конкретные названия линий: Горьковско - Замоскворецкая, Кировско - Фрунзенская, Арбатско - Покровкая, Кольцевая линии(вместо 1,2,3 очередей).");
            images.Add(new List<Image>());
            images[9].Add(Resources._10_1);
            images[9].Add(Resources._10_2);
            images[9].Add(Resources._10_3);



            texts.Add("Карта-схема метро на 1962 год.\r\n\r\n" +
                "Мы видим Рижскую линию со станциями 'Ботанический сад', 'Рижская', 'Мир' и уже 'ВДНХ'(станция 'Мир' будет переименована в 1966 году в 'Щербаковскую' в честь видного политического деятеля сталинской эпохи, которая в конце концов обретёт своё нынешнее название 'Алексеевская' в 1990 в память бывшего одноимённого села).\r\n\r\n" +
                "Открытая в 1958 году Филёвская линия до 1970 года будет носить официально название 'Арбатско-Филёвской'.\r\n\r\n" +
                "На Арбатско - Покровской линии в 1961 году открыта наземная станция 'Измайловский парк' вместо упразднённой станции 'Первомайской'.И конечная станция 'Первомайская', которая по своей архитектуре является сороконожкой.\r\n\r\n" +
                "На Кировско - Фрунзенской линии в 1959 году открыта станция 'Ленинские горы' и 'Университет'.На последней станции будет в 1960 году сниматься заключительная сцена фильма Георгия Данелии 'Я шагаю по Москве'.В финальной сцене герои садятся по очереди в поезда, движущиеся в разных направлениях, но поскольку на тот момент станция «Университет» была конечной(в кадр попал указатель «Посадки нет»), один из поездов на самом деле уезжал в оборотный тупик.");
            images.Add(new List<Image>());
            images[10].Add(Resources._11_1);
            images[10].Add(Resources._11_2);
            images[10].Add(Resources._11_3);



            texts.Add("А это схемы метро с 1964 по 1967 годы. Что изменилось?\r\n\r\n" +
                "На участке 'Кировско-Фрунзенской линии' ветка прирастает станциями 'Юго-Западная' и 'Проспектом Вернадского'.Красные ворота тогда носит название 'Лермонтовская'.\r\n\r\n" +
                "Обратите внимание, что на схеме 64 года Калужский и Рижский радиусы не образуют одну линию и даже показаны разными цветами!Дело в том, что в этот период планировалось только продление линий и строительство новых радиусов от станций Кольцевой линии, без соединения радиусов в диаметры через центр города.Лишь впоследствии это решение было признано ошибочным, и строительство центрального участка образовало Калужско - Рижскую линию.\r\n\r\n" +
                "На Калужской линии открываются станции 'Ленинский проспект', 'Октябрьская', 'Академическая'(рядом с первыми хрущёвками в СССР), 'Профсоюзная' и 'Новые Черёмушки'(рядом с первым жилым кварталом 'хрущёвок') и 'Калужской'(ныне не существует, закрыта в 1974 году, рядом с ней открыта одноимённая станция мелкого заложения).\r\n\r\n" +
                "На тогда ещё называемой Арбатско - Филёвской линии открывается станция 'Пионерская'.\r\n\r\n" +
                "На Горьковско - Замоскворецкой линии проектируется продление до 'Речного вокзала'.\r\n\r\n" +
                "Арбатско - Покровская линия прирастает станцией 'Щёлковская'.\r\n\r\nПроектируется нынешняя Таганско-Краснопресненская линия.");
            images.Add(new List<Image>());
            images[11].Add(Resources._12_1);
            images[11].Add(Resources._12_2);
            images[11].Add(Resources._12_3);
            images[11].Add(Resources._12_4);
            images[11].Add(Resources._12_5);




            texts.Add("Карта линий метро на 1970 год - первая попытка геометризировать схему. Уходят в прошлое плавные изгибы линий. Теперь в моде простота, четкость и стремительность. Кольцевая линия впервые изображается правильной окружностью, а радиусы - прямыми линиями. Изменилось и изображение логотипа метрополитена - стилизованной буквы 'М'. Если на схеме 70-го года еще остаются изломы на Таганско-Краснопресненской линии, то к концу десятилетия все линии изображаются прямыми без углов и изломов.\r\n\r\n" +
                "Обратите внимание на строящиеся линии - на продолжение Таганско - Краснопресненской(до 1989 года, Ждановско - Краснопресненской).В 1971 году будет открыта станция 'Площадь Ногина' с кроссплатформенной пересадкой на Калужско - Рижскую линию.\r\n\r\n" +
                "Филёвкая линия приобрела своё нынешнее наименование(вместо Арбатско - Филёвской).\r\n\r\nДалее схемы 1973 и 1975 годов");
            images.Add(new List<Image>());
            images[12].Add(Resources._13_1);
            images[12].Add(Resources._13_2);
            images[12].Add(Resources._13_3);



            texts.Add("Схема с новым дизайном от 1978 года: Кольцевая линия изображается в форме эллипса. В связи с проведением в Москве XXII Олимпийских игр выпускается много схем на иностранных языках.\r\n\r\n" +
                "Теперь Кольцевая линия представляет собой две полуокружности, соединенные вертикальными прямыми.Начертания линий опять становятся более плавными.Станции в периферийной части города теперь изображаются вплотную друг к другу.\r\n\r\n" +
                "Такое решение позволило увеличить размеры и более наглядно изобразить центральную часть схемы, где топология сети наиболее сложна.\r\n\r\n" +
                "В это же время появляется и нынешний логотип Московского метрополитена - стилизованная буква 'М' красного цвета в урезанной окружности синего цвета, напоминающей очертаниями обделку тоннеля.\r\n\r\n" +
                "Каких ещё линий не хватает по сравнению с 2019 годом? Калининской, Серпуховско - Тимирязевской, Люблинской, Солнцевской, Косинской, МЦК, БК и Каховской.Причём, будущая Каховская линия, образованная в 1995 году, изначально состояла из последних трёх станций Горьковско - Замоскворецкой ветки(Варшавская, Каширская, Каховская станции).");
            images.Add(new List<Image>());
            images[13].Add(Resources._14_1);
            images[13].Add(Resources._14_2);




            texts.Add("Схема от 1979 года с введённым новым участком Калининской линии, строительство которой специально было приурочено к Московской Олимпиаде с 6 станциями: «Марксистская» (с переходами на станции «Таганская» Кольцевой и Ждановско-Краснопресненской линий), «Площадь Ильича», «Авиамоторная», «Шоссе Энтузиастов», «Перово» и «Новогиреево». 16 декабря 1979 года по участку впервые прошёл пробный поезд с пассажирами-метростроевцами. 30 декабря 1979 года, в канун Нового года, участок был сдан в постоянную эксплуатацию; расчётное время поездки по новому радиусу метрополитена составляло тогда 14 минут.\r\n\r\nДалее схемы 1980 и 1981 годов.");
            images.Add(new List<Image>());
            images[14].Add(Resources._15_1);
            images[14].Add(Resources._15_2);
            images[14].Add(Resources._15_4);




            texts.Add("Схема пассажирского транспорта от 1984 года с появившимся годом ранее южным участком Серпуховско-Тимирязевской линии, состоявшей из 8 станций от станции 'Серпуховская' до 'Южной'.\r\n\r\n" +
                "С пуском первого участка Серпуховско - Тимирязевской линии появляются новые схемы.Кольцевая линия опять рисуется правильной окружностью, и станции за пределами центральной части не прижаты друг к другу.Поначалу линии изображаются ломаными, но позднее заменяются более спрямленными.\r\n\r\n" +
                "Каховской линии ещё нет, до 1995 года осуществлялось вилочное движение по Горьковско - Замоскворецкой линии одновременно до Каховской станции и до 'Красногвардейской'. Далее схемы 1986-1988 годов");
            images.Add(new List<Image>());
            images[15].Add(Resources._16_1);
            images[15].Add(Resources._16_2);
            images[15].Add(Resources._16_3);
            images[15].Add(Resources._16_4);



            texts.Add("Уже 1989 год.\r\n\r\n" +
                "Здесь станция 'Ждановская переименована в 'Выхино', как и сама линия в Таганско-Краснопресненскую.\r\n\r\n" +
                "В канун 1990 года будет открыта станция 'Крылатское' Филёвской линии.\r\n\r\n" +
                "Вообще сама Филёвская линия является рекордсменом по числу 'нестандартностей':\r\n\r\n" +
                "- 4 из 6 станций этой линии имеют 'кривую' платформу('Александровский сад', 'Кутузовская', 'Международная', 'Выставочная'.\r\n" +
                "- самый короткий перегон в московском метро между станциями 'Арбатская' и 'Калиниская'(ныне между 'Александровским садом' и 'Арбатской').\r\n" +
                "- на линии самый длинный наземный участок от 'Студенческой' до 'Кунцевской'\r\n" +
                "- единственная линия метро Москвы, где нет оборотных тупиков за всеми конечными станциями.Направление поезда меняется прямо на станции\r\n" +
                "- линия, имеющая как и один из старейших участков, так и новейших в метро.");
            images.Add(new List<Image>());
            images[16].Add(Resources._17_1);




            texts.Add("Схема за 1990 год.\r\n\r\n" +
                "В этом году начата работа участка Калужско - Рижской линии от 'Тёплого стана' до 'Битцевского парка'.\r\n\r\n" +
                "Кировско - Фрунзенская линия прирастает станциями 'Улица Подбельского' и 'Черкизовской'.\r\n\r\n" +
                "В 1990 году происходит масштабное переименование многих станций и целых линий в современные названия: Кировско - Фрунзенская — в Сокольническую, Горьковско - Замоскворецкая — в Замоскворецкую, Серпуховская — в Серпуховско - Тимирязевскую.\r\n\r\n" +
                "Из станций переименованы: «Кировская» — в «Чистые пруды», «Дзержинская» — в «Лубянку», «Площадь Ногина» — в «Китай - город», «Проспект Маркса» — обратно в «Охотный Ряд», «Площадь Свердлова» — в «Театральную», «Горьковская» — в «Тверскую», «Калининская» — в «Александровский сад», «Колхозная» — в «Сухаревскую», «Щербаковская» — в «Алексеевскую», «Ленино» — в «Царицыно».\r\n\r\n\r\n\r\nДалее схемы 1991, 1992, 1993 годов");
            images.Add(new List<Image>());
            images[17].Add(Resources._18_1);
            images[17].Add(Resources._18_2);
            images[17].Add(Resources._18_3);
            images[17].Add(Resources._18_4);




            texts.Add("Схема метро за 1996 год.\r\n\r\n" +
                "Годом ранее будет введён в эксплуатацию участок Люблинской линии с 6 станциями — «Чкаловская», «Римская», «Крестьянская застава», «Кожуховская», «Печатники» и «Волжская». Проезд будет повышаться до 600, 800, 1000 и 1500 рублей.\r\n\r\n" +
                "За 1996 год подземка прирастёт ещё 3 станциями: 'Марьино', «Люблино» и «Братиславская».\r\n\r\n Уже в 1997 проезд возрастет до 2000 рублей, а в 1998 после деноминации будет стоить 2 рубля");
            images.Add(new List<Image>());
            images[18].Add(Resources._19_1);
            images[18].Add(Resources._19_2);
            images[18].Add(Resources._19_3);
            images[18].Add(Resources._19_4);





            texts.Add("Схемы 2000-2005 годов. Открытие 'Воробьевых гор' после реконструкции с 1983 года. Открытие новых станций 'Бульвар Дмитрия Донского', 'Анино', 'Парк Победы', 'Измайловский парк', 'Деловой центр'(ныне 'Выставочная'). Начинает работать развилка от станции Киевская");
            images.Add(new List<Image>());
            images[19].Add(Resources._20_1);
            images[19].Add(Resources._20_2);
            images[19].Add(Resources._20_3);
            images[19].Add(Resources._20_4);




            texts.Add("Официальные схемы 2010 и 2013 года, разработанные в студии Артемия Лебедева\r\n\r\nУчасток Люблинско-Дмитровской линии продлен от станции 'Чкаловская' до станции 'Марьина роща'.\r\n\r\nПроектируется участок Бутовской линии. Работает Монорельс");
            images.Add(new List<Image>());
            images[20].Add(Resources._21_1);
            images[20].Add(Resources._21_2);




            texts.Add("Официальные схемы 2015 и 2016 года, разработанные в студии Артемия Лебедева.\r\n\r\nСтроится МЦК, первый участок Солнцевской линии от 'Делового центра' до 'Парка Победы'. Планируется продление Солнцевской и Люблинско-Дмитровской линии. Достраивается Бутовская линия. Строится участок Большого Кольца от 'Савеловской' до 'Выставочной'");
            images.Add(new List<Image>());
            images[21].Add(Resources._22_1);
            images[21].Add(Resources._22_2);



            texts.Add("Официальные схемы 2018 и 2019 года, разработанные в студии Артемия Лебедева\r\nСолнцевская линия продлена до 'Рассказовки'. Люблинско-Дмитровская линия продлена до станции 'Селигерская'. Открываются станции 'Ховрино' и 'Беломорская'. Открыт участок  Большого Кольца от 'Савеловской' до 'Выставочной'.\r\n\r\nВ 2019 году Сокольническая линия продлена до станции 'Коммунарка', открыта Косинская линия, Каховская же закрыта до 2021 года для строительства Большой Кольцевой линии.");
            images.Add(new List<Image>());
            images[22].Add(Resources._23_1);
            images[22].Add(Resources._23_2);
        }
    }
}