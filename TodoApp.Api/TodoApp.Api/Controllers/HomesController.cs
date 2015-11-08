using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TodoApp.Api.Db;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    public class HomesController : ApiController
    {
        private TodoDbContext db = new TodoDbContext();

        // GET: api/Homes
        public IQueryable<Home> GetHomes()
        {
            return db.Homes;
        }

        // GET: api/Homes/5
        [ResponseType(typeof(Home))]
        public IHttpActionResult GetHome(int id)
        {
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return NotFound();
            }

            return Ok(home);
        }

        // PUT: api/Homes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHome(int id, Home home)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != home.HomeId)
            {
                return BadRequest();
            }

            db.Entry(home).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Homes
        [ResponseType(typeof(Home))]
        public IHttpActionResult PostHome(Home home)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Homes.Add(home);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = home.HomeId }, home);
        }

        // DELETE: api/Homes/5
        [ResponseType(typeof(Home))]
        public IHttpActionResult DeleteHome(int id)
        {
            Home home = db.Homes.Find(id);
            if (home == null)
            {
                return NotFound();
            }

            db.Homes.Remove(home);
            db.SaveChanges();

            return Ok(home);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HomeExists(int id)
        {
            return db.Homes.Count(e => e.HomeId == id) > 0;
        }
    }
}