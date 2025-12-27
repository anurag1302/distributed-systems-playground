using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddHttpClient("payments", client =>
// {
//     client.Timeout = TimeSpan.FromSeconds(2);
// });


builder.Services.AddHttpClient("payments", client =>
    {
        client.Timeout = TimeSpan.FromSeconds(2);
    })
    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3,
        retryAttempt =>
        {
            Console.WriteLine($"Retry attempt - {retryAttempt}");
            return TimeSpan.FromMilliseconds(200);
        }))
    .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, 
        TimeSpan.FromSeconds(30)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();