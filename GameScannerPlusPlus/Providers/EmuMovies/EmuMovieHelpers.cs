using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using GameBrowser.Providers.EmuMovies;

namespace GameScannerplusplus.Providers.EmuMovies
{
    public class EmuMovieProvider
    {
        private string userName;
        private string passWord;
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

        //private string GetEmuMoviesPlatformFromGameSystem(string platform)
        //{
        //    string emuMoviesPlatform = null;

        //    switch (platform)
        //    {
        //        case "3DO":
        //            emuMoviesPlatform = "Panasonic_3DO";

        //            break;

        //        case "Amiga":
        //            emuMoviesPlatform = "";

        //            break;

        //        case "Arcade":
        //            emuMoviesPlatform = "MAME";

        //            break;

        //        case "Atari 2600":
        //            emuMoviesPlatform = "Atari_2600";

        //            break;

        //        case "Atari 5200":
        //            emuMoviesPlatform = "Atari_5200";

        //            break;

        //        case "Atari 7800":
        //            emuMoviesPlatform = "Atari_7800";

        //            break;

        //        case "Atari XE":
        //            emuMoviesPlatform = "Atari_8_bit";

        //            break;

        //        case "Atari Jaguar":
        //            emuMoviesPlatform = "Atari_Jaguar";

        //            break;

        //        case "Atari Jaguar CD":
        //            emuMoviesPlatform = "Atari_Jaguar";

        //            break;

        //        case "Colecovision":
        //            emuMoviesPlatform = "Coleco_Vision";

        //            break;

        //        case "Commodore 64":
        //            emuMoviesPlatform = "Commodore_64";

        //            break;

        //        case "Commodore Vic-20":
        //            emuMoviesPlatform = "";

        //            break;

        //        case "Intellivision":
        //            emuMoviesPlatform = "Mattel_Intellivision";

        //            break;

        //        case "Xbox":
        //            emuMoviesPlatform = "Microsoft_Xbox";

        //            break;

        //        case "Neo Geo":
        //            emuMoviesPlatform = "SNK_Neo_Geo_AES";

        //            break;

        //        case "N64":
        //        case "Nintendo 64":
        //            emuMoviesPlatform = "Nintendo_N64";

        //            break;

        //        case "Nintendo DS":
        //            emuMoviesPlatform = "Nintendo_DS";

        //            break;

        //        case "Nintendo":
        //        case "NES":
        //            emuMoviesPlatform = "Nintendo_NES";

        //            break;

        //        case "Game Boy":
        //        case "GameBoy":
        //        case "GB":
        //            emuMoviesPlatform = "Nintendo_Game_Boy";

        //            break;

        //        case "Game Boy Advance":
        //        case "GBA":
        //            emuMoviesPlatform = "Nintendo_Game_Boy_Advance";

        //            break;

        //        case "Game Boy Color":
        //            emuMoviesPlatform = "Nintendo_Game_Boy_Color";

        //            break;

        //        case "Gamecube":
        //            emuMoviesPlatform = "Nintendo_GameCube";

        //            break;

        //        case "Super Nintendo":
        //        case "SNES":
        //            emuMoviesPlatform = "Nintendo_SNES";

        //            break;

        //        case "Virtual Boy":
        //            emuMoviesPlatform = "";

        //            break;

        //        case "Wii":
        //        case "Nintendo Wii":
        //            emuMoviesPlatform = "";

        //            break;

        //        case "DOS":
        //            emuMoviesPlatform = "";

        //            break;

        //        case "Windows":
        //            emuMoviesPlatform = "";

        //            break;

        //        case "Sega 32X":
        //            emuMoviesPlatform = "Sega_Genesis";

        //            break;

        //        case "Sega CD":
        //            emuMoviesPlatform = "Sega_Genesis";

        //            break;

        //        case "Dreamcast":
        //        case "DC":
        //            emuMoviesPlatform = "Sega_Dreamcast";

        //            break;

        //        case "GameGear":
        //        case "Game Gear":
        //            emuMoviesPlatform = "Sega_Game_Gear";

        //            break;

        //        case "Sega Genesis":
        //            emuMoviesPlatform = "Sega_Genesis";

        //            break;

        //        case "Sega Master System":
        //            emuMoviesPlatform = "Sega_Master_System";

        //            break;

        //        case "Sega Mega Drive":
        //            emuMoviesPlatform = "Sega_Genesis";

        //            break;

        //        case "Sega Saturn":
        //            emuMoviesPlatform = "Sega_Saturn";

        //            break;

        //        case "Sony Playstation":
        //        case "Playstation":
        //        case "PSX":
        //            emuMoviesPlatform = "Sony_Playstation";

        //            break;

        //        case "PS2":
        //            emuMoviesPlatform = "Sony_Playstation_2";

        //            break;

        //        case "PSP":
        //            emuMoviesPlatform = "Sony_PSP";

        //            break;

        //        case "TurboGrafx 16":
        //            emuMoviesPlatform = "NEC_TurboGrafx_16";

        //            break;

        //        case "TurboGrafx CD":
        //            emuMoviesPlatform = "NEC_TurboGrafx_16";
        //            break;

        //        case "ZX Spectrum":
        //            emuMoviesPlatform = "";
        //            break;
        //    }

        //    return emuMoviesPlatform;

        //}

        private string sessionId;

        public async Task<List<string>> FetchImages(TitleModel titleModel)
        {
            List<string> result = await FetchImages(titleModel.Title, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.Cart);
            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Title, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.CD));
            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Title, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.Disc));

            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Code, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.Cart));
            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Code, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.CD));
            if (result.Count == 0) result.AddRange(await FetchImages(titleModel.Code, titleModel.EmuMoviesSystem, EmuMoviesMediaTypes.Disc));

            return result;
        }

        public async Task<List<string>> FetchImages(string search, string system, EmuMoviesMediaTypes mediaType)
        {
            var _httpClient = new HttpClient();

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                sessionId = GetEmuMoviesToken();
            }

            var list = new List<string>();
            try
            {
                if (sessionId == null) return list;

                var url = string.Format(EmuMoviesUrls.Search, HttpUtility.UrlEncode(search), system, mediaType, sessionId);
                Debug.WriteLine(url);
                using (var stream = await _httpClient.GetStreamAsync(url))
                {
                    var doc = new XmlDocument();
                    doc.Load(stream);

                    if (doc.HasChildNodes)
                    {
                        var nodes = doc.SelectNodes("Results/Result");

                        if (nodes != null)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                XmlAttribute urlAttribute = node?.Attributes?["URL"];

                                if (urlAttribute != null && !string.IsNullOrEmpty(urlAttribute.Value))
                                {
                                    list.Add(urlAttribute.Value);
                                }
                            }
                        }

                    }
                }
            }
            catch
            {
            }

            return list;
        }
    }
}
