using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HQF.Tutorials.ElasticSearch.UnitTest
{
    public class TestEsInMemoryConnection
    {
        private readonly ITestOutputHelper _testOutputHelper;


        public TestEsInMemoryConnection(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }


        [Fact]
        public void Test()
        {
            var esInMemoryConnection =new EsInMemoryConnection();
            var persons= esInMemoryConnection.GetResponseFromInMemory();
            Assert.True(persons.Count>0);
            foreach (var person in persons)
            {
                _testOutputHelper.WriteLine(person.FirstName);
            }

        }
    }
}
