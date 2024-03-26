```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.3324/22H2/2022Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method               | N    | Mean           | Error        | StdDev       | Gen0    | Gen1    | Allocated |
|--------------------- |----- |---------------:|-------------:|-------------:|--------:|--------:|----------:|
| **FullOuterLinqJoin**    | **10**   |     **3,462.7 ns** |     **28.33 ns** |     **26.50 ns** |  **0.5455** |  **0.0076** |   **8.93 KB** |
| ShortLinqs           | 10   |     1,187.5 ns |     20.84 ns |     29.89 ns |  0.1316 |       - |   2.16 KB |
| ManualForeach        | 10   |       804.0 ns |      4.19 ns |      3.92 ns |  0.0858 |       - |   1.41 KB |
| ManualForWithoutLinq | 10   |       840.7 ns |     13.31 ns |     12.45 ns |  0.0858 |       - |   1.41 KB |
| **FullOuterLinqJoin**    | **100**  |    **43,305.4 ns** |    **580.69 ns** |    **543.17 ns** |  **5.0049** |  **0.4883** |  **82.59 KB** |
| ShortLinqs           | 100  |    39,167.9 ns |    361.67 ns |    338.31 ns |  0.9766 |       - |  16.61 KB |
| ManualForeach        | 100  |    49,680.8 ns |    229.66 ns |    214.83 ns |  0.7935 |       - |  13.02 KB |
| ManualForWithoutLinq | 100  |    49,697.5 ns |    233.93 ns |    218.82 ns |  0.7935 |       - |  13.02 KB |
| **FullOuterLinqJoin**    | **1000** |   **433,700.3 ns** |  **7,099.68 ns** |  **6,641.05 ns** | **48.8281** | **33.6914** | **805.17 KB** |
| ShortLinqs           | 1000 | 2,892,682.6 ns |  8,540.54 ns |  7,570.96 ns |  7.8125 |       - | 160.63 KB |
| ManualForeach        | 1000 | 5,038,493.6 ns | 26,035.01 ns | 23,079.36 ns |  7.8125 |       - | 129.03 KB |
| ManualForWithoutLinq | 1000 | 5,012,059.0 ns | 10,518.48 ns |  9,838.99 ns |  7.8125 |       - | 129.03 KB |
