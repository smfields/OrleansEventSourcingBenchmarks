namespace Runner.Configuration.Utilities;

public static class ParameterConstants
{
    public static class EventStorageProvider
    {
        public const string ParameterName = nameof(EventStorageProvider);
        public const string Memory = nameof(Memory);
        public const string EventStore = nameof(EventStore);
    }
    
    public static class GrainStorageProvider
    {
        public const string ParameterName = nameof(GrainStorageProvider);
        public const string Memory = nameof(Memory);
        public const string Postgres = nameof(Postgres);
        public const string Redis = nameof(Redis);
    }
    
    public static class LogConsistencyProvider
    {
        public const string ParameterName = nameof(LogConsistencyProvider);
        public const string LogStorage = nameof(LogStorage);
        public const string StateStorage = nameof(StateStorage);
        public const string EventStorage = nameof(EventStorage);
    }
}