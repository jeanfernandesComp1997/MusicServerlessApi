# MusicServerlessApi
Aws Serverless Lambda API - Search releases musics integrate with spotify

This API was developed for learning purposes

Use the AWS Lambda functions with .NET Core 2.1;
This API it consisits get music releases according to the parameters passed by the user, using API Spotify. 
The DataBase is MongoDb Atlas

 - For launch de API local, you must create a Credentials.cs file in Helpers folder.
 He should look like this:
 
 namespace ReleaseMusicServerlessApi.Helpers
  {
      public static class Credentials
      {
          public static readonly string authorizationSpotify = "";
          public static readonly string connectionString = "";
          public static readonly string dataBaseName = "";
          public static readonly string encryptionKey = "";
      }
  }
  
  This project has integrated with a Project tests for debug local.
