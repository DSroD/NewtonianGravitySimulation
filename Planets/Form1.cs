using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Planets
{
    public partial class Form1 : Form
    {

        BodySystem s;
        BufferedGraphicsContext contx;
        BufferedGraphics bg;

        float deltaT = 0.01f;
        float size = 12f;
        bool drawPath = false;
        bool showInfo = false;
        bool pause = false;

        bool follow;
        Body fl;

        float posx = 0;
        float posy = 0;

        bool drag = false;
        float prx;
        float pry;

        int szx;
        int szy;


        public Form1()
        {
            InitializeComponent();
            Body[] bds = new Body[] {
                new Body("Sun", 1f, new Vector(0f, 0f), new Vector(0f, 0f), Color.Orange)
                ,new Body("Mercury", 1.66f * (float)Math.Pow(10, -7), new Vector(0f, 1.405f), new Vector(0.439f, 0f), Color.PaleVioletRed)
                ,new Body("Venus",  2.45f * (float)Math.Pow(10, -6), new Vector(0f, -1.177f), new Vector(-0.7227f, 0f), Color.OrangeRed)
                ,new Body("Earth", 3f * (float)Math.Pow(10, -6), new Vector(0.9835f, 0f), new Vector(0f, -1.01409533f), Color.Blue)
                ,new Body("Moon", 3.694f * (float)Math.Pow(10, -8), new Vector(0.9493f, 0f), new Vector(0, -1.01671033f), Color.Gray)
                ,new Body("Mars", 3.23f * (float)Math.Pow(10, -7), new Vector(0f, -0.869f), new Vector(-1.417f, 0f), Color.Red)
                ,new Body("Jupiter", 9.54f * (float)Math.Pow(10, -4), new Vector(0f, -0.4225f), new Vector(-5.397f, 0f), Color.DarkOrange)
                ,new Body("Ganymede", 7.4506f * (float)Math.Pow(10, -8), new Vector(0f, -0.7877f), new Vector(-5.4041552f, 0f), Color.YellowGreen)
                ,new Body("Saturn", 2.86f * (float)Math.Pow(10, -4), new Vector(-0.3058f, 0f), new Vector(0f, 10.07f), Color.Orange)
                ,new Body("Titan", 6.7652f * (float)Math.Pow(10, -8), new Vector(-0.4928f, 0f), new Vector(0f, 10.07817108f), Color.White)
                ,new Body("Uranus", 4.37f * (float)Math.Pow(10, -5), new Vector(0.2199f, 0f), new Vector(0f, -19.88f), Color.Teal)
                ,new Body("Neptune", 5.15f * (float)Math.Pow(10, -5), new Vector(0f, -0.1829f), new Vector(-29.94f, 0f), Color.DarkBlue)
                //,new Body("Anotha Star", 14f, new Vector(1.082f, -0.128f), new Vector(-60.06f, 15f), Color.OrangeRed)

            };
            s = new BodySystem(bds, 1);

            szx = this.Width;
            szy = this.Height;
            contx = BufferedGraphicsManager.Current;
            bg = contx.Allocate(this.CreateGraphics(), new Rectangle(0, 0, szx, szy));

            timer1.Interval = 50;
            timer1.Start();
        }

        private void drawPlanets()
        {
            if(szx != this.Width || szy != this.Height)
            {
                szx = this.Width;
                szy = this.Height;
                contx = BufferedGraphicsManager.Current;
                bg = contx.Allocate(this.CreateGraphics(), new Rectangle(0, 0, szx, szy));
            }


            using (Font f = new Font(FontFamily.GenericSansSerif, 8))
            {
                bg.Graphics.Clear(Color.Black);
                foreach (Body b in s.Bodies)
                {
                    bool toosmall = false;
                    bool toosmall2 = false;
                    float sz = 30 * b.Mass / size;
                    if (sz <= 4 && sz > 0.0001f)
                    {
                        sz = 4;
                    }
                    else if (sz <= 0.0001f && sz > 0.000006f)
                    {
                        sz = 3;
                    }
                    else if (sz <= 0.000006f && sz > 0.0000004f)
                    {
                        sz = 2;
                        toosmall2 = true;
                    }
                    else if(sz <= 0.0000004f && sz > 0.0000001f)
                    {
                        sz = 1;
                        toosmall = true;
                        toosmall2 = true;
                    }
                    else if (sz <= 0.0000001f)
                    {
                        continue;
                    }

                    float flx = 0;
                    float fly = 0;

                    if(fl != null)
                    {
                        flx = fl.position.X;
                        fly = fl.position.Y;
                    }

                    using (Brush br = new SolidBrush(b.Color))
                    {
                        if (drawPath)
                        {
                            using (Pen pen = new Pen(br, 1))
                            {
                                for(int i = 0; i < b.p.Length - 1; i++)
                                {
                                    bg.Graphics.DrawLine(pen, scale(-size, size, 0, szx, b.p[i].X - posx - flx), scale(-size, size, 0, szy, b.p[i].Y - posy - fly), scale(-size, size, 0, szx, b.p[i + 1].X - posx - flx), scale(-size, size, 0, szy, b.p[i + 1].Y - posy - fly));
                                }
                            }
                        }
                        bg.Graphics.FillEllipse(br, scale(-size, size, 0, szx, b.position.X - posx - flx) - sz / 2, scale(-size, size, 0, szy, b.position.Y - posy - fly) - sz / 2, sz, sz);
                        if(!toosmall)
                        bg.Graphics.DrawString(b.Name, f, br, scale(-size, size, 0, szx, b.position.X - posx - flx) + sz + 1, scale(-size, size, 0, szy, b.position.Y - posy - fly) + sz + 3);
                        if (showInfo && !toosmall2)
                        {
                            bg.Graphics.DrawString("Position: (" + b.position.X + "," + b.position.Y + ")", f, br, scale(-size, size, 0, szx, b.position.X - posx - flx) + sz + 1, scale(-size, size, 0, szy, b.position.Y - posy - fly) + sz + 14);
                            bg.Graphics.DrawString("Velocity elements: (" + b.tendency.X + "," + b.tendency.Y + ")", f, br, scale(-size, size, 0, szx, b.position.X - posx - flx) + sz + 1, scale(-size, size, 0, szy, b.position.Y - posy - fly) + sz + 26);
                            bg.Graphics.DrawString("Velocity: " + Math.Sqrt(Math.Pow(b.tendency.X, 2) + Math.Pow(b.tendency.Y, 2)), f, br, scale(-size, size, 0, szx, b.position.X - posx - flx) + sz + 1, scale(-size, size, 0, szy, b.position.Y - posy - fly) + sz + 38);
                            if (b.Name != "Sun")
                            {
                                bg.Graphics.DrawString("Distance from Sun: " + Math.Sqrt(Math.Pow(b.position.X - s.Bodies[0].position.X, 2) + Math.Pow(b.position.Y - s.Bodies[0].position.Y, 2)), f, br, scale(-size, size, 0, szx, b.position.X - posx - flx) + sz + 1, scale(-size, size, 0, szy, b.position.Y - posy - fly) + sz + 50);
                            }
                        }
                    }
                }
            }
            bg.Render();
        }



        private float scale(float pmin, float pmax, float nmin, float nmax, float s)
        {
            float prange = pmax - pmin;
            float nrange = nmax - nmin;
            float nir = (s-pmin) * nrange / prange;
            return nir + nmin;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!pause)
            {
                s.simulateStep(deltaT);
                drawPlanets();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.deltaT = (float)trackBar1.Value / 10000;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.size = (float)trackBar2.Value / 1000;
            drawPlanets();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.drawPath = checkBox1.Checked;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!drag && e.Button == MouseButtons.Left)
            {
                prx = (posx + e.X * size / 200);
                pry = (posy + e.Y * size / 200);
                drag = true;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                posx = (prx - e.X * size / 200);
                posy = (pry - e.Y * size / 200);
                drawPlanets();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.showInfo = checkBox2.Checked;
            drawPlanets();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.pause = checkBox3.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo sInfo = new System.Diagnostics.ProcessStartInfo("http://dezrodino.mzf.cz");
            System.Diagnostics.Process.Start(sInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fl = s.getBody(textBox1.Text, false);
            if(fl == null)
            {
                label6.Text = "Following: none";
            }
            else
            {
                label6.Text = "Following: " + fl.Name;
                posx = 0;
                posy = 0;
            }
        }
    }
}
