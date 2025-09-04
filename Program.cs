using Amazon;
using Amazon.S3;
using MyWebApiApp.Data;
using MyWebApiApp.Kafka;
using MyWebApiApp.Middlewares;
using MyWebApiApp.Utilities;
using System.Reflection;
using WebApiProject.Kafka;

var builder = WebApplication.CreateBuilder(args);

// ✅ CORS setup - replace with your real frontend URL
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5501", "http://localhost:4200") // e.g., http://127.0.0.1:5500
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); // ✅ allow sending cookies
        });

});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    // ✅ Required for cross-origin cookies
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
var awsConfig = builder.Configuration.GetSection("AWS");

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    return new AmazonS3Client(
        awsConfig["AccessKey"],
        awsConfig["SecretKey"],
        RegionEndpoint.GetBySystemName(awsConfig["Region"]));
});

builder.Services.AddSingleton<string>(awsConfig["BucketName"]); // register bucket name
// ✅ Dependencies
builder.Services.AddSingleton<DBHelper>();
builder.Services.AddSingleton<KafkaProducer>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<CartItemRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<InvoiceItemRepository>();
builder.Services.AddScoped<LogRepository>();
builder.Services.AddScoped<ReportRepository>();
builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(classes => classes.InNamespaces("MyWebApiApp.Services.Implementations"))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

// ✅ Session config
builder.Services.AddDistributedMemoryCache();
// builder.Services.AddScoped<LogActionAttribute>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
// ✅ Use correct CORS policy

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
// ✅ Order matters — session BEFORE auth & middleware
app.UseSession();

app.UseMiddleware<SessionAuthMiddleware>();

app.MapControllers();

app.Run();
