using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace p21_neural {

    internal class Evolver : Form {

        private Evolver() {
            var parameters = new RandomParameters(
                new EdgeParameters(new Parameter(-1, 1, .005)),
                new NodeParameters(
                    new Parameter(-1, 1, .005),
                    new FunctionParameters(.005, new Parameter(-1, 1, .005))
                )
            );

            Func<Network, double> fitnessFunction = network => {
                return -Enumerable.Range(-5, 11).Select(i => (Math.Abs(network.Evaluate(new double[] {i})[0] - Math.Abs(i))))
                    .Sum();
            };

            var random = new Random();

            var evolution = new Evolution(random, parameters, fitnessFunction) {
                Size = Size = new Size(700, 600),
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Left
            };
            Controls.Add(evolution);

            var layerSizes = new int[] {1, 1, 1};

            evolution.SetNetworks(Enumerable.Range(0, 16).Select(i => CreateLayered(random, parameters, layerSizes))
                .ToList());

            evolution.Start();
        }

        private static Network CreateLayered(Random random, RandomParameters randomParameters, IList<int> layerSizes) {
            var result = new Network();

            var layers = layerSizes.Count;
            var innovationNumber = 0;

            var layerNodes = Enumerable.Range(0, layers)
                .Select(i => Enumerable.Range(0, layerSizes[i])
                    .Select(a =>
                        result.AddNode(new Node(innovationNumber++, randomParameters.Node.Bias.GetNew(random))))
                    .ToArray())
                .ToArray();

            layerNodes[layers - 1].ToList()
                .ForEach(node => node.ActivationFunction = new ActivationFunctions.LeakyRelu(1, 1));

            result.InNodes = layerNodes[0];
            result.OutNodes = layerNodes[layers - 1];

            Enumerable.Range(0, layers - 1).ToList()
                .ForEach(i => layerNodes[i + 1].ToList()
                    .ForEach(outNode => layerNodes[i].ToList()
                        .ForEach(inNode => result.AddEdge(
                            new Edge(innovationNumber++, randomParameters.Edge.Weight.GetNew(random), inNode, outNode)
                        ))));

            return result;
        }

        [STAThread]
        public static void Main(string[] args) {
            Application.Run(new Evolver());
        }

    }

}