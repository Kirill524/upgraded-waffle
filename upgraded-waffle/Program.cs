//Завдання 1
//Створити додаток «Словники».
//Основне завдання проєкту: зберігати словники різними мовами і дозволяти користувачеві знаходити переклад потрібного слова або фрази.
//Інтерфейс додатку повинен надавати такі можливості:
//■ Створювати словник. Під час створення необхідно вказати тип словника. Наприклад, англо-російський або російсько-англійський.
//■ Додавати слово і його переклад до вже існуючого словника. Оскільки слово може
//мати декілька перекладів, необхідно дотримуватися можливості створення декількох варіантів перекладу.
//■ Замінювати слово або його переклад у словнику.
//■ Видаляти слово або переклад. Якщо слово видаляється, усі його переклади видаляються разом з ним. Не можна видалити переклад слова, якщо це останній
//варіант перекладу.
//■ Шукати переклад слова.
//■ Словники повинні зберігатися у файлах.
//■ Слово і варіанти його перекладів можна експортувати до окремого файлу результату.
//■ При старті програми потрібно показувати меню для роботи з програмою. Якщо
//вибір пункту меню відкриває підменю, тоді в ньому потрібно передбачити можливість повернення до попереднього меню.

using System;

namespace Словники
{
    public class WordEntry
    {
        public string Word { get; set; }
        public List<string> Translations { get; set; } = new List<string>();
    }

    public class DictionaryModel
    {
        public string Name { get; set; }
        public List<WordEntry> Words { get; set; } = new List<WordEntry>();

        public void AddWord(string word, string translation)
        {
            var entry = Words.Find(w => w.Word == word);
            if (entry != null)
            {
                if (!entry.Translations.Contains(translation))
                    entry.Translations.Add(translation);
            }
            else
            {
                Words.Add(new WordEntry { Word = word, Translations = new List<string> { translation } });
            }
        }

        public List<string> GetTranslations(string word)
        {
            var entry = Words.Find(w => w.Word == word);
            return entry?.Translations;
        }

        public void RemoveTranslation(string word, string translation)
        {
            var entry = Words.Find(w => w.Word == word);
            if (entry != null)
            {
                if (entry.Translations.Count > 1)
                    entry.Translations.Remove(translation);
                else
                    Console.WriteLine("Неможливо видалити останній переклад.");
            }
        }

        public void RemoveWord(string word)
        {
            Words.RemoveAll(w => w.Word == word);
        }

        public void SaveToFile(string path)
        {
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static DictionaryModel LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<DictionaryModel>(json);
        }
    }

    class Program
    {
        static void Main()
        {
            DictionaryModel dict = new DictionaryModel { Name = "Англо-український" };
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n--- Меню словника ---");
                Console.WriteLine("1. Додати слово");
                Console.WriteLine("2. Пошук перекладу");
                Console.WriteLine("3. Видалити слово або переклад");
                Console.WriteLine("4. Зберегти словник у файл");
                Console.WriteLine("5. Завантажити словник з файлу");
                Console.WriteLine("6. Вийти");
                Console.Write("Вибір: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Слово: ");
                        string word = Console.ReadLine();
                        Console.Write("Переклад: ");
                        string translation = Console.ReadLine();
                        dict.AddWord(word, translation);
                        break;
                    case "2":
                        Console.Write("Слово для перекладу: ");
                        string search = Console.ReadLine();
                        var translations = dict.GetTranslations(search);
                        if (translations != null)
                            Console.WriteLine($"Переклади: {string.Join(", ", translations)}");
                        else
                            Console.WriteLine("Слово не знайдено.");
                        break;
                    case "3":
                        Console.WriteLine("1. Видалити слово  2. Видалити переклад");
                        string delChoice = Console.ReadLine();
                        if (delChoice == "1")
                        {
                            Console.Write("Слово для видалення: ");
                            dict.RemoveWord(Console.ReadLine());
                        }
                        else if (delChoice == "2")
                        {
                            Console.Write("Слово: ");
                            string w = Console.ReadLine();
                            Console.Write("Переклад для видалення: ");
                            string t = Console.ReadLine();
                            dict.RemoveTranslation(w, t);
                        }
                        break;
                    case "4":
                        Console.Write("Шлях файлу для збереження: ");
                        dict.SaveToFile(Console.ReadLine());
                        break;
                    case "5":
                        Console.Write("Шлях файлу для завантаження: ");
                        dict = DictionaryModel.LoadFromFile(Console.ReadLine());
                        Console.WriteLine("Словник завантажено.");
                        break;
                    case "6":
                        exit = true;
                        break;
                }
            }
        }
    }
}

//Завдання 2
//Створити додаток «Вікторина».
//Основне завдання проєкту: надати користувачеві можливість перевірити свої знання
//у різних галузях.
//Інтерфейс додатку повинен надавати такі можливості:
//■ При старті програми користувач вводить логін і пароль для входу. Якщо користувач не зареєстрований, він має пройти процес реєстрації.
//■ При реєстрації потрібно вказати:
//• логін(не можна зареєструвати вже існуючий логін);
//• пароль;
//• дату народження.
//■ Після входу в систему користувач може:
//• стартувати нову вікторину;
//• переглянути результати своїх минулих вікторин;
//• переглянути Топ-20 з конкретної вікторини;
//• змінити налаштування: можна змінювати пароль та дату народження;
//• вихід.
//■ Для старту нової вікторини користувач повинен обрати розділ знань вікторини.
//Наприклад, «Історія», «Географія», «Біологія» і т.д. Також потрібно передбачити
//змішану вікторину, коли питання будуть обиратися з різних вікторин за рандомним
//принципом.
//■ Конкретна вікторина складається із двадцяти питань. Кожне питання може мати
//один або декілька правильних варіантів відповідей. Якщо питання передбачає
//декілька правильних відповідей, а користувач вказав не все, питання не зараховується.
//■ Після завершення вікторини користувач отримує кількість правильних відповідей,
//а також отримує своє місце у таблиці результатів гравців вікторини.
//Необхідно також розробити утиліту для створення і редагування вікторин і їх питань. Цей додаток має передбачати вхід за логіном і паролем.

using System;

namespace Вікторина
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public List<QuizResult> Results { get; set; } = new List<QuizResult>();
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public List<int> CorrectOptionIndexes { get; set; } = new List<int>();
    }

    public class Quiz
    {
        public string Section { get; set; }
        public List<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    }

    public class QuizResult
    {
        public string QuizSection { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime Date { get; set; }
    }

    class Program
    {
        static List<User> users = new List<User>();
        static List<Quiz> quizzes = new List<Quiz>();

        static void Main()
        {
            Console.WriteLine("=== Вікторина ===");
            User currentUser = Login();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Старт нової вікторини");
                Console.WriteLine("2. Перегляд результатів");
                Console.WriteLine("3. Вихід");
                Console.Write("Вибір: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        if (quizzes.Count == 0)
                        {
                            Console.WriteLine("Вікторини відсутні. Створіть хоча б одну вікторину.");
                        }
                        else
                        {
                            StartQuiz(currentUser);
                        }
                        break;
                    case "2":
                        ShowResults(currentUser);
                        break;
                    case "3":
                        exit = true;
                        break;
                }
            }
        }

        static User Login()
        {
            Console.Write("Логін: ");
            string login = Console.ReadLine();
            var user = users.Find(u => u.Login == login);
            if (user != null)
            {
                Console.Write("Пароль: ");
                string pass = Console.ReadLine();
                if (user.Password == pass)
                {
                    Console.WriteLine("Успішний вхід!");
                    return user;
                }
            }

            Console.WriteLine("Користувач не знайдений, реєстрація.");
            return Register(login);
        }

        static User Register(string login)
        {
            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            Console.Write("Дата народження (yyyy-mm-dd): ");
            DateTime birth = DateTime.Parse(Console.ReadLine());

            var newUser = new User { Login = login, Password = password, BirthDate = birth };
            users.Add(newUser);
            Console.WriteLine("Користувач зареєстрований!");
            return newUser;
        }

        static void StartQuiz(User user)
        {
            Quiz quiz = quizzes.FirstOrDefault();
            if (quiz == null)
            {
                Console.WriteLine("Вікторина відсутня.");
                return;
            }

            int correctCount = 0;
            foreach (var q in quiz.Questions)
            {
                Console.WriteLine("\n" + q.Question);
                for (int i = 0; i < q.Options.Count; i++)
                    Console.WriteLine($"{i + 1}. {q.Options[i]}");

                Console.Write("Введіть номери правильних варіантів через кому: ");
                var input = Console.ReadLine().Split(',');
                var selected = Array.ConvertAll(input, s => int.Parse(s.Trim()) - 1);

                Array.Sort(selected);
                var correct = q.CorrectOptionIndexes.ToArray();
                Array.Sort(correct);

                if (selected.Length == correct.Length && selected.SequenceEqual(correct))
                    correctCount++;
            }

            Console.WriteLine($"\nКількість правильних відповідей: {correctCount}");
            user.Results.Add(new QuizResult { QuizSection = quiz.Section, CorrectAnswers = correctCount, Date = DateTime.Now });
        }

        static void ShowResults(User user)
        {
            if (user.Results.Count == 0)
            {
                Console.WriteLine("Результатів немає.");
                return;
            }

            foreach (var r in user.Results)
            {
                Console.WriteLine($"{r.QuizSection} - Правильних відповідей: {r.CorrectAnswers} ({r.Date})");
            }
        }
    }
}