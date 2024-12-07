using System.Text.RegularExpressions;

using SipPOS.Models.Entity;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.DataAccess.Implementations;

class MockStaffDao : IStaffDao
{
    private readonly List<Staff> _staffs =
    [
        new Staff(
            id: 0,
            dto: new StaffDto()
            {
                StoreId = 0,
                PositionPrefix = "SM",
                Name = "Mai Thị Quỳnh",
                Gender = "Nữ",
                DateOfBirth = new DateOnly(1999, 5, 20),
                Email = "quynhnguyen@gmail.com",
                Tel = "0928512665",
                Address = "123 Đường Nguyễn Thị Minh Khai, Phường 9, Quận Tân Bình, Thành phố Hồ Chí Minh",
                EmploymentStatus = "InEmployment",
                EmploymentStartDate = new DateOnly(2021, 3, 29)
            }
        ),
        
        new Staff(
            id: 1,
            dto: new StaffDto()
            {
                StoreId = 0,
                PositionPrefix = "ST",
                Name = "Nguyễn Văn Đức",
                Gender = "Nam",
                DateOfBirth = new DateOnly(2002, 1, 20),
                Email = "vanduc@gmail.com",
                Tel = "0903514265",
                Address = "463 Đường Đồng Văn Cống, Phường 2, Quận Thủ Đức, Thành phố Hồ Chí Minh",
                EmploymentStatus = "OutOfEmployment",
                EmploymentStartDate = new DateOnly(2023, 12, 03),
                EmploymentEndDate = new DateOnly(2024, 5, 15)
            }
        )
    ];

    public Task<StaffDto?> GetByCompositeUsername(string compositeUsername)
    {
        // Splits composite username into (position prefix, store id, staff id)
        Regex re = new Regex(Staff.CompositeUsernamePattern);
        Match match = re.Match(compositeUsername);

        if (!match.Success)
        {
            return Task.FromResult<StaffDto?>(null);
        }

        var positionPrefix = match.Groups[1].Value;
        long storeId;
        long staffId;

        try
        {
            storeId = Int64.Parse(match.Groups[2].Value);
            staffId = Int64.Parse(match.Groups[3].Value);
        }
        catch (FormatException)
        {
            return Task.FromResult<StaffDto?>(null);
        }

        var staff = _staffs.FirstOrDefault(staff => staff.Id == staffId &&
                                                    staff.StoreId == storeId &&
                                                    staff.PositionPrefix == positionPrefix);

        if (staff == null)
        {
            return Task.FromResult<StaffDto?>(null);
        }

        return Task.FromResult<StaffDto?>(new StaffDto
        {
            Id = staff.Id,
            StoreId = staff.StoreId,
            PositionPrefix = staff.PositionPrefix,
            Name = staff.Name,
            Gender = staff.Gender,
            DateOfBirth = staff.DateOfBirth,
            Email = staff.Email,
            Tel = staff.Tel,
            Address = staff.Address,
            EmploymentStatus = staff.EmploymentStatus,
            EmploymentStartDate = staff.EmploymentStartDate,
            EmploymentEndDate = staff.EmploymentEndDate
        });
    }

    // MockStaffDao should not have the methods below implemented, because it is used for the quick-login
    // when developing only

    public Task<(long id, StaffDto? dto)> InsertAsync(long storeId, StaffDto staffDto) => throw new NotImplementedException();
    public Task<List<StaffDto>?> GetAllAsync(long storeId) => throw new NotImplementedException();
    public Task<StaffDto?> GetByIdAsync(long storeId, long id) => throw new NotImplementedException();
    public Task<StaffDto?> UpdateByIdAsync(long storeId, long id, StaffDto updatedStaffDto, Staff author) => throw new NotImplementedException();
    public Task<StaffDto?> DeleteByIdAsync(long storeId, long id, Staff author) => throw new NotImplementedException();
}
