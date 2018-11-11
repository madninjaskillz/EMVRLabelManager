using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using EVRLabelManager;
using GameBrowser.Providers.EmuMovies;

namespace GameScannerplusplus.Providers.EmuMovies
{
    public class EmuMovieProvider
    {
        private readonly string userName;
        private readonly string passWord;
        public EmuMovieProvider(string username, string password)
        {
            userName = username;
            passWord = password;
        }
        public string GetEmuMoviesToken()
        {

            string apiKey = @"4D8621EE919A13EB6E89B7EDCA6424FC33D6";
            var _httpClient = new HttpClient();

            var url = String.Format(EmuMoviesUrls.Login, userName, passWord, apiKey);


            using (var stream = _httpClient.GetStreamAsync(url).Result)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                if (doc.HasChildNodes)
                {
                    XmlNode resultNode = doc.SelectSingleNode("Results/Result");

                    string sessionId = resultNode?.Attributes?["Session"].Value;

                    if (sessionId != null)
                        return sessionId;
                }
            }


            return "";
        }
        
        private string sessionId;

        public async Task<List<string>> FetchImages(TitleModel titleModel)
        {
            List<string> result = await FetchImages(titleModel.Title, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.Cart);
            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Title, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.CD));
            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Title, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.Disc));

            return result;
        }

        public async Task<List<string>> FetchImages(string search, string system, EmuMoviesMediaTypes mediaType)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                sessionId = GetEmuMoviesToken();
            }

            List<string> list = new List<string>();
            try
            {
                if (sessionId == null) return list;

                var url = string.Format(EmuMoviesUrls.Search, HttpUtility.UrlEncode(search), system, mediaType, sessionId);
                Debug.WriteLine(url);

                using (var wc = new System.Net.WebClient())
                {
                    string contents = wc.DownloadString(url);

                    EmuMoviesClasses.Results result = contents.XmlDeserializeFromString<EmuMoviesClasses.Results>();

                    if (bool.Parse(result.Result.Found))
                    {
                        list.Add(result.Result.URL);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return list;
        }
    }
}
