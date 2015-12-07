using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace VeraCruzMobile.Business
{
	public class JsonHelper <T> where T : class
	{
		public static string ObjectToJson(T ObjectToConvert)
		{
			JsonSerializerSettings _jsonWriter = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			};

			return JsonConvert.SerializeObject(ObjectToConvert, _jsonWriter);
		}

		public static T JsonToObject(string jsonText)
		{
			var retorno = JsonConvert.DeserializeObject<T>(jsonText);

			return retorno;
		}
	}




}

