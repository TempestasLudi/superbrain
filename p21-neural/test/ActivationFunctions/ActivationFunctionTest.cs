using System;
using NUnit.Framework;
using Moq;

namespace p21_neural.test {

    [TestFixture]
    public partial class ActivationFunctionTest {

        protected const double Delta = 10e-8;

        protected static Random MockRandom(double randomDouble, int randomInt=0) {
            var mock = new Mock<Random>();
            mock.Setup(rand => rand.NextDouble()).Returns(randomDouble);
            mock.Setup(rand => rand.Next()).Returns(randomInt);
            return mock.Object;
        }

    }

}