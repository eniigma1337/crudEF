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
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class WeaponDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/WeaponData/ListWeapons
        [HttpGet]
        public IEnumerable<WeaponDto> ListWeapons()
        {
            List<Weapon> Weapons = db.Weapons.ToList();
            List<WeaponDto> WeaponDtos = new List<WeaponDto>();

            Weapons.ForEach(a => WeaponDtos.Add(new WeaponDto()
            {
                WeaponID = a.WeaponID,
                WeaponName = a.WeaponName,
                WeaponCreationYear = a.WeaponCreationYear,
                WeaponDescription = a.WeaponDescription,
                CollectionsID = a.Collections.CollectionsID,
                CollectionsName = a.Collections.CollectionsName,
                CollectionsStandard = a.Collections.CollectionsStandard
            }));
            return WeaponDtos;
        }

        /// <summary>
        /// Gathers information about all weapons related to a particular collections ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all weapons in the database, including their associated collections matched with a particular collections ID
        /// </returns>
        /// <param name="id">Collections ID.</param>
        /// <example>
        /// GET: api/WeaponData/ListWeaponsForCollections/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(WeaponDto))]
        public IHttpActionResult ListWeaponsForCollections(int id)
        {
            List<Weapon> Weapons = db.Weapons.Where(a => a.CollectionsID == id).ToList();
            List<WeaponDto> WeaponDtos = new List<WeaponDto>();

            Weapons.ForEach(a => WeaponDtos.Add(new WeaponDto()
            {
                WeaponID = a.WeaponID,
                WeaponName = a.WeaponName,
                WeaponCreationYear = a.WeaponCreationYear,
                WeaponDescription = a.WeaponDescription,
                CollectionsID = a.Collections.CollectionsID,
                CollectionsName = a.Collections.CollectionsName,
                CollectionsStandard = a.Collections.CollectionsStandard
            }));

            return Ok(WeaponDtos);
        }

        // GET: api/WeaponData/FindWeapon/5
        [ResponseType(typeof(Weapon))]
        [HttpGet]
        public IHttpActionResult FindWeapon(int id)
        {
            Weapon Weapon = db.Weapons.Find(id);
            WeaponDto WeaponDto = new WeaponDto()
            {
                WeaponID = Weapon.WeaponID,
                WeaponName = Weapon.WeaponName,
                WeaponCreationYear = Weapon.WeaponCreationYear,
                WeaponDescription = Weapon.WeaponDescription,
                CollectionsID = Weapon.Collections.CollectionsID,
                CollectionsName = Weapon.Collections.CollectionsName,
                CollectionsStandard = Weapon.Collections.CollectionsStandard
            };
            if (Weapon == null)
            {
                return NotFound();
            }

            return Ok(WeaponDto);
        }

        // Post: api/WeaponData/UpdateWeapon/5
        [ResponseType(typeof(void))]
        [HttpPost]

        public IHttpActionResult UpdateWeapon(int id, Weapon weapon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weapon.WeaponID)
            {
                return BadRequest();
            }

            db.Entry(weapon).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeaponExists(id))
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

        // POST: api/WeaponData/AddWeapon
        [HttpPost]
        [ResponseType(typeof(Weapon))]
        public IHttpActionResult AddWeapon(Weapon weapon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Weapons.Add(weapon);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = weapon.WeaponID }, weapon);
        }

        /// <summary>
        /// Deletes an weapon from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the weapon</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/WeaponData/DeleteWeapon/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Weapon))]
        [HttpPost]
        public IHttpActionResult DeleteWeapon(int id)
        {
            Weapon weapon = db.Weapons.Find(id);
            if (weapon == null)
            {
                return NotFound();
            }

            db.Weapons.Remove(weapon);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WeaponExists(int id)
        {
            return db.Weapons.Count(e => e.WeaponID == id) > 0;
        }
    }
}