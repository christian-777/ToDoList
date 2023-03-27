using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    internal class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Person(string n)
        {
            this.Name = n;
            this.Id = Guid.NewGuid();
        }

        public Person(Guid i, string n)
        {
            this.Name = n;
            this.Id = i;
        }

        public void SetName(string n)
        {
            this.Name = n;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string ToFile()
        {
            return this.Id.ToString() + ";" + this.Name;
        }
    }
}
