using HtmlAgilityPack;
using System.Xml;
using System.Xml.XPath;
using System.Timers;
using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

class Program
{
    public static float prevValue = 0;
    private static System.Timers.Timer aTimer;
    static void Main(string[] args)
    {
        while (true)
        {

            Console.WriteLine("Bitte gib in Sekunden an, in welchem Zeitintervall du den aktuellen Wert haben willst.");

            int interval = 20; // standardmäßig alle 10 Sekunden

            string intervalInput = Console.ReadLine();
            if (!int.TryParse(intervalInput, out interval))
            {
                Console.WriteLine("Keine gültige Eingabe, es wird das Standardintervall (20 Sekunden verwendet");
            }

            Console.WriteLine("Bitte gib an, wie viele Aktien du beobachten willst.");
            string StrIterations = Console.ReadLine();
            int iterations = int.Parse(StrIterations);

            Console.WriteLine("Bitte gib {0} Kurznamen von Aktien an, die du beobachten willst, zB. TSLA für Tesla.", iterations);
            // Console.WriteLine("Gib 'DONE' ein, wenn du fertig bist.");


            string[] stocknames = new string[iterations];
            Console.WriteLine(stocknames.Length);

            string[] urls = new string[iterations];
            var web = new HtmlWeb();

            for (int i = 0; i < iterations; i++)
            {
                stocknames[i] = Console.ReadLine();
                Console.WriteLine(stocknames[i]);

                if (stocknames[i] == "DONE")
                {
                    break;
                }
                urls[i] = "https://finance.yahoo.com/quote/" + stocknames[i] + "/history";
            }

            /*
            Console.WriteLine("Gib 'EXIT' ein, um das Programm zu beenden.");

            string stockname = Console.ReadLine();
            if (stockname == "EXIT")
            {
                break;
            } */



            aTimer = new System.Timers.Timer();
            aTimer.Interval = interval * 1000;

            var document = web.Load(urls[0]);
            
            for (int j = 0; j < iterations; j++)
            {
                document = web.Load(urls[j]);
                Console.WriteLine(stocknames[j]);
                aTimer.Elapsed += (sender, args) => StockDataOnInterval(sender, args, document);
            }
             
            
            aTimer.AutoReset = true;   // Have the timer fire repeated events (true is the default)
            aTimer.Enabled = true;  // Start timer


            Console.WriteLine("Drücke eine beliebige Taste um den Prozess abzubrechen");
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            aTimer.Stop(); // Timer stoppen
            aTimer.Enabled = false; // Timer deaktivieren
            Console.WriteLine("Prozess abgebrochen.");

        }
    }

    private static void StockDataOnInterval(Object source, System.Timers.ElapsedEventArgs e, HtmlDocument document)
    {
        // string node = document.DocumentNode.SelectSingleNode("//*[@id='Col1-1-HistoricalDataTable-Proxy']/section/div[2]/table/tbody/tr[1]/td[2]/span")?.InnerText;
        float currValue;

        // if (float.TryParse(node, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out currValue))
        // {
        
        Random random = new Random();
        currValue = 100 + (float)(random.NextDouble() * 50);
        
        char stockChange = 'o';

            // prevValue = prevValue == 0 ? currValue : prevValue;

            if (currValue > prevValue)
            {
                stockChange = '+'; // increase
            }
            else if (currValue < prevValue)
            {
                stockChange = '-'; // decrease
            }

            var url = document;
            Console.WriteLine("Um {0} beträgt der Kurs für {4} {1} ({2}), davor ({3})", e.SignalTime, currValue, stockChange, prevValue, url);
           prevValue = currValue;
        // }


    }
}



