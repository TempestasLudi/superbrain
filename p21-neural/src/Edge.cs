using System;
using System.Collections.Generic;

namespace p21_neural {

    public class Edge : IEquatable<Edge> {

        public readonly long InnovationNumber;

        private double _weight;

        private bool _isEnabled = true;

        public readonly Node InNode;
        public readonly Node OutNode;

        public double Value => _isEnabled ? InNode.Value * _weight : 0;

        public Edge(long innovationNumber, double weight, Node inNode, Node outNode) {
            InnovationNumber = innovationNumber;
            _weight = weight;
            InNode = inNode;
            OutNode = outNode;
            outNode.IncomingEdges.Add(this);
        }

        public Edge Clone(Dictionary<long, Node> nodeMap) {
            if (!nodeMap.ContainsKey(InNode.InnovationNumber) || !nodeMap.ContainsKey(OutNode.InnovationNumber)) {
                return null;
            }

            return new Edge(InnovationNumber, _weight, nodeMap[InNode.InnovationNumber],
                nodeMap[OutNode.InnovationNumber]);
        }

        public void Mutate(Random random, EdgeParameters parameters) {
            _weight += parameters.Weight.GetChange(random);
            
            if (random.NextDouble() < (_isEnabled ? parameters.DisableRate : parameters.EnableRate)) {
                _isEnabled = !_isEnabled;
            }
        }

        public override string ToString() {
            return string.Format("E({0}, {1}, {2} => {3})", InnovationNumber, _weight, InNode.InnovationNumber,
                OutNode.InnovationNumber);
        }

        public bool Equals(Edge other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return InnovationNumber == other.InnovationNumber;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode() {
            return InnovationNumber.GetHashCode();
        }

    }

}