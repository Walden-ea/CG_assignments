using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_plot
{
    public partial class Form1 : Form
    {
        //паскаль кейс для чокнутых
        Bitmap picture;
        double a = -10;
        double b = 10;
        double delta;
        bool fitIn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picture = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = picture;

            delta  = (b - a) / pictureBox1.Width;
            fitIn = false;

            Draw(Func());
        }

        Func<double, double> sinx = x => Math.Sin(x);
        Func<double, double> xsquare = x => x*x;

        Func<double,double> Func()
        {
            if (comboBox1.SelectedIndex == 0)
                return sinx;
            else return xsquare;
        }

        void Draw(Func<double, double> func)
        {
            Plot(CalcValues(func, a, b).Item2);
        }

        Tuple<List<double>,List<double>> CalcValues(Func<double,double> func, double a, double b)
        {
            //delta = (b - a) / pictureBox1.Width;

            List<double> args = Enumerable.Range(1, pictureBox1.Width).ToList().Select(x => a + delta * x).ToList();
            List<double> vals = args.Select(x => func(x)).ToList();

            return new Tuple<List<double>, List<double>>(args, vals);
        }

        /*double SqueezingFactor(List<double> values) // фактор раскукоживания (с) Василиса Дельгато
        {
            return pictureBox1.Height / (values.Max() - values.Min());
        }*/

        void Plot(List<double> values, double factor = 1)
        {
            Graphics g = Graphics.FromImage(picture);
            g.Clear(Color.White);
            //double sh = pictureBox1.Height / 2;
            if (fitIn)
            {
                //factor = (pictureBox1.Height - 1) / (values.Max() - values.Min());// SqueezingFactor(values);
                //sh = values.Min();
                for (int i = 1; i < pictureBox1.Width; i++)
                {
                    // picture.SetPixel(i, (int)(sh - (values[i] * factor))-1,Color.Red);
                    //picture.SetPixel(i, (int)((values[i]*(-1) - sh) * factor), Color.Red);
                    /*picture.SetPixel(i,
                        (int)((values[i] * (-1) + (values.Max() - values.Min())) * ((pictureBox1.Height-1)/(values.Max()-values.Min()))),
                        Color.Red);*/
                    factor = (pictureBox1.Height - 1) / (values.Max() - values.Min());
                    int y = (int)(((values[i] * (-1)) +
                     values.Max()) * factor);
                    //int y = (int)(((-1) * values[i] - values.Min()) * factor);
                    if (y > 0 && y < pictureBox1.Height)
                    {
                        picture.SetPixel(i,
                        y,
                        Color.Red);
                    }
                }
            }
            else
            {
                for (int i = 1; i < pictureBox1.Width; i++)
                {
                    // picture.SetPixel(i, (int)(sh - (values[i] * factor))-1,Color.Red);
                    //picture.SetPixel(i, (int)((values[i]*(-1) - sh) * factor), Color.Red);
                    int y = (int)((values[i] * (-1)) * (1 / delta) +
                        pictureBox1.Height / 2);
                    if (y > 0 && y < pictureBox1.Height)
                    {
                        picture.SetPixel(i, y, Color.Red);
                    }
                }
                //factor = 1 / delta;
                //sh = pictureBox1.Height / 2;
            }


        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fitIn = !fitIn;
            Draw(Func());
            pictureBox1.Refresh();
            if (fitIn)
            {
                button1.Text = "скукожить";
            }
            else
            {
                button1.Text = "раскукожить";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Draw(Func());
            pictureBox1.Refresh();
        }
    }
}
