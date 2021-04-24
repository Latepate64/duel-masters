using DuelMastersInterfaceModels.Cards;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DuelMastersClientForms
{
    internal class CreatureView : FlowLayoutPanel
    {
        internal CreatureView(CreatureWrapper creature)
        {
            //TODO: Consider multiple civilizations
            BackColor = GetCivilizationColor(creature.Civilizations[0]);
            Height = (int)(CardScale * CardHeight);
            Width = (int)(CardScale * CardWidth);

            Enabled = true;

            Label cost = new() { Text = creature.Cost.ToString() };
            Label name = new() { Text = CardNames[creature.CardID] };
            Label race = new() { Text = GetRaceText(creature.Races) };
            Label power = new() { Text = creature.Power.ToString() };
            Controls.Add(cost);
            Controls.Add(name);
            Controls.Add(race);
            Controls.Add(power);

            MouseDown += Card_MouseDown;
            MouseMove += CreatureView_MouseMove;
        }

        private void CreatureView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left = e.X + Left - MouseDownLocation.X;
                Top = e.Y + Top - MouseDownLocation.Y;
            }
        }

        internal const int CardWidth = 222;
        internal const int CardHeight = 307;
        internal const double CardScale = 0.395;

        private Point MouseDownLocation;

        private static string GetRaceText(IEnumerable<Race> races)
        {
            return string.Join(" / ", races.Select(r => _races[r]));
        }

        private static Color GetCivilizationColor(Civilization civilization)
        {
            return _civilizationColors[civilization];
        }

        private void Card_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        internal static readonly Dictionary<CardIdentifier, string> CardNames = new()
        {
            { CardIdentifier.AquaHulcus, "Aqua Hulcus" },
            { CardIdentifier.BurningMane, "Burning Mane" },
        };

        private static readonly Dictionary<Race, string> _races = new()
        {
            { Race.BeastFolk, "Beast Folk" },
            { Race.LiquidPeople, "Liquid People" },
        };

        private static readonly Dictionary<Civilization, Color> _civilizationColors = new()
        {
            { Civilization.Light, Color.LightYellow },
            { Civilization.Water, Color.LightBlue },
            { Civilization.Darkness, Color.LightGray },
            { Civilization.Fire, Color.Red },
            { Civilization.Nature, Color.LightGreen },
        };
    }
}
