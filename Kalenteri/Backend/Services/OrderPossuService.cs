


using System.Data;
using Kalenteri.Backend.Models;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL.Converters;

namespace Kalenteri.Backend.Services;

public static class OrderPossuService
{
    private static readonly IConfiguration configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    private static readonly string? ConnectionString = configuration.GetConnectionString("KalenteriDatabase");

    

    /*

    create table kalenteri_order
       (
           id               uuid primary key not null default gen_random_uuid(),
           identifier       varchar          not null,
           location       varchar          not null
       );
       
       create table kalenteri_box
       (
           id               uuid primary key not null default gen_random_uuid(),
           order_id        uuid             not null,
           delivery_time     timestamp,
           pickup_time     timestamp
       );
       
       
       insert into kalenteri_order (id, identifier, location) values ('3c2cf677-bb9b-4254-b6e3-b0f1b6c846cd', 'test', 'location1');
       
       insert into kalenteri_box (order_id, delivery_time) values ('3c2cf677-bb9b-4254-b6e3-b0f1b6c846cd', '2024-07-07 19:10:25-07');
       insert into kalenteri_box (order_id, delivery_time) values ('3c2cf677-bb9b-4254-b6e3-b0f1b6c846cd', '2024-07-02 19:10:25-07');
       
    
     */


    public static Order UpdateData(string identifier, Box box)
    {
        IDbConnection db = CreateConnection();
        db.Update(box);
        db.Close();

        return GetData(identifier);
    }


    public static Order GetData(string identifier)
    {
        IDbConnection db = CreateConnection();

        var q = db.From<Order>()
            .Join<Box>()
            .Where(o => o.Identifier == identifier);

        var results = db.SelectMulti<Order, Box>(q);

        Order order = new Order();
        foreach (var tuple in results)
        {
            if (order.Identifier == String.Empty)
            {
                order = tuple.Item1;
            }
            Box box = tuple.Item2;
            order.Boxes.Add(box);
        }

        return order;
    }


    private static IDbConnection CreateConnection()
    {
        //TODO: TEMP LOGGING, remove
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger("Kalenteri");
        
        PostgreSqlDialect.Provider.RegisterConverter<List<DateTime>>(new PostgreSqlDateTimeTimeStampArrayConverter());
        var dbFactory = new OrmLiteConnectionFactory(ConnectionString, PostgreSqlDialect.Provider);
        //TODO: TEMP LOGGING, remove
        logger.LogInformation("ConnectionString: " + dbFactory.ConnectionString);
        var keys = configuration.AsEnumerable().ToList();
        foreach (var key in keys) {
            logger.LogInformation("key: " + key.Key + " value: " + key.Value);
        }
        return dbFactory.OpenDbConnection();
    }
}