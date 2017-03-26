#  ANN-based Traffic Light Controller

An Artificial Neural Network based Traffic Light Transition Controller for intersections.

| [AppVeyor](https://ci.appveyor.com) | [![Build status](https://ci.appveyor.com/api/projects/status/0ix0yy9mhjm4m5cc?svg=true)](https://ci.appveyor.com/project/raymelon/trafficlightneuralnetwork) |
| - | - |
| **[License](https://opensource.org/licenses)** | [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) | |

![ANN Architecture](https://github.com/raymelon/TrafficLightNeuralNetwork/blob/master/misc/demo.gif)

## Overview

Given the previous and current light pair, the controller must predict the next light pair.

The controller must receive two inputs, 

 - Previous light pair at `t - 1`

 - Current light pair at `t`.
 
 ```
                    +-----------+ 
 TL(t - 1) -------->|           |
                    |    ANN    |-------> TL(t + 1)
 TL(t)     -------->|           |
                    +-----------+ 
 ```
 
And outputs the next pair at `t + 1`.

Transitions will be implemented for a pair of traffic lights with the following sequence of pairs:

| Light Sequence Pair | | |
| - | - | - |
| **Traffic Light 1** | **Traffic Light 2** | **Time**
| RED | GREEN | t1
| RED | AMBER | t2
| RED | RED | t3
| GREEN | RED | t4
| AMBER | RED | t5
| RED | RED | t6

## Neural Network
### Design

Having two inputs and one output, the Neural Network architecture will be based on the typical logic gate Neural Network architecture:

- Two input neurons `I1`, `I2`
- Two hidden layer neurons `H1`, `H2`
- One output neuron `O1`

![ANN Architecture](https://github.com/raymelon/TrafficLightNeuralNetwork/blob/master/misc/ANNarchi.png)

The neural network will be a Feedforward neural network, having
the Logistic Sigmoid equation as its activation function.
```
             1
S(t) =  __________
               -t
          1 + e
```

Training will be done using Gradient Descent Backpropagation.

### Training

In order to simplify training, numerical values are mapped with each pair, just like how an index is associated for each row in an array.
We made sure that these numerical values assigned are in the Logistic Sigmoid's curve ranges (`0` to `1`).

| Light Sequence Pair | | |
| - | - | - |
| **Traffic Light 1** | **Traffic Light 2** | **Numerical Value**
| RED | GREEN | 0.1
| RED | AMBER | 0.2
| RED | RED | 0.3
| GREEN | RED | 0.4
| AMBER | RED | 0.5
| RED | RED | 0.6

The training data are as follows:

| Inputs at [`input.csv`](https://github.com/raymelon/TrafficLightNeuralNetwork/tree/master/data/input.csv) | | Outputs at 
   [`target.csv`](https://github.com/raymelon/TrafficLightNeuralNetwork/tree/master/data/target.csv) |
|-|-|-|
| **t - 1** | **t** | **t + 1**
0.1 | 0.2 | 0.3
0.2 | 0.3 | 0.4
0.3 | 0.4 | 0.5
0.4 | 0.5 | 0.6
0.5 | 0.6 | 0.7
0.6 | 0.7 | 0.1
0.7 | 0.8 | 0.2

Using 50 training sets (or repetitions of these in a file) in 1000 epochs is enough for the network to learn.

## Misc

### Usage
 - To re-train, run [`NeuralNetwork.exe`](https://github.com/raymelon/TrafficLightNeuralNetwork/tree/master/bin/NeuralNetwork.exe).
 
      *parameters are optional, in case nothing is provided, the defaults are `data/input.csv`, `data/target.csv`, `1000`*
  ```
  > .\TrafficLightNeuralNetwork\bin> NeuralNetwork.exe <string:input_path> <string:target_path> <int:max_epochs>
  ```

 -  [`weights.csv`](https://github.com/raymelon/TrafficLightNeuralNetwork/tree/master/data/weights.csv) has its weights updated after re-train, with the following format:
 
 | I1 to H1 | I2 to H1 | Bias to H1 | I1 to H2 | I2 to H2 | Bias to H2 |
 |-|-|-|-|-|-|
 | H1 to O1 | H2 to O2 | Bias to O2 | none | none | none |


 - Training data can be viewed at
  [`input.csv`](https://github.com/raymelon/TrafficLightNeuralNetwork/tree/master/data/input.csv) and
  [`target.csv`](https://github.com/raymelon/TrafficLightNeuralNetwork/tree/master/data/target.csv)



### Timeline
- March 19-22, 2017 (Initial Development)
- March 23, 2017 onwards (Maintenance)
- March 26, 2017 (GitHub debut)

### Authors
| | |
| - | - |
| **Developers** |
| [Raymel Francisco](http://stackoverflow.com/users/4895040/raymelfrancisco) | franciscoraymel@gmail.com |
| [Emilson Olaño](https://github.com/EmilsonME) | emilsonbolano@gmail.com |
| **Contributors** |
|  John Paul Magturo | jp.magturo@gmail.com |
|  Denzel Rañada | denzeliranada@gmail.com |
|  Kaiser Sternberg | kaiser.duque629@gmail.com |
|  Christian Bisnar | christian_bisnar1551@yahoo.com |

### License

This project is licensed under [MIT License](https://github.com/raymelon/TrafficLightNeuralNetwork/blob/master/LICENSE.md).

> MIT License

> Copyright (c) 2017 Raymel Francisco, Emilson Olaño, John Paul Magturo, Denzel Rañada, Kaiser Sternberg, and Christian Bisnar

> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
