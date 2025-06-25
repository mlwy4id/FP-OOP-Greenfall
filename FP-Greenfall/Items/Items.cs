using FP_Greenfall.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.items
{
    public class Items
    {
        protected PictureBox itemsPictureBox;
        protected Image itemsImg;

        protected int itemsWidth;
        protected int itemsHeight;

        public bool isClaimed = false;

        public PictureBox GetItemPictureBox() => itemsPictureBox;
    }
}
