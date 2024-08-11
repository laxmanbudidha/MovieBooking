using Business_Logic_Layer.UserServices;
using Data_Access_Layer.UserServices;
using Business_Logic_Layer.MovieServices;
using Data_Access_Layer.MovieRepository;
using Business_Logic_Layer.TicketServices;
using Data_Access_Layer.TicketRepository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommonMethods;


namespace MyShow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Configure services
            builder.Services.AddControllers();
            builder.Services.AddTransient<IUserRepository>(provider =>
                new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddTransient<IUserService, UserService>(); // Register IUserService

            

            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            // Register services to the container.
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<ITicketService, TicketService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlingMiddleware>();  // Register your custom middleware

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();


           

            
        }
    }
}