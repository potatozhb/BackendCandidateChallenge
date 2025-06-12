using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizService.Data;
using QuizService.Repos;
using QuizService.Repos.Interfaces;
using QuizService.Services;
using QuizService.Services.Interfaces;

namespace QuizService;

public class Startup
{
    /*TODO:
    1. refactor dto.
    2. automap or create a mapper class for data modeul and dto
    3. Delete function need check more things. for instance, 
        if delete right answer, need give user a warning.
    4. when we add answer, don't need check quiz

    */
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddSingleton(InitializeDb());

        services.AddDbContext<QuizDbContext>((sp, options) =>
        {
            var conn = sp.GetRequiredService<IDbConnection>() as SqliteConnection;
            options.UseSqlite(conn);

        });

        services.AddControllers();

        services.AddTransient<Seed>();
        services.AddScoped<IAnswerRepo, AnswerRepo>();
        services.AddScoped<IQuestionRepo, QuestionRepo>();
        services.AddScoped<IQuizRepo, QuizRepo>();
        services.AddScoped<IQuizService, QuizzesService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAnswerService, AnswerService>();

        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizService API V1");
            });
            app.UseDeveloperExceptionPage();
        }
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
        db.Database.EnsureCreated();

        var seed = scope.ServiceProvider.GetRequiredService<Seed>();
        seed.AddData();
    }

    private IDbConnection InitializeDb()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        return connection;
    }

}