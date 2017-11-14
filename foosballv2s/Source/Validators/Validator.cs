using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace foosballv2s
{
    class Validator
    {
        private string input { get; set; }
        private string pattern { get; set; }

        public Validator()
        {
            this.pattern = @"^[0\w][A-Za-z\d]{2,19}$";
        }

        public Validator(string pattern)
        {
            this.pattern = pattern;
        }

        public bool Validate(string input)
        {
            if (Regex.IsMatch(input, pattern))
            {
                return true;
            }
            return false;
        }
    }
}