﻿namespace twitter_emitter_server.Data {

    // This ConfigurationHelper class and method GetByName from https://code.4noobz.net/read-settings-from-the-appsettings-json/
    public static class ConfigurationHelper {
        public static string GetByName(string configKeyName) {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IConfigurationSection section = config.GetSection(configKeyName);

            return section.Value;
        }
    }
}
