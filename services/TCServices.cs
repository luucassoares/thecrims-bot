using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using thecrims_bot.models;
using thecrims_bot.parser;
using Console = Colorful.Console;

namespace thecrims_bot.services
{

    public class TCServices
    {

        private HttpClient client;
        private HttpClientHandler handler;
        private CookieContainer cookies;
        private Uri url;
        public bool logged;
        int robs = 0;
        public User user { get; set; }
        public Robberies rob { get; set; }
        public List<Robberies> robberies { get; set; }
        public List<Nightclubs> nightclubs { get; set; }
        public List<Drug> drugs { get; set; }
        TCParser parser = new TCParser();

        public TCServices()
        {
            url = new Uri("https://www.thecrims.com/");
            cookies = new CookieContainer();
            user = new User();
            rob = new Robberies();
            robberies = new List<Robberies>();
            nightclubs = new List<Nightclubs>();
            drugs = new List<Drug>();
            handler = new HttpClientHandler() { CookieContainer = cookies };
            client = new HttpClient(handler) { BaseAddress = url };
            client.DefaultRequestHeaders.Clear();
            this.logged = false;
        }

        public async Task LoginAsync(string user, string password)
        {

            var dict = new Dictionary<string, string>();
            dict.Add("username", user);
            dict.Add("password", password);
            var req = new HttpRequestMessage(HttpMethod.Post, "https://www.thecrims.com/login") { Content = new FormUrlEncodedContent(dict) };
            var res = await client.SendAsync(req);

            if (res.IsSuccessStatusCode)
            {

                await setXRequest();
                Console.WriteLine("Logado com sucesso!");
                this.logged = true;                
            }
            
        }

        public async Task setXRequest()
        {
            var getNewspaper = await client.GetAsync("newspaper#/newspaper");
            getNewspaper.EnsureSuccessStatusCode();
            string newspaperHtml = getNewspaper.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            string xrequest = parser.getRequest(newspaperHtml);
            this.client.DefaultRequestHeaders.Add("x-request", xrequest);
        }

        public async Task getUser()
        {

            var getTasks = await client.GetAsync("api/v1/user/tasks");
            getTasks.EnsureSuccessStatusCode();
            string jsonTasks = getTasks.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            this.user = parser.parseUser(jsonTasks);
        }

        public async Task getRobberies()
        {
            string jsonRobberies = "";

            try
            {
                var getRobberies = await client.GetAsync("api/v1/robberies");
                getRobberies.EnsureSuccessStatusCode();
                jsonRobberies = getRobberies.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch
            {
                await getRobberies();
            }
            

            this.robberies = parser.parseRobberies(jsonRobberies);
            this.rob = getBestRob();
            this.user = parser.parseUser(jsonRobberies);

        }

        public async Task getNightclubs()
        {

            var getNightclubs = await client.GetAsync("api/v1/nightclubs");
            getNightclubs.EnsureSuccessStatusCode();
            string jsonNightclubs = getNightclubs.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            this.nightclubs = parser.parseNightclubs(jsonNightclubs);


        }

        public async Task enterNightclub()
        {
            Console.WriteLine();
            await getNightclubs();

            Nightclubs nightclub = new Nightclubs();

            nightclub = this.nightclubs.Where(w => w.business_id == 1).First();

            Console.WriteLine("Entrando na " + nightclub.name, Color.Red);

            string jsonEnterNightclub = "{\"id\": \"" + nightclub.id.ToString() + "\", \"input_counters\":{}, \"action_timestamp\":" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + "}";
            var enterNightClub = await client.PostAsync("api/v1/nightclub", new StringContent(jsonEnterNightclub, Encoding.UTF8, "application/json"));
            enterNightClub.EnsureSuccessStatusCode();
            var enterNightClubGet = await client.GetAsync("api/v1/nightclub");

            string jsonDrugs = enterNightClubGet.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            await buyDrugs(jsonDrugs);

            string jsonExitNightClub = "{\"exit_key\": \"" + nightclub.id.ToString() + "\", \"e_at\":null, \"reason\":\"Manual exit\", \"input_counters\":{}, \"action_timestamp\":" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + "}";
            var exitNightClub = await client.PostAsync("api/v1/nightclub/exit", new StringContent(jsonExitNightClub, Encoding.UTF8, "application/json"));
            exitNightClub.EnsureSuccessStatusCode();
            Console.WriteLine("");

        }

        public async Task buyDrugs(string jsonDrugs)
        {
            this.drugs = parser.parseDrugs(jsonDrugs);

            Console.WriteLine("Comprando " + this.drugs[0].name, Color.Red);
            
            string jsonBuyDrugs = "{\"id\": " + this.drugs[0].id + ", \"input_counters\":{}, \"action_timestamp\":" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + "}";
            _ = await client.PostAsync("api/v1/nightclub/drug", new StringContent(jsonBuyDrugs, Encoding.UTF8, "application/json"));
            

        }

        public async Task Rob()
        {

            await getRobberies();
         

            if (this.rob.energy > this.user.stamina)
            {
                try {
                    await enterNightclub();
                } catch {
                    Console.WriteLine("Erro ao fazer fluxo do nightclub, tentar de novo.");
                    await enterNightclub();
                }
                
            }

            Console.WriteLine("Roubando " + this.rob.translated_name, Color.Yellow);

            string jsonRob = "{\"id\": " + this.rob.id + ", \"input_counters\":{}, \"action_timestamp\":" + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + "}";

            try
            {
                var rob = await client.PostAsync("api/v1/rob", new StringContent(jsonRob, Encoding.UTF8, "application/json"));
                robs += 1;
                this.user = parser.parseUser(rob.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                Console.WriteLine(user.ToString(), Color.Green);
                if (robs % 5 == 0)
                {
                    Console.WriteLine("Roubos da sessão :" + robs, Color.DarkGreen);
                }

            }
            catch
            {
                Console.Write("Erro!", Color.Red);
                Console.Write("Tentando novamente...");
                await Rob();
            }
        }

        public Robberies getBestRob()
        {
 
            return this.robberies.OrderByDescending(id => id.difficulty).First(x => x.successprobability == 100);

        }

    }
}
