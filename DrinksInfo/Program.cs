using DrinksInfo;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Reporter");



var response = await ProcessRepositoriesAsync(client);

foreach(var drink in response)
{
    Console.WriteLine(drink.idDrink);
    Console.WriteLine(drink.strDrink);
}

static async Task<List<Drinks>> ProcessRepositoriesAsync(HttpClient client)
{
    var response = await client.GetFromJsonAsync<Root>(
        "http://www.thecocktaildb.com/api/json/v1/1/filter.php?c=Cocktail");

    return response?.drinks ?? new List<Drinks>();
}