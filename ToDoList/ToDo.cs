using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    internal class ToDo
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string? Category { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Duedate { get; set; }
        public bool Status { get; set; }
        public Person Owner { get; set; }


        public ToDo(string d)
        {
            this.Description = d;
            this.Id = Guid.NewGuid();
            this.Created = DateTime.Now;
            this.Status = false;
        }

        public ToDo(Guid id, string description, string? category, DateTime created, DateTime? duedate, bool status, Person owner)
        {
            Id = id;
            Description = description;
            Category = category;
            Created = created;
            Duedate = duedate;
            Status = status;
            Owner = owner;
        }

        public override string ToString()
        {
            var txt = "\nO que tem pra fazer: " + this.Description + "\nDa categoria: " + this.Category + "\nFoi criada: " + this.Created + "\nPrevisao de termino: " + this.Duedate + "\n";
            if (this.Status)
            {
                txt += "Status: Finalizado";
            }
            else
            {
                txt += "Status: Em andamento";
            }
            txt += "\nPessoa respnsavel: " + this.Owner.Name+"\n";
            return txt;
        }

        public string ToFile()
        {
            return this.Id + ";" + this.Description + ";" + this.Category + ";" + this.Created.ToString() + ";" + this.Duedate.ToString() + ";" + this.Status + ";" + this.Owner.ToFile();
        }

        public void SetStatus()
        {
            this.Status = !this.Status;
        }

        public void SetPerson(Person o)
        {
            this.Owner = o;
        }

        public void SetDescription(string d)
        {
            this.Description = d;
        }

        public void SetCategory(string c)
        {
            this.Category = c;
        }

        public void SetDueDate(DateTime dd)
        {
            this.Duedate = dd;
        }
    }
}
