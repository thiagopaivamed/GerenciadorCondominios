using Microsoft.AspNetCore.Identity;

namespace GerenciadorCondominios.BLL.Models
{
    public class Funcao : IdentityRole<string>
    {
        public string Descricao { get; set; }
    }
}
