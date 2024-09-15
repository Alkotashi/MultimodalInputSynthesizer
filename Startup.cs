using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

var envFilePath = ".env";
if (File.Exists(envFilePath))
{
    var envVars = File.ReadAllLines(envFilePath)
        .Where(line => line.Contains('='))
        .Select(line => line.Split('=', 2))
        .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

    foreach (var envVar in envVars)
    {
        Environment.SetEnvironmentVariable(envVar.Key, envVar.Value);
    }
}

builder.Services.Configure<YourServiceOptions>(builder.Configuration.GetSection("YourService"));
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();