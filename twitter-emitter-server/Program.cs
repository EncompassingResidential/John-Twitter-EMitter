using twitter_emitter_server.Models;
using twitter_emitter_server.Data;
using Npgsql;
// using Microsoft.Extensions.Configuration.Json;
// using Microsoft.Extensions.Configuration.Binder;

var builder = WebApplication.CreateBuilder(args);

/*
 * Add services to the container.
 * 
 * from file launchSettings.json
 * builder.Environment.EnvironmentName string is <Development315>
 * builder.Environment.ApplicationName string is <twitter-emitter-server>
 *
 */
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

Console.WriteLine($"builder.Environment.EnvironmentName string is <{env}>");
Console.WriteLine($"builder.Environment.ApplicationName string is <{appName}>");

var configDBPostgres = ConfigurationHelper.GetByName("DatabaseSettings:PostgreSQL-twitter-emitter");
Console.WriteLine($"config.GetSection('DatabaseSettings')['PostgreSQL - twitter - emitter'] <<{configDBPostgres}>>");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* 
 * this didn't work for PostgreSQL
 * it was originally for MS SQL Server connection
 * and due to Package Dependency MS.EntityFrameworkCore I thought it would work with PostgreSQL
 * 
 * builder.Services.AddDbContext<IssueDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("PostgreSQL"))
    ); */

// var cs = builder.Configuration.GetConnectionString("PostgreSQL-twitter-emitter");

//  The next PostgreSQL lines are from https://zetcode.com/csharp/postgresql/
var cs = "Server=localhost;Database=twitter-emitter;Port=1463;User Id=twitter-emitter-write;Password=";

Console.WriteLine($"sql connection string is <{cs}>");

var con = new NpgsqlConnection(cs);

con.Open();

var sql = "SELECT version()";
Console.WriteLine($"sql string is <{sql}>");

var cmd = new NpgsqlCommand(sql, con);

if (cmd is not null) {
    var version = cmd.ExecuteScalar().ToString();
    Console.WriteLine($"PostgreSQL version: {version}");
}
else { 
    Console.WriteLine($"PostgreSQL NpgsqlCommand returned NULL for : {sql}");
}

sql = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname != 'information_schema' AND schemaname != 'pg_catalog'";
Console.WriteLine($"SQL string is <{sql}>");

cmd = new NpgsqlCommand(sql, con);

using NpgsqlDataReader rdr = cmd.ExecuteReader();

if (cmd is not null && rdr.HasRows) {
    // https://www.npgsql.org/doc/api/Npgsql.NpgsqlDataReader.html

    object[] rowObjects = new object[rdr.FieldCount];

    while (rdr.Read()) {
        Console.WriteLine($"PG: rdr.GetFieldType(0) {rdr.GetFieldType(0)},  rdr.GetFieldType(1) {rdr.GetFieldType(1)},  rdr.GetFieldType(2) {rdr.GetFieldType(2)},  rdr.GetFieldType(3) {rdr.GetFieldType(3)},  rdr.GetFieldType(4) {rdr.GetFieldType(4)}");
        Console.WriteLine($"PG: rdr.GetValue(0)     {rdr.GetValue(0)},         rdr.GetValue(1)     {rdr.GetValue(1)},  rdr.GetValue(2)     {rdr.GetValue(2)},  rdr.GetValue(3)     {rdr.GetValue(3)},  rdr.GetValue(4)     {rdr.GetValue(4)}");

        int columnsReturned = rdr.GetValues(rowObjects);
        Console.WriteLine($"PG: # of Columns returned {columnsReturned}  rdr.GetValues(rowObjects) ::: {rowObjects}");

        for (int i = 0; i < columnsReturned; i++) {
            Console.Write(rowObjects[i]);
            if (i == columnsReturned - 1) {
                Console.WriteLine(" ;");
            } else {
                Console.Write(", ");
            }
        }

        // With NpgsqlDataReader.Get<type>(column) if the value is null you have to check for it or C# craps out.
        // Console.WriteLine($"PG: rdr.GetString(0)    {rdr.GetString(0)},         rdr.GetString(1)    {rdr.GetString(1)},  rdr.GetString(2)    {rdr.GetString(2)}, {rdr.GetString(3)}, {rdr.GetBoolean(4)}"); // , {rdr.GetBoolean(5)}, {rdr.GetBoolean(6)}, {rdr.GetBoolean(7)}");
    }
}
else {
    Console.WriteLine($"PostgreSQL NpgsqlCommand returned NULL for : {sql}");
}

rdr.Close();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
