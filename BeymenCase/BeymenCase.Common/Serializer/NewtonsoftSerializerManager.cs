using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeymenCase.Common.Serializer
{
	public class NewtonsoftSerializerManager : ISerializerService
	{

		public T DeserializeItem<T>(string serializedContent)
		{
			if (typeof(T) == typeof(string))
				return (T)((object)serializedContent);

			return JsonConvert.DeserializeObject<T>(serializedContent);
		}

		public string SerializeItem(object item)
		{
			return JsonConvert.SerializeObject(item);
		}


	}
}
