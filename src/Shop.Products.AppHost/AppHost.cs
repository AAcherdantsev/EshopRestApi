using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder
    .AddSqlServer("sql", null, 1433)
    .WithImageTag("2022-latest")
    .WithBindMount("./sql-data", "/var/opt/mssql")
    .AddDatabase("ProductsDb");

var kafka = builder.AddKafka("kafka", 51800).WithKafkaUI();

builder.AddProject<Shop_Products_Api>("shop-products-api")
    .WaitFor(kafka)
    .WaitFor(sql)
    .WithReference(kafka)
    .WithReference(sql);

await builder.Build().RunAsync();