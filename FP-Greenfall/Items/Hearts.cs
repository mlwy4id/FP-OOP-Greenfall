using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Items
{
    public class Hearts
    {
        private PictureBox heartsPictureBox;
        private Image heartImg;

        private int heartsWidth;
        private int heartsHeight;

        public Hearts(Point position)
        {
            heartsWidth = 40;
            heartsHeight = 40;

            using (MemoryStream ms = new MemoryStream(Resource.Heart.HeartImg))
            { 
                heartImg = Image.FromStream(ms);
            };

            heartsPictureBox = new PictureBox
            {
                Size = new Size(heartsWidth, heartsHeight),
                Location = position,
                Image = heartImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Tag = "HealingItem"
            };
        }

        public PictureBox GetHeartsPictureBox() => heartsPictureBox;
    }
}
