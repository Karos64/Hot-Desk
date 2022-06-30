using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
namespace HotDeskTests
{
    [TestClass]
    public class HotDeskTests
    {
        [TestMethod]
        public async Task CreateLocationKrakow_ShouldReturnOK()
        {
            HttpClient client = GetNewHttpClient();
            HttpStatusCode responseStatusCode = await CreateNewLocation(client, "Kraków", 1);
            Assert.AreEqual(HttpStatusCode.Created, responseStatusCode);
        }

        [TestMethod]
        public async Task CreateDeskAtPoznan_ShouldReturnOK()
        {
            HttpClient client = GetNewHttpClient();
            HttpStatusCode locationResponse = await CreateNewLocation(client, "Poznañ", 2);
            Assert.AreEqual(HttpStatusCode.Created, locationResponse);

            HttpStatusCode deskResponse = await CreateNewDesk(client, 2, "A New Desk", true);
            Assert.AreEqual(HttpStatusCode.Created, deskResponse);
        }

        [TestMethod]
        public async Task BookDeskAtWrongLocation_ShouldReturn404()
        {
            HttpClient client = GetNewHttpClient();
            HttpStatusCode reservationResponse = await BookDesk(client, 999, 1);
            Assert.AreEqual(HttpStatusCode.NotFound, reservationResponse);
        }

        [TestMethod]
        public async Task BookDeskAtWrongDesk_ShouldReturn404()
        {
            HttpClient client = GetNewHttpClient();
            HttpStatusCode locationResponse = await CreateNewLocation(client, "Wroc³aw", 3);
            Assert.AreEqual(HttpStatusCode.Created, locationResponse);

            HttpStatusCode reservationResponse = await BookDesk(client, 3, 999);
            Assert.AreEqual(HttpStatusCode.NotFound, reservationResponse);
        }

        [TestMethod]
        public async Task BookDeskAtGdynia_ShouldReturnOK()
        {
            HttpClient client = GetNewHttpClient();
            HttpStatusCode locationResponse = await CreateNewLocation(client, "Gdynia", 4);
            Assert.AreEqual(HttpStatusCode.Created, locationResponse);

            HttpStatusCode deskResponse = await CreateNewDesk(client, 4, "A New Desk", true);
            Assert.AreEqual(HttpStatusCode.Created, deskResponse);

            HttpStatusCode reservationResponse = await BookDesk(client, 4, 1);
            Assert.AreEqual(HttpStatusCode.OK, reservationResponse);
        }

        [TestMethod]
        public async Task BookUnavailableDesk_ShouldReturnConflict()
        {
            HttpClient client = GetNewHttpClient();
            HttpStatusCode locationResponse = await CreateNewLocation(client, "Katowice", 5);
            Assert.AreEqual(HttpStatusCode.Created, locationResponse);

            HttpStatusCode deskResponse = await CreateNewDesk(client, 5, "In renovation", false);
            Assert.AreEqual(HttpStatusCode.Created, deskResponse);

            HttpStatusCode reservationResponse = await BookDesk(client, 5, 2);
            Assert.AreEqual(HttpStatusCode.Conflict, reservationResponse);
        }


        static HttpClient GetNewHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7009/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("login-token", "1234567890");
            return client;
        }

        // Create new location with given name
        static async Task<HttpStatusCode> CreateNewLocation(HttpClient client, string name, int? id = null)
        {
            var locData = new Dictionary<string, object>
            {
                { "Name", name }
            };
            if (id != null) locData["Id"] = id;
            StringContent jsonData = new StringContent(
                JsonConvert.SerializeObject(locData, Formatting.Indented), Encoding.UTF32, "application/json");

            HttpResponseMessage response = await client.PostAsync("/admin/locations", jsonData);
            return response.StatusCode;
        }

        // Create a new desk with description 'desc' at location with specific 'locid'
        static async Task<HttpStatusCode> CreateNewDesk(HttpClient client, int locid, string desc, bool available = true)
        {
            var deskData = new Dictionary<string, object>
            {
                { "description", desc },
                { "isAvailable", available }
            };
            StringContent jsonData = new StringContent(
                JsonConvert.SerializeObject(deskData, Formatting.Indented), Encoding.UTF32, "application/json");

            string url = string.Format("admin/locations/{0}/desks", locid);
            HttpResponseMessage response = await client.PostAsync(url, jsonData);
            return response.StatusCode;
        }

        // Makes a new reservation
        static async Task<HttpStatusCode> BookDesk(HttpClient client, int locid, int deskid)
        {
            var deskData = new Dictionary<string, object>
            {
                { "ReservedBy", "Unit Tests" }
            };
            StringContent jsonData = new StringContent(
                JsonConvert.SerializeObject(deskData, Formatting.Indented), Encoding.UTF32, "application/json");

            string url = string.Format("locations/{0}/desks/{1}/book", locid, deskid);
            HttpResponseMessage response = await client.PostAsync(url, jsonData);
            return response.StatusCode;
        }

        // Returns all locations
        static async Task<string> GetLocations(HttpClient client)
        {
            HttpResponseMessage resp = await client.GetAsync("/admin/locations");
            string responseBody = await resp.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}