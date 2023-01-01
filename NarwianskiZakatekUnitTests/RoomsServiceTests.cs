using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class RoomsServiceTests
    {
        public readonly MockData _mockData;

        public RoomsServiceTests()
        {
            _mockData = new MockData();
        }
    }
}
