using GesCPSI_Project.Components;
using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using GesCPSI_Project.Reports;
using GesCPSI_Project.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MudBlazor
builder.Services.AddMudServices();

builder.Services.AddControllers();

// DbContext PostgreSQL
builder.Services.AddDbContext<GesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Services métier
builder.Services.AddScoped(typeof(ICrud<>), typeof(ServiceBase<>));
builder.Services.AddScoped<IAjoutAct, AjoutActService>();
builder.Services.AddScoped<ITypesAct, TypesActService>();
builder.Services.AddScoped<IClient, ClientService>();
builder.Services.AddScoped<IClientAct, ClientActService>();
builder.Services.AddScoped<IRolesClientAct, RolesClientActService>();
builder.Services.AddScoped<IBanque, BanqueService>();

builder.Services.AddScoped<ICautionnement, CautionnementService>();
builder.Services.AddScoped<IPret, PretService>();
builder.Services.AddScoped<ICharge, ChargeService>();
builder.Services.AddScoped<IAutorisation, AutorisationService>();
builder.Services.AddScoped<IEntiteJur, EntiteJurService>();
builder.Services.AddScoped<IUser, UserService>();

//builder.Services.AddScoped<IEmail, EmailService>();
//ilder.Services.AddScoped<IStorageService, MinioStorageService>();
builder.Services.AddSingleton<ActeRefreshService>();

builder.Services.AddScoped<ActReportDataBuilder>();
builder.Services.AddScoped<ActReportJsonService>();
builder.Services.AddScoped<ActReportPdfService>();
builder.Services.AddScoped<ActReportWorkflowService>();










// AddMudServices
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
});



// HttpClient pour API
builder.Services.AddHttpClient("ApiCPSI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7071/");
});


// Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});



var app = builder.Build();

// ? Seed User #1 (SYSTEM) si absent
await SeedSystemUserAsync(app);

static async Task SeedSystemUserAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GesDbContext>();

    var exists = await db.Set<UserModel>().AnyAsync(u => u.IdUser == 1);
    if (!exists)
    {
        db.Set<UserModel>().Add(new UserModel
        {
            IdUser = 1,
            NameUser = "SYSTEM",
            SurnameUser = "CPSI",
            EmailUser = "system@cpsi.local",
            KeycloakId = "LOCAL-SEED-USER-1",
            Departement = "Juridique",
            Fonction = "Admin",
            DateCreation = DateTime.UtcNow,
            IsActive = true
        });

        await db.SaveChangesAsync();
    }
}


// ? Seed RoleClient 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GesDbContext>();

    var needed = new[] { "Debiteur", "Caution", "Temoin1", "Temoin2" };

    foreach (var lib in needed)
    {
        var exists = await db.Set<RolesClientActModel>()
            .AnyAsync(r => r.Libelle.ToLower() == lib.ToLower());

        if (!exists)
        {
            db.Set<RolesClientActModel>().Add(new RolesClientActModel
            {
                Libelle = lib,
                MentionManuelle = ""
            });
        }
    }

    await db.SaveChangesAsync();
}



// ==================== PIPELINE ====================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
