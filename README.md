#  ANN-based Traffic Light Controller
   
An Artificial Neural Network based Traffic Light Transition Controller for intersections.

## Overview

Given the previous and current light pair, the controller must predict the next light pair.

The controller must receive two inputs, 

 - Previous light pair at `t - 1`

 - Current light pair at `t`.
 
 ```
                    +-----------+ 
 TL(t - 1) -------->|           |
                    |    ANN    |------->TL(t + 1)
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
- One output neuron `O`

```
             ----
   +------> ( H1 ) ---+
( I1 )--+--> ----      |
        X              ---> ( O )
( I2 )--+--> ----      |
   +------> ( H2 ) ---+
             ----
```
The Logistic Sigmoid equation will be used as the activation function,
```
             1
S(t) =  __________
               -t
          1 + e
```

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

| t - 1 | t | t + 1
|-|-|-|
0.1 | 0.2 | 0.3
0.2 | 0.3 | 0.4
0.3 | 0.4 | 0.5
0.4 | 0.5 | 0.6
0.5 | 0.6 | 0.7
0.6 | 0.7 | 0.1
0.7 | 0.8 | 0.2

Using 50 training sets (or repetitions of these in a file) in 1000 epochs is enough for the network to learn.

## Misc
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
|  Christian Bisnar |

### License

This project is license under MIT License.
