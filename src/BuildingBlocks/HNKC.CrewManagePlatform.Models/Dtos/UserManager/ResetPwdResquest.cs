﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Models.Dtos.UserManager
{
    public class ResetPwdResquest
    {
        public string OldOnePwd { get; set; }
        public string NewOnePwd { get; set; }
        public string NewTwoPwd { get; set; }
        
    }
}
