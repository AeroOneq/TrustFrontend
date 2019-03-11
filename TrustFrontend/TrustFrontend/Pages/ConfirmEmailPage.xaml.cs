﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerLib;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrustFrontend
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfirmEmailPage : ContentPage
	{
        private UserInfo NewUser { get; set; }

		public ConfirmEmailPage(UserInfo newUser)
		{
			InitializeComponent ();

            NewUser = newUser;
		}

        private async void CreateNewAcc(object sender, EventArgs e)
        {
            if (Email.CurrentCode == emailCodeEntry.Text)
            {
                try
                {
                    await UserService.CreateNewRecord(NewUser);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка при создании аккаунта", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Код неверный", "OK");
            }
        }

    }
}