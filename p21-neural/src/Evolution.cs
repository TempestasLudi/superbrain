using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace p21_neural {

    public class Evolution : Panel {

        private const int Population = 128;

        private const double DeathRate = .75;

        private long _iteration = 0;

        private long _innovationNumber = 0;

        private readonly Random _randomGenerator;

        private RandomParameters _parameters;

        private List<Network> _networks = new List<Network>();

        private readonly Func<Network, double> _fitnessFunction;

        private FlowLayoutPanel interfaceContainer;

        private readonly Label _iterationLabel;
        private readonly Label _fitnessLabel;
        private readonly Label _outputLabel;
        private readonly Label _networkLabel;
        private readonly Mutex _networksMutex = new Mutex();

        public Evolution(Random randomGenerator, RandomParameters parameters, Func<Network, double> fitnessFunction) {
            SetStyle(ControlStyles.DoubleBuffer, true);
            _randomGenerator = randomGenerator;
            _parameters = parameters;
            _fitnessFunction = fitnessFunction;

            Controls.Add(interfaceContainer = new FlowLayoutPanel() {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            });
            interfaceContainer.FlowDirection = FlowDirection.TopDown;
            interfaceContainer.Controls.Add(_iterationLabel = new Label());
            interfaceContainer.Controls.Add(_fitnessLabel = new Label());
            interfaceContainer.Controls.Add(_outputLabel = new Label() {
                AutoSize = true,
                Font = new Font(FontFamily.GenericMonospace, 8)
            });
            interfaceContainer.Controls.Add(_networkLabel = new Label() {
                AutoSize = true,
                Font = new Font(FontFamily.GenericMonospace, 8)
            });
        }

        public void Start() {
            var timer = new Timer {Interval = 10};
            timer.Tick += (sender, args) => { Invalidate(); };
            timer.Start();

            var thread = new Thread(() => {
                while (true) {
                    _networksMutex.WaitOne();
                    Iterate();
                    _networksMutex.ReleaseMutex();
                }
            }) {IsBackground = true};
            thread.Start();
        }

        protected override void OnPaint(PaintEventArgs args) {
            var graphics = args.Graphics;
            var size = args.ClipRectangle;

            _networksMutex.WaitOne();
            
            _iterationLabel.Text = _iteration.ToString("N0");
            _fitnessLabel.Text = _fitnessFunction(_networks[0]).ToString("N8");
            _outputLabel.Text = Enumerable.Range(-5, 11)
                .Select(i => i + " => " + _networks[0].Evaluate(new double[] {i})[0].ToString("N8"))
                .Aggregate((a, b) => a + "\n" + b);
            _networkLabel.Text = _networks[0].ToString();
            
            _networksMutex.ReleaseMutex();
        }

        public void SetNetworks(List<Network> networks) {
            _networks = networks;
        }

        private void RestorePopulation() {
            var size = _networks.Count;

            if (size == 0) {
                return;
            }

            for (var i = 0; _networks.Count < Population; i = i + 1 % size) {
                var clone = _networks[i].Clone();
                clone.Mutate(_randomGenerator, () => _innovationNumber++, _parameters);
                _networks.Add(clone);
            }
        }

        public void Iterate() {
            RestorePopulation();

            _networks = _networks.OrderBy(network => -_fitnessFunction(network)).ToList();

            var killFrom = (int) Math.Max(_networks.Count * (1 - DeathRate), 1);

            while (killFrom < _networks.Count) {
                _networks.RemoveAt(killFrom);
            }

            _parameters.Edge.Weight.Rate = _parameters.Node.Bias.Rate =
                _parameters.Node.Function.Coefficients.Rate = Math.Min(-_fitnessFunction(_networks[0]) / 10, 1);
            
            _iteration++;
        }

    }

}