using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace TCPRelayCommon
{
    internal sealed class KrakenIngestResponse
    {
        public Dictionary<string, string> _links = new Dictionary<string, string>();

        public List<KrakenIngest> Ingests = new List<KrakenIngest>();
    }

    internal sealed class KrakenIngest
    {
        public string Name;
        public bool Default;
        public int _id;
        public string url_template;
        public double Availability;
    }

    public class TwitchTvIngestServerData : IComparable<TwitchTvIngestServerData>
    {
        public readonly string Name;
        public readonly string Uri;
        public readonly bool Default;

        public TwitchTvIngestServerData(string name, string uri, bool def)
        {
            this.Name = name;
            this.Uri = uri;
            this.Default = def;
        }

        public static List<TwitchTvIngestServerData> Retrieve()
        {
            MemoryStream buf = new MemoryStream();
            HttpWebRequest req = HttpWebRequest.Create("https://api.twitch.tv/kraken/ingests") as HttpWebRequest;
            req.KeepAlive = false;
            using (WebResponse response = req.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                byte[] b = new byte[4096];
                int len;
                while ((len = stream.Read(b, 0, b.Length)) > 0)
                {
                    buf.Write(b, 0, len);
                }
            }
            
            string jsonResponse = Encoding.Default.GetString(buf.ToArray());
            KrakenIngestResponse krakenResponse = JsonConvert.DeserializeObject<KrakenIngestResponse>(jsonResponse);
            
            List<TwitchTvIngestServerData> servers = new List<TwitchTvIngestServerData>();
            foreach (KrakenIngest ingest in krakenResponse.Ingests) 
            {
                servers.Add(new TwitchTvIngestServerData(ingest.Name, ingest.url_template.Replace("/{stream_key}", ""), ingest.Default));
            }
            
            return servers;
        }

        public int CompareTo(TwitchTvIngestServerData other)
        {
            return Name.CompareTo(other.Name);
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + (Name == null ? 0 : Name.GetHashCode());
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            if (!(obj is TwitchTvIngestServerData)) return false;
            TwitchTvIngestServerData other = obj as TwitchTvIngestServerData;
            if (Name == null) return other.Name == null;
            return Name.Equals(other.Name);
        }

        public override string ToString()
        {
            return Name + " -- " + Uri;
        }
    }
}
