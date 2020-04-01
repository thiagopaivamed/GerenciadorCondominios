using GerenciadorCondominios.DAL.Interfaces;
using GerenciadorCondominios.DAL.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciadorCondominios.DAL
{
    public static class ConfiguracaoRepositoriosExtension
    {
        public static void ConfigurarRepositorios(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddTransient<IFuncaoRepositorio, FuncaoRepositorio>();
            services.AddTransient<IVeiculoRepositorio, VeiculoRepositorio>();
            services.AddTransient<IEventoRepositorio, EventoRepositorio>();
            services.AddTransient<IServicoRepositorio, ServicoRepositorio>();
            services.AddTransient<IServicoPredioRepositorio, ServicoPredioRepositorio>();
            services.AddTransient<IHistoricoRecursosRepositorio, HistoricoRecursosRepositorio>();
            services.AddTransient<IApartamentoRepositorio, ApartamentoRepositorio>();
            services.AddTransient<IMesRepositorio, MesRepositorio>();
            services.AddTransient<IAluguelRepositorio, AluguelRepositorio>();
            services.AddTransient<IPagamentoRepositorio, PagamentoRepositorio>();

        }
    }
}
