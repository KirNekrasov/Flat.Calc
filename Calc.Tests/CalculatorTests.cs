using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Calc.Models;

namespace Calc.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void CalculatorCalculate()
        {
            var calc = new Calculator();

            Assert.AreEqual(calc.Calculate("1 + 1"), 2);
            Assert.AreEqual(calc.Calculate("1 + 1 + 1"), 3);
            Assert.AreEqual(calc.Calculate("1 / 0"), Double.PositiveInfinity);
            Assert.AreEqual(calc.Calculate("(1 + 2) * 3"), 9);
            Assert.AreEqual(calc.Calculate("1 + 2 * 3"), 7);
        }
    }
}
