using Microsoft.Extensions.Logging;
using Shared.Dtos;
using Shared.Entities;
using Shared.Repositories;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services;

public class UserService(AddressRepository addressRepository, PhoneNumberRepository phoneNumberRepository, ProfileRepository profileRepository, RoleRepository roleRepository, UserRepository userRepository)
{

    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly PhoneNumberRepository _phoneNumberRepository = phoneNumberRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    

    public async Task<bool> CreateUser(UserDto user)
    {
        try
        {
            if (!await CheckIfUserExistsAsync(user.Email))
            {
                var addressEntity = new AddressEntity
                {
                    StreetName = user.StreetName,
                    City = user.City,
                    PostalCode = user.PostalCode,
                };
                await _addressRepository.Create(addressEntity);
                var phoneNumberEntity = new PhoneNumberEntity
                {
                    PhoneNumber = user.PhoneNumber
                };
                await _phoneNumberRepository.Create(phoneNumberEntity);
                var profileEntity = new ProfileEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                await _profileRepository.Create(profileEntity);
                var roleEntity = new RoleEntity
                {
                    RoleName = user.RoleName.ToUpper(),
                };
                await _roleRepository.Create(roleEntity);
                var userEntity = new UserEntity
                {
                    AddressId = addressEntity.AddressId,
                    PhoneNumberId = phoneNumberEntity.PhoneNumberId,
                    ProfileId = profileEntity.ProfileId,
                    RoleId = roleEntity.RoleId,
                    Email = user.Email,
                    Password = GenerateSecurePassword(user.Password)
                };
                await _userRepository.Create(userEntity);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR::" + ex.Message); }
        return false;
        
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var user = new List<UserDto>();
        

        try
        {
            var result = await _userRepository.GetAll();

            foreach (var item in result)
            {
                
                user.Add(new UserDto
                {
                    FirstName = item.Profile.FirstName,
                    LastName = item.Profile.LastName,
                    PhoneNumber = item.PhoneNumber.PhoneNumber,
                    RoleName = item.Role.RoleName,
                    Email = item.Email,
                    StreetName = item.Address.StreetName,
                    PostalCode = item.Address.PostalCode,
                    City = item.Address.City,
                });
                
            }
            return user;
        }

        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
        
    }
    public async Task<bool> UpdateUser(UserDto user)
    {
        
        try
        {
            var userEntityUpdate = _userRepository.GetOne(x => x.Email == user.Email);
            if (userEntityUpdate != null)
            {
                var addressEntity = new AddressEntity
                {
                    StreetName = user.StreetName,
                    City = user.City,
                    PostalCode = user.PostalCode,
                };     
                await _addressRepository.Update(x => x.AddressId == addressEntity.AddressId, addressEntity);
                var phoneNumberEntity = new PhoneNumberEntity
                {
                    PhoneNumber = user.PhoneNumber
                };
                await _phoneNumberRepository.Update(x => x.PhoneNumberId == phoneNumberEntity.PhoneNumberId, phoneNumberEntity);
                var roleEntity = new RoleEntity
                {
                    RoleName = user.RoleName
                };
                await _roleRepository.Update(x => x.RoleId == roleEntity.RoleId, roleEntity);
                var profileEntity = new ProfileEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                await _profileRepository.Update(x => x.ProfileId == profileEntity.ProfileId ,profileEntity);
                var userEntity = new UserEntity
                {
                    Email = user.Email,
                    Password = GenerateSecurePassword(user.Password)
                };
                await _userRepository.Update(x=> x.UserId == userEntity.UserId ,userEntity);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR::" + ex.Message); }
        return false;
    }
    public async Task<bool> DeleteUser(UserDto user)
    {
        try
        {
            var userEntity = await _userRepository.GetOne(x => x.Email == user.Email);
            if (userEntity != null)
            {
                await _addressRepository.Delete(x => x.AddressId == userEntity.AddressId);
                await _phoneNumberRepository.Delete(x => x.PhoneNumberId == userEntity.PhoneNumberId);
                await _profileRepository.Delete(x => x.ProfileId == userEntity.ProfileId);
                await _roleRepository.Delete(x => x.RoleId == userEntity.RoleId);
                await _userRepository.Delete(x => x.UserId == userEntity.UserId);

                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ERROR::" + ex.Message);
        }
        return false;
    }
    public async Task<bool> CheckIfUserExistsAsync(string email)
    {
        if (await _userRepository.Exists(x => x.Email == email))
        {
            Debug.WriteLine($"User with {email} already exists.");
            return true;
        }

        return false;
    }
    public string GenerateSecurePassword(string password)
    {
        using var hmac = new HMACSHA256();
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }
}
