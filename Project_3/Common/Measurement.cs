using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<string, string> consumption; // Potrošnja električne energije po mesecima

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
        public Dictionary<string, string> Consumption { get => consumption; set => consumption = value; }

        // Funkcija za ispis
        public override string ToString()
        {
            string consumptionStr = string.Join("; ", Consumption.Select(kv => $"{kv.Key} = {kv.Value:F2}"));

            return $"Id = {Id}, Region = {Region}, City = {City}, Year = {Year}, Consumption = {{ {consumptionStr} }}" + "\n";
        }

        // Konstruktor bez parametara
        public Measurement()
        {
        }
    }
}