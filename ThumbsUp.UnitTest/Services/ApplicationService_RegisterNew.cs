﻿#region Using
using FakeItEasy;
using Nancy.Helper;
using Shouldly;
using ThumbsUp.Service;
using ThumbsUp.Service.Domain;
using ThumbsUp.Service.Raven;
using Xunit;
#endregion

namespace ThumbsUp.UnitTest.Services
{
	public class ApplicationService_RegisterNew
	{
		[Fact]
		public void Should_return_new_Application_when_new_application_is_registered()
		{
			// Given
			var applicationName = MakeFake.Name;
			var instanceToLoad = new Application() { Name = applicationName};
			var fakeRavenSessionProvider = MakeFake.RavenSessionProvider<Application>(instanceToLoad);
			var applicationService = new ApplicationService(fakeRavenSessionProvider);

			// When
			var application = applicationService.RegisterNew(applicationName);

			// Then
			application.Name.ShouldBe(applicationName);
			application.Id.IsGuid().ShouldBe(true);
		}

		[Fact]
		public void Should_return_null_when_application_name_is_missing()
		{
			// Given
			var name = string.Empty;
			var applicationService = new ApplicationService(A.Dummy<IRavenSessionProvider>());

			// When
			var application = applicationService.RegisterNew(name);

			// Then
			application.ShouldBe(null);
		}
	}
}
