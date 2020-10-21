using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy.Contributors;
using GeoInfoApp.OpenWeatherMap;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Moq;
using Moq.Protected;

namespace GeoInfoApp.Tests
{
	public class OpenWeatherMapClientTests
	{
		private ServiceCollection _serviceCollection;

		#region Data

		private const string SuccessResponse =
			"{\"coord\":{\"lon\":28.3,\"lat\":58.17},\"weather\":[{\"id\":701,\"main\":\"Mist\",\"description\":\"mist\",\"icon\":\"50d\"}],\"base\":\"stations\",\"main\":{\"temp\":6,\"feels_like\":3.47,\"temp_min\":6,\"temp_max\":6,\"pressure\":1012,\"humidity\":93},\"visibility\":3800,\"wind\":{\"speed\":2,\"deg\":150},\"clouds\":{\"all\":90},\"dt\":1603289019,\"sys\":{\"type\":1,\"id\":8928,\"country\":\"RU\",\"sunrise\":1603256178,\"sunset\":1603291569},\"timezone\":10800,\"id\":0,\"name\":\"Moscow1\",\"cod\":200}";

		#endregion

		[SetUp]
		public void Setup()
		{
			_serviceCollection = new ServiceCollection();
			_serviceCollection.AddTransient<OpenWeatherMapClient>();

			var someOptions = Options.Create<OpenWeatherMapOptions>(new OpenWeatherMapOptions
			{
				ApiKey = "TestKey",
				Units = "metrics"
			});
			_serviceCollection.AddSingleton(someOptions);
		}

		[Test]
		public void ShouldCreate()
		{
			_serviceCollection.AddTransient<HttpClient>();
			var provider = _serviceCollection.BuildServiceProvider();

			var client = provider.GetService<OpenWeatherMapClient>();

			Assert.That(client, Is.Not.Null);
		}

		[Test]
		public async Task SuccessTest()
		{
			var handlerMock = new Mock<HttpMessageHandler>();
			var response = new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(SuccessResponse),
			};

			handlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);

			var httpClient = new HttpClient(handlerMock.Object);

			_serviceCollection.AddSingleton(httpClient);

			var provider = _serviceCollection.BuildServiceProvider();

			var client = provider.GetService<OpenWeatherMapClient>();

			var result = await client.GetWeatherInfoByZip(Guid.NewGuid().ToString());

			Assert.That(result, Is.Not.Null);

			Assert.That(result.Name, Is.EqualTo("Moscow1"));
			const float delta = 0.000001f;
			Assert.That(result.Coord?.Lon, Is.EqualTo(28.3f).Within(delta));
			Assert.That(result.Coord?.Lat, Is.EqualTo(58.17f).Within(delta));
		}
	}
}