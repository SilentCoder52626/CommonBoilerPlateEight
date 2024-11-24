﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class QuestionSettingIndexAndFilterViewModel
    {
        public QuestionSettingFilterViewModel Filter { get; set; }
        public IPagedList<QuestionSettingResponseViewModel> Results { get; set; }
    }
}
