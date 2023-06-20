namespace XMessenger.Database.Extensions
{
    public static class ExportImportMappingExtensions
    {
        public static Country MapFromModel(this CountryModel country)
        {
            if (country == null)
                return null;

            return new Country
            {
                Name = country.Name,
                Flag = country.Flag,
                ItemId = country.ItemId,
                Populations = country.Populations,
                Regions = country.Regions != null ? country.Regions.Select(r => new Region
                {
                    Name = r.Name,
                    Flag = r.Flag,
                    ItemId = r.ItemId,
                    Areas = r.Areas != null ? r.Areas.Select(a => new Area
                    {
                        Name = a.Name,
                        Flag = a.Flag,
                        ItemId = a.ItemId,
                        Settlements = a.Settlements != null ? a.Settlements.Select(s => new Settlement
                        {
                            Name = s.Name,
                            Flag = s.Flag,
                            ItemId = s.ItemId,
                            Type = s.Type,
                            OldNames = s.OldNames,
                            Universities = s.Universities != null ? s.Universities.Select(u => new University
                            {
                                FullName = u.FullName,
                                ShortName = u.ShortName,
                                Address = u.Address,
                                Image = u.Image,
                                Emblem = u.Emblem,
                                Lat = u.Lat,
                                Long = u.Long,
                                CodeEDBO = u.CodeEDBO,
                                ItemId = u.ItemId,
                                Faculties = u.Faculties != null ? u.Faculties.Select(f => new Faculty
                                {
                                    Name = f.Name,
                                    ItemId = f.ItemId,
                                    Specialties = f.Specialties != null ? f.Specialties.Select(s => new Specialty
                                    {
                                        Name = s.Name,
                                        Code = s.Code,
                                        ItemId = s.ItemId,
                                    }).ToList() : null
                                }).ToList() : null
                            }).ToList() : null,
                            Metro = s.Metro != null ? s.Metro.Select(m => new Metro
                            {
                                Name = m.Name,
                                Flag = m.Flag,
                                Description = m.Description,
                                ItemId = m.ItemId,
                                Lines = m.Lines != null ? m.Lines.Select(l => new MetroLine
                                {
                                    Name = l.Name,
                                    Color = l.Color,
                                    ItemId = l.ItemId,
                                    Stations = l.Stations != null ? l.Stations.Select(s => new MetroStation
                                    {
                                        Name = s.Name,
                                        Code = s.Code,
                                        IsTransfer = s.IsTransfer,
                                        TransferToCode = s.TransferToCode,
                                        ItemId = s.ItemId
                                    }).ToList() : null,
                                }).ToList() : null,
                            }).ToList() : null
                        }).ToList() : null,
                    }).ToList() : null
                }).ToList() : null
            };
        }

        public static IEnumerable<Country> MapListFromModel(this IEnumerable<CountryModel> countryList)
        {
            return countryList.Select(s => MapFromModel(s));
        }


        public static CountryModel MapToModel(this Country country)
        {
            if (country == null)
                return null;

            return new CountryModel
            {
                Name = country.Name,
                Flag = country.Flag,
                ItemId = country.ItemId,
                Populations = country.Populations,
                Regions = country.Regions != null ? country.Regions.Select(r => new RegionModel
                {
                    Name = r.Name,
                    Flag = r.Flag,
                    ItemId = r.ItemId,
                    Areas = r.Areas != null ? r.Areas.Select(a => new AreaModel
                    {
                        Name = a.Name,
                        Flag = a.Flag,
                        ItemId = a.ItemId,
                        Settlements = a.Settlements != null ? a.Settlements.Select(s => new SettlementModel
                        {
                            Name = s.Name,
                            Flag = s.Flag,
                            ItemId = s.ItemId,
                            Type = s.Type,
                            OldNames = s.OldNames,
                            Universities = s.Universities != null ? s.Universities.Select(u => new UniversityModel
                            {
                                FullName = u.FullName,
                                ShortName = u.ShortName,
                                Address = u.Address,
                                Image = u.Image,
                                Emblem = u.Emblem,
                                Lat = u.Lat,
                                Long = u.Long,
                                CodeEDBO = u.CodeEDBO,
                                ItemId = u.ItemId,
                                Faculties = u.Faculties != null ? u.Faculties.Select(f => new FacultyModel
                                {
                                    Name = f.Name,
                                    ItemId = f.ItemId,
                                    Specialties = f.Specialties != null ? f.Specialties.Select(s => new SpecialtyModel
                                    {
                                        Name = s.Name,
                                        Code = s.Code,
                                        ItemId = s.ItemId,
                                    }).ToList() : null
                                }).ToList() : null
                            }).ToList() : null,
                            Metro = s.Metro != null ? s.Metro.Select(m => new MetroModel
                            {
                                Name = m.Name,
                                Flag = m.Flag,
                                Description = m.Description,
                                ItemId = m.ItemId,
                                Lines = m.Lines != null ? m.Lines.Select(l => new MetroLineModel
                                {
                                    Name = l.Name,
                                    Color = l.Color,
                                    ItemId = l.ItemId,
                                    Stations = l.Stations != null ? l.Stations.Select(s => new MetroStationModel
                                    {
                                        Name = s.Name,
                                        Code = s.Code,
                                        IsTransfer = s.IsTransfer,
                                        TransferToCode = s.TransferToCode,
                                        ItemId = s.ItemId
                                    }).ToList() : null,
                                }).ToList() : null,
                            }).ToList() : null
                        }).ToList() : null,
                    }).ToList() : null
                }).ToList() : null
            };
        }

        public static IEnumerable<CountryModel> MapListToModel(this IEnumerable<Country> countryList)
        {
            return countryList.Select(s => MapToModel(s));
        }
    }
}