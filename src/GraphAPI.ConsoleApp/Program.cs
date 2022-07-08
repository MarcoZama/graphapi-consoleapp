using GraphAPI.Library;
using GraphAPI.Library.Interfaces;
using GraphAPI.Library.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

var graphApiConfig = config.GetSection("GraphApi").Get<GraphApiConfig>();

var serviceProvider = new ServiceCollection()           
           .AddSingleton<IGraphAPIService, GraphAPIService>()           
           .BuildServiceProvider();

var service = new GraphAPIService(graphApiConfig);

var result = await service.CheckGroupByGroupIdAndUserAsync("554b0cf3-7266-471c-ba1d-c698aee5381f", "07d76e5f-8084-47b9-91e5-834e662417f9");

Console.WriteLine(result);