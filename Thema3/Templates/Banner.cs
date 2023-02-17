using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thema3
{
    internal class Banner : PictureBox
    {
        private Image img = Image.FromFile("../../Image/logo.png");
        public Banner(int width, int bannerWidth)
        {
            double bannerHeight = bannerWidth / 2.5;
            Image = img;
            SizeMode = PictureBoxSizeMode.StretchImage;
            Size = new Size(bannerWidth, (int)bannerHeight);
            Location = new Point(width / 2 - Width / 2, 10);
        }
    }
}
