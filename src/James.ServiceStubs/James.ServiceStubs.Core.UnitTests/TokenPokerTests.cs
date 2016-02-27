using System;
using System.Collections.Generic;

using FluentAssertions;
using Nancy;

using NUnit.Framework;

namespace James.ServiceStubs.Core.UnitTests
{
    [TestFixture]
    public class TokenPokerTests
    {
        [TestCase("http://myurl/{id}/{firstName}", "http://myurl/1/Todd")]
        [TestCase("http://myurl/{Id}/{FirstName}", "http://myurl/1/Todd")]
        [TestCase("http://myurl/{Address.state}/{address.City}/{firstname}", "http://myurl/TX/Frisco/Todd")]
        [TestCase("http://myurl.com/phones/mobile/{phones.Home}", "http://myurl.com/phones/mobile/972-712-2771")]
        public void given_value_with_tokens_and_existing_data_when_poking_should_return_poked_value(string valueWithTokens, string pokedValue)
        {
            var address = new {City = "Frisco", State = "TX", ZipCode = "75034"};
            var phones = new Dictionary<string, object> {{ "Home", "972-712-2771"}};

            var data = new Dictionary<string, object> {{"Id", 1}, {"FirstName", "Todd"}, {"LastName", "Meinershagen"}, {"Address", address}, {"Phones", phones} };

            var poker = new TokenPoker();
            var result = poker.PokeData(valueWithTokens, data);

            result.Should().Be(pokedValue);
        }
    }
}
