using EduCrew.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace EduCrew.Controllers.API
{
    public class PostAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PostAPI
        public IQueryable<Post> GetPosts()
        {
            return  db.Posts;
        }

        // GET: api/PostAPI/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(int id)
        {
            Post hospital = db.Posts.Find(id);
            if (hospital == null)
            {
                return NotFound();
            }

            return Ok(hospital);
        }

        // PUT: api/HospitalAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(int id, Post hospital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hospital.PostId)
            {
                return BadRequest();
            }

            db.Entry(hospital).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HospitalExists(id))
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

        // POST: api/HospitalAPI
        [ResponseType(typeof(Post))]
        public IHttpActionResult PostHospital(Post hospital)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(hospital);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hospital.PostId }, hospital);
        }

        // DELETE: api/HospitalAPI/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeleteHospital(int id)
        {
            Post hospital = db.Posts.Find(id);
            if (hospital == null)
            {
                return NotFound();
            }

            db.Posts.Remove(hospital);
            db.SaveChanges();

            return Ok(hospital);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HospitalExists(int id)
        {
            return db.Posts.Count(e => e.PostId == id) > 0;
        }

    }
}
