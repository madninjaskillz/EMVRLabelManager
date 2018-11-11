using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScannerplusplus
{
    public class TitleModel
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string System { get; set; }
        public string EmuMoviesSystem { get; set; }

        public string CartUrl { get; set; }
        public string CartImage { get; set; }
        public string CartImagePath { get; set; }
        public string LabelImagePath { get; set; }
        public string Folder { get; set; }
    }

    public class TitleMetaData
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public int ReleaseYear { get; set; }
        public string ManualUrl { get; set; }
        public string MusicUrl { get; set; }
    }


}
