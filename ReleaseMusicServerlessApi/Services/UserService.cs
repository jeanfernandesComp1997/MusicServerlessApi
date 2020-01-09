using ReleaseMusicServerlessApi.Helpers;
using ReleaseMusicServerlessApi.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReleaseMusicServerlessApi.Services
{
    public class UserService
    {
        private MongoDBService<User> mongoDbService;

        public UserService()
        {
            this.mongoDbService = new MongoDBService<User>("user");
        }

        public void CreateUser(dynamic request)
        {
            IList<User> users = this.GetUserByEmail((string)request.email);

            if (users.Count > 0)
                throw new Exception("Email is already use !");

            User user = new User((string)request.email, (string)request.password);
            this.mongoDbService.InsertOne(user);
        }

        public bool LoginUser(dynamic request)
        {
            string email = (string)request.email;
            string password = (string)request.password;

            Expression<Func<User, bool>> filter = x => x.Email == (email) && x.Password == password;
            IList<User> items = this.mongoDbService.ListByFilter(filter);

            if (items.Count > 0)
                return true;
            else
                return false;
        }

        public dynamic GenerateTokenUser(dynamic request)
        {
            if (this.LoginUser(request))
            {
                string key1 = (string)request.email;
                string key2 = (string)request.password;
                DateTime now = CryptHelper.BrazilEast(DateTime.Now);

                string key1Encrypted = CryptHelper.ToCrypt(key1);
                string key2Encrypted = CryptHelper.ToCrypt(key2);
                string nowEncrypted = CryptHelper.ToCrypt(now.ToString());

                return key1Encrypted + ":" + key2Encrypted + ":" + nowEncrypted;
            }
            else
                throw new Exception("User credentials invalids !");
        }

        public bool CheckToken(string token)
        {
            IList<string> keys = token.Split(":");
            string date = CryptHelper.ToDecrypt(keys[2]);
            
            dynamic keysUser = new
            {
                email = CryptHelper.ToDecrypt(keys[0]),
                password = CryptHelper.ToDecrypt(keys[1])
            };

            bool loginValid = this.LoginUser(keysUser);

            if (loginValid)
            {
                DateTime now = CryptHelper.BrazilEast(DateTime.Now);
                DateTime tokenDate = Convert.ToDateTime(date);

                if (now.Date == tokenDate.Date)
                    return true;
                else
                    throw new Exception("Token expired !");
            }
            else
                throw new Exception("Token invalid !");
        }

        private IList<User> GetUserByEmail(string email)
        {
            Expression<Func<User, bool>> filter = x => x.Email.Contains(email);
            IList<User> items = this.mongoDbService.ListByFilter(filter);

            return items;
        }
    }
}
