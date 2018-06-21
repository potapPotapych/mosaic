using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mosaic
{
    // Класс главной формы, размещает на себе элементы
    // управления - панель, прямоугольники PictureBox и возможно другие.
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new string[] { "Пятнашки", "Переставки" });
        }
        bool img = false;
        // Массив объектов прямоугольников для хранения сегментов картинки.
        PictureBox[] PB = null;
        // Длина стороны в прямоугольниках.
        int LengthSides = 3;
        // Объект хранения картинки.
        public static Bitmap Picture = null;

        //Создание области для картинки
        void CreatePictureRegion()
        {
            // Удаление предыдущего массива картинок, чтобы создать новый.
            if (PB != null)
            {
                for (int i = 0; i < PB.Length; i++)
                {
                    PB[i].Dispose();
                }
                PB = null;
            }


            int num = LengthSides;
            // Создаем массив прямоугольников размером LengthSides Х LengthSides.
            PB = new PictureBox[num * num];

            // Вычислим габаритные размеры прямоугольников.
            int w = panel1.Width / num;
            int h = panel1.Height / num;

            int countX = 0; // счетчик прямоугольников по координате Х в одном ряду
            int countY = 0; // счетчик прямоуголников по координате Y в одном столбце
            for (int i = 0; i < PB.Length; i++)
            {
                PB[i] = new PictureBox(); // непосредственное создание прямоугольника, элемента массива

                // Размеры и координаты размещения созданного прямоугольника.
                PB[i].Width = w;
                PB[i].Height = h;
                PB[i].Left = 0 + countX * PB[i].Width;
                PB[i].Top = 0 + countY * PB[i].Height;

                // Запомним начальные координаты прямоугольника для
                // восстановления перемешанной картинки,
                // определения полной сборки картинки.
                Point pt = new Point();
                pt.X = PB[i].Left;
                pt.Y = PB[i].Top;
                PB[i].Tag = pt; // сохраним координаты в свойстве Tag каждого прямоугольника

                // Считаем прямоугольники по рядам и столбцам.
                countX++;
                if (countX == num)
                {
                    countX = 0;
                    countY++;
                }


                PB[i].Parent = panel1; // разместим прямоугольники на панели
                PB[i].BorderStyle = BorderStyle.None;
                PB[i].SizeMode = PictureBoxSizeMode.StretchImage; // размеры картинки будут подгоняться под размеры прямоугольника
                PB[i].Show(); // гарантия видимости прямоугольника

                // Для всех прямоугольников массива событие клика мыши
                // будет обрабатываться в одной и той же функции, для удобства
                // вычисления координат прямоугольников в одном месте.
                //c.Show();
               
                    //if (FormMain.ch == 1)
                    //{
                       
                    //    PB[i].Click += new EventHandler(PB_Click1);
                    //}
                    //if (FormMain.ch == 2)
                    //{
                        
                    //    PB[i].Click += new EventHandler(PB_Click2);
                    //}
                

            }

            // Раскидываем картинку на сегменты и рисуем каждый сегмент  на своем прямоугольнике.
            DrawPicture();

        }


        // Загрузка картинки
        void DrawPicture()
        {
            if (Picture == null) return;
            int countX = 0;
            int countY = 0;
            int num = LengthSides;
            for (int i = 0; i < PB.Length; i++)
            {
                int w = Picture.Width / num;
                int h = Picture.Height / num;
                PB[i].Image = Picture.Clone(new RectangleF(countX * w, countY * h, w, h), Picture.PixelFormat);
                countX++;
                if (countX == LengthSides)
                {
                    countX = 0;
                    countY++;
                }
            }

        }

        void PB_Click1(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            for (int i = 0; i < PB.Length; i++)
            {
                // Сначала определим пустое место на области рисования картинки.
                if (PB[i].Visible == false)
                {
                    // Затем проверим кликнутый прямоугольник и
                    // если у него совпадают координаты по X или Y,
                    // (кроме прямоугольников, расположенных по диагонали)
                    if (
                        (pb.Location.X == PB[i].Location.X &&
                         Math.Abs(pb.Location.Y - PB[i].Location.Y) == PB[i].Height)
                        ||
                        (pb.Location.Y == PB[i].Location.Y &&
                         Math.Abs(pb.Location.X - PB[i].Location.X) == PB[i].Width)
                       )
                    {
                        // После успешной проверки
                        // меняем местами пустой и кликнутый прямоугольники.
                        Point pt = PB[i].Location;
                        PB[i].Location = pb.Location;
                        pb.Location = pt;

                        // После каждого хода проверка на полную сборку картинки.

                        // Если хоть у одного прямоугольника не совпадают
                        // реальные координаты и первичные выходим из метода.
                        for (int j = 0; j < PB.Length; j++)
                        {
                            Point point = (Point)PB[j].Tag;
                            if (PB[j].Location != point)
                            {
                                return;
                            }
                        }

                        // Если у всех прямоугольников совпали реальные и первичные 
                        // координаты - картинка собрана!
                        for (int m = 0; m < PB.Length; m++) // убираем обрамление прямоугольников
                        {
                            PB[m].Visible = true;
                            PB[m].BorderStyle = BorderStyle.None;
                        }
                        MessageBox.Show("Картинка собрана");
                    }

                    break;
                }
            }
        }

        // Второй вариант игры
        Point pt1, pt2, pt3;
        bool a = true;

        void PB_Click2(object sender, EventArgs e)
        {
            int p1 = 0, p2 = 0;
            
            PictureBox pb = (PictureBox)sender;

            if (a == true)
            {
                pt1 = pb.Location;
                a = false;
                //MessageBox.Show("{0}" + pt1.X);
            }

            if (pb.Location != pt1)
            {
                pt2 = pb.Location;

                for (int j = 0; j < PB.Length; j++)
                {
                    if (PB[j].Location == pt1) p1 = j;
                    if (PB[j].Location == pt2) p2 = j;
                }

                if (pt1 != pt2)
                {
                    // Затем проверим кликнутый прямоугольник и
                    // если у него совпадают координаты по X или Y,
                    // (кроме прямоугольников, расположенных по диагонали)
                    if (
                        (pb.Location.X == pt1.X &&
                         Math.Abs(pb.Location.Y - pt1.Y) == PB[1].Height)
                        ||
                        (pb.Location.Y == pt1.Y &&
                         Math.Abs(pb.Location.X - pt1.X) == PB[1].Width)
                       )
                    {
                        
                        // После успешной проверки
                        // меняем местами пустой и кликнутый прямоугольники.
                        //Point pt = pb.Location;
                        //pb.Location = pt1;
                        //pt1 = pt;
                        PB[p1].Location = pt2;
                        PB[p2].Location = pt1;
                        a = true;
                        p1 = p2 = 0;
                        pt1 = pt2 = pt3;


                        // После каждого хода проверка на полную сборку картинки.

                        // Если хоть у одного прямоугольника не совпадают
                        // реальные координаты и первичные выходим из метода.
                        for (int j = 0; j < PB.Length; j++)
                        {
                            Point point = (Point)PB[j].Tag;
                            if (PB[j].Location != point)
                            {
                                //pt1 = null;
                                return;
                            }
                        }

                        // Если у всех прямоугольников совпали реальные и первичные 
                        // координаты - картинка собрана!
                        for (int m = 0; m < PB.Length; m++) // убираем обрамление прямоугольников
                        {
                            PB[m].Visible = true;
                            PB[m].BorderStyle = BorderStyle.None;
                        }
                        MessageBox.Show("Картинка собрана");
                    }
                }
            }

        }

        // Открытие диалогового окна выбора файла и создание новой области прорисовки картинки.
        private void toolStripButtonLoadPicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDlg = new OpenFileDialog();
            // Фильтр показа только файлов с определенным расширением.
            ofDlg.Filter = "файлы картинок (*.bmp;*.jpg;*.jpeg;)|*.bmp;*.jpg;.jpeg";
            ofDlg.FilterIndex = 1;
            ofDlg.RestoreDirectory = true;

            if (ofDlg.ShowDialog() == DialogResult.OK)
            {
                // Загружаем выбранную картинку.
                Picture = new Bitmap(ofDlg.FileName);
                
                // Создаем новую область прорисовки.
                CreatePictureRegion();
            }
        }

        // Перемешивание прямоугольников, хаотично меняем их координаты.
        private void toolStripButtonMixed_Click(object sender, EventArgs e)
        {
            //ch = 0;
            //c.Show();
            if (Picture == null) return;
            // Создаем объект генерирования случайных чисел,
            
            Random rand = new Random(Environment.TickCount);
            int r = 0;
            for (int i = 0; i < PB.Length; i++)
            {
                PB[i].Visible = true;
                r = rand.Next(0, PB.Length);
                Point ptR = PB[r].Location;
                Point ptI = PB[i].Location;
                PB[i].Location = ptR;
                PB[r].Location = ptI;
                PB[i].BorderStyle = BorderStyle.FixedSingle;

                //if (FormMain.ch == 1)
                //{
                    
                //    PB[i].Click += new EventHandler(PB_Click1);
                //}
                //if (FormMain.ch == 2)
                //{
                    
                //    PB[i].Click += new EventHandler(PB_Click2);
                //}
            }
            if (comboBox1.SelectedItem == "Пятнашки")
            {
                // Случайным образом выбираем пустой прямоугольник и делаем его невидимым
                r = rand.Next(0, PB.Length);
                PB[r].Visible = false;
            }


        }
        public static int ch;
        public static choice c = new choice();




        // Восстанавливаем картинку соответсвенно первичным координатам
        private void toolStripButtonRestore_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < PB.Length; i++)
                {
                    Point pt = (Point)PB[i].Tag;
                    PB[i].Location = pt;
                    PB[i].Visible = true;

                }
                //for (int m = 0; m < PB.Length; m++)
                //{
                //    PB[m].Visible = true;
                //    PB[m].BorderStyle = BorderStyle.None; 

                //}
            } catch
            {
                MessageBox.Show("Сначала загрузите изображение");
            }
        }

        // Открываем диалоговое окно настроек приложения
        private void toolStripButtonSetting_Click(object sender, EventArgs e)
        {
            SetDlg setDlg = new SetDlg();
            setDlg.LengthSides = LengthSides;
            if (setDlg.ShowDialog() == DialogResult.OK)
            {
                LengthSides = setDlg.LengthSides;
                CreatePictureRegion();
            }
        }

        // Открываем окно помощи
        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            //HelpDlg helpDlg = new HelpDlg();
            //helpDlg.ImageDuplicate = Picture;
            //helpDlg.ShowDialog();
            //help h = new help();
            //h.ShowDialog();
            assistMain a = new assistMain();
            a.assistant();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            a = true;
            //checkBox2.Checked = !checkBox1.Checked;
            int r = 0;
            Random rand = new Random(Environment.TickCount);
            CreatePictureRegion();
            toolStripButtonMixed_Click(sender, e);
            if (comboBox1.SelectedItem == "Пятнашки")
            {
                //r = rand.Next(0, PB.Length);
                //PB[r].Visible = false;
                for (int i = 0; i < PB.Length; i++)
                    PB[i].Click += new EventHandler(PB_Click1);
            }
            else
            {
                for (int i = 0; i < PB.Length; i++)
                {
                    PB[i].Visible = true;
                    PB[i].Click += new EventHandler(PB_Click2);
                }
            }

        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    a = true;
        //    //checkBox2.Checked = !checkBox1.Checked;
        //    if (checkBox1.Checked == true) checkBox1.Text = "Пятнашки";
        //    else checkBox1.Text = "Перестановки ";
        //    int r = 0;
        //    Random rand = new Random(Environment.TickCount);
        //    CreatePictureRegion();
        //    toolStripButtonMixed_Click(sender, e);

        //    if (checkBox1.Checked == true)
        //    {
        //        //r = rand.Next(0, PB.Length);
        //        //PB[r].Visible = false;
        //        for (int i = 0; i < PB.Length; i++)
        //            PB[i].Click += new EventHandler(PB_Click1);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < PB.Length; i++)
        //        {
        //            PB[i].Visible = true;
        //            PB[i].Click += new EventHandler(PB_Click2);
        //        }
        //    }
        //}


        //private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    checkBox1.Checked = !checkBox2.Checked;
        //    int r = 0;
        //    Random rand = new Random(Environment.TickCount);
        //    if (checkBox1.Checked == true)
        //    {
        //        r = rand.Next(0, PB.Length);
        //        PB[r].Visible = false;
        //        for (int i = 0; i < PB.Length; i++)
        //            PB[i].Click += new EventHandler(PB_Click1);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < PB.Length; i++)
        //        {
        //            PB[i].Visible = true;
        //            PB[i].Click += new EventHandler(PB_Click2);
        //        }
        //    }

        //}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            aboutAuthor OOPopen = new aboutAuthor();
            OOPopen.ShowDialog();
        }
    }

    public class assistMain
    {
        public virtual void assistant()
        {
            //MessageBox.Show("Error");
            HelpDlg helpDlg = new HelpDlg();
            helpDlg.ImageDuplicate = FormMain.Picture;
            helpDlg.ShowDialog();
        }
    }


}
