using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ClassLibrary10;

namespace Lab14
{
    class Dist
    {
        public string Name { get; set; }
        public List<Organization> orgs;

        public Dist(string n ,params Organization[] arr)
        {
            Name = n;
            orgs = new List<Organization>();
            foreach (Organization x in arr)  orgs.Add(x); 
        }
    }
}
