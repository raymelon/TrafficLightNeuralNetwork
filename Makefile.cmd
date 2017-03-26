
csc /out:bin\NeuralNetwork.exe /target:winexe src\NeuralNetwork.cs

csc /out:bin\TrafficLightControllerGUI.exe /target:winexe /recurse:src\*.cs /main:TrafficLightsEyEnEn.TrafficLightControllerGUI
