using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NaturalFirstAPI.Model;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql;

namespace NaturalFirstAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowSpecificOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                //c.AddPolicy("AllowSpecificOrigin", options => options.WithOrigins("https://naturalfirst.in").AllowAnyMethod().AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}