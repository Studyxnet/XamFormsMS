using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace VeraCruzMobile.Business
{
	public class WebServiceBase <T> where T : class
	{
		private static string BaseUrl = "http://developers.agenciaideias.com.br/tempo/json/{0}-{1}";


		public WebServiceBase ()
		{
		}


		public static async Task<T> RequestAsync (string cidade, string uf, RequestType requestType = RequestType.Get, T SendObject = null, 
		                                          int triesNumber = 0, string contentType = "application/json")
		{

			T tReturn = null;

			for (int i = 0; i <= triesNumber; i++) {
				try {

					using (HttpClient client = new HttpClient ()) {

						HttpResponseMessage response = new HttpResponseMessage ();

						var requestBody = JsonHelper<T>.ObjectToJson (SendObject);

						client.BaseAddress = new Uri (BaseUrl);

						HttpContent httpContent = new StringContent (requestBody, Encoding.UTF8, contentType);


						response = await client.GetAsync (String.Format (BaseUrl, cidade, uf));
						break;
						

						if (response.IsSuccessStatusCode) {
							string responseString = await response.Content.ReadAsStringAsync ();

							var objetoRetorno = JsonHelper<T>.JsonToObject (responseString);


							tReturn = objetoRetorno;

						} else {
							if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout) {
								if (i == triesNumber)
									throw  new Exception ("Tempo limite atingido");
							} else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
								throw  new Exception ("Serviço não encontrado");
							else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
								if (i == triesNumber)
									throw new Exception ("Serviço indisponível no momento");
							} else
								throw new Exception ("Falha no acesso!");
						}
					}

				} catch (JsonException x) {
					throw x;
				} catch (Exception x) {
					throw x;
				}
			}

			return tReturn;

		}

		public static async Task<T> RequestAsync (string URL, RequestType requestType = RequestType.Get, KeyValuePair<string,string>[] SendObject = null, int triesNumber = 0)
		{

			T tReturn = null;

			for (int i = 0; i <= triesNumber; i++) {
				try {

					using (HttpClient client = new HttpClient ()) {

						HttpResponseMessage response = new HttpResponseMessage ();

						client.BaseAddress = new Uri (BaseUrl);

						HttpContent httpContent = new FormUrlEncodedContent (SendObject);

						switch (requestType) {
						case RequestType.Get:
							response = await client.GetAsync (URL);
							break;
						case RequestType.Post:
							response = await client.PostAsync (URL, httpContent);
							break;
						case RequestType.Put:
							response = await client.PutAsync (URL, httpContent);
							break;
						case RequestType.Delete:
							response = await client.DeleteAsync (URL);
							break;
						default:
							break;
						}

						if (response.IsSuccessStatusCode) {
							string responseString = await response.Content.ReadAsStringAsync ();

							var objetoRetorno = JsonHelper<T>.JsonToObject (responseString);


							tReturn = objetoRetorno;

						} else {
							if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout) {
								if (i == triesNumber)
									throw  new Exception ("Tempo limite atingido");
							} else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
								throw  new Exception ("Serviço não encontrado");
							else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) {
								if (i == triesNumber)
									throw new Exception ("Serviço indisponível no momento");
							} else
								throw new Exception ("Falha no acesso!");
						}
					}

				} catch (JsonException x) {
					throw x;
				} catch (Exception x) {
					throw x;
				}
			}

			return tReturn;

		}
	}

	public enum RequestType
	{
		Get,
		Post,
		Put,
		Delete
	}

}

