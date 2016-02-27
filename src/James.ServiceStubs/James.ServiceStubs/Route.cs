using System;
using System.Collections.Generic;

using Nancy;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace James.ServiceStubs
{
    public class Route
    {
        private static readonly object PadLock = new object();

        public RequestType Type { get; set; }
        public string Template { get; set; }
        public string Path { get; set; }

        [JsonConverter(typeof (SingleOrArrayConverter<int>))]
        public List<int> DelayInMilliseconds { get; set; } = new List<int>{ 0 };

        public DelayStrategy DelayStrategy { get; set; }

        public int CurrentDelayInMilliseconds => DelayStrategy == DelayStrategy.Random 
            ? GetDelayUsingRandom() 
            : GetDelayUsingRoundRobin();

        private int _currentDelayIndex = 0;
        private int GetDelayUsingRoundRobin()
        {
            lock (PadLock)
            {
                return DelayInMilliseconds[_currentDelayIndex++ % DelayInMilliseconds.Count];
            }
        }

        private int GetDelayUsingRandom()
        {
            return DelayInMilliseconds[GetRandomIndex(DelayInMilliseconds.Count)];
        }

        private int GetRandomIndex(int numberOfItems)
        {
            return Math.Abs(Guid.NewGuid().GetHashCode() % numberOfItems);
        }

        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
    }

    public enum DelayStrategy
    {
        RoundRobin = 0,
        Random = 1
    }

    class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.Type == JTokenType.Array 
                ? token.ToObject<List<T>>() 
                : new List<T> { token.ToObject<T>() };
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}