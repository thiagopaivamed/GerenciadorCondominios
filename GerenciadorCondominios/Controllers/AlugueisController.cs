using GerenciadorCondominios.BLL.Models;
using GerenciadorCondominios.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorCondominios.Controllers
{
    [Authorize(Roles = "Administrador,Sindico")]
    public class AlugueisController : Controller
    {
        private readonly IAluguelRepositorio _aluguelRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly IMesRepositorio _mesRepositorio;

        public AlugueisController(IAluguelRepositorio aluguelRepositorio, IUsuarioRepositorio usuarioRepositorio,
            IPagamentoRepositorio pagamentoRepositorio, IMesRepositorio mesRepositorio)
        {
            _aluguelRepositorio = aluguelRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _pagamentoRepositorio = pagamentoRepositorio;
            _mesRepositorio = mesRepositorio;
        }        
        
        public async Task<IActionResult> Index()
        {
            return View(await _aluguelRepositorio.PegarTodos());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["MesId"] = new SelectList(await _mesRepositorio.PegarTodos(), "MesId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AluguelId,Valor,MesId,Ano")] Aluguel aluguel)
        {
            if (ModelState.IsValid)
            {
                if (!_aluguelRepositorio.AluguelJaExiste(aluguel.MesId, aluguel.Ano))
                {
                    await _aluguelRepositorio.Inserir(aluguel);
                    IEnumerable<Usuario> usuarios = await _usuarioRepositorio.PegarTodos();
                    Pagamento pagamento;
                    List<Pagamento> pagamentos = new List<Pagamento>();

                    foreach (Usuario u in usuarios)
                    {
                        pagamento = new Pagamento
                        {
                            AluguelId = aluguel.AluguelId,
                            UsuarioId = u.Id,
                            DataPagamento = null,
                            Status = StatusPagamento.Pendente
                        };

                        pagamentos.Add(pagamento);
                    }

                    await _pagamentoRepositorio.Inserir(pagamentos);
                    TempData["NovoRegistro"] = $"Aluguel de valor {aluguel.Valor} do mês {aluguel.MesId} ano {aluguel.Ano} adicionado";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Aluguel já existe");
                    ViewData["MesId"] = new SelectList(await _mesRepositorio.PegarTodos(), "MesId", "Nome", aluguel.MesId);
                    return View(aluguel);
                }

            }
            ViewData["MesId"] = new SelectList(await _mesRepositorio.PegarTodos(), "MesId", "Nome", aluguel.MesId);
            return View(aluguel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Aluguel aluguel = await _aluguelRepositorio.PegarPeloId(id);
            if (aluguel == null)
            {
                return NotFound();
            }
            ViewData["MesId"] = new SelectList(await _mesRepositorio.PegarTodos(), "MesId", "Nome", aluguel.MesId);
            return View(aluguel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AluguelId,Valor,MesId,Ano")] Aluguel aluguel)
        {
            if (id != aluguel.AluguelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _aluguelRepositorio.Atualizar(aluguel);
                TempData["Atualizacao"] = $"Aluguel do mês {aluguel.MesId} ano {aluguel.Ano} atualizado";
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(await _mesRepositorio.PegarTodos(), "MesId", "Nome", aluguel.MesId);
            return View(aluguel);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await _aluguelRepositorio.Excluir(id);
            TempData["Exclusao"] = $"Aluguel excluído";
            return Json("Aluguel excluído");
        }
    }
}
