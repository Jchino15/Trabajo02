using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trabajo02.Models;
using Microsoft.EntityFrameworkCore;

namespace Trabajo02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext equiposContext;

        public equiposController(equiposContext equiposContext)
        {
            this.equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("Getall")]
        public IActionResult Get ()
        {
            List<equipos> listadoEquipo = (from e in equiposContext.equipos select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get (int id)
        {
            equipos? equipo = (from e in equiposContext.equipos where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindBdDescripcion (string filtro)
        {
            equipos? equipo = (from e in equiposContext.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if(equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEquipo([FromBody] equipos equipo)
        {
            try
            {
                equiposContext.equipos.Add(equipo);
                equiposContext.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoActual = (from e in equiposContext.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            if(equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            equiposContext.Entry(equipoActual).State = EntityState.Modified;
            equiposContext.SaveChanges();

            return Ok(equipoModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            equipos? equipo = (from e in equiposContext.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if(equipo == null)
            {
                return NotFound();
            }

            equiposContext.equipos.Attach(equipo);
            equiposContext.equipos.Remove(equipo);
            equiposContext.SaveChanges();

            return Ok(equipo);
        }

        
    }
}
