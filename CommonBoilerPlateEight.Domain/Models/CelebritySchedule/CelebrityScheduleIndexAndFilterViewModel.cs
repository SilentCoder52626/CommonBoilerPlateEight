using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityScheduleIndexAndFilterViewModel
    {
        public CelebrityScheduleFilterViewModel Filter { get; set; } = new CelebrityScheduleFilterViewModel();
        public IPagedList<CelebrityScheduleViewModel> Results { get; set; }
    }
}
