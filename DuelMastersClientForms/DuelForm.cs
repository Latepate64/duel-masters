using System.Drawing;
using System.Windows.Forms;

namespace DuelMastersClientForms
{
    public partial class DuelForm : Form
    {
        public DuelForm()
        {
            InitializeComponent();
        }

        private static FlowLayoutPanel CreateCreature()
        {
            const int Width = 222;
            const int Height = 307;
            double scale = 0.395;
            Label cost = new() { Text = "2" };
            Label name = new() { Text = "Burning Mane" };
            Label race = new() { Text = "Beast Folk" };
            Label power = new() { Text = "2000" };
            FlowLayoutPanel card = new() { BackColor = Color.Green, Height = (int)(scale * Height), Width = (int)(scale * Width) };
            card.Controls.Add(cost);
            card.Controls.Add(name);
            card.Controls.Add(race);
            card.Controls.Add(power);
            return card;
        }

        private void CreateHandCardButton_Click(object sender, System.EventArgs e)
        {
            FlowLayoutPanel card = CreateCreature();
            PlayerHand.Controls.Add(card);
        }

        private void PlayerHand_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PlayerBattleZone_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
