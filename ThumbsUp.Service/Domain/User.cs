﻿using Nancy.Security;
using System;
using System.Collections.Generic;

namespace ThumbsUp.Domain
{
	public class User
	{		
		public string Id { get; set; }
		public string Salt { get; set; }
		public string PasswordHash { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
	}
}
