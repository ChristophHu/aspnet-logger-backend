using aspnet_logger_backend.Services;
using log4net.Config;
using Microsoft.IdentityModel.Logging;
using aspnet_logger_backend.Utils;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace aspnet_logger_backend;

public class Program {
    public static void Main(string[] args) {

        // Log4net - logger configuration
        XmlConfigurator.Configure(new FileInfo("log4net.config"));

        var builder = WebApplication.CreateBuilder(args);

        ConfigurationManager cfgmgr = builder.Configuration;
        ConfigService config = new ConfigService(cfgmgr);

        IdentityModelEventSource.ShowPII = true;

        SwaggerExtensions.ConfigureSwaggerBuilder(builder, cfgmgr);
        ConfigurationHelper.configureBuilder(builder, cfgmgr, config);


        // Add services to the container.
        //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();
        //builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        var app = builder.Build();

        SwaggerExtensions.ConfigureSwaggerApp(app, cfgmgr);
        ConfigurationHelper.configureApp(app);

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    // app.MapOpenApi();
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        // app.UseHttpsRedirection();

        //app.MapControllers();

        app.Run();
    }
}