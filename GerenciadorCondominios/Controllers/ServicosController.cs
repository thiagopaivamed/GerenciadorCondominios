using GerenciadorCondominios.BLL.Models;
using GerenciadorCondominios.DAL.Interfaces;
using GerenciadorCondominios.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GerenciadorCondominios.Controllers
{
    [Authorize]
    public class ServicosController : Controller
    {
        private readonly IServicoRepositorio _servicoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IServicoPredioRepositorio _servicoPredioRepositorio;
        private readonly IHistoricoRecursosRepositorio _historicoRecursosRepositorio;

        public ServicosController(IServicoRepositorio servicoRepositorio, IUsuarioRepositorio usuarioRepositorio, IServicoPredioRepositorio servicoPredioRepositorio, IHistoricoRecursosRepositorio historicoRecursosRepositorio)
        {
            _servicoRepositorio = servicoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _servicoPredioRepositorio = servicoPredioRepositorio;
            _historicoRecursosRepositorio = historicoRecursosRepositorio;
        }

        // GET: Servicos
        public async Task<IActionResult> Index()
        {
            Usuario usuario = await _usuarioRepositorio.PegarUsuarioPeloNome(User);
            if (await _usuarioRepositorio.VerificarSeUsuarioEstaEmFuncao(usuario, "Morador"))
            {
                return View(await _servicoRepositorio.PegarServicosPeloUsuario(usuario.Id));
            }

            return View(await _servicoRepositorio.PegarTodos());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Usuario usuario = await _usuarioRepositorio.PegarUsuarioPeloNome(User);
            Servico servico = new Servico
            {
                UsuarioId = usuario.Id
            };

            return View(servico);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServicoId,Nome,Valor,Status,UsuarioId")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                servico.Status = StatusServico.Pendente;
                await _servicoRepositorio.Inserir(servico);
                TempData["NovoRegistro"] = $"Serviço {servico.Nome} solicitado";
                return RedirectToAction(nameof(Index));
            }

            return View(servico);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Servico servico = await _servicoRepositorio.PegarPeloId(id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicoId,Nome,Valor,Status,UsuarioId")] Servico servico)
        {
            if (id != servico.ServicoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _servicoRepositorio.Atualizar(servico);
                TempData["Atualizacao"] = $"Serviço {servico.Nome} atualizado";
                return RedirectToAction(nameof(Index));
            }
            return View(servico);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await _servicoRepositorio.Excluir(id);
            TempData["Exclusao"] = $"Serviço excluído";
            return Json("Serviço excluído");
        }

        [Authorize(Roles = "Administrador,Sindico")]
        [HttpGet]
        public async Task<IActionResult> AprovarServico(int id)
        {
            Servico servico = await _servicoRepositorio.PegarPeloId(id);
            ServicoAprovadoViewModel viewModel = new ServicoAprovadoViewModel
            {
                ServicoId = servico.ServicoId,
                Nome = servico.Nome
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Administrador,Sindico")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprovarServico(ServicoAprovadoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Servico servico = await _servicoRepositorio.PegarPeloId(viewModel.ServicoId);
                servico.Status = StatusServico.Aceito;
                await _servicoRepositorio.Atualizar(servico);

                ServicoPredio servicoPredio = new ServicoPredio
                {
                    ServicoId = viewModel.ServicoId,
                    DataExecucao = viewModel.Data
                };

                await _servicoPredioRepositorio.Inserir(servicoPredio);

                HistoricoRecursos hr = new HistoricoRecursos
                {
                    Valor = servico.Valor,
                    MesId = servicoPredio.DataExecucao.Month,
                    Dia = servicoPredio.DataExecucao.Day,
                    Ano = servicoPredio.DataExecucao.Year,
                    Tipo = Tipos.Saida
                };

                await _historicoRecursosRepositorio.Inserir(hr);
                TempData["NovoRegistro"] = $"{servico.Nome} escalado com sucesso";

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }


        [Authorize(Roles = "Administrador,Sindico")]
        public async Task<IActionResult> RecusarServico(int id)
        {
            Servico servico = await _servicoRepositorio.PegarPeloId(id);
            if (servico == null)
                return NotFound();

            servico.Status = StatusServico.Recusado;
            await _servicoRepositorio.Atualizar(servico);
            TempData["Exclusao"] = $"{servico.Nome} recusado";

            return RedirectToAction(nameof(Index));
        }

    }
}
