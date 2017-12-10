using System;
using System.Collections.Generic;
using System.Linq;

namespace p21_neural {

    public class Node : IEquatable<Node> {

        public readonly long InnovationNumber;

        public readonly List<Edge> IncomingEdges = new List<Edge>();

        private double _bias;
        private bool _isCalculated;
        private double _value;

        private bool _isEnabled = true;

        public IActivationFunction ActivationFunction = new ActivationFunctions.LeakyRelu(0, 1);

        public double Value {
            get {
                if (!_isEnabled) {
                    return 0;
                }
                
                if (!_isCalculated) {
                    _value = ActivationFunction.GetActivation(IncomingEdges.Sum(edge => edge.Value) + _bias);
                    _isCalculated = true;
                }

                return _value;
            }
            set {
                _value = value;
                _isCalculated = true;
            }
        }

        public Node(long innovationNumber, double bias) {
            InnovationNumber = innovationNumber;
            _bias = bias;
        }

        public Node(long innovationNumber, double bias, IActivationFunction activationFunction) {
            InnovationNumber = innovationNumber;
            _bias = bias;
            ActivationFunction = activationFunction;
        }

        public void Flush() {
            _isCalculated = false;
        }

        public Node Clone() {
            return new Node(InnovationNumber, _bias, ActivationFunction.Clone());
        }

        public void Mutate(Random random, NodeParameters parameters) {
            _bias += (random.NextDouble() * 2 - 1) * parameters.Bias.Rate;
            
            if (random.NextDouble() < parameters.Function.SwapRate) {
                ActivationFunction = ActivationFunctions.GetRandom(random, parameters.Function);
            } else {
                ActivationFunction.Mutate(random, parameters.Function);
            }

            if (random.NextDouble() < (_isEnabled ? parameters.DisableRate : parameters.EnableRate)) {
                _isEnabled = !_isEnabled;
            }
        }

        public void AddEdge(Edge edge) {
            IncomingEdges.Add(edge);
        }

        public override string ToString() {
            return string.Format("N({0}, {1}, {2})", InnovationNumber, _bias, ActivationFunction);
        }

        public bool Equals(Node other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return InnovationNumber == other.InnovationNumber;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode() {
            return InnovationNumber.GetHashCode();
        }

    }

}