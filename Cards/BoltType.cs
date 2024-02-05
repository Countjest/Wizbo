using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using FSPRO;
using System.Collections.Generic;

namespace CountJest.Wizbo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoltType
    {
        Magic, Hex, Chaos
    }
}