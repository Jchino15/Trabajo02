using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trabajo02.Models;
using Microsoft.EntityFrameworkCore;

namespace Trabajo02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carreras : ControllerBase
    {
        private readonly equiposContext equiposContext;
        public carreras(equiposContext equiposContext)
        {
            this.equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("Getall")]
        public IActionResult Get()
        {
            var listadocarreras = (from e in equiposContext.carreras
                                 join m in equiposContext.facultades on e.facultad_id equals m.facultad_id
                                 select new
                                 {
                                     e.nombre_carrera,
                                     e.carrera_id,
                                     e.facultad_id,
                                     m.nombre_facultad
                                 }).ToList();

            if (listadocarreras.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadocarreras);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            var carreras = (from e in equiposContext.carreras
                          join m in equiposContext.facultades on e.facultad_id equals m.facultad_id
                          join te in equiposContext.carreras on e.carrera_id equals te.carrera_id
                          where e.carrera_id == id
                          select new
                          {
                              e.nombre_carrera,
                              e.carrera_id,
                              e.facultad_id,
                              m.nombre_facultad,
                              carreras = ("Marca " + m.nombre_facultad + "tipo " + te.carrera_id)
                          }
                          ).FirstOrDefault();

            if (carreras == null)
            {
                return NotFound();
            }

            return Ok(carreras);
        }

        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindBdDescripcion(string filtro)
        {
            equipos? equipo = (from e in equiposContext.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
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
            var carreraActual = (from e in equiposContext.carreras
                                join m in equiposContext.facultades on e.facultad_id equals m.facultad_id
                                select e).FirstOrDefault();

            if (carreraActual == null)
            {
                return NotFound();
            }

            carreraActual.nombre_carrera = equipoModificar.descripcion;
            carreraActual.carrera_id = equipoModificar.marca_id;

            equiposContext.Entry(carreraActual).State = EntityState.Modified;
            equiposContext.SaveChanges();

            return Ok(equipoModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            var equipo = (from e in equiposContext.equipos
                          join m in equiposContext.marcas on e.marca_id equals m.id_marcas
                          select e).FirstOrDefault();

            if (equipo == null)
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
