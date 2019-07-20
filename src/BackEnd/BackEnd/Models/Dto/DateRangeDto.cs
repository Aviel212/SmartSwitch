using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.Dto
{
    public class DateRangeDto
    {
        public string Mac { get; set; }
        public DateTime EarlierDate;
        public DateTime LaterDate;
    }
}
