﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GardenControlRepositories
{
    public class GardenControlContext : DbContext
    {
        public GardenControlContext(DbContextOptions<GardenControlContext> options) : base(options)
        { }

    }
}