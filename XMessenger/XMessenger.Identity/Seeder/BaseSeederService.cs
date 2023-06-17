using XMessenger.Database.Context;
using XMessenger.Database.Models;
using XMessenger.Helpers.Identity;
using XMessenger.Identity.Db.Context;
using XMessenger.Identity.Models;

namespace XMessenger.Identity.Seeder
{
    public class BaseSeederService : ISeederService
    {
        private readonly IdentityContext _identityDb;
        private readonly DatabaseContext _databaseDb;
        public BaseSeederService(IdentityContext identityDb, DatabaseContext databaseDb)
        {
            _identityDb = identityDb;
            _databaseDb = databaseDb;
        }

        public async virtual Task<Result<int>> SeedSystemAsync()
        {
            await SeedIdentityDbAsync();

            await SeedDatabaseDbAsync();

            return Result<int>.Success();
        }

        private async Task SeedIdentityDbAsync()
        {
            int counter = 0;
            if (!_identityDb.Roles.Any())
            {
                await _identityDb.Roles.AddRangeAsync(GetDefaultsRoles());
                counter++;
            }
            if (!_identityDb.Apps.Any())
            {
                await _identityDb.Apps.AddRangeAsync(GetDefaultsApp());
                counter++;
            }

            if (counter > 0)
                await _identityDb.SaveChangesAsync();
        }

        private IEnumerable<Role> GetDefaultsRoles()
        {
            yield return new Role
            {
                Name = DefaultsRoles.Administrator,
                NameNormalized = DefaultsRoles.Administrator.ToUpper(),
            };
            yield return new Role
            {
                Name = DefaultsRoles.Moderator,
                NameNormalized = DefaultsRoles.Moderator.ToUpper(),
            };
            yield return new Role
            {
                Name = DefaultsRoles.User,
                NameNormalized = DefaultsRoles.User.ToUpper(),
            };
        }

        private IEnumerable<App> GetDefaultsApp()
        {
            var now = DateTime.Now;

            yield return new App
            {
                Name = "Тестовий застосунок",
                Description = "Тестовий застосунок для процесу розробки",
                IsActive = true,
                ShortName = "Тест. зас.",
                ActiveFrom = now,
                ActiveTo = now.AddYears(5),
                Image = "/files/Other.png",
                ClientId = "F94A4E87C1FD23C102800D",
                ClientSecret = "b2e459a6c58a472da53e47a46d6c5ad4c1ecda073aa4402b9724ac55cd65dcc4",
            };
        }

        private async Task SeedDatabaseDbAsync()
        {
            if (!_databaseDb.Countries.Any())
            {
                await _databaseDb.Countries.AddAsync(new Country
                {
                    Flag = "",
                    Name = "Україна",
                    Populations = "46 000 000 осіб",
                    Regions = new List<Region>
                    {
                        new Region
                        {
                            Flag = "",
                            Name = "Київська Область",
                            Areas = new List<Area>
                            {
                                new Area
                                {
                                    Name = string.Empty,
                                    Settlements = new List<Settlement>
                                    {
                                        new Settlement
                                        {
                                            Flag = "",
                                            Name = "м. Київ",
                                            Type = SettlementType.City,
                                            Metro = new List<Metro>
                                            {
                                                new Metro
                                                {
                                                    Description = "Перша станція київського метрополітену була побудована 1965 року",
                                                    Name = "Київський метрополітен",
                                                    Flag = "",
                                                    Lines = new List<MetroLine>
                                                    {
                                                        new MetroLine
                                                        {
                                                            Name = "Оболоно-Теримківська",
                                                            Color = "Blue",
                                                            Stations = new List<MetroStation>
                                                            {
                                                                new MetroStation
                                                                {
                                                                    Code = "210",
                                                                    IsTransfer = false,
                                                                    Name = "Героїв Дніпра"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "211",
                                                                    IsTransfer = false,
                                                                    Name = "Мінська"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "212",
                                                                    IsTransfer = false,
                                                                    Name = "Оболонь"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "213",
                                                                    IsTransfer = false,
                                                                    Name = "Почайна"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "214",
                                                                    IsTransfer = false,
                                                                    Name = "Тараса Шевченка"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "215",
                                                                    IsTransfer = false,
                                                                    Name = "Контрактова Площа"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "216",
                                                                    IsTransfer = false,
                                                                    Name = "Поштова Площа"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "217",
                                                                    IsTransfer = true,
                                                                    Name = "Майдан Незалежності"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Code = "218",
                                                                    IsTransfer = true,
                                                                    Name = "Площа Льва Толстого"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Олімпійська",
                                                                    IsTransfer = false,
                                                                    Code = "219"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Палац \"Україна\"",
                                                                    IsTransfer = false,
                                                                    Code = "220"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Либідська",
                                                                    IsTransfer = false,
                                                                    Code = "221"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Деміївська",
                                                                    IsTransfer = false,
                                                                    Code = "222"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Голосіївська",
                                                                    IsTransfer = false,
                                                                    Code = "223"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Васильківська",
                                                                    IsTransfer = false,
                                                                    Code = "224"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Виставковий Центр",
                                                                    IsTransfer = false,
                                                                    Code = "225"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Іподром",
                                                                    IsTransfer = false,
                                                                    Code = "226"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Теремки",
                                                                    IsTransfer = false,
                                                                    Code = "227"
                                                                },
                                                            }
                                                        },
                                                        new MetroLine
                                                        {
                                                            Name = "Шевченківська",
                                                            Color = "Red",
                                                            Stations = new List<MetroStation>
                                                            {
                                                                new MetroStation
                                                                {
                                                                    Name = "Академістечко",
                                                                    IsTransfer = false,
                                                                    Code = "110"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Житомирська",
                                                                    IsTransfer = false,
                                                                    Code = "111"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Святошино",
                                                                    IsTransfer = false,
                                                                    Code = "112"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Нивки",
                                                                    IsTransfer = false,
                                                                    Code = "113"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Берестейська",
                                                                    IsTransfer = false,
                                                                    Code = "114"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Шулявська",
                                                                    IsTransfer = false,
                                                                    Code = "115"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Політехничний Інститут",
                                                                    IsTransfer = false,
                                                                    Code = "116"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Вокзальна",
                                                                    IsTransfer = false,
                                                                    Code = "117"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Університет",
                                                                    IsTransfer = false,
                                                                    Code = "118"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Театральна",
                                                                    IsTransfer = true,
                                                                    Code = "119"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Хрещатик",
                                                                    IsTransfer = true,
                                                                    Code = "120"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Арсенальна",
                                                                    IsTransfer = false,
                                                                    Code = "121"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Дніпро",
                                                                    IsTransfer = false,
                                                                    Code = "122"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Гідропарк",
                                                                    IsTransfer = false,
                                                                    Code = "123"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Лівобережна",
                                                                    IsTransfer = false,
                                                                    Code = "124"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Дарниця",
                                                                    IsTransfer = false,
                                                                    Code = "125"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Чернігівська",
                                                                    IsTransfer = false,
                                                                    Code = "126"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Лісова",
                                                                    IsTransfer = false,
                                                                    Code = "127"
                                                                }
                                                            }
                                                        },
                                                        new MetroLine
                                                        {
                                                            Name = "Сирецько-Вигурівська",
                                                            Color = "Green",
                                                            Stations = new List<MetroStation>
                                                            {
                                                                new MetroStation
                                                                {
                                                                    Name = "Сирець",
                                                                    IsTransfer = false,
                                                                    Code = "310"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Дорогожичі",
                                                                    IsTransfer = false,
                                                                    Code = "311"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Лукянівська",
                                                                    IsTransfer = false,
                                                                    Code = "312"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Золоті ворота",
                                                                    IsTransfer = true,
                                                                    Code = "314"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Палац спорту",
                                                                    IsTransfer = true,
                                                                    Code = "315"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Кловська",
                                                                    IsTransfer = false,
                                                                    Code = "316"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Печерська",
                                                                    IsTransfer = false,
                                                                    Code = "317"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Дружби народів",
                                                                    IsTransfer = false,
                                                                    Code = "318"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Видубичі",
                                                                    IsTransfer = false,
                                                                    Code = "319"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Славутич",
                                                                    IsTransfer = false,
                                                                    Code = "321"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Осокорки",
                                                                    IsTransfer = false,
                                                                    Code = "322"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Позняки",
                                                                    IsTransfer = false,
                                                                    Code = "323"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Харківська",
                                                                    IsTransfer = false,
                                                                    Code = "324"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Вирлиця",
                                                                    IsTransfer = false,
                                                                    Code = "325"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Бориспільська",
                                                                    IsTransfer = false,
                                                                    Code = "326"
                                                                },
                                                                new MetroStation
                                                                {
                                                                    Name = "Червоний хутір",
                                                                    IsTransfer = false,
                                                                    Code = "327"
                                                                },
                                                            }
                                                        }
                                                    }
                                                }
                                            },
                                            Universities = new List<University>
                                            {
                                                new University
                                                {
                                                    Address = "Україна, м. Київ, вул. Соломянська 7",
                                                    CodeEDBO = "82",
                                                    Emblem = "",
                                                    FullName = "Державний Університет Телекомунікацій",
                                                    Image = "",
                                                    Lat = 1,
                                                    Long = 2,
                                                    ShortName = "ДУТ",
                                                    Faculties = new List<Faculty>
                                                    {
                                                        new Faculty
                                                        {
                                                            Name = "Навчально-Науковий Інститут Інформаційних Технологій (ННІІТ)",
                                                            Specialties = new List<Specialty>
                                                            {
                                                                new Specialty
                                                                {
                                                                    Code = "121",
                                                                    Name = "Інженерія Програмного Забезпечення"
                                                                },
                                                                new Specialty
                                                                {
                                                                    Code = "122",
                                                                    Name = "Компютерні Науки"
                                                                },
                                                                new Specialty
                                                                {
                                                                    Code = "123",
                                                                    Name = "Компютерна Інженерія"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                });

                await _databaseDb.SaveChangesAsync();
            }
        }
    }
}
