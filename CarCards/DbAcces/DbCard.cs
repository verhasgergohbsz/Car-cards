using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using CarCards.Model;

namespace CarCards.DbAcces
{
    public class DbCard
    {
        DbConnection _connection;

        public DbCard(DbConnection connection)
        {
            _connection = connection;
        }

        public List<Card> Get()
        {
            var result = new List<Card>();
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
                        result.Add(card);
                    }
                }
            }

            return result;
        }
    }
}
