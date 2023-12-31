﻿using restful_api_joaodias.Model;
using restful_api_joaodias.Model.Context;
using restful_api_joaodias.Repository.Generic;

namespace restful_api_joaodias.Repository.PersonRepo
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(MySQLContext context) : base(context)
        {
        }

        public Person Disable(long id)
        {
            if (!_context.Persons.Any(p => p.Id.Equals(id)))
            {
                return null;
            }

            var user = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));

            if (user != null)
            {
                user.Enabled = false;
                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return user;
        }

        public List<Person>? FindByName(string? firstName, string? lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons
                 .Where(
                     p => p.FirstName.ToLower().Contains(firstName.ToLower()) &&
                         p.LastName.ToLower().Contains(lastName.ToLower()))
                 .ToList();
            }
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons
                 .Where(
                        p => p.LastName.ToLower().Contains(lastName.ToLower()))
                 .ToList();
            }
            else if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                return _context.Persons
                 .Where(
                     p => p.FirstName.ToLower().Contains(firstName.ToLower()))
                 .ToList();
            }
            return null;
        }
    }
}
