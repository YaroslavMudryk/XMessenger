namespace XMessenger.Database.Seeder
{
    public class DatabaseSeederService : ISeederService
    {
        private readonly string _countryUAPath;
        private readonly DatabaseContext _db;
        public DatabaseSeederService(DatabaseContext db)
        {
            _db = db;
            _countryUAPath = "export_countries_(18_23)_20.06.2023.json";
        }

        public async Task<Result<int>> SeedSystemAsync()
        {
            int counter = 0;
            var json = await File.ReadAllTextAsync(_countryUAPath);

            var countryModel = JsonSerializer.Deserialize<List<CountryModel>>(json);
            var country = countryModel[0].MapFromModel();

            SortNames(country);
            SetUniversitiesAndMetro(country);

            if (!await _db.Countries.AnyAsync(s => s.Name == country.Name))
            {
                await _db.Countries.AddAsync(country);
                await _db.SaveChangesAsync();
                counter++;
            }

            return Result<int>.SuccessWithData(counter);
        }

        private void SortNames(Country country)
        {
            country.Regions.ForEach(region =>
            {
                region.Areas.ForEach(area =>
                {
                    area.Settlements = area.Settlements.OrderBy(s => s.Name).ToList();
                });
                region.Areas = region.Areas.OrderBy(s => s.Name).ToList();
            });
            country.Regions = country.Regions.OrderBy(s => s.Name).ToList();
        }

        private void SetUniversitiesAndMetro(Country country)
        {
            var regionId = "bd8abba813a54113a7889490ecfe4283";
            var areaId = "f66a32fc1c034874b6d667be97caed74";
            var cityId = "238f405b07f6418d93ed1e297cf3eedc";

            var region = country.Regions.FirstOrDefault(s => s.ItemId == regionId);
            var area = region.Areas.FirstOrDefault(s => s.ItemId == areaId);
            var city = area.Settlements.FirstOrDefault(s => s.ItemId == cityId);

            if (city != null)
            {
                city.Metro = new List<Metro>
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
                                Name = "Оболонсько-Теремківська",
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
                                        TransferToCode = "120",
                                        Name = "Майдан Незалежності"
                                    },
                                    new MetroStation
                                    {
                                        Code = "218",
                                        IsTransfer = true,
                                        TransferToCode = "315",
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
                                Name = "Святошинсько-Броварська",
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
                                        TransferToCode = "314",
                                        Code = "119"
                                    },
                                    new MetroStation
                                    {
                                        Name = "Хрещатик",
                                        IsTransfer = true,
                                        TransferToCode = "217",
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
                                Name = "Сирецько-Печерська",
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
                                        Name = "Львівська брама",
                                        IsTransfer = false,
                                        Code = "313"
                                    },
                                    new MetroStation
                                    {
                                        Name = "Золоті ворота",
                                        IsTransfer = true,
                                        TransferToCode = "119",
                                        Code = "314"
                                    },
                                    new MetroStation
                                    {
                                        Name = "Палац спорту",
                                        IsTransfer = true,
                                        TransferToCode = "218",
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
                                        Name = "Теличка",
                                        IsTransfer = false,
                                        Code = "320"
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
                };
                city.Universities = new List<University>
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
                };
            }
        }
    }
}