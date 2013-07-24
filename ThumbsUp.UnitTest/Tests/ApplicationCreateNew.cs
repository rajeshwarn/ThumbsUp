﻿#region Using

using FakeItEasy;
using Nancy;
using Nancy.Helper;
using Nancy.Testing;
using Shouldly;
using System;
using System.Collections.Generic;
using ThumbsUp.Service;
using ThumbsUp.Service.Domain;
using ThumbsUp.Service.Module;
using Xunit;

#endregion

namespace ThumbsUp.UnitTest.Tests
{
	public class ApplicationCreateNew : _BaseTest
	{
		[Fact]
		public void Should_return_new_applicationid_when_new_application_is_registered()
		{
			// Given
			var applicationId = Guid.NewGuid().ToString();
			var mockApplicationService = A.Fake<IApplicationService>();
			A.CallTo(() => mockApplicationService.RegisterNew(A<string>.Ignored)).Returns(new Application { Id = applicationId});
			var applicationTestBrowser = MakeTestBrowser<ApplicationModule>(mockApplicationService: mockApplicationService);

			// When
			var result = applicationTestBrowser.Post("/application/register/new", with =>
			{
				with.HttpRequest();
				with.FormValue("name", "New Application");
			});

			// Then
			result.StatusCode.ShouldBe(HttpStatusCode.OK);

			var payload = result.Body.DeserializeJson<Dictionary<string, object>>();
			payload.ContainsItems("ApplicationId").ShouldBe(true);
			payload["ApplicationId"].ShouldBe(applicationId);
		}

		#region Errors
		[Fact]
		public void Should_return_MissingParameters_error_when_new_application_is_registered_with_missing_params()
		{
			TestMissingParams<ApplicationModule>("/application/register/new");
		}
		#endregion
	}
}