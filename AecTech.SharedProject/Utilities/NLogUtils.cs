using Elastic.CommonSchema.NLog;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets.ElasticSearch;

namespace AecTech.Utilities
{
    public static class NLogUtils
    {
        public static LoggingConfiguration Config { get; set; } = CreateConfiguration();

        public static LoggingConfiguration CreateConfiguration()
        {
            // (Konrad) ElasticSearch Target Setup
            Layout.Register<EcsLayout>("EcsLayout");

            var config = new LoggingConfiguration();
            var esTarget = new ElasticSearchTarget
            {
                CloudId = "agent-test:dXMtZWFzdC0yLmF3cy5lbGFzdGljLWNsb3VkLmNvbTo0NDMkYjdkOGY3ZTAzYmYxNGY0ODgwZDkyYzNmZDAwZWEzZGMkM2Q1Y2MyNTFjMGJiNGM0NGE1ZDhjMmM1ZTAwZmJlNWQ=",
                Username = "elastic",
                Password = "2W30vrdO3quYN6dQWpo8y6Cu",
                Index = "aec-tech-logs",
                DocumentType = "",
                IncludeDefaultFields = true,
                IncludeAllProperties = true,
                Layout = new EcsLayout(),
                EnableJsonLayout = true
            };

            config.AddTarget("es-database", esTarget);

            var rule1 = new LoggingRule("aec_tech_logger", LogLevel.Trace, esTarget);
            config.LoggingRules.Add(rule1);

            return config;
        }

        public static Logger GetAecTechLogger()
        {
            LogManager.Configuration = Config;
            return LogManager.GetLogger("aec_tech_logger");
        }
    }
}
