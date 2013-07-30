﻿#region Using
using Shouldly;
using SimpleCrypto;
using System;
using ThumbsUp.Service;
using ThumbsUp.Service.Domain;
using Xunit;
using Xunit.Extensions;

#endregion

namespace ThumbsUp.UnitTest.Services
{
	public class PasswordService_IsForgotPasswordTokenValid : _BaseServiceTest
	{
		[Fact]
		public void Should_return_true_when_password_matches_user_password()
		{
			// Given
			var token = ValidGuid;
			var fakeUser = new User() { ForgotPasswordRequestToken= token, ForgotPasswordRequestDate = DateTime.Now };
			var passwordService = new PasswordService(null);

			// When
			var isValid = passwordService.IsForgotPasswordTokenValid(fakeUser, token);

			// Then
			isValid.ShouldBe(true);
		}

		[Fact]
		public void Should_return_false_when_minutes_elapsed_since_the_token_was_requested_is_greater_than_the_ForgotPasswordTimeLimit()
		{
			// Given
			var token = ValidGuid;
			var requestDate = DateTime.MinValue;
			var fakeUser = new User() { ForgotPasswordRequestToken = token, ForgotPasswordRequestDate = requestDate };
			var passwordService = new PasswordService(null);

			// When
			var isValid = passwordService.IsForgotPasswordTokenValid(fakeUser, token);

			// Then
			isValid.ShouldBe(false);
		}

		public void Should_return_false_when_user_is_missing()
		{
			// Given
			var token = ValidGuid;
			User fakeUser = null;
			var passwordService = new PasswordService(null);

			// When
			var isValid = passwordService.IsForgotPasswordTokenValid(fakeUser, token);

			// Then
			isValid.ShouldBe(false);
		}

		[
			Theory(),
			InlineData(""),
			InlineData(InvalidGuid),
		]
		public void Should_return_false_when_token_is_missing_or_invalid(string token)
		{
			// Given
			var fakeUser = new User();
			var passwordService = new PasswordService(null);

			// When
			var isValid = passwordService.IsPasswordValid(fakeUser, token);

			// Then
			isValid.ShouldBe(false);
		}


	}
}