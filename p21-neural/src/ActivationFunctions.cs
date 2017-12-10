using System;

namespace p21_neural {

    public static class ActivationFunctions {

        public static IActivationFunction GetRandom(Random random, FunctionParameters parameters) {
            switch (random.Next(3)) {
                case 0:
                    return new Sigmoid(parameters.Coefficients.GetNew(random));
                case 1:
                    return new LeakyRelu(parameters.Coefficients.GetNew(random),
                        parameters.Coefficients.GetNew(random));
                case 2:
                    return new Tanh(parameters.Coefficients.GetNew(random));
                default:
                    return null;
            }
        }

        public class Sigmoid : IActivationFunction, IEquatable<Sigmoid> {

            private double _a = 1;

            public Sigmoid() { }

            public Sigmoid(double a) {
                _a = a;
            }

            public double GetActivation(double x) {
                return 1 / (1 + Math.Exp(-_a * x));
            }

            public void Mutate(Random random, FunctionParameters parameters) {
                _a += parameters.Coefficients.GetChange(random);
            }

            public IActivationFunction Clone() {
                return new Sigmoid(_a);
            }

            public override string ToString() {
                return string.Format("Sigmoid({0})", _a);
            }

            public bool Equals(Sigmoid other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Math.Abs(_a - other._a) < 10e-15;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Sigmoid) obj);
            }

        }

        public class LeakyRelu : IActivationFunction, IEquatable<LeakyRelu> {

            private double _a;
            private double _b;

            public LeakyRelu() { }

            public LeakyRelu(double a, double b) {
                _a = a;
                _b = b;
            }

            public double GetActivation(double x) {
                return x < 0 ? _a * x : _b * x;
            }

            public void Mutate(Random random, FunctionParameters parameters) {
                _a += parameters.Coefficients.GetChange(random);
                _b += parameters.Coefficients.GetChange(random);
            }

            public IActivationFunction Clone() {
                return new LeakyRelu(_a, _b);
            }

            public override string ToString() {
                return string.Format("LeakyRelu({0}, {1})", _a, _b);
            }

            public bool Equals(LeakyRelu other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Math.Abs(_a - other._a) < 10e-15 && Math.Abs(_b - other._b) < 10e-15;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((LeakyRelu) obj);
            }

        }

        public class Tanh : IActivationFunction, IEquatable<Tanh> {

            private double _a;

            public Tanh() { }

            public Tanh(double a) {
                _a = a;
            }

            public double GetActivation(double x) {
                return Math.Tanh(_a * x);
            }

            public void Mutate(Random random, FunctionParameters parameters) {
                _a += parameters.Coefficients.GetChange(random);
            }

            public IActivationFunction Clone() {
                return new Tanh(_a);
            }

            public override string ToString() {
                return string.Format("Tanh({0})", _a);
            }

            public bool Equals(Tanh other) {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Math.Abs(_a - other._a) < 10e-15;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Tanh) obj);
            }

        }

    }

    public interface IActivationFunction {

        double GetActivation(double x);

        void Mutate(Random random, FunctionParameters parameters);

        IActivationFunction Clone();

    }

}