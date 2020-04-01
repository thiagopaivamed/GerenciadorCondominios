using GerenciadorCondominios.BLL.Models;
using GerenciadorCondominios.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.DAL.Repositorios
{
    public class ApartamentoRepositorio : RepositorioGenerico<Apartamento>, IApartamentoRepositorio
    {
        private readonly Contexto _contexto;
        public ApartamentoRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Apartamento>> PegarApartamentoPeloUsuario(string usuarioId)
        {
            try
            {
                return await _contexto.Apartamentos
                    .Include(a => a.Morador).Include(a => a.Proprietario)
                    .Where(a => a.MoradorId == usuarioId || a.ProprietarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public new async Task<IEnumerable<Apartamento>> PegarTodos()
        {
            try
            {
                return await _contexto.Apartamentos.Include(a => a.Morador).Include(a => a.Proprietario).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
