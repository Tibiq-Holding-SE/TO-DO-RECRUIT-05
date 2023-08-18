using Microsoft.AspNetCore.Mvc;
using TaskManagerLite.Data;
using TaskManagerLite.Interfaces;
using TaskManagerLite.Repositories;
using TaskManagerLite.Services.Background;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<BackgroundWorker>();

builder.Services.AddDbContext<TaskContext>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IBackgroundRepository, BackgroundRepository>();

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

builder.Services.Configure<MvcOptions>(options =>
{
    options.FormatterMappings.SetMediaTypeMappingForFormat("ts", "application/typescript");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.UseAuthorization();

app.UseMvc();

app.MapRazorPages();

app.Run();
