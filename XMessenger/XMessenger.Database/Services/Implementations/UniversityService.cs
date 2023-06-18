using Microsoft.EntityFrameworkCore;
using XMessenger.Database.Db.Context;
using XMessenger.Database.Dtos;
using XMessenger.Database.Services.Interfaces;
using XMessenger.Database.ViewModels;

namespace XMessenger.Database.Services.Implementations
{
    public class UniversityService : IUniversityService
    {
        private readonly DatabaseContext _db;
        public UniversityService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<Result<FacultyViewModel>> CreateFacultyAsync(FacultyDto faculty)
        {
            if (!await _db.Universities.AnyAsync(s => s.Id == faculty.UniversityId))
                return Result<FacultyViewModel>.NotFound();

            if (await _db.Faculties.AnyAsync(s => s.Name.ToLower() == faculty.Name.ToLower() && s.UniversityId == faculty.UniversityId))
                return Result<FacultyViewModel>.Error("Faculty already exist");

            var newFaculty = new Faculty
            {
                Name = faculty.Name,
                UniversityId = faculty.UniversityId
            };

            await _db.Faculties.AddAsync(newFaculty);
            await _db.SaveChangesAsync();

            return Result<FacultyViewModel>.SuccessWithData(new FacultyViewModel
            {
                Id = newFaculty.Id,
                Name = faculty.Name
            });
        }

        public async Task<Result<SpecialtyViewModel>> CreateSpecialtyAsync(SpecialtyDto specialty)
        {
            if (!await _db.Faculties.AnyAsync(s => s.Id == specialty.FacultyId))
                return Result<SpecialtyViewModel>.NotFound();

            if (await _db.Specialties.AnyAsync(s => s.Name.ToLower() == specialty.Name.ToLower() && s.FacultyId == specialty.FacultyId))
                return Result<SpecialtyViewModel>.Error("Specialty already exist");

            var newSpecialty = new Specialty
            {
                Name = specialty.Name,
                FacultyId = specialty.FacultyId
            };

            await _db.Specialties.AddAsync(newSpecialty);
            await _db.SaveChangesAsync();

            return Result<SpecialtyViewModel>.SuccessWithData(new SpecialtyViewModel
            {
                Id = newSpecialty.Id,
                Name = newSpecialty.Name
            });
        }

        public async Task<Result<UniversityViewModel>> CreateUniversityAsync(UniversityDto university)
        {
            if (!await _db.Settlements.AnyAsync(s => s.Id == university.SettlementId))
                return Result<UniversityViewModel>.NotFound();

            if (await _db.Universities.AnyAsync(s => s.FullName.ToLower() == university.FullName.ToLower() && s.SettlementId == university.SettlementId))
                return Result<UniversityViewModel>.Error("University already exist");

            var newSpecialty = new University
            {
                Address = university.Address,
                CodeEDBO = university.CodeEDBO,
                Emblem = university.Emblem,
                FullName = university.FullName,
                Image = university.Image,
                Lat = university.Lat,
                Long = university.Long,
                ShortName = university.ShortName,
                SettlementId = university.SettlementId
            };

            await _db.Universities.AddAsync(newSpecialty);
            await _db.SaveChangesAsync();

            return Result<UniversityViewModel>.SuccessWithData(new UniversityViewModel
            {
                Id = newSpecialty.Id,
                Address = newSpecialty.Address,
                CodeEDBO = newSpecialty.CodeEDBO,
                Emblem = newSpecialty.Emblem,
                FullName = newSpecialty.FullName,
                Image = newSpecialty.Image,
                Lat = newSpecialty.Lat,
                Long = newSpecialty.Long,
                ShortName = newSpecialty.ShortName
            });
        }

        public async Task<Result<FacultyViewModel>> EditFacultyAsync(FacultyDto faculty)
        {
            var facultyForEdit = await _db.Faculties.AsNoTracking().FirstOrDefaultAsync(s => s.Id == faculty.Id);
            if (facultyForEdit == null)
                return Result<FacultyViewModel>.NotFound();

            facultyForEdit.Name = faculty.Name;
            facultyForEdit.UniversityId = faculty.UniversityId;

            _db.Faculties.Update(facultyForEdit);
            await _db.SaveChangesAsync();

            return Result<FacultyViewModel>.SuccessWithData(new FacultyViewModel
            {
                Id = facultyForEdit.Id,
                Name = facultyForEdit.Name,
            });
        }

        public async Task<Result<SpecialtyViewModel>> EditSpecialtyAsync(SpecialtyDto specialty)
        {
            var specialtyForEdit = await _db.Specialties.AsNoTracking().FirstOrDefaultAsync(s => s.Id == specialty.Id);
            if (specialtyForEdit == null)
                return Result<SpecialtyViewModel>.NotFound();

            specialtyForEdit.Name = specialty.Name;
            specialtyForEdit.FacultyId = specialty.FacultyId;

            _db.Specialties.Update(specialtyForEdit);
            await _db.SaveChangesAsync();

            return Result<SpecialtyViewModel>.SuccessWithData(new SpecialtyViewModel
            {
                Id = specialtyForEdit.Id,
                Name = specialtyForEdit.Name,
            });
        }

        public async Task<Result<UniversityViewModel>> EditUniversityAsync(UniversityDto university)
        {
            var universityForEdit = await _db.Universities.AsNoTracking().FirstOrDefaultAsync(s => s.Id == university.Id);
            if (universityForEdit == null)
                return Result<UniversityViewModel>.NotFound();

            universityForEdit.FullName = university.FullName;
            universityForEdit.Address = university.Address;
            universityForEdit.ShortName = university.ShortName;
            universityForEdit.CodeEDBO = university.CodeEDBO;
            universityForEdit.Emblem = university.Emblem;
            universityForEdit.Image = university.Image;
            universityForEdit.Lat = university.Lat;
            universityForEdit.Long = university.Long;
            universityForEdit.SettlementId = university.SettlementId;

            _db.Universities.Update(universityForEdit);
            await _db.SaveChangesAsync();

            return Result<UniversityViewModel>.SuccessWithData(new UniversityViewModel
            {
                Id = universityForEdit.Id,
                FullName = universityForEdit.FullName,
                ShortName = universityForEdit.ShortName,
                Address = universityForEdit.Address,
                CodeEDBO = universityForEdit.CodeEDBO,
                Emblem = universityForEdit.Emblem,
                Image = universityForEdit.Image,
                Lat = universityForEdit.Lat,
                Long = universityForEdit.Long
            });
        }

        public async Task<Result<List<FacultyViewModel>>> GetAllFacultiesAsync(int universityId)
        {
            var faculties = await _db.Faculties.AsNoTracking().Where(s => s.UniversityId == universityId).OrderBy(s => s.Name).ToListAsync();

            return Result<List<FacultyViewModel>>.SuccessList(faculties.Select(x => new FacultyViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList(), Meta.FromMeta(faculties.Count, 1));
        }

        public async Task<Result<List<SpecialtyViewModel>>> GetAllSpecialtiesAsync(int facultyId)
        {
            var specialties = await _db.Specialties.AsNoTracking().Where(s => s.FacultyId == facultyId).OrderBy(s => s.Name).ToListAsync();
            return Result<List<SpecialtyViewModel>>.SuccessList(specialties.Select(x => new SpecialtyViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(), Meta.FromMeta(specialties.Count, 1));
        }

        public async Task<Result<List<UniversityViewModel>>> GetAllUniversitiesAsync(int settlementId)
        {
            var universities = await _db.Universities.AsNoTracking().Where(s => s.SettlementId == settlementId).OrderBy(s => s.FullName).ToListAsync();
            return Result<List<UniversityViewModel>>.SuccessList(universities.Select(x => new UniversityViewModel
            {
                Id = x.Id,
                FullName = x.FullName,
                Address = x.Address,
                CodeEDBO = x.CodeEDBO,
                Emblem = x.Emblem,
                Image = x.Image,
                Lat = x.Lat,
                Long = x.Long,
                ShortName = x.ShortName
            }).ToList(), Meta.FromMeta(universities.Count, 1));
        }

        public async Task<Result<UniversityViewModel>> GetUniversityByIdAsync(int universityId)
        {
            var university = await _db.Universities
                .AsNoTracking()
                .Include(s => s.Faculties)
                .ThenInclude(s => s.Specialties)
                .FirstOrDefaultAsync(s => s.Id == universityId);

            if (university == null)
                return Result<UniversityViewModel>.NotFound();

            return Result<UniversityViewModel>.SuccessWithData(new UniversityViewModel
            {
                Id = university.Id,
                FullName = university.FullName,
                Address = university.Address,
                CodeEDBO = university.CodeEDBO,
                Emblem = university.Emblem,
                Image = university.Image,
                Lat = university.Lat,
                Long = university.Long,
                ShortName = university.ShortName,
                Faculties = university.Faculties?.Select(s => new FacultyViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Specialties = s.Specialties?.Select(x => new SpecialtyViewModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList()
                }).ToList()
            });
        }

        public async Task<Result<FacultyViewModel>> RemoveFacultyAsync(int facultyId)
        {
            var facultyToRemove = await _db.Faculties.AsNoTracking().FirstOrDefaultAsync(s => s.Id == facultyId);
            if (facultyToRemove == null)
                return Result<FacultyViewModel>.NotFound();

            _db.Faculties.Remove(facultyToRemove);
            await _db.SaveChangesAsync();
            return Result<FacultyViewModel>.Success();
        }

        public async Task<Result<SpecialtyViewModel>> RemoveSpecialtyAsync(int specialtyId)
        {
            var specialtyToRemove = await _db.Specialties.AsNoTracking().FirstOrDefaultAsync(s => s.Id == specialtyId);
            if (specialtyToRemove == null)
                return Result<SpecialtyViewModel>.NotFound();

            _db.Specialties.Remove(specialtyToRemove);
            await _db.SaveChangesAsync();
            return Result<SpecialtyViewModel>.Success();
        }

        public async Task<Result<UniversityViewModel>> RemoveUniversityAsync(int universityId)
        {
            var universityToRemove = await _db.Universities.AsNoTracking().FirstOrDefaultAsync(s => s.Id == universityId);
            if (universityToRemove == null)
                return Result<UniversityViewModel>.NotFound();

            _db.Universities.Remove(universityToRemove);
            await _db.SaveChangesAsync();
            return Result<UniversityViewModel>.Success();
        }
    }
}
