
using Microsoft.EntityFrameworkCore;
using Web_Triolingo.Interface.Courses;
using Web_Triolingo.Interface.Lessons;
using Web_Triolingo.Interface.QnA;
using Web_Triolingo.Interface.Settings;
using Web_Triolingo.Interface.Units;
using Web_Triolingo.Interface.User;
using Web_Triolingo.Logger;
using Web_Triolingo.ServiceManager.Courses;
using Web_Triolingo.ServiceManager.Lessons;
using Web_Triolingo.ServiceManager.QnA;
using Web_Triolingo.ServiceManager.Settings;
using Web_Triolingo.ServiceManager.Units;
using Web_Triolingo.ServiceManager.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddProvider(new Log4NetManager());
    builder.AddConsole();
});
builder.Services.AddTransient<IQuestion, QuestionService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<ILessonService, LessonService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUnitService, UnitService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(60);//You can set Time   
});
builder.Services.AddHttpContextAccessor();
var configuration = builder.Configuration;
string connectionString = configuration.GetConnectionString("TriolingoConStr");
builder.Services.AddDbContext<Triolingo.Core.DataAccess.TriolingoDbContext>(options =>
        options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
