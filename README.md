# SyntaxAnalyzerCSharp
## Setup
1. Build and run `Program.cs` with .NET 7.0 and C# 11 or newer. Previous versions did not tested
2. Input any sequence that this grammar allows. Maybe `i(i) = (i + i)` or just `while (i(i * i(i))) do i(i, i * i, i + (i * i + i)) = (i, i(i * i), i + i)`

## Task
Create syntax analyzer for this grammar in any programming language. You can convert this grammar to an equivalent one
```console
S -> P = E | while E do S
P -> I | I (E {, E})
E -> E + T | T
T -> T * F | F
F -> P | (E)
```

## Solution
Equivalent LL(1) grammar
```console
S -> D = Y|while Y
D -> iZ
Z -> (YP|EPSILON
P -> {, Y})
Y -> DA|ZA
A ->  B|EPSILON
B -> E|T|do S
E -> + YA
T -> * YA
```
