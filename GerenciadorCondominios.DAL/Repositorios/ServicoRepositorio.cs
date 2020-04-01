using GerenciadorCondominios.BLL.Models;
using GerenciadorCondominios.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.DAL.Repositorios
{
    public class ServicoRepositorio : RepositorioGenerico<Servico>, IServicoRepositorio
    {
        private readonly Contexto _contexto;
        public ServicoRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Servico>> PegarServicosPeloUsuario(string usuarioId)
        {
            try
            {
                return await _contexto.Servicos.Where(s => s.UsuarioId == usuarioId).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
