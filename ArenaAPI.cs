using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.IO;

namespace AutoGenLabel
{
    public class ArenaAPI
    {
        private string email;
        private string password;
        private string arenaSessionId;
        private static readonly HttpClient http_client = new HttpClient();
        public ArenaAPI()
        {
            GetCredentials();
        }
        private void GetCredentials()
        {
            try
            {
                var line = File.ReadAllLines("C:\\Users\\dejesust\\source\\repos\\AutoGenLabel\\cred.txt");
                if (line.Length >= 2)
                {
                    email = line[0];
                    password = line[1];
                }
                else
                    throw new Exception("Incorrect login format");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        public async Task<bool> LoginAsync()
        {
            var loginData = new
            {
                email = this.email,
                password = this.password
            };

            var loginResponse = await PostRequest("https://api.arenasolutions.com/v1/login", loginData);

            if (loginResponse != null && loginResponse["arenaSessionId"] != null)
            {
                arenaSessionId = loginResponse["arenaSessionId"].ToString();
                http_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", arenaSessionId);
                return true;
            }
            return false;
        }
        private async Task<JObject> PostRequest(string url, object data)
        {
            try
            {
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var jsonresponse = await http_client.PostAsync(url, content);

                if (jsonresponse.IsSuccessStatusCode)
                {
                    var responseString = await jsonresponse.Content.ReadAsStringAsync();
                    return JObject.Parse(responseString);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return null;
        }
        public async Task<string> GetGUID(string FG_PN)
        {
            //var url = "https://api.arenasolutions.com/v1/items?CUEX2SIHUM3L4N253I96=Encelium&category.guid=0I2LQG65IAT3M5HO1ZUR&limit=400";
            var url = $"https://api.arenasolutions.com/v1/items?number={FG_PN}";
            var response = await http_client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonresponse = await response.Content.ReadAsStringAsync();
                var parsedResponse = JObject.Parse(jsonresponse);
                var itemGUID = parsedResponse["results"]?[0]?["guid"]?.ToString();
                return itemGUID ?? "Item GUID not found";
            }
            else
            {
                MessageBox.Show("Request Failed Error: " + response.ReasonPhrase, response.StatusCode.ToString());
                return null;
            }
        }
        public async Task<string> GetLabelInfo(string Guid, Connector connector)
        {
            var url = $"https://api.arenasolutions.com/v1/items/${Guid}";
            var response = await http_client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonresponse = await response.Content.ReadAsStringAsync();
                var parsedResponse = JObject.Parse(jsonresponse);

                connector.ProcessItemInfo(parsedResponse);

                var itemNumber = parsedResponse["additionalAttributes"]?["number"]?.ToString();
                return itemNumber;
            }
            else
            {
                MessageBox.Show("Request Failed Error: " + response.ReasonPhrase, response.StatusCode.ToString());
                return null;
            }
        }
        public async Task LogoutAsync()
        {
            var url = "https://api.arenasolutions.com/v1/logout";
            var jsonresponse = await http_client.PutAsync(url, null);

            if (!jsonresponse.IsSuccessStatusCode)
                MessageBox.Show("Logout Failed", "PUT Request Failed");
        }
        ~ArenaAPI() { }
    }
}
