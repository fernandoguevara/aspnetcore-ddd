namespace Notes.BackgroundTasks
{
    public class BackgroundTaskSettings
    {
        public string DatabaseVendor { get; set; }
        public string ConnectionString { get; set; }
        public string SQLServerConnectionString { get; set; }
        public string PostgresConnectionString { get; set; }
        public string ReportEmail { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public int GracePeriodTime { get; set; }
        public int CheckUpdateTime { get; set; }
        public string SubscriptionClientName { get; set; }
    }
}