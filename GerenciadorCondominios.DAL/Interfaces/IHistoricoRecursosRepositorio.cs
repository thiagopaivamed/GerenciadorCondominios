using GerenciadorCondominios.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorCondominios.DAL.Interfaces
{
    public interface IHistoricoRecursosRepositorio : IRepositorioGenerico<HistoricoRecursos>
    {
        object PegarHistoricoGanhos(int ano);
        object PegarHistoricoDespesas(int ano);

        public Task<decimal> PegarSomaDespesas(int ano);
        public Task<decimal> PegarSomaGanhos(int ano);

        public Task<IEnumerable<HistoricoRecursos>> PegarUltimasMovimentacoes();
    }
}
