using sew_Projektverwaltung.Components;
using RabbitMQ.Client;

using System.Text;
using projektverwaltung;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();




/*
* Legen Sie für die folgende Aufgabenstellung eine Infrastruktur im Managementplugin an.
* Programmieren Sie anschließend die entsprechenden Komponenten mit dem .net Framework.
* Überglegen Sie welche Komponenten und Service Sie brauchen.
* Behalten Sie die Koppelung und dei Kohäsion des Systems im Auge.

* Zur Verwaltung von Projekten soll an einer Forschungseinrichtung ein neues Tool entwickelt.
______________________________________________________________________________________________________
ProjectApprovementService

   Schreiben Sie ein REST Service zur Genehmigung von Projekten

   REST Schnittstelle
   address: IP:PORT/project/approvement/init method: POST
   data: ProjectDTO

   Befindent sich ein Projekt im Zustand CREATED wird es zur Genehmigung frei- gegeben.
   Projekte in anderen Zustaenden werden verworfen.
______________________________________________________________________________________________________
Faculty Queues

   Je nach Umsetzender Fakultaet wird das Projekt an den Leiter der entsprechenden Fakultaet geschickt.

   Richten fuer jede Fakultaet eine eigene Queue ein.

   Der Leiter der Fakultaet bestaetigt das Projekt mit einer Wahrscheinlichkeit von
   80 Prozent. Wurde ein Projekt genehmigt wird der ProjectState auf APPROVED gesetzt.

   Wird ein Projekt nicht genehmigt wird der ProjectState auf CANCLED gesetzt.

   Simulieren Sie den Genehmigungsprozess entsprechend. Vergessen Sie nicht alles entsprechend zu  loggen.
______________________________________________________________________________________________________
Result Producer

   Alle genehmigten Projekte werden an eine bestimmte Queue zur Publikation weiter- geschickt.
   Abgelehnte Projekte werden an eine andere Queue geschickt.

*/


var factory = new ConnectionFactory
{
    HostName = "127.0.0.1",
    Port = 5672,
    UserName = "rabbit",
    Password = "rabbitpwd",
};

/*
static void AddRabbitMQService(this IServiceCollection services)
{
    services.AddSingleton<RabbitMQService>();
}
*/





app.Run();