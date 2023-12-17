# OrleansEventSourcingBenchmarks
Benchmarks for the various log consistency providers and storage providers supported in Orleans event sourcing

```
BenchmarkDotNet v0.13.11, Windows 11 (10.0.22631.2861/23H2/2023Update/SunValley3)
13th Gen Intel Core i7-13700K, 1 CPU, 24 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
```

## Raise Event Benchmarks
This benchmark tests the performance of appending a number of new events to a brand new grain.

**NumEvents** - The number of events being appended to the grain.

**LogConsistencyProvider** - The log consistency provider being used by the grain. One of:
- [LogStorage](https://learn.microsoft.com/en-us/dotnet/orleans/grains/event-sourcing/log-consistency-providers#log-storage) - Stores the complete sequence of events in a single record using standard storage
- [StateStorage](https://learn.microsoft.com/en-us/dotnet/orleans/grains/event-sourcing/log-consistency-providers#state-storage) - Stores only the latest grain snapshot using standard storage
- [EventStorage](https://github.com/smfields/Orleans.EventSourcing.EventStorage) - Stores all events as separate records in a configurable event store

**StorageProvider** - Grain storage provider or event storage provider used by the grain. One of:
- Memory - Stores all events in memory distributed amongst multiple storage grains
- [Redis](https://redis.io/) - Stores events in a Redis instance
- [PostgreSQL](https://www.postgresql.org/) - Stores events in a PostgreSQL instance
- [EventStore](https://www.eventstore.com/eventstoredb) - Stores events using EventStoreDB
- [Marten](https://martendb.io/events/) - Stores events in a PostgreSQL instance using Marten

**ConfirmEachEvent** - True if each event is confirmed as it's added, or false if all events are confirmed only at the end.

**Reentrant** - True if the grain is marked as reentrant, false for non-reentrant.

| NumEvents | LogConsistencyProvider | StorageProvider | ConfirmEachEvent | Reentrant | Mean            | Error         | StdDev          | Median          |
|---------- |----------------------- |---------------- |----------------- |---------- |----------------:|--------------:|----------------:|----------------:|
| **1**         | **EventStorage**           | **EventStore**      | **False**            | **False**     |      **2,144.3 μs** |      **42.54 μs** |        **66.23 μs** |      **2,143.7 μs** |
| **1**         | **EventStorage**           | **EventStore**      | **False**            | **True**      |      **2,209.8 μs** |      **39.44 μs** |        **66.98 μs** |      **2,217.3 μs** |
| **1**         | **EventStorage**           | **EventStore**      | **True**             | **False**     |      **2,158.9 μs** |      **42.81 μs** |        **81.45 μs** |      **2,152.6 μs** |
| **1**         | **EventStorage**           | **EventStore**      | **True**             | **True**      |      **2,206.5 μs** |      **43.49 μs** |        **50.08 μs** |      **2,218.4 μs** |
| **1**         | **EventStorage**           | **Marten**          | **False**            | **False**     |      **2,310.5 μs** |      **36.80 μs** |        **30.73 μs** |      **2,300.5 μs** |
| **1**         | **EventStorage**           | **Marten**          | **False**            | **True**      |      **2,301.9 μs** |      **34.15 μs** |        **30.27 μs** |      **2,303.4 μs** |
| **1**         | **EventStorage**           | **Marten**          | **True**             | **False**     |      **2,302.6 μs** |      **35.37 μs** |        **33.08 μs** |      **2,294.8 μs** |
| **1**         | **EventStorage**           | **Marten**          | **True**             | **True**      |      **2,315.3 μs** |      **25.95 μs** |        **24.28 μs** |      **2,317.9 μs** |
| **1**         | **EventStorage**           | **Memory**          | **False**            | **False**     |        **609.9 μs** |      **10.18 μs** |         **9.03 μs** |        **606.5 μs** |
| **1**         | **EventStorage**           | **Memory**          | **False**            | **True**      |        **606.5 μs** |      **10.18 μs** |         **9.52 μs** |        **609.3 μs** |
| **1**         | **EventStorage**           | **Memory**          | **True**             | **False**     |        **618.7 μs** |       **9.65 μs** |         **9.03 μs** |        **619.1 μs** |
| **1**         | **EventStorage**           | **Memory**          | **True**             | **True**      |        **608.1 μs** |       **3.67 μs** |         **3.07 μs** |        **608.7 μs** |
| **1**         | **LogStorage**             | **Memory**          | **False**            | **False**     |        **586.1 μs** |       **8.73 μs** |         **8.16 μs** |        **585.7 μs** |
| **1**         | **LogStorage**             | **Memory**          | **False**            | **True**      |        **581.2 μs** |       **7.28 μs** |         **6.81 μs** |        **581.4 μs** |
| **1**         | **LogStorage**             | **Memory**          | **True**             | **False**     |        **606.6 μs** |      **11.53 μs** |        **10.78 μs** |        **605.1 μs** |
| **1**         | **LogStorage**             | **Memory**          | **True**             | **True**      |        **603.8 μs** |       **8.16 μs** |         **7.64 μs** |        **605.4 μs** |
| **1**         | **LogStorage**             | **PostgreSQL**      | **False**            | **False**     |      **1,693.7 μs** |      **14.66 μs** |        **13.71 μs** |      **1,690.7 μs** |
| **1**         | **LogStorage**             | **PostgreSQL**      | **False**            | **True**      |      **1,711.6 μs** |      **20.39 μs** |        **18.07 μs** |      **1,713.1 μs** |
| **1**         | **LogStorage**             | **PostgreSQL**      | **True**             | **False**     |      **1,713.8 μs** |      **33.51 μs** |        **41.15 μs** |      **1,696.6 μs** |
| **1**         | **LogStorage**             | **PostgreSQL**      | **True**             | **True**      |      **1,688.9 μs** |      **33.35 μs** |        **27.85 μs** |      **1,688.4 μs** |
| **1**         | **LogStorage**             | **Redis**           | **False**            | **False**     |      **1,304.4 μs** |      **17.57 μs** |        **16.44 μs** |      **1,299.0 μs** |
| **1**         | **LogStorage**             | **Redis**           | **False**            | **True**      |      **1,318.5 μs** |      **21.98 μs** |        **19.48 μs** |      **1,325.7 μs** |
| **1**         | **LogStorage**             | **Redis**           | **True**             | **False**     |      **1,324.3 μs** |      **17.40 μs** |        **15.43 μs** |      **1,325.0 μs** |
| **1**         | **LogStorage**             | **Redis**           | **True**             | **True**      |      **1,340.2 μs** |      **22.72 μs** |        **21.26 μs** |      **1,339.1 μs** |
| **1**         | **StateStorage**           | **Memory**          | **False**            | **False**     |        **608.9 μs** |      **11.92 μs** |        **13.72 μs** |        **608.2 μs** |
| **1**         | **StateStorage**           | **Memory**          | **False**            | **True**      |        **583.9 μs** |       **9.51 μs** |         **8.89 μs** |        **583.0 μs** |
| **1**         | **StateStorage**           | **Memory**          | **True**             | **False**     |        **602.9 μs** |      **11.40 μs** |        **10.67 μs** |        **600.3 μs** |
| **1**         | **StateStorage**           | **Memory**          | **True**             | **True**      |        **605.2 μs** |      **11.33 μs** |        **10.04 μs** |        **605.4 μs** |
| **1**         | **StateStorage**           | **PostgreSQL**      | **False**            | **False**     |      **1,717.8 μs** |      **32.61 μs** |        **32.02 μs** |      **1,723.9 μs** |
| **1**         | **StateStorage**           | **PostgreSQL**      | **False**            | **True**      |      **1,719.1 μs** |      **26.46 μs** |        **28.31 μs** |      **1,713.2 μs** |
| **1**         | **StateStorage**           | **PostgreSQL**      | **True**             | **False**     |      **1,719.8 μs** |      **31.09 μs** |        **29.08 μs** |      **1,706.8 μs** |
| **1**         | **StateStorage**           | **PostgreSQL**      | **True**             | **True**      |      **1,712.5 μs** |      **22.98 μs** |        **20.37 μs** |      **1,715.1 μs** |
| **1**         | **StateStorage**           | **Redis**           | **False**            | **False**     |      **1,312.4 μs** |      **24.08 μs** |        **22.53 μs** |      **1,314.4 μs** |
| **1**         | **StateStorage**           | **Redis**           | **False**            | **True**      |      **1,296.6 μs** |      **19.04 μs** |        **16.88 μs** |      **1,301.0 μs** |
| **1**         | **StateStorage**           | **Redis**           | **True**             | **False**     |      **1,292.2 μs** |      **12.59 μs** |        **11.16 μs** |      **1,292.8 μs** |
| **1**         | **StateStorage**           | **Redis**           | **True**             | **True**      |      **1,296.2 μs** |      **20.27 μs** |        **16.93 μs** |      **1,290.5 μs** |
| **100**       | **EventStorage**           | **EventStore**      | **False**            | **False**     |      **3,898.1 μs** |      **76.35 μs** |        **90.89 μs** |      **3,902.0 μs** |
| **100**       | **EventStorage**           | **EventStore**      | **False**            | **True**      |      **3,887.7 μs** |      **57.26 μs** |        **50.76 μs** |      **3,888.9 μs** |
| **100**       | **EventStorage**           | **EventStore**      | **True**             | **False**     |     **81,913.4 μs** |   **1,612.44 μs** |     **2,462.37 μs** |     **81,894.4 μs** |
| **100**       | **EventStorage**           | **EventStore**      | **True**             | **True**      |      **4,186.7 μs** |      **83.39 μs** |        **81.90 μs** |      **4,198.7 μs** |
| **100**       | **EventStorage**           | **Marten**          | **False**            | **False**     |      **5,777.6 μs** |      **78.44 μs** |        **65.50 μs** |      **5,777.7 μs** |
| **100**       | **EventStorage**           | **Marten**          | **False**            | **True**      |      **6,022.6 μs** |      **62.76 μs** |        **58.71 μs** |      **6,014.5 μs** |
| **100**       | **EventStorage**           | **Marten**          | **True**             | **False**     |    **165,555.8 μs** |   **3,278.40 μs** |     **8,103.39 μs** |    **163,590.5 μs** |
| **100**       | **EventStorage**           | **Marten**          | **True**             | **True**      |      **6,050.1 μs** |      **96.10 μs** |        **89.89 μs** |      **6,030.8 μs** |
| **100**       | **EventStorage**           | **Memory**          | **False**            | **False**     |      **1,115.1 μs** |      **21.95 μs** |        **20.54 μs** |      **1,115.6 μs** |
| **100**       | **EventStorage**           | **Memory**          | **False**            | **True**      |      **1,129.0 μs** |      **14.29 μs** |        **13.37 μs** |      **1,129.1 μs** |
| **100**       | **EventStorage**           | **Memory**          | **True**             | **False**     |      **2,488.6 μs** |      **49.51 μs** |       **100.01 μs** |      **2,491.5 μs** |
| **100**       | **EventStorage**           | **Memory**          | **True**             | **True**      |      **1,234.6 μs** |      **23.11 μs** |        **26.61 μs** |      **1,234.0 μs** |
| **100**       | **LogStorage**             | **Memory**          | **False**            | **False**     |      **1,120.5 μs** |      **21.29 μs** |        **22.79 μs** |      **1,113.6 μs** |
| **100**       | **LogStorage**             | **Memory**          | **False**            | **True**      |      **1,119.9 μs** |      **18.50 μs** |        **17.31 μs** |      **1,115.4 μs** |
| **100**       | **LogStorage**             | **Memory**          | **True**             | **False**     |     **18,274.3 μs** |   **1,225.68 μs** |     **3,613.94 μs** |     **18,731.6 μs** |
| **100**       | **LogStorage**             | **Memory**          | **True**             | **True**      |      **1,203.3 μs** |      **22.76 μs** |        **22.36 μs** |      **1,203.2 μs** |
| **100**       | **LogStorage**             | **PostgreSQL**      | **False**            | **False**     |      **2,524.2 μs** |      **45.11 μs** |        **39.99 μs** |      **2,511.0 μs** |
| **100**       | **LogStorage**             | **PostgreSQL**      | **False**            | **True**      |      **2,490.4 μs** |      **38.38 μs** |        **34.03 μs** |      **2,490.9 μs** |
| **100**       | **LogStorage**             | **PostgreSQL**      | **True**             | **False**     |     **87,938.6 μs** |   **1,697.86 μs** |     **2,541.28 μs** |     **87,129.6 μs** |
| **100**       | **LogStorage**             | **PostgreSQL**      | **True**             | **True**      |      **2,561.0 μs** |      **46.57 μs** |        **41.29 μs** |      **2,562.5 μs** |
| **100**       | **LogStorage**             | **Redis**           | **False**            | **False**     |      **1,911.2 μs** |      **36.87 μs** |        **34.48 μs** |      **1,901.8 μs** |
| **100**       | **LogStorage**             | **Redis**           | **False**            | **True**      |      **1,905.0 μs** |      **37.82 μs** |        **35.38 μs** |      **1,899.6 μs** |
| **100**       | **LogStorage**             | **Redis**           | **True**             | **False**     |     **55,360.7 μs** |   **1,070.63 μs** |     **1,465.49 μs** |     **55,140.2 μs** |
| **100**       | **LogStorage**             | **Redis**           | **True**             | **True**      |      **2,018.3 μs** |      **39.64 μs** |        **35.14 μs** |      **2,013.4 μs** |
| **100**       | **StateStorage**           | **Memory**          | **False**            | **False**     |      **1,118.3 μs** |      **20.91 μs** |        **20.54 μs** |      **1,117.3 μs** |
| **100**       | **StateStorage**           | **Memory**          | **False**            | **True**      |      **1,116.3 μs** |      **22.16 μs** |        **18.50 μs** |      **1,124.3 μs** |
| **100**       | **StateStorage**           | **Memory**          | **True**             | **False**     |     **19,583.7 μs** |     **717.71 μs** |     **2,116.19 μs** |     **19,287.2 μs** |
| **100**       | **StateStorage**           | **Memory**          | **True**             | **True**      |      **1,206.9 μs** |      **23.16 μs** |        **28.44 μs** |      **1,204.2 μs** |
| **100**       | **StateStorage**           | **PostgreSQL**      | **False**            | **False**     |      **2,501.9 μs** |      **34.17 μs** |        **31.96 μs** |      **2,503.8 μs** |
| **100**       | **StateStorage**           | **PostgreSQL**      | **False**            | **True**      |      **2,492.4 μs** |      **39.50 μs** |        **32.99 μs** |      **2,496.3 μs** |
| **100**       | **StateStorage**           | **PostgreSQL**      | **True**             | **False**     |     **87,123.0 μs** |   **1,676.28 μs** |     **1,646.33 μs** |     **87,101.3 μs** |
| **100**       | **StateStorage**           | **PostgreSQL**      | **True**             | **True**      |      **2,575.7 μs** |      **39.71 μs** |        **35.20 μs** |      **2,579.6 μs** |
| **100**       | **StateStorage**           | **Redis**           | **False**            | **False**     |      **1,910.8 μs** |      **36.71 μs** |        **34.34 μs** |      **1,909.7 μs** |
| **100**       | **StateStorage**           | **Redis**           | **False**            | **True**      |      **1,904.3 μs** |      **30.83 μs** |        **27.33 μs** |      **1,910.3 μs** |
| **100**       | **StateStorage**           | **Redis**           | **True**             | **False**     |     **54,283.4 μs** |   **1,001.67 μs** |     **1,266.79 μs** |     **54,394.3 μs** |
| **100**       | **StateStorage**           | **Redis**           | **True**             | **True**      |      **2,019.1 μs** |      **39.77 μs** |        **37.20 μs** |      **2,013.8 μs** |
| **1000**      | **EventStorage**           | **EventStore**      | **False**            | **False**     |     **43,423.8 μs** |     **862.93 μs** |     **2,332.99 μs** |     **43,086.6 μs** |
| **1000**      | **EventStorage**           | **EventStore**      | **False**            | **True**      |     **44,758.6 μs** |     **891.39 μs** |     **2,316.85 μs** |     **44,603.2 μs** |
| **1000**      | **EventStorage**           | **EventStore**      | **True**             | **False**     |    **805,545.8 μs** |  **13,341.79 μs** |    **18,262.39 μs** |    **802,748.4 μs** |
| **1000**      | **EventStorage**           | **EventStore**      | **True**             | **True**      |     **39,137.7 μs** |     **776.37 μs** |     **1,009.50 μs** |     **39,009.3 μs** |
| **1000**      | **EventStorage**           | **Marten**          | **False**            | **False**     |     **35,104.9 μs** |     **454.15 μs** |       **402.59 μs** |     **35,087.2 μs** |
| **1000**      | **EventStorage**           | **Marten**          | **False**            | **True**      |     **37,672.0 μs** |     **536.69 μs** |       **502.02 μs** |     **37,536.4 μs** |
| **1000**      | **EventStorage**           | **Marten**          | **True**             | **False**     |  **1,555,584.1 μs** |  **12,117.26 μs** |    **10,118.46 μs** |  **1,552,623.1 μs** |
| **1000**      | **EventStorage**           | **Marten**          | **True**             | **True**      |     **39,296.3 μs** |     **342.72 μs** |       **286.19 μs** |     **39,406.6 μs** |
| **1000**      | **EventStorage**           | **Memory**          | **False**            | **False**     |      **5,849.0 μs** |     **116.65 μs** |       **294.79 μs** |      **5,802.9 μs** |
| **1000**      | **EventStorage**           | **Memory**          | **False**            | **True**      |      **5,741.4 μs** |     **114.24 μs** |       **296.92 μs** |      **5,766.0 μs** |
| **1000**      | **EventStorage**           | **Memory**          | **True**             | **False**     |     **35,577.2 μs** |   **1,553.03 μs** |     **4,480.86 μs** |     **35,893.0 μs** |
| **1000**      | **EventStorage**           | **Memory**          | **True**             | **True**      |      **7,375.2 μs** |     **146.70 μs** |       **289.57 μs** |      **7,378.8 μs** |
| **1000**      | **LogStorage**             | **Memory**          | **False**            | **False**     |      **6,718.3 μs** |     **133.90 μs** |       **392.70 μs** |      **6,691.9 μs** |
| **1000**      | **LogStorage**             | **Memory**          | **False**            | **True**      |      **6,706.4 μs** |     **176.67 μs** |       **518.14 μs** |      **6,726.1 μs** |
| **1000**      | **LogStorage**             | **Memory**          | **True**             | **False**     |  **1,885,640.6 μs** | **378,448.09 μs** | **1,115,862.73 μs** |  **2,236,936.6 μs** |
| **1000**      | **LogStorage**             | **Memory**          | **True**             | **True**      |      **8,149.6 μs** |     **167.99 μs** |       **484.68 μs** |      **8,164.5 μs** |
| **1000**      | **LogStorage**             | **PostgreSQL**      | **False**            | **False**     |      **9,507.4 μs** |     **188.24 μs** |       **303.98 μs** |      **9,499.9 μs** |
| **1000**      | **LogStorage**             | **PostgreSQL**      | **False**            | **True**      |      **9,633.8 μs** |     **175.15 μs** |       **287.77 μs** |      **9,563.0 μs** |
| **1000**      | **LogStorage**             | **PostgreSQL**      | **True**             | **False**     |  **3,591,248.0 μs** | **167,899.74 μs** |   **495,056.18 μs** |  **3,590,232.0 μs** |
| **1000**      | **LogStorage**             | **PostgreSQL**      | **True**             | **True**      |     **10,968.1 μs** |     **215.99 μs** |       **295.64 μs** |     **10,967.1 μs** |
| **1000**      | **LogStorage**             | **Redis**           | **False**            | **False**     |      **7,480.3 μs** |     **143.09 μs** |       **175.73 μs** |      **7,514.5 μs** |
| **1000**      | **LogStorage**             | **Redis**           | **False**            | **True**      |      **7,340.8 μs** |     **133.96 μs** |       **118.75 μs** |      **7,344.5 μs** |
| **1000**      | **LogStorage**             | **Redis**           | **True**             | **False**     |  **2,344,446.7 μs** | **100,203.47 μs** |   **295,452.21 μs** |  **2,294,063.2 μs** |
| **1000**      | **LogStorage**             | **Redis**           | **True**             | **True**      |      **8,761.5 μs** |     **172.61 μs** |       **236.27 μs** |      **8,781.8 μs** |
| **1000**      | **StateStorage**           | **Memory**          | **False**            | **False**     |      **6,820.0 μs** |     **146.97 μs** |       **431.03 μs** |      **6,822.5 μs** |
| **1000**      | **StateStorage**           | **Memory**          | **False**            | **True**      |      **6,705.4 μs** |     **187.25 μs** |       **552.10 μs** |      **6,724.6 μs** |
| **1000**      | **StateStorage**           | **Memory**          | **True**             | **False**     |  **1,778,929.8 μs** | **384,574.46 μs** | **1,133,926.48 μs** |  **2,189,982.7 μs** |
| **1000**      | **StateStorage**           | **Memory**          | **True**             | **True**      |      **8,051.1 μs** |     **160.68 μs** |       **397.16 μs** |      **8,081.5 μs** |
| **1000**      | **StateStorage**           | **PostgreSQL**      | **False**            | **False**     |      **9,575.7 μs** |     **187.02 μs** |       **256.00 μs** |      **9,617.6 μs** |
| **1000**      | **StateStorage**           | **PostgreSQL**      | **False**            | **True**      |      **9,478.6 μs** |     **185.97 μs** |       **310.71 μs** |      **9,502.6 μs** |
| **1000**      | **StateStorage**           | **PostgreSQL**      | **True**             | **False**     |  **3,637,618.8 μs** | **180,851.35 μs** |   **533,244.30 μs** |  **3,723,809.3 μs** |
| **1000**      | **StateStorage**           | **PostgreSQL**      | **True**             | **True**      |     **10,971.6 μs** |     **217.35 μs** |       **266.93 μs** |     **10,962.8 μs** |
| **1000**      | **StateStorage**           | **Redis**           | **False**            | **False**     |      **7,423.9 μs** |     **132.09 μs** |       **123.55 μs** |      **7,378.0 μs** |
| **1000**      | **StateStorage**           | **Redis**           | **False**            | **True**      |      **7,350.9 μs** |     **141.84 μs** |       **139.30 μs** |      **7,345.6 μs** |
| **1000**      | **StateStorage**           | **Redis**           | **True**             | **False**     |  **2,375,312.6 μs** | **118,779.81 μs** |   **350,224.94 μs** |  **2,354,002.4 μs** |
| **1000**      | **StateStorage**           | **Redis**           | **True**             | **True**      |      **8,743.2 μs** |     **172.76 μs** |       **230.63 μs** |      **8,704.2 μs** |


## Load Grain Benchmarks
This benchmark tests the performance of loading an existing grain that has a number of events already stored.

**NumEvents** - The number of events already appended to the grain.

**LogConsistencyProvider** - The log consistency provider being used by the grain. One of:
- [LogStorage](https://learn.microsoft.com/en-us/dotnet/orleans/grains/event-sourcing/log-consistency-providers#log-storage) - Stores the complete sequence of events in a single record using standard storage
- [StateStorage](https://learn.microsoft.com/en-us/dotnet/orleans/grains/event-sourcing/log-consistency-providers#state-storage) - Stores only the latest grain snapshot using standard storage
- [EventStorage](https://github.com/smfields/Orleans.EventSourcing.EventStorage) - Stores all events as separate records in a configurable event store

**StorageProvider** - Grain storage provider or event storage provider used by the grain. One of:
- Memory - Stores all events in memory distributed amongst multiple storage grains
- [Redis](https://redis.io/) - Stores events in a Redis instance
- [PostgreSQL](https://www.postgresql.org/) - Stores events in a PostgreSQL instance
- [EventStore](https://www.eventstore.com/eventstoredb) - Stores events using EventStoreDB
- [Marten](https://martendb.io/events/) - Stores events in a PostgreSQL instance using Marten

| NumEvents | LogConsistencyProvider | StorageProvider | Mean        | Error      | StdDev     | Median      |
|---------- |----------------------- |---------------- |------------:|-----------:|-----------:|------------:|
| **1**         | **EventStorage**           | **EventStore**      |   **660.20 μs** |   **6.997 μs** |   **6.545 μs** |   **660.50 μs** |
| **1**         | **EventStorage**           | **Marten**          |   **540.99 μs** |   **5.471 μs** |   **4.568 μs** |   **540.57 μs** |
| **1**         | **EventStorage**           | **Memory**          |    **50.37 μs** |   **1.007 μs** |   **1.508 μs** |    **50.58 μs** |
| **1**         | **LogStorage**             | **Memory**          |    **65.28 μs** |   **1.155 μs** |   **1.080 μs** |    **65.50 μs** |
| **1**         | **LogStorage**             | **PostgreSQL**      |   **548.18 μs** |  **10.235 μs** |   **9.574 μs** |   **546.80 μs** |
| **1**         | **LogStorage**             | **Redis**           |   **406.06 μs** |   **7.945 μs** |   **9.457 μs** |   **403.64 μs** |
| **1**         | **StateStorage**           | **Memory**          |    **65.07 μs** |   **1.286 μs** |   **2.002 μs** |    **65.22 μs** |
| **1**         | **StateStorage**           | **PostgreSQL**      |   **549.06 μs** |  **10.471 μs** |  **10.753 μs** |   **544.29 μs** |
| **1**         | **StateStorage**           | **Redis**           |   **407.99 μs** |   **7.795 μs** |   **6.910 μs** |   **405.98 μs** |
| **100**       | **EventStorage**           | **EventStore**      | **1,525.07 μs** |  **46.907 μs** | **137.571 μs** | **1,520.31 μs** |
| **100**       | **EventStorage**           | **Marten**          | **1,109.95 μs** |  **41.897 μs** | **122.217 μs** | **1,070.55 μs** |
| **100**       | **EventStorage**           | **Memory**          |   **892.53 μs** |  **17.105 μs** |  **21.006 μs** |   **896.86 μs** |
| **100**       | **LogStorage**             | **Memory**          |   **181.01 μs** |   **3.615 μs** |   **6.878 μs** |   **179.39 μs** |
| **100**       | **LogStorage**             | **PostgreSQL**      |   **877.18 μs** |  **23.539 μs** |  **69.035 μs** |   **888.86 μs** |
| **100**       | **LogStorage**             | **Redis**           |   **848.81 μs** |  **26.309 μs** |  **76.328 μs** |   **853.82 μs** |
| **100**       | **StateStorage**           | **Memory**          |   **564.84 μs** |  **11.261 μs** |   **9.983 μs** |   **566.27 μs** |
| **100**       | **StateStorage**           | **PostgreSQL**      |   **787.74 μs** |  **21.331 μs** |  **61.885 μs** |   **753.64 μs** |
| **100**       | **StateStorage**           | **Redis**           | **1,015.68 μs** |  **25.919 μs** |  **75.197 μs** | **1,027.74 μs** |
| **1000**      | **EventStorage**           | **EventStore**      | **7,560.21 μs** | **180.575 μs** | **526.746 μs** | **7,521.14 μs** |
| **1000**      | **EventStorage**           | **Marten**          | **5,218.52 μs** | **103.808 μs** | **209.696 μs** | **5,194.05 μs** |
| **1000**      | **EventStorage**           | **Memory**          | **6,658.80 μs** |  **58.893 μs** |  **55.089 μs** | **6,656.75 μs** |
| **1000**      | **LogStorage**             | **Memory**          | **6,453.05 μs** |  **77.761 μs** |  **72.738 μs** | **6,437.06 μs** |
| **1000**      | **LogStorage**             | **PostgreSQL**      | **5,809.24 μs** | **134.403 μs** | **392.060 μs** | **5,746.88 μs** |
| **1000**      | **LogStorage**             | **Redis**           | **6,368.12 μs** | **131.594 μs** | **383.865 μs** | **6,446.44 μs** |
| **1000**      | **StateStorage**           | **Memory**          | **9,102.03 μs** | **179.913 μs** | **184.758 μs** | **9,104.12 μs** |
| **1000**      | **StateStorage**           | **PostgreSQL**      | **5,484.41 μs** | **113.567 μs** | **331.281 μs** | **5,399.76 μs** |
| **1000**      | **StateStorage**           | **Redis**           | **6,464.13 μs** | **129.128 μs** | **368.409 μs** | **6,498.21 μs** |
