using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Measurement
    {
        private int id; // Jedinstveni identifikator
        private string? region; // Region
        private string? city; // Grad
        private string? year; // Godina
        private string? consumption; // Potrošnja električne energije za mesece

        // Svojstva (Properties)
        public int Id { get => id; set => id = value; }
        public string? Region { get => region; set => region = value; }
        public string? City { get => city; set => city = value; }
        public string? Year { get => year; set => year = value; }
        public string? Consumption { get => consumption; set => consumption = value; }

        // Funkcija za ispis
        public override string? ToString() => $"Id = {Id}, Region = {Region}, City = {City}, Year = {Year}, Consumption = {Consumption}" + "\n";

        // Konstruktor bez parametara
        public Measurement()
        {
        }

        // Konstruktor sa parametrima
        public Measurement(int id, string? region, string? city, string? year, string? consumption)
        {
            Id = id;
            Region = region;
            City = city;
            Year = year;
            Consumption = consumption;
        }
    }
}