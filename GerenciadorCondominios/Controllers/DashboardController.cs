using GerenciadorCondominios.DAL.Interfaces;
using GerenciadorCondominios.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace GerenciadorCondominios.Controllers
{
    [Authorize(Roles = "Administrador,Sindico")]
    public class DashboardController : Controller
    {
        private readonly IAluguelRepositorio _aluguelRepositorio;
        private readonly IHistoricoRecursosRepositorio _historicoRecursosRepositorio;

        public DashboardController(IAluguelRepositorio aluguelRepositorio, IHistoricoRecursosRepositorio historicoRecursosRepositorio)
        {
            _aluguelRepositorio = aluguelRepositorio;
            _historicoRecursosRepositorio = historicoRecursosRepositorio;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Anos"] = new SelectList(await _aluguelRepositorio.PegarTodosAnos());
            return View();
        }

        public JsonResult DadosGraficoGanhos(int ano)
        {
            return Json(_historicoRecursosRepositorio.PegarHistoricoGanhos(ano));
        }

        public JsonResult DadosGraficoDespesas(int ano)
        {
            return Json(_historicoRecursosRepositorio.PegarHistoricoDespesas(ano));
        }

        public async Task<JsonResult> DadosGraficoDespesasGanhosTotais()
        {
            int ano = DateTime.Now.Year;
            GanhosDespesasViewModel model = new GanhosDespesasViewModel
            {
                Despesas = await _historicoRecursosRepositorio.PegarSomaDespesas(ano),
                Ganhos = await _historicoRecursosRepositorio.PegarSomaGanhos(ano)
            };

            return Json(model);

        }
    }
}