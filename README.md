## About This Project

This project is a simple implementation of JWT(JSON Web Token) authorisation/authentication. 

## About JWT

#### What is JWT?

It's a standard that is used to share security information between 2 parties, a client and server.
A JWT contains encoded JSON objects which can have a set of claims for example username and user role. JWTs are signed using a cryptographic algorithm to ensure security.

### How it works?

JWTs contain a set of claims which is used to transfer information between the client and server, claims are dependent on the use case at hand. Some commonly used claims are who issued the token, how long it is valid for and what permissions the client has been granted.
JWT is a string that is made up of three parts, these are seperated by dots and serialised using base 64.

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0Iiwicm9sZSI6IjEiLCJleHAiOjE2NTkwMDcyMjksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzgxIn0.khLqKDurxV4kHvEGw88s1cwGR01bJp-lKSeNJjf01j4
```

You can put this token into a jwt decoder to extract information such as https://jwt.io/

### Installation

_You can clone the repo or just look at the code._

1. Clone the repo
   ```sh
   git clone https://github.com/DDSJoshuaAllen/SimpleJWTAuth.git
   ```


### Client Side
> Once you have setup authentication in your project

Once you login succesfully on your client side (front end), you will want to store your token either in a cookie or local storage.

You will want to attach an `Authorization` header on each of your requests to the server. This header will look something like this:

```
"Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ0ZXN0Iiwicm9sZSI6IjEiLCJleHAiOjE2NTkwMDcyMjksImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzgxIn0.khLqKDurxV4kHvEGw88s1cwGR01bJp-lKSeNJjf01j4"
```

If a request becomes invalid you may want to log your user out.

### Extras

#### Access/Refresh Tokens
> This is a more secure method of JWT authentication
1. When you do log in, send 2 tokens (Access token, Refresh token) in response to the client.
2. The access token will have less expiry time and Refresh will have long expiry time.
3. The client (Front end) will store refresh token in his local storage and access token in cookies.
4. The client will use an access token for calling APIs. But when it expires, you call auth server API to get the new token (refresh token is automatically added to http request since it's stored in cookies).
5. Your auth server will have an API exposed which will accept refresh token and checks for its validity and return a new access token.
6. Once the refresh token is expired, the User will be logged out.



