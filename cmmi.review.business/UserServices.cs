using System;
using System.Collections.Generic;
using cmmi.review.entities;
using cmmi.review.data;
using System.Linq;
using AutoMapper;
using System.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace cmmi.review.business
{
    public class UserServices : IUserServices
    {
        private readonly UnitOfWork _unitOfWork;

        public UserServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public int CreateUser(UserEntity userEntity, string password)
        {
            using (var scope = new TransactionScope())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<UserEntity, User>());
                var user = Mapper.Map<UserEntity, User>(userEntity);

                var salt = ComputeSalt(password);
                var hash = ComputeHash(password, salt);

                user.PasswordHash = Convert.ToBase64String(hash);
                user.PasswordSalt = Convert.ToBase64String(salt);

                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();

                return user.Id;
            }
        }

        public bool DeleteUser(int userId)
        {
            // Does not actually delete to avoid index fragmentation
            var user = GetUserById(userId);
            if (user != null)
            {
                user.Deleted = true;
                return UpdateUser(user.Id, user, "");
            }

            return false;
        }

        public ICollection<UserEntity> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetMany(u => !u.Deleted).ToList();

            if (users.Any())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<User, UserEntity>());
                var usersEntity = Mapper.Map<List<User>, List<UserEntity>>(users);
                return usersEntity;
            }

            return null;
        }

        public UserEntity GetUserById(int userId)
        {
            var user = _unitOfWork.UserRepository.GetByID(userId);

            if (user != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<User, UserEntity>());
                var userEntity = Mapper.Map<User, UserEntity>(user);
                return userEntity;
            }

            return null;
        }

        public bool UpdateUser(int userId, UserEntity userEntity, string password)
        {
            var success = false;
            if (userEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var user = _unitOfWork.UserRepository.GetByID(userId);
                    if (user != null)
                    {
                        user.FirstName = userEntity.FirstName;
                        user.LastName = userEntity.LastName;
                        user.Username = userEntity.Username;
                        user.RememberMe = userEntity.RememberMe;
                        user.Locked = userEntity.Locked;
                        user.ForcePasswordChange = userEntity.ForcePasswordChange;
                        user.Deleted = userEntity.Deleted;

                        if (password != "")
                        {
                            var salt = ComputeSalt(password);
                            var hash = ComputeHash(password, salt);

                            user.PasswordHash = Convert.ToBase64String(hash);
                            user.PasswordSalt = Convert.ToBase64String(salt);
                        }

                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                        scope.Complete();

                        success = true;
                    }
                }
            }
            return success;
        }

        #region Password Hashing
        private const string hashAlgorithm = "SHA256";

        private byte[] ComputeSalt(string password)
        {
            byte[] passwordSalt;

            // Define min and max salt sizes.
            int minSaltSize = 32;
            int maxSaltSize = 64;

            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            passwordSalt = new byte[saltSize];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(passwordSalt);

            return passwordSalt;
        }

        private byte[] ComputeHash(string password, byte[] passwordSalt)
        {
            // Convert plain text into a byte array.
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Allocate array, which will hold plain text and salt.
            byte[] passwordWithSaltBytes =
                    new byte[passwordBytes.Length + passwordSalt.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < passwordBytes.Length; i++)
                passwordWithSaltBytes[i] = passwordBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < passwordSalt.Length; i++)
                passwordWithSaltBytes[passwordBytes.Length + i] = passwordSalt[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(passwordWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] passwordHash = new byte[hashBytes.Length +
                                                passwordSalt.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                passwordHash[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < passwordSalt.Length; i++)
                passwordHash[hashBytes.Length + i] = passwordSalt[i];

            // Convert result into a base64-encoded string.

            return passwordHash;

        }

        #endregion
    }
}
