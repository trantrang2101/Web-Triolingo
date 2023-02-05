
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Logger;
using Web_Triolingo.ServiceManager.Lessons;
using Web_Triolingo.ServiceManager.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddProvider(new Log4NetManager());
    builder.AddConsole();
});
builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<ILessonService, LessonService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
