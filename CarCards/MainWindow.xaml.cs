using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
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
            //jatekos kartyainak frissitese
            var currentPlayerCard = _playerCards[0];
            playerMarka.Text = currentPlayerCard.Marka;
            playerTipus.Text = currentPlayerCard.Tipus;
            playerEvjarat.Text = currentPlayerCard.Evjarat.ToString();
            playerUzemanyag.Text = currentPlayerCard.Uzemanyag;
            //playerKep.Source = new BitmapImage(new Uri($"{currentPlayerCard.Kep_utvonala}"));

            vegsebesseg.Content = currentPlayerCard.Vegsebesseg.ToString();
            gyorsulas.Content = currentPlayerCard.Gyorsulas.ToString();
            suly.Content = currentPlayerCard.Suly.ToString();
            terfogat.Content = currentPlayerCard.Terfogat.ToString();
            loero.Content = currentPlayerCard.Loero.ToString();
            nyomatek.Content = currentPlayerCard.Nyomatek.ToString();

        }

        public MainWindow()
        {
            InitializeComponent();

            Get();
            Shuffle();
            UpdateUI();
        }
    }
}
