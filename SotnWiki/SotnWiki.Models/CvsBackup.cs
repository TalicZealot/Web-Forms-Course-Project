﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SotnWiki.Models
{
    public class CvsBackup
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string Runs { get; set; }
    }
}
