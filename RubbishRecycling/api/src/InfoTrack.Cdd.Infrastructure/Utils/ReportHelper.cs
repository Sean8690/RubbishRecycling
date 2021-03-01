using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace InfoTrack.Cdd.Infrastructure.Utils
{
    public static class ReportHelper
    {
        public static readonly TupleList<string, string> PersonDetailKeywords = new TupleList<string, string>()
        {
            {"aml.search_first_name.matched" ,"First Name Matched "},
            {"aml.search_middle_name.matched" ,"Middle Name Matched "},
            {"aml.search_last_name.matched" ,"Last Name Matched "},
            {"aml.datasource.text." ,"Aml DataSource Text "},
            {"aml.datasource." ,"Aml DataSource "},
            {"address." ,"Address "},
            { "aml.search_date_of_death", "Date of Birth Searched "},
            {"aml.search_pep_level", "Pep Level "},
             {"doc.reference." ,"Data Source "},
        };

        public static readonly TupleList<string, string> SanctionsKeywords = new TupleList<string, string>()
        {
            {"aml.search_result.score" ,"Search Result Score "},
            {"aml.search_fuzziness" ,"Search Fuzziness "},
            { "SOURCE.is_current" ,"Is Current "},
            {"doc.reference." ,"Data Source "}
        };

        public static readonly TupleList<string, string> WatchListKeywords = new TupleList<string, string>()
        {
            {"doc.reference." ,"Data Source "},
            {"aml.datasource.text.summary" ,"Data Source Summary Text"},
        };

        public static readonly TupleList<string, string> PoliticalKeywords = new TupleList<string, string>()
        {
            {"aml.search_result.score" ,"Search Result Score"},
            {"aml.search_fuzziness" ,"Search Fuzziness"},
            {"SOURCE.listing_started" ,"Listing Start Date"},
            {"SOURCE.listing_ended" ,"Listing End Date"},
            {"doc.reference." ,"Data Source "},
            {"reference.doc.","Document Source " }
        };

        public static readonly TupleList<string, string> AdverseMediaKeywords = new TupleList<string, string>()
        {
            {"aml.search_result.score" ,"Search Result Score"},
            {"aml.search_fuzziness" ,"Search Fuzziness"},
            {"SOURCE_url." ,"Source Url "},
            {"doc.reference." ,"Data Source "},
        };

        public class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }

        public static string ReplaceHeadings(TupleList<string, string> keywords, string replaceText)
        {
            if (keywords != null)
            {
                foreach (var searchText in keywords)
                {
                    // Search for heading and replace with relevant meaningful name.
                    // E.g. "doc.reference."  --> "Data Source "
                    var re = new Regex($@"\b{searchText.Item1}\b");
                    if (re.IsMatch(replaceText))
                        return re.Replace(replaceText, searchText.Item2);
                }
            }
            return replaceText;
        }
    }
}
