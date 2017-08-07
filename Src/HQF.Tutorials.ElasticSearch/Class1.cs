using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace HQF.Tutorials.ElasticSearch
{
    class Class1
    {

        public void Test()
        {
            Assert(
                q => q.Query() && q.Query() && q.Query() && !q.Query(),
                Query<> && Query && Query && !Query,
                c =>
                {
                    c.Bool.Must.Should().HaveCount(3);
                    c.Bool.MustNot.Should().HaveCount(1);
                });
        }

        
    }
}
