// using MyWebApiApp.Data;
// using MyWebApiApp.Middlewares;
// using MyWebApiApp.Services.Implementations;
// using MyWebApiApp.Services.Interfaces;
// using System.Reflection;
// using Scrutor;
// using MyWebApiApp.Utilities;

// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         policy =>
//         {
//             policy.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader()
//                   .AllowCredentials(); 
//         });
// });

// // Add services to the container.

// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddHttpContextAccessor();
// builder.Services.AddSingleton<DBHelper>();
// builder.Services.AddScoped<UserRepository>();
// builder.Services.AddScoped<ProductRepository>();
// builder.Services.AddScoped<CartRepository>();
// builder.Services.AddScoped<CartItemRepository>();
// builder.Services.AddScoped<InvoiceRepository>();

// builder.Services.Scan(scan => scan
//     .FromAssemblies(Assembly.GetExecutingAssembly())
//     .AddClasses(classes => classes.InNamespaces("MyWebApiApp.Services.Implementations"))
//     .AsImplementedInterfaces()
//     .WithScopedLifetime()
// );
// builder.Services.AddDistributedMemoryCache(); // Stores session in memory
// builder.Services.AddSession(options =>
// {
//     options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
//     options.Cookie.HttpOnly = true;
//     options.Cookie.IsEssential = true;
// });


// var app = builder.Build();
// app.UseCors("AllowAll");
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // app.UseHttpsRedirection();
// app.UseSession();

// app.UseAuthorization();
// app.UseMiddleware<SessionAuthMiddleware>();
// app.MapControllers();

// app.Run();
using MyWebApiApp.Data;
using MyWebApiApp.Middlewares;

using MyWebApiApp.Utilities;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Builder Cerated");

// ✅ CORS setup - replace with your real frontend URL
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5501","http://localhost:4200") // e.g., http://127.0.0.1:5500
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); // ✅ allow sending cookies
        });

});
Console.WriteLine("Cors added");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    // ✅ Required for cross-origin cookies
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

Console.WriteLine("sesssion Cerated");


// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

Console.WriteLine("Add sevices");


// ✅ Dependencies
builder.Services.AddSingleton<DBHelper>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<CartItemRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<InvoiceItemRepository>();
builder.Services.AddScoped<LogRepository>();

Console.WriteLine("Builder depedneci");



builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(classes => classes.InNamespaces("MyWebApiApp.Services.Implementations"))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

Console.WriteLine("Serivecessss");


// ✅ Session config
builder.Services.AddDistributedMemoryCache();
// builder.Services.AddScoped<LogActionAttribute>();
Console.WriteLine("memory cactch");


var app = builder.Build();

Console.WriteLine("App cretaed");


app.UseMiddleware<GlobalExceptionMiddleware>();
Console.WriteLine("global error");

// ✅ Use correct CORS policy

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
Console.WriteLine("app swagger");

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

Console.WriteLine("use cors");


// ✅ Order matters — session BEFORE auth & middleware
app.UseSession();
Console.WriteLine("use session");


app.UseAuthorization();
// app.Use(async (context, next) =>
// {
//     context.Request.EnableBuffering(); // ✅ Allows multiple reads of the body

//     using (var reader = new StreamReader(
//         context.Request.Body,
//         encoding: System.Text.Encoding.UTF8,
//         detectEncodingFromByteOrderMarks: false,
//         leaveOpen: true))
//     {
//         var body = await reader.ReadToEndAsync();
//         Console.WriteLine("Request Body: " + body);

//         // Reset the stream position so the next middleware/controller can read it
//         context.Request.Body.Position = 0;
//     }

//     await next();
// });
app.UseMiddleware<SessionAuthMiddleware>();

app.MapControllers();

app.Run();
