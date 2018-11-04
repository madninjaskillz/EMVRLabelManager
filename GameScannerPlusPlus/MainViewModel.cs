using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GameBrowser.Providers.EmuMovies;
using GameScannerplusplus.Providers.EmuMovies;
using Newtonsoft.Json;

namespace GameScannerplusplus
{
    public class MainViewModel : BaseViewModel
    {
        
        private string dbName = "db5.txt";
        public MainViewModel()
        {
            try
            {
                string json = File.ReadAllText(dbName);
                Titles = new ObservableCollection<TitleModel>(JsonConvert.DeserializeObject<List<TitleModel>>(json));
            }
            catch
            {
            }

            try
            {
                string configJson = File.ReadAllText("config.json");
                Config config = JsonConvert.DeserializeObject<Config>(configJson);

                GameScannerPath = config.Path;
                UserName = config.UserName;
                PassWord = config.Password;
            }
            catch
            {
            }

            try
            {
                SystemConfigs = new ObservableCollection<ConsoleLabelConfig>(JsonConvert.DeserializeObject<List<ConsoleLabelConfig>>(File.ReadAllText("systems.cfg")));
                foreach (ConsoleLabelConfig consoleLabelConfig in SystemConfigs)
                {

                    FoundSystems.Add(new System
                    {
                        HasConfig = true,
                        Name = consoleLabelConfig.FolderNames.First(),
                    });
                }
            }
            catch { }
        }

        public void SaveConfig()
        {
            Config config = new Config
            {
                Path = GameScannerPath,
                UserName = UserName,
                Password = PassWord
            };

            string json = JsonConvert.SerializeObject(config);

            File.WriteAllText("config.json", json);
        }

        public class Config : BaseViewModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Path { get; set; }
        }

        private void CheckButtons()
        {
            ButtonsEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(PassWord) && !string.IsNullOrWhiteSpace(GameScannerPath);
        }

        private bool buttonsEnabled;

        public bool ButtonsEnabled
        {
            get => buttonsEnabled;
            set => Set(ref buttonsEnabled, value);
        }
        private string userName;

        public string UserName
        {
            get => userName;
            set
            {
                Set(ref userName, value);
                CheckButtons();
            }
        }

        private string passWord;

        public string PassWord
        {
            get => passWord;
            set
            {
                Set(ref passWord, value);
                CheckButtons();
            }
        }

        private string gameScannerPath;

        public string GameScannerPath
        {
            get => gameScannerPath;
            set
            {
                Set(ref gameScannerPath, value);
                CheckButtons();
            }
        }

        private ObservableCollection<TitleModel> titles = new ObservableCollection<TitleModel>();

        public ObservableCollection<TitleModel> Titles
        {
            get => titles;
            set => Set(ref titles, value);
        }

        public Dictionary<string, ConsoleLabelConfig> ConsoleConfigs = new Dictionary<string, ConsoleLabelConfig>();

        private ObservableCollection<System> foundSystems = new ObservableCollection<System>();

        public ObservableCollection<System> FoundSystems
        {
            get => foundSystems;
            set => Set(ref foundSystems, value);
        }


        public class System
        {
            public string Name { get; set; }
            public bool HasConfig { get; set; }
        }

        public ObservableCollection<ConsoleLabelConfig> SystemConfigs= new ObservableCollection<ConsoleLabelConfig>();
        public ConsoleLabelConfig GetConfig(string system)
        {
            if (SystemConfigs.Any(x => x.FolderNames.Any(y => y.ToLower() == system.ToLower())))
            {
                var config = SystemConfigs.First(x=>x.FolderNames.Any(y=>y.ToLower()==system.ToLower()));

                if (!FoundSystems.Any(x => x.Name.ToLower() == system.ToLower()))
                {
                    FoundSystems.Add(new System
                    {
                        HasConfig = true,
                        Name = system,
                    });
                }

                return config;
            }
            else
            {
                var cfg = new ConsoleLabelConfig
                {
                    FolderNames = new List<string> {system},

                };

                SystemConfigs.Add(cfg);
                if (!FoundSystems.Any(x => x.Name.ToLower() == system.ToLower()))
                {
                    FoundSystems.Add(new System
                    {
                        HasConfig = false,
                        Name = system
                    });
                }

                throw new NotSupportedException();
            }

            

            if (!ConsoleConfigs.ContainsKey(system.ToLower()))
            {
                if (File.Exists(system + ".txt"))
                {
                    string txt = File.ReadAllText(system + ".txt");
                    ConsoleLabelConfig cfg = JsonConvert.DeserializeObject<ConsoleLabelConfig>(txt);
                    ConsoleConfigs.Add(system.ToLower(), cfg);

                    FoundSystems.Add(new System
                    {
                        HasConfig = true,
                        Name = system,
                    });

                    cfg.FolderNames.Add(system);

                    SystemConfigs.Add(cfg);

                    string scjson = JsonConvert.SerializeObject(SystemConfigs);
                    File.WriteAllText("systems.cfg",scjson);
                }
                else
                {
                    if (!FoundSystems.Any(x => x.Name.ToLower() == system.ToLower()))
                    {
                        FoundSystems.Add(new System
                        {
                            HasConfig = false,
                            Name = system
                        });

                        ConsoleConfigs.Add(system.ToLower(), new ConsoleLabelConfig()
                        {
                            EmuMoviesSystem = system
                            
                        });
                    }

                    throw new NotSupportedException();
                }
            }

            return ConsoleConfigs[system.ToLower()];
        }

        private string log = "";

        public string Log
        {
            get => log;
            set => Set(ref log, value);
        }

        public void DebugLog(string txt)
        {
            Log = Log + txt + Environment.NewLine;
        }

        public async Task ScanFile()
        {
            string playlist = File.ReadAllText(GameScannerPath + "Game Scanner\\emuvr_playlist.txt");

            List<string> lines = playlist.Split('\r').Select(x => x.Replace("\n", "")).ToList();

            int line = 0;


            while (line < lines.Count)
            {
                TitleModel model = new TitleModel();

                model.Path = lines[line];
                model.System = model.Path.Split('\\')[1];

                line++;

                model.Title = lines[line];

                line++;

                model.Code = lines[line].Split('|').First();

                line++;

                try
                {
                    model.EmuMoviesSystem = GetConfig(model.System).EmuMoviesSystem;

                    if (Titles.All(x => x.Code != model.Code))
                    {
                        Titles.Add(model);
                    }
                }
                catch
                {
                }
            }

            Debug.WriteLine(Titles);

            EmuMovieProvider emuMovies = new EmuMovieProvider(UserName, PassWord);

            int mx = Titles.Count(t => string.IsNullOrWhiteSpace(t.CartUrl));
            int ct = 0;
            foreach (TitleModel titleModel in Titles.Where(t => string.IsNullOrWhiteSpace(t.CartUrl)))
            {
                ct++;

                DebugLog("Fetching: " + ct + "/" + mx);
                List<string> result;

                result = await emuMovies.FetchImages(titleModel);

                if (result.Count > 0)
                {
                    titleModel.CartUrl = result.First();
                }
            }

            string json = JsonConvert.SerializeObject(Titles.ToList());
            File.WriteAllText(dbName, json);


        }

        public async void DownloadCartImages()
        {
            int ct = 0;
            DebugLog("Carts to download: " + Titles.Where(t => !string.IsNullOrWhiteSpace(t.CartUrl)).Count());
            foreach (TitleModel titleModel in Titles.Where(t => !string.IsNullOrWhiteSpace(t.CartUrl)))
            {
                ct++;
                DebugLog("Downloading " + ct + "/" + Titles.Where(t => !string.IsNullOrWhiteSpace(t.CartUrl)).Count());
                string filename = titleModel.Path.Split('\\').Last().Split('#').First();
                string extension = filename.Split('.').Last();
                string imageExtension = titleModel.CartUrl.Split('.').Last();

                string imagePath = GameScannerPath + "Custom\\Labels\\" + titleModel.System + "\\" +
                                   filename.Substring(0, filename.Length - extension.Length) + imageExtension;

                string imagePathUnmodified = GameScannerPath + "Custom\\Carts\\" + titleModel.System + "\\" +
                                             filename.Substring(0, filename.Length - extension.Length) + imageExtension;


                DebugLog("Downloading " + titleModel.Title);
                try
                {
                    EnsureFolderExists(imagePathUnmodified);
                    if (!File.Exists(imagePathUnmodified))
                    {
                        Bitmap test = await DownloadAndAutoCrop(titleModel.CartUrl, titleModel.System);


                        test.Save(imagePathUnmodified);
                    }
                }
                catch
                {
                }

            }
        }

        public async Task<Bitmap> DownloadAndAutoCrop(string url, string system)
        {
            using (WebClient wc = new WebClient())
            {
                var stream = wc.OpenRead(url);
                Bitmap bitmap2 = new Bitmap(stream);

                return bitmap2;
            }
        }

        public async void EnsureFolderExists(string path)
        {
            var parts = path.Split('\\').ToList();
            string fullpath = "";

            parts.Remove(parts.Last());

            foreach (string part in parts)
            {
                fullpath = fullpath + part + "\\";
                try
                {
                    Directory.CreateDirectory(fullpath);
                }
                catch
                {
                }
            }
        }


        public void ConvertCarts()
        {
            int ct = 0;
            DebugLog(Titles.Where(t => !string.IsNullOrWhiteSpace(t.CartUrl)).Count() + " to convert");
            foreach (TitleModel titleModel in Titles.Where(t => !string.IsNullOrWhiteSpace(t.CartUrl)))
            {
                ct++;
                DebugLog("Convertering " + ct + "/" + Titles.Where(t => !string.IsNullOrWhiteSpace(t.CartUrl)).Count());
                string filename = titleModel.Path.Split('\\').Last().Split('#').First();
                string extension = filename.Split('.').Last();
                string imageExtension = titleModel.CartUrl.Split('.').Last();

                string imagePath = GameScannerPath + "Custom\\Labels\\" + titleModel.System + "\\" +
                                   filename.Substring(0, filename.Length - extension.Length) + imageExtension;

                string imagePathUnmodified = GameScannerPath + "Custom\\Carts\\" + titleModel.System + "\\" +
                                             filename.Substring(0, filename.Length - extension.Length) + imageExtension;

                if (File.Exists(imagePathUnmodified) && !File.Exists(imagePath))
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(imagePathUnmodified);

                        Rectangle rect = GetRectangleForSystem(titleModel.System, bmp.Width, bmp.Height);

                        var cropped = bmp.Clone(rect, bmp.PixelFormat);
                        EnsureFolderExists(imagePath);

                        var cfg = GetConfig(titleModel.System);

                        if (!string.IsNullOrWhiteSpace(cfg.Template))
                        {
                            Bitmap template = new Bitmap(cfg.Template);
                            Graphics g = Graphics.FromImage(template);
                            g.DrawImage(cropped,new PointF[]
                            {
                                new PointF(cfg.TemplateLabelSize.X,cfg.TemplateLabelSize.Y),
                                new PointF(cfg.TemplateLabelSize.X+cfg.TemplateLabelSize.Width,cfg.TemplateLabelSize.Y),
                                //new PointF(cfg.TemplateLabelSize.X+cfg.TemplateLabelSize.Width,cfg.TemplateLabelSize.Y+cfg.TemplateLabelSize.Height),
                                new PointF(cfg.TemplateLabelSize.X,cfg.TemplateLabelSize.Y+cfg.TemplateLabelSize.Height)
                            });

                           // g.Save();
                            template.Save(imagePath);
                        }
                        else
                        {
                            cropped.Save(imagePath);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
        }

        public Rectangle GetRectangleForSystem(string system, int width, int height)
        {

            ConsoleLabelConfig cfg = GetConfig(system);

            float wRatio = width / (float)cfg.ImageWidth;
            float hRatio = height / (float)cfg.ImageHeight;

            return new Rectangle((int)((float)cfg.LabelSize.X * wRatio), (int)((float)cfg.LabelSize.Y * hRatio), (int)((float)cfg.LabelSize.Width * wRatio), (int)((float)cfg.LabelSize.Height * hRatio));



            throw new NotSupportedException();

        }

        public class ConsoleLabelConfig
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public float ImageWidth { get; set; }
            public float ImageHeight { get; set; }
            public LabelSize LabelSize { get; set; }
            public LabelSize TemplateLabelSize { get; set; }

            public string Template { get; set; }

            public string EmuMoviesSystem { get; set; }
            public List<string> FolderNames { get; set; } = new List<string>();
        }

        public class LabelSize : BaseViewModel
        {
            private int x, y, width, height;

            public int X { get=>x; set=>Set(ref x,value); }
            public int Y { get => y; set => Set(ref y, value); }
            public int Width { get => width; set => Set(ref width, value); }
            public int Height { get => height; set => Set(ref height, value); }
        }

        public void ClearGames()
        {
            Titles = new ObservableCollection<TitleModel>();
        }

        public void SetSelectedSystem(object selectedItem)
        {
            System system = (System) selectedItem;

            SelectedSystem = GetConfig(system.Name);

            if (SelectedSystem.TemplateLabelSize == null)
            {
                SelectedSystem.TemplateLabelSize = new LabelSize();
            }
        }

        private ConsoleLabelConfig selectedSystem;

        public ConsoleLabelConfig SelectedSystem
        {
            get => selectedSystem;
            set => Set(ref selectedSystem, value);
        }

        public void SaveConsoleConfig()
        {
            if (SystemConfigs.Any(x => x.Id == SelectedSystem.Id))
            {
                var current = SystemConfigs.First(x => x.Id == SelectedSystem.Id);
                SystemConfigs.Remove(current);
            }

            SystemConfigs.Add(SelectedSystem);

            string scjson = JsonConvert.SerializeObject(SystemConfigs, Formatting.Indented);
            File.WriteAllText("systems.cfg", scjson);
        }
    }
}
