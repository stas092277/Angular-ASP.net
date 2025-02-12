﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Links
{
    public class TagPost
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public bool IsDeleted { get; set; }
    }
}
