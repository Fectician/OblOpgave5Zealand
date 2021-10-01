using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using OBL_Opgave_5_TCP_server.Models;
using System.Text.Json;

namespace OBL_Opgave_5_TCP_server
{
    class Program
    {
        //Kode af Nicolas Lauridsen, Datamatiker 3b Zealand Erhvervsakademi.
        private static Dictionary<int, FootballPlayer> PlayerList = new Dictionary<int, FootballPlayer>
        {
            {1, new FootballPlayer {Id = 1, Price = 48, Name = "Carlos", ShirtNumber = 6}},
            {2, new FootballPlayer {Id = 2, Price = 44, Name = "Steve", ShirtNumber = 8}}
        };
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 2121);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();

                Task.Run(() => { NewClient(socket); });
            }
        }

        static void NewClient(TcpClient socket)
        {
            Console.WriteLine("A client connected.");
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            /* debug linjer for at gøre det nemmere at se, om man er connected til serveren på clienten.
            writer.WriteLine("Afventer Kommando 1, enten \'hentalle\', \'hent\' eller \'gem\' (case insensitive).");
            writer.Flush();
            */
            string query = reader.ReadLine();
            /*
            writer.WriteLine("Afventer Kommando 2, ligegyldigt for hentalle, id af objekt for hent, eller et jsonstring objekt for gem.");
            writer.Flush();
            */
            string target = reader.ReadLine();
            if (query.ToLower() == "hentalle")
            {
                writer.WriteLine(Program.HentAlle());
                writer.Flush();
            }
            else if (query.ToLower() == "hent")
            {
                writer.WriteLine(Program.Hent(int.Parse(target)));
                writer.Flush();
            }
            else if (query.ToLower() == "gem")
            {
                Program.Gem(target, writer);
            }
            else
            {
                writer.WriteLine("Ikke genkendt kommando.");
                writer.Flush();
            }
            /*
            writer.WriteLine("Terminerer forbindelse.");
            writer.Flush();
            */
            socket.Close();
        }
        public static string HentAlle()
        {
            var jsonlist = JsonSerializer.Serialize(PlayerList.Values);
            return jsonlist;
        }
        public static string Hent(int id)
        {
            try
            {
                var jsonobject = JsonSerializer.Serialize(PlayerList[id]);
                return jsonobject;
            }
            catch
            {
                return "Id'et blev ikke fundet.";
            }
        }
        public static void Gem(string json, StreamWriter writer)
        {
            try
            {
                PlayerList.Add(JsonSerializer.Deserialize<FootballPlayer>(json).Id,JsonSerializer.Deserialize<FootballPlayer>(json));
                writer.WriteLine($"Objektet med id {JsonSerializer.Deserialize<FootballPlayer>(json).Id} er nu gemt.");
            }
            catch
            {
                writer.WriteLine("En fejl opstod, og intet blev gemt.");
            }
        }
    }
}
