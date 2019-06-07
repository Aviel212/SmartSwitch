﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BackEnd.Models.Task;

namespace BackEnd.Models.Dto
{
    public class TaskDto
    {
        public Operations Operation { get; set; }
        public string DeviceMac { get; set; }
        public TaskTypes TaskType { get; set; }
        public int RepeatEvery { get; set; } // minutes
        public DateTime StartDate { get; set; }
    }
}