using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public static class UserService
    {
        public static async Task UpdateRecordAsync(UserInfo oldUser, UserInfo newUser) =>
            await Task.Run(() => UserDB.UpdateUserRecord(oldUser, newUser));

        public static async Task<UserInfo> AuthorizeUserAsync(string login, string password) =>
            await Task.Run(() => UserDB.AuthorizeUser(login, password));
        public static async Task<UserInfo> AuthorizeUserAsync(string login, byte[] faceID) =>
            await Task.Run(() => UserDB.AuthorizeUserWithFaceID(login, faceID));

        public static async Task CreateNewRecord(UserInfo newUser) =>
            await Task.Run(() => UserDB.CreateNewUser(newUser));

        public static async Task<bool> CheckLoginAvailability(string login) =>
            await Task.Run(() => UserDB.CheckLoginAvailability(login));

        public static async Task<List<int>> GetUserIDs(string[] userLogins) =>
            await Task.Run(() => UserDB.GetUserIDs(userLogins));
    }
}
