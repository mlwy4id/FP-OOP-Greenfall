using FP_Greenfall.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FP_Greenfall.Sprites;

namespace FP_Greenfall
{
    public class Camera
    {
        private Point cameraOffset;
        private int lastOffset;

        public Camera(Point point)
        {
            cameraOffset = point;
        }

        public void UpdateCamera(Form form, Player player)
        {
            int centerX = form.ClientSize.Width / 2;
            int playerX = player.GetPlayerPictureBox().Left;

            cameraOffset.X = centerX - playerX;

            int delta = cameraOffset.X - lastOffset;
            int speed = 8;

            foreach (Control c in form.Controls)
            {
                if (c != player.GetPlayerPictureBox())
                {
                    if(player.MovingLeft)
                    {
                        c.Left += delta + speed;
                    } else if (player.MovingRight)
                    {
                        c.Left += delta - speed;
                    }
                }
            }

            lastOffset = cameraOffset.X;
        }
    }
}
