using System.Text;

namespace restful_api_joaodias.Hypermedia
{
    public class HyperMediaLink
    {
        private string _href;

        public string Action { get; set; }

        public string Href
        {
            get
            {
                object _lock = new object();
                lock (_lock)
                {
                    StringBuilder sb = new StringBuilder(_href);
                    return sb.Replace("%2F", "/").ToString();
                }
            }
            set { _href = value; }
        }

        public string Rel { get; set; }

        public string Type { get; set; }
    }
}
