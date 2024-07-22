using System;
using System.Collections.Generic;
using System.Text;

namespace BaymenCase.ConfigurationReader.Interfaces
{
    public interface IConfigurationReader
    {
        T GetValue<T>(string key);
    }
}
