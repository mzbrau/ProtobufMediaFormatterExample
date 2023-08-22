using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProtoBuf;

namespace ProtobufApi.Test;

[TestFixture]
public class Tests
{
    private WebApplicationFactory<Program> _app;

    [SetUp]
    public void Setup()
    {
        _app = new();
    }

    [Test]
    public async Task CallApiJson()
    {
        var client = _app.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var weather = await client.GetFromJsonAsync<List<WeatherForecast>>("/WeatherForecast");

        Assert.That(weather, Is.Not.Null);
        Assert.That(weather!.Count, Is.EqualTo(5));
    }

    [Test]
    public async Task CallApiProtobuf()
    {
        var client = _app.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
        var response = await client.GetAsync("/WeatherForecast");
        var content = await response.Content.ReadAsStreamAsync();
        
        var weather = Serializer.Deserialize<List<WeatherForecast>>(content);
        Assert.That(weather, Is.Not.Null);
        Assert.That(weather!.Count, Is.EqualTo(5));
    }
    
    [Test]
    public async Task CallApiNoHeaders()
    {
        var client = _app.CreateClient();
        var weather = await client.GetFromJsonAsync<List<WeatherForecast>>("/WeatherForecast");

        Assert.That(weather, Is.Not.Null);
        Assert.That(weather!.Count, Is.EqualTo(5));
    }
}