using System;

namespace p21_neural {

    public struct RandomParameters {

        public EdgeParameters Edge;
        public NodeParameters Node;

        public RandomParameters(EdgeParameters edge, NodeParameters node) {
            Edge = edge;
            Node = node;
        }

    }

    public struct EdgeParameters {

        public Parameter Weight;

        public double AddRate;
        public double RemoveRate;
        public double EnableRate;
        public double DisableRate;

        public EdgeParameters(Parameter weight) : this(weight, 0, 0, 0, 0) { }

        public EdgeParameters(Parameter weight, double addRate, double removeRate, double enableRate,
            double disableRate) {
            Weight = weight;
            AddRate = addRate;
            RemoveRate = removeRate;
            EnableRate = enableRate;
            DisableRate = disableRate;
        }

    }

    public struct NodeParameters {

        public Parameter Bias;

        public double AddRate;
        public double RemoveRate;
        public double EnableRate;
        public double DisableRate;

        public FunctionParameters Function;

        public NodeParameters(Parameter bias, FunctionParameters function) : this(bias, 0, 0, 0, 0, function) { }

        public NodeParameters(Parameter bias, double addRate, double removeRate, double enableRate, double disableRate,
            FunctionParameters function) {
            Bias = bias;
            AddRate = addRate;
            RemoveRate = removeRate;
            EnableRate = enableRate;
            DisableRate = disableRate;
            Function = function;
        }

    }

    public struct FunctionParameters {

        public double SwapRate;

        public Parameter Coefficients;

        public FunctionParameters(double swapRate, Parameter coefficients) {
            SwapRate = swapRate;
            Coefficients = coefficients;
        }

    }

    public struct Parameter {

        public readonly double Min;
        public readonly double Max;
        public double Rate;

        public Parameter(double min, double max, double rate) {
            Min = min;
            Max = max;
            Rate = rate;
        }

        public double GetNew(Random random) {
            return Min + (Max - Min) * random.NextDouble();
        }

        public double GetChange(Random random) {
            return (random.NextDouble() * 2 - 1) * Rate;
        }

    }

}