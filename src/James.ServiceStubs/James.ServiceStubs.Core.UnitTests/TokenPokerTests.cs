using System;
using System.Collections.Generic;

using FluentAssertions;

using James.ServiceStubs.Core.Rz;

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
        public void given_value_with_tokens_and_existing_data_when_poking_should_return_poked_value(string valueWithTokens, string pokedValue)
        {
            var address = new {City = "Frisco", State = "TX", ZipCode = "75034"};

            var data = new Dictionary<string, object> {{"Id", 1}, {"FirstName", "Todd"}, {"LastName", "Meinershagen"}, {"Address", address} };

            var poker = new TokenPoker();
            var result = poker.PokeData(valueWithTokens, data);

            result.Should().Be(pokedValue);
        }
    }
}
