using System;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Task_3
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            while (true)
            {
                string connectionString = @"Data Source=DESKTOP-5BD88QO\SQLEXPRESS;Initial Catalog=StudBD; Integrated Security=True";
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nВыберите таблицу для дальнейших действий");
                    Console.ResetColor();
                    Console.WriteLine("1. Студенты");
                    Console.WriteLine("2. Предметы");
                }
                catch (Exception ex)
                {

                }
                int N = Convert.ToInt16(Console.ReadLine());
                switch (N)
                {
                    case 1:
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = $"SELECT * FROM Students;";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                {

                                    if (reader.HasRows) // если есть данные
                                    {
                                        string columnName1 = reader.GetName(0);
                                        string columnName2 = reader.GetName(1);
                                        string columnName3 = reader.GetName(2);
                                        string columnName4 = reader.GetName(3);
                                        string columnName5 = reader.GetName(4);
                                        //Шапка таблицы
                                        Console.ForegroundColor = ConsoleColor.Magenta;
                                        Console.WriteLine($"{columnName1}\t     {columnName2}\t     {columnName3}\t      {columnName4}\t     {columnName5}");
                                        Console.ResetColor();

                                        //Чтение данных из таблицы
                                        while (await reader.ReadAsync())
                                        {
                                            object StudID = reader.GetValue(0);
                                            object NameStud = reader.GetValue(1);
                                            object FamilyStud = reader.GetValue(2);
                                            object ObjectID = reader.GetValue(3);
                                            object Credit= reader.GetValue(4);
                                            Console.WriteLine($"{StudID}\t     {NameStud}\t     {FamilyStud}\t         {ObjectID}\t        {Credit}");
                                        }
                                    }
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nВыберите действие");
                            Console.ResetColor();
                            Console.WriteLine("1. Добавить Студента");
                            Console.WriteLine("2. Обновить данные студента");
                            Console.WriteLine("3. Удалить");
                            Console.WriteLine("4. Список студентов, изучающие конкретный предмет");
                            Console.WriteLine("5. Средняя успеваемость");
                            Console.WriteLine("6. Вывести список студентов в обратном порядке");

                            int L = Convert.ToInt16(Console.ReadLine());
                            switch (L)
                            {
                                //CREATE STUDENT
                                case 1:
                                    try
                                    {
                                        using (SqlCommand createUser = new SqlCommand(sql, connection))
                                        {
                                            int num = await createUser.ExecuteNonQueryAsync();
                                            Console.WriteLine("\nВведите Имя:");
                                            string firstname = Console.ReadLine();
                                            Console.WriteLine("\nВведите Фамилию");
                                            string lastname = Console.ReadLine();
                                            Console.WriteLine("\nВведите ID Предмета");
                                            string objec = Console.ReadLine();

                                            sql = $"INSERT INTO Students (NameStud,FamilyStud,ObjectID) VALUES ('{firstname}','{lastname}','{objec}')";
                                            createUser.CommandText = sql;
                                            num = await createUser.ExecuteNonQueryAsync();
                                            Console.WriteLine($"\nДобавлено объектов: {num}");

                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    break;

                                    //UPDATE STUDENT
                                    case 2:
                                        using (SqlCommand updateUser = new SqlCommand(sql, connection))
                                        {
                                            int up = await updateUser.ExecuteNonQueryAsync();
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine("\nВыберите действие");
                                            Console.ResetColor();
                                            Console.WriteLine("1. Изменить Имя");
                                            Console.WriteLine("2. Изменить Фамилию");
                                            Console.WriteLine("3. Вернуться назад\n");

                                            //Выбор что обновить
                                            int obnova = Convert.ToInt16(Console.ReadLine());
                                            switch (obnova)
                                            {
                                                case 1:
                                                    Console.WriteLine("Введите ID пользователя для изменения");
                                                    string studID = Console.ReadLine();
                                                    Console.WriteLine("\nВведите новое Имя:");
                                                    string StudName = Console.ReadLine();
                                                    sql = $"UPDATE Students set NameStud='{StudName}' WHERE StudID={studID}";

                                                    break;
                                                case 2:
                                                    Console.WriteLine("Введите ID пользователя для изменения");
                                                    string stuID = Console.ReadLine();
                                                    Console.WriteLine("\nВведите новую Фамилию:");
                                                    string second = Console.ReadLine();
                                                    sql = $"UPDATE Students set FamilyStud='{second}' WHERE StudID='{stuID}'";

                                                    break;
                                            }

                                            updateUser.CommandText = sql;
                                            up = await updateUser.ExecuteNonQueryAsync();
                                        }
                                    break;

                                    //DELETE STUDENT
                                    case 3:
                                    try
                                    {
                                        using (SqlCommand deleteOrder = new SqlCommand(sql, connection))
                                        {
                                            int del = await deleteOrder.ExecuteNonQueryAsync();
                                            Console.WriteLine("Введите ID студента для удаления");
                                            string orID = Console.ReadLine();
                                            sql = $"delete from Students where StudID='{orID}'";
                                            deleteOrder.CommandText = sql;
                                            del = await deleteOrder.ExecuteNonQueryAsync();
                                            Console.WriteLine($"\nУдалено объектов: {del}");
                                        }
                                    }
                                    catch
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\nОШИБКА!!! Некорректный ввод данных\n");
                                        Console.ResetColor();
                                    }
                                    break;

                                    //СТУДЕНТЫ ИЗУЧАЮЩИЕ КОНКРЕТНЫЙ ПРЕДМЕТ
                                    case 4:
                                           Console.ForegroundColor = ConsoleColor.Yellow;
                                           Console.WriteLine("\nВведите имя предмета:");
                                           Console.ResetColor();
                                            string objectname = Console.ReadLine();
                                            sql = $"select Students.NameStud,Students.FamilyStud,Object.ObjectName from Students join Object on Students.ObjectID=Object.ObjectID where Object.ObjectName='{objectname}'";
                                    
                                    using (SqlCommand Vibor = new SqlCommand(sql, connection))
                                        {
                                               using (SqlDataReader reader = await Vibor.ExecuteReaderAsync())
                                                {
                                                 Console.ForegroundColor = ConsoleColor.Magenta;
                                                 Console.WriteLine("\nСтуденты изучающие предмет: " +  objectname);
                                                 Console.ResetColor();
                                                        if (reader.Read())
                                                        {
                                                                while (reader.Read())
                                                                {
                                                                    string studentName = reader["NameStud"].ToString();
                                                                    string studentLastName = reader["FamilyStud"].ToString();
                                                       
                                                                    Console.WriteLine($"{studentName} {studentLastName}");
                                                                }
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Данных по этому предмету нет");
                                                        }
                                                }
                                        }
                                    
                                    break;

                                    //Среднее значение оценок
                                    case 5:  
                                    
                                        sql = $"select AVG(Credit) AS Srednie From Students";
                                          using(SqlCommand Sred=new SqlCommand(sql, connection))
                                            {
                                          
                                                using (SqlDataReader reader=await Sred.ExecuteReaderAsync())
                                                {

                                                while (reader.Read())
                                                {
                                                    string srednie = reader["Srednie"].ToString();
                                                    Console.WriteLine($"Средняя успеваемость студентов: {srednie} ");
                                                }
                                            
                                                }
                                            }
                                    break;
                                  
                                    //Вывод списка в обратном порядке
                                    case 6:
                                        sql = $"select * FROM Students Order by FamilyStud DESC;";
                                        using(SqlCommand revSpisok=new SqlCommand(sql, connection))
                                        {
                                                using(SqlDataReader reader=await revSpisok.ExecuteReaderAsync())
                                            { 
                                                Console.ForegroundColor= ConsoleColor.Yellow;
                                                Console.WriteLine("Список студентов в обратном порядке:\n");
                                                Console.ResetColor();
                                                    while (reader.Read())
                                                    {
                                               
                                                        string name = reader["NameStud"].ToString();
                                                        string family= reader["FamilyStud"].ToString();
                                                        Console.WriteLine(name + " " + family);
                                                    }
                                            }
                                        }
                                    break;

                            }
                           
                        }
                        break;


                        //ТАБЛИЦА ПРЕДМЕТЫ
                    case 2:

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();
                                    string sql = $"SELECT * FROM Object;";
                                    using (SqlCommand command = new SqlCommand(sql, connection))
                                    {
                               
                                         using (SqlDataReader reader = await command.ExecuteReaderAsync())
                                         {

                                                if (reader.HasRows) // если есть данные
                                                {
                                                    string columnName1 = reader.GetName(0);
                                                    string columnName2 = reader.GetName(1);
                                                    //Шапка таблицы
                                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                                    Console.WriteLine($"{columnName1}\t     {columnName2}\t  ");
                                                    Console.ResetColor();

                                                    //Чтение данных из таблицы
                                                    while (await reader.ReadAsync())
                                                    {
                                                        object ObjectID = reader.GetValue(0);
                                                        object ObjectName = reader.GetValue(1);

                                                        Console.WriteLine($"   {ObjectID}\t            {ObjectName}\t");
                                                    }
                                                }
                                         }
                                
                                    }
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("\nВыберите действие");
                                        Console.ResetColor();
                                        Console.WriteLine("1. Добавить предмет");
                                        Console.WriteLine("2. Обновить данные предмета");
                                        Console.WriteLine("3. Удалить");
                                        Console.WriteLine("4. Количество студентов,изучающие конкректный предмет");
                                        Console.WriteLine("5. Вернуться назад\n");

                                        string H = Console.ReadLine();
                                        switch (H)
                                        {
                                    //Добавление предмета
                                            case "1":
                                                using (SqlCommand createObject = new SqlCommand(sql, connection))
                                                {
                                                    int num = await createObject.ExecuteNonQueryAsync();
                                                    Console.WriteLine("\nНазвание предмета:");
                                                    string firstname = Console.ReadLine();
                                                    sql = $"INSERT INTO Object (ObjectName) VALUES ('{firstname}')";
                                                    createObject.CommandText = sql;
                                                    num = await createObject.ExecuteNonQueryAsync();
                                                    Console.WriteLine($"\nДобавлено объектов: {num}");
                                                }
                                                break;

                                     //Апдейт предмета
                                            case "2":
                                                
                                                using (SqlCommand updateObject = new SqlCommand(sql, connection))
                                                {
                                                    int up = await updateObject.ExecuteNonQueryAsync(); 
                                                    Console.WriteLine("Введите название предмета для замены:");
                                                    string objectname = Console.ReadLine();
                                                    Console.WriteLine("\nВведите новое название:");
                                                    string newobject = Console.ReadLine();
                                                    sql = $"UPDATE Object set ObjectName='{newobject}' WHERE ObjectName='{objectname}'";
                                                    updateObject.CommandText = sql;
                                                    up = await updateObject.ExecuteNonQueryAsync();
                                                    Console.WriteLine("Предмет " + objectname + " заменён на: " + newobject);
                                                }
                                                break;
                                    
                                    //Удаление предмета
                                            case "3":
                                                using (SqlCommand deleteObject = new SqlCommand(sql, connection))
                                                {
                                                     int del = await deleteObject.ExecuteNonQueryAsync();
                                                     Console.WriteLine("Введите название предмета для удаления:");
                                                     string objectname = Console.ReadLine();
                                                     sql = $"delete from Object where ObjectName='{objectname}'";
                                                     deleteObject.CommandText = sql;
                                                     del = await deleteObject.ExecuteNonQueryAsync();
                                                }
                                                break; 
                                            
                                    //Количество студентов на конкретный предмет
                                            case "4":
                                                sql = $"SELECT Object.ObjectName, COUNT(Students.StudID) AS StudentsCount FROM Object JOIN Students ON Object.ObjectID = Students.ObjectID GROUP BY Object.ObjectName;";
                                                 using (SqlCommand Vibor = new SqlCommand( sql, connection))
                                                 {
                                                    using (SqlDataReader reader = await Vibor.ExecuteReaderAsync())
                                                    {                                                     
                                                       while (reader.Read())
                                                       {
                                                           string objectName = reader["ObjectName"].ToString();
                                                           string student = reader["StudentsCount"].ToString();
                                                           Console.WriteLine($"Предмет: {objectName} >> Количество студентов: {student}");
                                                       }
                                                    }
                                                 }                                           
                                                break;
                                        }
                                    
                                }
                      
                        break;
                    }  
                 }
            }
        }
}
