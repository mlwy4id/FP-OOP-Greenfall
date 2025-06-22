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

            int targetOffsetX = centerX - playerX;

            int distance = Math.Abs(targetOffsetX - cameraOffset.X);
            double lerpFactor = Math.Min(1.0, 0.05 + (distance / 300.0));

            cameraOffset.X = (int)(lastOffset + (targetOffsetX - lastOffset) * lerpFactor);

            int delta = cameraOffset.X - lastOffset;

            foreach (Control c in form.Controls)
            {
                if (c == player.GetPlayerPictureBox()) continue;
                if (c.Tag?.ToString() == "UI") continue;

                c.Left += delta;
            }

            lastOffset = cameraOffset.X;
        }

    }
}
