using Gadz.Tetris.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// raiz de composição da Web: o único lugar que cria a infraestrutura do navegador e a registra para o aparelho
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// textos do Gadz.Tetris.Texts, na cultura do navegador
builder.Services.AddLocalization();

await builder.Build().RunAsync();
