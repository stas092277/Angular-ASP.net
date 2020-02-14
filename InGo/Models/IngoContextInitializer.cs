using InGo.Models.Links;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models
{
    public static class IngoContextInitializer
    {
        public static void Initialize(this ModelBuilder modelBuilder)
        {
            InitializePosts(modelBuilder);
            InitializeTags(modelBuilder);
            InitializeTagPosts(modelBuilder);
            InitializeUsers(modelBuilder);
            InitializeComments(modelBuilder);
            InitializeDepartmens(modelBuilder);
            InitializeFaq(modelBuilder);
            InitializeFaqCategories(modelBuilder);
            InitializeEvents(modelBuilder);
            InitializeEventCategories(modelBuilder);
        }


        private static void InitializePosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasData(
                new Post
                {
                    Id = 1,
                    PublishDate = new DateTime(2019, 1, 1),
                    Title = "\"Щи *redacted*\" и другие отзывы в \"Мануфактуре\"",
                    Content = "Столовая славится своей новой фичей - теперь удобно оставлять отзывы на еду и обслуживание. <strong>Никакой модерации!</strong>",
                    AuthorId = 1
                },
                new Post
                {
                    Id = 2,
                    PublishDate = new DateTime(2019, 8, 10),
                    Title = "Нехватка припасов",
                    Content = "Блокноты закончились вчера, сахара хватит ещё часа на четыре...",
                    AuthorId = 2
                },
                new Post
                {
                    Id = 3,
                    PublishDate = new DateTime(2019, 7, 8),
                    Title = "Евгений Понасенков безапелляционно разоблачил очередную дешёвку",
                    Content = "Маэстро наносит ответный удар! <i>СНОВА!</i>",
                    AuthorId = 3
                },
                new Post
                {
                    Id = 4,
                    PublishDate = new DateTime(2019, 6, 9),
                    Title = "Хан Замай поедет на Евровидение от России",
                    Content = "...",
                    AuthorId = 4
                }
                );
        }

        private static void InitializeTags(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
                new Tag
                {
                    Id = 1,
                    Name = "ЖизньСтажёра"
                },
                new Tag
                {
                    Id = 2,
                    Name = "Ингос"
                }
                );

        }

        private static void InitializeTagPosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagPost>().HasData(
                new TagPost { Id = 1, TagId = 1, PostId = 1},
                new TagPost { Id = 2 , TagId = 2, PostId = 1}
                );
        }

        private static void InitializeUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "",
                    About = "То, что было изначально и что будет во веки веков",
                    ImgUrl = "https://ubistatic19-a.akamaihd.net/resource/ru-ru/game/rainbow6/siege-v3/r6-operators-list-warden_349633.png",
                    Type = UserType.Admin
                },
                new
                {
                    Id = 2,
                    FirstName = "Ярослав ",
                    LastName = "Колотилов",
                    Type = UserType.Intern,
                    Email = "yapk@ingos.ru",
                    About = "Монстр разработки",
                    ImgUrl = "https://randomuser.me/api/portraits/men/2.jpg",
                    IsDeleted = false,
                    DepartmentId = 1
                },
                 new
                 {
                     Id = 3,
                     FirstName = "Андрей ",
                     LastName = "Афанасенков",
                     Type = UserType.Intern,
                     Email = "andrewa@ingos.ru",
                     About = "Ы",
                     ImgUrl = "https://randomuser.me/api/portraits/men/15.jpg",
                     IsDeleted = false,
                     DepartmentId = 2
                 },
                new
                {
                    Id = 4,
                    FirstName = "Юлия ",
                    LastName = "Мальцева",
                    Type = UserType.Intern,
                    Email = "vasya@mail.ru",
                    About = "Ку",
                    ImgUrl = "https://randomuser.me/api/portraits/women/66.jpg",
                    IsDeleted = false,
                    DepartmentId = 3
                },
                new
                {
                    Id = 5,
                    FirstName = "Бойченко ",
                    LastName = "Александр",
                    Type = UserType.Mentor,
                    Email = "somemail@ingos.ru",
                    About = ")))))",
                    ImgUrl = "https://randomuser.me/api/portraits/men/19.jpg",
                    IsDeleted = false,
                    DepartmentId = 4
                },
                new
                {
                    Id = 6,
                    FirstName = "Станислав ",
                    LastName = "Чебыкин",
                    Type = UserType.Intern,
                    Email = "vasya@mail.ru",
                    About = "Спанч Боб",
                    ImgUrl = "https://randomuser.me/api/portraits/men/2.jpg",
                    IsDeleted = false,
                    DepartmentId = 5
                },
                new
                {
                    Id = 7,
                    FirstName = "Иван ",
                    LastName = "Найденов",
                    Type = UserType.Intern,
                    Email = "naydenovI@ingos.ru",
                    About = "ЫЫЫЫ",
                    ImgUrl = "https://randomuser.me/api/portraits/men/79.jpg",
                    IsDeleted = false,
                    DepartmentId = 1
                },
                new
                {
                    Id = 8,
                    FirstName = "Biba ",
                    LastName = "Freeman",
                    Type = UserType.Intern,
                    Email = "anita.freeman33@example.com",
                    About = "Хэй гайс у меня все найс",
                    ImgUrl = "https://randomuser.me/api/portraits/women/33.jpg",
                    IsDeleted = false,
                    DepartmentId = 2
                },
                new
                {
                    Id = 9,
                    FirstName = "Boba",
                    LastName = "Armstrong",
                    Type = UserType.Intern,
                    Email = "robin.armstrong94@example.com",
                    About = "Принцеска",
                    ImgUrl = "https://randomuser.me/api/portraits/women/64.jpg",
                    IsDeleted = false,
                    DepartmentId = 3
                },
                new
                {
                    Id = 10,
                    FirstName = "Cherly ",
                    LastName = "Gonzales",
                    Type = UserType.Intern,
                    Email = "cherly.gonzales66@example.com",
                    About = "Лупа и пупа ахаха",
                    ImgUrl = "https://randomuser.me/api/portraits/women/77.jpg",
                    IsDeleted = false,
                    DepartmentId = 4
                },
                new
                {
                    Id = 11,
                    FirstName = "Marion",
                    LastName = "Franklin",
                    Type = UserType.Mentor,
                    Email = "vasya@mail.ru",
                    About = "Забаню за флуд!",
                    ImgUrl = "https://randomuser.me/api/portraits/women/81.jpg",
                    IsDeleted = false,
                    DepartmentId = 5
                }
                );
        }

        private static void InitializeDepartmens(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(new Department() { Id = 1, Name = "IT", IsDeleted = false },
                new Department() { Id = 2, Name = "Медицинская аналитика", IsDeleted = false },
                new Department() { Id = 3, Name = "Продуктовый менеджмент", IsDeleted = false },
                new Department() { Id = 4, Name = "Клиентский менеджмент", IsDeleted = false },
                new Department() { Id = 5, Name = "Развитие бизнеса", IsDeleted = false },
                new Department() { Id = 6, Name = "Маркетинг", IsDeleted = false },
                new Department() { Id = 7, Name = "HR", IsDeleted = false },
                new Department() { Id = 8, Name = "Юриспруденция", IsDeleted = false });

        }

        private static void InitializeComments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasData(new
            {
                Id = 1,
                Content = "Всем привет",
                IsDeleted = false,
                PublishDate = DateTime.Now,
                AuthorId = 1,
                PostId = 1
            },
                new
                {
                    Id = 2,
                    Content = "Первый)0)0)0)))00)0",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 3,
                    PostId = 1
                },
                new
                {
                    Id = 3,
                    Content = "Лайк",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 3,
                    PostId = 1
                },
                new
                {
                    Id = 4,
                    Content = "Где скачать?",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 4,
                    PostId = 2
                },
                new
                {
                    Id = 5,
                    Content = "Автор молодец",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 5,
                    PostId = 2
                },
                new
                {
                    Id = 6,
                    Content = "wtf?",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 6,
                    PostId = 3
                },
                new
                {
                    Id = 7,
                    Content = "Хорошая тема, сохранила",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 7,
                    PostId = 3
                },
                new
                {
                    Id = 8,
                    Content = "Мммм, интересно",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 8,
                    PostId = 1
                },
                new
                {
                    Id = 9,
                    Content = "ахаххаха",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 9,
                    PostId = 2
                },
                new
                {
                    Id = 10,
                    Content = "А??",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 6,
                    PostId = 3
                },
                new
                {
                    Id = 11,
                    Content = "Ыыыыы",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 5,
                    PostId = 4
                },
                new
                {
                    Id = 12,
                    Content = "Прикольно",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 4,
                    PostId = 4
                },
                new
                {
                    Id = 13,
                    Content = "Дизлайк!!!!",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 3,
                    PostId = 3
                },
                new
                {
                    Id = 14,
                    Content = "Худшая тема евер",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 2,
                    PostId = 2
                },
                new
                {
                    Id = 15,
                    Content = "ого себе, я в шоке",
                    IsDeleted = false,
                    PublishDate = DateTime.Now,
                    AuthorId = 1,
                    PostId = 1
                });
        }


        private static void InitializeFaqCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FaqCategory>().HasData(
                new FaqCategory
                {
                    Id = 1,
                    Description = "Лайфхаки для GIT",
                    IsDeleted = false
                },
                new FaqCategory
                {
                    Id = 2,
                    Description = "Angular",
                    IsDeleted = false
                },
                new FaqCategory
                {
                    Id = 3,
                    Description = "Мероприятия стажировок",
                    IsDeleted = false
                },
                new FaqCategory
                {
                    Id = 4,
                    Description = "Выживание в ингосе",
                    IsDeleted = false
                },
                new FaqCategory
                {
                    Id = 5,
                    Description = "Общие вопросы",
                    IsDeleted = false
                }
                );
        }


        private static void InitializeFaq(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faq>().HasData(
                new Faq
                {
                    Id = 1,
                    Question = "Как сварить пельмени при помощи кулера?",
                    Answer = "<ul><li>Закидываем пельмени в кружку</li><li>Заливаем кипятком(красный краник)</li><li>???</li><li>PROFIT</li></ul>",
                    IsDeleted = false,
                    FaqCategoryId = 5
                },
                new Faq
                {
                    Id = 2,
                    Question = "У меня оч много конфликтов, что делать?",
                    Answer = "<ol><li>Создаем папку в любом новом месте</li><li>Открываем командную строку в этой папке</li><li><code>git clone \"здесь тип ссылка на тфс\"</code></li></ol>",
                    IsDeleted = false,
                    FaqCategoryId = 1
                },
                new Faq
                {
                    Id = 3,
                    Question = "Где вкусно покушать в Ингосе?",
                    Answer = "В столовой - дорого, в панде - много лука, ????",
                    IsDeleted = false,
                    FaqCategoryId = 4


                },
                new Faq
                {
                    Id = 4,
                    Question = "Как выжить в кабинете при 31 градусе тепла?",
                    Answer = "Пить. <strong>Водичку</strong>. Или купить вентилятор как у Филиппа. Такой, с котиком.",
                    IsDeleted = false,
                    FaqCategoryId = 4


                },
                new Faq
                {
                    Id = 5,
                    Question = "Смотр проектов",
                    Answer = "Последняя предзащита пройдет <strong>20 сентября в 18:00</strong> в амфитеатре.",
                    IsDeleted = false,
                    FaqCategoryId = 3


                },
                new Faq
                {
                    Id = 6,
                    Question = "Дресс-код: понять и простить?",
                    Answer = "Нет.",
                    IsDeleted = false,
                    FaqCategoryId = 5


                },
                new Faq
                {
                    Id = 7,
                    Question = "МЫ РЕШИЛИ ПЕРЕЕХАТЬ НА АНГУЛЯР И ВСЁ СЛОМАЛОСЬ. ЧТО ДЕЛАТЬ?",
                    Answer = "Страдать и гуглить, ребят. С:",
                    IsDeleted = false,
                    FaqCategoryId = 2


                }
                );
        }

        private static void InitializeEventCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventCategory>().HasData(
                new EventCategory
                {
                    Id = 1,
                    Description = "Category 1",
                    IsDeleted = false,
                },
                new EventCategory
                {
                    Id = 2,
                    Description = "Category 2",
                    IsDeleted = false,
                },
                new EventCategory
                {
                    Id = 3,
                    Description = "Category 3",
                    IsDeleted = false,
                },
                new EventCategory
                {
                    Id = 4,
                    Description = "Category 4",
                    IsDeleted = false,
                }
                );
        }

        private static void InitializeEvents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "Йога для всех",
                    Description = "Красочное описание йоги",
                    Address = "Щепкина, 3",
                    IsDeleted = false,
                    EventCategoryId = 1
                },
                 new Event
                 {
                     Id = 2,
                     Title = "Бег с тренером",
                     Description = "Красочное описание бега",
                     Address = "Сад Баумана",
                     IsDeleted = false,
                     EventCategoryId = 2
                 },
                 new Event
                 {
                     Id = 3,
                     Title = "Тренинг по agile",
                     Description = "Красочное описание бега",
                     Address = "Сад Баумана",
                     IsDeleted = false,
                     EventCategoryId = 3
                 }
                 );

        }
    }
}