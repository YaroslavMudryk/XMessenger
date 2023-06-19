namespace XMessenger.Database.Services.Interfaces
{
    public interface IUniversityService
    {
        Task<Result<List<UniversityViewModel>>> GetAllUniversitiesAsync(int settlementId);
        Task<Result<List<FacultyViewModel>>> GetAllFacultiesAsync(int universityId);
        Task<Result<List<SpecialtyViewModel>>> GetAllSpecialtiesAsync(int facultyId);
        Task<Result<UniversityViewModel>> GetUniversityByIdAsync(int universityId);


        Task<Result<UniversityViewModel>> CreateUniversityAsync(UniversityDto university);
        Task<Result<UniversityViewModel>> EditUniversityAsync(UniversityDto university);
        Task<Result<UniversityViewModel>> RemoveUniversityAsync(int universityId);

        Task<Result<FacultyViewModel>> CreateFacultyAsync(FacultyDto faculty);
        Task<Result<FacultyViewModel>> EditFacultyAsync(FacultyDto faculty);
        Task<Result<FacultyViewModel>> RemoveFacultyAsync(int facultyId);

        Task<Result<SpecialtyViewModel>> CreateSpecialtyAsync(SpecialtyDto specialty);
        Task<Result<SpecialtyViewModel>> EditSpecialtyAsync(SpecialtyDto specialty);
        Task<Result<SpecialtyViewModel>> RemoveSpecialtyAsync(int specialtyId);
    }
}
