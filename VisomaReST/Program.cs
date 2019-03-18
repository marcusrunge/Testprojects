using System;
using System.IO;
using System.Net;

namespace VisomaReST
{
    class Program
    {
        static void Main(string[] args)
        {
            //Konsolenausgabe
            ResponseToConsole(GetHttpWebResponse(@"https://tickets.visoma.de/api2/AddinOutlookSettings/search", "mrun", "Start2017!"));
            ResponseToConsole(GetHttpWebResponse(@"https://tickets.visoma.de/api2/ticket/search/params[QueryLimit]/100", "mrun", "Start2017!"));
            ResponseToConsole(GetHttpWebResponse(@"https://tickets.visoma.de/api2/timer/search/params[QueryLimit]/1", "mrun", "Start2017!"));
            Console.ReadKey();
        }

        static void ResponseToConsole(HttpWebResponse httpWebResponse)
        {
            //Erhalten des Datenstroms aus der Rückantwort
            var stream = httpWebResponse.GetResponseStream();
            //Erzeugung eines für den Datenstrom passenden Datenlesers
            var streamReader = new StreamReader(stream);
            //Auslesen des Datenstroms in Form von Zeichenfolgen
            var responseFromServer = streamReader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            //Schließen des Lesers und der Rückantwort
            streamReader.Close();
            httpWebResponse.Close();
        }

        static HttpWebResponse GetHttpWebResponse(string domain, string username, string password)
        {
            //Erstellung einer Webanforderung auf Basis der übergebenen Domäne
            var webRequest = WebRequest.Create(domain);
            //Setzen der Anforderungsmethode aug "GET"
            webRequest.Method = "GET";
            //Setzen der Kopfdaten mit Benutzerdaten
            webRequest.Headers.Add("X_VSM_USERNAME", username);
            webRequest.Headers.Add("X_VSM_PASSWORD", password);
            //Erhalten der Rückantwort von der angesprochenen Quelle
            return webRequest.GetResponse() as HttpWebResponse;
        }
    }
}
