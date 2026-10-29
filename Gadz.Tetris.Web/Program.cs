using Gadz.Tetris.Domain.Models.Statistics;
using Gadz.Tetris.Web;
using Gadz.Tetris.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

// raiz de composição da Web: o único lugar que cria a infraestrutura do navegador e a registra para o aparelho
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// textos do Gadz.Tetris.Texts, na cultura do navegador
builder.Services.AddLocalization();

// no WebAssembly o runtime JavaScript também atende chamadas síncronas, que a IStatsRepository precisa
builder.Services.AddSingleton<IStatsRepository>(services
    => new LocalStorageStatsRepository((IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>()));

await builder.Build().RunAsync();
