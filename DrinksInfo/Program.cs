using DrinksInfo;
using Spectre.Console;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Reporter");


var table = new Table();

var response = await ProcessRepositoriesAsync(client);

table.AddColumn("Id");
table.AddColumn("Name");

foreach (var drink in response)
{
    table.AddRow(drink.idDrink,  drink.strDrink);
    //AnsiConsole.WriteLine(drink.idDrink!);
    //AnsiConsole.WriteLine(drink.strDrink!);
    //using (var cliente = new HttpClient())
    //{
    //    var steam = await cliente.GetStreamAsync(drink.strDrinkThumb);
    //    var image = new CanvasImage(steam)
    //        .MaxWidth(20)
    //        .BicubicResampler();
    //    AnsiConsole.Write(image);
    //}
}

var selected = AnsiConsole.Prompt(
    new SelectionPrompt<Drinks>()
        .Title("Select a [green]drink[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to see more drinks)[/]")
        .UseConverter(drink => $"Id: {drink.idDrink}, Name: {drink.strDrink}"!)
        .EnableSearch()
        .SearchPlaceholderText("Type to search drinks...")
        .WrapAround()
        .AddChoices(response));
AnsiConsole.MarkupLine($"You selected: [yellow]{selected.strDrink}[/]");


static async Task<List<Drinks>> ProcessRepositoriesAsync(HttpClient client)
{
    var response = await client.GetFromJsonAsync<Root>(
        "http://www.thecocktaildb.com/api/json/v1/1/filter.php?c=Cocktail");

    return response?.drinks ?? new List<Drinks>();
}