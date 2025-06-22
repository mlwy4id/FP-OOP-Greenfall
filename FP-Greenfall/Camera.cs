using FP_Greenfall.Sprites;
using System.Windows.Forms;

public class Camera
{
    private int worldOffsetX;
    private int worldOffsetY;

    public int WorldOffsetX => worldOffsetX;
    public int WorldOffsetY => worldOffsetY;

    public Camera()
    {
        worldOffsetX = 0;
        worldOffsetY = 0;
    }

    public void UpdateCamera(Form form, Player player)
    {
        var playerBox = player.GetPlayerPictureBox();
        if (playerBox == null) return;

        int playerCenterX = playerBox.Left + (playerBox.Width / 2);
        int playerCenterY = playerBox.Top + (playerBox.Height / 2);

        int screenCenterX = form.ClientSize.Width / 2;
        int screenCenterY = form.ClientSize.Height - 200;

        int deltaX = screenCenterX - playerCenterX;
        int deltaY = screenCenterY - playerCenterY;

        if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
        {
            foreach (Control control in form.Controls)
            {
                if (control == playerBox) continue;
                if (control.Tag?.ToString() == "UI") continue;

                control.Left += deltaX;
                control.Top += deltaY;
            }

            if(player.FacingLeft)
            {
                playerBox.Left += deltaX - player.Speed;
            } else
            {
                playerBox.Left += deltaX + player.Speed;       
            }

            playerBox.Top += deltaY;

            worldOffsetX += deltaX;
            worldOffsetY += deltaY;
        }
    }
}
