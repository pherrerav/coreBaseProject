using baseproject.data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;

namespace baseproject.data.Repositorio.perfil
{
    public class PerfilRepositorio : Repositorio<Perfil>, IPerfilRepositorio
    {
        private readonly MyDbContext Context;

        public PerfilRepositorio(MyDbContext dbContext)
        : base(dbContext)
        {
            this.Context = dbContext;
        }
        /**********************************************************************************
         EXTRAER LA LISTA DE ID DE PÁGINAS Y ID DE PERFILES
        **********************************************************************************/
        public ICollection BuscarPaginaPerfil(int id)
        {
            var query = Context.Pagina_Perfil
             .Where(x => x.PerfilId == id)
             .Select(x => new { x.PaginaId, x.PerfilId })
             .ToList() /// To get data from database
             .Select(x => new Pagina_Perfil()
             {
                 PaginaId = x.PaginaId,
                 PerfilId = x.PerfilId
             });
            return query.ToList();
        }
        /**********************************************************************************
         TRAER LA LISTA DE PERFILES 
        **********************************************************************************/
        public IEnumerable GetPerfiles()
        {
            var Perfiles = Context.Perfil.Select(x => new
            {
                id = x.Id,
                perfilNombre = x.PerfilNombre,

            }).ToList();

            return Perfiles.Select(x => new Perfil()
            {
                Id = x.id,
                PerfilNombre = x.perfilNombre
            }).ToList();
        }
        /**********************************************************************************
         AGREGAR UN NUEVO PERFIL A LA BASE DE DATOS ASÍ COMO LAS PÁGINAS A LAS QUE TENDRÁ 
         ACCESO.
        **********************************************************************************/
        public int GuardarPerfil(Perfil obj)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                int resultado = 1;
                try
                {
                    Context.Perfil.Add(obj);
                    Context.SaveChanges();

                    var Paginas_Perfiles = obj.Paginas_Perfiles;
                    Context.Pagina_Perfil.AddRange(Paginas_Perfiles);
                    Context.SaveChanges();
                    // Commit transaction if all commands succeed, transaction will auto-rollback
                    // when disposed if either commands fails
                    transaction.Commit();
                }
                catch (Exception)
                {
                    resultado = -1;
                }
                return resultado;
            }
        }
        /**********************************************************************************
         ELIMINAR LAS PAGINAS Y PERFILES DE LA BASE DE DATOS ANTES ANTES DE MODIFICAR
        **********************************************************************************/
        public int EliminarPaginaPerfil(int perfilId)
        {
            int resultado = 1;
            try
            {
                var perfiles = Context.Pagina_Perfil.Where(b => EF.Property<int>(b, "PerfilId") == perfilId);
                foreach (var perfil in perfiles)
                {
                    Context.Pagina_Perfil.Remove(perfil);
                }
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = -1;
            }
            return resultado;
        }
        /**********************************************************************************
         AGREGAR LAS PAGINAS SELECCIONADAS PARA EL ACCESO DE UN PERFIL
        **********************************************************************************/
        public int AgregarPaginaPerfil(Perfil obj)
        {
            int resultado = 1;
            try
            {
                var pagina_perfil = obj.Paginas_Perfiles;
                Context.Pagina_Perfil.AddRange(pagina_perfil);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = -1;
            }
            return resultado;
        }
        /**********************************************************************************
         MODIFICAR EL NOMBRE DE UN PERFIL EN LA BASE DE DATOS
        **********************************************************************************/
        public int ModificarPerfil(Perfil obj)
        {
            int resultado = 1;
            try
            {
                Context.Perfil.Update(obj);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                resultado = -1;
                string mensaje = e.Message.ToString();
            }
            return resultado;
        }
        public int ValidarAcceso(int pagina, int perfil)
        {
            var query = Context.Pagina_Perfil
             .Where(x => x.PerfilId == perfil && x.PaginaId == pagina)
             .Select(x => new { x.PaginaId, x.PerfilId })
             .Count();

            return query;
        }
        public bool IsDuplicate(string perfil)
        {
            return table.Any(x => x.PerfilNombre == perfil);
        }
    }
}
