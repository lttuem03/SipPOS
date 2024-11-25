using SipPOS.Models.Entity;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.DataAccess.Implementations;

public class MockStoreDao : IStoreDao
{
    private readonly List<Store> _stores =
    [
        new Store(
            id: 0, 
            dto: new StoreDto()
            {
                Name = "Món Ông Cảnh Khéo Quán",
                Address = "123 Đường Mộc Oanh Cửa Khẩu, Phường 9, Quận Tân Bình, Thành phố Hồ Chí Minh",
                Email = "canhkheoquan@gmail.com",
                Tel = "09323654213",
                TaxCode = "7200000010001",
                Username = "mock",
                // Password for authentication: mock
                LastLogin = DateTime.Now
            }
        )
    ];

    public Task<StoreDto?> GetByUsernameAsync(string username)
    {
        var store = _stores.FirstOrDefault(store => store.Username == username);

        if (store == null)
        {
            return Task.FromResult<StoreDto?>(null);
        }

        return Task.FromResult<StoreDto?>(new StoreDto()
        {
            Id = 0,
            Name = store.Name,
            Address = store.Address,
            Email = store.Email,
            Tel = store.Tel,
            TaxCode = store.TaxCode,
            Username = store.Username,
            PasswordHash = "8B7E4258694259386AF545199FDBF898CA9127C0BCAA2E81449F706789AA62A3",
            Salt = "C7FC0185B7C8F6A48C7E00AEE44B1EBF61CB60E4B4C4D62EE97BD07F4DD826DD",
            LastLogin = store.LastLogin
        });
    }

    // MockStoreDao should not have the methods below implemented, because it is used for the quick-login
    // when developing only

    public Task<StoreDto?> DeleteByIdAsync(long id) => throw new NotImplementedException();
    public Task<StoreDto?> DeleteByUsernameAsync(string username) => throw new NotImplementedException();
    public Task<StoreDto?> GetByIdAsync(long id) => throw new NotImplementedException();
    public Task<(long id, StoreDto? dto)> InsertAsync(StoreDto storeDto) => throw new NotImplementedException();
    public Task<StoreDto?> UpdateByIdAsync(long id, StoreDto updatedStoreDto) => throw new NotImplementedException();
    public Task<StoreDto?> UpdateByUsernameAsync(string username, StoreDto updatedStoreDto)
    {
        // The only usage of this method is when the mock store is 
        // authenticated and the last login time is updated
        // so we don't need to check the store by username

        return Task.FromResult<StoreDto?>(new StoreDto(){
            Id = 0,
            Name = updatedStoreDto.Name,
            Address = updatedStoreDto.Address,
            Email = updatedStoreDto.Email,
            Tel = updatedStoreDto.Tel,
            TaxCode = updatedStoreDto.TaxCode,
            Username = updatedStoreDto.Username,
            PasswordHash = "8B7E4258694259386AF545199FDBF898CA9127C0BCAA2E81449F706789AA62A3",
            Salt = "C7FC0185B7C8F6A48C7E00AEE44B1EBF61CB60E4B4C4D62EE97BD07F4DD826DD",
            LastLogin = updatedStoreDto.LastLogin
        });
    }
}