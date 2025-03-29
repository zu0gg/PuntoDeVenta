var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// Habilitar sesiones
builder.Services.AddDistributedMemoryCache(); // Necesario para usar sesi�n
builder.Services.AddSession(); // Agregar servicio de sesi�n

var app = builder.Build();


app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=IniciarSesion}/{id?}");

app.Run();
