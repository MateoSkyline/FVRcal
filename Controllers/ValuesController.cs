﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FVRcal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FVRcal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController(AuthenticationContext context)
        {

        }
    }
}
