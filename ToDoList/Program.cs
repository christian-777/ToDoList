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

        if (!File.Exists("Person.txt") && !File.Exists("Category.txt"))
        {
            person.Add(CreatePerson());
            WriteFilePerson(person);

            category.Add(CreateCategory());
            WriteFileCategory(category);
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
            Console.WriteLine("5- Reativar tarefas");
            Console.WriteLine("6- Mostrar tarefas em andamento");
            Console.WriteLine("7- Mostrar tarefas concluidas");
            Console.WriteLine("8- Editar tarefas");
            Console.WriteLine("0- Sair");
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Digite um numero");
                Console.ReadKey();
                continue;
            }

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Saindo...");
                    Thread.Sleep(2000);
                    break;
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
                    ChangeToDoStatus(todo, todoFinalized);
                    todo = ReadFileToDo("ToDo.txt", false);
                    todoFinalized = ReadFileToDo("ToDo.txt", true);
                    break;
                case 5:
                    ChangeToDoStatus(todoFinalized, todo);
                    todo = ReadFileToDo("ToDo.txt", false);
                    todoFinalized = ReadFileToDo("ToDo.txt", true);
                    break;
                case 6:
                    Console.WriteLine(PrintToDo(todo));
                    break;
                case 7:
                    Console.WriteLine(PrintToDo(todoFinalized));
                    break;
                case 8:
                    todo = EditTodo(todo, person, category);
                    WriteFileToDo(todo, todoFinalized);
                    break;
                default:
                    Console.WriteLine("Opcao invalida, digite novamente.");
                    Console.ReadKey();
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
        char choice;
        int cc = 0;
        int cp = 0;
        do
        {
            Console.WriteLine("deseja adicionar uma categoria? [s] - sim [n] - nao");
            if (!char.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Digite uma letra valida.");
                Console.ReadLine();
                continue;
            }


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
            if (!char.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Digite uma letra valida.");
                Console.ReadLine();
                continue;
            }

            if (choice == 's')
            {
                int i = 1;
                Console.WriteLine("Qual dia: ");
                if (!int.TryParse(Console.ReadLine(), out var day))
                {
                    Console.WriteLine("Digite um numero valido.");
                    Console.ReadLine();
                    continue;
                }
                Console.WriteLine("Qual mes: ");
                if (!int.TryParse(Console.ReadLine(), out var month))
                {
                    Console.WriteLine("Digite um numero valido.");
                    Console.ReadLine();
                    continue;
                }
                Console.WriteLine("Qual ano: ");
                if (!int.TryParse(Console.ReadLine(), out var year))
                {
                    Console.WriteLine("Digite um numero valido.");
                    Console.ReadLine();
                    continue;
                }
                todo.SetDueDate(DateTime.Parse(day + "/" + month + "/" + year));
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
            if (!char.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Digite uma letra valida.");
                Console.ReadLine();
                continue;
            }

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

    private static void ChangeToDoStatus(List<ToDo> l1, List<ToDo> l2)
    {

        Console.WriteLine("Altere o status da sua tarefa aqui: ");
        foreach (ToDo item in l1)
        {
            Console.WriteLine(item.ToString());
            Console.WriteLine("Deseja alterar essa tarefa? [s] - sim [n] - nao");
            if (!char.TryParse(Console.ReadLine(), out var choice))
            {
                Console.WriteLine("Digite uma letra valida.");
                Console.ReadLine();
                continue;
            }

            if (choice == 's')
            {
                item.SetStatus();
            }
        }
        WriteFileToDo(l1, l2);
    }

    private static string PrintToDo(List<ToDo> l)
    {
        string txt = "";
        foreach (ToDo item in l)
        {
            txt += item.ToString();
        }
        return txt;
    }

    private static List<ToDo> EditTodo(List<ToDo>? todo, List<Person> person, List<string> category)
    {
        int i = 1;
        int choice;
        foreach (ToDo item in todo)
        {
            Console.WriteLine("Gostaria de editar a tarefa:");
            Console.WriteLine(item.ToString());
            do
            {
                Console.WriteLine("1 - Descricao");
                Console.WriteLine("2 - Categoria");
                Console.WriteLine("3 - Data de finalizacao");
                Console.WriteLine("4 - Pessoa responsável");
                Console.WriteLine("0 - Nao desejo editar nenhum item/ sair do menu");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Digite um numero valido.");
                    Console.ReadKey();
                    continue;
                }

                switch (choice)
                {
                    case 0:
                        Console.WriteLine("Saindo...");
                        Thread.Sleep(2000);
                        break;
                    case 1:
                        Console.WriteLine("Informe a nova descricao:");
                        string d = Console.ReadLine();
                        item.SetDescription(d);
                        break;
                    case 2:
                        i = 1;
                        int cc = 0;
                        Console.WriteLine("Escolha uma categoria: ");
                        foreach (string s in category)
                        {
                            Console.WriteLine(i + "- " + s);
                            i++;
                        }
                        Console.Write(": ");
                        if (int.TryParse(Console.ReadLine(), out cc))
                        {
                            Console.WriteLine("Digite um numero valido.");
                            Console.ReadKey();
                            continue;
                        }
                        item.SetCategory(category[cc - 1]);
                        break;
                    case 3:
                        Console.WriteLine("Informe a nova data de finalizacao.");
                        Console.WriteLine("Qual dia: ");
                        if (!int.TryParse(Console.ReadLine(), out var day))
                        {
                            Console.WriteLine("Digite um numero valido.");
                            Console.ReadLine();
                            continue;
                        }
                        Console.WriteLine("Qual mes: ");
                        if (!int.TryParse(Console.ReadLine(), out var month))
                        {
                            Console.WriteLine("Digite um numero valido.");
                            Console.ReadLine();
                            continue;
                        }
                        Console.WriteLine("Qual ano: ");
                        if (!int.TryParse(Console.ReadLine(), out var year))
                        {
                            Console.WriteLine("Digite um numero valido.");
                            Console.ReadLine();
                            continue;
                        }
                        string dueDate = day + "/" + month + "/" + year;
                        item.SetDueDate(DateTime.Parse(dueDate));
                        break;
                    case 4:
                        i = 1;
                        int cp = 0;
                        Console.WriteLine("Escolha um responsavel: ");
                        foreach (var p in person)
                        {
                            Console.WriteLine(i + "- " + p);
                            i++;
                        }
                        Console.Write(": ");
                        cp = int.Parse(Console.ReadLine());
                        item.SetPerson(person[(cp - 1)]);
                        break;

                }
            } while (choice != 0);
        }
        return todo;
    }
}
