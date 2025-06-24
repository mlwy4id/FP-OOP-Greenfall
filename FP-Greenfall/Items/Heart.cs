using FP_Greenfall.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.items
{
    public class Heart : Items
    {
        public Heart(Point position)
        {
            itemsWidth = 40;
            itemsHeight = 40;

            using (MemoryStream ms = new MemoryStream(Resource.Heart.HeartImg))
            { 
                itemsImg = Image.FromStream(ms);
            };

            itemsPictureBox = new PictureBox
            {
                Size = new Size(itemsWidth, itemsHeight),
                Location = position,
                Image = itemsImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Tag = "HealingItem"
            };
        }
    }
}
