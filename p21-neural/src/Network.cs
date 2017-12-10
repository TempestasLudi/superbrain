using System;
using System.Collections.Generic;
using System.Linq;

namespace p21_neural {

    public class Network {

        public Node[] InNodes = new Node[0];
        public Node[] OutNodes = new Node[0];

        private Dictionary<long, Node> _nodes = new Dictionary<long, Node>();
        private Dictionary<long, Edge> _edges = new Dictionary<long, Edge>();

        public Node AddNode(Node node) {
            if (node != null) {
                _nodes[node.InnovationNumber] = node;
            }

            return node;
        }

        public Edge AddEdge(Edge edge) {
            if (edge != null) {
                _edges[edge.InnovationNumber] = edge;
            }

            return edge;
        }

        public double[] Evaluate(double[] input) {
            _nodes.Values.ToList().ForEach(node => node.Flush());

            for (var i = 0; i < InNodes.Length; i++) {
                InNodes[i].Value = input[i];
            }

            return OutNodes.Select(node => node.Value).ToArray();
        }

        public Network Clone() {
            var result = new Network();

            result._nodes = _nodes.Select(entry => entry.Value.Clone())
                .ToDictionary(node => node.InnovationNumber, node => node);

            result._edges = _edges.Select(entry => entry.Value.Clone(result._nodes))
                .ToDictionary(node => node.InnovationNumber, node => node);

            result.InNodes = InNodes.Select(node => result._nodes[node.InnovationNumber]).ToArray();
            result.OutNodes = OutNodes.Select(node => result._nodes[node.InnovationNumber]).ToArray();

            return result;
        }

        public void Mutate(Random random, Func<long> innovationNumberGenerator, RandomParameters parameters) {
            _nodes.Values.ToList().ForEach(node => {
                node.Mutate(random, parameters.Node);

                if (random.NextDouble() < parameters.Node.RemoveRate) {
                    _nodes.Remove(node.InnovationNumber);
                    return;
                }

                // TODO: Add S new edges (in which S follows a binomial distribution with parameters n = _nodes.Count * _nodes.Count and p = parameters.Edge.AddRate;
            });
            _edges.Values.ToList().ForEach(edge => {
                edge.Mutate(random, parameters.Edge);

                if (random.NextDouble() < parameters.Edge.RemoveRate ||
                    !_nodes.ContainsKey(edge.InNode.InnovationNumber) ||
                    !_nodes.ContainsKey(edge.OutNode.InnovationNumber)) {
                    _edges.Remove(edge.InnovationNumber);
                    return;
                }

                if (random.NextDouble() < parameters.Node.AddRate) {
                    Func<Random, double> newWeight = parameters.Edge.Weight.GetNew;
                    Func<Random, double> newBias = parameters.Node.Bias.GetNew;

                    edge.OutNode.IncomingEdges.Remove(edge);
                    _edges.Remove(edge.InnovationNumber);
                    var newNode = AddNode(new Node(innovationNumberGenerator(), newBias(random)));
                    AddEdge(new Edge(innovationNumberGenerator(), newWeight(random), edge.InNode, newNode));
                    AddEdge(new Edge(innovationNumberGenerator(), newWeight(random), newNode, edge.OutNode));
                }
            });
        }

        public static Network CrossOver(Random random, params Network[] parents) {
            var result = new Network();

            parents.SelectMany(parent => parent._nodes.Values.Select(entry => entry.InnovationNumber)).Distinct()
                .ToList().ForEach(i => {
                    var parent = parents[random.Next(parents.Length)];
                    if (parent._nodes.ContainsKey(i)) {
                        result.AddNode(parent._nodes[i]?.Clone());
                    }
                });

            parents.SelectMany(parent => parent._nodes.Values.Select(node => node.InnovationNumber)).Distinct().ToList()
                .ForEach(i => {
                    var parent = parents[random.Next(parents.Length)];
                    if (parent._edges.ContainsKey(i)) {
                        result.AddEdge(parent._edges[i]?.Clone(result._nodes));
                    }
                });

            result.InNodes = parents.SelectMany(parent => parent.InNodes.Select(node => node.InnovationNumber))
                .Distinct().Select(i => result._nodes[i]).ToArray();
            result.OutNodes = parents.SelectMany(parent => parent.OutNodes.Select(node => node.InnovationNumber))
                .Distinct().Select(i => result._nodes[i]).ToArray();

            return result;
        }

        public override string ToString() {
            return string.Format("({0}\n{1})",
                _nodes.Values.Select(node => node.ToString()).Aggregate((a, b) => a + ",\n" + b),
                _edges.Values.Select(edge => edge.ToString()).Aggregate((a, b) => a + ",\n" + b));
        }

    }

}