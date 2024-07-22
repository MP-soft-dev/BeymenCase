using System;
using System.Collections.Generic;
using System.Text;

namespace BeymenCase.Common.Serializer
{
	public interface ISerializerService
	{
		string SerializeItem(object item);
		T DeserializeItem<T>(string serializedContent);
	}
}
