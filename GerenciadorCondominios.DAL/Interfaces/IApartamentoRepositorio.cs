using GerenciadorCondominios.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorCondominios.DAL.Interfaces
{
    public interface IApartamentoRepositorio : IRepositorioGenerico<Apartamento>
    {
        new Task<IEnumerable<Apartamento>> PegarTodos();

        Task<IEnumerable<Apartamento>> PegarApartamentoPeloUsuario(string usuarioId);
    }
}
