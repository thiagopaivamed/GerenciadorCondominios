using GerenciadorCondominios.BLL.Models;
using GerenciadorCondominios.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorCondominios.DAL.Repositorios
{
    public class HistoricoRecursosRepositorio : RepositorioGenerico<HistoricoRecursos>, IHistoricoRecursosRepositorio
    {
        private readonly Contexto _contexto;
        public HistoricoRecursosRepositorio(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public object PegarHistoricoDespesas(int ano)
        {
            try
            {
                return _contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipos.Saida).ToList()
                    .OrderBy(hr => hr.MesId).GroupBy(hr => hr.Mes.Nome)
                    .Select(hr => new { Meses = hr.Key, Valores = hr.Sum(v => v.Valor) });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public object PegarHistoricoGanhos(int ano)
        {
            try
            {
                return _contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipos.Entrada).ToList()
                    .OrderBy(hr => hr.MesId).GroupBy(hr => hr.Mes.Nome)
                    .Select(hr => new { Meses = hr.Key, Valores = hr.Sum(v => v.Valor) });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<decimal> PegarSomaDespesas(int ano)
        {
            try
            {
                return await _contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipos.Saida)
                    .SumAsync(hr => hr.Valor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<decimal> PegarSomaGanhos(int ano)
        {
            try
            {
                return await _contexto.HistoricoRecursos.Include(hr => hr.Mes)
                    .Where(hr => hr.Ano == ano && hr.Tipo == Tipos.Entrada)
                    .SumAsync(hr => hr.Valor);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<HistoricoRecursos>> PegarUltimasMovimentacoes()
        {
            try
            {
                return await _contexto.HistoricoRecursos.Include(hr => hr.Mes).OrderByDescending(hr => hr.HistoricoRecursosId)
                    .Take(5).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
