using System;
using NUnit.Framework;

namespace p21_neural.test {

    public class TanhTest : ActivationFunctionTest {

        [Test]
        public void TestTanhNormal() {
            var f = new ActivationFunctions.Tanh(1);
            Assert.AreEqual(Math.Tanh(3), f.GetActivation(3), Delta);
        }

        [Test]
        public void TestTanhScaled() {
            var f = new ActivationFunctions.Tanh(3);
            Assert.AreEqual(Math.Tanh(3 * 5), f.GetActivation(6), Delta);
        }

        [Test]
        public void TestTanhScaledReversed() {
            var f = new ActivationFunctions.Tanh(-.5);
            Assert.AreEqual(Math.Tanh(-.5 * 7), f.GetActivation(7), Delta);
        }

        [Test]
        public void TestMutate() {
            var f = new ActivationFunctions.Tanh(1);
            var r = MockRandom(.2);
            f.Mutate(r, new FunctionParameters(0, new Parameter(0, 0, .2)));
            Assert.AreEqual(new ActivationFunctions.Tanh(.88), f);
        }

        [Test]
        public void TestClone() {
            var f = new ActivationFunctions.Tanh(2.3);
            Assert.AreEqual(f, f.Clone());
            Assert.AreNotSame(f, f.Clone());
        }

        [Test]
        public void TestEqualsTanhPositive() {
            Assert.True(new ActivationFunctions.Tanh(4.5).Equals(new ActivationFunctions.Tanh(4.5)));
        }

        [Test]
        public void TestEqualsTanhNegative() {
            Assert.False(new ActivationFunctions.Tanh(-3.2).Equals(new ActivationFunctions.Tanh(-2.1)));
        }

        [Test]
        public void TestEqualsNumber() {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(new ActivationFunctions.Tanh(3).Equals(3));
        }

        [Test]
        public void TestEqualsNull() {
            Assert.False(new ActivationFunctions.Tanh(7).Equals(null));
        }

    }

}