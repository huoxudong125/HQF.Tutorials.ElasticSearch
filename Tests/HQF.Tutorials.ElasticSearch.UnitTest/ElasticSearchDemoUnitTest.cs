using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HQF.Tutorials.ElasticSearch.UnitTest
{
    public class ElasticSearchDemoUnitTest
    {
        [Fact]
        public void Test()
        {
            var els = new ElasticSearchDemo();
            var count= els.connectElasticSearch();
            Assert.Equal(1,count);
        }
    }
}
