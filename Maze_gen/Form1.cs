using System;

using System.Drawing;

using System.Windows.Forms;

namespace Maze_gen
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            
            InitializeComponent();
            textBox3.ReadOnly = true;

        }

        bool begin = false;
        int pass = 1;
        int wall = 0;
        int side = 2;

        int height = 0;
        int width = 0;
        int[,] maze;




       
        void DrawBox(int x, int y, Brush br, Graphics gr)
        {
            gr.FillRectangle(br, x * side, y * side, side, side);
        }
        bool deadend(int x, int y, int[,] maze, int height, int width, int pass, int wall)
        {
            int a = 0;

            if (x != 1)
            {
                if (maze[y, x - 2] == pass)
                {
                    a += 1;
                }
            }
            else
            {
                a += 1;
            }

            if (y != 1)
            {
                if (maze[y - 2, x] == pass)
                {
                    a += 1;
                }
            }
            else
            {
                a += 1;
            }

            if (x != width - 2)
            {
                if (maze[y, x + 2] == pass)
                {
                    a += 1;
                }
            }
            else
            {
                a += 1;
            }

            if (y != height - 2)
            {
                if (maze[y + 2, x] == pass)
                {
                    a += 1;
                }
            }
            else
            {
                a += 1;
            }

            if (a == 4)
            {
                return true;
            }
            else
            {
                return false; //клеток больше нет, завершить цикл
            }
        }
        void mazemake(int[,] maze, int height, int width, Random r, int pass, int wall)
        {
            int x_width;
            int y_height;
            int c;
            int a;

            x_width = 3; y_height = 3; a = 0; // Точка приземления крота и счетчик
            while (a < 1000000)   // Выход из цикла
            {
                maze[y_height, x_width] = pass; // y - высота, x_width - ширина
                a++;

                while (true) // Бесконечный цикл, который прерывается только тупиком
                {
                    c = (r.Next(0, 4)); // Напоминаю, что крот прорывает по две клетки в одном направлении за прыжок

                    switch (c)          // 
                    {
                        case 0: // Вверх
                            if (y_height != 1)
                            {
                                if (maze[y_height - 2, x_width] == wall)
                                {
                                    maze[y_height - 1, x_width] = pass;
                                    maze[y_height - 2, x_width] = pass;
                                    y_height -= 2;
                                }
                            }
                            break;

                        case 1: // Вниз
                            if (y_height != height - 2)
                            {
                                if (maze[y_height + 2, x_width] == wall)
                                {
                                    maze[y_height + 1, x_width] = pass;
                                    maze[y_height + 2, x_width] = pass;
                                    y_height += 2;
                                }
                            }
                            break;

                        case 2: // Налево
                            if (x_width != 1)
                            {
                                if (maze[y_height, x_width - 2] == wall)
                                {
                                    maze[y_height, x_width - 1] = pass;
                                    maze[y_height, x_width - 2] = pass;
                                    x_width -= 2;
                                }
                            }
                            break;

                        case 3: // Направо
                            if (x_width != width - 2) //массив не четный, по этому это проверка на нечетное число
                            {
                                if (maze[y_height, x_width + 2] == wall)
                                {
                                    maze[y_height, x_width + 1] = pass;
                                    maze[y_height, x_width + 2] = pass;
                                    x_width += 2;
                                }
                            }
                            break;
                    }
                    if (deadend(x_width, y_height, maze, height, width, pass, wall))
                    {
                        break;
                    }
                }

                if (deadend(x_width, y_height, maze, height, width, pass, wall)) // Вытаскиваем крота из тупика, генерируем координаты - четные
                {
                    do
                    {
                        /*x_width = 2 * (r.Next() % ((width - 1) / 2)) + 1;
                        y_height = 2 * (r.Next() % ((height - 1) / 2)) + 1;*/
                         x_width = r.Next(1,width-1);
                        if (x_width % 2 ==0)
                        {
                            x_width++;
                        }
                         y_height = r.Next(1, height-1);
                        if (y_height % 2 == 0)
                        {
                            y_height++;
                        }
                    } while (maze[y_height, x_width] != pass);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            try
            {
                side = Convert.ToInt32(textBox3.Text);

                if (side <= 0)
                {
                    MessageBox.Show("Введенное значение должно быть числом и числом больше 0.");
                    return;
                }
                //Main();
                
                height = Convert.ToInt32(textBox1.Text);
                if (height % 2 == 0 & height != 0)
                {
                    height++;
                }
                else if (height <= 0 || height <=5)
                {
                    MessageBox.Show("Введенное значение должно быть числом и числом больше 0.");
                    return;
                }
                width = Convert.ToInt32(textBox2.Text);
                if (width % 2 == 0 & width != 0)
                {
                    width++;
                }
                else if (width <= 0 || width <= 5)
                {
                    MessageBox.Show("Введенное значение должно быть числом и числом больше 0.");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Введенное значение должно быть натуральным числом и числом больше 0.");
                return;
            }
            
            begin = true;
            maze = new int[height, width];

            mazemake(maze, height, width, r, pass, wall);

          //  panel1.Size = new Size(width * side, height * side);

            
            panel1.Invalidate();
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (begin == true) {
                progressBar1.Value = 0;
                //double progVal = (width * height) / 100;
                //int step = Convert.ToInt16(Math.Round(progVal));
                progressBar1.Minimum = 0;
                progressBar1.Maximum = width;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        switch (maze[j, i])
                        {
                            case 0:
                                //Console.Write("# ");
                                DrawBox(i, j, Brushes.Black, e.Graphics);
                                break;
                            case 1:
                                // Console.Write("  ");
                                //DrawBox(i, j, Brushes.White, e.Graphics);
                                break;
                        }
                    }
                    progressBar1.Value += 1;
                    //Console.Write("\n");
                }
                begin = false;
            }
            else
            {
                return;
            }
            
           // Console.Write("\n");
        }

        private void trackBar1_Scroll(object sender, EventArgs e) //обработка трек бара
        {
            int x = Convert.ToUInt16(trackBar1.Value);

            textBox3.Text = Convert.ToString(x);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)  //полное закрытие из вторичной формы
        {
            Application.Exit();
        }
    }

}


