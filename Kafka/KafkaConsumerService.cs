using Confluent.Kafka;
using System.Text.Json;
using MyWebApiApp.Models;
using MyWebApiApp.Data;

namespace WebApiProject.Kafka
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _config;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<KafkaConsumerService> logger)
        {
            _scopeFactory = scopeFactory;
            _config = config;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _config["Kafka:BootstrapServers"],
                    GroupId = "report-api-consumer",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true
                };

                using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
                consumer.Subscribe(_config["Kafka:ReportResponseTopic"]);

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = consumer.Consume(stoppingToken);
                            if (result == null) continue;

                            HandleMessage(result.Message.Value).GetAwaiter().GetResult();
                        }
                        catch (ConsumeException ex)
                        {
                            _logger.LogError("Kafka consume error: {Error}", ex.Error.Reason);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Unexpected error while consuming Kafka message.");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Kafka consumer stopping...");
                    consumer.Close();
                }
            }, stoppingToken);
        }

        private async Task HandleMessage(string message)
        {
            _logger.LogInformation("Received Kafka message: {Message}", message);

            var response = JsonSerializer.Deserialize<ReportResponse>(message);
            if (response == null)
            {
                _logger.LogWarning("Kafka message could not be deserialized.");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ReportRepository>();

            var existing = await repo.GetReportAsync(response.ReportId);
            if (existing != null)
            {
                existing.Status = response.Status;
                existing.S3Key = response.S3Key;
                existing.CompletedAt = DateTime.UtcNow;
                await repo.UpdateReportAsync(existing);
                _logger.LogInformation("Report {ReportId} updated to {Status}", existing.ReportId, existing.Status);
            }
            else
            {
                _logger.LogWarning("Report {ReportId} not found in DB.", response.ReportId);
            }
        }

    }
}
