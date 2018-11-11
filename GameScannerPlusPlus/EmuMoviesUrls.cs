using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScannerplusplus
{
    public class EmuMoviesUrls
    {
        public static string Login = @"https://api.gamesdbase.com/login.aspx?user={0}&api={1}&product={2}";
        public static string GetSystems = @"https://api.gamesdbase.com/getsystems.aspx?sessionid={0}";
        public static string GetMedias = @"https://api.gamesdbase.com/getmedias.aspx?sessionid={0}";
        public static string Search = @"https://api.gamesdbase.com/search.aspx?search={0}&system={1}&media={2}&sessionid={3}";
    }

    public class EmuMoviesClasses
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Results
        {

            private ResultsResult resultField;

            /// <remarks/>
            public ResultsResult Result
            {
                get
                {
                    return this.resultField;
                }
                set
                {
                    this.resultField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ResultsResult
        {

            private string foundField;

            private string cachedField;

            private string uRLField;

            private string timeTakenField;

            private string cRCField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Found
            {
                get
                {
                    return this.foundField;
                }
                set
                {
                    this.foundField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Cached
            {
                get
                {
                    return this.cachedField;
                }
                set
                {
                    this.cachedField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string URL
            {
                get
                {
                    return this.uRLField;
                }
                set
                {
                    this.uRLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TimeTaken
            {
                get
                {
                    return this.timeTakenField;
                }
                set
                {
                    this.timeTakenField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string CRC
            {
                get
                {
                    return this.cRCField;
                }
                set
                {
                    this.cRCField = value;
                }
            }
        }


    }
}
