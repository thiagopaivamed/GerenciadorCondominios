using System;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorCondominios.ViewModels
{
    public class ServicoAprovadoViewModel
    {
        public int ServicoId { get; set; }

        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Data de execução")]
        public DateTime Data { get; set; }
    }
}
