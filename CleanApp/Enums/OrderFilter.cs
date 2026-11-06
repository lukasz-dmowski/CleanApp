namespace CleanApp.Enums
{
    public enum OrderFilter
    {
        Overdue = 1,
        Today = 2,
        Upcoming = 3,

        Pending = 11,
        Partial = 12,
        Completed = 13,

        Notified = 21,
        NotNotified = 22,

        Collected = 31,
        NotCollected = 32,

        SearchPhrase = 41,
    }
}
