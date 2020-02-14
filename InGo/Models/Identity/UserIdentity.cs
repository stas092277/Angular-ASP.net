﻿using InGo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Identity
{
    public class UserIdentity : IdentityUser
    {
        //public User User { get; set; }

        public bool IsDeleted { get; set; }
    }
}
