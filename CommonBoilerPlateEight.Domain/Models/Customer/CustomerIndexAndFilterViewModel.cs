using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CustomerIndexAndFilterViewModel
    {
        public CustomerFilterViewModel Filter { get; set; } = new CustomerFilterViewModel();
        public IPagedList<CustomerBasicDetailResponseViewModel> Results { get; set; }
    }
}
