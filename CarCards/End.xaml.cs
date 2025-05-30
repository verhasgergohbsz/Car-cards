using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarCards
{
    /// <summary>
    /// Interaction logic for End.xaml
    /// </summary>
    public partial class End : Window
    {
        public End(int playerPoint, int botPoint)
        {
            InitializeComponent();
            playerScore.Text = playerPoint.ToString();
            botScore.Text = botPoint.ToString();
            if(playerPoint > botPoint)
            {
                result.Text = "Nyertél!";
                VictorySound();
            }
            else if(playerPoint < botPoint)
            {
                result.Text = "Vesztettél";
            }
            else
            {
                result.Text = "Döntetlen";
            }
        }
        private void VictorySound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("sounds/win.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a hang lejátszásakor: {ex.Message}");
            }
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
