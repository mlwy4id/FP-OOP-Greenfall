using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.LevelForm
{
    public class ListOfLevel : Form
    {
        public ListOfLevel()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Levels";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = Image.FromFile("Resources/listLevelsbg.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
