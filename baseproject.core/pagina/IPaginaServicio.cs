using baseproject.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace proyectomvc.core.pagina
{
    public interface IPaginaServicio
    {
        IEnumerable<Pagina> GetPaginas();
    }
}
