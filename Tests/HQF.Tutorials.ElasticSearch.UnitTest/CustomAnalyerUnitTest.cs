using Xunit;

namespace HQF.Tutorials.ElasticSearch.UnitTest
{
    public class CustomAnalyerUnitTest
    {
        [Fact]
        public void Test()
        {
            //CustomAnalyer.Do();
            CustomAnalyer.Index(new UserDoc() {Id = 1000,IsArtist = true,NickName = "Candy Cat",ProductCount = 10,ShopId = 500});
        }
    }
}