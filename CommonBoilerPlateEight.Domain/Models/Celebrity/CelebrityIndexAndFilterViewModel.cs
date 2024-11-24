using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityIndexAndFilterViewModel
    {
        public CelebrityFilterViewModel Filter { get; set; } = new CelebrityFilterViewModel();
        public IPagedList<CelebrityBasicDetailViewModel> Results { get; set; }
    }
}
