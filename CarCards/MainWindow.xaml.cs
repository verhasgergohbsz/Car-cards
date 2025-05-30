using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarCards.DbAcces;
using CarCards.Model;
using MySql.Data.Common;
using MySql.Data.MySqlClient;

namespace CarCards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Card> _allCards;
        List<Card> _playerCards;
        List<Card> _botCards;
        int round = 0;
        int playerPoint = 0;
        int botPoint = 0;


        DbConnection _connection;

        
        public void Get()
        {
            const string _connectionString = "server=localhost;port=3306;uid=root;pwd=;database=autok";
            var connection = new MySqlConnection(_connectionString);
            _connection = connection;
            connection.Open();

            _allCards = new List<Card>();
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = "SELECT `id`,`marka`,`tipus`,`orszag`,`uzemanyag`,`evjarat`,`vegsebesseg`,`gyorsulas`,`suly`,`terfogat`,`loero`,`nyomatek`,`kep_utvonala` FROM `autokartya`;";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var card = new Card
                        {
                            Id = reader.GetInt32(0),
                            Marka = reader.GetString(1),
                            Tipus = reader.GetString(2),
                            Orszag = reader.GetString(3),
                            Uzemanyag = reader.GetString(4),
                            Evjarat = reader.GetInt32(5),
                            Vegsebesseg = reader.GetInt32(6),
                            Gyorsulas = reader.GetFloat(7),
                            Suly = reader.GetInt32(8),
                            Terfogat = reader.GetInt32(9),
                            Loero = reader.GetInt32(10),
                            Nyomatek = reader.GetInt32(11),
                            Kep_utvonala = reader.GetString(12)
                        };
                        _allCards.Add(card);
                    }
                }
            }

        }

        public void Shuffle()
        {
            _playerCards = new  List<Card>();
            _botCards = new List<Card>();
            Random rnd = new Random();
            for(int i = 0; i < 5; i++)
            {
                int index = rnd.Next(_allCards.Count);
                _playerCards.Add(_allCards[index]);
                _allCards.RemoveAt(index);

                index = rnd.Next(_allCards.Count);
                _botCards.Add(_allCards[index]);
                _allCards.RemoveAt(index);
            }
        }

        public void UpdateUI()
        {
            //scoreboard
            playerScore.Text = playerPoint.ToString();
            botScore.Text = botPoint.ToString();
            //jatekos kartyainak frissitese
            var currentPlayerCard = _playerCards[0];
            playerMarka.Text = currentPlayerCard.Marka;
            playerTipus.Text = currentPlayerCard.Tipus;
            playerEvjarat.Text = currentPlayerCard.Evjarat.ToString();
            playerOrszag.Text = currentPlayerCard.Orszag;
            playerUzemanyag.Text = currentPlayerCard.Uzemanyag;
            //playerKep.Source = new BitmapImage(new Uri($"{currentPlayerCard.Kep_utvonala}"));

            vegsebesseg.Content = currentPlayerCard.Vegsebesseg.ToString();
            gyorsulas.Content = currentPlayerCard.Gyorsulas.ToString();
            suly.Content = currentPlayerCard.Suly.ToString();
            terfogat.Content = currentPlayerCard.Terfogat.ToString();
            loero.Content = currentPlayerCard.Loero.ToString();
            nyomatek.Content = currentPlayerCard.Nyomatek.ToString();

            
            //bot karyta
            var currentBotCard = _botCards[0];
            botMarka.Text = currentBotCard.Marka;
            botTipus.Text = currentBotCard.Tipus;
            botEvjarat.Text = currentBotCard.Evjarat.ToString();
            botOrszag.Text= currentBotCard.Orszag;
            botUzemanyag.Text = currentBotCard.Uzemanyag;
            //botKep.Source = new BitmapImage(new Uri($"{currentBotCard.Kep_utvonala}"));
        }
        private void AttributeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playerCards.Count == 0 || _botCards.Count == 0)
                return;

            Button clickedButton = sender as Button;
            string attribute = clickedButton.Tag.ToString();

            Card playerCar = _playerCards[0];
            Card botCar = _botCards[0];

            double playerValue = 0;
            double botValue = 0;
            //ha a kisebb erteknek kell nyernie, nem a nagyobbnak
            bool lower = false;

            // Attribútum lekérdezés és összehasonlítási szabály
            switch (attribute)
            {
                case "Vegsebesseg":
                    playerValue = playerCar.Vegsebesseg;
                    botValue = botCar.Vegsebesseg;
                    break;

                case "Gyorsulas":
                    playerValue = playerCar.Gyorsulas;
                    botValue = botCar.Gyorsulas;
                    lower = true;
                    break;

                case "Suly":
                    playerValue = playerCar.Suly;
                    botValue = botCar.Suly;
                    lower = true;
                    break;

                case "Terfogat":
                    playerValue = playerCar.Terfogat;
                    botValue = botCar.Terfogat;
                    break;

                case "Loero":
                    playerValue = playerCar.Loero;
                    botValue = botCar.Loero;
                    break;

                case "Nyomatek":
                    playerValue = playerCar.Nyomatek;
                    botValue = botCar.Nyomatek;
                    break;
            }

            string resultMessage;

            if (playerValue == botValue)
            {
                resultMessage = $"Döntetlen! ({playerValue} = {botValue})";
            }
            else if ((lower && playerValue < botValue) || (!lower && playerValue > botValue))
            {
                resultMessage = $"Nyertél! ({playerValue} vs {botValue})";
                playerPoint++;
            }
            else
            {
                resultMessage = $"Vesztettél! ({playerValue} vs {botValue})";
                botPoint++;
            }

            MessageBox.Show(resultMessage, "Eredmény", MessageBoxButton.OK, MessageBoxImage.Information);

            _playerCards.RemoveAt(0);
            _botCards.RemoveAt(0);

            if (_playerCards.Count == 0)
            {
                End endWindow = new End(playerPoint, botPoint);
                endWindow.Show();
                this.Close();
                return;
            }

            UpdateUI();
        }





        public MainWindow(string name)
        {
            InitializeComponent();

            playerName.Text = name;
            Get();
            Shuffle();
            UpdateUI();
        }
    }
}
