using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHttp;
using System.Text.RegularExpressions;


namespace RandomCodeOrg.Pluto.Http.Filters {
    public class PathRequestFilter : RequestFilter {

        private readonly Regex regex;

        public PathRequestFilter(string pattern) {
            regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public override bool Filter(HttpRequestEventArgs e) {
            return regex.IsMatch(e.Request.Url.LocalPath);
        }
    }
}
