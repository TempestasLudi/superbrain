using System;
using Moq;
using NUnit.Framework;

namespace p21_neural.test {

    public class SigmoidTest : ActivationFunctionTest {

        [Test]
        public void TestNormal() {
            var f = new ActivationFunctions.Sigmoid(1);
            Assert.AreEqual(1 / (1 + Math.Exp(-3)), f.GetActivation(3), Delta);
        }

        [Test]
        public void TestScaled() {
            var f = new ActivationFunctions.Sigmoid(7);
            Assert.AreEqual(1 / (1 + Math.Exp(-7 * 2)), f.GetActivation(2), Delta);
        }

        [Test]
        public void TestScaledReversed() {
            var f = new ActivationFunctions.Sigmoid(-5);
            Assert.AreEqual(1 / (1 + Math.Exp(5 * 6)), f.GetActivation(6), Delta);
        }

        [Test]
        public void TestMutate() {
            var f = new ActivationFunctions.Sigmoid(1);
            var r = MockRandom(.7);
            f.Mutate(r, new FunctionParameters(0, new Parameter(0, 0, .3)));
            Assert.AreEqual(new ActivationFunctions.Sigmoid(1.12), f);
        }

        [Test]
        public void TestClone() {
            var f = new ActivationFunctions.Sigmoid(2.3);
            Assert.AreEqual(f, f.Clone());
            Assert.AreNotSame(f, f.Clone());
        }

        [Test]
        public void TestEqualsSigmoidPositive() {
            Assert.True(new ActivationFunctions.Sigmoid(4.5).Equals(new ActivationFunctions.Sigmoid(4.5)));
        }

        [Test]
        public void TestEqualsSigmoidNegative() {
            Assert.False(new ActivationFunctions.Sigmoid(-3.2).Equals(new ActivationFunctions.Sigmoid(-2.1)));
        }

        [Test]
        public void TestEqualsNumber() {
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.False(new ActivationFunctions.Sigmoid(3).Equals(3));
        }

        [Test]
        public void TestEqualsNull() {
            Assert.False(new ActivationFunctions.Sigmoid(7).Equals(null));
        }

    }

}