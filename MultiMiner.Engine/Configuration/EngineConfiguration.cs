﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MultiMiner.Engine.Configuration
{
    public class EngineConfiguration
    {
        public EngineConfiguration()
        {
            DeviceConfigurations = new List<DeviceConfiguration>();
            CoinConfigurations = new List<CoinConfiguration>();
            MinerConfiguration = new MinerConfiguration();
        }

        public List<DeviceConfiguration> DeviceConfigurations { get; set; }
        public List<CoinConfiguration> CoinConfigurations { get; set; }
        public MinerConfiguration MinerConfiguration { get; set; }

        private static string AppDataPath()
        {
            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(rootPath, "MultiMiner");
        }

        private static string DeviceConfigurationsFileName()
        {
            return Path.Combine(AppDataPath(), "DeviceConfigurations.xml");
        }

        
        public void SaveDeviceConfigurations()
        {
            SaveConfiguration(DeviceConfigurations, DeviceConfigurationsFileName());
        }

        public void LoadDeviceConfigurations()
        {
            DeviceConfigurations = LoadConfiguration<List<DeviceConfiguration>>(DeviceConfigurationsFileName());
        }

        private static string CoinConfigurationsFileName()
        {
            return Path.Combine(AppDataPath(), "CoinConfigurations.xml");
        }

        public void LoadCoinConfigurations()
        {
            CoinConfigurations = LoadConfiguration<List<CoinConfiguration>>(CoinConfigurationsFileName());
        }

        public void SaveCoinConfigurations()
        {
            SaveConfiguration(CoinConfigurations, CoinConfigurationsFileName());
        }

        public static T LoadConfiguration<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StreamReader(fileName))
                    return (T)serializer.Deserialize(reader);
            }

            return default(T);
        }

        private static void SaveConfiguration(object source, string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            Type type = source.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, source);
            }
        }
    }
}
