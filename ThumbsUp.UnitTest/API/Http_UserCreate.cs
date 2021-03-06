﻿#region Using

using FakeItEasy;
using Nancy;
using Nancy.Helper;
using Nancy.Testing;
using Shouldly;
using System.Collections.Generic;
using System.Configuration;
using ThumbsUp.Domain;
using ThumbsUp.Service;
using ThumbsUp.Service.Module;
using Xunit;

#endregion

namespace ThumbsUp.UnitTest.API
{
	public class Http_UserCreate : _BaseTest
	{
		[Fact]
		public void Should_return_password_when_user_is_created()
		{
			// Given
			var passwordLength = int.Parse(ConfigurationManager.AppSettings["ThumbsUp.PasswordCharacters.Count"]);
			User userToLoad = null;
			var fakeUserService = MakeFake.UserService(userToLoad);
			A.CallTo(() => fakeUserService.CreateUser(A<string>.Ignored, A<string>.Ignored)).Returns(new string('*', passwordLength));
			var userTestBrowser = MakeTestBrowser<UserModule>(fakeUserService: fakeUserService);

			// When
			var result = userTestBrowser.Post("/user/create", with =>
			{
				with.HttpRequest();
				with.FormValue("username", "<username>");
				with.FormValue("email", "valid@email.com");
			});

			// Then
			result.StatusCode.ShouldBe(HttpStatusCode.OK);

			var payload = result.Body.DeserializeJson<Dictionary<string, object>>();
			payload.ContainsItems("Password").ShouldBe(true);
			payload["Password"].ToString().Length.ShouldBe(passwordLength);
		}

		#region Errors

		[Fact]
		public void Should_return_MissingParameters_error_when_endpoint_is_hit_with_missing_params()
		{
			TestMissingParams<UserModule>("/user/create");
		}

		[Fact]
		public void Should_return_InvalidParameters_error_when_endpoint_is_hit_with_invalid_params()
		{
			//Given
			var formValues = new Dictionary<string, string>() { { "username", MakeFake.Username }, { "email", MakeFake.InvalidEmail } };
			User userToLoad = null;
			var fakeUserService = MakeFake.UserService(userToLoad);
			var userTestBrowser = MakeTestErrorBrowser<UserModule>(fakeUserService: fakeUserService);
			TestInvalidParams<UserModule>("/user/create", formValues, browser: userTestBrowser);
		}

		[Fact]
		public void Should_return_UserNameTaken_error_when_user_is_created_with_existing_username()
		{
			// Given
			User userToLoad = new User();
			var fakeUserService = MakeFake.UserService(userToLoad);
			var userTestBrowser = MakeTestErrorBrowser<UserModule>(fakeUserService: fakeUserService);

			// When
			var result = userTestBrowser.Post("/user/create", with =>
			{
				with.HttpRequest();
				with.FormValue("username", "<username>");
				with.FormValue("email", "valid@email.com");
			});

			// Then
			result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

			var payload = result.Body.DeserializeJson<Dictionary<string, object>>();
			payload.ContainsItems("ErrorCode", "ErrorMessage").ShouldBe(true);
			payload["ErrorCode"].ShouldBe((int)ErrorCode.UserNameTaken);
		}
		#endregion
	}
}