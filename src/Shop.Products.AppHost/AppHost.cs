var builder = DistributedApplication.CreateBuilder(args);

var saPassword = builder.AddParameter("SqlPassword", secret: true);

var sql = builder
    .AddSqlServer("sql", password: saPassword)
    .AddDatabase("ProductsDb");

var kafka = builder.AddKafka(name: "kafka", port: 51800).WithKafkaUI();

builder.AddProject<Projects.Shop_Products_Api>("shop-products-api")
    .WaitFor(kafka)
    .WaitFor(sql)
    .WithReference(kafka)
    .WithReference(sql);

await builder.Build().RunAsync();