using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using ServerLib;

namespace TrustFrontend
{
    public class UserProfilePageModel
    {
        public UserProfilePageModel(UserInfo user)
        {
            HeaderName = user.Name;
            HeaderSurname = user.Surname;
            HeaderFamilyName = user.FName;

            UserData = new ObservableCollection<DataCell>()
            {
                new DataCell
                {
                    PropertyName = "Email",
                    PropertyValue = user.Email
                },
                new DataCell
                {
                    PropertyName = "Code word",
                    PropertyValue = user.CodeWord
                },
            };
        }

        public string HeaderName { get; set; } = "Евгений";
        public string HeaderSurname { get; set; } = "Степанов";
        public string HeaderFamilyName { get; set; } = "Вадимович";

        public ObservableCollection<DataCell> UserData { get; set; } =
            new ObservableCollection<DataCell>()
            {
                new DataCell
                {
                    PropertyName = "Email",
                    PropertyValue = "asldasldasdl;asd"
                },
                new DataCell
                {
                    PropertyName = "Email",
                    PropertyValue = "asldasldasdl;asd"
                },
                new DataCell
                {
                    PropertyName = "Email",
                    PropertyValue = "asldasldasdl;asd"
                },
                new DataCell
                {
                    PropertyName = "Email",
                    PropertyValue = "asldasldasdl;asd"
                },
            };
    }
}
