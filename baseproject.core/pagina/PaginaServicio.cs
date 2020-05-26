using baseproject.data.Models;
using baseproject.data.Repositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace proyectomvc.core.pagina
{
    public class PaginaServicio : IPaginaServicio
    {
        private readonly IRepositorio<Pagina> repositorio;
        public PaginaServicio(IRepositorio<Pagina> repositorio)
        {
            this.repositorio = repositorio;
        }
        public IEnumerable<Pagina> GetPaginas()
        {
            return repositorio.GetAll();
        }
    }
}
