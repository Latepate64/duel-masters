using DuelMastersModels.Cards;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;

namespace DuelMastersModels.Factories
{
    /// <summary>
    /// Manages the instantiation of card templates from JSON.
    /// </summary>
    public static class JsonCardFactory
    {
        /// <summary>
        /// Parses cards from a JSON-file.
        /// </summary>
        /// <param name="path">Path to the JSON-file.</param>
        /// <returns>The parsed JsonCards.</returns>
        public static Collection<JsonCard> GetJsonCards(string path)
        {
            return new Collection<JsonCard>(JsonConvert.DeserializeObject<Collection<JsonCard>>(File.ReadAllText(path)).ToList());
        }

        public static Collection<JsonCard> GetJsonCardsFromUrl(System.Uri url)
        {
            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return new Collection<JsonCard>(JsonConvert.DeserializeObject<Collection<JsonCard>>(html).ToList());
        }
    }
}
