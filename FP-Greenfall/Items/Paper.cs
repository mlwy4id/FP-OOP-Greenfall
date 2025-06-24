using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.items
{
    public class Paper : Items
    {
        public Paper(Point position)
        {
            itemsWidth = 40;
            itemsHeight = 40;

            using (MemoryStream ms = new MemoryStream(Resource.Paper.Papers))
            {
                itemsImg = Image.FromStream(ms);
            }
            ;

            itemsPictureBox = new PictureBox
            {
                Size = new Size(itemsWidth, itemsHeight),
                Location = position,
                Image = itemsImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Tag = "PaperItem"
            };
        }
    }
}
