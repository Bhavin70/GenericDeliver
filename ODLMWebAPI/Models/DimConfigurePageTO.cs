using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimConfigurePageTO
    {
        public int IdConfiguration { get; set; }
        public string ColumnName { get; set; }
        public int IsShow { get; set; }
        public int IsDisabled { get; set; }
        public int IsMandatory { get; set; }
        public int PageId { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public String CreatedOnStr
        {
            get { return CreatedOn.ToString(StaticStuff.Constants.DefaultDateFormat); }
        }
    }
}
