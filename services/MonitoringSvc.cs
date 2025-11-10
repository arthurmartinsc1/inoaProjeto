using inoaProjectx.Services;
using inoaProjectx.Models;
using System.Threading.Tasks;
using inoaProjectx.Config;

namespace inoaProjectx.Services
{
    public class MonitoringSvc
    {
        private readonly CotacaoSvc cotacaoService = new CotacaoSvc();
        private readonly EmailSvc emailSvc = new EmailSvc(Reader.ReadEmailSettings());
         bool _alertaVendaEnviado = false;
         bool _alertaCompraEnviado = false;
    
        

        public async Task MonitorarCotacao(string symbol, double precoMinimo, double precoMaximo)
        {
            while (true)
            {
                try
                {
                    if (_alertaCompraEnviado || _alertaVendaEnviado)
                    {
                        Console.WriteLine($"Buscando cotação do ativo {symbol} novamente...");
                        
                    }
                    else
                    {
                        Console.WriteLine($"Buscando cotação do ativo {symbol}...");

                    }


                    var cotacao = await cotacaoService.GetCotacaoAsync(symbol);
                    Console.WriteLine($"Cotação do ativo {symbol}: {cotacao.Price}");
                    
                    
                    if (cotacao.Price >= precoMinimo && cotacao.Price <= precoMaximo)
                    {
                        if (_alertaCompraEnviado || _alertaVendaEnviado)
                        {
                            Console.WriteLine($" Preço normalizado: {cotacao.Price}");
                            _alertaCompraEnviado = false;
                            _alertaVendaEnviado = false;
                        }
                    }
                    
                    else if (cotacao.Price < precoMinimo && !_alertaCompraEnviado)
                    {
                        Console.WriteLine($" Alerta de compra: {cotacao.Price} < {precoMinimo}");
                        
                        try
                        {
                            await emailSvc.SendEmail(
                                $" Alerta de Compra - {symbol}", 
                                $"O ativo {symbol} está com preço abaixo do mínimo!\n\n" +
                                $"Preço Atual: R$ {cotacao.Price:F2}\n" +
                                $"Preço Mínimo: R$ {precoMinimo:F2}\n" +
                                $"Data/Hora: {cotacao.Date}\n\n" +
                                $" Considere comprar!"
                            );
                            _alertaCompraEnviado = true;
                        }
                        catch (Exception emailEx)
                        {
                            Console.WriteLine($" Falha ao enviar email de compra: {emailEx.Message}");
                        }
                    }
                    // Verifica alerta de venda
                    else if (cotacao.Price > precoMaximo && !_alertaVendaEnviado)
                    {
                        Console.WriteLine($"Alerta de venda: {cotacao.Price} > {precoMaximo}");
                        
                        try
                        {
                            await emailSvc.SendEmail(
                                $" Alerta de Venda - {symbol}", 
                                $"O ativo {symbol} está com preço acima do máximo!\n\n" +
                                $"Preço Atual: R$ {cotacao.Price:F2}\n" +
                                $"Preço Máximo: R$ {precoMaximo:F2}\n" +
                                $"Data-Hora: {cotacao.Date}\n\n" +
                                $" Considere vender!"
                            );
                            _alertaVendaEnviado = true;
                        }
                        catch (Exception emailEx)
                        {
                            Console.WriteLine($"Falha ao enviar email de venda: {emailEx.Message}");
                        }
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($" Erro de rede ao buscar cotação: {httpEx.Message}");
                    Console.WriteLine("Tentando novamente em 50 segundos...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Erro inesperado ao buscar cotação: {ex.Message}");
                    Console.WriteLine("Tentando novamente em 50 segundos...");
                }
                
                await Task.Delay(50000); // 50 segundos de delay
            }
        }
    }
}
