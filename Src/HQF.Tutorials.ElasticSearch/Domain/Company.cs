using System.Collections.Generic;

namespace HQF.Tutorials.ElasticSearch
{
    public class Company
    {
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
    }
}