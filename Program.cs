using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // Hier zetten we de URL van de API die we gaan aanspreken
        string apiUrl = "http://localh2-env.eba-rpvpzjqz.eu-central-1.elasticbeanstalk.com/plug/48551917CE6C";

        // Vraag de gebruiker om de gebruikersnaam in te voeren
        Console.Write("Voer je gebruikersnaam in: ");
        string username = Console.ReadLine();

        // Vraag de gebruiker om het wachtwoord in te voeren (gebruik Console.ReadLine() om spaties te ondersteunen)
        Console.Write("Voer je wachtwoord in: ");
        string password = ReadPassword();

        // We maken als het ware een virtuele browser (HttpClient) om met de API te communiceren
        using (HttpClient client = new HttpClient())
        {
            // We doen ons best om een soort sleutel te maken met de gebruikersnaam en het wachtwoord
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

            // We plakken die sleutel in ons 'verzoek' zodat de API weet dat we toestemming hebben
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            // We roepen de API aan en hopen op een antwoord (response)
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            // We checken of alles goed is gegaan
            if (response.IsSuccessStatusCode)
            {
                // Als alles goed is, lezen we het antwoord van de API
                string responseData = await response.Content.ReadAsStringAsync();

                // En laten het zien in de console
                Console.WriteLine("Hier is wat de API zegt:");
                Console.WriteLine(responseData);
            }
            else
            {
                // Als er iets mis is gegaan, laten we een soort alarmbel rinkelen
                Console.WriteLine($"Er is een fout opgetreden: {response.StatusCode}");
            }
        }
    }

    // Hulpmethode om een wachtwoord in te lezen zonder het op de console weer te geven
    private static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Voeg alleen tekens toe als het geen Enter is
            if (key.Key != ConsoleKey.Enter)
            {
                password.Append(key.KeyChar);
            }

        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Nieuwe regel na het invoeren van het wachtwoord
        return password.ToString();
    }
}
