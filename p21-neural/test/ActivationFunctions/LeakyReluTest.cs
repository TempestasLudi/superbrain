using NUnit.Framework;

namespace p21_neural.test {

    public class LeakyReluTest : ActivationFunctionTest {

        [Test]
        public void TestPositive() {
            var f = new ActivationFunctions.LeakyRelu(2, 3);
            Assert.AreEqual(15, f.GetActivation(5));
        }

        [Test]
        public void TestNegative() {
            var f = new ActivationFunctions.LeakyRelu(2, 3);
            Assert.AreEqual(-8, f.GetActivation(-4));
        }

        [Test]
        public void TestNegativeReverse() {
            var f = new ActivationFunctions.LeakyRelu(-5, 3);
            Assert.AreEqual(5, f.GetActivation(-1));
        }
        
        [Test]
        public void TestMutate() {
            var f = new ActivationFunctions.LeakyRelu(-5, 1);
            var r = MockRandom(.3);
            f.Mutate(r, new FunctionParameters(0, new Parameter(0, 0, .5)));
            Assert.AreEqual(new ActivationFunctions.LeakyRelu(-5.2, .8), f);
        }

        [Test]
        public void TestClone() {
            var f = new ActivationFunctions.LeakyRelu(2.3, 3.1);
            Assert.AreEqual(f, f.Clone());
            Assert.AreNotSame(f, f.Clone());
        }

        [Test]
        public void TestEqualsLeakyReluPositive() {
            Assert.True(new ActivationFunctions.LeakyRelu(4.5, 1.3).Equals(new ActivationFunctions.LeakyRelu(4.5, 1.3)));
        }

        [Test]
        public void TestEqualsLeakyReluNegative() {
            Assert.False(new ActivationFunctions.LeakyRelu(-2.1, 1).Equals(new ActivationFunctions.LeakyRelu(-2.1, 1.3)));
        }

        [Test]
        public void TestEqualsNumber() {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(new ActivationFunctions.LeakyRelu(3, 1).Equals(3));
        }

        [Test]
        public void TestEqualsNull() {
            Assert.False(new ActivationFunctions.LeakyRelu(7, 1).Equals(null));
        }

    }

}