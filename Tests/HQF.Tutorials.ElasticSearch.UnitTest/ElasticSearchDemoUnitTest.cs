using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HQF.Tutorials.ElasticSearch.UnitTest
{
    public class ElasticSearchDemoUnitTest
    {
        private readonly ElasticSearchDemo _elasticSearch;
        private readonly ITestOutputHelper _testOutputHelper;

        public ElasticSearchDemoUnitTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _elasticSearch=new ElasticSearchDemo();
        }

        [Fact]
        public void Test()
        {
            var els = new ElasticSearchDemo();
            var count= els.connectElasticSearch();
            Assert.Equal(1,count);
        }


        [Fact]
        public void TestIndexPerson()
        {
            _elasticSearch.InsertData(new Person() {FirstName = "Frank", Id = 2, LastName = "Huo"});
            var persons = _elasticSearch.GetData<Person>("Frank");
            var count = persons.Count;

            Assert.Equal(1, count);

            foreach (var person in persons)
            {
                _testOutputHelper.WriteLine(person.FirstName);
            }

        }

        [Fact]
        public void TestIndexPersonChinese()
        {
            _elasticSearch.InsertData(new Person() { FirstName = "考虑旭东", Id = 3, LastName = "Huo" });
            var persons = _elasticSearch.GetData<Person>("虑");
            var count = persons.Count;

            Assert.Equal(1, count);

            foreach (var person in persons)
            {
                _testOutputHelper.WriteLine(person.FirstName);
            }

        }
    }
}
