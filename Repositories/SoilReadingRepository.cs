using IOT.DBContext;
using IOT.DTOs;
using IOT.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IOT.Repositories
{

    public interface ISoilReadingRepository
    {
        Task<IEnumerable<SoilReading>> SoilReadingsAsync();
        Task<IEnumerable<SoilReading>> SoilReadingsAsyncByUserId(int UserId);
        Task<bool> AddSoilReadingAsync(MultiSensorPayloadDto multiSensorPayloadDto);
    }
    public class SoilReadingRepository : ISoilReadingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _ipAddress;
        private readonly string _userId;
        public SoilReadingRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _ipAddress = HttpContextExtensions.GetClientIpAddress(_httpContextAccessor.HttpContext);
            ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
            
            if (user != null)
            {
                _userId = user.GetUserId(); // Using your extension method
            }
        }
        public async Task<IEnumerable<SoilReading>> SoilReadingsAsync()
        {
            {
                return await _context.SoilReadings.ToListAsync();
            }
        }

        public async Task<IEnumerable<SoilReading>> SoilReadingsAsyncByUserId(int UserId)
        {
            return await _context.SoilReadings.Where(s => s.Userid == UserId).ToListAsync();
        }

        public async Task<bool> AddSoilReadingAsync(MultiSensorPayloadDto multiSensorPayloadDto)
        {
            try
            {
                var readingjsonString = JsonConvert.SerializeObject(multiSensorPayloadDto);

                if (!string.IsNullOrEmpty(readingjsonString))
                {
                    SoilReading soilReading = new SoilReading
                    {
                        Userid = Convert.ToInt32(_userId),
                        Moisturevalue = readingjsonString,
                        Createdby = Convert.ToInt32(_userId),
                        Createddate = DateTime.UtcNow,
                        Isactive = 1,
                        Isdelete = 0,
                        Ipaddress = _ipAddress
                    };
                    _context.SoilReadings.Add(soilReading);
                    await _context.SaveChangesAsync(); 

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
