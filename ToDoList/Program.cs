using System;
using ToDoList;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> category = new List<string>(); 
        List<Person> person = new List<Person>();
        List<ToDo> todo = new List<ToDo>();
        List<ToDo> todoFinalized = new List<ToDo>();

        if (!File.Exists("Person.txt"))
        {
            person.Add(CreatePerson());
            WriteFilePerson(person);
        }
        else
        {
            todo = ReadFileToDo("ToDo.txt", false);
            todoFinalized = ReadFileToDo("ToDo.txt", true);
            person = ReadFilePerson("Person.txt");
            category = ReadFileCategory("Category.txt");
        }

        Menu(category, person, todo, todoFinalized);
    }

    private static void Menu(List<string> category, List<Person> person, List<ToDo> todo, List<ToDo> todoFinalized)
    {
        int choice;
        do
        {
            Console.WriteLine("Escolha uma opcao:");
            Console.WriteLine("1- Cadastrar pessoa");
            Console.WriteLine("2- Cadastrar categoria");
            Console.WriteLine("3- Cadastrar Tarefa");
            Console.WriteLine("4- Concluir tarefas");
            Console.WriteLine("5- Mostrar tarefas em andamento");
            Console.WriteLine("6- Mostrar tarefas concluidas");
            Console.WriteLine("7- Editar tarefas");
            Console.WriteLine("0- Sair");
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    person.Add(CreatePerson());
                    WriteFilePerson(person);
                    break;

                case 2:
                    category.Add(CreateCategory());
                    WriteFileCategory(category);
                    break;

                case 3:
                    todo.Add(CreateToDo(category, person));
                    WriteFileToDo(todo, todoFinalized);
                    break;

                case 4:
                    FinalizeToDo(todo, todoFinalized);
                    todo = ReadFileToDo("ToDo.txt", false);
                    todoFinalized = ReadFileToDo("ToDo.txt", true);
                    break;
            }
        } while (choice != 0);
    }

    private static List<string>? ReadFileCategory(string p)
    {
        List<string> l = new List<string>();
        try
        {
            StreamReader sr = new StreamReader(p);
            do
            {
                l.Add(sr.ReadLine());

            } while (!sr.EndOfStream);
            sr.Close();
        }
        catch
        {
            Console.WriteLine("Sem Categorias cadastradas");
        }
        return l;
    }

    private static List<Person>? ReadFilePerson(string p)
    {
        List<Person> l = new List<Person>();
        try
        {
            StreamReader sr = new StreamReader(p);
            do
            {
                var aux = sr.ReadLine().Split(";");

                var idPerson = Guid.Parse(aux[0]);
                var name = aux[1];
                Person person = new Person(idPerson, name);
                l.Add(person);

            } while (!sr.EndOfStream);
            sr.Close();
        }
        catch
        {
            Console.WriteLine("Sem Pessoas cadastradas");
        }
        return l;
    }

    private static List<ToDo>? ReadFileToDo(string p, bool s)
    {
        List<ToDo> l = new List<ToDo>();
        try
        {
            DateTime duedate;
            StreamReader sr = new StreamReader(p);
            do
            {
                var aux = sr.ReadLine().Split(";");
                if (Boolean.Parse(aux[5]) == s)
                {
                    var id = Guid.Parse(aux[0]);
                    var description = aux[1];
                    var category = aux[2];
                    var created = DateTime.Parse(aux[3]);
                    if (aux[4] != "")
                    {
                        duedate = DateTime.Parse(aux[4]);
                    }
                    else
                    {
                        duedate = created;
                    }

                    var status = Boolean.Parse(aux[5]);
                    var idPerson = Guid.Parse(aux[6]);
                    var name = aux[7];

                    var person = new Person(idPerson, name);

                    l.Add(new ToDo(id, description, category, created, duedate, status, person));
                }

            } while (!sr.EndOfStream);
            sr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Sua ToDo esta vazia");
        }
        return l;
    }

    private static void WriteFilePerson(List<Person> lp)
    {
        try
        {
            StreamWriter sw = new StreamWriter("Person.txt");
            foreach (Person person in lp)
            {
                sw.WriteLine(person.ToFile());
            }
            sw.Close();

        }
        catch
        {

        }
    }

    private static void WriteFileCategory(List<string> lc)
    {
        try
        {
            StreamWriter sw = new StreamWriter("Category.txt");
            foreach (var category in lc)
            {
                sw.WriteLine(category);
            }
            sw.Close();

        }
        catch
        {

        }
    }

    private static void WriteFileToDo(List<ToDo> lt, List<ToDo> ltf)
    {
        try
        {
            StreamWriter sw = new StreamWriter("ToDo.txt");
            foreach (var todo in lt)
            {
                sw.WriteLine(todo.ToFile());
            }
            foreach (var todo in ltf)
            {
                sw.WriteLine(todo.ToFile());
            }
            sw.Close();

        }
        catch
        {
        }
    }

    private static Person CreatePerson()
    {
        Console.WriteLine("Digite seu nome: ");
        var name = Console.ReadLine();

        return new Person(name);
    }

    private static string CreateCategory()
    {
        Console.WriteLine("Digite o nome da categoria: ");
        var category = Console.ReadLine();

        return category;
    }

    private static ToDo CreateToDo(List<string> category, List<Person> person)
    {
        Console.WriteLine("Qual a descricao da tarefa: ");
        var description = Console.ReadLine();

        ToDo todo = new ToDo(description);
        int cc = 0;
        int cp = 0;
        do
        {
            Console.WriteLine("deseja adicionar uma categoria? [s] - sim [n] - nao");
            var choice = char.Parse(Console.ReadLine());

            if (choice == 's')
            {
                int i = 1;
                Console.WriteLine("Escolha uma categoria: ");
                foreach (string s in category)
                {
                    Console.WriteLine(i + "- " + s);
                    i++;
                }
                Console.Write(": ");
                cc = int.Parse(Console.ReadLine());
                todo.SetCategory(category[cc - 1]);
                break;
            }
            else
            {
                break;
            }
        } while (true);

        do
        {
            Console.WriteLine("deseja adicionar uma data de termino? [s] - sim [n] - nao");
            var choice = char.Parse(Console.ReadLine());

            if (choice == 's')
            {
                int i = 1;
                Console.WriteLine("Qual dia: ");
                var day = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual mes: ");
                var month = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual ano: ");
                var year = int.Parse(Console.ReadLine());

                todo.SetDueDate(DateTime.Parse(month + "/" + day + "/" + year));
                break;
            }
            else
            {
                break;
            }
        } while (true);

        do
        {
            Console.WriteLine("deseja adicionar um responsavel pela tarefa? [s] - sim [n] - nao");
            var choice = char.Parse(Console.ReadLine());

            if (choice == 's')
            {
                int i = 1;
                Console.WriteLine("Escolha um responsavel: ");
                foreach (var p in person)
                {
                    Console.WriteLine(i + "- " + p);
                    i++;
                }
                Console.Write(": ");
                cp = int.Parse(Console.ReadLine());
                todo.SetPerson(person[(cp - 1)]);
                break;
            }
            else
            {
                todo.SetPerson(person[0]);
                break;
            }
        } while (true);

        return todo;
    }

    private static void FinalizeToDo(List<ToDo> todo, List<ToDo> todoFinalized)
    {

        Console.WriteLine("Finalize suas tarefas aqui: ");
        foreach (ToDo item in todo)
        {
            Console.WriteLine(item.ToString());
            Console.WriteLine("Deseja finalizar essa tarefa? [s] - sim [n] - nao");
            var choice = char.Parse(Console.ReadLine());

            if (choice == 's')
            {
                item.SetStatus();
            }
        }
        WriteFileToDo(todo, todoFinalized);
    }
}
