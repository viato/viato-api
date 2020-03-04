﻿using System;

namespace Viato.Api
{
    public class AppSettings
    {
        public string PostgresConnectionString { get; set; }
        public Uri GoogleUserInfoEndpoint { get; set; } 
        public Uri TwitterUserInfoEndpoint { get; set; }
        public Uri FacebookUserInfoEndpoint { get; set; }
    }
}
