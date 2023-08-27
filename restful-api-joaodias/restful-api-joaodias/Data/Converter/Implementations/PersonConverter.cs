﻿using restful_api_joaodias.Data.Converter.Contract;
using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Model;

namespace restful_api_joaodias.Data.Converter.Implementations
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        public Person Parse(PersonVO origin)
        {
            if (origin == null)
            {
                return null;
            }
            return new Person
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender
            };
        }

        public List<Person> Parse(List<PersonVO> origin)
        {
            if (origin == null)
            {
                return null;
            }
            return origin.Select(item => Parse(item)).ToList();
        }

        public PersonVO Parse(Person origin)
        {
            if (origin == null)
            {
                return null;
            }
            return new PersonVO
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender
            };
        }

        public List<PersonVO> Parse(List<Person> origin)
        {
            if (origin == null)
            {
                return null;
            }
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
