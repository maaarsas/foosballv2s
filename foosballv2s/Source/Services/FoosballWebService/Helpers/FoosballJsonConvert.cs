using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace foosballv2s.Source.Services.FoosballWebService
{
    public static class FoosballJsonConvert
    {
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value,
                new JsonSerializerSettings
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        args.ErrorContext.Handled = true;
                    },
                }
            );
        }
    }
}