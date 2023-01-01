using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class PostsServiceTests
    {
        public readonly MockData _mockData;

        public PostsServiceTests()
        {
            _mockData = new MockData();
        }
    }
}
