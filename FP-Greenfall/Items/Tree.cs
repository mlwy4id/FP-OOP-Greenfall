using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.items
{
    public class Tree : Items
    {
        public Tree(Point position)
        {
            itemsWidth = 150;
            itemsHeight = 150;

            using (MemoryStream ms = new MemoryStream(Resource.Tree.Trees))
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
                Tag = "Tree"
            };
        }
    }
}
