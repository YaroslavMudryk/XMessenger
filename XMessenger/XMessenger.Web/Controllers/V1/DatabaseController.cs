using Microsoft.AspNetCore.Mvc;
using XMessenger.Database.Dtos;
using XMessenger.Database.Services;

namespace XMessenger.Web.Controllers.V1
{
    [ApiVersion("1.0")]
    public class DatabaseController : ApiBaseController
    {
        private readonly ICountryService _countryService;
        private readonly IRegionService _regionService;
        private readonly ISettlementService _settlementService;
        private readonly IUniversityService _universityService;
        private readonly IMetroService _metroService;
        public DatabaseController(ICountryService countryService, IRegionService regionService, ISettlementService settlementService, IUniversityService universityService, IMetroService metroService)
        {
            _countryService = countryService;
            _regionService = regionService;
            _settlementService = settlementService;
            _universityService = universityService;
            _metroService = metroService;
        }


        #region Read

        [HttpGet("countries")]
        public async Task<IActionResult> GetAllContries()
        {
            return JsonResult(await _countryService.GetAllContriesAsync());
        }

        [HttpGet("countries/{id}/regions")]
        public async Task<IActionResult> GetAllRegions(int id)
        {
            return JsonResult(await _regionService.GetAllRegionsAsync(id));
        }

        [HttpGet("regions/{regionId}/settlements")]
        public async Task<IActionResult> GetAllSettlements(int regionId, int page = 1)
        {
            return JsonResult(await _settlementService.GetAllSettlementsAsync(regionId, page));
        }

        [HttpGet("settlements/{settlementId}/universities")]
        public async Task<IActionResult> GetAllUniversities(int settlementId)
        {
            return JsonResult(await _universityService.GetAllUniversitiesAsync(settlementId));
        }

        [HttpGet("universities/{universityId}")]
        public async Task<IActionResult> GetUniversityDetails(int universityId)
        {
            return JsonResult(await _universityService.GetUniversityByIdAsync(universityId));
        }

        [HttpGet("universities/{universityId}/faculties")]
        public async Task<IActionResult> GetAllFaculties(int universityId)
        {
            return JsonResult(await _universityService.GetAllFacultiesAsync(universityId));
        }

        [HttpGet("faculties/{facultyId}/specialties")]
        public async Task<IActionResult> GetAllSpecialties(int facultyId)
        {
            return JsonResult(await _universityService.GetAllSpecialtiesAsync(facultyId));
        }

        [HttpGet("settlements/{settlementId}/metro")]
        public async Task<IActionResult> GetAllMetro(int settlementId)
        {
            return JsonResult(await _metroService.GetAllMetroAsync(settlementId));
        }

        [HttpGet("metro/{id}")]
        public async Task<IActionResult> GetMetro(int id)
        {
            return JsonResult(await _metroService.GetMetroAsync(id));
        }

        [HttpGet("metro/{id}/lines")]
        public async Task<IActionResult> GetAllLines(int id)
        {
            return JsonResult(await _metroService.GetAllMetroLinesAsync(id));
        }

        [HttpGet("lines/{id}/stations")]
        public async Task<IActionResult> GetAllStations(int id)
        {
            return JsonResult(await _metroService.GetAllMetroStationsAsync(id));
        }

        #endregion

        #region Country
        [HttpPost("countries")]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDto country)
        {
            return JsonResult(await _countryService.CreateAsync(country));
        }

        [HttpPut("countries/{id}")]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] CountryDto country)
        {
            country.Id = id;
            return JsonResult(await _countryService.EditAsync(country));
        }

        [HttpDelete("countries/{id}")]
        public async Task<IActionResult> RemoveCountry(int id)
        {
            return JsonResult(await _countryService.RemoveAsync(id));
        }
        #endregion

        #region Regions
        [HttpPost("regions")]
        public async Task<IActionResult> CreateRegion([FromBody] RegionDto region)
        {
            return JsonResult(await _regionService.CreateAsync(region));
        }

        [HttpPut("regions/{id}")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] RegionDto region)
        {
            region.Id = id;
            return JsonResult(await _regionService.EditAsync(region));
        }

        [HttpDelete("regions/{id}")]
        public async Task<IActionResult> RemoveRegion(int id)
        {
            return JsonResult(await _regionService.RemoveAsync(id));
        }
        #endregion

        #region Settlements

        [HttpPost("settlements")]
        public async Task<IActionResult> CreateSettlement([FromBody] SettlementDto settlement)
        {
            return JsonResult(await _settlementService.CreateSettlementAsync(settlement));
        }

        [HttpPut("settlements/{id}")]
        public async Task<IActionResult> EditSettlement(int id, [FromBody] SettlementDto settlement)
        {
            settlement.Id = id;
            return JsonResult(await _settlementService.EditSettlementAsync(settlement));
        }

        [HttpDelete("settlements/{id}")]
        public async Task<IActionResult> RemoveSettlement(int id)
        {
            return JsonResult(await _settlementService.RemoveSettlementAsync(id));
        }

        #endregion

        #region University

        [HttpPost("universities")]
        public async Task<IActionResult> CreateUniversity([FromBody] UniversityDto university)
        {
            return JsonResult(await _universityService.CreateUniversityAsync(university));
        }

        [HttpPut("universities/{id}")]
        public async Task<IActionResult> EditUniversity(int id, [FromBody] UniversityDto university)
        {
            university.Id = id;
            return JsonResult(await _universityService.EditUniversityAsync(university));
        }

        [HttpDelete("universities/{id}")]
        public async Task<IActionResult> RemoveUniversity(int id)
        {
            return JsonResult(await _universityService.RemoveUniversityAsync(id));
        }

        [HttpPost("faculties")]
        public async Task<IActionResult> CreateFaculty([FromBody] FacultyDto faculty)
        {
            return JsonResult(await _universityService.CreateFacultyAsync(faculty));
        }

        [HttpPut("faculties/{id}")]
        public async Task<IActionResult> EditUniversity(int id, [FromBody] FacultyDto faculty)
        {
            faculty.Id = id;
            return JsonResult(await _universityService.EditFacultyAsync(faculty));
        }

        [HttpDelete("faculties/{id}")]
        public async Task<IActionResult> RemoveFaculty(int id)
        {
            return JsonResult(await _universityService.RemoveFacultyAsync(id));
        }


        [HttpPost("specialties")]
        public async Task<IActionResult> CreateSpecialty([FromBody] SpecialtyDto specialty)
        {
            return JsonResult(await _universityService.CreateSpecialtyAsync(specialty));
        }

        [HttpPut("specialties/{id}")]
        public async Task<IActionResult> EditSpecialty(int id, [FromBody] SpecialtyDto specialty)
        {
            specialty.Id = id;
            return JsonResult(await _universityService.EditSpecialtyAsync(specialty));
        }

        [HttpDelete("specialties/{id}")]
        public async Task<IActionResult> RemoveSpecialty(int id)
        {
            return JsonResult(await _universityService.RemoveSpecialtyAsync(id));
        }


        #endregion

        #region Metro

        [HttpPost("metro")]
        public async Task<IActionResult> CreateMetro([FromBody] MetroDto metro)
        {
            return JsonResult(await _metroService.CreateMetroAsync(metro));
        }

        [HttpPut("metro/{id}")]
        public async Task<IActionResult> EditMetro(int id, [FromBody] MetroDto metro)
        {
            metro.Id = id;
            return JsonResult(await _metroService.EditMetroAsync(metro));
        }

        [HttpDelete("metro/{id}")]
        public async Task<IActionResult> DeleteMetro(int id)
        {
            return JsonResult(await _metroService.RemoveMetroAsync(id));
        }

        [HttpPost("metro-lines")]
        public async Task<IActionResult> CreateMetroLine([FromBody] MetroLineDto metro)
        {
            return JsonResult(await _metroService.CreateMetroLineAsync(metro));
        }

        [HttpPut("metro-lines/{id}")]
        public async Task<IActionResult> EditMetroLine(int id, [FromBody] MetroLineDto metro)
        {
            metro.Id = id;
            return JsonResult(await _metroService.EditMetroLineAsync(metro));
        }

        [HttpDelete("metro-lines/{id}")]
        public async Task<IActionResult> DeleteMetroLine(int id)
        {
            return JsonResult(await _metroService.RemoveMetroLineAsync(id));
        }

        [HttpPost("metro-stations")]
        public async Task<IActionResult> CreateMetroStation([FromBody] MetroStationDto metro)
        {
            return JsonResult(await _metroService.CreateMetroStationAsync(metro));
        }

        [HttpPut("metro-stations/{id}")]
        public async Task<IActionResult> EditMetroStation(int id, [FromBody] MetroStationDto metro)
        {
            metro.Id = id;
            return JsonResult(await _metroService.EditMetroStationAsync(metro));
        }

        [HttpDelete("metro-stations/{id}")]
        public async Task<IActionResult> DeleteMetroStation(int id)
        {
            return JsonResult(await _metroService.RemoveMetroStationAsync(id));
        }

        #endregion
    }
}
