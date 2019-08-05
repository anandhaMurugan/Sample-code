using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helseboka.Core.Terms.Model
{
    public class TermsInfo
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Required { get; set; }
        public bool Accepted { get; set; }
    }
}
