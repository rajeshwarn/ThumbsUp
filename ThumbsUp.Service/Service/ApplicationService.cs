﻿using Raven.Client;
using SimpleCrypto;
using System;
using ThumbsUp.Domain;
using ThumbsUp.Raven;

namespace ThumbsUp.Service
{
	public class ApplicationService
	{
		private readonly IDocumentSession Db;
		public ApplicationService(RavenSessionProvider documentSessionProvider)
		{
			Db = documentSessionProvider.Get();
		}

		public Application Create(string name)
		{
			var application = new Application()
			{
				Id = Guid.NewGuid().ToString(),
				Name = name
			};
			Db.Store(application);
			return application;
		}

		public Application Get(string id)
		{
			return Db.Load<Application>(id);
		}

		public bool ApplicationDoesNotExist(string id)
		{
			return Get(id)==null;
		}
	}
}