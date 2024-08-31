using Blog.Extensions;
using Blog.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddProjectServices(builder.Configuration);
builder.Services.AddDependencyServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

