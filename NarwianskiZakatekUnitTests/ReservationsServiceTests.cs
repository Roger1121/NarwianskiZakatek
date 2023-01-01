using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarwianskiZakatekUnitTests
{
    [TestClass]
    public class ReservationsServiceTests
    {
        public readonly MockData _mockData;

        public ReservationsServiceTests()
        {
            _mockData = new MockData();
        }
    }
}
