using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Measurement
    {
        private int id; // Jedinstveni identifikator
        private string region; // Region
        private string city; // Grad
        private string year; // Godina
        private string consumption; // Potrošnja električne energije za mesece

        // Svojstva (Properties)
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public string Region { get => region; set => region = value; }
        [DataMember]
        public string City { get => city; set => city = value; }
        [DataMember]
        public string Year { get => year; set => year = value; }
        [DataMember]
        public string Consumption { get => consumption; set => consumption = value; }

        // Funkcija za ispis
        public override string ToString() => $"Id = {Id}, Region = {Region}, City = {City}, Year = {Year}, Consumption = {Consumption}" + "\n";

        // Konstruktor bez parametara
        public Measurement()
        {
        }

        // Konstruktor sa parametrima
        public Measurement(int id, string region, string city, string year, string consumption)
        {
            Id = id;
            Region = region;
            City = city;
            Year = year;
            Consumption = consumption;
        }
    }
}