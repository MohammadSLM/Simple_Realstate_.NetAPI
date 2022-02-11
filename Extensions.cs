using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi1
{
    public static class Extensions
    {
        public static string GetDomainWithScheme(this HttpRequest request)
        {
            string scheme = $"{request.Scheme}://";
            string domain = request.Host.Value;

            return scheme + domain;
        }

        public class SettingsConfigHelper
        {
            private static SettingsConfigHelper _appSettings;

            public string appSettingValue { get; set; }

            public static string AppSetting(string Key)
            {
                _appSettings = GetCurrentSettings(Key);
                return _appSettings.appSettingValue;
            }

            public SettingsConfigHelper(IConfiguration config, string Key)
            {
                this.appSettingValue = config.GetValue<string>(Key);
            }

            // Get a valued stored in the appsettings.
            // Pass in a key like TestArea:TestKey to get TestValue
            public static SettingsConfigHelper GetCurrentSettings(string Key)
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var settings = new SettingsConfigHelper(configuration.GetSection("BaseConfiguration"), Key);

                return settings;
            }
        }

        public static class BaseConfiguration
        {
            public static string GetBaseUrl()
            {
                return SettingsConfigHelper.AppSetting("BaseUrl");
            }
        }
    }
}
