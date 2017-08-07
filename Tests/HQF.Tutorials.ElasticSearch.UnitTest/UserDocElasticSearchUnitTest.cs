using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace HQF.Tutorials.ElasticSearch.UnitTest
{
    public class UserDocElasticSearchUnitTest
    {
        public UserDocElasticSearchUnitTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _userElasticSearch = new UserDocElasticSearch();
        }

        private readonly ITestOutputHelper _outputHelper;
        private readonly UserDocElasticSearch _userElasticSearch;

        [Fact]
        public void Test()
        {
            

            //_userElasticSearch.InitUsers();

            var userDocs = _userElasticSearch.SearchUser("M6");
            Assert.True(userDocs.Any(), "userCount 应该大于0");


            _outputHelper.WriteLine("UserCount:[{0}]", userDocs.Count);

            foreach (var userDoc in userDocs)
                _outputHelper.WriteLine("Name:[{0}],Id:[{1}],ProductCount[{2}]"
                    , userDoc.NickName, userDoc.Id,userDoc.ProductCount);

        }
    }
}