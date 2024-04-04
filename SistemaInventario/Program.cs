using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.data;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Agregamos el servicio de Unidad de Trabajo- tiene que ser antes de var app = builder.Build();//
//El AddScoped permite que la instancia se cree 1 ves y pueda usarse las veces que se requiera//

builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
