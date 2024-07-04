using Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Configure IAppConfig as a singleton service

// Configure HttpClient and FizzBuzz service
builder.Services.AddHttpClient<IFizzBuzz, FizzBuzz>(client =>
{
    string url = builder.Configuration.GetValue<string>("AppConfig:Url") ?? string.Empty;
    client.BaseAddress = new Uri(url);
});

// Add other necessary services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
