using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using inoaProjectx.Models;
using Microsoft.Extensions.Configuration.Json;

namespace inoaProjectx.Config
{
    public class Reader
    {
        public static EmailSettings ReadEmailSettings()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var config = new EmailSettings();
                configuration.GetSection("Email").Bind(config);

                if (string.IsNullOrEmpty(config.Usuario) || string.IsNullOrEmpty(config.Senha))
                {
                        throw new Exception("Configurações de email incompletas no appsettings.json");
                }

                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Erro ao ler configurações: {ex.Message}");
                throw;
            }
        }
    }
}