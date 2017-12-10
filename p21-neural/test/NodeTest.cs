using NUnit.Framework;

namespace p21_neural.test {

    [TestFixture]
    public class NodeTest {

        [Test]
        public void TestValue() {
            var node = new Node(1, 7);
            Assert.AreEqual(7, node.Value);
            node.Value = 1;
            Assert.AreEqual(1, node.Value);
            node.Flush();
            Assert.AreEqual(7, node.Value);
        }

        [Test]
        public void TestFunction() {
            var node = new Node(1, 7, new ActivationFunctions.LeakyRelu(0, .5));
            Assert.AreEqual(3.5, node.Value);
        }

    }

}