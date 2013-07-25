﻿using Nancy;
using System.Collections.Generic;

namespace ThumbsUp.Service.Domain
{
	public enum ErrorCode 
	{ 
		InternalError,
		InvalidParameters,
		MissingParameters,
		UserNameTaken,
		NoUserForCredentials,
		NoUserForThumbkey
	}

	public interface IErrorService
	{
		Response Generate(IResponseFormatter response, ErrorCode code);
		string Decode(string code);
	}

	public class ErrorService : IErrorService
	{
		private struct Error
		{
			public string Message { get; set; }
			public Nancy.HttpStatusCode StatusCode { get; set; }
		}

		private readonly Dictionary<ErrorCode, Error> Errors = new Dictionary<ErrorCode, Error>{
			{ErrorCode.InternalError, new Error {Message = "Unrecognised internal error", StatusCode = Nancy.HttpStatusCode.BadRequest}},
			{ErrorCode.InvalidParameters, new Error {Message = "One or more required values were invalid", StatusCode = Nancy.HttpStatusCode.BadRequest}},
			{ErrorCode.MissingParameters, new Error {Message = "One or more required values were missing", StatusCode = Nancy.HttpStatusCode.BadRequest}},
			{ErrorCode.UserNameTaken, new Error {Message = "The UserName has already been taken", StatusCode = Nancy.HttpStatusCode.BadRequest}},
			{ErrorCode.NoUserForCredentials, new Error {Message = "No User could be found for the supplied credentials", StatusCode = Nancy.HttpStatusCode.NotFound}},
			{ErrorCode.NoUserForThumbkey, new Error {Message = "No User could be found for the supplied ThumbKey", StatusCode = Nancy.HttpStatusCode.NotFound}}
		};

		public Response Generate(IResponseFormatter response, ErrorCode code)
		{
			var error = (Errors.ContainsKey(code)) ? Errors[code] : Errors[0];
			return response.AsJson(new
			{
				ErrorCode = (int)code,
				ErrorMessage = error.Message
			})
			.WithStatusCode(error.StatusCode);
		}

		public string Decode(string code)
		{
			var key = (ErrorCode)int.Parse(code);
			var error = (Errors.ContainsKey(key)) ? Errors[key] : Errors[0];
			return error.Message;
		}
	}
}