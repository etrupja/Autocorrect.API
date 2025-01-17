﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.ResponseModels
{
    public class TokenResponse
    {
        public Token token { get; set; }
    }

    public class Token
    {
        public string id { get; set; }
        public string _object { get; set; }
        public Card card { get; set; }
        public string client_ip { get; set; }
        public int created { get; set; }
        public string email { get; set; }
        public bool livemode { get; set; }
        public string type { get; set; }
        public bool used { get; set; }
        public long price { get; set; }
        public string client_name { get; set; }
        public string client_email { get; set; }
        public int maximum_utilization_count { get; set; }
    }

    public class Card
    {
        public string id { get; set; }
        public string _object { get; set; }
        public object address_city { get; set; }
        public object address_country { get; set; }
        public object address_line1 { get; set; }
        public object address_line1_check { get; set; }
        public object address_line2 { get; set; }
        public object address_state { get; set; }
        public object address_zip { get; set; }
        public object address_zip_check { get; set; }
        public string brand { get; set; }
        public string country { get; set; }
        public object customer { get; set; }
        public string cvc_check { get; set; }
        public object dynamic_last4 { get; set; }
        public int exp_month { get; set; }
        public int exp_year { get; set; }
        public string fingerprint { get; set; }
        public string funding { get; set; }
        public string last4 { get; set; }
        public Metadata metadata { get; set; }
        public string name { get; set; }
        public object tokenization_method { get; set; }
        public string type { get; set; }
    }

    public class Metadata
    {
    }
}
