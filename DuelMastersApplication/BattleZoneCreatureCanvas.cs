﻿using DuelMastersModels.Abilities;
using DuelMastersModels.Abilities.Static;
using DuelMastersModels.Abilities.Trigger;
using DuelMastersModels.Cards;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DuelMastersApplication
{
    public class BattleZoneCreatureCanvas : RectangleCardCanvas
    {
        public static readonly DependencyProperty SummoningSicknessProperty = DependencyProperty.Register("SummoningSickness", typeof(bool), typeof(BattleZoneCreatureCanvas), new PropertyMetadata(OnSummoningSicknessChanged));
        public static readonly DependencyProperty AbilitiesProperty = DependencyProperty.Register("Abilities", typeof(ReadOnlyAbilityCollection), typeof(BattleZoneCreatureCanvas), new PropertyMetadata(OnAbilitiesChanged));
        public static readonly DependencyProperty CivilizationProperty = DependencyProperty.Register("Civilizations", typeof(ReadOnlyCivilizationCollection), typeof(BattleZoneCreatureCanvas), new PropertyMetadata(OnCivilizationsChanged));

        private Image _imageSummoningSickness = new Image() { Stretch = Stretch.Fill, Opacity = 0, Source = new BitmapImage(new Uri("../../Images/summoning_sickness.png", UriKind.Relative)) };
        private Rectangle _rectanglePowerFrame = new Rectangle() { Fill = new SolidColorBrush(Colors.Black) };
        private ItemsControl _abilitiesItemsControl = new ItemsControl() { HorizontalContentAlignment = HorizontalAlignment.Left, Background = Brushes.Transparent, IsEnabled = true };

        public BattleZoneCreatureCanvas()
        {
            CanvasFrame.Children.Add(RectangleColorFrame);
            CanvasFrame.Children.Add(Artwork);
            CanvasFrame.Children.Add(TextBoxCardName);
            CanvasFrame.Children.Add(_rectanglePowerFrame);
            CanvasFrame.Children.Add(TextBoxPower);
            CanvasFrame.Children.Add(_imageSummoningSickness);
            Children.Add(_abilitiesItemsControl);

            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            stackPanelFactory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
            _abilitiesItemsControl.ItemsPanel = new ItemsPanelTemplate() { VisualTree = stackPanelFactory };

            Artwork.Stretch = Stretch.Fill;

            TextBoxPower.HorizontalAlignment = HorizontalAlignment.Center;
            SizeChanged += CardCanvas_SizeChanged;

            _rectanglePowerFrame.SetBinding(Shape.FillProperty, new Binding("Fill") { Source = RectangleCardFrame });
        }

        private void CardCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double cardCanvasWidth = e.NewSize.Width;
            double cardCanvasHeight = e.NewSize.Height;
            double frameWidth = 0.95 * e.NewSize.Width;
            double frameHeight = 0.9 * e.NewSize.Width;
            AdjustSize(cardCanvasWidth, cardCanvasHeight, frameWidth, frameHeight);

            const double FontScalePower = 0.12;
            double fontSizeCardType = Math.Max(MinimumFontSize, cardCanvasHeight * FontScalePower);
            TextBoxPower.FontSize = fontSizeCardType;

            AdjustCardNameSize(frameWidth, frameHeight, fontScale: 0.07, yPositionScale: 0.07);

            Artwork.Width = (1 - 2 * FrameOffset) * frameWidth;
            Artwork.Height = 0.8 * frameHeight;
            SetLeft(Artwork, (frameWidth - Artwork.Width) / 2);
            SetTop(Artwork, 0.115 * frameWidth);

            TextBoxPower.Measure(new Size(frameWidth, 0.2 * frameHeight));
            TextBoxPower.Arrange(new Rect(TextBoxPower.DesiredSize));
            SetLeft(TextBoxPower, (frameWidth - TextBoxPower.ActualWidth) / 2);
            SetTop(TextBoxPower, (1 - FrameOffset) * frameHeight * 0.98 - TextBoxPower.ActualHeight / 2);

            _imageSummoningSickness.Width = (1 - 2 * FrameOffset) * frameWidth;
            _imageSummoningSickness.Height = 0.8 * frameHeight;
            SetLeft(_imageSummoningSickness, (frameWidth - _imageSummoningSickness.Width) / 2);
            SetTop(_imageSummoningSickness, 0.115 * frameWidth);

            _rectanglePowerFrame.Width = 1.1 * TextBoxPower.ActualWidth;
            _rectanglePowerFrame.Height = 1.1 * TextBoxPower.ActualHeight;
            SetLeft(_rectanglePowerFrame, (frameWidth - _rectanglePowerFrame.Width) / 2);
            SetTop(_rectanglePowerFrame, (1 - FrameOffset) * frameHeight * 0.98 - _rectanglePowerFrame.Height / 2);

            _abilitiesItemsControl.Width = 0.2 * frameWidth;
            _abilitiesItemsControl.Height = frameHeight + _abilitiesItemsControl.Width / 2;

            while (_abilitiesItemsControl.Items.Count > 0)
            {
                AbilityIconCanvas item = _abilitiesItemsControl.Items[0] as AbilityIconCanvas;
                double totalHeight = _abilitiesItemsControl.Items.Count * _abilitiesItemsControl.Width;
                if (totalHeight > _abilitiesItemsControl.Height)
                {
                    _abilitiesItemsControl.Width = 0.99 * _abilitiesItemsControl.Width;
                    
                }
                else
                {
                    break;
                }
            }
            
            SetLeft(_abilitiesItemsControl, GetLeft(this) - _abilitiesItemsControl.Width / 2);
            SetBottom(_abilitiesItemsControl, GetBottom(this));

            RectangleColorFrame.Width = frameWidth - frameWidth * FrameOffset;
            RectangleColorFrame.Height = frameHeight - frameHeight * FrameOffset;
            SetLeft(RectangleColorFrame, (frameWidth - RectangleColorFrame.Width) / 2);
            SetTop(RectangleColorFrame, (frameHeight - RectangleColorFrame.Height) / 2);
        }

        private static void OnSummoningSicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BattleZoneCreatureCanvas canvas = d as BattleZoneCreatureCanvas;
            bool summoningSickness = (bool)e.NewValue;
            if (summoningSickness)
            {
                canvas._imageSummoningSickness.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, new Duration(new TimeSpan(0, 0, 0, 1, 200)))
                {
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                });
            }
            else
            {
                canvas._imageSummoningSickness.BeginAnimation(OpacityProperty, null);
            }
        }

        private static void OnAbilitiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BattleZoneCreatureCanvas canvas = d as BattleZoneCreatureCanvas;
            ReadOnlyAbilityCollection abilities = e.NewValue as ReadOnlyAbilityCollection;
            foreach (Ability ability in abilities)
            {
                if (ability is Blocker)
                {
                    AddAbilityIcon(canvas, "blocker");
                }
                else if (ability is ThisCreatureCannotAttackPlayers)
                {
                    AddAbilityIcon(canvas, "this_creature_cannot_attack_players");
                }

                else if (ability is TriggerAbility triggerAbility)
                {
                    if (triggerAbility.TriggerCondition is WhenYouPutThisCreatureIntoTheBattleZone)
                    {
                        AddAbilityIcon(canvas, "put_into_battle_zone");
                    }
                    else
                    {
                        AddAbilityIcon(canvas, "trigger_ability");
                    }
                }
            }
        }

        private static void OnCivilizationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BattleZoneCreatureCanvas).RectangleColorFrame.Fill = GetBrushForCivilizations(e.NewValue as ReadOnlyCivilizationCollection);
        }

        private static void AddAbilityIcon(BattleZoneCreatureCanvas canvas, string fileName)
        {
            canvas._abilitiesItemsControl.Items.Add(new AbilityIconCanvas(new Uri(string.Format("../../Images/AbilityIcons/{0}.png", fileName), UriKind.Relative), canvas._abilitiesItemsControl));
        }
    }
}
