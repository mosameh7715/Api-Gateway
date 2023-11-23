using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/**ASSIGN OCELOT.JSON FILE CONFIGURATIONS**/
builder.Configuration.AddJsonFile("ocelot.json",optional: false,reloadOnChange: true);

builder.Services.AddAuthentication("Bearer")
	.AddJwtBearer(options =>
	{
		options.Authority = "https://localhost:7218";
		options.TokenValidationParameters.ValidateAudience = false;
	});

/**CUSTOME CONFIGURATIONS FOR OCELOT CACHING And QoS**/
builder.Services.AddOcelot(builder.Configuration)
	.AddCacheManager(x =>
	{
		x.WithDictionaryHandle();
	})
	.AddPolly();

var app = builder.Build();


if(app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


/**USE OCELOT**/
app.UseOcelot().Wait();

app.Run();
