```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.3324/22H2/2022Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  

```
| Method               | N    | Mean           | Error        | StdDev        | Median         | Gen0    | Gen1    | Allocated |
|--------------------- |----- |---------------:|-------------:|--------------:|---------------:|--------:|--------:|----------:|
| **FullOuterLinqJoin**    | **10**   |     **4,251.4 ns** |     **60.51 ns** |      **50.53 ns** |     **4,256.8 ns** |  **0.5417** |  **0.0076** |   **8.93 KB** |
| ShortLinqs           | 10   |     1,299.9 ns |     10.81 ns |      10.11 ns |     1,299.0 ns |  0.1316 |       - |   2.16 KB |
| ManualForeach        | 10   |       895.6 ns |      6.59 ns |       6.16 ns |       896.1 ns |  0.0858 |       - |   1.41 KB |
| ManualForWithoutLinq | 10   |       846.8 ns |      5.85 ns |       5.19 ns |       848.2 ns |  0.0858 |       - |   1.41 KB |
| **FullOuterLinqJoin**    | **100**  |    **38,215.8 ns** |    **241.64 ns** |     **226.03 ns** |    **38,256.7 ns** |  **5.0049** |  **0.4883** |  **82.59 KB** |
| ShortLinqs           | 100  |    38,799.5 ns |    123.78 ns |     115.79 ns |    38,737.7 ns |  0.9766 |       - |  16.61 KB |
| ManualForeach        | 100  |    48,083.6 ns |    151.30 ns |     134.12 ns |    48,094.6 ns |  0.7935 |       - |  13.02 KB |
| ManualForWithoutLinq | 100  |    52,235.2 ns |    393.54 ns |     328.62 ns |    52,311.6 ns |  0.7935 |       - |  13.02 KB |
| **FullOuterLinqJoin**    | **1000** |   **419,154.2 ns** |  **7,216.56 ns** |   **9,633.90 ns** |   **414,821.4 ns** | **48.8281** | **33.6914** | **805.17 KB** |
| ShortLinqs           | 1000 | 2,969,764.9 ns | 54,461.13 ns |  50,942.97 ns | 2,978,427.7 ns |  7.8125 |       - | 160.63 KB |
| ManualForeach        | 1000 | 4,212,185.2 ns | 63,032.10 ns |  58,960.27 ns | 4,202,343.0 ns |  7.8125 |       - | 129.03 KB |
| ManualForWithoutLinq | 1000 | 4,885,492.3 ns | 96,995.83 ns | 206,706.01 ns | 4,977,554.7 ns |  7.8125 |       - | 129.03 KB |
