// See https://aka.ms/new-console-template for more information

using inoaProjectx.Services;
using System.Threading.Tasks;
using inoaProjectx.Config;


namespace inoaProjectx
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                
                if(args.Length != 3)
                {
                    Console.WriteLine(" Erro: Uso incorreto!");
                    Console.WriteLine("Uso: inoaProjectx <nome_do_ativo> <preco_minimo> <preco_maximo>");
                    Console.WriteLine("\nExemplo: inoaProjectx PETR4 30.50 35.00");
                    return;
                }

               
                string nomeAtivo = args[0]?.Trim() ?? string.Empty;
                if(string.IsNullOrWhiteSpace(nomeAtivo))
                {
                    Console.WriteLine(" Erro: Nome do ativo não pode ser vazio!");
                    return;
                }

                // Validação do preço mínimo
                if(!double.TryParse(args[1], System.Globalization.NumberStyles.Float, 
                    System.Globalization.CultureInfo.InvariantCulture, out double precoMinimo))
                {
                    Console.WriteLine($" Erro: Preço mínimo inválido '{args[1]}'");
                    Console.WriteLine("Use formato decimal com ponto, exemplo: 30.50");
                    return;
                }

                // Validação do preço máximo
                if(!double.TryParse(args[2], System.Globalization.NumberStyles.Float, 
                    System.Globalization.CultureInfo.InvariantCulture, out double precoMaximo))
                {
                    Console.WriteLine($" Erro: Preço máximo inválido '{args[2]}'");
                    Console.WriteLine("Use formato decimal com ponto, exemplo: 35.00");

                    return;
                }

                
                if(precoMinimo <= 0)
                {
                    Console.WriteLine($" Erro: Preço mínimo deve ser maior que zero! (Valor informado: {precoMinimo})");
                    return;
                }

                if(precoMaximo <= 0)
                {
                    Console.WriteLine($" Erro: Preço máximo deve ser maior que zero! (Valor informado: {precoMaximo})");
                    return;
                }

                if(precoMinimo >= precoMaximo)
                {
                    Console.WriteLine($" Erro: Preço mínimo ({precoMinimo}) deve ser menor que o preço máximo ({precoMaximo})!");
                    return;
                }

                
                
                Console.WriteLine($" Monitorando ativo: {nomeAtivo.ToUpper()}");
                Console.WriteLine($" Preço mínimo: R$ {precoMinimo:F2}");
                Console.WriteLine($" Preço máximo: R$ {precoMaximo:F2}");
                
                
                Console.WriteLine();

               
                Console.WriteLine("Carregando configurações de email...");
                var emailSettings = Reader.ReadEmailSettings();
                Console.WriteLine("Configurações carregadas com sucesso!");
                Console.WriteLine();

                
                var monitoringSvc = new MonitoringSvc();
                Console.WriteLine("Iniciando \n");
                await monitoringSvc.MonitorarCotacao(nomeAtivo.ToUpper(), precoMinimo, precoMaximo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Erro fatal: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }


}


