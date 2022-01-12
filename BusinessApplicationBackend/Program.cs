var builder = WebApplication.CreateBuilder(args);

var AllowFrontEnd = "_AllowFrontEnd";

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowFrontEnd,
    builder =>
    {
        builder.WithOrigins("http://localhost")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(AllowFrontEnd);


app.UseAuthorization();

app.MapControllers();

app.Run();
