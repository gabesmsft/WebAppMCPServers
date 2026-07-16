var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMcpServer()
    .WithHttpTransport() // With streamable HTTP
    .WithToolsFromAssembly(); // Add all classes marked with [McpServerToolType]

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    // Keep local MCP client traffic on HTTP to avoid dev certificate trust issues.
    app.UseWhen(
        context => !context.Request.Path.StartsWithSegments("/api/mcp"),
        branch => branch.UseHttpsRedirection());
}
else
{
    app.UseHttpsRedirection();
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapMcp("/api/mcp");
app.UseCors();

app.Run();
