﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global5.Application.ViewModels.Requests.Users
{
    public class UsersRequest : ICreatedResponse
    {
        public string Name { get; set; }
    }
}