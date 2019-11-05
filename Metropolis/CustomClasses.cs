using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;    
using System.Windows.Forms;

namespace Metropolis
{
    public class MyBtn : Button
    {
        public MyBtn()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }   

    public class RoundButton : Control
    {
        public Color BackColor2 { get; set; }
        public Color ButtonBorderColor { get; set; }
        public int   ButtonRoundRadius { get; set; }

        public Color ButtonHighlightColor { get; set; }
        public Color ButtonHighlightColor2 { get; set; }
        public Color ButtonHighlightForeColor { get; set; }

        public Color ButtonPressedColor { get; set; }
        public Color ButtonPressedColor2 { get; set; }
        public Color ButtonPressedForeColor { get; set; }

        private bool IsHighlighted;
        private bool IsPressed;

        //public Microsoft.VisualBasic.PowerPacks.LineShape lineShape { get; set; }


    // сюда складываем ссылки на связанные с этой кнопкой контролы линий, чтобы таскать
    public Microsoft.VisualBasic.PowerPacks.LineShape[] ConLine = new Microsoft.VisualBasic.PowerPacks.LineShape[5];
//        public System.Windows.Forms.Control  StBtCtl;   // ссылка на экранный control станции
//        public System.Windows.Forms.Control  LbBtCtl;   // ссылка на экранный control метки
        public StationLabel  LbBtCtl;   // ссылка на экранный control метки

        // public int  st_id { get; set; }        // id станции. Крайне хорошо ему быть уникальным
        // public int  line_id { get; set; }   // ID линии
        // public int  stationname { get; set; } 
        public int coordX { get; set; } // это фикс.координата схемы.хранится в файле
        public int coordY { get; set; } //
        public int UniqueId { get; set; }   // уникальный идентификатор. равен 10000*line_id + st_id. Будет и в кнопках

        int X = 0, Y = 0;
        //  X = 100, Y = 100;
        public int st_id { get; set; }             // id станции
        public int st_line_id { get; set; }        // id линии
 
        public string st_name { get; set; }        // название станции

        // это координата на схеме. с учетом масштабирования
        private int scurrX;
        public int ScurrX
        { 
          get { return scurrX; }
          set
            {
                if (scurrX != value)
                {
                    foreach (var line in ConLine)
                    {
                        if (line != null)
                        {
                            // найдем нужный "конец" и откорректируем координату по текущему значению
                            if ((scurrX + (int)(Height / 2)) == line.X1 && (scurrY + (int)(Height / 2)) == line.Y1)
                            {
                                line.X1 = value + (int)(Height / 2);
                            }
                            if ((scurrX + (int)(Height / 2)) == line.X2 && (scurrY + (int)(Height / 2)) == line.Y2)
                            {
                                line.X2 = value + (int)(Height / 2);
                            }
                        }
                        else break;
                    }
                    scurrX = value;
                //    ScurrX = scurrX;
                    // if (StBtCtl != null) StBtCtl.BringToFront();
                }
            }
        }

        private int scurrY;
        public int ScurrY
        {
            get { return scurrY; }
            set
            {
                if (scurrY != value)
                {
                    foreach (var line in ConLine)
                    {
                        if (line != null)
                        {
                            // найдем нужный "конец" и откорректируем координату по текущему значению
                            if ((scurrY + (int)(Height / 2)) == line.Y1 && (scurrX + (int)(Height / 2)) == line.X1) line.Y1 = value + (int)(Height / 2);
                            if ((scurrY + (int)(Height / 2)) == line.Y2 && (scurrX + (int)(Height / 2)) == line.X2) line.Y2 = value + (int)(Height / 2);
                        }
                        else break;
                    }
                   
                    scurrY = value;
                //    ScurrY = scurrY;
                //    if (StBtCtl != null) StBtCtl.BringToFront();
                }
            }
        }
        public int LblX { get; set; }            // координата X метки станции на схеме
        public int LblY { get; set; }            // координата Y метки станции на схеме
        public int LcurrX { get; set; }  // это координата на схеме. с учетом масштабирования
        public int LcurrY { get; set; }  //
        public string lbl_name { get; set; }        // название метки станции (может отсутствовать, например, для двух из трех Киевских)
 //       public StationLabel StLblPointer;     // ссылка на метку, чтоб таскать половчее. пока не используется

 //       int DeltaX, DeltaY;

        public void SetXY(int cX, int cY)
        {
            X = cX; Y = cY; // для установки отступа формы от края экрана, чтобы нормально осуществлялся Drag кнопок
        }
        public RoundButton()
        {
            // ResizeRedraw = true;
            Size = new Size(100, 40);
            ButtonRoundRadius = 30;
            BackColor = Color.Gainsboro;
            BackColor2 = Color.Silver;
            ButtonBorderColor = Color.Black;
            ButtonHighlightColor = Color.Orange;
            ButtonHighlightColor2 = Color.OrangeRed;
            ButtonHighlightForeColor = Color.Black;

            ButtonPressedColor = Color.Red;
            ButtonPressedColor2 = Color.Maroon;
            ButtonPressedForeColor = Color.White;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            var foreColor = IsPressed ? ButtonPressedForeColor : IsHighlighted ? ButtonHighlightForeColor : ForeColor;
            var backColor = IsPressed ? ButtonPressedColor : IsHighlighted ? ButtonHighlightColor : BackColor;
            var backColor2 = IsPressed ? ButtonPressedColor2 : IsHighlighted ? ButtonHighlightColor2 : BackColor2;


            using (var pen = new Pen(ButtonBorderColor, 1))
                e.Graphics.DrawPath(pen, Path);

            using (var brush = new LinearGradientBrush(ClientRectangle, backColor, backColor2, LinearGradientMode.Vertical))
                e.Graphics.FillPath(brush, Path);

            using (var brush = new SolidBrush(foreColor))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                var rect = ClientRectangle;
                rect.Inflate(-4, -4);
                e.Graphics.DrawString(Text, Font, brush, rect, sf);
            }

            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
       
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            IsHighlighted = true;
            Parent.Invalidate(Bounds, false);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            IsHighlighted = false;
            IsPressed = false;
            Parent.Invalidate(Bounds, false);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Parent.Invalidate(Bounds, false);
            Invalidate();
    //        if ((e.Button & MouseButtons.Left) != 0  && IsPressed)
    //        {
    //            Location = new System.Drawing.Point(MousePosition.X - X, MousePosition.Y - Y);
    //            if (this.StLblPointer != null)
    //            {
    //                this.StLblPointer.Location = new System.Drawing.Point(MousePosition.X - X + DeltaX, MousePosition.Y - Y + DeltaY);
    //            }
    //        }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Parent.Invalidate(Bounds, false);
            Invalidate();
            IsPressed = true;
 //           if (this.StLblPointer != null)
 //           {
 //               DeltaX = coordX - this.StLblPointer.coordX;
 //               DeltaY = coordY - this.StLblPointer.coordY;
 //           }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Parent.Invalidate(Bounds, false);
            Invalidate();
            IsPressed = false;
        }

        protected GraphicsPath Path
        {
            get
            {
                var rect = ClientRectangle;
                rect.Inflate(-5, -5);
                return GetRoundedRectangle(rect, ButtonRoundRadius);
            }
        }

        public static GraphicsPath GetRoundedRectangle(Rectangle rect, int d)
        {
            var gp = new GraphicsPath();

            gp.AddArc(rect.X, rect.Y, d, d, 180, 90);
            gp.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90);
            gp.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90);
            gp.CloseFigure();

            return gp;
        }
    }
    public class StationLabel : Label
    {
        public int coordX { get; set; }
        public int coordY { get; set; }
        public Color ButtonHighlightForeColor { get; set; }
        private bool IsPressed;
        public RoundButton ParentStation;  // ССЫЛКА НА станцию, к которой привязана метка

        int X = 100, Y = 100;

        public void SetXY(int cX, int cY)
        {
            X = cX; Y = cY; // для установки отступа формы от края экрана, чтобы нармально осуществлялся Drag кнопок
        }
        public StationLabel()
        {
            Size = new Size(50, 10);
            //BorderStyle = BorderStyle.FixedSingle;

            ForeColor = System.Drawing.Color.Black;
            AutoSize = true;

        }
 

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Parent.Invalidate(Bounds, false);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            IsPressed = false;
            Parent.Invalidate(Bounds, false);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Parent.Invalidate(Bounds, false);
            Invalidate();
            if ((e.Button & MouseButtons.Left) != 0 && IsPressed)
            {
                //       Location = new System.Drawing.Point(MousePosition.X - X, MousePosition.Y - Y);
                //      public System.Drawing.Point PointToClient(System.Drawing.Point p);
                if (Program.EditMode) Location = Parent.PointToClient(new Point(MousePosition.X, MousePosition.Y));
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Parent.Invalidate(Bounds, false);
            Invalidate();
            IsPressed = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Parent.Invalidate(Bounds, false);
            Invalidate();
            IsPressed = false;

        }
    }

    // нужность класса пока под вопросом, т.к.с перегонами не работаем
    /*
    public class StConnector
    {
       // public string   name;  // 
        public int st_id_from;
        public int st_id_to;
        public int StartX;
        public int StartY;
        public int EndX;
        public int EndY;
        public int show;        // показывать линию или нет. пока предполагаем, что сюда попадают только с show=1
        public RoundButton button_from;  // связь с кнопкой, к которой привязана линия
        public RoundButton button_to;    // связь с кнопкой, к которой привязана линия вторым концом
        public Control ConLine;          // связь с экранным контролом 
    }
    */
    public class Station
    {
        public RoundButton Button { get; set; }
        public string Line { get; set; }
        public string Name { get; set; }
        public List<Image> Images { get; set; }
        public string RusText { get; set; }
        public string EngText { get; set; }
        public int TimeToNextStation { get; set; }
        public Color Color { get; set; }
        public string Info { get; set; }
        public string History { get; set; }
        public string Route { get; set; }

        public Station(RoundButton b, string n)
        {
            Images = new List<Image>();
            Button = b;
            Name = n;
            Color = b.BackColor;
        }
    }
}
