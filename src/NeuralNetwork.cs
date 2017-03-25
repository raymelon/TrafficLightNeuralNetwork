using System;
using System.IO;

namespace TrafficLightsEyEnEn
{
    public class NeuralNetwork
    {
        static void Main(string[] args)
        {
            var TrafficNN = new NeuralNetwork();

            TrafficNN.HiddenLayers = 1;

            TrafficNN.AllocateNetwork();

            TrafficNN.TrainingSize = 300;
            TrafficNN.LoadInputsFrom(@"../data/input.csv");
            TrafficNN.LoadTargetsFrom(@"../data/target.csv");
            TrafficNN.MaxEpochs = 4000;

            Console.WriteLine("Inputs loaded from 'data/input.csv'.");
            Console.WriteLine("Targets loaded from 'data/target.csv'.");

            // load saved weights
            // WARNING: comment if training for the first time (no weights file yet)
            TrafficNN.LoadNetworkFrom(@"../data/weights.csv");
            Console.WriteLine("Trained network loaded from 'data/weights.csv'.");

            Console.WriteLine("Training started...");
            TrafficNN.TrainNetwork();
            Console.WriteLine("Training complete.");

            TrafficNN.SaveNetworkTo(@"weights.csv");    // saves optimized weights
            Console.WriteLine("Trained network saved to 'data/weights.csv'.");

            // test simulation
            Console.WriteLine("Test simulation...");
            TrafficNN.FeedToNetwork(0.1, 0.2);
            TrafficNN.FeedToNetwork(0.2, 0.3);
            TrafficNN.FeedToNetwork(0.3, 0.4);
            TrafficNN.FeedToNetwork(0.4, 0.5);
            TrafficNN.FeedToNetwork(0.5, 0.6);
            TrafficNN.FeedToNetwork(0.6, 0.1);

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
        
        #region NerualNetwork subclasses
        public class Sigmoid
        {
            public static double Output(double x)
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }

            public static double Derivative(double x)
            {
                return x * (1 - x);
            }
        } // Sigmoid

        public class Neuron
        {
            public Neuron(string neuronID)
            {
                this.NeuronID = neuronID;
                this.Inputs = new double[2];
                this.Weights = new double[2];
                RandomizeWeights();
            }

            public void RandomizeWeights()
            {
                var random = new Random();

                this.Weights[0] = random.NextDouble();
                this.Weights[1] = random.NextDouble();
                this.BiasWeight = random.NextDouble();
            }

            public void AdjustWeights()
            {
                this.Weights[0] += this.Error * this.Inputs[0];
                this.Weights[1] += this.Error * this.Inputs[1];
                this.BiasWeight += this.Error;
            }

            public void WriteWeights()
            {
                Console.WriteLine("{0}: ", this.NeuronID);
                Console.WriteLine("[{0}, {1}]", this.Weights[0], this.Weights[1]);
            }
            
            public double Output
            {
                get 
                { 
                    return Sigmoid.Output(
                        this.Weights[0] * this.Inputs[0] + 
                        this.Weights[1] * this.Inputs[1] + 
                        this.BiasWeight
                    );
                }
            }

            public string NeuronID { get; set; }
            public double[] Inputs { get; set; }
            public double[] Weights { get; set; }
            public double Error { get; set; }
            public double BiasWeight { get; set; }

        } // Neuron
        #endregion NerualNetwork subclasses

        #region NerualNetwork methods
        public void LoadInputsFrom(string inputsPath)
        {
            Inputs = new double[TrainingSize, 2];

            using(var fs = File.OpenRead(inputsPath))
            using(var reader = new StreamReader(fs))
            {
                for(int i = 0; !reader.EndOfStream && i < TrainingSize; i++)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    Inputs[i, 0] = Convert.ToDouble(values[0]);
                    Inputs[i, 1] = Convert.ToDouble(values[1]);
                }
            }
        }

        public void LoadTargetsFrom(string targetsPath)
        {
            Targets = new double[TrainingSize];

            using(var fs = File.OpenRead(targetsPath))
            using(var reader = new StreamReader(fs))
            {
                for(int i = 0; !reader.EndOfStream && i < TrainingSize; i++)
                {
                    var line = reader.ReadLine();
                    Targets[i] = Convert.ToDouble(line);
                }
            }
        }

        public void AllocateNetwork()
        {
            // creating the hidden neurons
            HiddenNeurons = new Neuron[HiddenLayers, 2];

            for(int h = 0; h < HiddenLayers; h++)
            {
                HiddenNeurons[h, 0] = new Neuron( string.Format("Hidden {0}0", h) );
                HiddenNeurons[h, 1] = new Neuron( string.Format("Hidden {0}1", h) );
            }

            // creating the Output neuron
            OutputNeuron = new Neuron("Output");
        }

        public void SaveNetworkTo(string weightsPath)
        {
            using(var writer = new StreamWriter(weightsPath))
            {
                string[] lines = new string[HiddenLayers + 1];

                for(int h = 0; h < HiddenLayers; h++)
                {
                    lines[h] = string.Format("{0},{1},{2},{3},{4},{5}",

                        HiddenNeurons[h, 0].Weights[0],
                        HiddenNeurons[h, 0].Weights[1],
                        HiddenNeurons[h, 0].BiasWeight,

                        HiddenNeurons[h, 1].Weights[0],
                        HiddenNeurons[h, 1].Weights[1],
                        HiddenNeurons[h, 1].BiasWeight
                    );
                }

                lines[HiddenLayers] = string.Format("{0},{1},{2}", 
                    OutputNeuron.Weights[0],
                    OutputNeuron.Weights[1],
                    OutputNeuron.BiasWeight
                );

                foreach(var line in lines)
                {
                    writer.WriteLine(line);
                    writer.Flush();
                }
            }
        }

        public void LoadNetworkFrom(string weightsPath)
        {
            using(var fs = File.OpenRead(weightsPath))
            using(var reader = new StreamReader(fs))
            {
                string line;
                string[] values;

                for(int h = 0; h < HiddenLayers; h++)
                {
                    line = reader.ReadLine();
                    values = line.Split(',');

                    HiddenNeurons[h, 0].Weights[0] = Convert.ToDouble(values[0]);
                    HiddenNeurons[h, 0].Weights[1] = Convert.ToDouble(values[1]);
                    HiddenNeurons[h, 0].BiasWeight = Convert.ToDouble(values[2]);

                    HiddenNeurons[h, 1].Weights[0] = Convert.ToDouble(values[3]);
                    HiddenNeurons[h, 1].Weights[1] = Convert.ToDouble(values[4]);
                    HiddenNeurons[h, 1].BiasWeight = Convert.ToDouble(values[5]);
                }

                line = reader.ReadLine();
                values = line.Split(',');

                OutputNeuron.Weights[0] = Convert.ToDouble(values[0]);
                OutputNeuron.Weights[1] = Convert.ToDouble(values[1]);
                OutputNeuron.BiasWeight = Convert.ToDouble(values[2]);
            }
        }

        public double FeedToNetwork(double previousSequence, double currentSequence)
        {
            double nextSequence;

            for(int h = 0; h < HiddenLayers; h++)
            {
                HiddenNeurons[h, 0].Inputs = new double[] { previousSequence, currentSequence };
                HiddenNeurons[h, 1].Inputs = new double[] { previousSequence, currentSequence };
            }

            OutputNeuron.Inputs = new double[] { 
                HiddenNeurons[HiddenLayers - 1, 0].Output, 
                HiddenNeurons[HiddenLayers - 1, 1].Output 
            };

            double[] normalizedInputs = new double[] { 
                previousSequence * 10, 
                currentSequence * 10 
            };
            double normalizedOutput = OutputNeuron.Output * 10;

            Console.WriteLine("Inputs: {0} {1} | Output: {2}, Rounded: {3}, Expected {4} | Error: {5}", 
                normalizedInputs[0], 
                normalizedInputs[1], 
                normalizedOutput, 
                Math.Round(normalizedOutput),
                normalizedInputs[1] == 6 ? 1 : normalizedInputs[1] + 1,
                OutputNeuron.Error
            );

            nextSequence = Math.Round(normalizedOutput);

            return nextSequence;
        }

        private void BackPropagation(double target)
        {
            OutputNeuron.Error = 
                Sigmoid.Derivative(OutputNeuron.Output) * 
                (target - OutputNeuron.Output);
            
            OutputNeuron.AdjustWeights();

            for(int h = 0; h < HiddenLayers; h++)
            {
                HiddenNeurons[h, 0].Error = 
                    Sigmoid.Derivative(HiddenNeurons[h, 0].Output) * 
                    OutputNeuron.Error * 
                    OutputNeuron.Weights[0];
                
                HiddenNeurons[h, 1].Error = 
                    Sigmoid.Derivative(HiddenNeurons[h, 1].Output) * 
                    OutputNeuron.Error * 
                    OutputNeuron.Weights[1];
            
                HiddenNeurons[h, 0].AdjustWeights();
                HiddenNeurons[h, 1].AdjustWeights();
            }
        }

        public void TrainNetwork()
        {
            for(int currentEpoch = 0; currentEpoch < MaxEpochs; currentEpoch++) {
                for (int i = 0; i < Targets.Length; i++)
                {
                    // Feed-forward
                    FeedToNetwork(Inputs[i, 0], Inputs[i, 1]);

                    // Back-propagation
                    BackPropagation(Targets[i]);
                }
            }
        } // TrainNetwork()
        #endregion NerualNetwork methods

        public int TrainingSize { get; set; }
        public int MaxEpochs { get; set; }
        public int HiddenLayers { get; set; }
        public double[,] Inputs { get; set; }
        public double[] Targets { get; set; }
        public Neuron[,] HiddenNeurons { get; set; }
        public Neuron OutputNeuron { get; set; }

    } // NeuralNetwork
}