using Helseboka.Core.Common.Interfaces;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Terms.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helseboka.Core.Terms.Model
{
    public class TermsListModel 
    {
        private ITermsAPI DataHandler
        {
            get => ApplicationCore.Container.Resolve<ITermsAPI>();
            
        }

        public List<TermsInfo> Terms { get; set; }
        public List<ParagraphInfo> Paragraphs { get; set; }
        public List<TermsAndParagraphs> TermsAndParagraphsList { get=>CombinedDatas(); } 
       
        private List<TermsAndParagraphs> CombinedDatas()
        {
            var datas = new List<TermsAndParagraphs>();
            if (Terms != null && Paragraphs != null)
            {
                foreach (var paragraphItem in Paragraphs)
                {
                    var item = new TermsAndParagraphs
                    {
                        Id = paragraphItem.Id,
                        Text = paragraphItem.Text,
                        Required = false,
                        Accepted = false,
                        IsTerms = false,
                        ReadMore = paragraphItem.ReadMore
                    };
                    datas.Add(item);
                }
                foreach (var termItem in Terms)
                {
                    var item = new TermsAndParagraphs
                    {
                        Id = termItem.Id,
                        Text = termItem.Text,
                        Required = termItem.Required,
                        Accepted = termItem.Accepted,
                        IsTerms = true,
                        ReadMore = null
                    };
                    datas.Add(item);
                }
            }
            return datas;
        }
       
        public async Task<Response<TermsListModel>> GetAllTerms()
        {
            return await DataHandler.GetAllTerms();
        }

        public async Task<Response> UpdateAllTerms(List<int> acceptedIds)
        {
            return await DataHandler.UpdateAllTerms(acceptedIds);
        }

    }
}
